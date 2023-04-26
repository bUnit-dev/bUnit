using AngleSharp.Dom;

namespace AngleSharpWrappers;

    public class TestFactory<T> : IElementFactory<T>
            where T : class, IElement
    {
        private readonly Func<T> _factory;

        public TestFactory(Func<T> factory)
        {
            _factory = factory;
        }

        public T GetElement() => _factory();
    }
