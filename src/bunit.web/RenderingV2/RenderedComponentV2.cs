using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Bunit.RenderingV2;

public class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	public TComponent Instance { get; }
	public INodeList Nodes { get; }

	public RenderedComponentV2(TComponent instance)
	{
		Instance = instance;
		Nodes = default!;
	}
}
