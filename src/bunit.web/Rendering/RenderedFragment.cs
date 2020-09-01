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
	/// <inheritdoc />
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

		/// <inheritdoc/>
		public event Action? OnAfterRender;

		/// <inheritdoc/>
		public event Action? OnMarkupUpdated;

		/// <inheritdoc/>
		public bool IsDisposed { get; private set; }

		/// <inheritdoc/>
		public int ComponentId { get; protected set; }

		/// <inheritdoc/>
		public string Markup
		{
			get
			{
				EnsureComponentNotDisposed();

				// The lock prevents a race condition between the renderers thread
				// and the test frameworks thread, where one might be reading the Markup
				// while the other is updating it due to async code in a rendered component.
				lock (_markupAccessLock)
				{
					// Volatile read is necessary to ensure the updated markup
					// is available across CPU cores. Without it, the pointer to the
					// markup string can be stored in a CPUs register and not
					// get updated when another CPU changes the string.
					return Volatile.Read(ref _markup);
				}
			}
		}

		/// <inheritdoc/>
		public int RenderCount { get; protected set; }

		/// <inheritdoc/>
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

		/// <inheritdoc/>
		public IServiceProvider Services { get; }

		internal RenderedFragment(int componentId, IServiceProvider service)
		{
			ComponentId = componentId;
			Services = service;
			_htmlParser = Services.GetRequiredService<HtmlParser>();
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceFirstRender()
		{
			if (_firstRenderNodes is null)
				_firstRenderNodes = _htmlParser.Parse(FirstRenderMarkup);

			return Nodes.CompareTo(_firstRenderNodes);
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
		{
			if (_snapshotMarkup is null)
				throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(SaveSnapshot)}() to create one.");

			if (_snapshotNodes is null)
				_snapshotNodes = _htmlParser.Parse(_snapshotMarkup);

			return Nodes.CompareTo(_snapshotNodes);
		}

		/// <inheritdoc/>
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

			// The lock prevents a race condition between the renderers thread
			// and the test frameworks thread, where one might be reading the Markup
			// while the other is updating it due to async code in a rendered component.
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

			// The order here is important, since consumers of the events
			// expect that markup has indeed changed when OnAfterRender is invoked
			// (assuming there are markup changes)
			if (changed)
				OnMarkupUpdated?.Invoke();
			if (rendered)
				OnAfterRender?.Invoke();
		}

		protected void UpdateMarkup(RenderTreeFrameCollection framesCollection)
		{
			// The lock prevents a race condition between the renderers thread
			// and the test frameworks thread, where one might be reading the Markup
			// while the other is updating it due to async code in a rendered component.
			lock (_markupAccessLock)
			{
				_latestRenderNodes = null;
				var newMarkup = Htmlizer.GetHtml(ComponentId, framesCollection);

				// Volatile write is necessary to ensure the updated markup
				// is available across CPU cores. Without it, the pointer to the
				// markup string can be stored in a CPUs register and not
				// get updated when another CPU changes the string.
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
