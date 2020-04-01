using System;
using System.Threading.Tasks;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using EC = Microsoft.AspNetCore.Components.EventCallback;

namespace Bunit
{
	/// <summary>
	/// Base class for test classes that contains XUnit Razor component tests.
	/// </summary>
	public abstract class ComponentTestFixture : TestContext
    {
        /// <summary>
        /// Wait for the next render to happen, or the <paramref name="timeout"/> is reached (default is one second).
        /// If a <paramref name="renderTrigger"/> action is provided, it is invoked before the waiting.
        /// </summary>
        /// <param name="renderTrigger">The action that somehow causes one or more components to render.</param>
        /// <param name="timeout">The maximum time to wait for the next render. If not provided the default is 1 second. During debugging, the timeout is automatically set to infinite.</param>        
        /// <exception cref="WaitForRenderFailedException">Thrown if no render happens within the specified <paramref name="timeout"/>, or the default of 1 second, if non is specified.</exception>
        [Obsolete("Use either the WaitForState or WaitForAssertion method instead. It will make your test more resilient to insignificant changes, as they will wait across multiple renders instead of just one. To make the change, run any render trigger first, then call either WaitForState or WaitForAssertion with the appropriate input. This method will be removed before the 1.0.0 release.", false)]
        protected void WaitForNextRender(Action? renderTrigger = null, TimeSpan? timeout = null)
            => RenderWaitingHelperExtensions.WaitForNextRender(this, renderTrigger, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="statePredicate"/> action returns true,
        /// or the <paramref name="timeout"/> is reached (default is one second).
        /// 
        /// The <paramref name="statePredicate"/> is evaluated initially, and then each time
        /// the renderer in the test context renders.
        /// </summary>
        /// <param name="statePredicate">The predicate to invoke after each render, which returns true when the desired state has been reached.</param>
        /// <param name="timeout">The maximum time to wait for the desired state.</param>
        /// <exception cref="WaitForStateFailedException">Thrown if the <paramref name="statePredicate"/> throw an exception during invocation, or if the timeout has been reached. See the inner exception for details.</exception>
        protected void WaitForState(Func<bool> statePredicate, TimeSpan? timeout = null)
            => RenderWaitingHelperExtensions.WaitForState(this, statePredicate, timeout);

        /// <summary>
        /// Wait until the provided <paramref name="assertion"/> action passes (i.e. does not throw an 
        /// assertion exception), or the <paramref name="timeout"/> is reached (default is one second).
        /// 
        /// The <paramref name="assertion"/> is attempted initially, and then each time
        /// the renderer in the test context renders.
        /// </summary>
        /// <param name="assertion">The verification or assertion to perform.</param>
        /// <param name="timeout">The maximum time to attempt the verification.</param>
        /// <exception cref="WaitForAssertionFailedException">Thrown if the timeout has been reached. See the inner exception to see the captured assertion exception.</exception>
        protected void WaitForAssertion(Action assertion, TimeSpan? timeout = null)
            => RenderWaitingHelperExtensions.WaitForAssertion(this, assertion, timeout);

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
        /// Creates a template component parameter which will pass the <paramref name="template"/> <see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" />
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
        {
            return Template<TValue>(name, value => (RenderTreeBuilder builder) => builder.AddMarkupContent(0, markupFactory(value)));
        }
    }
}
