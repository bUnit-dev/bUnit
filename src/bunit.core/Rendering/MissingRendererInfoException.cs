#if NET9_0_OR_GREATER
namespace Bunit.Rendering;

/// <summary>
/// Represents an exception that is thrown when a component under test is trying to access the 'RendererInfo' property, which has not been specified.
/// </summary>
public sealed class MissingRendererInfoException : Exception
{
	/// <summary>
	/// Initializes a new instance of the <see cref="MissingRendererInfoException"/> class.
	/// </summary>
	public MissingRendererInfoException()
		: base("""
		       A component under test is trying to access the 'RendererInfo' property, which has not been specified. Set it via TestContext.Renderer.SetRendererInfo.

		       For example:

		       public class SomeTestClass : TestContext
		       {
		         [Fact]
		       	 public void SomeTestCase()
		       	 {
		       	   Renderer.SetRendererInfo(new RendererInfo("Server", true));
		       	   ...
		         }
		       }

		       The four built in render names are 'Static', 'Server', 'WebAssembly', and 'WebView'.
		       """)
	{
	}
}
#endif
