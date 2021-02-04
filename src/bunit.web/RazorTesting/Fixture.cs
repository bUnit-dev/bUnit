using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bunit.Extensions;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Bunit
{
	/// <inheritdoc/>
	public class Fixture : FixtureBase<Fixture>
	{
		private readonly Dictionary<string, IRenderedFragment> renderedFragments = new(StringComparer.Ordinal);
		private IReadOnlyList<FragmentBase>? testData;

		private IReadOnlyList<FragmentBase> TestData
		{
			get
			{
				if (testData is null)
				{
					var renderedFragment = Renderer.RenderFragment(ChildContent!);
					var comps = Renderer.FindComponents<FragmentBase>(renderedFragment);
					testData = comps.Select(x => x.Instance).ToArray();
				}

				return testData;
			}
		}

		/// <summary>
		/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
		/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
		/// </summary>
		public BunitJSInterop JSInterop { get; } = new BunitJSInterop();

		/// <summary>
		/// Initializes a new instance of the <see cref="Fixture"/> class.
		/// </summary>
		public Fixture()
		{
			Services.AddDefaultTestContextServices(this, JSInterop);
		}

		/// <summary>
		/// Gets (and renders) the markup/component defined in the &lt;Fixture&gt;&lt;ComponentUnderTest&gt;...&lt;ComponentUnderTest/&gt;&lt;Fixture/&gt; element.
		///
		/// The HTML/component is only rendered the first this method is called.
		/// </summary>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		public IRenderedFragment GetComponentUnderTest() => GetOrRenderFragment(nameof(GetComponentUnderTest), SelectComponentUnderTest, Factory);

		/// <summary>
		/// Gets (and renders) the component of type <typeparamref name="TComponent"/> defined in the
		/// &lt;Fixture&gt;&lt;ComponentUnderTest&gt;...&lt;ComponentUnderTest/&gt;&lt;Fixture/&gt; element.
		///
		/// The HTML/component is only rendered the first this method is called.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render.</typeparam>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
		public IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>()
		    where TComponent : IComponent
		{
			var result = GetOrRenderFragment(nameof(GetComponentUnderTest), SelectComponentUnderTest, Factory<TComponent>);
			return TryCastTo<TComponent>(result);
		}

		/// <summary>
		/// Gets (and renders) the markup/component defined in the
		/// &lt;Fixture&gt;&lt;Fragment id="<paramref name="id"/>" &gt;...&lt;Fragment/&gt;&lt;Fixture/&gt; element.
		///
		/// If <paramref name="id"/> is null/not provided, the component defined in the first &lt;Fragment/&gt; in
		/// the &lt;Fixture/&gt; element is returned.
		///
		/// The HTML/component is only rendered the first this method is called.
		/// </summary>
		/// <param name="id">The id of the fragment where the HTML/component is defined in Razor syntax.</param>
		/// <returns>A <see cref="IRenderedFragmentBase"/>.</returns>
		public IRenderedFragment GetFragment(string? id = null)
		{
			var key = id ?? SelectFirstFragment().Id;
			var result = GetOrRenderFragment(key, SelectFragmentById, Factory);
			return result;
		}

		/// <summary>
		/// Gets (and renders) the component of type <typeparamref name="TComponent"/> defined in the
		/// &lt;Fixture&gt;&lt;Fragment id="<paramref name="id"/>" &gt;...&lt;Fragment/&gt;&lt;Fixture/&gt; element.
		///
		/// If <paramref name="id"/> is null/not provided, the component defined in the first &lt;Fragment/&gt; in
		/// the &lt;Fixture/&gt; element is returned.
		///
		/// The HTML/component is only rendered the first this method is called.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render.</typeparam>
		/// <param name="id">The id of the fragment where the component is defined in Razor syntax.</param>
		/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
		public IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null)
		    where TComponent : IComponent
		{
			var key = id ?? SelectFirstFragment().Id;
			var result = GetOrRenderFragment(key, SelectFragmentById, Factory<TComponent>);
			return TryCastTo<TComponent>(result);
		}

		/// <summary>
		/// Gets or renders the fragment specified in the id.
		/// For internal use mainly.
		/// </summary>
		private IRenderedFragment GetOrRenderFragment(string id, Func<string, FragmentBase> fragmentSelector, Func<RenderFragment, IRenderedFragment> renderedFragmentFactory)
		{
			if (renderedFragments.TryGetValue(id, out var renderedFragment))
			{
				return renderedFragment;
			}

			var fragment = fragmentSelector(id);
			var result = renderedFragmentFactory(fragment.ChildContent);
			renderedFragments.Add(id, result);
			return result;
		}

		private Fragment SelectFirstFragment()
		{
			return TestData.OfType<Fragment>().First();
		}

		private Fragment SelectFragmentById(string id)
		{
			return TestData.OfType<Fragment>().Single(x => x.Id.Equals(id, StringComparison.Ordinal));
		}

		private ComponentUnderTest SelectComponentUnderTest(string name)
		{
			return TestData.OfType<ComponentUnderTest>().Single();
		}

		private IRenderedComponent<TComponent> Factory<TComponent>(RenderFragment fragment)
		    where TComponent : IComponent
		{
			return this.RenderInsideRenderTree<TComponent>(fragment);
		}

		private IRenderedFragment Factory(RenderFragment fragment)
		{
			return this.RenderInsideRenderTree(fragment);
		}

		private static IRenderedComponent<TComponent> TryCastTo<TComponent>(IRenderedFragment target, [System.Runtime.CompilerServices.CallerMemberName] string sourceMethod = "")
		    where TComponent : IComponent
		{
			if (target is IRenderedComponent<TComponent> result)
			{
				return result;
			}

			if (target is IRenderedComponent<IComponent> other)
			{
				throw new InvalidOperationException($"The generic version of {sourceMethod} has previously returned an object of type IRenderedComponent<{other.Instance.GetType().Name}>. " +
					$"That cannot be cast to an object of type IRenderedComponent<{typeof(TComponent).Name}>.");
			}

			throw new InvalidOperationException($"It is not possible to call the generic version of {sourceMethod} after " +
				$"the non-generic version has been called on the same test context. Change all calls to the same generic version and try again.");
		}

		/// <inheritdoc/>
		protected override Task RunAsync()
		{
			return RunAsync(this);
		}
	}
}
