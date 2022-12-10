namespace Bunit;

public class InputEventDispatchExtensionsTest : EventDispatchExtensionsTest<ChangeEventArgs>
{
	private static readonly string[] ExcludedMethodsFromGenericTests = new[] { "Change", "Input", "Submit" };
        public static IEnumerable<object[]> Helpers { get; }
        	= GetEventHelperMethods(typeof(InputEventDispatchExtensions), x => !ExcludedMethodsFromGenericTests
        	.Contains(x.Name.Replace("Async", string.Empty, StringComparison.Ordinal)));

	private static readonly Fixture Fixture = new();

	protected override string ElementName => "input";

	[UITheory(DisplayName = "Input events are raised correctly through helpers")]
	[MemberData(nameof(Helpers))]
	public void CanRaiseEvents(MethodInfo helper)
	{
		var expected = new ChangeEventArgs()
		{
			Value = "SOME VALUE",
		};

		VerifyEventRaisesCorrectly(helper, expected);
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test000(string value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test001(string? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(string));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test002(bool value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test003(bool? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(bool));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test004(int value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test005(int? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(int));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test006(long value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test007(long? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(long));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test008(short value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test009(short? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(short));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test010(float value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test011(float? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(float));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test012(double value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test013(double? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(double));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test014(decimal value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test015(decimal? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(decimal));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test016(DateTime value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test017(DateTime? value)
	{
		VerifySingleBindValue(DateTimeWithoutMillisecond(value));
		VerifySingleBindValue(default(DateTime));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test018(DateTimeOffset value) => VerifySingleBindValue(DateTimeWithoutMillisecond(value));

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test019(DateTimeOffset? value)
	{
		VerifySingleBindValue(DateTimeWithoutMillisecond(value));
		VerifySingleBindValue(default(DateTimeOffset));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test020(Foo value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test021(Foo? value)
		=> VerifySingleBindValue(value);

	[UIFact(DisplayName = "Change and Input events are raised correctly for null object")]
	public void Test022()
		=> VerifySingleBindValue(default(Foo));

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test023(Cars value) => VerifySingleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test024(Cars? value)
	{
		VerifySingleBindValue(value);
		VerifySingleBindValue(default(Cars));
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test100(string[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test101(string?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new string?[] { default(string), default(string) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test102(bool[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test103(bool?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new bool?[] { default(bool), default(bool) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test104(int[] value) => VerifyMultipleBindValue(value);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test105(int?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new int?[] { default(int), default(int) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test106(long[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test107(long?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new long?[] { default(long), default(long) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test108(short[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test109(short?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new short?[] { default(short), default(short) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test110(float[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test111(float?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new float?[] { default(float), default(float) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test112(double[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test113(double?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new double?[] { default(double), default(double) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test114(decimal[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test115(decimal?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new decimal?[] { default(decimal), default(decimal) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test116(DateTime[] values) => VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test117(DateTime?[] values)
	{
		VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));
		VerifyMultipleBindValue(new DateTime?[] { default(DateTime), default(DateTime) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test118(DateTimeOffset[] values) => VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	[UseCulture("en-US")]
	public void Test119(DateTimeOffset?[] values)
	{
		VerifyMultipleBindValue(DateTimeWithoutMillisecond(values));
		VerifyMultipleBindValue(new DateTimeOffset?[] { default(DateTimeOffset), default(DateTimeOffset) });
	}

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test123(Cars[] values) => VerifyMultipleBindValue(values);

	[UITheory(DisplayName = "Change and Input events are raised correctly"), AutoData]
	public void Test124(Cars?[] values)
	{
		VerifyMultipleBindValue(values);
		VerifyMultipleBindValue(new Cars?[] { default(Cars), default(Cars) });
	}

	private void VerifyMultipleBindValue<T>(T[] values)
	{
		var cut = RenderComponent<OnChangeMultipleBindSample<T>>(ps => ps
			.Add(p => p.Options, values));

		cut.Find("#bind").Change(values);
		cut.Find("#onChangeInput").Change(values);
		cut.Find("#onInputInput").Input(values);

		cut.Instance.BindValues.ShouldBe(values);
		cut.Instance.OnChangeValue.ShouldBe(FormatValues(values));
		cut.Instance.OnInputValue.ShouldBe(FormatValues(values));
	}

	private void VerifySingleBindValue<T>(T value)
	{
		var cut = RenderComponent<OnChangeBindSample<T>>(ps => ps
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
