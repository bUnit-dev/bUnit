namespace Bunit;

[AttributeUsage(AttributeTargets.All, Inherited = false)]
internal sealed class RemovedInFutureVersionAttribute(string todo) : Attribute
{
	public string Todo { get; } = todo;
}
