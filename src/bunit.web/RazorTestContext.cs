using System;
using System.Collections.Generic;
using System.Linq;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A razor test context is a factory that makes it possible to create components under tests,
	/// either directly or through components declared in razor code.
	/// </summary>
	public class RazorTestContext : TestContext, IRazorTestContext
	{
		private readonly IReadOnlyList<FragmentBase> _testData;
		private readonly Dictionary<string, IWebRenderedFragment> _renderedFragments = new Dictionary<string, IWebRenderedFragment>();

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
		public IWebRenderedFragment GetComponentUnderTest() => GetOrRenderFragment(nameof(GetComponentUnderTest), SelectComponentUnderTest, Factory);

		/// <inheritdoc/>
		public IWebRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : IComponent
		{
			var result = GetOrRenderFragment(nameof(GetComponentUnderTest), SelectComponentUnderTest, Factory<TComponent>);
			return TryCastTo<TComponent>(result);
		}

		/// <inheritdoc/>
		public IWebRenderedFragment GetFragment(string? id = null)
		{
			var key = id ?? SelectFirstFragment().Id;
			var result = GetOrRenderFragment(key, SelectFragmentById, Factory);
			return result;
		}

		/// <inheritdoc/>
		public IWebRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent : IComponent
		{
			var key = id ?? SelectFirstFragment().Id;
			var result = GetOrRenderFragment(key, SelectFragmentById, Factory<TComponent>);
			return TryCastTo<TComponent>(result);
		}

		/// <summary>
		/// Gets or renders the fragment specified in the id.
		/// For internal use mainly.
		/// </summary>
		private IWebRenderedFragment GetOrRenderFragment(string id, Func<string, FragmentBase> fragmentSelector, Func<RenderFragment, IWebRenderedFragment> renderedFragmentFactory)
		{
			if (_renderedFragments.TryGetValue(id, out var renderedFragment))
			{
				return renderedFragment;
			}
			else
			{
				var fragment = fragmentSelector(id);
				var result = renderedFragmentFactory(fragment.ChildContent);
				_renderedFragments.Add(id, result);
				return result;
			}
		}

		private Fragment SelectFirstFragment()
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

		private IWebRenderedComponent<TComponent> Factory<TComponent>(RenderFragment fragment) where TComponent : IComponent
		{
			var renderId = Renderer.RenderFragment(fragment).GetAwaiter().GetResult();
			var compInfo = Renderer.FindComponent<TComponent>(renderId);
			return new RenderedComponent<TComponent>(Services, compInfo.ComponentId, compInfo.Component);
		}

		private IWebRenderedFragment Factory(RenderFragment fragment)
		{
			var renderId = Renderer.RenderFragment(fragment).GetAwaiter().GetResult();
			return new RenderedFragment(Services, renderId);
		}

		private IWebRenderedComponent<TComponent> TryCastTo<TComponent>(IWebRenderedFragment target, [System.Runtime.CompilerServices.CallerMemberName] string sourceMethod = "") where TComponent : IComponent
		{
			if (target is IWebRenderedComponent<TComponent> result)
			{
				return result;
			}

			if (target is IWebRenderedComponent<IComponent> other)
			{
				throw new InvalidOperationException($"The generic version of {sourceMethod} has previously returned an object of type IRenderedComponent<{other.Instance.GetType().Name}>. " +
					$"That cannot be cast to an object of type IRenderedComponent<{typeof(TComponent).Name}>.");
			}

			if (target is IRenderedFragment)
			{
				throw new InvalidOperationException($"It is not possible to call the generic version of {sourceMethod} after " +
					$"the non-generic version has been called on the same test context. Change all calls to the same generic version and try again.");
			}
			else
			{
				throw new Exception($"This line should never have been reached. An unknown type was placed inside the {nameof(_renderedFragments)}.");
			}
		}

	}

}
