using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Html.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    internal interface INodeWrapper
    {
        void MarkAsStale();
    }

    internal abstract class DomNodeWrapper<TNode> : INodeWrapper where TNode : class, INode
    {
        private readonly Func<TNode?> _getDomNode;
        private readonly Dictionary<int, INodeWrapper> _wrappers = new Dictionary<int, INodeWrapper>();

        private TNode? _wrappedNode;

        public TNode WrappedNode
        {
            get
            {
                if (_wrappedNode is null)
                {
                    _wrappedNode = _getDomNode();
                    if (_wrappedNode is null)
                        throw new NodeNoLongerAvailableException();
                }
                return _wrappedNode!;
            }
        }
        
        internal DomNodeWrapper(Func<TNode?> getWrappedNode)
        {
            _getDomNode = getWrappedNode;
            _wrappedNode = getWrappedNode();
        }

        public void MarkAsStale()
        {
            _wrappedNode = null;
            foreach (var wrapped in _wrappers.Values) wrapped.MarkAsStale();
        }

        protected IElement GetOrWrap(int key, Func<IElement> nodeQuery)
        {
            if (!_wrappers.TryGetValue(key, out var result))
            {
                result = new ElementWrapper(nodeQuery);
                _wrappers.Add(key, result);
            }
            return (IElement)result;
        }
    }

    internal class ElementWrapper : DomNodeWrapper<IElement>, IElement
    {
        public ElementWrapper(Func<IElement?> getDomNode) : base(getDomNode)
        {
        }

        public string Prefix => WrappedNode.Prefix;

        public string LocalName => WrappedNode.LocalName;

        public string NamespaceUri => WrappedNode.NamespaceUri;

        public INamedNodeMap Attributes => WrappedNode.Attributes;

        public ITokenList ClassList => WrappedNode.ClassList;

        public string ClassName { get => WrappedNode.ClassName; set => WrappedNode.ClassName = value; }
        public string Id { get => WrappedNode.Id; set => WrappedNode.Id = value; }
        public string InnerHtml { get => WrappedNode.InnerHtml; set => WrappedNode.InnerHtml = value; }
        public string OuterHtml { get => WrappedNode.OuterHtml; set => WrappedNode.OuterHtml = value; }

        public string TagName => WrappedNode.TagName;

        public IElement AssignedSlot => WrappedNode.AssignedSlot;

        public string Slot { get => WrappedNode.Slot; set => WrappedNode.Slot = value; }

        public IShadowRoot ShadowRoot => WrappedNode.ShadowRoot;

        public bool IsFocused => WrappedNode.IsFocused;

        public ISourceReference SourceReference => WrappedNode.SourceReference;

        public string BaseUri => WrappedNode.BaseUri;

        public Url BaseUrl => WrappedNode.BaseUrl;

        public string NodeName => WrappedNode.NodeName;

        public INodeList ChildNodes => WrappedNode.ChildNodes;

        public IDocument Owner => WrappedNode.Owner;

        public static int ParentElementKey = HashCode.Combine(nameof(ParentElement));
        public IElement ParentElement => GetOrWrap(ParentElementKey, () => WrappedNode.ParentElement);

        public INode Parent => WrappedNode.Parent;

        public INode FirstChild => WrappedNode.FirstChild;

        public INode LastChild => WrappedNode.LastChild;

        public INode NextSibling => WrappedNode.NextSibling;

        public INode PreviousSibling => WrappedNode.PreviousSibling;

        public NodeType NodeType => WrappedNode.NodeType;

        public string NodeValue { get => WrappedNode.NodeValue; set => WrappedNode.NodeValue = value; }
        public string TextContent { get => WrappedNode.TextContent; set => WrappedNode.TextContent = value; }

        public bool HasChildNodes => WrappedNode.HasChildNodes;

        public NodeFlags Flags => WrappedNode.Flags;

        public IHtmlCollection<IElement> Children => WrappedNode.Children;

        public IElement FirstElementChild => WrappedNode.FirstElementChild;

        public IElement LastElementChild => WrappedNode.LastElementChild;

        public int ChildElementCount => WrappedNode.ChildElementCount;

        public IElement NextElementSibling => WrappedNode.NextElementSibling;

        public IElement PreviousElementSibling => WrappedNode.PreviousElementSibling;

        public void Insert(AdjacentPosition position, string html) => WrappedNode.Insert(position, html);
        public bool HasAttribute(string name) => WrappedNode.HasAttribute(name);
        public bool HasAttribute(string namespaceUri, string localName) => WrappedNode.HasAttribute(namespaceUri, localName);
        public string GetAttribute(string name) => WrappedNode.GetAttribute(name);
        public string GetAttribute(string namespaceUri, string localName) => WrappedNode.GetAttribute(namespaceUri, localName);
        public void SetAttribute(string name, string value) => WrappedNode.SetAttribute(name, value);
        public void SetAttribute(string namespaceUri, string name, string value) => WrappedNode.SetAttribute(namespaceUri, name, value);
        public bool RemoveAttribute(string name) => WrappedNode.RemoveAttribute(name);
        public bool RemoveAttribute(string namespaceUri, string localName) => WrappedNode.RemoveAttribute(namespaceUri, localName);
        public IHtmlCollection<IElement> GetElementsByClassName(string classNames) => WrappedNode.GetElementsByClassName(classNames);
        public IHtmlCollection<IElement> GetElementsByTagName(string tagName) => WrappedNode.GetElementsByTagName(tagName);
        public IHtmlCollection<IElement> GetElementsByTagNameNS(string namespaceUri, string tagName) => WrappedNode.GetElementsByTagNameNS(namespaceUri, tagName);
        public bool Matches(string selectors) => WrappedNode.Matches(selectors);
        public IElement Closest(string selectors) => WrappedNode.Closest(selectors);
        public IShadowRoot AttachShadow(ShadowRootMode mode = ShadowRootMode.Open) => WrappedNode.AttachShadow(mode);
        public INode Clone(bool deep = true) => WrappedNode.Clone(deep);
        public bool Equals(INode otherNode) => WrappedNode.Equals(otherNode);
        public DocumentPositions CompareDocumentPosition(INode otherNode) => WrappedNode.CompareDocumentPosition(otherNode);
        public void Normalize() => WrappedNode.Normalize();
        public bool Contains(INode otherNode) => WrappedNode.Contains(otherNode);
        public bool IsDefaultNamespace(string namespaceUri) => WrappedNode.IsDefaultNamespace(namespaceUri);
        public string LookupNamespaceUri(string prefix) => WrappedNode.LookupNamespaceUri(prefix);
        public string LookupPrefix(string namespaceUri) => WrappedNode.LookupPrefix(namespaceUri);
        public INode AppendChild(INode child) => WrappedNode.AppendChild(child);
        public INode InsertBefore(INode newElement, INode referenceElement) => WrappedNode.InsertBefore(newElement, referenceElement);
        public INode RemoveChild(INode child) => WrappedNode.RemoveChild(child);
        public INode ReplaceChild(INode newChild, INode oldChild) => WrappedNode.ReplaceChild(newChild, oldChild);
        public void AddEventListener(string type, DomEventHandler? callback = null, bool capture = false) => WrappedNode.AddEventListener(type, callback, capture);
        public void RemoveEventListener(string type, DomEventHandler? callback = null, bool capture = false) => WrappedNode.RemoveEventListener(type, callback, capture);
        public void InvokeEventListener(Event ev) => WrappedNode.InvokeEventListener(ev);
        public bool Dispatch(Event ev) => WrappedNode.Dispatch(ev);
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => WrappedNode.ToHtml(writer, formatter);
        public void Append(params INode[] nodes) => WrappedNode.Append(nodes);
        public void Prepend(params INode[] nodes) => WrappedNode.Prepend(nodes);
        public IElement QuerySelector(string selectors) => WrappedNode.QuerySelector(selectors);
        public IHtmlCollection<IElement> QuerySelectorAll(string selectors) => WrappedNode.QuerySelectorAll(selectors);
        public void Before(params INode[] nodes) => WrappedNode.Before(nodes);
        public void After(params INode[] nodes) => WrappedNode.After(nodes);
        public void Replace(params INode[] nodes) => WrappedNode.Replace(nodes);
        public void Remove() => WrappedNode.Remove();
    }
}
