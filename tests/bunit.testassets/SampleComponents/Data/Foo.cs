using System.ComponentModel;

namespace Bunit.TestAssets.SampleComponents;

[TypeConverter(typeof(FooTypeConverter))]
public class Foo
{
	public int Age { get; set; }

	public string Name { get; set; }
}
