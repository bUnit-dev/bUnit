using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit.SampleComponents
{
	public class OneFixtureComponent : TestComponentBase2
	{
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<Fixture>(0);
			builder.AddAttribute(1, nameof(Fixture.Description), "Fixture name");
			builder.CloseComponent();
		}
	}
}
