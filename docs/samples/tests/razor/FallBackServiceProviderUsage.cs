public class FallBackServiceProviderUssageExample
{
    public void Example()
    {
        var sut = new TestProvider();
        ctx.AddFallbackServiceProvider(new FallBackServiceProvider());

        var dummyService = sut.GetService<DummyService>();
    }
}