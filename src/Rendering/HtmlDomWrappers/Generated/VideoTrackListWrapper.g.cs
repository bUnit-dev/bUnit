using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Media.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IVideoTrackList"/> type.
    /// </summary>
    public partial class VideoTrackListWrapper : Wrapper<IVideoTrackList>, IVideoTrackList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="VideoTrackListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public VideoTrackListWrapper(Func<IVideoTrackList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public event DomEventHandler Changed { add => WrappedObject.Changed += value; remove => WrappedObject.Changed -= value; }

        /// <inheritdoc/>
        public event DomEventHandler TrackAdded { add => WrappedObject.TrackAdded += value; remove => WrappedObject.TrackAdded -= value; }

        /// <inheritdoc/>
        public event DomEventHandler TrackRemoved { add => WrappedObject.TrackRemoved += value; remove => WrappedObject.TrackRemoved -= value; }

        /// <inheritdoc/>
        public IVideoTrack this[Int32 index] { get => WrappedObject[index]; }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public Int32 SelectedIndex { get => WrappedObject.SelectedIndex; }

        /// <inheritdoc/>
        public void AddEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.AddEventListener(type, callback, capture);

        /// <inheritdoc/>
        public Boolean Dispatch(Event ev)
            => WrappedObject.Dispatch(ev);

        /// <inheritdoc/>
        public IVideoTrack GetTrackById(String id)
            => WrappedObject.GetTrackById(id);

        /// <inheritdoc/>
        public void InvokeEventListener(Event ev)
            => WrappedObject.InvokeEventListener(ev);

        /// <inheritdoc/>
        public void RemoveEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.RemoveEventListener(type, callback, capture);

        /// <inheritdoc/>
        public IEnumerator<IVideoTrack> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
