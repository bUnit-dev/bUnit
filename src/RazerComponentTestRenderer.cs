using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Egil.RazorComponents.Testing
{
    public class RazerComponentTestRenderer : IDisposable
    {
        private const string RenderResultsElement = "RenderResults";

        private readonly Func<string, string> _encoder = (t) => HtmlEncoder.Default.Encode(t);

        public bool HasRendered { get; private set; } = false;

        public IReadOnlyList<TestRenderResult> RenderResults { get; private set; } = Array.Empty<TestRenderResult>();

        public RazerComponentTestRenderer() { }

        public void Dispose() { }

        public void Render(RenderFragment renderFragment, IServiceCollection services)
        {
            using var serviceProvider = services.BuildServiceProvider();
            using var htmlRenderer = new HtmlRenderer(serviceProvider, NullLoggerFactory.Instance, _encoder);
            RenderResults = GetTestResults(htmlRenderer, renderFragment);

            HasRendered = true;
        }

        private IReadOnlyList<TestRenderResult> GetTestResults(HtmlRenderer htmlRenderer, RenderFragment renderFragment)
        {
            var parameters = ParameterView.FromDictionary(new Dictionary<string, object>() { { "ChildContent", renderFragment } });
            var renderTask = htmlRenderer.Dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<RenderFragmentWrapper>(parameters));
            var renderResult = GetResult(renderTask);
            return ProcessRenderResult(string.Concat(renderResult.Tokens));
        }

        private static ComponentRenderedText GetResult(Task<ComponentRenderedText> task)
        {
            if (!task.IsCompleted) throw new InvalidOperationException("This should not happen!");

            if (task.Status == TaskStatus.RanToCompletion)
            {
                return task.Result;
            }
            else
            {
                ExceptionDispatchInfo.Capture(task.Exception!).Throw();
                throw new InvalidOperationException("We will never hit this line");
            }
        }

        private static IReadOnlyList<TestRenderResult> ProcessRenderResult(string renderResults)
        {
            var xml = LoadRenderResult(renderResults);

            var result = new List<TestRenderResult>();
            foreach (XmlNode? node in xml.SelectNodes($"{RenderResultsElement}/{Fact.ElementName}"))
            {
                if (node is null) continue;

                result.Add(new TestRenderResult(
                    id: node.Attributes.GetNamedItem("Id")?.Value ?? string.Empty,
                    displayName: node.Attributes.GetNamedItem("DisplayName")?.Value ?? string.Empty,
                    actual: node.SelectSingleNode(TestSetup.ElementName),
                    expected: node.SelectSingleNode(ExpectedHtml.ElementName),
                    xmlSnippets: node.SelectNodes(HtmlSnippet.ElementName)
                    ));
            }

            return result.AsReadOnly();
        }

        private static XmlDocument LoadRenderResult(string renderResults)
        {
            var renderResultXml = $"<{RenderResultsElement}>{renderResults}</{RenderResultsElement}>";
            var xml = new XmlDocument();
            try
            {
                xml.LoadXml(renderResultXml);
                return xml;
            }
            catch (XmlException ex)
            {
                throw new RazorComponentRenderResultParseException($"An error occurred while trying to parse the render result of the test. {Environment.NewLine}" +
                    $"{ex.Message} Result XML:{Environment.NewLine}" +
                    $"{renderResultXml}", ex);
            }
        }
    }
}