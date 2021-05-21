#if NET5_0_OR_GREATER
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Shouldly;
using Xunit;

namespace Bunit.ComponentFactories
{
	public class StubComponentFactoryTest : TestContext
	{
		[Theory(DisplayName = "UseStubFor<TComponent>(renderParameters: false) does not render parameters passed to TComopnent")]
		[AutoData]
		public void Test005(string header)
		{
			ComponentFactories.UseStubFor<Simple1>(new() { AddParameters = false });

			var cut = RenderComponent<Wrapper>(ps => ps
				.AddChildContent<Simple1>(cps => cps
					.Add(p => p.Header, header)));

			cut.Find("Simple1").HasAttribute("header").ShouldBeFalse();
		}
	}
}

#endif
