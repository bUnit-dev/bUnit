using System;
using System.Collections.Generic;
using System.Linq;
using Egil.RazorComponents.Testing.Diffing;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Egil.RazorComponents.Testing
{
    public sealed class TestContext : IDisposable
    {
        private readonly ServiceCollection _serviceCollection = new ServiceCollection();
        private readonly Lazy<TestRenderer> _renderer;
        private readonly IReadOnlyList<FragmentBase> _testData;
        private readonly Lazy<HtmlParser> _htmlParser;
        private readonly Dictionary<string, IRenderedFragment> _renderedFragments = new Dictionary<string, IRenderedFragment>();

        private TestRenderer Renderer => _renderer.Value;
        private HtmlParser HtmlParser => _htmlParser.Value;

        public TestContext(IReadOnlyList<FragmentBase> testData)
        {
            _renderer = new Lazy<TestRenderer>(() =>
            {
                var serviceProvider = _serviceCollection.BuildServiceProvider();
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>() ?? new NullLoggerFactory();
                return new TestRenderer(serviceProvider, loggerFactory);
            });
            _htmlParser = new Lazy<HtmlParser>(() =>
            {
                return new HtmlParser(Renderer, new HtmlComparer());
            });
            _testData = testData;
        }

        public void AddService<T>(T implementation) => AddService<T, T>(implementation);

        public void AddService<TService, TImplementation>(TImplementation implementation) where TImplementation : TService
        {
            if (_renderer.IsValueCreated)
                throw new InvalidOperationException("Cannot configure services after the host has started operation");

            _serviceCollection.AddSingleton(typeof(TService), implementation);
        }

        public void WaitForNextRender(Action trigger)
        {
            if (trigger is null) throw new ArgumentNullException(nameof(trigger));
            var task = Renderer.NextRender;
            trigger();
            task.Wait(millisecondsTimeout: 1000);

            if (!task.IsCompleted)
            {
                throw new TimeoutException("No render occurred within the timeout period.");
            }
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>() where TComponent : class, IComponent
        {
            return AddComponent<TComponent>(ParameterView.Empty);
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>(params (string paramName, object valueValue)[] parameters) where TComponent : class, IComponent
        {
            var paramDict = parameters.ToDictionary(x => x.paramName, x => x.valueValue);
            var parameterView = ParameterView.FromDictionary(paramDict);
            return AddComponent<TComponent>(parameterView);
        }

        public RenderedComponent<TComponent> AddComponent<TComponent>(ParameterView parameters) where TComponent : class, IComponent
        {
            var result = new RenderedComponent<TComponent>(Renderer, HtmlParser);
            result.SetParametersAndRender(parameters);
            return result;
        }

        public RenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : class, IComponent
        {
            var fragmentKey = nameof(GetComponentUnderTest);

            if (_renderedFragments.TryGetValue(fragmentKey, out var fragment))
            {
                return (RenderedComponent<TComponent>)fragment;
            }
            else
            {
                var componentUnderTest = _testData.OfType<ComponentUnderTest>().Single();
                var result = new RenderedComponent<TComponent>(Renderer, HtmlParser);
                result.Render(componentUnderTest.ChildContent);
                _renderedFragments.Add(fragmentKey, result);

                return result;
            }
        }

        public RenderedFragment GetFragment(string id)
        {
            var fragmentKey = nameof(GetFragment) + id;
            if (_renderedFragments.TryGetValue(fragmentKey, out var renderedFragment))
            {
                return (RenderedFragment)renderedFragment;
            }
            else
            {
                var fragment = _testData.OfType<Fragment>().Single(x => x.Id.Equals(id, StringComparison.Ordinal));

                var result = new RenderedFragment(Renderer, HtmlParser);
                result.Render(fragment.ChildContent);
                _renderedFragments.Add(fragmentKey, result);

                return result;
            }
        }

        public RenderedComponent<TComponent> GetFragment<TComponent>(string id) where TComponent : class, IComponent
        {
            var fragmentKey = nameof(GetFragment) + id;
            if (_renderedFragments.TryGetValue(fragmentKey, out var renderedFragment))
            {
                return (RenderedComponent<TComponent>)renderedFragment;
            }
            else
            {
                var fragment = _testData.OfType<Fragment>().Single(x => x.Id.Equals(id, StringComparison.Ordinal));

                var result = new RenderedComponent<TComponent>(Renderer, HtmlParser);
                result.Render(fragment.ChildContent);
                _renderedFragments.Add(fragmentKey, result);

                return result;
            }
        }

        public void Dispose()
        {
            if (_renderer.IsValueCreated)
                _renderer.Value.Dispose();
            if (_htmlParser.IsValueCreated)
                _htmlParser.Value.Dispose();
        }
    }
}
