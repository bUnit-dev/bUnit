using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Media.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="ITextTrackCueList"/> type.
    /// </summary>
    public partial class TextTrackCueListWrapper : Wrapper<ITextTrackCueList>, ITextTrackCueList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="TextTrackCueListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public TextTrackCueListWrapper(Func<ITextTrackCueList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public ITextTrackCue this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public IVideoTrack GetCueById(String id)
            => WrappedObject.GetCueById(id);

        /// <inheritdoc/>
        public IEnumerator<ITextTrackCue> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
