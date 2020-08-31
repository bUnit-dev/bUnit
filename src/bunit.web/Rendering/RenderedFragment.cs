using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

using AngleSharp.Diffing.Core;
using AngleSharp.Dom;

using Bunit.Diffing;
using Bunit.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Bunit.Rendering
{
	internal class RenderedFragment : IRenderedFragment
	{
		private readonly object _markupAccessLock = new object();
		[SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Instance is owned by the service provider and should not be disposed here.")]
		private readonly HtmlParser _htmlParser;
		private string _markup = string.Empty;
		private string? _snapshotMarkup;

		private INodeList? _firstRenderNodes;
		private INodeList? _latestRenderNodes;
		private INodeList? _snapshotNodes;

		/// <summary>
		/// Gets the first rendered markup.
		/// </summary>
		protected string FirstRenderMarkup { get; private set; } = string.Empty;

		public event Action? OnAfterRender;
		public event Action? OnMarkupUpdated;

		public bool IsDisposed { get; private set; }

		public int ComponentId { get; protected set; }

		public string Markup
		{
			get
			{
				EnsureComponentNotDisposed();
				lock (_markupAccessLock)
				{
					return Volatile.Read(ref _markup);
				}
			}
		}

		public int RenderCount { get; protected set; }

		public INodeList Nodes
		{
			get
			{
				EnsureComponentNotDisposed();

				// The lock ensures that latest nodes is always based on the latest rendered markup.
				lock (_markupAccessLock)
				{
					if (_latestRenderNodes is null)
						_latestRenderNodes = _htmlParser.Parse(Markup);

					return _latestRenderNodes;
				}
			}
		}

		public IServiceProvider Services { get; }

		public RenderedFragment(int componentId, IServiceProvider service)
		{
			ComponentId = componentId;
			Services = service;
			_htmlParser = Services.GetRequiredService<HtmlParser>();
		}

		public IReadOnlyList<IDiff> GetChangesSinceFirstRender()
		{
			if (_firstRenderNodes is null)
				_firstRenderNodes = _htmlParser.Parse(FirstRenderMarkup);

			return Nodes.CompareTo(_firstRenderNodes);
		}

		public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
		{
			if (_snapshotMarkup is null)
				throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(SaveSnapshot)}() to create one.");

			if (_snapshotNodes is null)
				_snapshotNodes = _htmlParser.Parse(_snapshotMarkup);

			return Nodes.CompareTo(_snapshotNodes);
		}

		public void SaveSnapshot()
		{
			_snapshotNodes = null;
			_snapshotMarkup = Markup;
		}

		void IRenderedFragmentBase.OnRender(RenderEvent renderEvent)
		{
			if (IsDisposed)
				return;

			var (rendered, changed, disposed) = renderEvent.GetRenderStatus(this);

			if (disposed)
			{
				((IDisposable)this).Dispose();
				return;
			}

			lock (_markupAccessLock)
			{
				if (rendered)
				{
					OnRender(renderEvent);
					RenderCount++;
				}

				if (changed)
				{
					UpdateMarkup(renderEvent.Frames);
				}
			}

			if (changed)
				OnMarkupUpdated?.Invoke();
			if (rendered)
				OnAfterRender?.Invoke();
		}

		protected void UpdateMarkup(RenderTreeFrameCollection framesCollection)
		{
			lock (_markupAccessLock)
			{
				_latestRenderNodes = null;
				var newMarkup = Htmlizer.GetHtml(ComponentId, framesCollection);
				Volatile.Write(ref _markup, newMarkup);
				if (RenderCount == 1)
					FirstRenderMarkup = newMarkup;
			}
		}

		protected virtual void OnRender(RenderEvent renderEvent) { }

		/// <summary>
		/// Ensures that the underlying component behind the
		/// fragment has not been removed from the render tree.
		/// </summary>
		protected void EnsureComponentNotDisposed()
		{
			if (IsDisposed)
				throw new ComponentDisposedException(ComponentId);
		}

		void IDisposable.Dispose()
		{
			IsDisposed = true;
			_markup = string.Empty;
			OnAfterRender = null;
			FirstRenderMarkup = string.Empty;
		}
	}
}
