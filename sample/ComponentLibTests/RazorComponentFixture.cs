using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Xml;
using Egil.RazorComponents.Testing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Xunit;

namespace ComponentLib
{

    public abstract class RazorComponentFixture<T>
    {
        private static readonly IServiceCollection EmptyServiceCollection = new ServiceCollection();

        public static IEnumerable<object[]> GetTestComponentsResults
        {
            get
            {
                return Assembly.GetAssembly(typeof(T))?.GetTypes()
                    .Where(x => typeof(ComponentBase).IsAssignableFrom(x) && x.Name.EndsWith("test", StringComparison.OrdinalIgnoreCase))
                    .SelectMany((x, i) =>
                    {
                        RenderFragment fragment = builder =>
                        {
                            builder.OpenComponent(0, x);
                            builder.CloseComponent();
                        };

                        var testResult1 = RenderFragmentAsText(fragment, EmptyServiceCollection);
                        var testResult = $"<TestResults>{string.Concat(testResult1.Tokens)}</TestResults>";
                        var xmlDoc = new XmlDocument();
                        xmlDoc.LoadXml(testResult);
                        return xmlDoc.SelectNodes("TestResults/TestResult").Cast<XmlNode>()
                            .Select(node => new object[] { node.Attributes.GetNamedItem("DisplayName")?.Value ?? $"{x.Name}[{i + 1}]", node });
                    })
                    .ToArray() ?? Array.Empty<object[]>();
            }
        }

        [Theory]
#pragma warning disable xUnit1019 // MemberData must reference a member providing a valid data type
        [MemberData(nameof(GetTestComponentsResults))]
#pragma warning restore xUnit1019 // MemberData must reference a member providing a valid data type
        public virtual void TestComponent2(string name, XmlNode result)
        {
            var expectedOutputNode = result.SelectSingleNode("ExpectedOutput");
            var actualOutputNode = result.SelectSingleNode("ActualOutput");
            var diff = ShouldlyRazorComponentTestExtensions.CreateDiff(expectedOutputNode.InnerXml, actualOutputNode.InnerXml);
            diff.Differences.ShouldBeEmpty();
        }

        private static ComponentRenderedText RenderFragmentAsText(RenderFragment renderFragment, IServiceCollection services)
        {
            IDispatcher _dispatcher = Renderer.CreateDefaultDispatcher();
            Func<string, string> _encoder = (t) => HtmlEncoder.Default.Encode(t);

            var paramCollection = ParameterCollection.FromDictionary(new Dictionary<string, object>() { { "ChildContent", renderFragment } });

            using var htmlRenderer = new HtmlRenderer(services.BuildServiceProvider(), _encoder, _dispatcher);

            return GetResult(_dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<RenderFragmentWrapper>(paramCollection)));
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
    }
}
