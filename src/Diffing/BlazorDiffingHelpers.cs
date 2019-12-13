using System;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing.Diffing
{
    public static class BlazorDiffingHelpers
    {
        public static FilterDecision BlazorEventHandlerIdAttrFilter(in AttributeComparisonSource attrSource, FilterDecision currentDecision)
        {
            if (currentDecision == FilterDecision.Exclude) return currentDecision;

            if (Htmlizer.IsBlazorAttribute(attrSource.Attribute.Name))
                return FilterDecision.Exclude;

            return currentDecision;
        }
    }

}