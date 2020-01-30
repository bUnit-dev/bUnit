using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IStyleSheetList"/> type.
    /// </summary>
    public partial class StyleSheetListWrapper : Wrapper<IStyleSheetList>, IStyleSheetList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="StyleSheetListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public StyleSheetListWrapper(Func<IStyleSheetList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IStyleSheet this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public IEnumerator<IStyleSheet> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
