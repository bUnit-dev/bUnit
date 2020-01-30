using System;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Dom.Events;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IText"/> type.
    /// </summary>
    public partial class TextWrapper : Wrapper<IText>, IText, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="TextWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public TextWrapper(Func<IText?> getObject) : base(getObject) { }

        private static readonly int AssignedSlotKey = HashCode.Combine(nameof(AssignedSlot));
        /// <inheritdoc/>
        public IElement AssignedSlot { get => GetOrWrap(AssignedSlotKey, () => WrappedObject.AssignedSlot); }

        /// <inheritdoc/>
        public String BaseUri { get => WrappedObject.BaseUri; }

        /// <inheritdoc/>
        public Url BaseUrl { get => WrappedObject.BaseUrl; }

        private static readonly int ChildNodesKey = HashCode.Combine(nameof(ChildNodes));
        /// <inheritdoc/>
        public INodeList ChildNodes { get => GetOrWrap(ChildNodesKey, () => WrappedObject.ChildNodes); }

        /// <inheritdoc/>
        public String Data { get => WrappedObject.Data; set => WrappedObject.Data = value;}

        private static readonly int FirstChildKey = HashCode.Combine(nameof(FirstChild));
        /// <inheritdoc/>
        public INode FirstChild { get => GetOrWrap(FirstChildKey, () => WrappedObject.FirstChild); }

        /// <inheritdoc/>
        public NodeFlags Flags { get => WrappedObject.Flags; }

        /// <inheritdoc/>
        public Boolean HasChildNodes { get => WrappedObject.HasChildNodes; }

        private static readonly int LastChildKey = HashCode.Combine(nameof(LastChild));
        /// <inheritdoc/>
        public INode LastChild { get => GetOrWrap(LastChildKey, () => WrappedObject.LastChild); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

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

        private static readonly int OwnerKey = HashCode.Combine(nameof(Owner));
        /// <inheritdoc/>
        public IDocument Owner { get => GetOrWrap(OwnerKey, () => WrappedObject.Owner); }

        private static readonly int ParentKey = HashCode.Combine(nameof(Parent));
        /// <inheritdoc/>
        public INode Parent { get => GetOrWrap(ParentKey, () => WrappedObject.Parent); }

        private static readonly int ParentElementKey = HashCode.Combine(nameof(ParentElement));
        /// <inheritdoc/>
        public IElement ParentElement { get => GetOrWrap(ParentElementKey, () => WrappedObject.ParentElement); }

        private static readonly int PreviousElementSiblingKey = HashCode.Combine(nameof(PreviousElementSibling));
        /// <inheritdoc/>
        public IElement PreviousElementSibling { get => GetOrWrap(PreviousElementSiblingKey, () => WrappedObject.PreviousElementSibling); }

        private static readonly int PreviousSiblingKey = HashCode.Combine(nameof(PreviousSibling));
        /// <inheritdoc/>
        public INode PreviousSibling { get => GetOrWrap(PreviousSiblingKey, () => WrappedObject.PreviousSibling); }

        /// <inheritdoc/>
        public String Text { get => WrappedObject.Text; }

        /// <inheritdoc/>
        public String TextContent { get => WrappedObject.TextContent; set => WrappedObject.TextContent = value;}

        /// <inheritdoc/>
        public void AddEventListener(String type, DomEventHandler callback, Boolean capture)
            => WrappedObject.AddEventListener(type, callback, capture);

        /// <inheritdoc/>
        public void After(INode[] nodes)
            => WrappedObject.After(nodes);

        /// <inheritdoc/>
        public void Append(String value)
            => WrappedObject.Append(value);

        /// <inheritdoc/>
        public INode AppendChild(INode child)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(AppendChild), child), () => WrappedObject.AppendChild(child));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public void Before(INode[] nodes)
            => WrappedObject.Before(nodes);

        /// <inheritdoc/>
        public INode Clone(Boolean deep)
            => GetOrWrap(HashCode.Combine(nameof(Clone), deep), () => WrappedObject.Clone(deep));

        /// <inheritdoc/>
        public DocumentPositions CompareDocumentPosition(INode otherNode)
            => WrappedObject.CompareDocumentPosition(otherNode);

        /// <inheritdoc/>
        public Boolean Contains(INode otherNode)
            => WrappedObject.Contains(otherNode);

        /// <inheritdoc/>
        public void Delete(Int32 offset, Int32 count)
            => WrappedObject.Delete(offset, count);

        /// <inheritdoc/>
        public Boolean Dispatch(Event ev)
            => WrappedObject.Dispatch(ev);

        /// <inheritdoc/>
        public Boolean Equals(INode otherNode)
            => WrappedObject.Equals(otherNode);

        /// <inheritdoc/>
        public void Insert(Int32 offset, String value)
            => WrappedObject.Insert(offset, value);

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
        public void Normalize()
            => WrappedObject.Normalize();

        /// <inheritdoc/>
        public void Remove()
            => WrappedObject.Remove();

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
        public void Replace(Int32 offset, Int32 count, String value)
            => WrappedObject.Replace(offset, count, value);

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
        public IText Split(Int32 offset)
            => GetOrWrap(HashCode.Combine(nameof(Split), offset), () => WrappedObject.Split(offset));

        /// <inheritdoc/>
        public String Substring(Int32 offset, Int32 count)
            => WrappedObject.Substring(offset, count);

        /// <inheritdoc/>
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
            => WrappedObject.ToHtml(writer, formatter);
    }
}
