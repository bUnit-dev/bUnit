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
    public sealed class TestContext : TestHost
    {
        private readonly IReadOnlyList<FragmentBase> _testData;
        private readonly Dictionary<string, IRenderedFragment> _renderedFragments = new Dictionary<string, IRenderedFragment>();

        public TestContext(IReadOnlyList<FragmentBase> testData)
        {
            _testData = testData;
        }

        public RenderedComponent<IComponent> GetComponentUnderTest()
        {
            return GetComponentUnderTest<IComponent>();
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
                var result = new RenderedComponent<TComponent>(this, componentUnderTest.ChildContent);
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

                var result = new RenderedComponent<TComponent>(this, fragment.ChildContent);
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

                var result = new RenderedFragment(this, fragment.ChildContent);
                _renderedFragments.Add(fragmentKey, result);

                return result;
            }
        }
    }
}
