using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// A razor test context is a factory that makes it possible to create components under tests,
    /// either directly or through components declared in razor code.
    /// </summary>
    public interface IRazorTestContext : ITestContext
    {
        /// <summary>
        /// Gets (and renders) the HTML/component defined in the &lt;Fixture&gt;&lt;ComponentUnderTest&gt;...&lt;ComponentUnderTest/&gt;&lt;Fixture/&gt; element.
        /// 
        /// The HTML/component is only rendered the first this method is called.
        /// </summary>
        /// <returns>A <see cref="IRenderedFragment"/></returns>
        IRenderedFragment GetComponentUnderTest();

        /// <summary>
        /// Gets (and renders) the component of type <typeparamref name="TComponent"/> defined in the 
        /// &lt;Fixture&gt;&lt;ComponentUnderTest&gt;...&lt;ComponentUnderTest/&gt;&lt;Fixture/&gt; element.
        /// 
        /// The HTML/component is only rendered the first this method is called.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to render</typeparam>
        /// <returns>A <see cref="IRenderedComponent{TComponent}"/></returns>
        IRenderedComponent<TComponent> GetComponentUnderTest<TComponent>() where TComponent : class, IComponent;

        /// <summary>
        /// Gets (and renders) the HTML/component defined in the 
        /// &lt;Fixture&gt;&lt;Fragment id="<paramref name="id"/>" &gt;...&lt;Fragment/&gt;&lt;Fixture/&gt; element.
        ///
        /// If <paramref name="id"/> is null/not provided, the component defined in the first &lt;Fragment/&gt; in 
        /// the &lt;Fixture/&gt; element is returned.
        /// 
        /// The HTML/component is only rendered the first this method is called.
        /// </summary>
        /// <param name="id">The id of the fragment where the HTML/component is defined in Razor syntax.</param>
        /// <returns>A <see cref="IRenderedFragment"/></returns>
        IRenderedFragment GetFragment(string? id = null);

        /// <summary>
        /// Gets (and renders) the component of type <typeparamref name="TComponent"/> defined in the 
        /// &lt;Fixture&gt;&lt;Fragment id="<paramref name="id"/>" &gt;...&lt;Fragment/&gt;&lt;Fixture/&gt; element.
        /// 
        /// If <paramref name="id"/> is null/not provided, the component defined in the first &lt;Fragment/&gt; in 
        /// the &lt;Fixture/&gt; element is returned.
        /// 
        /// The HTML/component is only rendered the first this method is called.
        /// </summary>
        /// <typeparam name="TComponent">The type of component to render</typeparam>
        /// <param name="id">The id of the fragment where the component is defined in Razor syntax.</param>
        /// <returns>A <see cref="IRenderedComponent{TComponent}"/></returns>
        IRenderedComponent<TComponent> GetFragment<TComponent>(string? id = null) where TComponent : class, IComponent;
    }
}