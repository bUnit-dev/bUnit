using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IHtmlFormControlsCollection"/> type.
    /// </summary>
    public partial class HtmlFormControlsCollectionWrapper : Wrapper<IHtmlFormControlsCollection>, IHtmlFormControlsCollection, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlFormControlsCollectionWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public HtmlFormControlsCollectionWrapper(Func<IHtmlFormControlsCollection?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IHtmlElement this[Int32 index] { get => GetOrWrap(HashCode.Combine("this+System.Int32", index), () => WrappedObject[index]); }

        /// <inheritdoc/>
        public IHtmlElement this[String id] { get => GetOrWrap(HashCode.Combine("this+System.String", id), () => WrappedObject[id]); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public IEnumerator<IHtmlElement> GetEnumerator()
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
