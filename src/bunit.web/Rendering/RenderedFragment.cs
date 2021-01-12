using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using AngleSharp.Diffing.Core;
using AngleSharp.Dom;
using Microsoft.Extensions.DependencyInjection;

namespace Bunit.Rendering
{
	/// <inheritdoc />
	internal class RenderedFragment : IRenderedFragment
	{
		[SuppressMessage("Usage", "CA2213:Disposable fields should be disposed", Justification = "Instance controlled by IOC container.")]
		private readonly BunitHtmlParser htmlParser;
		private readonly object markupAccessLock = new();
		private string markup = string.Empty;
		private string? snapshotMarkup;

		private INodeList? firstRenderNodes;
		private INodeList? latestRenderNodes;
		private INodeList? snapshotNodes;

		/// <summary>
		/// Gets the first rendered markup.
		/// </summary>
		protected string FirstRenderMarkup { get; private set; } = string.Empty;

		/// <inheritdoc/>
		public event EventHandler? OnAfterRender;

		/// <inheritdoc/>
		public event EventHandler? OnMarkupUpdated;

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
				lock (markupAccessLock)
				{
					// Volatile read is necessary to ensure the updated markup
					// is available across CPU cores. Without it, the pointer to the
					// markup string can be stored in a CPUs register and not
					// get updated when another CPU changes the string.
					return Volatile.Read(ref markup);
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
				lock (markupAccessLock)
				{
					if (latestRenderNodes is null)
						latestRenderNodes = htmlParser.Parse(Markup);

					return latestRenderNodes;
				}
			}
		}

		/// <inheritdoc/>
		public IServiceProvider Services { get; }

		internal RenderedFragment(int componentId, IServiceProvider service)
		{
			ComponentId = componentId;
			Services = service;
			htmlParser = Services.GetRequiredService<BunitHtmlParser>();
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceFirstRender()
		{
			if (firstRenderNodes is null)
				firstRenderNodes = htmlParser.Parse(FirstRenderMarkup);

			return Nodes.CompareTo(firstRenderNodes);
		}

		/// <inheritdoc/>
		public IReadOnlyList<IDiff> GetChangesSinceSnapshot()
		{
			if (snapshotMarkup is null)
				throw new InvalidOperationException($"No snapshot exists to compare with. Call {nameof(SaveSnapshot)}() to create one.");

			if (snapshotNodes is null)
				snapshotNodes = htmlParser.Parse(snapshotMarkup);

			return Nodes.CompareTo(snapshotNodes);
		}

		/// <inheritdoc/>
		public void SaveSnapshot()
		{
			snapshotNodes = null;
			snapshotMarkup = Markup;
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
			lock (markupAccessLock)
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
				OnMarkupUpdated?.Invoke(this, EventArgs.Empty);

			if (rendered)
				OnAfterRender?.Invoke(this, EventArgs.Empty);
		}

		protected void UpdateMarkup(RenderTreeFrameDictionary framesCollection)
		{
			// The lock prevents a race condition between the renderers thread
			// and the test frameworks thread, where one might be reading the Markup
			// while the other is updating it due to async code in a rendered component.
			lock (markupAccessLock)
			{
				latestRenderNodes = null;
				var newMarkup = Htmlizer.GetHtml(ComponentId, framesCollection);

				// Volatile write is necessary to ensure the updated markup
				// is available across CPU cores. Without it, the pointer to the
				// markup string can be stored in a CPUs register and not
				// get updated when another CPU changes the string.
				Volatile.Write(ref markup, newMarkup);

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

		/// <inheritdoc/>
		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Disposes of the render fragment content.
		/// </summary>
		/// <remarks>
		/// The disposing parameter should be false when called from a finalizer, and true when called from the
		/// <see cref="Dispose()"/> method. In other words, it is true when deterministically called and false when non-deterministically called.
		/// </remarks>
		/// <param name="disposing">Set to true if called from <see cref="Dispose()"/>, false if called from a finalizer.f.</param>
		protected virtual void Dispose(bool disposing)
		{
			if (IsDisposed || !disposing)
				return;

			IsDisposed = true;
			markup = string.Empty;
			OnAfterRender = null;
			FirstRenderMarkup = string.Empty;
		}
	}
}
