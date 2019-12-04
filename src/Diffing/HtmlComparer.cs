using System.Collections.Generic;
using AngleSharp.Diffing.Strategies;
using AngleSharp.Diffing;
using AngleSharp.Dom;
using AngleSharp.Diffing.Core;
using Xunit.Abstractions;
using Egil.RazorComponents.Testing.Diffing;

namespace Egil.RazorComponents.Testing.Diffing
{
    public sealed class HtmlComparer
    {
        private readonly HtmlDifferenceEngine _differenceEngine;

        public HtmlComparer()
        {
            var strategy = new DiffingStrategyPipeline();
            strategy.AddDefaultOptions();
            strategy.AddFilter(BlazorDiffingHelpers.BlazorEventHandlerIdAttrFilter, isSpecializedFilter: true);
            _differenceEngine = new HtmlDifferenceEngine(strategy);
        }

        public IEnumerable<IDiff> Compare(INode controlHtml, INode testHtml)
        {
            return _differenceEngine.Compare(controlHtml, testHtml);
        }

        public IEnumerable<IDiff> Compare(IEnumerable<INode> controlHtml, IEnumerable<INode> testHtml)
        {
            return _differenceEngine.Compare(controlHtml, testHtml);
        }
    }

}