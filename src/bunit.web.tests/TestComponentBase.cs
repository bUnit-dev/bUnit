using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Xunit;

namespace Bunit
{
    public class TestComponentBase<TTestComponent> : TestComponentBase
    {
        public static IEnumerable<FragmentBase[]> RazorTests()
		{
			var type = typeof(TTestComponent);
			yield break;
		}

		[Theory]
		[MemberData(nameof(RazorTests))]
		public void MyTheory(FragmentBase fragment)
		{
			Assert.NotNull(fragment);
		}
    }
}
