using System;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class TestSetup<TComponentUnderTest>
    {
        public TComponentUnderTest Component { get; set; }
        public IHtmlElement Html { get; set; }
        public IHtmlElement this[int index]
        {
            get => null;// {  /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }
        public IHtmlElement this[string id]
        {
            get => null;// { /* return the specified index here */ }
            set { /* set the specified index to value here */ }
        }

        public void Deconstruct(out TComponentUnderTest component)
        {
            component = Component;
        }

        public void Deconstruct(out TComponentUnderTest component, out IHtmlElement html)
        {
            component = Component;
            html = Html;
        }
    }


    public class Test<TComponent> : ComponentBase
    {
        [Parameter] public string DisplayName { get; set; }

        [Parameter] public string Given { get; set; }

        [Parameter] public Action<TestSetup<TComponent>>? Method { get; set; }

        [Parameter] public Action<TComponent, IHtmlElement>? When { get; set; }

        [Parameter] public Action<TComponent, IHtmlElement>? Then { get; set; }

        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class Setup : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class Arrange : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class Given : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class Expect : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class Snippet : ComponentBase
    {
        [Parameter] public string Id { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }
    }

    public class CompareTest : ComponentBase
    {
        [Parameter] public string DisplayName { get; set; }
        [Parameter] public RenderFragment Given { get; set; }
        [Parameter] public RenderFragment Expect { get; set; }
        [Parameter] public bool Strict { get; set; } = false;
    }
}
