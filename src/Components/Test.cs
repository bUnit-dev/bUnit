namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Represents a test method that takes an <see cref="IRazorTestContext"/> as input
    /// and perform arrange, act and assert steps on the related render tree.
    /// </summary>
    /// <param name="context"></param>
    public delegate void Test(IRazorTestContext context);
}
