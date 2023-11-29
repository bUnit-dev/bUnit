//HintName: ElementWrapper.g.cs
#nullable enable
using System.Runtime.CompilerServices;

namespace Bunit.Web.AngleSharp;

/// <inheritdoc/>
[System.Diagnostics.DebuggerDisplay("{OuterHtml,nq}")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Bunit.Web.AngleSharp", "1.0.0.0")]
internal sealed class ElementWrapper : WrapperBase<global::AngleSharp.Dom.IElement>, global::AngleSharp.Dom.IElement
{

	internal ElementWrapper(global::AngleSharp.Dom.IElement element, Bunit.Web.AngleSharp.IElementWrapperFactory elementFactory) : base(element, elementFactory) { }

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void AddEventListener(string type, global::AngleSharp.Dom.DomEventHandler? callback = null, bool capture = false)
		=> WrappedElement.AddEventListener(type, callback, capture);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void After(params global::AngleSharp.Dom.INode[] nodes)
		=> WrappedElement.After(nodes);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Append(params global::AngleSharp.Dom.INode[] nodes)
		=> WrappedElement.Append(nodes);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.INode AppendChild(global::AngleSharp.Dom.INode child)
		=> WrappedElement.AppendChild(child);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IShadowRoot AttachShadow(global::AngleSharp.Dom.ShadowRootMode mode = global::AngleSharp.Dom.ShadowRootMode.Open)
		=> WrappedElement.AttachShadow(mode);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Before(params global::AngleSharp.Dom.INode[] nodes)
		=> WrappedElement.Before(nodes);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.INode Clone(bool deep = true)
		=> WrappedElement.Clone(deep);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IElement? Closest(string selectors)
		=> WrappedElement.Closest(selectors);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.DocumentPositions CompareDocumentPosition(global::AngleSharp.Dom.INode otherNode)
		=> WrappedElement.CompareDocumentPosition(otherNode);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Contains(global::AngleSharp.Dom.INode otherNode)
		=> WrappedElement.Contains(otherNode);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Dispatch(global::AngleSharp.Dom.Events.Event ev)
		=> WrappedElement.Dispatch(ev);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Equals(global::AngleSharp.Dom.INode otherNode)
		=> WrappedElement.Equals(otherNode);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string? GetAttribute(string name)
		=> WrappedElement.GetAttribute(name);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string? GetAttribute(string? namespaceUri, string localName)
		=> WrappedElement.GetAttribute(namespaceUri, localName);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IHtmlCollection<global::AngleSharp.Dom.IElement> GetElementsByClassName(string classNames)
		=> WrappedElement.GetElementsByClassName(classNames);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IHtmlCollection<global::AngleSharp.Dom.IElement> GetElementsByTagName(string tagName)
		=> WrappedElement.GetElementsByTagName(tagName);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IHtmlCollection<global::AngleSharp.Dom.IElement> GetElementsByTagNameNS(string? namespaceUri, string tagName)
		=> WrappedElement.GetElementsByTagNameNS(namespaceUri, tagName);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasAttribute(string name)
		=> WrappedElement.HasAttribute(name);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool HasAttribute(string? namespaceUri, string localName)
		=> WrappedElement.HasAttribute(namespaceUri, localName);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Insert(global::AngleSharp.Dom.AdjacentPosition position, string html)
		=> WrappedElement.Insert(position, html);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.INode InsertBefore(global::AngleSharp.Dom.INode newElement, global::AngleSharp.Dom.INode? referenceElement)
		=> WrappedElement.InsertBefore(newElement, referenceElement);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void InvokeEventListener(global::AngleSharp.Dom.Events.Event ev)
		=> WrappedElement.InvokeEventListener(ev);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool IsDefaultNamespace(string namespaceUri)
		=> WrappedElement.IsDefaultNamespace(namespaceUri);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string? LookupNamespaceUri(string prefix)
		=> WrappedElement.LookupNamespaceUri(prefix);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public string? LookupPrefix(string? namespaceUri)
		=> WrappedElement.LookupPrefix(namespaceUri);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool Matches(string selectors)
		=> WrappedElement.Matches(selectors);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Normalize()
		=> WrappedElement.Normalize();

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Prepend(params global::AngleSharp.Dom.INode[] nodes)
		=> WrappedElement.Prepend(nodes);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IElement? QuerySelector(string selectors)
		=> WrappedElement.QuerySelector(selectors);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.IHtmlCollection<global::AngleSharp.Dom.IElement> QuerySelectorAll(string selectors)
		=> WrappedElement.QuerySelectorAll(selectors);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Remove()
		=> WrappedElement.Remove();

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool RemoveAttribute(string name)
		=> WrappedElement.RemoveAttribute(name);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public bool RemoveAttribute(string? namespaceUri, string localName)
		=> WrappedElement.RemoveAttribute(namespaceUri, localName);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.INode RemoveChild(global::AngleSharp.Dom.INode child)
		=> WrappedElement.RemoveChild(child);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void RemoveEventListener(string type, global::AngleSharp.Dom.DomEventHandler? callback = null, bool capture = false)
		=> WrappedElement.RemoveEventListener(type, callback, capture);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void Replace(params global::AngleSharp.Dom.INode[] nodes)
		=> WrappedElement.Replace(nodes);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public global::AngleSharp.Dom.INode ReplaceChild(global::AngleSharp.Dom.INode newChild, global::AngleSharp.Dom.INode oldChild)
		=> WrappedElement.ReplaceChild(newChild, oldChild);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetAttribute(string name, string? value)
		=> WrappedElement.SetAttribute(name, value);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void SetAttribute(string? namespaceUri, string name, string? value)
		=> WrappedElement.SetAttribute(namespaceUri, name, value);

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void ToHtml(global::System.IO.TextWriter writer, global::AngleSharp.IMarkupFormatter formatter)
		=> WrappedElement.ToHtml(writer, formatter);

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? AssignedSlot
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.AssignedSlot;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INamedNodeMap Attributes
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Attributes;
	}

	/// <inheritdoc/>
	public string BaseUri
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.BaseUri;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.Url? BaseUrl
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.BaseUrl;
	}

	/// <inheritdoc/>
	public int ChildElementCount
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ChildElementCount;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INodeList ChildNodes
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ChildNodes;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IHtmlCollection<global::AngleSharp.Dom.IElement> Children
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Children;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.ITokenList ClassList
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ClassList;
	}

	/// <inheritdoc/>
	public string? ClassName
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ClassName;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.ClassName = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INode? FirstChild
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.FirstChild;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? FirstElementChild
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.FirstElementChild;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.NodeFlags Flags
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Flags;
	}

	/// <inheritdoc/>
	public string? GivenNamespaceUri
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.GivenNamespaceUri;
	}

	/// <inheritdoc/>
	public bool HasChildNodes
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.HasChildNodes;
	}

	/// <inheritdoc/>
	public string? Id
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Id;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.Id = value;
	}

	/// <inheritdoc/>
	public string InnerHtml
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.InnerHtml;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.InnerHtml = value;
	}

	/// <inheritdoc/>
	public bool IsFocused
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsFocused;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INode? LastChild
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.LastChild;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? LastElementChild
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.LastElementChild;
	}

	/// <inheritdoc/>
	public string LocalName
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.LocalName;
	}

	/// <inheritdoc/>
	public string? NamespaceUri
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NamespaceUri;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? NextElementSibling
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NextElementSibling;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INode? NextSibling
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NextSibling;
	}

	/// <inheritdoc/>
	public string NodeName
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NodeName;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.NodeType NodeType
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NodeType;
	}

	/// <inheritdoc/>
	public string NodeValue
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.NodeValue;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.NodeValue = value;
	}

	/// <inheritdoc/>
	public string OuterHtml
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.OuterHtml;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.OuterHtml = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IDocument? Owner
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Owner;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INode? Parent
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Parent;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? ParentElement
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ParentElement;
	}

	/// <inheritdoc/>
	public string? Prefix
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Prefix;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IElement? PreviousElementSibling
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.PreviousElementSibling;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.INode? PreviousSibling
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.PreviousSibling;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IShadowRoot? ShadowRoot
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ShadowRoot;
	}

	/// <inheritdoc/>
	public string? Slot
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Slot;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.Slot = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.ISourceReference? SourceReference
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.SourceReference;
	}

	/// <inheritdoc/>
	public string TagName
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.TagName;
	}

	/// <inheritdoc/>
	public string TextContent
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.TextContent;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.TextContent = value;
	}
}
#nullable restore
