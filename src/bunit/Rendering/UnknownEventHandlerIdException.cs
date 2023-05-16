namespace Bunit.Rendering;

/// <summary>
/// Represents an exception that is thrown when the Blazor <see cref="Renderer"/>
/// does not have any event handler with the specified a given ID.
/// </summary>
public sealed class UnknownEventHandlerIdException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UnknownEventHandlerIdException"/> class.
	/// </summary>
	public UnknownEventHandlerIdException(ulong eventHandlerId, EventFieldInfo fieldInfo, Exception innerException)
		: base(CreateMessage(eventHandlerId, fieldInfo), innerException)
	{
	}

	private static string CreateMessage(ulong eventHandlerId, EventFieldInfo fieldInfo)
		=> $"There is no event handler with ID '{eventHandlerId}' associated with the '{fieldInfo.FieldValue}' event " +
		   $"in the current render tree.{Environment.NewLine}{Environment.NewLine}" +
		   $"This can happen, for example, when using `cut.FindAll()`, and calling event trigger methods " +
			"on the found elements after a re-render of the render tree. The workaround is to use re-issue the `cut.FindAll()` call after " +
		   $"each render of a component, this ensures you have the latest version of the render tree and DOM tree available in your test code.{Environment.NewLine}{Environment.NewLine}" +
		   $"This can happen with code like this, `cut.Find(\"button\").Click()`, where the buttons event handler can be removed " +
		   $"between the time button is found with the Find method and the Click method is triggered. The workaround is to use " +
		   $"wrap the Find and Click method calls in InvokeAsync, i.e. `cut.InvokeAsync(() => cut.Find(\"button\").Click());`. " +
		   $"This ensures that there are no changes to the DOM between Find method and the Click method calls.";
}
