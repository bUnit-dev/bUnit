using System.Collections.Generic;

namespace Egil.RazorComponents.Testing.Rendering
{
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
