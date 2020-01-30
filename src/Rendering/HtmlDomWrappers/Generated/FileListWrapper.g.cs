using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Io.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IFileList"/> type.
    /// </summary>
    public partial class FileListWrapper : Wrapper<IFileList>, IFileList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="FileListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public FileListWrapper(Func<IFileList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IFile this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public void Add(IFile file)
            => WrappedObject.Add(file);

        /// <inheritdoc/>
        public void Clear()
            => WrappedObject.Clear();

        /// <inheritdoc/>
        public Boolean Remove(IFile file)
            => WrappedObject.Remove(file);

        /// <inheritdoc/>
        public IEnumerator<IFile> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
