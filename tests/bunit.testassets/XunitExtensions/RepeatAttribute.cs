using System.Reflection;
using Xunit.Sdk;

namespace Xunit;

public sealed class RepeatAttribute : DataAttribute
{
	public int Count { get; }

	public RepeatAttribute(int count)
	{
		if (count < 1)
		{
			throw new ArgumentOutOfRangeException(
				paramName: nameof(count),
				message: "Repeat count must be greater than 0.");
		}

		Count = count;
	}

	public override IEnumerable<object[]> GetData(MethodInfo testMethod)
	{
		for (int count = 1; count <= Count; count++)
		{
			yield return new object[] { count };
		}
	}
}
