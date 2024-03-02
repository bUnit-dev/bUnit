namespace Bunit.Web.Stubs.AttributeStubGenerator;

internal static class ComponentStubAttribute
{
	public static string ComponentStubAttributeSource = $$"""
	                                                    {{HeaderProvider.Header}}

	                                                    #if NET5_0_OR_GREATER
	                                                    namespace Bunit;

	                                                    /// <summary>
	                                                    /// Indicates that the component will be enriched by a generated class.
	                                                    /// </summary>
	                                                    [global::System.AttributeUsage(global::System.AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	                                                    internal sealed class ComponentStubAttribute<T> : global::System.Attribute
	                                                        where T : global::Microsoft.AspNetCore.Components.IComponent
	                                                    {
	                                                    }
	                                                    #endif
	                                                    """;
}
