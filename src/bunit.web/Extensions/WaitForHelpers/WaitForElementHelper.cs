using System;
using System.Threading.Tasks;
using AngleSharp.Dom;

namespace Bunit.Extensions.WaitForHelpers
{
	/// <summary>
	/// Represents an async wait helper, that will wait for a specified time for element(s) to become available in the DOM.
	/// </summary>
	internal class WaitForElementHelper : WaitForHelper<IElement>
	{
		internal const string TimeoutBeforeFoundMessage = "The CSS selector and/or predicate did not result in matching element(s) before the timeout period passed.";
		internal const string ExceptionInPredicateMessage = "The element(s) predicate throw an unhandled exception.";

		/// <inheritdoc/>
		protected override string? TimeoutErrorMessage => TimeoutBeforeFoundMessage;

		/// <inheritdoc/>
		protected override string? CheckThrowErrorMessage => ExceptionInPredicateMessage;

		/// <inheritdoc/>
		protected override bool StopWaitingOnCheckException => false;

		public WaitForElementHelper(IRenderedFragment renderedFragment, string cssSelector, TimeSpan? timeout = null)
			: base(renderedFragment, () =>
			{
				var element = renderedFragment.Find(cssSelector);
				return (true, element); 
			}, timeout)
		{
		}
	}
}
