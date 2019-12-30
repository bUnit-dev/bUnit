﻿using System;
using Egil.RazorComponents.Testing.Asserting;
using Egil.RazorComponents.Testing.EventDispatchExtensions;
using Microsoft.AspNetCore.Components;
using Xunit;
using Shouldly;
using System.Threading.Tasks;
using Egil.RazorComponents.Testing.Extensions;
using Egil.RazorComponents.Testing.SampleComponents;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    [SuppressMessage("Reliability", "CA2007:Consider calling ConfigureAwait on the awaited task", Justification = "<Pending>")]
    public class AllTypesOfParamsTest : ComponentTestFixture
    {
        [Fact(DisplayName = "All types of parameters are correctly assigned to component on render")]
        public void Test001()
        {
            Services.AddMockJsRuntime();

            var cut = RenderComponent<AllTypesOfParams<string>>(
                ("some-unmatched-attribute", "unmatched value"),
                (nameof(AllTypesOfParams<string>.RegularParam), "some value"),
                CascadingValue(42),
                CascadingValue(nameof(AllTypesOfParams<string>.NamedCascadingValue), 1337),
                EventCallback(nameof(AllTypesOfParams<string>.NonGenericCallback), () => throw new Exception("NonGenericCallback")),
                EventCallback(nameof(AllTypesOfParams<string>.GenericCallback), (EventArgs args) => throw new Exception("GenericCallback")),
                ChildContent(nameof(ChildContent)),
                RenderFragment(nameof(AllTypesOfParams<string>.OtherContent), nameof(AllTypesOfParams<string>.OtherContent)),
                Template<string>(nameof(AllTypesOfParams<string>.ItemTemplate), (item) => (builder) => throw new Exception("ItemTemplate"))
            );

            // assert that all parameters have been set correctly
            var instance = cut.Instance;
            instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
            instance.RegularParam.ShouldBe("some value");
            instance.UnnamedCascadingValue.ShouldBe(42);
            instance.NamedCascadingValue.ShouldBe(1337);
            Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(null)).Message.ShouldBe("NonGenericCallback");
            Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");
            new RenderedFragment(this, instance.ChildContent!).GetMarkup().ShouldBe(nameof(ChildContent));
            new RenderedFragment(this, instance.OtherContent!).GetMarkup().ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
            Should.Throw<Exception>(() => instance.ItemTemplate!("")(null)).Message.ShouldBe("ItemTemplate");
        }

        [Fact(DisplayName = "All types of parameters are correctly assigned to component on re-render")]
        public void Test002()
        {
            // arrange
            Services.AddMockJsRuntime();
            var cut = RenderComponent<AllTypesOfParams<string>>();

            // assert that no parameters have been set initially
            var instance = cut.Instance;
            instance.Attributes.ShouldBeNull();
            instance.RegularParam.ShouldBeNull();
            instance.UnnamedCascadingValue.ShouldBeNull();
            instance.NamedCascadingValue.ShouldBeNull();
            instance.NonGenericCallback.HasDelegate.ShouldBeFalse();
            instance.GenericCallback.HasDelegate.ShouldBeFalse();
            instance.ChildContent.ShouldBeNull();
            instance.OtherContent.ShouldBeNull();
            instance.ItemTemplate.ShouldBeNull();

            // act - set components params and render
            cut.SetParametersAndRender(
                ("some-unmatched-attribute", "unmatched value"),
                (nameof(AllTypesOfParams<string>.RegularParam), "some value"),
                EventCallback(nameof(AllTypesOfParams<string>.NonGenericCallback), () => throw new Exception("NonGenericCallback")),
                EventCallback(nameof(AllTypesOfParams<string>.GenericCallback), (EventArgs args) => throw new Exception("GenericCallback")),
                ChildContent<Wrapper>(ChildContent(nameof(ChildContent))),
                RenderFragment<Wrapper>(nameof(AllTypesOfParams<string>.OtherContent), ChildContent(nameof(AllTypesOfParams<string>.OtherContent))),
                Template<string>(nameof(AllTypesOfParams<string>.ItemTemplate), (item) => (builder) => throw new Exception("ItemTemplate"))
            );

            instance.Attributes["some-unmatched-attribute"].ShouldBe("unmatched value");
            instance.RegularParam.ShouldBe("some value");
            Should.Throw<Exception>(async () => await instance.NonGenericCallback.InvokeAsync(null)).Message.ShouldBe("NonGenericCallback");
            Should.Throw<Exception>(async () => await instance.GenericCallback.InvokeAsync(EventArgs.Empty)).Message.ShouldBe("GenericCallback");
            new RenderedFragment(this, instance.ChildContent!).GetMarkup().ShouldBe(nameof(ChildContent));
            new RenderedFragment(this, instance.OtherContent!).GetMarkup().ShouldBe(nameof(AllTypesOfParams<string>.OtherContent));
            Should.Throw<Exception>(() => instance.ItemTemplate!("")(null)).Message.ShouldBe("ItemTemplate");
        }

        [Fact(DisplayName = "Trying to set CascadingValue during SetParametersAndRender throws")]
        public void Test003()
        {
            // arrange
            Services.AddMockJsRuntime();
            var cut = RenderComponent<AllTypesOfParams<string>>();

            // assert
            Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRender(CascadingValue(42)));
            Should.Throw<InvalidOperationException>(() => cut.SetParametersAndRender(CascadingValue(nameof(AllTypesOfParams<string>.NamedCascadingValue), 1337)));
        }
    }
}
