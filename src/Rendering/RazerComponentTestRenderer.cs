using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

namespace Egil.RazorComponents.Testing.Rendering
{
    public sealed class RazerComponentTestRenderer : IDisposable
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

        private static IReadOnlyList<TestRenderResult> GetTestResults(HtmlRenderer htmlRenderer, RenderFragment renderFragment)
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
            // Workaround for https://github.com/egil/razor-components-testing-library/issues/13 and https://github.com/aspnet/AspNetCore/issues/13793
            renderResults = EscapeSelfClosingTags(renderResults);
            var renderResultXml = $"<{RenderResultsElement}>{renderResults}</{RenderResultsElement}>";
            var result = new XmlDocument();
            result.LoadRenderResultXml(renderResultXml);
            return result;
        }

        private static readonly Regex SelfClosingTagsFinder = new Regex(@"<(area|base|br|col|embed|hr|img|input|link|meta|param|source|track|wbr)>", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string EscapeSelfClosingTags(string html)
        {
            return SelfClosingTagsFinder.Replace(html, "<$1/>");
        }
    }
}