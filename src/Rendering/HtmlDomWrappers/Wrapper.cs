using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    /// <summary>
    /// Represents a wrapper class.
    /// </summary>
    public abstract partial class Wrapper<T> : IWrapper where T : class
    {
        private readonly Func<T?> _getObject;
        private T? _wrappedObject;

        private Dictionary<int, IWrapper> Wrappers { get; } = new Dictionary<int, IWrapper>();

        /// <summary>
        /// Gets the wrapped object.
        /// </summary>
        [SuppressMessage("Design", "CA1065:Do not raise exceptions in unexpected locations")]
        public T WrappedObject
        {
            get
            {
                if (_wrappedObject is null)
                {
                    _wrappedObject = _getObject();
                    if (_wrappedObject is null) throw new NodeNoLongerAvailableException();
                }
                return _wrappedObject;
            }
        }

        /// <summary>
        /// Creates an instance of the <see cref="Wrapper{T}"/> class.
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        protected Wrapper(Func<T?> getObject)
        {
            if (getObject is null) throw new ArgumentNullException(nameof(getObject));
            _getObject = getObject;
            _wrappedObject = getObject();
        }

        /// <summary>
        /// Marks the wrapped object as stale, and forces the wrapper to retrieve it again.
        /// </summary>
        public void MarkAsStale()
        {
            _wrappedObject = null;
            foreach (var wrapped in Wrappers.Values) wrapped.MarkAsStale();
        }

        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <typeparam name="TWrapped">The type of object to wrap.</typeparam>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="wrapperFactory">A wrapper object factory.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected TWrapped GetOrWrap<TWrapped>(int key, WrapperFactory<TWrapped> wrapperFactory, Func<TWrapped?> objectQuery) where TWrapped : class
        {
            if (wrapperFactory is null) throw new ArgumentNullException(nameof(wrapperFactory));
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = wrapperFactory(objectQuery);
                Wrappers.Add(key, result);
            }
            return (TWrapped)result;
        }
    }
}
