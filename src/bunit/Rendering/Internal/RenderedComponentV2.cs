using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bunit.Rendering.Internal;

internal class RenderedComponentV2<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	private readonly BunitComponentState state;

	public TComponent Instance => (TComponent)state.Component;

	public int RenderCount { get; }

	public bool IsDisposed { get; private set; }

	public int ComponentId { get; }

	public IServiceProvider Services { get; }

	public string Markup => state.Markup.ToString();

	public INodeList Nodes { get; }

	public event EventHandler OnAfterRender;
	public event EventHandler OnMarkupUpdated;

	public RenderedComponentV2(BunitComponentState state)
	{
		this.state = state;
	}

	public void Dispose()
	{
		IsDisposed = true;
	}

	public void OnRender(RenderEvent renderEvent) => throw new NotImplementedException();
}
