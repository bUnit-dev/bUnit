using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace ComponentLib
{
    public class RazorComponentFixture : IDisposable
    {
        private readonly IDispatcher _dispatcher = Renderer.CreateDefaultDispatcher();
        private readonly Func<string, string> _encoder = (t) => HtmlEncoder.Default.Encode(t);
        private ServiceProvider? _serviceProvider;
        private HtmlRenderer? _htmlRenderer;

        public bool HasRendered { get; private set; } = false;

        public IReadOnlyList<TestRenderResult> RenderResults { get; private set; } = Array.Empty<TestRenderResult>();

        public RazorComponentFixture()
        {

        }

        public void Render(RenderFragment renderFragment, IServiceCollection services)
        {
            if (HasRendered) return;

            _serviceProvider = services.BuildServiceProvider();
            _htmlRenderer = new HtmlRenderer(_serviceProvider, _encoder, _dispatcher);
            var paramCollection = ParameterCollection.FromDictionary(
                new Dictionary<string, object>() { { "ChildContent", renderFragment } }
                );
            RenderResults = GetTestResults(paramCollection);

            HasRendered = true;
        }

        public void Dispose()
        {
            _serviceProvider?.Dispose();
            _htmlRenderer?.Dispose();
        }

        private IReadOnlyList<TestRenderResult> GetTestResults(ParameterCollection parameterCollection)
        {
            var renderResult = GetResult(
                _dispatcher.InvokeAsync(() => _htmlRenderer.RenderComponentAsync<RenderFragmentWrapper>(parameterCollection))
                );
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
            foreach (XmlNode node in xml.SelectNodes($"{renderResultsElement}/{Fact.RenderResultElement}"))
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