namespace Bunit.RenderingPort;

public interface IEventDispatchResult
{
	bool DefaultPrevented { get; }

    Task DispatchCompleted { get; }

	// IDEA: could also collect a list of nodes who had event handlers triggered.
}
