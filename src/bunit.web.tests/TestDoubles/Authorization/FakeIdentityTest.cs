using System;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeIdentityTest
	{
		[Fact(DisplayName = "Identity simple test")]
		public void Test001()
		{
			// arrange

			// act
			var identity = new FakeIdentity { Name = "TestUser", AuthenticationType = "AnotherTestType" };

			// assert
			Assert.Equal("TestUser", identity.Name);
			Assert.Equal("AnotherTestType", identity.AuthenticationType);
			Assert.True(identity.IsAuthenticated);
		}

		[Fact(DisplayName = "Identity with null/empty Name set")]
		public void Test002()
		{
			// arrange

			// act
			Assert.Throws<ArgumentNullException>(
				() => new FakeIdentity { Name = string.Empty, AuthenticationType = "AnotherTestType" } );

		}

		[Fact(DisplayName = "Identity with null/empty AuthenticationType set")]
		public void Test003()
		{
			// arrange

			// act
			Assert.Throws<ArgumentNullException>(
				() => new FakeIdentity { Name = "TestUser", AuthenticationType = string.Empty });

		}
	}
}
