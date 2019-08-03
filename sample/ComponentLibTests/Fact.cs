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
        internal const string RenderResultElement = "RenderResult";
        internal const string ExpectedHtmlElement = nameof(ExpectedOutput);
        internal const string RenderedHtmlElement = "RenderedHtml";

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public string? DisplayName { get; set; }

        [Parameter]
        public RenderFragment? Setup { get; set; }

        [Parameter]
        public RenderFragment? ExpectedOutput { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, RenderResultElement);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            if (!string.IsNullOrEmpty(DisplayName))
                builder.AddAttribute(2, nameof(DisplayName), DisplayName);

            builder.OpenElement(10, ExpectedHtmlElement);
            builder.AddContent(11, ExpectedOutput);
            builder.CloseElement();

            builder.OpenElement(20, RenderedHtmlElement);
            builder.AddContent(21, Setup);
            builder.CloseElement();

            builder.CloseElement();
        }
    }
}
