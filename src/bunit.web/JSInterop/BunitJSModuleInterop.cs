namespace Bunit;

/// <summary>
/// Represents a bUnit JSInterop module.
/// </summary>
public sealed class BunitJSModuleInterop : BunitJSInterop
{
	private readonly BunitJSInterop parent;
	private JSRuntimeMode? handlerMode;


	/// <summary>
	/// Gets or sets whether this <see cref="BunitJSInterop"/>
	/// is running in <see cref="JSRuntimeMode.Loose"/> or <see cref="JSRuntimeMode.Strict"/>.
	/// </summary>
	/// <remarks>
	/// When this is not set explicitly, the mode from <see cref="BunitJSInterop.Mode"/> is used.
	/// As soon as this is set, the mode will no longer be changed when the <see cref="BunitJSInterop.Mode"/>
	/// changes.
	/// </remarks>
	[SuppressMessage("Critical Bug", "S4275:Getters and setters should access the expected fields", Justification = "Analyzer bug. The property does correctly refer to the correct field(s).")]
	public override JSRuntimeMode Mode
	{
		get => handlerMode ?? parent.Mode;
		set => handlerMode = value;
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="BunitJSModuleInterop"/> class.
	/// </summary>
	/// <param name="parent">The parent <see cref="BunitJSInterop"/>.</param>
	public BunitJSModuleInterop(BunitJSInterop parent)
		: base()
	{
		this.parent = parent;
		handlerMode = null;
	}

	/// <inheritdoc/>
	internal override void RegisterInvocation(JSRuntimeInvocation invocation)
	{
		Invocations.RegisterInvocation(invocation);
		parent.RegisterInvocation(invocation);
	}
}
