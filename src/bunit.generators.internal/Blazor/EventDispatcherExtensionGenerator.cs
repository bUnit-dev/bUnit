using System.Text;
using Microsoft.CodeAnalysis;

namespace Bunit.Blazor;

[Generator]
public class EventDispatcherExtensionGenerator : IIncrementalGenerator
{
	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		context.RegisterPostInitializationOutput(GenerateEventDispatchExtensions);
	}

	private static void GenerateEventDispatchExtensions(IncrementalGeneratorPostInitializationContext context)
	{
		var sourceBuilder = new StringBuilder(4000);
		sourceBuilder.AppendLine("#nullable enable");
		sourceBuilder.AppendLine("using AngleSharp.Dom;");
		sourceBuilder.AppendLine("using Microsoft.AspNetCore.Components.Web;");
		sourceBuilder.AppendLine("using System.Threading.Tasks;");
		sourceBuilder.AppendLine();
		sourceBuilder.AppendLine("namespace Bunit;");
		sourceBuilder.AppendLine();

		GenerateExtensionsForEventArgsType(sourceBuilder, "ChangeEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "ClipboardEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "DragEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "FocusEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "KeyboardEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "MouseEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "PointerEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "ProgressEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "TouchEventArgs");
		GenerateExtensionsForEventArgsType(sourceBuilder, "WheelEventArgs");

		context.AddSource("EventDispatchExtensions.g.cs", sourceBuilder.ToString());
	}

	private static void GenerateExtensionsForEventArgsType(StringBuilder sourceBuilder, string eventArgsType)
	{
		var className = GetClassNameForEventArgsType(eventArgsType);
		sourceBuilder.AppendLine("/// <summary>");
		sourceBuilder.AppendLine($"/// Event dispatch helper extension methods for {eventArgsType}.");
		sourceBuilder.AppendLine("/// </summary>");
		sourceBuilder.AppendLine($"public static partial class {className}");
		sourceBuilder.AppendLine("{");

		if (eventArgsType == "MouseEventArgs")
		{
			GenerateMouseEventExtensions(sourceBuilder);
		}
		else if (eventArgsType == "KeyboardEventArgs")
		{
			GenerateKeyboardEventExtensions(sourceBuilder);
		}
		else if (eventArgsType == "ChangeEventArgs")
		{
			GenerateInputEventExtensions(sourceBuilder);
		}
		else
		{
			GenerateGenericEventExtension(sourceBuilder, eventArgsType);
		}

		sourceBuilder.AppendLine("}");
		sourceBuilder.AppendLine();
	}

	private static void GenerateMouseEventExtensions(StringBuilder sourceBuilder)
	{
		var mouseEvents = new[]
		{
			("MouseOver", "onmouseover"),
			("MouseOut", "onmouseout"),
			("MouseMove", "onmousemove"),
			("MouseDown", "onmousedown"),
			("MouseUp", "onmouseup"),
			("Click", "onclick"),
			("DoubleClick", "ondblclick"),
			("ContextMenu", "oncontextmenu")
		};

		foreach (var (methodName, eventName) in mouseEvents)
		{
			// Special treatment for Click and DoubleClick to set Detail property by default
			if (methodName == "Click")
			{
				GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, "MouseEventArgs", "new MouseEventArgs() { Detail = 1 }");
			}
			else if (methodName == "DoubleClick")
			{
				GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, "MouseEventArgs", "new MouseEventArgs() { Detail = 2 }");
			}
			else
			{
				GenerateAsyncEventMethod(sourceBuilder, methodName, eventName, "MouseEventArgs");
			}
		}
	}

	private static void GenerateKeyboardEventExtensions(StringBuilder sourceBuilder)
	{
		var keyboardEvents = new[]
		{
			("KeyDown", "onkeydown"),
			("KeyUp", "onkeyup"),
			("KeyPress", "onkeypress")
		};

		foreach (var (methodName, eventName) in keyboardEvents)
		{
			GenerateAsyncEventMethod(sourceBuilder, methodName, eventName, "KeyboardEventArgs");
		}
	}

	private static void GenerateGenericEventExtension(StringBuilder sourceBuilder, string eventArgsType)
	{
		var methodName = eventArgsType.Replace("EventArgs", "");
		var eventName = $"on{methodName.ToLowerInvariant()}";
		GenerateAsyncEventMethod(sourceBuilder, methodName, eventName, eventArgsType);
	}

	private static void GenerateAsyncEventMethod(StringBuilder sourceBuilder, string methodName, string eventName, string eventArgsType)
	{
		GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, eventArgsType, $"new {eventArgsType}()");
	}

	private static void GenerateAsyncEventMethodWithDefaultArgs(StringBuilder sourceBuilder, string methodName, string eventName, string eventArgsType, string defaultArgs)
	{
		sourceBuilder.AppendLine("\t/// <summary>");
		sourceBuilder.AppendLine($"\t/// Raises the <c>@{eventName}</c> event on <paramref name=\"element\"/>, passing the provided <paramref name=\"eventArgs\"/>");
		sourceBuilder.AppendLine($"\t/// to the event handler. If <paramref name=\"eventArgs\"/> is null, a new instance of {eventArgsType} will be created.");
		sourceBuilder.AppendLine("\t/// </summary>");
		sourceBuilder.AppendLine("\t/// <param name=\"element\">The element to raise the event on.</param>");
		sourceBuilder.AppendLine("\t/// <param name=\"eventArgs\">The event arguments to pass to the event handler.</param>");
		sourceBuilder.AppendLine($"\tpublic static void {methodName}(this IElement element, {eventArgsType}? eventArgs = null) => _ = {methodName}Async(element, eventArgs);");
		sourceBuilder.AppendLine();

		sourceBuilder.AppendLine("\t/// <summary>");
		sourceBuilder.AppendLine($"\t/// Raises the <c>@{eventName}</c> event on <paramref name=\"element\"/>, passing the provided <paramref name=\"eventArgs\"/>");
		sourceBuilder.AppendLine($"\t/// to the event handler. If <paramref name=\"eventArgs\"/> is null, a new instance of {eventArgsType} will be created.");
		sourceBuilder.AppendLine("\t/// </summary>");
		sourceBuilder.AppendLine("\t/// <param name=\"element\">The element to raise the event on.</param>");
		sourceBuilder.AppendLine("\t/// <param name=\"eventArgs\">The event arguments to pass to the event handler.</param>");
		sourceBuilder.AppendLine("\t/// <returns>A task that completes when the event handler is done.</returns>");
		sourceBuilder.AppendLine($"\tpublic static Task {methodName}Async(this IElement element, {eventArgsType}? eventArgs = null) => element.TriggerEventAsync(\"{eventName}\", eventArgs ?? {defaultArgs});");
		sourceBuilder.AppendLine();

		sourceBuilder.AppendLine("\t/// <summary>");
		sourceBuilder.AppendLine($"\t/// Raises the <c>@{eventName}</c> event on the element returned by <paramref name=\"elementTask\"/>, passing the provided <paramref name=\"eventArgs\"/>");
		sourceBuilder.AppendLine($"\t/// to the event handler. If <paramref name=\"eventArgs\"/> is null, a new instance of {eventArgsType} will be created.");
		sourceBuilder.AppendLine("\t/// </summary>");
		sourceBuilder.AppendLine("\t/// <param name=\"elementTask\">A task that returns the element to raise the element on.</param>");
		sourceBuilder.AppendLine("\t/// <param name=\"eventArgs\">The event arguments to pass to the event handler.</param>");
		sourceBuilder.AppendLine("\t/// <returns>A task that completes when the event handler is done.</returns>");
		sourceBuilder.AppendLine($"\tpublic static async Task {methodName}Async(this Task<IElement> elementTask, {eventArgsType}? eventArgs = null)");
		sourceBuilder.AppendLine("\t{");
		sourceBuilder.AppendLine("\t\tvar element = await elementTask;");
		sourceBuilder.AppendLine($"\t\tawait {methodName}Async(element, eventArgs);");
		sourceBuilder.AppendLine("\t}");
		sourceBuilder.AppendLine();
	}

	private static string GetClassNameForEventArgsType(string eventArgsType)
	{
		return eventArgsType switch
		{
			"MouseEventArgs" => "MouseEventDispatchExtensions",
			"KeyboardEventArgs" => "KeyboardEventDispatchExtensions",
			"ChangeEventArgs" => "InputEventDispatchExtensions",
			"WheelEventArgs" => "WheelEventDispatchExtensions",
			"FocusEventArgs" => "FocusEventDispatchExtensions",
			"DragEventArgs" => "DragEventDispatchExtensions",
			"ClipboardEventArgs" => "ClipboardEventDispatchExtensions",
			"TouchEventArgs" => "TouchEventDispatchExtensions",
			"PointerEventArgs" => "PointerEventDispatchExtensions",
			"ProgressEventArgs" => "ProgressEventDispatchExtensions",
			_ => $"{eventArgsType.Replace("EventArgs", "")}EventDispatchExtensions"
		};
	}

	private static void GenerateInputEventExtensions(StringBuilder sourceBuilder)
	{
		var inputEvents = new[]
		{
			("Change", "onchange"),
			("Input", "oninput")
		};

		foreach (var (methodName, eventName) in inputEvents)
		{
			GenerateAsyncEventMethod(sourceBuilder, methodName, eventName, "ChangeEventArgs");
		}
	}
}
