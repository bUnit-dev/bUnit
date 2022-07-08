using AngleSharp.Dom;

namespace Bunit.RenderingPort;

public static class EventDispatchExtensions
{
	public static IEventDispatchResult Click(this INode node, MouseEventArgs? eventArgs = null)
	{
		var @event = new BunitEvent(eventArgs ?? new(), "click", bubbles: true, cancelable: true);
		node.Dispatch(@event);
		ThrowIfNoEventHandlersInvoked(node, @event);
		return @event;
	}

	public static async Task<IEventDispatchResult> ClickAsync(this INode node, MouseEventArgs? eventArgs = null)
	{
		var @event = Click(node, eventArgs);
		await @event.DispatchCompleted;
		return @event;
	}

	public static IEventDispatchResult KeyPress(this INode node, KeyboardEventArgs eventArgs)
	{
		eventArgs.Type = "keypress";
		var @event = new BunitEvent(eventArgs, eventArgs.Type, bubbles: true, cancelable: true);
		node.Dispatch(@event);
		ThrowIfNoEventHandlersInvoked(node, @event);
		return @event;
	}

	public static async Task<IEventDispatchResult> KeyPressAsync(this INode node, KeyboardEventArgs eventArgs)
	{
		var @event = KeyPress(node, eventArgs);
		await @event.DispatchCompleted;
		return @event;
	}

	public static IEventDispatchResult Change<T>(this INode node, T value)
	{
		var @event = new BunitEvent(new ChangeEventArgs { Value = value }, "change", bubbles: true, cancelable: true);
		node.Dispatch(@event);
		ThrowIfNoEventHandlersInvoked(node, @event);
		return @event;
	}

	public static async Task<IEventDispatchResult> ChangeAsync<T>(this INode node, T value)
	{
		var @event = Change<T>(node, value);
		await @event.DispatchCompleted;
		return @event;
	}

	private static void ThrowIfNoEventHandlersInvoked(INode target, BunitEvent @event)
	{
		if (@event.InvokedHandlerCount == 0)
		{
			throw new MissingEventHandlerException(target, @event.Type);
		}
	}
}
