using System;
using System.IO;
using System.Net;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;
using AngleSharp.Text;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IDocument"/> type.
    /// </summary>
    public partial class DocumentWrapper : Wrapper<IDocument>, IDocument, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="DocumentWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public DocumentWrapper(Func<IDocument?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public event DomEventHandler Aborted { add => WrappedObject.Aborted += value; remove => WrappedObject.Aborted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Blurred { add => WrappedObject.Blurred += value; remove => WrappedObject.Blurred -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Cancelled { add => WrappedObject.Cancelled += value; remove => WrappedObject.Cancelled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CanPlay { add => WrappedObject.CanPlay += value; remove => WrappedObject.CanPlay -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CanPlayThrough { add => WrappedObject.CanPlayThrough += value; remove => WrappedObject.CanPlayThrough -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Changed { add => WrappedObject.Changed += value; remove => WrappedObject.Changed -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Clicked { add => WrappedObject.Clicked += value; remove => WrappedObject.Clicked -= value; }

        /// <inheritdoc/>
        public event DomEventHandler CueChanged { add => WrappedObject.CueChanged += value; remove => WrappedObject.CueChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DoubleClick { add => WrappedObject.DoubleClick += value; remove => WrappedObject.DoubleClick -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Drag { add => WrappedObject.Drag += value; remove => WrappedObject.Drag -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragEnd { add => WrappedObject.DragEnd += value; remove => WrappedObject.DragEnd -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragEnter { add => WrappedObject.DragEnter += value; remove => WrappedObject.DragEnter -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragExit { add => WrappedObject.DragExit += value; remove => WrappedObject.DragExit -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragLeave { add => WrappedObject.DragLeave += value; remove => WrappedObject.DragLeave -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragOver { add => WrappedObject.DragOver += value; remove => WrappedObject.DragOver -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DragStart { add => WrappedObject.DragStart += value; remove => WrappedObject.DragStart -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Dropped { add => WrappedObject.Dropped += value; remove => WrappedObject.Dropped -= value; }

        /// <inheritdoc/>
        public event DomEventHandler DurationChanged { add => WrappedObject.DurationChanged += value; remove => WrappedObject.DurationChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Emptied { add => WrappedObject.Emptied += value; remove => WrappedObject.Emptied -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Ended { add => WrappedObject.Ended += value; remove => WrappedObject.Ended -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Error { add => WrappedObject.Error += value; remove => WrappedObject.Error -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Focused { add => WrappedObject.Focused += value; remove => WrappedObject.Focused -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Input { add => WrappedObject.Input += value; remove => WrappedObject.Input -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Invalid { add => WrappedObject.Invalid += value; remove => WrappedObject.Invalid -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyDown { add => WrappedObject.KeyDown += value; remove => WrappedObject.KeyDown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyPress { add => WrappedObject.KeyPress += value; remove => WrappedObject.KeyPress -= value; }

        /// <inheritdoc/>
        public event DomEventHandler KeyUp { add => WrappedObject.KeyUp += value; remove => WrappedObject.KeyUp -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Loaded { add => WrappedObject.Loaded += value; remove => WrappedObject.Loaded -= value; }

        /// <inheritdoc/>
        public event DomEventHandler LoadedData { add => WrappedObject.LoadedData += value; remove => WrappedObject.LoadedData -= value; }

        /// <inheritdoc/>
        public event DomEventHandler LoadedMetadata { add => WrappedObject.LoadedMetadata += value; remove => WrappedObject.LoadedMetadata -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Loading { add => WrappedObject.Loading += value; remove => WrappedObject.Loading -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseDown { add => WrappedObject.MouseDown += value; remove => WrappedObject.MouseDown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseEnter { add => WrappedObject.MouseEnter += value; remove => WrappedObject.MouseEnter -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseLeave { add => WrappedObject.MouseLeave += value; remove => WrappedObject.MouseLeave -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseMove { add => WrappedObject.MouseMove += value; remove => WrappedObject.MouseMove -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseOut { add => WrappedObject.MouseOut += value; remove => WrappedObject.MouseOut -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseOver { add => WrappedObject.MouseOver += value; remove => WrappedObject.MouseOver -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseUp { add => WrappedObject.MouseUp += value; remove => WrappedObject.MouseUp -= value; }

        /// <inheritdoc/>
        public event DomEventHandler MouseWheel { add => WrappedObject.MouseWheel += value; remove => WrappedObject.MouseWheel -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Paused { add => WrappedObject.Paused += value; remove => WrappedObject.Paused -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Played { add => WrappedObject.Played += value; remove => WrappedObject.Played -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Playing { add => WrappedObject.Playing += value; remove => WrappedObject.Playing -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Progress { add => WrappedObject.Progress += value; remove => WrappedObject.Progress -= value; }

        /// <inheritdoc/>
        public event DomEventHandler RateChanged { add => WrappedObject.RateChanged += value; remove => WrappedObject.RateChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler ReadyStateChanged { add => WrappedObject.ReadyStateChanged += value; remove => WrappedObject.ReadyStateChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Resetted { add => WrappedObject.Resetted += value; remove => WrappedObject.Resetted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Resized { add => WrappedObject.Resized += value; remove => WrappedObject.Resized -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Scrolled { add => WrappedObject.Scrolled += value; remove => WrappedObject.Scrolled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Seeked { add => WrappedObject.Seeked += value; remove => WrappedObject.Seeked -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Seeking { add => WrappedObject.Seeking += value; remove => WrappedObject.Seeking -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Selected { add => WrappedObject.Selected += value; remove => WrappedObject.Selected -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Shown { add => WrappedObject.Shown += value; remove => WrappedObject.Shown -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Stalled { add => WrappedObject.Stalled += value; remove => WrappedObject.Stalled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Submitted { add => WrappedObject.Submitted += value; remove => WrappedObject.Submitted -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Suspended { add => WrappedObject.Suspended += value; remove => WrappedObject.Suspended -= value; }

        /// <inheritdoc/>
        public event DomEventHandler TimeUpdated { add => WrappedObject.TimeUpdated += value; remove => WrappedObject.TimeUpdated -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Toggled { add => WrappedObject.Toggled += value; remove => WrappedObject.Toggled -= value; }

        /// <inheritdoc/>
        public event DomEventHandler VolumeChanged { add => WrappedObject.VolumeChanged += value; remove => WrappedObject.VolumeChanged -= value; }

        /// <inheritdoc/>
        public event DomEventHandler Waiting { add => WrappedObject.Waiting += value; remove => WrappedObject.Waiting -= value; }

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
