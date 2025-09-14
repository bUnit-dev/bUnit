using System.Reflection;
using Xunit.Sdk;
using Xunit.v3;

namespace Xunit;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
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

	public override ValueTask<IReadOnlyCollection<ITheoryDataRow>> GetData(MethodInfo testMethod,
		DisposalTracker disposalTracker)
	{
		var rows = Enumerable.Range(1, Count).Select(i => new TheoryDataRow(new object[] { i })).ToArray();
		return new ValueTask<IReadOnlyCollection<ITheoryDataRow>>(rows);
	}

	public override bool SupportsDiscoveryEnumeration() => false;
}
