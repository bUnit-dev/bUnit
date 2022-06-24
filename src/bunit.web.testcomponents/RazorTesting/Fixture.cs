using Bunit.Extensions;
using Bunit.RazorTesting;
using Microsoft.JSInterop;

namespace Bunit;

/// <inheritdoc/>
public class Fixture : FixtureBase<Fixture>
{
	private readonly Dictionary<string, IRenderedFragment> renderedFragments = new(StringComparer.Ordinal);
	private IReadOnlyList<FragmentBase>? testData;

	private async Task<IReadOnlyList<FragmentBase>> TestData()
	{
		if (testData is null)
		{
			var renderedFragment = await Renderer.RenderFragment(ChildContent!);
			var comps = Renderer.FindComponents<FragmentBase>(renderedFragment);
			testData = comps.Select(x => x.Instance).ToArray();
		}

		return testData;
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
	public async Task<IRenderedFragment> GetComponentUnderTest()
	{
		return await GetOrRenderFragment(nameof(GetComponentUnderTest), async s => await SelectComponentUnderTest(s), async fragment => await Factory(fragment));
	}

	/// <summary>
	/// Gets (and renders) the component of type <typeparamref name="TComponent"/> defined in the
	/// &lt;Fixture&gt;&lt;ComponentUnderTest&gt;...&lt;ComponentUnderTest/&gt;&lt;Fixture/&gt; element.
	///
	/// The HTML/component is only rendered the first this method is called.
	/// </summary>
	/// <typeparam name="TComponent">The type of component to render.</typeparam>
	/// <returns>A <see cref="IRenderedComponentBase{TComponent}"/>.</returns>
	public async Task<IRenderedComponent<TComponent>> GetComponentUnderTest<TComponent>()
		where TComponent : IComponent
	{
		var result = await GetOrRenderFragment(nameof(GetComponentUnderTest), async s => await SelectComponentUnderTest(s), async fragment => await Factory<TComponent>(fragment));
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
	public async Task<IRenderedFragment> GetFragment(string? id = null)
	{
		var key = id ?? (await SelectFirstFragment()).Id;
		return await GetOrRenderFragment(key, async s => await SelectFragmentById(s), async fragment => await Factory(fragment));
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
	public async Task<IRenderedComponent<TComponent>> GetFragment<TComponent>(string? id = null)
		where TComponent : IComponent
	{
		var key = id ?? (await SelectFirstFragment()).Id;
		var result = await GetOrRenderFragment(key, async s => await SelectFragmentById(s), async fragment => await Factory<TComponent>(fragment));
		return TryCastTo<TComponent>(result);
	}

	/// <summary>
	/// Gets or renders the fragment specified in the id.
	/// For internal use mainly.
	/// </summary>
	private async Task<IRenderedFragment> GetOrRenderFragment(string id, Func<string, Task<FragmentBase>> fragmentSelector, Func<RenderFragment, Task<IRenderedFragment>> renderedFragmentFactory)
	{
		if (renderedFragments.TryGetValue(id, out var renderedFragment))
		{
			return renderedFragment;
		}

		var fragment = await fragmentSelector(id);
		var result = await renderedFragmentFactory(fragment.ChildContent);
		renderedFragments.Add(id, result);
		return result;
	}

	private async Task<Fragment> SelectFirstFragment()
	{
		return (await TestData()).OfType<Fragment>().First();
	}

	private async Task<Fragment> SelectFragmentById(string id)
	{
		return (await TestData()).OfType<Fragment>().Single(x => x.Id.Equals(id, StringComparison.Ordinal));
	}

	private async Task<ComponentUnderTest> SelectComponentUnderTest(string name)
	{
		return (await TestData()).OfType<ComponentUnderTest>().Single();
	}

	private async Task<IRenderedComponent<TComponent>> Factory<TComponent>(RenderFragment fragment)
		where TComponent : IComponent
	{
		return (IRenderedComponent<TComponent>) await this.RenderInsideRenderTree<TComponent>(fragment);
	}

	private async Task<IRenderedFragment> Factory(RenderFragment fragment)
	{
		return (IRenderedFragment) await this.RenderInsideRenderTree(fragment);
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
