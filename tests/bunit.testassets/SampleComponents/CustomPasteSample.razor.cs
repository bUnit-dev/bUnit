namespace Bunit.TestAssets.SampleComponents;

public partial class CustomPasteSample
{
	string message = string.Empty;

	void HandleCustomPaste(CustomPasteEventArgs eventArgs)
	{
		message = $"You pasted: {eventArgs.PastedData}";
	}
}

[EventHandler("oncustompaste", typeof(CustomPasteEventArgs), enableStopPropagation: true, enablePreventDefault: true)]
#pragma warning disable CA1724 // The type name EventHandlers conflicts in whole or in part with the namespace name 'Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.EventHandlers'.
public static class EventHandlers
#pragma warning restore
{
	// This static class doesn't need to contain any members. It's just a place where we can put
	// [EventHandler] attributes to configure event types on the Razor compiler. This affects the
	// compiler output as well as code completions in the editor.
}

public class CustomPasteEventArgs : EventArgs
{
	// Data for these properties will be supplied by custom JavaScript logic
	public DateTime EventTimestamp { get; set; }
	public string PastedData { get; set; }
}
