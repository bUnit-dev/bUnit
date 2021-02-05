public class FallbackServiceProvider : IServiceProvider
{
    public object GetService(Type serviceType)
    {
        return new DummyService();
    }
}
