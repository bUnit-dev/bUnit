using System;

namespace Bunit.Extensions
{
	/// <summary>
	/// Helpful string extension methods for common operations.
	/// </summary>
	public static class StringExtensions
	{
		/// <summary>
		/// Verifies that the required string has an actual value.
		/// Throws an exception if the value is null or empty.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		public static string VerifyRequiredValue(this string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				throw new ArgumentNullException(nameof(value));
			}

			return value;
		}
	}
}
