using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit.RazorTesting
{
	/// <summary>
	/// Represents a component used to define tests in Razor files.
	/// </summary>
	public abstract class RazorTest : FragmentBase
	{
		/// <summary>
		/// A description or name for the test that will be displayed if the test fails.
		/// </summary>
		[Parameter] public string? Description { get; set; }

		/// <summary>
		/// Gets or sets a reason for skipping the test. If not set (null), the test will not be skipped.
		/// </summary>
		[Parameter] public string? Skip { get; set; }

		/// <summary>
		/// Run the test logic of the <see cref="RazorTest"/>.
		/// </summary>
		/// <returns></returns>
		public abstract Task RunTest();

		/// <inheritdoc/>
		public override string ToString() => $"{Description ?? "[no description]"}";
	}
}
