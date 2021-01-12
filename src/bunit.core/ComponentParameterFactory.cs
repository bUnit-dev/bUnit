using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Bunit
{
	/// <summary>
	/// <see cref="ComponentParameter"/> factory methods.
	/// </summary>
	public static class ComponentParameterFactory
	{
		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback(string name, Action callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback(string name, Action<object> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback(string name, Func<Task> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback(string name, Func<object, Task> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <typeparam name="TValue">The value returned in the <see cref="Microsoft.AspNetCore.Components.EventCallback{TValue}"/>.</typeparam>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback<TValue>(string name, Action callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback<TValue>(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <typeparam name="TValue">The value returned in the <see cref="Microsoft.AspNetCore.Components.EventCallback{TValue}"/>.</typeparam>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback<TValue>(string name, Action<TValue> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback<TValue>(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <typeparam name="TValue">The value returned in the <see cref="Microsoft.AspNetCore.Components.EventCallback{TValue}"/>.</typeparam>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback<TValue>(string name, Func<Task> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback<TValue>(receiver: null, callback));
		}

		/// <summary>
		/// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> that will call the provided <paramref name="callback"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="callback">The event callback.</param>
		/// <typeparam name="TValue">The value returned in the <see cref="Microsoft.AspNetCore.Components.EventCallback{TValue}"/>.</typeparam>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter EventCallback<TValue>(string name, Func<TValue, Task> callback)
		{
			return ComponentParameter.CreateParameter(name, new EventCallback<TValue>(receiver: null, callback));
		}

		/// <summary>
		/// Creates a component parameter which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Value or null of the parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter Parameter(string name, object? value)
		{
			return ComponentParameter.CreateParameter(name, value);
		}

		/// <summary>
		/// Creates a cascading value which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="value">Value of the parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter CascadingValue(string name, object value)
		{
			return ComponentParameter.CreateCascadingValue(name, value);
		}

		/// <summary>
		/// Creates a cascading value which can be passed to a test contexts render methods.
		/// </summary>
		/// <param name="value">Value of the parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter CascadingValue(object value)
		{
			return ComponentParameter.CreateCascadingValue(name: null, value);
		}

		/// <summary>
		/// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided
		/// <paramref name="markup"/> as rendered output.
		/// </summary>
		/// <param name="markup">Markup to pass to the child content parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter ChildContent(string markup)
		{
			return RenderFragment(nameof(ChildContent), markup);
		}

		/// <summary>
		/// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
		/// with the provided <paramref name="parameters"/> as input.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/>.</typeparam>
		/// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter ChildContent<TComponent>(params ComponentParameter[] parameters)
		    where TComponent : class, IComponent
		{
			return RenderFragment<TComponent>(nameof(ChildContent), parameters);
		}

		/// <summary>
		/// Creates a ChildContent parameter that will pass the provided <paramref name="renderFragment"/>
		/// to the parameter in the component.
		/// </summary>
		/// <param name="renderFragment">The <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> to pass to the ChildContent parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter ChildContent(RenderFragment renderFragment)
		{
			return Parameter(nameof(ChildContent), renderFragment);
		}

		/// <summary>
		/// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided
		/// <paramref name="markup"/> as rendered output and passes it to the parameter specified in <paramref name="name"/>.
		/// </summary>
		/// <param name="name">Parameter name.</param>
		/// <param name="markup">Markup to pass to the render fragment parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter RenderFragment(string name, string markup)
		{
			return ComponentParameter.CreateParameter(name, markup.ToMarkupRenderFragment());
		}

		/// <summary>
		/// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
		/// with the provided <paramref name="parameters"/> as input, and passes it to the parameter specified in <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/>.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter RenderFragment<TComponent>(string name, params ComponentParameter[] parameters)
		    where TComponent : class, IComponent
		{
			var cpc = new ComponentParameterCollection() { parameters };
			return ComponentParameter.CreateParameter(name, cpc.ToRenderFragment<TComponent>());
		}

		/// <summary>
		/// Creates a template component parameter which will pass the <paramref name="template"/> <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
		/// to the parameter with the name <paramref name="name"/>.
		/// </summary>
		/// <typeparam name="TValue">The value used to build the content.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="template"><see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" /> to pass to the parameter.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter Template<TValue>(string name, RenderFragment<TValue> template)
		{
			return ComponentParameter.CreateParameter(name, template);
		}

		/// <summary>
		/// Creates a template component parameter which will pass the a <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
		/// to the parameter with the name <paramref name="name"/>.
		/// The <paramref name="markupFactory"/> will be used to generate the markup inside the template.
		/// </summary>
		/// <typeparam name="TValue">The value used to build the content.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="markupFactory">A markup factory that takes a <typeparamref name="TValue"/> as input and returns markup/HTML.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter Template<TValue>(string name, Func<TValue, string> markupFactory)
		{
			return Template<TValue>(name, value => (RenderTreeBuilder builder) => builder.AddMarkupContent(0, markupFactory(value)));
		}

		/// <summary>
		/// Creates a template component parameter which will pass the a <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
		/// to the <paramref name="parameterCollectionBuilder"/> at runtime. The parameters returned from it
		/// will be passed to the <typeparamref name="TComponent"/> and it will be rendered as the template.
		/// </summary>
		/// <typeparam name="TComponent">The type of component to render in template.</typeparam>
		/// <typeparam name="TValue">The value used to build the content.</typeparam>
		/// <param name="name">Parameter name.</param>
		/// <param name="parameterCollectionBuilder">The parameter collection builder function that will be passed the template <typeparamref name="TValue"/>.</param>
		/// <returns>The <see cref="ComponentParameter"/>.</returns>
		public static ComponentParameter Template<TComponent, TValue>(string name, Func<TValue, ComponentParameter[]> parameterCollectionBuilder)
			where TComponent : IComponent
		{
			return Template<TValue>(name, value =>
			{
				var cpc = new ComponentParameterCollection() { parameterCollectionBuilder(value) };
				return cpc.ToRenderFragment<TComponent>();
			});
		}
	}
}
