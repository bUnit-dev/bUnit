using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing.Rendering
{
    [SuppressMessage("Performance", "CA1815:Override equals and operator equals on value types", Justification = "<Pending>")]
    public readonly struct ComponentRenderedText
    {
        public ComponentRenderedText(int componentId, IEnumerable<string> tokens)
        {
            ComponentId = componentId;
            Tokens = tokens;
        }

        public int ComponentId { get; }

        public IEnumerable<string> Tokens { get; }
    }
}
