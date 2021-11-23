using System;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json;
using Bunit.TestAssets.SampleComponents;

namespace Bunit.TestAssets.SampleComponents;

public class FooTypeConverter : TypeConverter
{
	public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
	{
		return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
	}

	public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
	{
		return destinationType == typeof(string) || base.CanConvertFrom(context, destinationType);
	}

	public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
	{
		if (value is string input)
		{
			return JsonSerializer.Deserialize<Foo>(input);
		}
		return base.ConvertFrom(context, culture, value);
	}

	public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
	{
		if (value is Foo input)
		{
			return JsonSerializer.Serialize(input);
		}

		return base.ConvertTo(context, culture, value, destinationType);
	}
}
