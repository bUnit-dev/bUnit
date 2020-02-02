using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Media.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="ITextTrackList"/> type.
    /// </summary>
    public partial class TextTrackListWrapper : Wrapper<ITextTrackList>, ITextTrackList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="TextTrackListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public TextTrackListWrapper(Func<ITextTrackList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public event DomEventHandler TrackAdded { add => WrappedObject.TrackAdded += value; remove => WrappedObject.TrackAdded -= value; }

        /// <inheritdoc/>
        public event DomEventHandler TrackRemoved { add => WrappedObject.TrackRemoved += value; remove => WrappedObject.TrackRemoved -= value; }

        /// <inheritdoc/>
        public ITextTrack this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public void AddEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.AddEventListener(type, callback, capture);

        /// <inheritdoc/>
        public Boolean Dispatch(Event ev)
            => WrappedObject.Dispatch(ev);

        /// <inheritdoc/>
        public void InvokeEventListener(Event ev)
            => WrappedObject.InvokeEventListener(ev);

        /// <inheritdoc/>
        public void RemoveEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.RemoveEventListener(type, callback, capture);

        /// <inheritdoc/>
        public IEnumerator<ITextTrack> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
