using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AngleSharp;
using AngleSharp.Css.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IMediaList"/> type.
    /// </summary>
    public partial class MediaListWrapper : Wrapper<IMediaList>, IMediaList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="MediaListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public MediaListWrapper(Func<IMediaList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public String this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public String MediaText { get => WrappedObject.MediaText; set => WrappedObject.MediaText = value;}

        /// <inheritdoc/>
        public void Add(String medium)
            => WrappedObject.Add(medium);

        /// <inheritdoc/>
        public void Remove(String medium)
            => WrappedObject.Remove(medium);

        /// <inheritdoc/>
        public void ToCss(TextWriter writer, IStyleFormatter formatter)
            => WrappedObject.ToCss(writer, formatter);

        /// <inheritdoc/>
        public IEnumerator<ICssMedium> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
