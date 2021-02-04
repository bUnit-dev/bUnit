using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using AngleSharp.Dom;
using Bunit.TestAssets.SampleComponents;
using Shouldly;

namespace Bunit
{
	public abstract class EventDispatchExtensionsTest<TEventArgs> : TestContext
		where TEventArgs : EventArgs, new()
	{
		protected static readonly Type EventArgsType = typeof(TEventArgs);

		protected abstract string ElementName { get; }

		protected TriggerEventSpy<EventArgs> CreateTriggerSpy(string element, string eventName)
		{
			return new TriggerEventSpy<EventArgs>(p => RenderComponent<TriggerTester<EventArgs>>(p), element, eventName);
		}

		protected void VerifyEventRaisesCorrectly(MethodInfo helper, TEventArgs expected, params (string MethodName, string EventName)[] methodNameEventMap)
		{
			if (helper is null)
				throw new ArgumentNullException(nameof(helper));

			var eventName = methodNameEventMap.SingleOrDefault(x => x.MethodName.Equals(helper.Name, StringComparison.Ordinal)).EventName
				?? GetEventNameFromMethod(helper);

			var spy = CreateTriggerSpy(ElementName, eventName);
			var evtArg = new TEventArgs();

			if (helper.GetParameters().Any(p => p.ParameterType == EventArgsType))
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
				var args = EventArgsType.GetProperties().ToDictionary(x => x.Name.ToUpperInvariant(), x => x.GetValue(expected, null), StringComparer.Ordinal);

				spy.Trigger(element =>
				{
					args.Add("ELEMENT", element);

					var helperArgs = helper.GetParameters().Select(x => args[x.Name!.ToUpperInvariant()]).ToArray();

					helper.Invoke(null, helperArgs);
				});

				spy.RaisedEvent.ShouldBeOfType<TEventArgs>().ShouldBeEquivalentTo(expected);
			}
		}

		[SuppressMessage("Globalization", "CA1308:Normalize strings to uppercase", Justification = "Events has to be in lowercase.")]
		private static string GetEventNameFromMethod(MethodInfo helper)
		{
			if (helper is null)
				throw new ArgumentNullException(nameof(helper));

			var nameLength = helper.Name.Length;

			if (helper.Name.EndsWith("Async", StringComparison.Ordinal))
			{
				nameLength -= 5;
			}

			var eventName = $"on{helper.Name.Substring(0, nameLength).ToLowerInvariant()}";

			return eventName;
		}

		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
		public static IEnumerable<MethodInfo[]> GetEventHelperMethods(Type helperClassType)
		{
			if (helperClassType is null)
				throw new ArgumentNullException(nameof(helperClassType));

			return helperClassType.GetMethods()
				.Where(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(IElement))
				.Select(x => new[] { x })
				.ToArray();
		}

		[SuppressMessage("Design", "CA1000:Do not declare static members on generic types")]
		public static IEnumerable<MethodInfo[]> GetEventHelperMethods(Type helperClassType, Func<MethodInfo, bool> customFilter)
		{
			if (helperClassType is null)
				throw new ArgumentNullException(nameof(helperClassType));

			return helperClassType.GetMethods()
				.Where(x => x.GetParameters().FirstOrDefault()?.ParameterType == typeof(IElement) && customFilter(x))
				.Select(x => new[] { x })
				.ToArray();
		}
	}
}
