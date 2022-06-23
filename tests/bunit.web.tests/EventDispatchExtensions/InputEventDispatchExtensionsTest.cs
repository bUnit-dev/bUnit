namespace Bunit
{
	public class InputEventDispatchExtensionsTest : EventDispatchExtensionsTest<ChangeEventArgs>
	{
		private static readonly string[] ExcludedMethodsFromGenericTests = new[] { "Change", "Input", "Submit" };
        public static IEnumerable<object[]> Helpers { get; }
        	= GetEventHelperMethods(typeof(InputEventDispatchExtensions), x => !ExcludedMethodsFromGenericTests
        	.Contains(x.Name.Replace("Async", string.Empty, StringComparison.Ordinal)));

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

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test000(string value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test001(string? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(string));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test002(bool value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test003(bool? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(bool));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test004(int value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test005(int? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(int));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test006(long value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test007(long? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(long));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test008(short value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test009(short? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(short));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test010(float value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test011(float? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(float));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test012(double value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test013(double? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(double));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test014(decimal value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test015(decimal? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(decimal));
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public Task Test016(DateTime value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public async Task Test017(DateTime? value)
		{
			await VerifySingleBindValue(DateTimeWithoutMillisecond(value));
			await VerifySingleBindValue(default(DateTime));
		}

#if NET6_0_OR_GREATER
		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public Task Test018(DateTimeOffset value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public async Task Test019(DateTimeOffset? value)
		{
			await VerifySingleBindValue(DateTimeWithoutMillisecond(value));
			await VerifySingleBindValue(default(DateTimeOffset));
		}
#endif

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test020(Foo value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test021(Foo? value)
			=> VerifySingleBindValue(value);

#if NET6_0_OR_GREATER

		[Fact(DisplayName = "Change and Input events are raised correctly for null object")]
		public Task Test022()
			=> VerifySingleBindValue(default(Foo));

#endif

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test023(Cars value) => VerifySingleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test024(Cars? value)
		{
			await VerifySingleBindValue(value);
			await VerifySingleBindValue(default(Cars));
		}

#if NET6_0_OR_GREATER

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test100(string[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test101(string?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new string?[] { default(string), default(string) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test102(bool[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test103(bool?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new bool?[] { default(bool), default(bool) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test104(int[] value) => VerifyMultipleBindValue(value);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test105(int?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new int?[] { default(int), default(int) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test106(long[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test107(long?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new long?[] { default(long), default(long) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test108(short[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test109(short?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new short?[] { default(short), default(short) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test110(float[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test111(float?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new float?[] { default(float), default(float) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test112(double[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test113(double?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new double?[] { default(double), default(double) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test114(decimal[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test115(decimal?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new decimal?[] { default(decimal), default(decimal) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public Task Test116(DateTime[] values) => VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public async Task Test117(DateTime?[] values)
		{
			await VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));
			await VerifyMultipleBindValue(new DateTime?[] { default(DateTime), default(DateTime) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public Task Test118(DateTimeOffset[] values) => VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		[UseCulture("en-US")]
		public async Task Test119(DateTimeOffset?[] values)
		{
			await VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));
			await VerifyMultipleBindValue(new DateTimeOffset?[] { default(DateTimeOffset), default(DateTimeOffset) });
		}

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public Task Test123(Cars[] values) => VerifyMultipleBindValue(values);

		[Theory(DisplayName = "Change and Input events are raised correctly"), AutoData]
		public async Task Test124(Cars?[] values)
		{
			await VerifyMultipleBindValue(values);
			await VerifyMultipleBindValue(new Cars?[] { default(Cars), default(Cars) });
		}

		private async Task VerifyMultipleBindValue<T>(T[] values)
		{
			var cut = await RenderComponent<OnChangeMultipleBindSample<T>>(ps => ps
				.Add(p => p.Options, values));

			cut.Find("#bind").Change(values);
			cut.Find("#onChangeInput").Change(values);
			cut.Find("#onInputInput").Input(values);

			cut.Instance.BindValues.ShouldBe(values);
			cut.Instance.OnChangeValue.ShouldBe(FormatValues(values));
			cut.Instance.OnInputValue.ShouldBe(FormatValues(values));
		}
#endif

		private async Task VerifySingleBindValue<T>(T value)
		{
			var cut = await RenderComponent<OnChangeBindSample<T>>(ps => ps
				.Add(p => p.BindValue, Fixture.Create<T>())
				.Add(p => p.OnChangeValue, Fixture.Create<T>())
				.Add(p => p.OnInputValue, Fixture.Create<T>()));

			cut.Find("#bind").Change(value);
			cut.Find("#onChangeInput").Change(value);
			cut.Find("#onInputInput").Input(value);

			cut.Instance.BindValue.ShouldBeEquivalentTo(value);
			cut.Instance.OnChangeValue.ShouldBeEquivalentTo(FormatValue(value));
			cut.Instance.OnInputValue.ShouldBeEquivalentTo(FormatValue(value));
		}

		private static object?[] FormatValues<T>(T[] values)
			=> values.Select(x => FormatValue(x)).ToArray();

		private static object? FormatValue<T>(T value)
			=> value is not null
				? BindConverter.FormatValue(value)
				: null;

		private static DateTime?[] DateTimeWithoutMillisecond(DateTime?[] maybeDate)
			=> maybeDate.Select(x => DateTimeWithoutMillisecond(x)).ToArray();

		private static DateTime? DateTimeWithoutMillisecond(DateTime? maybeDate)
			=> maybeDate is DateTime date
				? new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind)
				: null;

		private static DateTime[] DateTimeWithoutMillisecond(DateTime[] maybeDate)
			=> maybeDate.Select(x => DateTimeWithoutMillisecond(x)).ToArray();

		private static DateTime DateTimeWithoutMillisecond(DateTime date)
			=> new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Kind);

		private static DateTimeOffset?[] DateTimeWithoutMillisecond(DateTimeOffset?[] maybeDate)
			=> maybeDate.Select(x => DateTimeWithoutMillisecond(x)).ToArray();

		private static DateTimeOffset? DateTimeWithoutMillisecond(DateTimeOffset? maybeDate)
			=> maybeDate is DateTimeOffset date
				? new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset)
				: null;

		private static DateTimeOffset[] DateTimeWithoutMillisecond(DateTimeOffset[] maybeDate)
			=> maybeDate.Select(x => DateTimeWithoutMillisecond(x)).ToArray();

		private static DateTimeOffset DateTimeWithoutMillisecond(DateTimeOffset date)
			=> new DateTimeOffset(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, date.Offset);
	}
}
