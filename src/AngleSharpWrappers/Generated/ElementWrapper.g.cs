using System;
using System.Diagnostics;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IElement"/> type.
    /// </summary>
    [DebuggerDisplay("{OuterHtml,nq}")]
    public sealed class ElementWrapper : Wrapper<IElement>, IElement
    {
        /// <summary>
        /// Creates an instance of the <see cref="ElementWrapper"/> type;
        /// </summary>
        public ElementWrapper(IElementFactory<IElement> elementFactory) : base(elementFactory) { }
/// <inheritdoc/>

        #region Properties and indexers
        [DebuggerHidden]
        public IElement? AssignedSlot { get => WrappedElement.AssignedSlot; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INamedNodeMap Attributes { get => WrappedElement.Attributes; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String BaseUri { get => WrappedElement.BaseUri; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public Url? BaseUrl { get => WrappedElement.BaseUrl; }
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
        public INode? FirstChild { get => WrappedElement.FirstChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? FirstElementChild { get => WrappedElement.FirstElementChild; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public NodeFlags Flags { get => WrappedElement.Flags; }
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
        public Boolean IsFocused { get => WrappedElement.IsFocused; }
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
        public String? NamespaceUri { get => WrappedElement.NamespaceUri; }
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
        public String? Prefix { get => WrappedElement.Prefix; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IElement? PreviousElementSibling { get => WrappedElement.PreviousElementSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public INode? PreviousSibling { get => WrappedElement.PreviousSibling; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public IShadowRoot? ShadowRoot { get => WrappedElement.ShadowRoot; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? Slot { get => WrappedElement.Slot; set => WrappedElement.Slot = value;}
        /// <inheritdoc/>
        [DebuggerHidden]
        public ISourceReference? SourceReference { get => WrappedElement.SourceReference; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String TagName { get => WrappedElement.TagName; }
        /// <inheritdoc/>
        [DebuggerHidden]
        public String TextContent { get => WrappedElement.TextContent; set => WrappedElement.TextContent = value;}
        /// <inheritdoc/>
        #endregion

        #region Methods
        [DebuggerHidden]
        public void AddEventListener(String type, DomEventHandler? callback, Boolean capture) => WrappedElement.AddEventListener(type, callback, capture);
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
        public IHtmlCollection<IElement> GetElementsByTagNameNS(String namespaceUri, String tagName) => WrappedElement.GetElementsByTagNameNS(namespaceUri, tagName);
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
        public String? LookupNamespaceUri(String prefix) => WrappedElement.LookupNamespaceUri(prefix);
        /// <inheritdoc/>
        [DebuggerHidden]
        public String? LookupPrefix(String namespaceUri) => WrappedElement.LookupPrefix(namespaceUri);
        /// <inheritdoc/>
        [DebuggerHidden]
        public Boolean Matches(String selectors) => WrappedElement.Matches(selectors);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void Normalize() => WrappedElement.Normalize();
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
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
		public Boolean RemoveAttribute(String namespaceUri, String localName) => WrappedElement.RemoveAttribute(namespaceUri, localName);
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
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
        public void SetAttribute(String name, String value) => WrappedElement.SetAttribute(name, value);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void SetAttribute(String namespaceUri, String name, String value) => WrappedElement.SetAttribute(namespaceUri, name, value);
        /// <inheritdoc/>
        [DebuggerHidden]
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter) => WrappedElement.ToHtml(writer, formatter);
        #endregion
    }
}
