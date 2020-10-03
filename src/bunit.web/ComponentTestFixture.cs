using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit
{

	/// <summary>
	/// Base class for test classes that contains Razor component tests.
	/// </summary>
	[Obsolete("Inherit from TestContext instead, and add a 'using static Bunit.ComponentParameterFactory' to your test class to keep using " +
			  "the component parameter factories/helpers, provided in this class. Alternatively, you can switch to using the RenderComponent " +
			  "overload that takes an ComponentParameterBuilder as input. " +
			  "This class will be removed in a later release.")]
	public abstract class ComponentTestFixture : TestContext
	{
		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value 
		/// for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback(string name, Action callback)
			=> ComponentParameterFactory.EventCallback(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback(string name, Action<object> callback)
			=> ComponentParameterFactory.EventCallback(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback(string name, Func<Task> callback)
			=> ComponentParameterFactory.EventCallback(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback(string name, Func<object, Task> callback)
			=> ComponentParameterFactory.EventCallback(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback<TValue>(string name, Action callback)
			=> ComponentParameterFactory.EventCallback<TValue>(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback<TValue>(string name, Action<TValue> callback)
			=> ComponentParameterFactory.EventCallback<TValue>(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback<TValue>(string name, Func<Task> callback)
			=> ComponentParameterFactory.EventCallback<TValue>(name, callback);

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
		/// <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter EventCallback<TValue>(string name, Func<TValue, Task> callback)
			=> ComponentParameterFactory.EventCallback<TValue>(name, callback);

		/// <summary>
		/// Creates a component parameter which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="value">Value or null of the parameter</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter Parameter(string name, object? value)
			=> ComponentParameterFactory.Parameter(name, value);

		/// <summary>
		/// Creates a cascading value which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="name">Parameter name</param>
		/// <param name="value">Value of the parameter</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter CascadingValue(string name, object value)
			=> ComponentParameterFactory.CascadingValue(name, value);


		/// <summary>
		/// Creates a cascading value which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="value">Value of the parameter</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter CascadingValue(object value)
			=> ComponentParameterFactory.CascadingValue(value);

		/// <summary>
		/// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided 
		/// <paramref name="markup"/> as rendered output.
		/// </summary>
		/// <param name="markup">Markup to pass to the child content parameter</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter ChildContent(string markup)
			=> ComponentParameterFactory.ChildContent(markup);

		/// <summary>
		/// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
		/// with the provided <paramref name="parameters"/> as input.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/></typeparam>
		/// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter ChildContent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent
			=> ComponentParameterFactory.ChildContent<TComponent>(parameters);

		/// <summary>
		/// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided 
		/// <paramref name="markup"/> as rendered output and passes it to the parameter specified in <paramref name="name"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="markup">Markup to pass to the render fragment parameter</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter RenderFragment(string name, string markup)
			=> ComponentParameterFactory.RenderFragment(name, markup);


		/// <summary>
		/// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
		/// with the provided <paramref name="parameters"/> as input, and passes it to the parameter specified in <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/></typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter RenderFragment<TComponent>(string name, params ComponentParameter[] parameters) where TComponent : class, IComponent
			=> ComponentParameterFactory.RenderFragment<TComponent>(name, parameters);

		/// <summary>
		/// Creates a template component parameter which will pass the <paramref name="template"/> <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
		/// to the parameter with the name <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="TValue">The value used to build the content.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="template"><see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" /> to pass to the parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter Template<TValue>(string name, RenderFragment<TValue> template)
			=> ComponentParameterFactory.Template<TValue>(name, template);

		/// <summary>
		/// Creates a template component parameter which will pass the a <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
		/// to the parameter with the name <paramref name="name"/>.
		/// The <paramref name="markupFactory"/> will be used to generate the markup inside the template.
		/// </summary>
		/// <typeparam name="TValue">The value used to build the content.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="markupFactory">A markup factory that takes a <typeparamref name="TValue"/> as input and returns markup/HTML.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		protected static ComponentParameter Template<TValue>(string name, Func<TValue, string> markupFactory)
			=> ComponentParameterFactory.Template<TValue>(name, markupFactory);
	}
}
