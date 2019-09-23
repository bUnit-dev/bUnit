using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.AngleSharpDiffing
{
    public class DiffBuilder
    {
        private readonly INode _controlNode;

        private DiffBuilder(INode controlNode) {
            _controlNode = controlNode;
        }

        public static DiffBuilder Compare(INode controlNode)
        {
            if(controlNode is null) throw new ArgumentNullException(nameof(controlNode));
            return new DiffBuilder(controlNode);
        }

        public DiffBuilder WithTest(INode testNode)
        {
            if(testNode is null) throw new ArgumentNullException(nameof(testNode));
            return this;
        }

        public Diff Build() { return null; }
    }

    public class DifferenceEngine
    {

    }
}
