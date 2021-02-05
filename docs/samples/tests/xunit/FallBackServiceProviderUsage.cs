public class FallBackServiceProviderUssageExample
{
    [Fact]
    public void FallBackServiceProviderReturns()
    {
        var sut = new TestProvider();
        ctx.AddFallbackServiceProvider(new FallBackServiceProvider());

        var dummyService = sut.GetService<DummyService>();

        Assert.NotNull(dummyService);
    }
}