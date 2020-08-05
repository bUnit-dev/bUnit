using System;
using System.Threading.Tasks;
using Bunit.Rendering;

using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// Represents a rendered component-under-test. 
	/// </summary>
	/// <typeparam name="TComponent">The type of the component under test</typeparam>
	public interface IRenderedComponentBase<TComponent> : IRenderedFragmentBase where TComponent : IComponent
	{
		/// <summary>
		/// Gets the component under test
		/// </summary>
		TComponent Instance { get; }

		/// <summary>
		/// Invokes the given <paramref name="callback"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="callback"></param>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		Task InvokeAsync(Action callback);

		/// <summary>
		/// Invokes the given <paramref name="callback"/> in the context of the associated <see cref="ITestRenderer"/>.
		/// </summary>
		/// <param name="callback"></param>
		/// <returns>A <see cref="Task"/> that will be completed when the action has finished executing.</returns>
		Task InvokeAsync(Func<Task> callback);

		/// <summary>
		/// Render the component under test again.
		/// </summary>
		void Render();

		/// <summary>
		/// Render the component under test again with the provided <paramref name="parameters"/>.
		/// </summary>
		/// <param name="parameters">Parameters to pass to the component upon rendered</param>
		void SetParametersAndRender(ParameterView parameters);

		/// <summary>
		/// Render the component under test again with the provided <paramref name="parameters"/>.
		/// </summary>
		/// <param name="parameters">Parameters to pass to the component upon rendered</param>
		void SetParametersAndRender(params ComponentParameter[] parameters);

		/// <summary>
		/// Render the component under test again with the provided parameters from the <paramref name="parameterBuilder"/>.
		/// </summary>
		/// <param name="parameterBuilder">An action that receives a <see cref="ComponentParameterBuilder{TComponent}"/>.</param>
		void SetParametersAndRender(Action<ComponentParameterBuilder<TComponent>> parameterBuilder);
	}
}
