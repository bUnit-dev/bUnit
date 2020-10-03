using System.Globalization;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

namespace Bunit.TestDoubles.Localization
{
	/// <summary>
	/// This IStringLocalizer is used to provide users with helpful exceptions if they fail to provide a mock when required.
	/// </summary>
	public class PlaceholderStringLocalization : IStringLocalizer
	{
		public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
		{
			throw new MissingMockStringLocalizationException(nameof(GetAllStrings), includeParentCultures);
		}

		public IStringLocalizer WithCulture(CultureInfo culture)
		{
			throw new MissingMockStringLocalizationException(nameof(WithCulture), culture);
		}

		public LocalizedString this[string name]
			=> throw new MissingMockStringLocalizationException("GetByIndex", name);

		public LocalizedString this[string name, params object[] arguments]
			=> throw new MissingMockStringLocalizationException("GetByIndex", name, arguments);
	}
}
