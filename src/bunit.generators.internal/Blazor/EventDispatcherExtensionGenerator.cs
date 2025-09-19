using System;
using System.Collections.Frozen;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;

namespace Bunit.Blazor;

[Generator]
public class EventDispatcherExtensionGenerator : IIncrementalGenerator
{
	private static readonly Dictionary<string, string> EventNameOverrides = new()
	{
		["ondblclick"] = "DoubleClick",
		["onmousewheel"] = "MouseWheel"
	};

	private static readonly FrozenSet<string> WordBoundaries =
	[
		"key", "mouse", "pointer", "touch", "drag", "drop", "focus", "blur",
		"click", "dbl", "context", "menu", "copy", "cut", "paste",
		"down", "up", "over", "out", "move", "enter", "leave", "start", "end",
		"cancel", "change", "input", "wheel", "got", "lost", "capture",
		"in", "before", "after", "load", "time", "abort", "progress", "error",
		"activate", "deactivate", "ended", "full", "screen", "data", "metadata",
		"lock", "ready", "state", "scroll", "toggle", "close", "seeking", "seeked",
		"loaded", "duration", "emptied", "stalled", "suspend", "volume", "waiting",
		"play", "playing", "pause", "press", "rate", "stop", "cue", "can", "through", "update"
	];

	public void Initialize(IncrementalGeneratorInitializationContext context)
	{
		var compilationProvider = context.CompilationProvider;

		context.RegisterSourceOutput(compilationProvider, GenerateEventDispatchExtensions);
	}

	private static void GenerateEventDispatchExtensions(SourceProductionContext context, Compilation compilation)
	{
		var eventHandlerAttributeSymbol = compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Components.EventHandlerAttribute");
		if (eventHandlerAttributeSymbol is null)
		{
			return;
		}

		var eventHandlersSymbol = compilation.GetTypeByMetadataName("Microsoft.AspNetCore.Components.Web.EventHandlers");
		if (eventHandlersSymbol is null)
		{
			return;
		}

		var eventMappings = new Dictionary<string, List<(string EventName, string MethodName)>>();

		foreach (var attribute in eventHandlersSymbol.GetAttributes())
		{
			if (!SymbolEqualityComparer.Default.Equals(attribute.AttributeClass, eventHandlerAttributeSymbol))
				continue;

			if (attribute.ConstructorArguments.Length < 2)
				continue;

			var eventName = attribute.ConstructorArguments[0].Value?.ToString();
			var eventArgsType = attribute.ConstructorArguments[1].Value as INamedTypeSymbol;

			if (string.IsNullOrEmpty(eventName) || eventArgsType == null)
				continue;

			var eventArgsTypeName = eventArgsType.Name;

			var methodName = GenerateMethodNameFromEventName(eventName);

			if (!eventMappings.ContainsKey(eventArgsTypeName))
			{
				eventMappings[eventArgsTypeName] = [];
			}

			eventMappings[eventArgsTypeName].Add((eventName, methodName));
		}

		if (eventMappings.Count == 0)
		{
			return;
		}

		var sourceBuilder = new StringBuilder(8000);
		sourceBuilder.AppendLine("#nullable enable");
		sourceBuilder.AppendLine("using AngleSharp.Dom;");
		sourceBuilder.AppendLine("using Microsoft.AspNetCore.Components.Web;");
		sourceBuilder.AppendLine("using System.Threading.Tasks;");
		sourceBuilder.AppendLine();
		sourceBuilder.AppendLine("namespace Bunit;");
		sourceBuilder.AppendLine();

		sourceBuilder.AppendLine("/// <summary>");
		sourceBuilder.AppendLine("/// Event dispatch helper extension methods.");
		sourceBuilder.AppendLine("/// </summary>");
		sourceBuilder.AppendLine("public static partial class EventHandlerDispatchExtensions");
		sourceBuilder.AppendLine("{");

		foreach (var kvp in eventMappings.OrderBy(x => x.Key))
		{
			GenerateExtensionsForEventArgsType(sourceBuilder, kvp.Key, kvp.Value);
		}

		sourceBuilder.AppendLine("}");

		context.AddSource("EventHandlerDispatchExtensions.g.cs", sourceBuilder.ToString());
	}

	private static string GenerateMethodNameFromEventName(string eventName)
	{
		if (EventNameOverrides.TryGetValue(eventName, out var overrideName))
		{
			return overrideName;
		}

		if (eventName.StartsWith("on"))
		{
			eventName = eventName[2..];
		}

		if (eventName.Length == 0)
		{
			return eventName;
		}

		var result = new StringBuilder();

		var i = 0;
		while (i < eventName.Length)
		{
			var foundWord = false;

			foreach (var word in WordBoundaries.OrderByDescending(w => w.Length))
			{
				var isWithinStringBounds = i + word.Length <= eventName.Length;
				var isWordMatch = isWithinStringBounds && eventName.AsSpan(i, word.Length).Equals(word.AsSpan(), StringComparison.OrdinalIgnoreCase);

				if (isWordMatch)
				{
					result.Append(char.ToUpper(word[0]));
					result.Append(word[1..].ToLower());
					i += word.Length;
					foundWord = true;
					break;
				}
			}

			if (!foundWord)
			{
				result.Append(i == 0 ? char.ToUpper(eventName[i]) : eventName[i]);
				i++;
			}
		}

		return result.ToString();
	}

	private static void GenerateExtensionsForEventArgsType(StringBuilder sourceBuilder, string eventArgsType, List<(string EventName, string MethodName)> events)
	{
		sourceBuilder.AppendLine($"\t// {eventArgsType} events");

		foreach (var (eventName, methodName) in events)
		{
			var qualifiedEventArgsType = eventArgsType == "ErrorEventArgs"
				? "Microsoft.AspNetCore.Components.Web.ErrorEventArgs"
				: eventArgsType;

			if (methodName == "Click")
			{
				GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, qualifiedEventArgsType, "new MouseEventArgs() { Detail = 1 }");
			}
			else if (methodName == "DoubleClick")
			{
				GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, qualifiedEventArgsType, "new MouseEventArgs() { Detail = 2 }");
			}
			else
			{
				GenerateAsyncEventMethodWithDefaultArgs(sourceBuilder, methodName, eventName, qualifiedEventArgsType, $"new {qualifiedEventArgsType}()");
			}
		}
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
}
