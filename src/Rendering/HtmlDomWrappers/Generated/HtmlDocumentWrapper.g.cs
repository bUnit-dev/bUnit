using System;
using System.IO;
using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Text;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IHtmlDocument"/> type.
    /// </summary>
    public partial class HtmlDocumentWrapper : Wrapper<IHtmlDocument>, IHtmlDocument, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="HtmlDocumentWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public HtmlDocumentWrapper(Func<IHtmlDocument?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public event DomEventHandler Aborted { add => ((IGlobalEventHandlers)WrappedObject).Aborted += value; remove => ((IGlobalEventHandlers)WrappedObject).Aborted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Blurred { add => ((IGlobalEventHandlers)WrappedObject).Blurred += value; remove => ((IGlobalEventHandlers)WrappedObject).Blurred -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Cancelled { add => ((IGlobalEventHandlers)WrappedObject).Cancelled += value; remove => ((IGlobalEventHandlers)WrappedObject).Cancelled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CanPlay { add => ((IGlobalEventHandlers)WrappedObject).CanPlay += value; remove => ((IGlobalEventHandlers)WrappedObject).CanPlay -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CanPlayThrough { add => ((IGlobalEventHandlers)WrappedObject).CanPlayThrough += value; remove => ((IGlobalEventHandlers)WrappedObject).CanPlayThrough -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Changed { add => ((IGlobalEventHandlers)WrappedObject).Changed += value; remove => ((IGlobalEventHandlers)WrappedObject).Changed -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Clicked { add => ((IGlobalEventHandlers)WrappedObject).Clicked += value; remove => ((IGlobalEventHandlers)WrappedObject).Clicked -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CueChanged { add => ((IGlobalEventHandlers)WrappedObject).CueChanged += value; remove => ((IGlobalEventHandlers)WrappedObject).CueChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DoubleClick { add => ((IGlobalEventHandlers)WrappedObject).DoubleClick += value; remove => ((IGlobalEventHandlers)WrappedObject).DoubleClick -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Drag { add => ((IGlobalEventHandlers)WrappedObject).Drag += value; remove => ((IGlobalEventHandlers)WrappedObject).Drag -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragEnd { add => ((IGlobalEventHandlers)WrappedObject).DragEnd += value; remove => ((IGlobalEventHandlers)WrappedObject).DragEnd -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragEnter { add => ((IGlobalEventHandlers)WrappedObject).DragEnter += value; remove => ((IGlobalEventHandlers)WrappedObject).DragEnter -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragExit { add => ((IGlobalEventHandlers)WrappedObject).DragExit += value; remove => ((IGlobalEventHandlers)WrappedObject).DragExit -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragLeave { add => ((IGlobalEventHandlers)WrappedObject).DragLeave += value; remove => ((IGlobalEventHandlers)WrappedObject).DragLeave -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragOver { add => ((IGlobalEventHandlers)WrappedObject).DragOver += value; remove => ((IGlobalEventHandlers)WrappedObject).DragOver -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragStart { add => ((IGlobalEventHandlers)WrappedObject).DragStart += value; remove => ((IGlobalEventHandlers)WrappedObject).DragStart -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Dropped { add => ((IGlobalEventHandlers)WrappedObject).Dropped += value; remove => ((IGlobalEventHandlers)WrappedObject).Dropped -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DurationChanged { add => ((IGlobalEventHandlers)WrappedObject).DurationChanged += value; remove => ((IGlobalEventHandlers)WrappedObject).DurationChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Emptied { add => ((IGlobalEventHandlers)WrappedObject).Emptied += value; remove => ((IGlobalEventHandlers)WrappedObject).Emptied -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Ended { add => ((IGlobalEventHandlers)WrappedObject).Ended += value; remove => ((IGlobalEventHandlers)WrappedObject).Ended -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Error { add => ((IGlobalEventHandlers)WrappedObject).Error += value; remove => ((IGlobalEventHandlers)WrappedObject).Error -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Focused { add => ((IGlobalEventHandlers)WrappedObject).Focused += value; remove => ((IGlobalEventHandlers)WrappedObject).Focused -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Input { add => ((IGlobalEventHandlers)WrappedObject).Input += value; remove => ((IGlobalEventHandlers)WrappedObject).Input -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Invalid { add => ((IGlobalEventHandlers)WrappedObject).Invalid += value; remove => ((IGlobalEventHandlers)WrappedObject).Invalid -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyDown { add => ((IGlobalEventHandlers)WrappedObject).KeyDown += value; remove => ((IGlobalEventHandlers)WrappedObject).KeyDown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyPress { add => ((IGlobalEventHandlers)WrappedObject).KeyPress += value; remove => ((IGlobalEventHandlers)WrappedObject).KeyPress -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyUp { add => ((IGlobalEventHandlers)WrappedObject).KeyUp += value; remove => ((IGlobalEventHandlers)WrappedObject).KeyUp -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Loaded { add => ((IGlobalEventHandlers)WrappedObject).Loaded += value; remove => ((IGlobalEventHandlers)WrappedObject).Loaded -= value; }

        /// <inheritdoc/>
        public event DomEventHandler LoadedData { add => ((IGlobalEventHandlers)WrappedObject).LoadedData += value; remove => ((IGlobalEventHandlers)WrappedObject).LoadedData -= value; }

        /// <inheritdoc/>
        public event DomEventHandler LoadedMetadata { add => ((IGlobalEventHandlers)WrappedObject).LoadedMetadata += value; remove => ((IGlobalEventHandlers)WrappedObject).LoadedMetadata -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Loading { add => ((IGlobalEventHandlers)WrappedObject).Loading += value; remove => ((IGlobalEventHandlers)WrappedObject).Loading -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseDown { add => ((IGlobalEventHandlers)WrappedObject).MouseDown += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseDown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseEnter { add => ((IGlobalEventHandlers)WrappedObject).MouseEnter += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseEnter -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseLeave { add => ((IGlobalEventHandlers)WrappedObject).MouseLeave += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseLeave -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseMove { add => ((IGlobalEventHandlers)WrappedObject).MouseMove += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseMove -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseOut { add => ((IGlobalEventHandlers)WrappedObject).MouseOut += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseOut -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseOver { add => ((IGlobalEventHandlers)WrappedObject).MouseOver += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseOver -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseUp { add => ((IGlobalEventHandlers)WrappedObject).MouseUp += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseUp -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseWheel { add => ((IGlobalEventHandlers)WrappedObject).MouseWheel += value; remove => ((IGlobalEventHandlers)WrappedObject).MouseWheel -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Paused { add => ((IGlobalEventHandlers)WrappedObject).Paused += value; remove => ((IGlobalEventHandlers)WrappedObject).Paused -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Played { add => ((IGlobalEventHandlers)WrappedObject).Played += value; remove => ((IGlobalEventHandlers)WrappedObject).Played -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Playing { add => ((IGlobalEventHandlers)WrappedObject).Playing += value; remove => ((IGlobalEventHandlers)WrappedObject).Playing -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Progress { add => ((IGlobalEventHandlers)WrappedObject).Progress += value; remove => ((IGlobalEventHandlers)WrappedObject).Progress -= value; }

        /// <inheritdoc/>
        public event DomEventHandler RateChanged { add => ((IGlobalEventHandlers)WrappedObject).RateChanged += value; remove => ((IGlobalEventHandlers)WrappedObject).RateChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler ReadyStateChanged { add => WrappedObject.ReadyStateChanged += value; remove => WrappedObject.ReadyStateChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Resetted { add => ((IGlobalEventHandlers)WrappedObject).Resetted += value; remove => ((IGlobalEventHandlers)WrappedObject).Resetted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Resized { add => ((IGlobalEventHandlers)WrappedObject).Resized += value; remove => ((IGlobalEventHandlers)WrappedObject).Resized -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Scrolled { add => ((IGlobalEventHandlers)WrappedObject).Scrolled += value; remove => ((IGlobalEventHandlers)WrappedObject).Scrolled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Seeked { add => ((IGlobalEventHandlers)WrappedObject).Seeked += value; remove => ((IGlobalEventHandlers)WrappedObject).Seeked -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Seeking { add => ((IGlobalEventHandlers)WrappedObject).Seeking += value; remove => ((IGlobalEventHandlers)WrappedObject).Seeking -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Selected { add => ((IGlobalEventHandlers)WrappedObject).Selected += value; remove => ((IGlobalEventHandlers)WrappedObject).Selected -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Shown { add => ((IGlobalEventHandlers)WrappedObject).Shown += value; remove => ((IGlobalEventHandlers)WrappedObject).Shown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Stalled { add => ((IGlobalEventHandlers)WrappedObject).Stalled += value; remove => ((IGlobalEventHandlers)WrappedObject).Stalled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Submitted { add => ((IGlobalEventHandlers)WrappedObject).Submitted += value; remove => ((IGlobalEventHandlers)WrappedObject).Submitted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Suspended { add => ((IGlobalEventHandlers)WrappedObject).Suspended += value; remove => ((IGlobalEventHandlers)WrappedObject).Suspended -= value; }

        /// <inheritdoc/>
        public event DomEventHandler TimeUpdated { add => ((IGlobalEventHandlers)WrappedObject).TimeUpdated += value; remove => ((IGlobalEventHandlers)WrappedObject).TimeUpdated -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Toggled { add => ((IGlobalEventHandlers)WrappedObject).Toggled += value; remove => ((IGlobalEventHandlers)WrappedObject).Toggled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler VolumeChanged { add => ((IGlobalEventHandlers)WrappedObject).VolumeChanged += value; remove => ((IGlobalEventHandlers)WrappedObject).VolumeChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Waiting { add => ((IGlobalEventHandlers)WrappedObject).Waiting += value; remove => ((IGlobalEventHandlers)WrappedObject).Waiting -= value; }

        private static readonly int ActiveElementKey = HashCode.Combine(nameof(ActiveElement));
        /// <inheritdoc/>
        public IElement ActiveElement { get => GetOrWrap(ActiveElementKey, () => WrappedObject.ActiveElement); }

        private static readonly int AllKey = HashCode.Combine(nameof(All));
        /// <inheritdoc/>
        public IHtmlAllCollection All { get => GetOrWrap(AllKey, () => WrappedObject.All); }

        private static readonly int AnchorsKey = HashCode.Combine(nameof(Anchors));
        /// <inheritdoc/>
        public IHtmlCollection<IHtmlAnchorElement> Anchors { get => GetOrWrap(AnchorsKey, () => WrappedObject.Anchors); }

        /// <inheritdoc/>
        public String BaseUri { get => WrappedObject.BaseUri; }

        /// <inheritdoc/>
        public Url BaseUrl { get => WrappedObject.BaseUrl; }

        private static readonly int BodyKey = HashCode.Combine(nameof(Body));
        /// <inheritdoc/>
        public IHtmlElement Body { get => GetOrWrap(BodyKey, () => WrappedObject.Body); set { WrappedObject.Body = value; MarkAsStale(); }}

        /// <inheritdoc/>
        public String CharacterSet { get => WrappedObject.CharacterSet; }

        /// <inheritdoc/>
        public Int32 ChildElementCount { get => WrappedObject.ChildElementCount; }

        private static readonly int ChildNodesKey = HashCode.Combine(nameof(ChildNodes));
        /// <inheritdoc/>
        public INodeList ChildNodes { get => GetOrWrap(ChildNodesKey, () => WrappedObject.ChildNodes); }

        private static readonly int ChildrenKey = HashCode.Combine(nameof(Children));
        /// <inheritdoc/>
        public IHtmlCollection<IElement> Children { get => GetOrWrap(ChildrenKey, () => WrappedObject.Children); }

        private static readonly int CommandsKey = HashCode.Combine(nameof(Commands));
        /// <inheritdoc/>
        public IHtmlCollection<IElement> Commands { get => GetOrWrap(CommandsKey, () => WrappedObject.Commands); }

        /// <inheritdoc/>
        public String CompatMode { get => WrappedObject.CompatMode; }

        /// <inheritdoc/>
        public String ContentType { get => WrappedObject.ContentType; }

        /// <inheritdoc/>
        public IBrowsingContext Context { get => WrappedObject.Context; }

        /// <inheritdoc/>
        public String Cookie { get => WrappedObject.Cookie; set => WrappedObject.Cookie = value;}

        private static readonly int CurrentScriptKey = HashCode.Combine(nameof(CurrentScript));
        /// <inheritdoc/>
        public IHtmlScriptElement CurrentScript { get => GetOrWrap(CurrentScriptKey, () => WrappedObject.CurrentScript); }

        /// <inheritdoc/>
        public IWindow DefaultView { get => WrappedObject.DefaultView; }

        /// <inheritdoc/>
        public String DesignMode { get => WrappedObject.DesignMode; set => WrappedObject.DesignMode = value;}

        /// <inheritdoc/>
        public String Direction { get => WrappedObject.Direction; set => WrappedObject.Direction = value;}

        private static readonly int DoctypeKey = HashCode.Combine(nameof(Doctype));
        /// <inheritdoc/>
        public IDocumentType Doctype { get => GetOrWrap(DoctypeKey, () => WrappedObject.Doctype); }

        private static readonly int DocumentElementKey = HashCode.Combine(nameof(DocumentElement));
        /// <inheritdoc/>
        public IElement DocumentElement { get => GetOrWrap(DocumentElementKey, () => WrappedObject.DocumentElement); }

        /// <inheritdoc/>
        public String DocumentUri { get => WrappedObject.DocumentUri; }

        /// <inheritdoc/>
        public String Domain { get => WrappedObject.Domain; set => WrappedObject.Domain = value;}

        /// <inheritdoc/>
        public IEntityProvider Entities { get => WrappedObject.Entities; }

        private static readonly int FirstChildKey = HashCode.Combine(nameof(FirstChild));
        /// <inheritdoc/>
        public INode FirstChild { get => GetOrWrap(FirstChildKey, () => WrappedObject.FirstChild); }

        private static readonly int FirstElementChildKey = HashCode.Combine(nameof(FirstElementChild));
        /// <inheritdoc/>
        public IElement FirstElementChild { get => GetOrWrap(FirstElementChildKey, () => WrappedObject.FirstElementChild); }

        /// <inheritdoc/>
        public NodeFlags Flags { get => WrappedObject.Flags; }

        private static readonly int FormsKey = HashCode.Combine(nameof(Forms));
        /// <inheritdoc/>
        public IHtmlCollection<IHtmlFormElement> Forms { get => GetOrWrap(FormsKey, () => WrappedObject.Forms); }

        /// <inheritdoc/>
        public Boolean HasChildNodes { get => WrappedObject.HasChildNodes; }

        private static readonly int HeadKey = HashCode.Combine(nameof(Head));
        /// <inheritdoc/>
        public IHtmlHeadElement Head { get => GetOrWrap(HeadKey, () => WrappedObject.Head); }

        private static readonly int ImagesKey = HashCode.Combine(nameof(Images));
        /// <inheritdoc/>
        public IHtmlCollection<IHtmlImageElement> Images { get => GetOrWrap(ImagesKey, () => WrappedObject.Images); }

        /// <inheritdoc/>
        public IImplementation Implementation { get => WrappedObject.Implementation; }

        private static readonly int ImportAncestorKey = HashCode.Combine(nameof(ImportAncestor));
        /// <inheritdoc/>
        public IDocument ImportAncestor { get => GetOrWrap(ImportAncestorKey, () => WrappedObject.ImportAncestor); }

        private static readonly int LastChildKey = HashCode.Combine(nameof(LastChild));
        /// <inheritdoc/>
        public INode LastChild { get => GetOrWrap(LastChildKey, () => WrappedObject.LastChild); }

        private static readonly int LastElementChildKey = HashCode.Combine(nameof(LastElementChild));
        /// <inheritdoc/>
        public IElement LastElementChild { get => GetOrWrap(LastElementChildKey, () => WrappedObject.LastElementChild); }

        /// <inheritdoc/>
        public String LastModified { get => WrappedObject.LastModified; }

        /// <inheritdoc/>
        public String LastStyleSheetSet { get => WrappedObject.LastStyleSheetSet; }

        private static readonly int LinksKey = HashCode.Combine(nameof(Links));
        /// <inheritdoc/>
        public IHtmlCollection<IElement> Links { get => GetOrWrap(LinksKey, () => WrappedObject.Links); }

        /// <inheritdoc/>
        public ILocation Location { get => WrappedObject.Location; }

        private static readonly int NextSiblingKey = HashCode.Combine(nameof(NextSibling));
        /// <inheritdoc/>
        public INode NextSibling { get => GetOrWrap(NextSiblingKey, () => WrappedObject.NextSibling); }

        /// <inheritdoc/>
        public String NodeName { get => WrappedObject.NodeName; }

        /// <inheritdoc/>
        public NodeType NodeType { get => WrappedObject.NodeType; }

        /// <inheritdoc/>
        public String NodeValue { get => WrappedObject.NodeValue; set => WrappedObject.NodeValue = value;}

        /// <inheritdoc/>
        public String Origin { get => WrappedObject.Origin; }

        private static readonly int OwnerKey = HashCode.Combine(nameof(Owner));
        /// <inheritdoc/>
        public IDocument Owner { get => GetOrWrap(OwnerKey, () => WrappedObject.Owner); }

        private static readonly int ParentKey = HashCode.Combine(nameof(Parent));
        /// <inheritdoc/>
        public INode Parent { get => GetOrWrap(ParentKey, () => WrappedObject.Parent); }

        private static readonly int ParentElementKey = HashCode.Combine(nameof(ParentElement));
        /// <inheritdoc/>
        public IElement ParentElement { get => GetOrWrap(ParentElementKey, () => WrappedObject.ParentElement); }

        private static readonly int PluginsKey = HashCode.Combine(nameof(Plugins));
        /// <inheritdoc/>
        public IHtmlCollection<IHtmlEmbedElement> Plugins { get => GetOrWrap(PluginsKey, () => WrappedObject.Plugins); }

        /// <inheritdoc/>
        public String PreferredStyleSheetSet { get => WrappedObject.PreferredStyleSheetSet; }

        private static readonly int PreviousSiblingKey = HashCode.Combine(nameof(PreviousSibling));
        /// <inheritdoc/>
        public INode PreviousSibling { get => GetOrWrap(PreviousSiblingKey, () => WrappedObject.PreviousSibling); }

        /// <inheritdoc/>
        public DocumentReadyState ReadyState { get => WrappedObject.ReadyState; }

        /// <inheritdoc/>
        public String Referrer { get => WrappedObject.Referrer; }

        private static readonly int ScriptsKey = HashCode.Combine(nameof(Scripts));
        /// <inheritdoc/>
        public IHtmlCollection<IHtmlScriptElement> Scripts { get => GetOrWrap(ScriptsKey, () => WrappedObject.Scripts); }

        /// <inheritdoc/>
        public String SelectedStyleSheetSet { get => WrappedObject.SelectedStyleSheetSet; set => WrappedObject.SelectedStyleSheetSet = value;}

        /// <inheritdoc/>
        public TextSource Source { get => WrappedObject.Source; }

        /// <inheritdoc/>
        public HttpStatusCode StatusCode { get => WrappedObject.StatusCode; }

        private static readonly int StyleSheetsKey = HashCode.Combine(nameof(StyleSheets));
        /// <inheritdoc/>
        public IStyleSheetList StyleSheets { get => GetOrWrap(StyleSheetsKey, () => WrappedObject.StyleSheets); }

        private static readonly int StyleSheetSetsKey = HashCode.Combine(nameof(StyleSheetSets));
        /// <inheritdoc/>
        public IStringList StyleSheetSets { get => GetOrWrap(StyleSheetSetsKey, () => WrappedObject.StyleSheetSets); }

        /// <inheritdoc/>
        public String TextContent { get => WrappedObject.TextContent; set => WrappedObject.TextContent = value;}

        /// <inheritdoc/>
        public String Title { get => WrappedObject.Title; set => WrappedObject.Title = value;}

        /// <inheritdoc/>
        public String Url { get => WrappedObject.Url; }

        /// <inheritdoc/>
        public void AddEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.AddEventListener(type, callback, capture);

        /// <inheritdoc/>
        public INode Adopt(INode externalNode)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(Adopt), externalNode), () => WrappedObject.Adopt(externalNode));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void Append(INode[] nodes)
            => WrappedObject.Append(nodes);

        /// <inheritdoc/>
        public INode AppendChild(INode child)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(AppendChild), child), () => WrappedObject.AppendChild(child));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public INode Clone(Boolean deep)
            => GetOrWrap(HashCode.Combine(nameof(Clone), deep), () => WrappedObject.Clone(deep));

        /// <inheritdoc/>
        public void Close()
            => WrappedObject.Close();

        /// <inheritdoc/>
        public DocumentPositions CompareDocumentPosition(INode otherNode)
            => WrappedObject.CompareDocumentPosition(otherNode);

        /// <inheritdoc/>
        public Boolean Contains(INode otherNode)
            => WrappedObject.Contains(otherNode);

        /// <inheritdoc/>
        public IAttr CreateAttribute(String name)
            => GetOrWrap(HashCode.Combine(nameof(CreateAttribute), name), () => WrappedObject.CreateAttribute(name));

        /// <inheritdoc/>
        public IAttr CreateAttribute(String namespaceUri, String name)
            => GetOrWrap(HashCode.Combine(nameof(CreateAttribute), namespaceUri, name), () => WrappedObject.CreateAttribute(namespaceUri, name));

        /// <inheritdoc/>
        public IComment CreateComment(String data)
            => GetOrWrap(HashCode.Combine(nameof(CreateComment), data), () => WrappedObject.CreateComment(data));

        private static readonly int CreateDocumentFragmentKey = HashCode.Combine(nameof(CreateDocumentFragment));
        /// <inheritdoc/>
        public IDocumentFragment CreateDocumentFragment()
            => GetOrWrap(CreateDocumentFragmentKey, () => WrappedObject.CreateDocumentFragment());

        /// <inheritdoc/>
        public IElement CreateElement(String name)
            => GetOrWrap(HashCode.Combine(nameof(CreateElement), name), () => WrappedObject.CreateElement(name));

        /// <inheritdoc/>
        public IElement CreateElement(String namespaceUri, String name)
            => GetOrWrap(HashCode.Combine(nameof(CreateElement), namespaceUri, name), () => WrappedObject.CreateElement(namespaceUri, name));

        /// <inheritdoc/>
        public Event CreateEvent(String type)
            => WrappedObject.CreateEvent(type);

        /// <inheritdoc/>
        public INodeIterator CreateNodeIterator(INode root, FilterSettings settings, NodeFilter filter)
            => WrappedObject.CreateNodeIterator(root, settings, filter);

        /// <inheritdoc/>
        public IProcessingInstruction CreateProcessingInstruction(String target, String data)
            => GetOrWrap(HashCode.Combine(nameof(CreateProcessingInstruction), target, data), () => WrappedObject.CreateProcessingInstruction(target, data));

        /// <inheritdoc/>
        public IRange CreateRange()
            => WrappedObject.CreateRange();

        /// <inheritdoc/>
        public IText CreateTextNode(String data)
            => GetOrWrap(HashCode.Combine(nameof(CreateTextNode), data), () => WrappedObject.CreateTextNode(data));

        /// <inheritdoc/>
        public ITreeWalker CreateTreeWalker(INode root, FilterSettings settings, NodeFilter filter)
            => WrappedObject.CreateTreeWalker(root, settings, filter);

        /// <inheritdoc/>
        public Boolean Dispatch(Event ev)
            => WrappedObject.Dispatch(ev);

        /// <inheritdoc/>
        public void Dispose()
            => WrappedObject.Dispose();

        /// <inheritdoc/>
        public void EnableStyleSheetsForSet(String name)
            => WrappedObject.EnableStyleSheetsForSet(name);

        /// <inheritdoc/>
        public Boolean Equals(INode otherNode)
            => WrappedObject.Equals(otherNode);

        /// <inheritdoc/>
        public Boolean ExecuteCommand(String commandId, Boolean showUserInterface, String value)
            => WrappedObject.ExecuteCommand(commandId, showUserInterface, value);

        /// <inheritdoc/>
        public String GetCommandValue(String commandId)
            => WrappedObject.GetCommandValue(commandId);

        /// <inheritdoc/>
        public IElement GetElementById(String elementId)
            => GetOrWrap(HashCode.Combine(nameof(GetElementById), elementId), () => WrappedObject.GetElementById(elementId));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByClassName(String classNames)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByClassName), classNames), () => WrappedObject.GetElementsByClassName(classNames));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByName(String name)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByName), name), () => WrappedObject.GetElementsByName(name));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByTagName(String tagName)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByTagName), tagName), () => WrappedObject.GetElementsByTagName(tagName));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByTagName(String namespaceUri, String tagName)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByTagName), namespaceUri, tagName), () => WrappedObject.GetElementsByTagName(namespaceUri, tagName));

        /// <inheritdoc/>
        public Boolean HasFocus()
            => WrappedObject.HasFocus();

        /// <inheritdoc/>
        public INode Import(INode externalNode, Boolean deep)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(Import), externalNode, deep), () => WrappedObject.Import(externalNode, deep));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public INode InsertBefore(INode newElement, INode referenceElement)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(InsertBefore), newElement, referenceElement), () => WrappedObject.InsertBefore(newElement, referenceElement));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void InvokeEventListener(Event ev)
            => WrappedObject.InvokeEventListener(ev);

        /// <inheritdoc/>
        public Boolean IsCommandEnabled(String commandId)
            => WrappedObject.IsCommandEnabled(commandId);

        /// <inheritdoc/>
        public Boolean IsCommandExecuted(String commandId)
            => WrappedObject.IsCommandExecuted(commandId);

        /// <inheritdoc/>
        public Boolean IsCommandIndeterminate(String commandId)
            => WrappedObject.IsCommandIndeterminate(commandId);

        /// <inheritdoc/>
        public Boolean IsCommandSupported(String commandId)
            => WrappedObject.IsCommandSupported(commandId);

        /// <inheritdoc/>
        public Boolean IsDefaultNamespace(String namespaceUri)
            => WrappedObject.IsDefaultNamespace(namespaceUri);

        /// <inheritdoc/>
        public void Load(String url)
            => WrappedObject.Load(url);

        /// <inheritdoc/>
        public String LookupNamespaceUri(String prefix)
            => WrappedObject.LookupNamespaceUri(prefix);

        /// <inheritdoc/>
        public String LookupPrefix(String namespaceUri)
            => WrappedObject.LookupPrefix(namespaceUri);

        /// <inheritdoc/>
        public void Normalize()
            => WrappedObject.Normalize();

        /// <inheritdoc/>
        public IDocument Open(String type, String replace)
            => GetOrWrap(HashCode.Combine(nameof(Open), type, replace), () => WrappedObject.Open(type, replace));

        /// <inheritdoc/>
        public void Prepend(INode[] nodes)
            => WrappedObject.Prepend(nodes);

        /// <inheritdoc/>
        public IElement QuerySelector(String selectors)
            => GetOrWrap(HashCode.Combine(nameof(QuerySelector), selectors), () => WrappedObject.QuerySelector(selectors));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> QuerySelectorAll(String selectors)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(QuerySelectorAll), selectors), () => WrappedObject.QuerySelectorAll(selectors));

        /// <inheritdoc/>
        public INode RemoveChild(INode child)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(RemoveChild), child), () => WrappedObject.RemoveChild(child));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void RemoveEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.RemoveEventListener(type, callback, capture);

        /// <inheritdoc/>
        public INode ReplaceChild(INode newChild, INode oldChild)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(ReplaceChild), newChild, oldChild), () => WrappedObject.ReplaceChild(newChild, oldChild));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
            => WrappedObject.ToHtml(writer, formatter);

        /// <inheritdoc/>
        public void Write(String content)
            => WrappedObject.Write(content);

        /// <inheritdoc/>
        public void WriteLine(String content)
            => WrappedObject.WriteLine(content);
    }
}
