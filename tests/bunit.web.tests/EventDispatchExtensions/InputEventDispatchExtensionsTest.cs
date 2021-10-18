using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using AutoFixture;
using AutoFixture.Xunit2;
using Bunit.TestAssets.SampleComponents;
using Microsoft.AspNetCore.Components;
using Shouldly;
using Xunit;

namespace Bunit
{
	public class InputEventDispatchExtensionsTest : EventDispatchExtensionsTest<ChangeEventArgs>
	{
		private static readonly string[] ExcludedMethodsFromGenericTests = new[] { "Change", "Input" };
		public static IEnumerable<object[]> Helpers { get; }
			= GetEventHelperMethods(typeof(InputEventDispatchExtensions), x => !ExcludedMethodsFromGenericTests.Contains(x.Name));

		private static readonly Fixture Fixture = new();

		protected override string ElementName => "input";

		[Theory(DisplayName = "Input events are raised correctly through helpers")]
		[MemberData(nameof(Helpers))]
		public void CanRaiseEvents(MethodInfo helper)
		{
			var expected = new ChangeEventArgs()
			{
				Value = "SOME VALUE",
			};

			VerifyEventRaisesCorrectly(helper, expected);
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test000(string value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test001(string? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(string));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test002(bool value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test003(bool? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(bool));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test004(int value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test005(int? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(int));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test006(long value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test007(long? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(long));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test008(short value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test009(short? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(short));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test010(float value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test011(float? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(float));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test012(double value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test013(double? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(double));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test014(decimal value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test015(decimal? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(decimal));
		}

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test016(DateTime value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		[UseCulture("en-US")]
		public void Test017(DateTime? value)
		{
			VerifySingleBindValue(DateTimeWithoutMillisecond(value));
			VerifySingleBindValue(default(DateTime));
		}

#if NET6_0_OR_GREATER
		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test018(DateTimeOffset value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		[UseCulture("en-US")]
		public void Test019(DateTimeOffset? value)
		{
			VerifySingleBindValue(DateTimeWithoutMillisecond(value));
			VerifySingleBindValue(default(DateTimeOffset));
		}
#endif

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		public void Test020(Foo value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change of value with Bind"), AutoData]
		[UseCulture("en-US")]
		public void Test021(Foo? value)
		{
			VerifySingleBindValue(value);
			VerifySingleBindValue(default(Foo));
		}

		private void VerifySingleBindValue<T>(T value)
		{
			var cut = RenderComponent<OnChangeBindSample<T>>(ps => ps
				.Add(p => p.BindValue, Fixture.Create<T>())
			 	.Add(p => p.OnChangeValue, Fixture.Create<string>())
				.Add(p => p.OnInputValue, Fixture.Create<string>()));

			cut.Find("#bind").Change(value);
			cut.Find("#onChangeInput").Change(value);
			cut.Find("#onInputInput").Input(value);

			cut.Instance.BindValue.ShouldBeEquivalentTo(value);
			cut.Instance.OnChangeValue.ShouldBeEquivalentTo(BindConverter.FormatValue(value));
			cut.Instance.OnInputValue.ShouldBeEquivalentTo(BindConverter.FormatValue(value));
		}

		private static DateTime? DateTimeWithoutMillisecond(DateTime? maybeDate)
			=> maybeDate is DateTime date
				? new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind)
				: null;

		private static DateTime DateTimeWithoutMillisecond(DateTime date)
			=> new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);

		private static DateTimeOffset? DateTimeWithoutMillisecond(DateTimeOffset? maybeDate)
			=> maybeDate is DateTimeOffset date
				? new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset)
				: null;

		private static DateTimeOffset DateTimeWithoutMillisecond(DateTimeOffset date)
			=> new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset);	
	}
}
