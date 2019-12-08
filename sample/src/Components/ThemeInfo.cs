using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Library.SampleApp.Components
{
    public class ThemeInfo
    {
        private const string DEFAULT_BUTTON_CLASS = "btn btn-primary";
        private string _buttonClass = DEFAULT_BUTTON_CLASS;

        public string Class { get => _buttonClass; set => _buttonClass = value ?? DEFAULT_BUTTON_CLASS; }
    }
}
