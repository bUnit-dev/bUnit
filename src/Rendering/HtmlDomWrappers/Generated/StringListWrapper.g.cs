using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IStringList"/> type.
    /// </summary>
    public partial class StringListWrapper : Wrapper<IStringList>, IStringList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="StringListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public StringListWrapper(Func<IStringList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public String this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public Boolean Contains(String entry)
            => WrappedObject.Contains(entry);

        /// <inheritdoc/>
        public IEnumerator<String> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
