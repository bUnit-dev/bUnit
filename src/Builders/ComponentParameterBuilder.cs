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
    /// A builder to set a value for strongly typed ComponentParameters.
    /// </summary>
    /// <typeparam name="TComponent">The type of component under test to add the parameters</typeparam>
    public sealed class ComponentParameterBuilder<TComponent> where TComponent : class, IComponent
    {
        private const string ParameterNameChildContent = "ChildContent";
        private readonly List<ComponentParameter> _componentParameters = new List<ComponentParameter>();

        /// <summary>
        /// Add an unnamed attribute value for a component under test.
        /// </summary>
        /// <param name="key">The value to set for an unnamed cascading parameter</param>
        /// <param name="value">The value to set for an unnamed cascading parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
        public ComponentParameterBuilder<TComponent> Add(string key, object value)
        {
            return AddParameterToList(key, value, false);
        }

        /// <summary>
        /// Add an unnamed cascading value for a component under test.
        /// </summary>
        /// <param name="value">The value to set for an unnamed cascading parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
        public ComponentParameterBuilder<TComponent> Add(object value)
        {
            return AddParameterToList(null, value, true);
        }

        /// <summary>
        /// Add a strongly typed parameter with a value for a component under test.
        /// </summary>
        /// <typeparam name="TValue">The generic value type</typeparam>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="value">The value, which cannot be null in case of cascading parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
        public ComponentParameterBuilder<TComponent> Add<TValue>(Expression<Func<TComponent, TValue>> parameterSelector, [AllowNull] TValue value)
        {
            if (parameterSelector is null)
            {
                throw new ArgumentNullException(nameof(parameterSelector));
            }

            var (name, isCascading) = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(name, value, isCascading);
        }

        /// <summary>
        /// Add a strongly typed <see cref="RenderFragment" /> parameter with a html markup value for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="markup">Markup to render as output</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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

            var (name, isCascading) = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(name, markup.ToMarkupRenderFragment(), isCascading);
        }

        /// <summary>
        /// Add a strongly typed <see cref="RenderFragment{TValue}" /> parameter with a template for a component under test.
        /// </summary>
        /// <typeparam name="TValue">The generic value type</typeparam>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="template"><see cref="RenderFragment{TValue}" /> to pass to the parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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

            var (name, isCascading) = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(name, template, isCascading);
        }

        /// <summary>
        /// Add a strongly typed <see cref="RenderFragment{TValue}" /> parameter with a markupFactory for a component under test.
        /// </summary>
        /// <typeparam name="TValue">The generic value type</typeparam>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="markupFactory">A markup factory that takes a <typeparamref name="TValue"/> as input and returns markup/HTML.</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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
        /// Add a strongly typed <see cref="EventCallback" /> parameter with a callback for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="callback">A callback that returns a <see cref="Task"/>.</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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
        /// Add a strongly typed <see cref="EventCallback{TValue}" /> parameter with a callback for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="callback">A callback that takes a <typeparamref name="TValue"/> and returns a <see cref="Task"/>.</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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

            var (name, isCascading) = GetDetailsFromExpression(parameterSelector);
            return AddParameterToList(name, EC.Factory.Create(this, callback), isCascading);
        }

        /// <summary>
        /// Add a strongly typed <see cref="RenderFragment" /> parameter with a <see cref="ComponentParameterBuilder{TChildComponent}"/> for a component under test.
        /// </summary>
        /// <param name="parameterSelector">The parameter selector which defines the parameter to add</param>
        /// <param name="childBuilderAction">The builder action for the child component.</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
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

            var (name, isCascading) = GetDetailsFromExpression(parameterSelector);

            var childComponentParameterBuilder = new ComponentParameterBuilder<TChildComponent>();
            childBuilderAction(childComponentParameterBuilder);

            var childFragment = childComponentParameterBuilder.Build().ToList().ToComponentRenderFragment<TChildComponent>();
            return AddParameterToList(name, childFragment, isCascading);
        }

        /// <summary>
        /// Add a <see cref="ComponentParameterBuilder{TChildComponent}"/> to build a ChildContent parameter.
        /// </summary>
        /// <param name="childBuilderAction">The builder action for the child component.</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
        public ComponentParameterBuilder<TComponent> AddChildContent<TChildComponent>(Action<ComponentParameterBuilder<TChildComponent>> childBuilderAction) where TChildComponent : class, IComponent
        {
            if (childBuilderAction is null)
            {
                throw new ArgumentNullException(nameof(childBuilderAction));
            }

            var (name, isCascading) = GetChildContentParameterDetails();

            var childComponentParameterBuilder = new ComponentParameterBuilder<TChildComponent>();
            childBuilderAction(childComponentParameterBuilder);

            var childFragment = childComponentParameterBuilder.Build().ToList().ToComponentRenderFragment<TChildComponent>();
            return AddParameterToList(name, childFragment, isCascading);
        }

        /// <summary>
        /// Add a child component markup for a ChildContent parameter.
        /// </summary>
        /// <param name="markup">Markup to render as output for the ChildContent parameter</param>
        /// <returns>A <see cref="ComponentParameterBuilder{TComponent}"/> which can be chained</returns>
        public ComponentParameterBuilder<TComponent> AddChildContent(string markup)
        {
            if (markup is null)
            {
                throw new ArgumentNullException(nameof(markup));
            }

            var (name, isCascading) = GetChildContentParameterDetails();
            return AddParameterToList(name, markup.ToMarkupRenderFragment(), isCascading);
        }

        /// <summary>
        /// Create a <see cref="IReadOnlyList{ComponentParameter}"/>.
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
                    // The CascadingParameterAttribute is defined and has a valid name, get the defined name from this attribute and indicate that it's a cascading property
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