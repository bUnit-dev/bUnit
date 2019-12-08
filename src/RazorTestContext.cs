using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A razor test context is a factory that makes it possible to create components under tests,
    /// either directly or through components declared in razor code.
    /// </summary>
    public class RazorTestContext : TestContext, IRazorTestContext
    {
        private readonly IReadOnlyList<FragmentBase> _testData;
        private readonly Dictionary<string, IRenderedFragment> _renderedFragments = new Dictionary<string, IRenderedFragment>();

        /// <summary>
        /// Creates an instance of the <see cref="RazorTestContext"/> that has access the fragments defined 
        /// in the associated &lt;Fixture&gt; element.
        /// </summary>
        /// <param name="testData"></param>
        public RazorTestContext(IReadOnlyList<FragmentBase> testData)
        {
            _testData = testData;
        }

        /// <inheritdoc/>
        public IRenderedFragment RenderComponentUnderTest()
        {
            return RenderFragment(nameof(RenderComponentUnderTest), SelectComponentUnderTest, Factory);

            static IRenderedFragment Factory(RazorTestContext context, RenderFragment fragment)
                => new RenderedFragment(context, fragment);
        }

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> RenderComponentUnderTest<TComponent>() where TComponent : class, IComponent
        {
            return RenderFragment(nameof(RenderComponentUnderTest), SelectComponentUnderTest, Factory);

            static IRenderedComponent<TComponent> Factory(RazorTestContext context, RenderFragment fragment)
                => new RenderedComponent<TComponent>(context, fragment);
        }

        /// <inheritdoc/>
        public IRenderedComponent<TComponent> RenderFragment<TComponent>(string? id = null) where TComponent : class, IComponent
        {
            var key = nameof(RenderFragment) + (id ?? string.Empty);

            return id is null
                ? RenderFragment(key, SelectFirstFragment, Factory)
                : RenderFragment(key, SelectFragmentById, Factory);

            static IRenderedComponent<TComponent> Factory(RazorTestContext context, RenderFragment fragment)
                => new RenderedComponent<TComponent>(context, fragment);
        }

        /// <inheritdoc/>
        public IRenderedFragment RenderFragment(string? id = null)
        {
            var key = nameof(RenderFragment) + (id ?? string.Empty);

            return id is null
                ? RenderFragment(key, SelectFirstFragment, Factory)
                : RenderFragment(key, SelectFragmentById, Factory);

            static IRenderedFragment Factory(RazorTestContext context, RenderFragment fragment)
                => new RenderedFragment(context, fragment);
        }

        /// <inheritdoc/>
        private TRenderedFragment RenderFragment<TRenderedFragment>(string id, Func<string, FragmentBase> fragmentSelector, Func<RazorTestContext, RenderFragment, TRenderedFragment> renderedFragmentFactory)
            where TRenderedFragment : IRenderedFragment
        {
            if (_renderedFragments.TryGetValue(id, out var renderedFragment))
            {
                return (TRenderedFragment)renderedFragment;
            }
            else
            {
                var fragment = fragmentSelector(id);// 
                var result = renderedFragmentFactory(this, fragment.ChildContent);
                _renderedFragments.Add(id, result);
                return result;
            }
        }

        private Fragment SelectFirstFragment(string _)
        {
            return _testData.OfType<Fragment>().First();
        }

        private Fragment SelectFragmentById(string id)
        {
            return _testData.OfType<Fragment>().Single(x => x.Id.Equals(id, StringComparison.Ordinal));
        }

        private ComponentUnderTest SelectComponentUnderTest(string _)
        {
            return _testData.OfType<ComponentUnderTest>().Single();
        }
    }
}
