using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    internal abstract class WrapperBase<T> : INodeWrapper where T : class
    {
        private readonly Func<T?> _getObject;
        private readonly Dictionary<int, INodeWrapper> _wrappers = new Dictionary<int, INodeWrapper>();

        private T? _wrappedObject;

        public T WrappedObject
        {
            get
            {
                if (_wrappedObject is null)
                {
                    _wrappedObject = _getObject();
                    if (_wrappedObject is null)
                        throw new NodeNoLongerAvailableException();
                }
                return _wrappedObject;
            }
        }
        
        internal WrapperBase(Func<T?> getObject)
        {
            _getObject = getObject;
            _wrappedObject = getObject();
        }

        public void MarkAsStale()
        {
            _wrappedObject = null;
            foreach (var wrapped in _wrappers.Values) wrapped.MarkAsStale();
        }

        protected TElement GetOrWrap<TElement>(int key, Func<IElement?> nodeQuery)
            where TElement : IElement
        {
            if (!_wrappers.TryGetValue(key, out var result))
            {
                result = new ElementWrapper(nodeQuery);
                _wrappers.Add(key, result);
            }
            return (TElement)result;
        }
    }
}
