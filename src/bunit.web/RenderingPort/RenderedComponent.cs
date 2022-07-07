using System.Collections;
using AngleSharp;
using AngleSharp.Dom;

namespace Bunit.RenderingPort;

internal class RenderedComponent<TComponent> : IRenderedComponent<TComponent>
	where TComponent : IComponent
{
	internal int ComponentId { get; }

	public TComponent Instance { get; }

	public INodeList Nodes { get; }

	public string Markup => ((IMarkupFormattable)Nodes).ToMarkup();

	public RenderedComponent(IComponent component, INodeList nodes)
	{
		if (component is not TComponent instance)
		{
			throw new ArgumentException($"The component is not of type {typeof(TComponent)}", nameof(component));
		}

		Instance = instance;
		Nodes = new NoCommentsNodeList(nodes);
	}

	// HACK: filters out IComment nodes (Blazor component/markup markers) naively
	private sealed class NoCommentsNodeList : INodeList
	{
		private readonly INodeList original;

		public INode this[int index]
		{
			get
			{
				var position = 0;
				foreach (var node in this)
				{
					if (position == index)
						return node;

					position++;
				}

				throw new ArgumentOutOfRangeException(nameof(index));
			}
		}

		public int Length => original.Count(x => x is not IComment);

		public NoCommentsNodeList(INodeList original)
		{
			this.original = original;
		}

		public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
		{
			foreach (var node in this)
			{
				node.ToHtml(writer, formatter);
			}
		}

		public IEnumerator<INode> GetEnumerator() => original.Where(x => x is not IComment).GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
