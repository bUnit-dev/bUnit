using System;
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
    internal class ElementWrapper : WrapperBase<IElement>, IElement
    {
        public ElementWrapper(Func<IElement?> getWrapped) : base(getWrapped)
        {
        }

        public string Prefix => WrappedObject.Prefix;

        public string LocalName => WrappedObject.LocalName;

        public string NamespaceUri => WrappedObject.NamespaceUri;

        public INamedNodeMap Attributes => WrappedObject.Attributes;

        public ITokenList ClassList => WrappedObject.ClassList;

        public string ClassName { get => WrappedObject.ClassName; set => WrappedObject.ClassName = value; }
        public string Id { get => WrappedObject.Id; set => WrappedObject.Id = value; }
        public string InnerHtml { get => WrappedObject.InnerHtml; set => WrappedObject.InnerHtml = value; }
        public string OuterHtml { get => WrappedObject.OuterHtml; set => WrappedObject.OuterHtml = value; }

        public string TagName => WrappedObject.TagName;

        public IElement AssignedSlot => WrappedObject.AssignedSlot;

        public string Slot { get => WrappedObject.Slot; set => WrappedObject.Slot = value; }

        public IShadowRoot ShadowRoot => WrappedObject.ShadowRoot;

        public bool IsFocused => WrappedObject.IsFocused;

        public ISourceReference SourceReference => WrappedObject.SourceReference;

        public string BaseUri => WrappedObject.BaseUri;

        public Url BaseUrl => WrappedObject.BaseUrl;

        public string NodeName => WrappedObject.NodeName;

        public INodeList ChildNodes => WrappedObject.ChildNodes;

        public IDocument Owner => WrappedObject.Owner;

        public static int ParentElementKey = HashCode.Combine(nameof(ParentElement));
        public IElement ParentElement => GetOrWrap<IElement>(ParentElementKey, () => WrappedObject.ParentElement);

        public INode Parent => WrappedObject.Parent;

        public INode FirstChild => WrappedObject.FirstChild;

        public INode LastChild => WrappedObject.LastChild;

        public INode NextSibling => WrappedObject.NextSibling;

        public INode PreviousSibling => WrappedObject.PreviousSibling;

        public NodeType NodeType => WrappedObject.NodeType;

        public string NodeValue { get => WrappedObject.NodeValue; set => WrappedObject.NodeValue = value; }
        public string TextContent { get => WrappedObject.TextContent; set => WrappedObject.TextContent = value; }

        public bool HasChildNodes => WrappedObject.HasChildNodes;

        public NodeFlags Flags => WrappedObject.Flags;

        public IHtmlCollection<IElement> Children => WrappedObject.Children;

        public IElement FirstElementChild => WrappedObject.FirstElementChild;

        public IElement LastElementChild => WrappedObject.LastElementChild;

        public int ChildElementCount => WrappedObject.ChildElementCount;

        public IElement NextElementSibling => WrappedObject.NextElementSibling;

        public IElement PreviousElementSibling => WrappedObject.PreviousElementSibling;

        public void Insert(AdjacentPosition position, string html) => WrappedObject.Insert(position, html);
        public bool HasAttribute(string name) => WrappedObject.HasAttribute(name);
        public bool HasAttribute(string namespaceUri, string localName) => WrappedObject.HasAttribute(namespaceUri, localName);
        public string GetAttribute(string name) => WrappedObject.GetAttribute(name);
        public string GetAttribute(string namespaceUri, string localName) => WrappedObject.GetAttribute(namespaceUri, localName);
        public void SetAttribute(string name, string value) => WrappedObject.SetAttribute(name, value);
        public void SetAttribute(string namespaceUri, string name, string value) => WrappedObject.SetAttribute(namespaceUri, name, value);
        public bool RemoveAttribute(string name) => WrappedObject.RemoveAttribute(name);
        public bool RemoveAttribute(string namespaceUri, string localName) => WrappedObject.RemoveAttribute(namespaceUri, localName);
        public IHtmlCollection<IElement> GetElementsByClassName(string classNames) => WrappedObject.GetElementsByClassName(classNames);
        public IHtmlCollection<IElement> GetElementsByTagName(string tagName) => WrappedObject.GetElementsByTagName(tagName);
        public IHtmlCollection<IElement> GetElementsByTagNameNS(string namespaceUri, string tagName) => WrappedObject.GetElementsByTagNameNS(namespaceUri, tagName);
        public bool Matches(string selectors) => WrappedObject.Matches(selectors);
        public IElement Closest(string selectors) => WrappedObject.Closest(selectors);
        public IShadowRoot AttachShadow(ShadowRootMode mode = ShadowRootMode.Open) => WrappedObject.AttachShadow(mode);
        public INode Clone(bool deep = true) => WrappedObject.Clone(deep);
        public bool Equals(INode otherNode) => WrappedObject.Equals(otherNode);
        public DocumentPositions CompareDocumentPosition(INode otherNode) => WrappedObject.CompareDocumentPosition(otherNode);
        public void Normalize() => WrappedObject.Normalize();
        public bool Contains(INode otherNode) => WrappedObject.Contains(otherNode);
        public bool IsDefaultNamespace(string namespaceUri) => WrappedObject.IsDefaultNamespace(namespaceUri);
        public string LookupNamespaceUri(string prefix) => WrappedObject.LookupNamespaceUri(prefix);
        public string LookupPrefix(string namespaceUri) => WrappedObject.LookupPrefix(namespaceUri);
        public INode AppendChild(INode child) => WrappedObject.AppendChild(child);
        public INode InsertBefore(INode newElement, INode referenceElement) => WrappedObject.InsertBefore(newElement, referenceElement);
        public INode RemoveChild(INode child) => WrappedObject.RemoveChild(child);
        public INode ReplaceChild(INode newChild, INode oldChild) => WrappedObject.ReplaceChild(newChild, oldChild);
        public void AddEventListener(string type, DomEventHandler? callback = null, bool capture = false) => WrappedObject.AddEventListener(type, callback, capture);
        public void RemoveEventListener(string type, DomEventHandler? callback = null, bool capture = false) => WrappedObject.RemoveEventListener(type, callback, capture);
        public void InvokeEventListener(Event ev) => WrappedObject.InvokeEventListener(ev);
        public bool Dispatch(Event ev) => WrappedObject.Dispatch(ev);
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => WrappedObject.ToHtml(writer, formatter);
        public void Append(params INode[] nodes) => WrappedObject.Append(nodes);
        public void Prepend(params INode[] nodes) => WrappedObject.Prepend(nodes);
        public IElement QuerySelector(string selectors) => WrappedObject.QuerySelector(selectors);
        public IHtmlCollection<IElement> QuerySelectorAll(string selectors) => WrappedObject.QuerySelectorAll(selectors);
        public void Before(params INode[] nodes) => WrappedObject.Before(nodes);
        public void After(params INode[] nodes) => WrappedObject.After(nodes);
        public void Replace(params INode[] nodes) => WrappedObject.Replace(nodes);
        public void Remove() => WrappedObject.Remove();
    }
}
