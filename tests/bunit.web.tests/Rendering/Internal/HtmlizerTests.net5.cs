#if NET5_0
using System;
using Bunit.TestAssets.BlazorE2E;
using Bunit.TestAssets.SampleComponents;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;
using Shouldly;
using Xunit;

namespace Bunit.Rendering.Internal
{
	public partial class HtmlizerTests : TestContext
	{
		[Theory(DisplayName = "IsBlazorAttribute correctly identifies Blazor attributes")]
		[InlineData("b-twl12ishk1=\"\"")]
		[InlineData("blazor:onclick=\"1\"")]
		[InlineData("blazor:__internal_stopPropagation_onclick=\"\"")]
		[InlineData("blazor:__internal_preventDefault_onclick=\"\"")]
		public void TestNET5_001(string blazorAttribute)
		{
			Htmlizer.IsBlazorAttribute(blazorAttribute).ShouldBeTrue();
		}
	}
}
#endif
