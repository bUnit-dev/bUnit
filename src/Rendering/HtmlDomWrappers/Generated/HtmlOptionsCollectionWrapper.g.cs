using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Html.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IHtmlOptionsCollection"/> type.
    /// </summary>
    public partial class HtmlOptionsCollectionWrapper : Wrapper<IHtmlOptionsCollection>, IHtmlOptionsCollection, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlOptionsCollectionWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public HtmlOptionsCollectionWrapper(Func<IHtmlOptionsCollection?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IHtmlOptionElement this[Int32 index] { get => GetOrWrap(HashCode.Combine("this+System.Int32", index), () => WrappedObject[index]); }

        /// <inheritdoc/>
        public IHtmlOptionElement this[String id] { get => GetOrWrap(HashCode.Combine("this+System.String", id), () => WrappedObject[id]); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public Int32 SelectedIndex { get => WrappedObject.SelectedIndex; set => WrappedObject.SelectedIndex = value;}

        /// <inheritdoc/>
        public void Add(IHtmlOptionElement element, IHtmlElement before)
        {
            WrappedObject.Add(element, before);
            MarkAsStale();
        }

        /// <inheritdoc/>
        public void Add(IHtmlOptionsGroupElement element, IHtmlElement before)
        {
            WrappedObject.Add(element, before);
            MarkAsStale();
        }

        /// <inheritdoc/>
        public IHtmlOptionElement GetOptionAt(Int32 index)
            => GetOrWrap(HashCode.Combine(nameof(GetOptionAt), index), () => WrappedObject.GetOptionAt(index));

        /// <inheritdoc/>
        public void Remove(Int32 index)
            => WrappedObject.Remove(index);

        /// <inheritdoc/>
        public void SetOptionAt(Int32 index, IHtmlOptionElement option)
        {
            WrappedObject.SetOptionAt(index, option);
            MarkAsStale();
        }

        /// <inheritdoc/>
        public IEnumerator<IHtmlOptionElement> GetEnumerator()
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
