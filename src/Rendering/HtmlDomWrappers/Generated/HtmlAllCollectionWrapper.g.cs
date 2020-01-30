using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IHtmlAllCollection"/> type.
    /// </summary>
    public partial class HtmlAllCollectionWrapper : Wrapper<IHtmlAllCollection>, IHtmlAllCollection, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlAllCollectionWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public HtmlAllCollectionWrapper(Func<IHtmlAllCollection?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IElement this[Int32 index] { get => GetOrWrap(HashCode.Combine("this+System.Int32", index), () => WrappedObject[index]); }

        /// <inheritdoc/>
        public IElement this[String id] { get => GetOrWrap(HashCode.Combine("this+System.String", id), () => WrappedObject[id]); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public IEnumerator<IElement> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
