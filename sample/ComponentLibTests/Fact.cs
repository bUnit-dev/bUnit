using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ComponentLib
{
    public class Fact : ComponentBase
    {
        private static readonly IServiceCollection EmptyServiceCollection = new ServiceCollection();

        [Parameter]
        public string? DisplayName { get; set; }

        [Parameter]
        public IServiceCollection? Services { get; set; }

        [Parameter]
        public RenderFragment? Setup { get; set; }

        [Parameter]
        public RenderFragment? ExpectedOutput { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (Setup is null) throw new ArgumentNullException(nameof(Setup));
            var actualOutput = string.Concat(RenderFragmentAsText(Setup, Services ?? EmptyServiceCollection).Tokens).Trim();

            builder.OpenElement(0, "TestResult");
            builder.AddAttribute(1, "DisplayName", DisplayName);

            builder.OpenElement(10, "ExpectedOutput");
            builder.AddContent(11, ExpectedOutput);
            builder.CloseElement();
            
            builder.OpenElement(20, "ActualOutput");
            builder.AddMarkupContent(21, actualOutput);
            builder.CloseElement();

            builder.CloseElement();
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
