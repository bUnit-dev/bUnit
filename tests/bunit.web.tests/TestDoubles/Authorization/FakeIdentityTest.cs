using System;
using Xunit;

namespace Bunit.TestDoubles.Authorization
{
	public class FakeIdentityTest
	{
		[Fact(DisplayName = "Identity simple test")]
		public void Test001()
		{
			// act
			var identity = new FakeIdentity { Name = "TestUser", AuthenticationType = "AnotherTestType" };

			// assert
			Assert.Equal("TestUser", identity.Name);
			Assert.Equal("AnotherTestType", identity.AuthenticationType);
			Assert.True(identity.IsAuthenticated);
		}

		[Theory(DisplayName = "Identity with null/empty Name set")]
		[InlineData(null)]
		[InlineData("")]
		public void Test002(string name)
		{
			Assert.Throws<ArgumentNullException>(() => new FakeIdentity { Name = name, AuthenticationType = "AnotherTestType" });
		}

		[Theory(DisplayName = "Identity with null/empty AuthenticationType set")]
		[InlineData(null)]
		[InlineData("")]
		public void Test003(string authType)
		{
			Assert.Throws<ArgumentNullException>(() => new FakeIdentity { Name = "TestUser", AuthenticationType = authType });
		}
	}
}
