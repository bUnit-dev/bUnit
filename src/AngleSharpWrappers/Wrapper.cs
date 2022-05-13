using System;
using System.Diagnostics;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
	/// <summary>
	/// Represents a wrapper <see cref="IElement"/>.
	/// </summary>
	public abstract class Wrapper<TElement> : IWrapper<TElement> where TElement : class, INode
    {
        private readonly IElementFactory<TElement> elementFactory;

        /// <summary>
        /// Gets the wrapped element.
        /// </summary>
        [DebuggerHidden]
        public TElement WrappedElement => elementFactory.GetElement();

        /// <summary>
        /// Creates an instance of the <see cref="Wrapper{T}"/> class.
        /// </summary>
        protected Wrapper(IElementFactory<TElement> elementFactory)
        {
            if (elementFactory is null) throw new ArgumentNullException(nameof(elementFactory));

            this.elementFactory = elementFactory;
        }
/// <inheritdoc/>

        public override bool Equals(object obj) => WrappedElement.Equals(obj);
/// <inheritdoc/>

        public override int GetHashCode() => WrappedElement.GetHashCode();
/// <inheritdoc/>

        public static bool operator ==(Wrapper<TElement> x, TElement y)
        {
            if (x is null) return y is null;
            return x.WrappedElement == y;
        }
/// <inheritdoc/>

        public static bool operator !=(Wrapper<TElement> x, TElement y)
        {
            return !(x == y);
        }
/// <inheritdoc/>

        public static bool operator ==(TElement x, Wrapper<TElement> y)
        {
            if (y is null) return x is null;
            return x == y.WrappedElement;
        }
/// <inheritdoc/>

        public static bool operator !=(TElement x, Wrapper<TElement> y)
        {
            return !(x == y);
        }
    }
}
