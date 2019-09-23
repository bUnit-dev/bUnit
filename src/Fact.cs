using System;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
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

        [SuppressMessage("Usage", "CA2208:Instantiate argument exceptions correctly", Justification = "<Pending>")]
        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (ChildContent is null) throw new ArgumentNullException(nameof(ChildContent));
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            builder.OpenElement(0, ElementName);

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            if (!string.IsNullOrEmpty(DisplayName))
                builder.AddAttribute(2, nameof(DisplayName), DisplayName);

            builder.AddContent(11, ChildContent);

            builder.CloseElement();
        }
    }

    public abstract class FactPart : ComponentBase
    {
        public const string WrapperElement = "Html";

        protected abstract string GetElementName();

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        [Parameter]
        public string? Id { get; set; }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (builder is null) throw new ArgumentNullException(nameof(builder));

            builder.OpenElement(0, GetElementName());

            if (!string.IsNullOrEmpty(Id))
                builder.AddAttribute(1, nameof(Id), Id);

            builder.OpenElement(10, WrapperElement);
            builder.AddContent(11, ChildContent);
            builder.CloseElement();

            builder.CloseElement();
        }
    }

    public class TestSetup : FactPart
    {
        public const string ElementName = "Rendered";
        protected override string GetElementName() => ElementName;
    }

    public class ExpectedHtml : FactPart
    {
        public const string ElementName = "Expected";
        protected override string GetElementName() => ElementName;
    }

    public class HtmlSnippet : FactPart
    {
        public const string ElementName = "Snippet";
        protected override string GetElementName() => ElementName;
    }
}
