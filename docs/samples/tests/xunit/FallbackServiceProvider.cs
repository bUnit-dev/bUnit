using System;

namespace Bunit.Docs.Samples;

public class FallbackServiceProvider : IServiceProvider
{
    public object GetService(Type serviceType)
    { 
        return new DummyService();
    }
}

public class DummyService { }