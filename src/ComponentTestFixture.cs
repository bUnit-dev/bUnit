using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EC = Microsoft.AspNetCore.Components.EventCallback;

namespace Bunit
{
    /// <summary>
    /// Base class for test classes that contains XUnit Razor component tests.
    /// </summary>
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
        protected ComponentParameter EventCallback(string name, Action callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback(string name, Action<object> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback(string name, Func<Task> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback(string name, Func<object, Task> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback<TValue>(string name, Action callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create<TValue>(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback<TValue>(string name, Action<TValue> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create<TValue>(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback<TValue>(string name, Func<Task> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create<TValue>(this, callback));
        }

        /// <summary>
        /// Creates a <see cref="ComponentParameter"/> with an <see cref="Microsoft.AspNetCore.Components.EventCallback"/> as parameter value for this <see cref="TestContext"/> and
        /// <paramref name="callback"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected ComponentParameter EventCallback<TValue>(string name, Func<TValue, Task> callback)
        {
            return ComponentParameter.CreateParameter(name, EC.Factory.Create<TValue>(this, callback));
        }

        /// <summary>
        /// Creates a component parameter which can be passed to a test contexts render methods.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Value or null of the parameter</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter Parameter(string name, object? value)
        {
            return ComponentParameter.CreateParameter(name, value);
        }

        /// <summary>
        /// Creates a cascading value which can be passed to a test contexts render methods.
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="value">Value of the parameter</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter CascadingValue(string name, object value)
        {
            return ComponentParameter.CreateCascadingValue(name, value);
        }

        /// <summary>
        /// Creates a cascading value which can be passed to a test contexts render methods.
        /// </summary>
        /// <param name="value">Value of the parameter</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter CascadingValue(object value)
        {
            return ComponentParameter.CreateCascadingValue(null, value);
        }

        /// <summary>
        /// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided 
        /// <paramref name="markup"/> as rendered output.
        /// </summary>
        /// <param name="markup">Markup to pass to the child content parameter</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter ChildContent(string markup)
        {
            return RenderFragment(nameof(ChildContent), markup);
        }

        /// <summary>
        /// Creates a ChildContent <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
        /// with the provided <paramref name="parameters"/> as input.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/></typeparam>
        /// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter ChildContent<TComponent>(params ComponentParameter[] parameters) where TComponent : class, IComponent
        {
            return RenderFragment<TComponent>(nameof(ChildContent), parameters);
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> with the provided 
        /// <paramref name="markup"/> as rendered output and passes it to the parameter specified in <paramref name="name"/>.
        /// </summary>
        /// <param name="name">Parameter name.</param>
        /// <param name="markup">Markup to pass to the render fragment parameter</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter RenderFragment(string name, string markup)
        {
            return ComponentParameter.CreateParameter(name, markup.ToMarkupRenderFragment());
        }

        /// <summary>
        /// Creates a <see cref="Microsoft.AspNetCore.Components.RenderFragment"/> which will render a <typeparamref name="TComponent"/> component
        /// with the provided <paramref name="parameters"/> as input, and passes it to the parameter specified in <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TComponent">The type of the component to render with the <see cref="Microsoft.AspNetCore.Components.RenderFragment"/></typeparam>
        /// <param name="name">Parameter name.</param>
        /// <param name="parameters">Parameters to pass to the <typeparamref name="TComponent"/>.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter RenderFragment<TComponent>(string name, params ComponentParameter[] parameters) where TComponent : class, IComponent
        {
            return ComponentParameter.CreateParameter(name, parameters.ToComponentRenderFragment<TComponent>());
        }

        /// <summary>
        /// Creates a component parameter which will pass the <paramref name="template"/> <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
        /// to the parameter with the name <paramref name="name"/>.
        /// </summary>
        /// <typeparam name="TValue">The value used to build the content.</typeparam>
        /// <param name="name">Parameter name.</param>
        /// <param name="template"><see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" /> to pass to the parameter.</param>
        /// <returns>The <see cref="ComponentParameter"/>.</returns>
        protected static ComponentParameter Template<TValue>(string name, RenderFragment<TValue> template)
        {
            return ComponentParameter.CreateParameter(name, template);
        }
    }
}
