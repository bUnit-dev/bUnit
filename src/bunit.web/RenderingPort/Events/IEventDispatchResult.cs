using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public interface IEventDispatchResult
{
	int InvokedHandlerCount { get; }

	bool DefaultPrevented { get; }

    Task DispatchCompleted { get; }

	// IDEA: could also collect a list of nodes who had event handlers triggered.
}
