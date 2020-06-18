namespace Bunit.TestDoubles.Authorization
{
	/// <summary>
	/// Helper methods for registering the Authentication/Authorization services with
	/// a <see cref="TestServiceProvider"/>.
	/// </summary>
	public static class FakeAuthorizationExtensions
	{
		/// <summary>
		/// Adds the appropriate auth services to the <see cref="TestServiceProvider"/> to enable
		/// an authenticated user.
		/// </summary>
		public static TestAuthorizationContext AddTestAuthorization(this TestServiceProvider serviceProvider)
		{
			var adaptor = new TestAuthorizationContext();
			adaptor.Unauthenticate();
			adaptor.RegisterAuthorizationServices(serviceProvider);

			return adaptor;
		}
	}
}
