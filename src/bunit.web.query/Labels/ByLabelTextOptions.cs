namespace Bunit;

public record ByLabelTextOptions
{
	public static readonly ByLabelTextOptions Default = new();
	public StringComparison ComparisonType { get; set; } = StringComparison.Ordinal;
}
