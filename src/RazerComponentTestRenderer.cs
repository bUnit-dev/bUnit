using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace Egil.RazorComponents.Testing
{
    public class RazerComponentTestRenderer : IDisposable
    {
        private readonly IDispatcher _dispatcher = Renderer.CreateDefaultDispatcher();
        private readonly Func<string, string> _encoder = (t) => HtmlEncoder.Default.Encode(t);

        public bool HasRendered { get; private set; } = false;

        public IReadOnlyList<TestRenderResult> RenderResults { get; private set; } = Array.Empty<TestRenderResult>();

        public RazerComponentTestRenderer() { }

        public void Dispose() { }

        public void Render(RenderFragment renderFragment, IServiceCollection services)
        {
            if (HasRendered) return;

            var paramCollection = ParameterCollection.FromDictionary(new Dictionary<string, object>() { { "ChildContent", renderFragment } });

            using var serviceProvider = services.BuildServiceProvider();
            using var htmlRenderer = new HtmlRenderer(serviceProvider, _encoder, _dispatcher);
            RenderResults = GetTestResults(htmlRenderer, paramCollection);

            HasRendered = true;
        }

        private IReadOnlyList<TestRenderResult> GetTestResults(HtmlRenderer htmlRenderer, ParameterCollection parameterCollection)
        {
            var renderTask = _dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<RenderFragmentWrapper>(parameterCollection));
            var renderResult = GetResult(renderTask);
            return ParseRawXml(string.Concat(renderResult.Tokens));
        }

        private static ComponentRenderedText GetResult(Task<ComponentRenderedText> task)
        {
            if (!task.IsCompleted) throw new InvalidOperationException("This should not happen!");

            if (task.IsCompletedSuccessfully)
            {
                return task.Result;
            }
            else
            {
                ExceptionDispatchInfo.Capture(task.Exception!).Throw();
                throw new InvalidOperationException("We will never hit this line");
            }
        }

        private static IReadOnlyList<TestRenderResult> ParseRawXml(string renderResults)
        {
            const string renderResultsElement = "RenderResults";

            var xml = new XmlDocument();
            xml.LoadXml($"<{renderResultsElement}>{renderResults}</{renderResultsElement}>");

            var result = new List<TestRenderResult>();
            foreach (XmlNode? node in xml.SelectNodes($"{renderResultsElement}/{Fact.RenderResultElement}"))
            {
                if (node is null) continue;

                result.Add(new TestRenderResult(
                    id: node.Attributes.GetNamedItem("Id")?.Value ?? string.Empty,
                    displayName: node.Attributes.GetNamedItem("DisplayName")?.Value ?? string.Empty,
                    actual: node.SelectSingleNode(Fact.RenderedHtmlElement),
                    expected: node.SelectSingleNode(Fact.ExpectedHtmlElement)
                    ));
            }

            return result.AsReadOnly();
        }
    }
}