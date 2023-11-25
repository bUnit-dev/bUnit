using System.Text;

namespace Bunit.Web.Stubs;

internal static class StubAttributeGenerator
{
	public static string StubAttribute => @"
#if NET5_0_OR_GREATER
namespace Bunit;

/// <summary>
/// Indicates that the Component will be enriched by a generated class.
/// </summary>
[global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false)]
public sealed class StubAttribute : global::System.Attribute
{
    /// <summary>
    /// The target type of the component the stub represents.
    /// </summary>
    public global::System.Type TargetType { get; }

    /// <summary>
    /// Creates an instance of the <see cref=""StubAttribute""/>.
    /// </summary>
    /// <param name=""targetType"">The target type of the component the stub represents.</param>
    public StubAttribute(global::System.Type targetType)
    {
        TargetType = targetType;
    }
}
#endif";
}
