using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using EC = Microsoft.AspNetCore.Components.EventCallback;

namespace Bunit
{
    /// <summary>
    /// A builder which makes it possible to add typed ComponentParameters.
    /// </summary>
    /// <typeparam name="TComponent">The type of component to render</typeparam>
    public sealed class ComponentParameterBuilder<TComponent> where TComponent : class, IComponent
    {
        private const string ParameterNameChildContent = "ChildContent";
        private readonly List<ComponentParameter> _componentParameters = new List<ComponentParameter>();

        /// <summary>
        /// Add a property selector with a value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The generic Value type</typeparam>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="value">The value, which cannot be null in case of cascading parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, value, details.isCascading);
        }

        /// <summary>
        /// Add a property selector with a string value to this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="markup"/> as rendered output and passes it to the parameter specified in <paramref name="parameterSelector"/>.
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, RenderFragment?>> parameterSelector, string markup)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (markup is null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, markup.ToMarkupRenderFragment(), details.isCascading);
        }

        /// <summary>
        /// Add a property selector with a value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The value used to build the content.</typeparam>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="template"><see cref="Microsoft.AspNetCore.Components.RenderFragment{TValue}" /> to pass to the parameter.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, RenderFragment<TValue>?>> parameterSelector, RenderFragment<TValue> template)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (template is null)
            {
                throw new ArgumentNullException(nameof(template));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, template, details.isCascading);
        }

        /// <summary>
        /// Add a property selector with a value to this builder.
        /// </summary>
        /// <typeparam name="TValue">The value used to build the content.</typeparam>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="markupFactory">A markup factory that takes a <typeparamref name="TValue"/> as input and returns markup/HTML.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, RenderFragment<TValue>?>> parameterSelector, Func<TValue, string> markupFactory)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (markupFactory is null)
            {
                throw new ArgumentNullException(nameof(markupFactory));
            }

            return Add(parameterSelector, value => renderTreeBuilder => renderTreeBuilder.AddMarkupContent(0, markupFactory(value)));
        }

        /// <summary>
        /// Add a non generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add(Expression<Func<TComponent, EC>> parameterSelector, Func<Task> callback)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            return Add(parameterSelector, EC.Factory.Create(this, callback));
        }

        /// <summary>
        /// Add a generic <see cref="EventCallback"/> parameter with a value for this builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="callback">The event callback.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, EventCallback<TValue>>> parameterSelector, Func<TValue, Task> callback)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (callback is null)
            {
                throw new ArgumentNullException(nameof(callback));
            }

            var details = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(details.name, EC.Factory.Create(this, callback), details.isCascading);
        }

        /// <summary>
        /// Add a RenderFragment parameter using a child component builder.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector</param>
        /// <param name="childBuilderAction">The builder action for the child component.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> Add<TChildComponent>(Expression<Func<TComponent, RenderFragment?>> parameterSelector, Action<ComponentParameterBuilder<TChildComponent>> childBuilderAction) where TChildComponent : class, IComponent
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            if (childBuilderAction is null)
            {
                throw new ArgumentNullException(nameof(childBuilderAction));
            }

            var details = GetDetailsFromExpression(parameterSelector);

            var childComponentParameterBuilder = new ComponentParameterBuilder<TChildComponent>();
            childBuilderAction(childComponentParameterBuilder);

            var childFragment = childComponentParameterBuilder.Build().ToComponentRenderFragment<TChildComponent>();
            return AddParameterToList(details.name, childFragment, details.isCascading);
        }

        /// <summary>
        /// Add a child component builder for a ChildContent parameter.
        /// </summary>
        /// <param name="childBuilderAction">The builder action for the child component.</param>
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> AddChildContent<TChildComponent>(Action<ComponentParameterBuilder<TChildComponent>> childBuilderAction) where TChildComponent : class, IComponent
        {
            if (childBuilderAction is null)
            {
                throw new ArgumentNullException(nameof(childBuilderAction));
            }

            var details = GetChildContentParameterDetails();

            var childComponentParameterBuilder = new ComponentParameterBuilder<TChildComponent>();
            childBuilderAction(childComponentParameterBuilder);

            var childFragment = childComponentParameterBuilder.Build().ToComponentRenderFragment<TChildComponent>();
            return AddParameterToList(details.name, childFragment, details.isCascading);
        }

        /// <summary>
        /// Add a child component builder markup for a ChildContent parameter.
        /// </summary>
        /// <param name="markup"/> as rendered output and passes it to the ChildContent parameter./>.
        /// <returns>A <see cref="ComponentParameterBuilder&lt;TComponent&gt;"/> which can be chained.</returns>
        public ComponentParameterBuilder<TComponent> AddChildContent(string markup)
        {
            if (markup is null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            var details = GetChildContentParameterDetails();
            return AddParameterToList(details.name, markup.ToMarkupRenderFragment(), details.isCascading);
        }

        /// <summary>
        /// Create a <see cref="ComponentParameter"/> list.
        /// </summary>
        /// <returns>A ISet of <see cref="ComponentParameter"/></returns>
        public IReadOnlyList<ComponentParameter> Build()
        {
            return _componentParameters;
        }

        private static (string name, bool isCascading) GetChildContentParameterDetails()
        {
            var propertyInfo = typeof(TComponent).GetProperty(ParameterNameChildContent);
            if (propertyInfo is null || (propertyInfo.PropertyType != typeof(RenderFragment)))
            {
                throw new ArgumentException($"No public property with the name '{ParameterNameChildContent}' and type {typeof(RenderFragment).Name} is defined on the component '{typeof(TComponent)}'.");
            }

            if (!TryGetDetailsFromPropertyInfo(propertyInfo, out string name, out bool isCascading))
            {
                throw new ArgumentException($"The public property with the name '{ParameterNameChildContent}' does not have the [Parameter] or [CascadingParameter] attribute defined in the component '{typeof(TComponent)}'.");
            }

            return (name, isCascading);
        }

        private static (string name, bool isCascading) GetDetailsFromExpression<T>(Expression<Func<TComponent, T>> parameterSelector)
        {
            if (parameterSelector.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            {
                if (!TryGetDetailsFromPropertyInfo(propertyInfo, out string name, out bool isCascading))
                {
                    throw new ArgumentException($"The public property '{propertyInfo.Name}' selected by the provided '{parameterSelector}' does not have the [Parameter] or [CascadingParameter] attribute defined in the component '{typeof(TComponent)}'.");
                }

                return (name, isCascading);
            }

            throw new ArgumentException($"The parameterSelector '{parameterSelector}' does not resolve to a public property on the component '{typeof(TComponent)}'.");
        }

        private static bool TryGetDetailsFromPropertyInfo(PropertyInfo propertyInfo, out string name, out bool isCascading)
        {
            var parameterAttribute = propertyInfo.GetCustomAttribute<ParameterAttribute>();
            if (parameterAttribute is { })
            {
                // If ParameterAttribute is defined, get the name from the property and indicate that it's a normal property
                name = propertyInfo.Name;
                isCascading = false;
                return true;
            }

            var cascadingParameterAttribute = propertyInfo.GetCustomAttribute<CascadingParameterAttribute>();
            if (cascadingParameterAttribute is { })
            {
                if (!string.IsNullOrEmpty(cascadingParameterAttribute.Name))
                {
                    // The CascadingParameterAttribute is defined, get the defined name from this attribute and indicate that it's a cascading property
                    name = cascadingParameterAttribute.Name;
                    isCascading = true;
                    return true;
                }

                // The CascadingParameterAttribute is defined, get the name from the property and indicate that it's a cascading property
                name = propertyInfo.Name;
                isCascading = true;
                return true;
            }

            // If both attributes are missing, return false
            name = string.Empty;
            isCascading = default;
            return false;
        }

        private ComponentParameterBuilder<TComponent> AddParameterToList(string? name, object? value, bool isCascading)
        {
            if (_componentParameters.All(cp => cp.Name != name))
            {
                _componentParameters.Add((name, value, isCascading));

                return this;
            }

            throw new ArgumentException($"A parameter with the name '{name}' has already been added to the {nameof(ComponentParameterBuilder<TComponent>)}.");
        }
    }
}