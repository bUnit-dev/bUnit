using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Egil.RazorComponents.Testing
{
    public class Fact : ComponentBase
    {
        internal const string ElementName = "RenderResult";

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public string? DisplayName { get; set; }

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if(ChildContent is null) throw new ArgumentNullException(nameof(ChildContent));

            builder.OpenElement(0, ElementName);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            if (!string.IsNullOrEmpty(DisplayName))
                builder.AddAttribute(2, nameof(DisplayName), DisplayName);

            builder.AddContent(11, ChildContent);

            builder.CloseElement();
        }
    }

    public class TestSetup : ComponentBase
    {
        internal const string ElementName = "RenderedHtml";

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Id { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, ElementName);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            builder.AddContent(2, ChildContent);

            builder.CloseElement();
        }
    }

    public class ExpectedHtml : ComponentBase
    {
        internal const string ElementName = "ExpectedHtml";

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Id { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, ElementName);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            builder.AddContent(2, ChildContent);

            builder.CloseElement();
        }
    }

    public class HtmlSnippet : ComponentBase
    {
        internal const string ElementName = "Html";

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Id { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            builder.OpenElement(0, ElementName);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            builder.AddContent(2, ChildContent);

            builder.CloseElement();
        }
    }
}
