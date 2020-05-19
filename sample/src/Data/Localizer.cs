using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace SampleApp.Data
{
    public class Localizer
    {
        private readonly Dictionary<string, string> _localizations
            = new Dictionary<string, string>();

        public string CultureCode { get; set; } = "en";

        public string this[string key]
        {
            get => _localizations.TryGetValue(key, out var localized)
                ? localized
                : key;
        }

        public void Add(string key, string text)
        {
            _localizations.Add(key, text);
        }

        public static Localizer Empty { get; } = new Localizer();
    }
}
