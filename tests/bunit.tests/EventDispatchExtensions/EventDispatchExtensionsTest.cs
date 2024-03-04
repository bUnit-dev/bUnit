using AngleSharp.Dom;

namespace Bunit;

public abstract class EventDispatchExtensionsTest<TEventArgs> : TestContext
	where TEventArgs : EventArgs, new()
{
	protected static readonly Type EventArgsType = typeof(TEventArgs);

	protected abstract string ElementName { get; }

	protected EventDispatchExtensionsTest(ITestOutputHelper outputHelper)
	{
		Services.AddXunitLogger(outputHelper);
	}

	protected TriggerEventSpy<EventArgs> CreateTriggerSpy(string element, string eventName)
		=> new(Render, element, eventName);

	// This is a separate overload for useful in non-reflection based testing
	protected TriggerEventSpy<T> CreateTriggerSpy<T>(string element, string eventName) where T : EventArgs, new()
		=> new(Render, element, eventName);

	protected void VerifyEventRaisesCorrectly(MethodInfo helper, TEventArgs expected, params (string MethodName, string EventName)[] methodNameEventMap)
	{
		ArgumentNullException.ThrowIfNull(helper);

		var eventName = methodNameEventMap.SingleOrDefault(x => x.MethodName.Equals(helper.Name, StringComparison.Ordinal)).EventName
			?? GetEventNameFromMethod(helper);

		var spy = CreateTriggerSpy(ElementName, eventName);
		var evtArg = new TEventArgs();

		if (Array.Exists(helper.GetParameters(), p => p.ParameterType == EventArgsType))
		{
			// Matches methods like: public static void Xxxx(this IElement element, TEventArgs args)
			spy.Trigger(element =>
			{
				helper.Invoke(null, new object[] { element, evtArg });
			});
			spy.RaisedEvent.ShouldBe(evtArg);
		}
		else if (helper.GetParameters().Length == 1)
		{
			// Matches methods like: public static void Xxxx(this IElement element)
			spy.Trigger(element =>
			{
				helper.Invoke(null, new object[] { element });
			});
			spy.RaisedEvent.ShouldBe(EventArgs.Empty);
		}
		else
		{
			// Matches methods like: public static void Xxxx(this IElement element, other params, goes here)
			var args = EventArgsType.GetProperties().ToDictionary(x => x.Name.ToUpperInvariant(), x => x.GetValue(expected, index: null), StringComparer.Ordinal);

			spy.Trigger(element =>
			{
				args.Add("ELEMENT", element);

				var helperArgs = helper.GetParameters().Select(x => args[x.Name!.ToUpperInvariant()]).ToArray();

				helper.Invoke(null, helperArgs);
			});

			spy.RaisedEvent.ShouldBeOfType<TEventArgs>().ShouldBeEquivalentTo(expected);
		}
	}

	[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Event names are used in lower case.")]
	private static string GetEventNameFromMethod(MethodInfo helper)
	{
		ArgumentNullException.ThrowIfNull(helper);

		var nameLength = helper.Name.Length;

		if (helper.Name.EndsWith("Async", StringComparison.Ordinal))
		{
			nameLength -= 5;
		}

		var eventName = $"on{helper.Name[..nameLength].ToLowerInvariant()}";

		return eventName;
	}

	[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Can safely be shared")]
	public static IEnumerable<object[]> GetEventHelperMethods(Type helperClassType)
	{
		ArgumentNullException.ThrowIfNull(helperClassType);

		return helperClassType.GetMethods()
			.Where(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(IElement))
			.Select(x => new[] { x })
			.ToArray();
	}

	[SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "Can safely be shared")]
	public static IEnumerable<object[]> GetEventHelperMethods(Type helperClassType, Func<MethodInfo, bool> customFilter)
	{
		ArgumentNullException.ThrowIfNull(helperClassType);

		return helperClassType.GetMethods()
			.Where(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(IElement) && customFilter(x))
			.Select(x => new[] { x })
			.ToArray();
	}
}
