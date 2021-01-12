using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.Extensions.Localization;

namespace Bunit.TestDoubles
{
	/// <summary>
	/// This IStringLocalizer is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	internal sealed class PlaceholderStringLocalization : IStringLocalizer
	{
		/// <summary>
		/// Will throw exception to prompt the user.
		/// </summary>
		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
			=> throw new MissingMockStringLocalizationException(nameof(GetAllStrings), includeParentCultures);

		/// <summary>
		/// Will throw exception to prompt the user.
		/// </summary>
		public IStringLocalizer WithCulture(CultureInfo culture)
			=> throw new MissingMockStringLocalizationException(nameof(WithCulture), culture);

		/// <summary>
		/// Will throw exception to prompt the user.
		/// </summary>
		public LocalizedString this[string name]
			=> Throw(name);

		/// <summary>
		/// Will throw exception to prompt the user.
		/// </summary>
		public LocalizedString this[string name, params object[] arguments]
			=> Throw(name, arguments);

		private static LocalizedString Throw(string name, params object?[]? args)
			=> throw new MissingMockStringLocalizationException("GetByIndex", name, args);
	}
}
