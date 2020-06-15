using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakePrincipalTest
	{
		[Fact(DisplayName = "Principal is in role")]
		public void Test001()
		{
			// arrange
			var identity = new FakeIdentity { Name = "TestUser" };
			var roles = new List<string> { "User", "Admin", "Test" };
			var principal = new FakePrincipal { Identity = identity, Roles = roles };

			// act
			principal.IsInRole("User").ShouldBeTrue();
		}

		[Fact(DisplayName = "Principal is not in role")]
		public void Test002()
		{
			// arrange
			var identity = new FakeIdentity { Name = "TestUser" };
			var roles = new List<string> { "User", "Test" };
			var principal = new FakePrincipal { Identity = identity, Roles = roles };

			// act
			principal.IsInRole("Admin").ShouldBeFalse();
		}

		[Fact(DisplayName = "Principal has no roles")]
		public void Test003()
		{
			// arrange
			var identity = new FakeIdentity { Name = "TestUser" };
			var principal = new FakePrincipal { Identity = identity, Roles = null };

			// act
			principal.IsInRole("User").ShouldBeFalse();
		}
	}
}
