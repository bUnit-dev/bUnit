using System;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing.Diffing
{
    public static class BlazorDiffingHelpers
    {
        public static FilterDecision BlazorEventHandlerIdAttrFilter(in AttributeComparisonSource attrSource, FilterDecision currentDecision)
        {
            if (currentDecision == FilterDecision.Exclude) return currentDecision;

            if (attrSource.Attribute.Name.StartsWith(Htmlizer.EVENT_HANDLE_ID_ATTR_PREFIX, StringComparison.Ordinal))
                return FilterDecision.Exclude;

            return currentDecision;
        }
    }

}