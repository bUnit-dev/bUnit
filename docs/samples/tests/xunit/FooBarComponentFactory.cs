#if NET5_0_OR_GREATER
namespace Bunit.Docs.Samples
{
    using System;
    using Microsoft.AspNetCore.Components;
    using Bunit;

    public class FooBarComponentFactory : IComponentFactory
    {
        public bool CanCreate(Type componentType)
            => typeof(Foo) == componentType;

        public IComponent Create(Type componentType)
            => new Bar();
    }
}
#endif