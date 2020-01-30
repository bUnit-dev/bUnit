using System;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;
using AngleSharp.Svg.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="ISvgTitleElement"/> type.
    /// </summary>
    public partial class SvgTitleElementWrapper : Wrapper<ISvgTitleElement>, ISvgTitleElement, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="SvgTitleElementWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public SvgTitleElementWrapper(Func<ISvgTitleElement?> getObject) : base(getObject) { }

        private static readonly int AssignedSlotKey = HashCode.Combine(nameof(AssignedSlot));
        /// <inheritdoc/>
        public IElement AssignedSlot { get => GetOrWrap(AssignedSlotKey, () => WrappedObject.AssignedSlot); }

        private static readonly int AttributesKey = HashCode.Combine(nameof(Attributes));
        /// <inheritdoc/>
        public INamedNodeMap Attributes { get => GetOrWrap(AttributesKey, () => WrappedObject.Attributes); }

        /// <inheritdoc/>
        public String BaseUri { get => WrappedObject.BaseUri; }

        /// <inheritdoc/>
        public Url BaseUrl { get => WrappedObject.BaseUrl; }

        /// <inheritdoc/>
        public Int32 ChildElementCount { get => WrappedObject.ChildElementCount; }

        private static readonly int ChildNodesKey = HashCode.Combine(nameof(ChildNodes));
        /// <inheritdoc/>
        public INodeList ChildNodes { get => GetOrWrap(ChildNodesKey, () => WrappedObject.ChildNodes); }

        private static readonly int ChildrenKey = HashCode.Combine(nameof(Children));
        /// <inheritdoc/>
        public IHtmlCollection<IElement> Children { get => GetOrWrap(ChildrenKey, () => WrappedObject.Children); }

        private static readonly int ClassListKey = HashCode.Combine(nameof(ClassList));
        /// <inheritdoc/>
        public ITokenList ClassList { get => GetOrWrap(ClassListKey, () => WrappedObject.ClassList); }

        /// <inheritdoc/>
        public String ClassName { get => WrappedObject.ClassName; set => WrappedObject.ClassName = value;}

        private static readonly int FirstChildKey = HashCode.Combine(nameof(FirstChild));
        /// <inheritdoc/>
        public INode FirstChild { get => GetOrWrap(FirstChildKey, () => WrappedObject.FirstChild); }

        private static readonly int FirstElementChildKey = HashCode.Combine(nameof(FirstElementChild));
        /// <inheritdoc/>
        public IElement FirstElementChild { get => GetOrWrap(FirstElementChildKey, () => WrappedObject.FirstElementChild); }

        /// <inheritdoc/>
        public NodeFlags Flags { get => WrappedObject.Flags; }

        /// <inheritdoc/>
        public Boolean HasChildNodes { get => WrappedObject.HasChildNodes; }

        /// <inheritdoc/>
        public String Id { get => WrappedObject.Id; set => WrappedObject.Id = value;}

        /// <inheritdoc/>
        public String InnerHtml { get => WrappedObject.InnerHtml; set => WrappedObject.InnerHtml = value;}

        /// <inheritdoc/>
        public Boolean IsFocused { get => WrappedObject.IsFocused; }

        private static readonly int LastChildKey = HashCode.Combine(nameof(LastChild));
        /// <inheritdoc/>
        public INode LastChild { get => GetOrWrap(LastChildKey, () => WrappedObject.LastChild); }

        private static readonly int LastElementChildKey = HashCode.Combine(nameof(LastElementChild));
        /// <inheritdoc/>
        public IElement LastElementChild { get => GetOrWrap(LastElementChildKey, () => WrappedObject.LastElementChild); }

        /// <inheritdoc/>
        public String LocalName { get => WrappedObject.LocalName; }

        /// <inheritdoc/>
        public String NamespaceUri { get => WrappedObject.NamespaceUri; }

        private static readonly int NextElementSiblingKey = HashCode.Combine(nameof(NextElementSibling));
        /// <inheritdoc/>
        public IElement NextElementSibling { get => GetOrWrap(NextElementSiblingKey, () => WrappedObject.NextElementSibling); }

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
        public String OuterHtml { get => WrappedObject.OuterHtml; set => WrappedObject.OuterHtml = value;}

        private static readonly int OwnerKey = HashCode.Combine(nameof(Owner));
        /// <inheritdoc/>
        public IDocument Owner { get => GetOrWrap(OwnerKey, () => WrappedObject.Owner); }

        private static readonly int ParentKey = HashCode.Combine(nameof(Parent));
        /// <inheritdoc/>
        public INode Parent { get => GetOrWrap(ParentKey, () => WrappedObject.Parent); }

        private static readonly int ParentElementKey = HashCode.Combine(nameof(ParentElement));
        /// <inheritdoc/>
        public IElement ParentElement { get => GetOrWrap(ParentElementKey, () => WrappedObject.ParentElement); }

        /// <inheritdoc/>
        public String Prefix { get => WrappedObject.Prefix; }

        private static readonly int PreviousElementSiblingKey = HashCode.Combine(nameof(PreviousElementSibling));
        /// <inheritdoc/>
        public IElement PreviousElementSibling { get => GetOrWrap(PreviousElementSiblingKey, () => WrappedObject.PreviousElementSibling); }

        private static readonly int PreviousSiblingKey = HashCode.Combine(nameof(PreviousSibling));
        /// <inheritdoc/>
        public INode PreviousSibling { get => GetOrWrap(PreviousSiblingKey, () => WrappedObject.PreviousSibling); }

        private static readonly int ShadowRootKey = HashCode.Combine(nameof(ShadowRoot));
        /// <inheritdoc/>
        public IShadowRoot ShadowRoot { get => GetOrWrap(ShadowRootKey, () => WrappedObject.ShadowRoot); }

        /// <inheritdoc/>
        public String Slot { get => WrappedObject.Slot; set => WrappedObject.Slot = value;}

        /// <inheritdoc/>
        public ISourceReference SourceReference { get => WrappedObject.SourceReference; }

        /// <inheritdoc/>
        public String TagName { get => WrappedObject.TagName; }

        /// <inheritdoc/>
        public String TextContent { get => WrappedObject.TextContent; set => WrappedObject.TextContent = value;}

        /// <inheritdoc/>
        public void AddEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.AddEventListener(type, callback, capture);

        /// <inheritdoc/>
        public void After(INode[] nodes)
            => WrappedObject.After(nodes);

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
        public IShadowRoot AttachShadow(ShadowRootMode mode)
            => GetOrWrap(HashCode.Combine(nameof(AttachShadow), mode), () => WrappedObject.AttachShadow(mode));

        /// <inheritdoc/>
        public void Before(INode[] nodes)
            => WrappedObject.Before(nodes);

        /// <inheritdoc/>
        public INode Clone(Boolean deep)
            => GetOrWrap(HashCode.Combine(nameof(Clone), deep), () => WrappedObject.Clone(deep));

        /// <inheritdoc/>
        public IElement Closest(String selectors)
            => GetOrWrap(HashCode.Combine(nameof(Closest), selectors), () => WrappedObject.Closest(selectors));

        /// <inheritdoc/>
        public DocumentPositions CompareDocumentPosition(INode otherNode)
            => WrappedObject.CompareDocumentPosition(otherNode);

        /// <inheritdoc/>
        public Boolean Contains(INode otherNode)
            => WrappedObject.Contains(otherNode);

        /// <inheritdoc/>
        public Boolean Dispatch(Event ev)
            => WrappedObject.Dispatch(ev);

        /// <inheritdoc/>
        public Boolean Equals(INode otherNode)
            => WrappedObject.Equals(otherNode);

        /// <inheritdoc/>
        public String GetAttribute(String name)
            => WrappedObject.GetAttribute(name);

        /// <inheritdoc/>
        public String GetAttribute(String namespaceUri, String localName)
            => WrappedObject.GetAttribute(namespaceUri, localName);

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByClassName(String classNames)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByClassName), classNames), () => WrappedObject.GetElementsByClassName(classNames));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByTagName(String tagName)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByTagName), tagName), () => WrappedObject.GetElementsByTagName(tagName));

        /// <inheritdoc/>
        public IHtmlCollection<IElement> GetElementsByTagNameNS(String namespaceUri, String tagName)
            => GetOrWrap<IElement>(HashCode.Combine(nameof(GetElementsByTagNameNS), namespaceUri, tagName), () => WrappedObject.GetElementsByTagNameNS(namespaceUri, tagName));

        /// <inheritdoc/>
        public Boolean HasAttribute(String name)
            => WrappedObject.HasAttribute(name);

        /// <inheritdoc/>
        public Boolean HasAttribute(String namespaceUri, String localName)
            => WrappedObject.HasAttribute(namespaceUri, localName);

        /// <inheritdoc/>
        public void Insert(AdjacentPosition position, String html)
            => WrappedObject.Insert(position, html);

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
        public Boolean IsDefaultNamespace(String namespaceUri)
            => WrappedObject.IsDefaultNamespace(namespaceUri);

        /// <inheritdoc/>
        public String LookupNamespaceUri(String prefix)
            => WrappedObject.LookupNamespaceUri(prefix);

        /// <inheritdoc/>
        public String LookupPrefix(String namespaceUri)
            => WrappedObject.LookupPrefix(namespaceUri);

        /// <inheritdoc/>
        public Boolean Matches(String selectors)
            => WrappedObject.Matches(selectors);

        /// <inheritdoc/>
        public void Normalize()
            => WrappedObject.Normalize();

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
        public void Remove()
            => WrappedObject.Remove();

        /// <inheritdoc/>
        public Boolean RemoveAttribute(String name)
            => WrappedObject.RemoveAttribute(name);

        /// <inheritdoc/>
        public Boolean RemoveAttribute(String namespaceUri, String localName)
            => WrappedObject.RemoveAttribute(namespaceUri, localName);

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
        public void Replace(INode[] nodes)
            => WrappedObject.Replace(nodes);

        /// <inheritdoc/>
        public INode ReplaceChild(INode newChild, INode oldChild)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(ReplaceChild), newChild, oldChild), () => WrappedObject.ReplaceChild(newChild, oldChild));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void SetAttribute(String name, String value)
            => WrappedObject.SetAttribute(name, value);

        /// <inheritdoc/>
        public void SetAttribute(String namespaceUri, String name, String value)
            => WrappedObject.SetAttribute(namespaceUri, name, value);

        /// <inheritdoc/>
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
            => WrappedObject.ToHtml(writer, formatter);
    }
}
