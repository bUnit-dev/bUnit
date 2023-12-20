//HintName: HtmlDivElementWrapper.g.cs
#nullable enable
using System.Runtime.CompilerServices;

namespace Bunit.Web.AngleSharp;

/// <inheritdoc/>
[System.Diagnostics.DebuggerDisplay("{OuterHtml,nq}")]
[System.Diagnostics.DebuggerNonUserCode]
[System.CodeDom.Compiler.GeneratedCodeAttribute("Bunit.Web.AngleSharp", "1.0.0.0")]
internal sealed class HtmlDivElementWrapper : WrapperBase<global::AngleSharp.Html.Dom.IHtmlDivElement>, global::AngleSharp.Html.Dom.IHtmlDivElement
{

	internal HtmlDivElementWrapper(global::AngleSharp.Html.Dom.IHtmlDivElement element, Bunit.Web.AngleSharp.IElementWrapperFactory elementFactory) : base(element, elementFactory) { }

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Aborted
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Aborted += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Aborted -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Blurred
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Blurred += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Blurred -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Cancelled
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Cancelled += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Cancelled -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler CanPlay
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CanPlay += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CanPlay -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler CanPlayThrough
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CanPlayThrough += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CanPlayThrough -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Changed
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Changed += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Changed -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Clicked
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Clicked += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Clicked -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler CueChanged
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CueChanged += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).CueChanged -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DoubleClick
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DoubleClick += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DoubleClick -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Drag
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Drag += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Drag -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragEnd
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragEnd += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragEnd -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragEnter
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragEnter += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragEnter -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragExit
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragExit += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragExit -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragLeave
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragLeave += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragLeave -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragOver
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragOver += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragOver -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DragStart
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragStart += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DragStart -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Dropped
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Dropped += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Dropped -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler DurationChanged
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DurationChanged += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).DurationChanged -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Emptied
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Emptied += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Emptied -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Ended
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Ended += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Ended -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Error
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Error += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Error -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Focused
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Focused += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Focused -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Input
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Input += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Input -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Invalid
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Invalid += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Invalid -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler KeyDown
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyDown += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyDown -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler KeyPress
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyPress += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyPress -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler KeyUp
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyUp += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).KeyUp -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Loaded
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Loaded += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Loaded -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler LoadedData
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).LoadedData += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).LoadedData -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler LoadedMetadata
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).LoadedMetadata += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).LoadedMetadata -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Loading
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Loading += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Loading -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseDown
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseDown += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseDown -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseEnter
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseEnter += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseEnter -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseLeave
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseLeave += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseLeave -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseMove
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseMove += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseMove -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseOut
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseOut += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseOut -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseOver
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseOver += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseOver -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseUp
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseUp += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseUp -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler MouseWheel
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseWheel += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).MouseWheel -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Paused
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Paused += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Paused -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Played
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Played += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Played -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Playing
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Playing += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Playing -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Progress
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Progress += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Progress -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler RateChanged
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).RateChanged += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).RateChanged -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Resetted
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Resetted += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Resetted -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Resized
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Resized += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Resized -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Scrolled
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Scrolled += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Scrolled -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Seeked
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Seeked += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Seeked -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Seeking
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Seeking += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Seeking -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Selected
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Selected += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Selected -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Shown
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Shown += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Shown -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Stalled
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Stalled += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Stalled -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Submitted
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Submitted += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Submitted -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Suspended
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Suspended += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Suspended -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler TimeUpdated
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).TimeUpdated += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).TimeUpdated -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Toggled
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Toggled += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Toggled -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler VolumeChanged
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).VolumeChanged += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).VolumeChanged -= value;
	}

	/// <inheritdoc/>
	public event global::AngleSharp.Dom.DomEventHandler Waiting
	{
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		add => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Waiting += value;
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		[System.Diagnostics.DebuggerHidden]
		remove => Unsafe.As<global::AngleSharp.Dom.Events.IGlobalEventHandlers>(WrappedElement).Waiting -= value;
	}

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
	public void DoBlur()
		=> WrappedElement.DoBlur();

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DoClick()
		=> WrappedElement.DoClick();

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DoFocus()
		=> WrappedElement.DoFocus();

	/// <inheritdoc/>
	[System.Diagnostics.DebuggerHidden]
	[System.Diagnostics.DebuggerStepThrough]
	[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
	public void DoSpellCheck()
		=> WrappedElement.DoSpellCheck();

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
	public string? AccessKey
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.AccessKey;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.AccessKey = value;
	}

	/// <inheritdoc/>
	public string? AccessKeyLabel
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.AccessKeyLabel;
	}

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
	public string? ContentEditable
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ContentEditable;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.ContentEditable = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Html.Dom.IHtmlMenuElement? ContextMenu
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.ContextMenu;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.ContextMenu = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.IStringMap Dataset
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Dataset;
	}

	/// <inheritdoc/>
	public string? Direction
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Direction;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.Direction = value;
	}

	/// <inheritdoc/>
	public global::AngleSharp.Dom.ISettableTokenList DropZone
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.DropZone;
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
	public bool IsContentEditable
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsContentEditable;
	}

	/// <inheritdoc/>
	public bool IsDraggable
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsDraggable;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.IsDraggable = value;
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
	public bool IsHidden
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsHidden;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.IsHidden = value;
	}

	/// <inheritdoc/>
	public bool IsSpellChecked
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsSpellChecked;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.IsSpellChecked = value;
	}

	/// <inheritdoc/>
	public bool IsTranslated
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.IsTranslated;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.IsTranslated = value;
	}

	/// <inheritdoc/>
	public string? Language
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Language;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.Language = value;
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
	public int TabIndex
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.TabIndex;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.TabIndex = value;
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

	/// <inheritdoc/>
	public string? Title
	{
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		get => WrappedElement.Title;
		[System.Diagnostics.DebuggerHidden]
		[System.Diagnostics.DebuggerStepThrough]
		[System.Runtime.CompilerServices.MethodImpl(MethodImplOptions.AggressiveInlining)]
		set => WrappedElement.Title = value;
	}
}
#nullable restore
