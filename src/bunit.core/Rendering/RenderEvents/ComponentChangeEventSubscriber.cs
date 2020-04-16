using System;

namespace Bunit.Rendering.RenderEvents
{
	/// <inheritdoc/>
	public sealed class ComponentChangeEventSubscriber : ConcurrentRenderEventSubscriber
	{
		private readonly int _targetComponentId;

		/// <summary>
		/// Creates an instance of the <see cref="ComponentChangeEventSubscriber"/>.
		/// </summary>
		public ComponentChangeEventSubscriber(IRenderedFragmentBase testTarget, Action<RenderEvent>? onChange = null, Action? onCompleted = null)
			: base((testTarget ?? throw new ArgumentNullException(nameof(testTarget))).RenderEvents, onChange, onCompleted)
		{
			_targetComponentId = testTarget.ComponentId;
		}

		/// <inheritdoc/>
		public override void OnNext(RenderEvent value)
		{
			if (value.HasChangesTo(_targetComponentId))
				base.OnNext(value);
		}
	}
}
