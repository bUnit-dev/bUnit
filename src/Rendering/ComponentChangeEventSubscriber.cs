using System;

namespace Bunit
{
    /// <inheritdoc/>
    public sealed class ComponentChangeEventSubscriber : RenderEventSubscriber, IDisposable
    {
        private readonly IRenderedFragment _testTarget;

        /// <summary>
        /// Creates an instance of the <see cref="ComponentChangeEventSubscriber"/>.
        /// </summary>
        public ComponentChangeEventSubscriber(IRenderedFragment testTarget, Action<RenderEvent>? onChange = null, Action? onCompleted = null)
            : base((testTarget ?? throw new ArgumentNullException(nameof(testTarget))).RenderEvents, onChange, onCompleted)
        {
            _testTarget = testTarget;
        }

        /// <inheritdoc/>
        public override void OnNext(RenderEvent value)
        {
            if (value.HasChangesTo(_testTarget))
                base.OnNext(value);
        }
        /// <inheritdoc/>
        public void Dispose() => Unsubscribe();
    }
}
