using System;
using System.Diagnostics;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Io;
using AngleSharp.Media.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IHtmlVideoElement"/> type.
    /// </summary>
    [DebuggerDisplay("{OuterHtml,nq}")]
    public sealed class HtmlVideoElementWrapper : Wrapper<IHtmlVideoElement>, IHtmlVideoElement
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlVideoElementWrapper"/> type;
        /// </summary>
        public HtmlVideoElementWrapper(IElementFactory<IHtmlVideoElement> elementFactory) : base(elementFactory) { }

        #region Events
        /// <inheritdoc/>
        public event DomEventHandler Aborted { add => WrappedElement.Aborted += value; remove => WrappedElement.Aborted -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Blurred { add => WrappedElement.Blurred += value; remove => WrappedElement.Blurred -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Cancelled { add => WrappedElement.Cancelled += value; remove => WrappedElement.Cancelled -= value; }
        /// <inheritdoc/>
        public event DomEventHandler CanPlay { add => ((IMediaController)WrappedElement).CanPlay += value; remove => ((IMediaController)WrappedElement).CanPlay -= value; }
        /// <inheritdoc/>
        public event DomEventHandler CanPlayThrough { add => ((IMediaController)WrappedElement).CanPlayThrough += value; remove => ((IMediaController)WrappedElement).CanPlayThrough -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Changed { add => WrappedElement.Changed += value; remove => WrappedElement.Changed -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Clicked { add => WrappedElement.Clicked += value; remove => WrappedElement.Clicked -= value; }
        /// <inheritdoc/>
        public event DomEventHandler CueChanged { add => WrappedElement.CueChanged += value; remove => WrappedElement.CueChanged -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DoubleClick { add => WrappedElement.DoubleClick += value; remove => WrappedElement.DoubleClick -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Drag { add => WrappedElement.Drag += value; remove => WrappedElement.Drag -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragEnd { add => WrappedElement.DragEnd += value; remove => WrappedElement.DragEnd -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragEnter { add => WrappedElement.DragEnter += value; remove => WrappedElement.DragEnter -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragExit { add => WrappedElement.DragExit += value; remove => WrappedElement.DragExit -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragLeave { add => WrappedElement.DragLeave += value; remove => WrappedElement.DragLeave -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragOver { add => WrappedElement.DragOver += value; remove => WrappedElement.DragOver -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DragStart { add => WrappedElement.DragStart += value; remove => WrappedElement.DragStart -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Dropped { add => WrappedElement.Dropped += value; remove => WrappedElement.Dropped -= value; }
        /// <inheritdoc/>
        public event DomEventHandler DurationChanged { add => ((IMediaController)WrappedElement).DurationChanged += value; remove => ((IMediaController)WrappedElement).DurationChanged -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Emptied { add => ((IMediaController)WrappedElement).Emptied += value; remove => ((IMediaController)WrappedElement).Emptied -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Ended { add => ((IMediaController)WrappedElement).Ended += value; remove => ((IMediaController)WrappedElement).Ended -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Error { add => WrappedElement.Error += value; remove => WrappedElement.Error -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Focused { add => WrappedElement.Focused += value; remove => WrappedElement.Focused -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Input { add => WrappedElement.Input += value; remove => WrappedElement.Input -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Invalid { add => WrappedElement.Invalid += value; remove => WrappedElement.Invalid -= value; }
        /// <inheritdoc/>
        public event DomEventHandler KeyDown { add => WrappedElement.KeyDown += value; remove => WrappedElement.KeyDown -= value; }
        /// <inheritdoc/>
        public event DomEventHandler KeyPress { add => WrappedElement.KeyPress += value; remove => WrappedElement.KeyPress -= value; }
        /// <inheritdoc/>
        public event DomEventHandler KeyUp { add => WrappedElement.KeyUp += value; remove => WrappedElement.KeyUp -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Loaded { add => WrappedElement.Loaded += value; remove => WrappedElement.Loaded -= value; }
        /// <inheritdoc/>
        public event DomEventHandler LoadedData { add => ((IMediaController)WrappedElement).LoadedData += value; remove => ((IMediaController)WrappedElement).LoadedData -= value; }
        /// <inheritdoc/>
        public event DomEventHandler LoadedMetadata { add => ((IMediaController)WrappedElement).LoadedMetadata += value; remove => ((IMediaController)WrappedElement).LoadedMetadata -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Loading { add => WrappedElement.Loading += value; remove => WrappedElement.Loading -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseDown { add => WrappedElement.MouseDown += value; remove => WrappedElement.MouseDown -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseEnter { add => WrappedElement.MouseEnter += value; remove => WrappedElement.MouseEnter -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseLeave { add => WrappedElement.MouseLeave += value; remove => WrappedElement.MouseLeave -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseMove { add => WrappedElement.MouseMove += value; remove => WrappedElement.MouseMove -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseOut { add => WrappedElement.MouseOut += value; remove => WrappedElement.MouseOut -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseOver { add => WrappedElement.MouseOver += value; remove => WrappedElement.MouseOver -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseUp { add => WrappedElement.MouseUp += value; remove => WrappedElement.MouseUp -= value; }
        /// <inheritdoc/>
        public event DomEventHandler MouseWheel { add => WrappedElement.MouseWheel += value; remove => WrappedElement.MouseWheel -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Paused { add => ((IMediaController)WrappedElement).Paused += value; remove => ((IMediaController)WrappedElement).Paused -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Played { add => ((IMediaController)WrappedElement).Played += value; remove => ((IMediaController)WrappedElement).Played -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Playing { add => ((IMediaController)WrappedElement).Playing += value; remove => ((IMediaController)WrappedElement).Playing -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Progress { add => WrappedElement.Progress += value; remove => WrappedElement.Progress -= value; }
        /// <inheritdoc/>
        public event DomEventHandler RateChanged { add => ((IMediaController)WrappedElement).RateChanged += value; remove => ((IMediaController)WrappedElement).RateChanged -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Resetted { add => WrappedElement.Resetted += value; remove => WrappedElement.Resetted -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Resized { add => WrappedElement.Resized += value; remove => WrappedElement.Resized -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Scrolled { add => WrappedElement.Scrolled += value; remove => WrappedElement.Scrolled -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Seeked { add => WrappedElement.Seeked += value; remove => WrappedElement.Seeked -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Seeking { add => WrappedElement.Seeking += value; remove => WrappedElement.Seeking -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Selected { add => WrappedElement.Selected += value; remove => WrappedElement.Selected -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Shown { add => WrappedElement.Shown += value; remove => WrappedElement.Shown -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Stalled { add => WrappedElement.Stalled += value; remove => WrappedElement.Stalled -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Submitted { add => WrappedElement.Submitted += value; remove => WrappedElement.Submitted -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Suspended { add => WrappedElement.Suspended += value; remove => WrappedElement.Suspended -= value; }
        /// <inheritdoc/>
        public event DomEventHandler TimeUpdated { add => ((IMediaController)WrappedElement).TimeUpdated += value; remove => ((IMediaController)WrappedElement).TimeUpdated -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Toggled { add => WrappedElement.Toggled += value; remove => WrappedElement.Toggled -= value; }
        /// <inheritdoc/>
        public event DomEventHandler VolumeChanged { add => ((IMediaController)WrappedElement).VolumeChanged += value; remove => ((IMediaController)WrappedElement).VolumeChanged -= value; }
        /// <inheritdoc/>
        public event DomEventHandler Waiting { add => ((IMediaController)WrappedElement).Waiting += value; remove => ((IMediaController)WrappedElement).Waiting -= value; }
        #endregion

        #region Properties and indexers
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? AccessKey { get => WrappedElement.AccessKey; set => WrappedElement.AccessKey = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? AccessKeyLabel { get => WrappedElement.AccessKeyLabel; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? AssignedSlot { get => WrappedElement.AssignedSlot; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INamedNodeMap Attributes { get => WrappedElement.Attributes; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IAudioTrackList? AudioTracks { get => WrappedElement.AudioTracks; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String BaseUri { get => WrappedElement.BaseUri; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Url? BaseUrl { get => WrappedElement.BaseUrl; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITimeRanges? BufferedTime { get => WrappedElement.BufferedTime; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 ChildElementCount { get => WrappedElement.ChildElementCount; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INodeList ChildNodes { get => WrappedElement.ChildNodes; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlCollection<IElement> Children { get => WrappedElement.Children; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITokenList ClassList { get => WrappedElement.ClassList; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? ClassName { get => WrappedElement.ClassName; set => WrappedElement.ClassName = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? ContentEditable { get => WrappedElement.ContentEditable; set => WrappedElement.ContentEditable = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlMenuElement? ContextMenu { get => WrappedElement.ContextMenu; set => WrappedElement.ContextMenu = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IMediaController? Controller { get => WrappedElement.Controller; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? CrossOrigin { get => WrappedElement.CrossOrigin; set => WrappedElement.CrossOrigin = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IDownload? CurrentDownload { get => WrappedElement.CurrentDownload; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? CurrentSource { get => WrappedElement.CurrentSource; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Double CurrentTime { get => WrappedElement.CurrentTime; set => WrappedElement.CurrentTime = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IStringMap Dataset { get => WrappedElement.Dataset; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Double DefaultPlaybackRate { get => WrappedElement.DefaultPlaybackRate; set => WrappedElement.DefaultPlaybackRate = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Direction { get => WrappedElement.Direction; set => WrappedElement.Direction = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 DisplayHeight { get => WrappedElement.DisplayHeight; set => WrappedElement.DisplayHeight = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 DisplayWidth { get => WrappedElement.DisplayWidth; set => WrappedElement.DisplayWidth = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public ISettableTokenList DropZone { get => WrappedElement.DropZone; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Double Duration { get => WrappedElement.Duration; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? FirstChild { get => WrappedElement.FirstChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? FirstElementChild { get => WrappedElement.FirstElementChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public NodeFlags Flags { get => WrappedElement.Flags; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? GivenNamespaceUri { get => WrappedElement.GivenNamespaceUri; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean HasChildNodes { get => WrappedElement.HasChildNodes; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Id { get => WrappedElement.Id; set => WrappedElement.Id = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String InnerHtml { get => WrappedElement.InnerHtml; set => WrappedElement.InnerHtml = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsAutoplay { get => WrappedElement.IsAutoplay; set => WrappedElement.IsAutoplay = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsContentEditable { get => WrappedElement.IsContentEditable; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsDefaultMuted { get => WrappedElement.IsDefaultMuted; set => WrappedElement.IsDefaultMuted = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsDraggable { get => WrappedElement.IsDraggable; set => WrappedElement.IsDraggable = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsEnded { get => WrappedElement.IsEnded; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsFocused { get => WrappedElement.IsFocused; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsHidden { get => WrappedElement.IsHidden; set => WrappedElement.IsHidden = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsLoop { get => WrappedElement.IsLoop; set => WrappedElement.IsLoop = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsMuted { get => WrappedElement.IsMuted; set => WrappedElement.IsMuted = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsPaused { get => WrappedElement.IsPaused; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsSeeking { get => WrappedElement.IsSeeking; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsShowingControls { get => WrappedElement.IsShowingControls; set => WrappedElement.IsShowingControls = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsSpellChecked { get => WrappedElement.IsSpellChecked; set => WrappedElement.IsSpellChecked = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsTranslated { get => WrappedElement.IsTranslated; set => WrappedElement.IsTranslated = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Language { get => WrappedElement.Language; set => WrappedElement.Language = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? LastChild { get => WrappedElement.LastChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? LastElementChild { get => WrappedElement.LastElementChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String LocalName { get => WrappedElement.LocalName; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IMediaError? MediaError { get => WrappedElement.MediaError; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? MediaGroup { get => WrappedElement.MediaGroup; set => WrappedElement.MediaGroup = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? NamespaceUri { get => WrappedElement.NamespaceUri; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public MediaNetworkState NetworkState { get => WrappedElement.NetworkState; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? NextElementSibling { get => WrappedElement.NextElementSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? NextSibling { get => WrappedElement.NextSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String NodeName { get => WrappedElement.NodeName; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public NodeType NodeType { get => WrappedElement.NodeType; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String NodeValue { get => WrappedElement.NodeValue; set => WrappedElement.NodeValue = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 OriginalHeight { get => WrappedElement.OriginalHeight; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 OriginalWidth { get => WrappedElement.OriginalWidth; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String OuterHtml { get => WrappedElement.OuterHtml; set => WrappedElement.OuterHtml = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IDocument? Owner { get => WrappedElement.Owner; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? Parent { get => WrappedElement.Parent; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? ParentElement { get => WrappedElement.ParentElement; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Double PlaybackRate { get => WrappedElement.PlaybackRate; set => WrappedElement.PlaybackRate = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public MediaControllerPlaybackState PlaybackState { get => WrappedElement.PlaybackState; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITimeRanges? PlayedTime { get => WrappedElement.PlayedTime; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Poster { get => WrappedElement.Poster; set => WrappedElement.Poster = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Prefix { get => WrappedElement.Prefix; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Preload { get => WrappedElement.Preload; set => WrappedElement.Preload = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? PreviousElementSibling { get => WrappedElement.PreviousElementSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? PreviousSibling { get => WrappedElement.PreviousSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public MediaReadyState ReadyState { get => WrappedElement.ReadyState; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITimeRanges? SeekableTime { get => WrappedElement.SeekableTime; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IShadowRoot? ShadowRoot { get => WrappedElement.ShadowRoot; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Slot { get => WrappedElement.Slot; set => WrappedElement.Slot = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Source { get => WrappedElement.Source; set => WrappedElement.Source = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public ISourceReference? SourceReference { get => WrappedElement.SourceReference; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public DateTime StartDate { get => WrappedElement.StartDate; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Int32 TabIndex { get => WrappedElement.TabIndex; set => WrappedElement.TabIndex = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public String TagName { get => WrappedElement.TagName; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String TextContent { get => WrappedElement.TextContent; set => WrappedElement.TextContent = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITextTrackList? TextTracks { get => WrappedElement.TextTracks; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Title { get => WrappedElement.Title; set => WrappedElement.Title = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public IVideoTrackList? VideoTracks { get => WrappedElement.VideoTracks; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Double Volume { get => WrappedElement.Volume; set => WrappedElement.Volume = value;}
        #endregion

        #region Methods
        /// <inheritdoc/>
        [DebuggerHidden]
        public void AddEventListener(String type, DomEventHandler? callback, Boolean capture) => WrappedElement.AddEventListener(type, callback, capture);
        /// <inheritdoc/>
        [DebuggerHidden]
        public ITextTrack AddTextTrack(String kind, String? label, String? language) => WrappedElement.AddTextTrack(kind, label, language);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void After(INode[] nodes) => WrappedElement.After(nodes);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Append(INode[] nodes) => WrappedElement.Append(nodes);
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode AppendChild(INode child) => WrappedElement.AppendChild(child);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IShadowRoot AttachShadow(ShadowRootMode mode) => WrappedElement.AttachShadow(mode);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Before(INode[] nodes) => WrappedElement.Before(nodes);
        /// <inheritdoc/>
        [DebuggerHidden]
        public String CanPlayType(String type) => WrappedElement.CanPlayType(type);
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode Clone(Boolean deep) => WrappedElement.Clone(deep);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? Closest(String selectors) => WrappedElement.Closest(selectors);
        /// <inheritdoc/>
        [DebuggerHidden]
        public DocumentPositions CompareDocumentPosition(INode otherNode) => WrappedElement.CompareDocumentPosition(otherNode);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean Contains(INode otherNode) => WrappedElement.Contains(otherNode);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean Dispatch(Event ev) => WrappedElement.Dispatch(ev);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void DoBlur() => WrappedElement.DoBlur();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void DoClick() => WrappedElement.DoClick();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void DoFocus() => WrappedElement.DoFocus();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void DoSpellCheck() => WrappedElement.DoSpellCheck();
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean Equals(INode otherNode) => WrappedElement.Equals(otherNode);
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? GetAttribute(String name) => WrappedElement.GetAttribute(name);
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? GetAttribute(String? namespaceUri, String localName) => WrappedElement.GetAttribute(namespaceUri, localName);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlCollection<IElement> GetElementsByClassName(String classNames) => WrappedElement.GetElementsByClassName(classNames);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlCollection<IElement> GetElementsByTagName(String tagName) => WrappedElement.GetElementsByTagName(tagName);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlCollection<IElement> GetElementsByTagNameNS(String? namespaceUri, String tagName) => WrappedElement.GetElementsByTagNameNS(namespaceUri, tagName);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean HasAttribute(String name) => WrappedElement.HasAttribute(name);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean HasAttribute(String? namespaceUri, String localName) => WrappedElement.HasAttribute(namespaceUri, localName);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Insert(AdjacentPosition position, String html) => WrappedElement.Insert(position, html);
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode InsertBefore(INode newElement, INode? referenceElement) => WrappedElement.InsertBefore(newElement, referenceElement);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void InvokeEventListener(Event ev) => WrappedElement.InvokeEventListener(ev);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean IsDefaultNamespace(String namespaceUri) => WrappedElement.IsDefaultNamespace(namespaceUri);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Load() => WrappedElement.Load();
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? LookupNamespaceUri(String prefix) => WrappedElement.LookupNamespaceUri(prefix);
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? LookupPrefix(String? namespaceUri) => WrappedElement.LookupPrefix(namespaceUri);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean Matches(String selectors) => WrappedElement.Matches(selectors);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Normalize() => WrappedElement.Normalize();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Pause() => WrappedElement.Pause();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Play() => WrappedElement.Play();
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Prepend(INode[] nodes) => WrappedElement.Prepend(nodes);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? QuerySelector(String selectors) => WrappedElement.QuerySelector(selectors);
        /// <inheritdoc/>
        [DebuggerHidden]
        public IHtmlCollection<IElement> QuerySelectorAll(String selectors) => WrappedElement.QuerySelectorAll(selectors);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Remove() => WrappedElement.Remove();
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean RemoveAttribute(String name) => WrappedElement.RemoveAttribute(name);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean RemoveAttribute(String? namespaceUri, String localName) => WrappedElement.RemoveAttribute(namespaceUri, localName);
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode RemoveChild(INode child) => WrappedElement.RemoveChild(child);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void RemoveEventListener(String type, DomEventHandler? callback, Boolean capture) => WrappedElement.RemoveEventListener(type, callback, capture);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Replace(INode[] nodes) => WrappedElement.Replace(nodes);
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode ReplaceChild(INode newChild, INode oldChild) => WrappedElement.ReplaceChild(newChild, oldChild);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void SetAttribute(String name, String? value) => WrappedElement.SetAttribute(name, value);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void SetAttribute(String? namespaceUri, String name, String? value) => WrappedElement.SetAttribute(namespaceUri, name, value);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => WrappedElement.ToHtml(writer, formatter);
        #endregion
    }
}
