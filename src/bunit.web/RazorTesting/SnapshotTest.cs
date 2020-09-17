using System;
using System.Threading.Tasks;
using Bunit.Diffing;
using Bunit.Extensions;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A component used to create snapshot tests.
	/// Snapshot tests takes two child inputs, a TestInput section and a ExpectedOutput section.
	/// It then compares the result of rendering both using semantic HTML comparison.
	/// </summary>
	public class SnapshotTest : RazorTestBase
	{
		/// <inheritdoc/>
		public override string? DisplayName => Description;

		/// <summary>
		/// Sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
		/// is rendered and compared.
		/// </summary>
		[Parameter] public Action<SnapshotTest>? Setup { get; set; }

		/// <summary>
		/// Sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
		/// is rendered and compared.
		/// </summary>
		[Parameter] public Func<SnapshotTest, Task>? SetupAsync { get; set; }

		/// <summary>
		/// Gets or sets the input to the snapshot test.
		/// </summary>
		[Parameter] public RenderFragment? TestInput { get; set; }

		/// <summary>
		/// Gets or sets the expected output of the snapshot test.
		/// </summary>
		[Parameter] public RenderFragment? ExpectedOutput { get; set; }

		/// <inheritdoc/>
		protected override async Task Run()
		{
			Validate();

			Services.AddDefaultTestContextServices();

			if (Setup is { })
				TryRun(Setup, this);
			if (SetupAsync is { })
				await TryRunAsync(SetupAsync, this).ConfigureAwait(false);

			var renderedTestInput = (IRenderedFragment)Renderer.RenderFragment(TestInput!);
			var inputHtml = renderedTestInput.Markup;

			var renderedExpectedRender = (IRenderedFragment)Renderer.RenderFragment(ExpectedOutput!);
			var expectedHtml = renderedExpectedRender.Markup;

			VerifySnapshot(inputHtml, expectedHtml);
		}

		private void VerifySnapshot(string inputHtml, string expectedHtml)
		{
			using var parser = new HtmlParser();
			var inputNodes = parser.Parse(inputHtml);
			var expectedNodes = parser.Parse(expectedHtml);

			var diffs = inputNodes.CompareTo(expectedNodes);

			if (diffs.Count > 0)
				throw new HtmlEqualException(diffs, expectedNodes, inputNodes, Description ?? "Snapshot test failed.");
		}

		/// <inheritdoc/>
		public override Task SetParametersAsync(ParameterView parameters)
		{
			Setup = parameters.GetValueOrDefault<Action<SnapshotTest>>(nameof(Setup));
			SetupAsync = parameters.GetValueOrDefault<Func<SnapshotTest, Task>>(nameof(SetupAsync));
			TestInput = parameters.GetValueOrDefault<RenderFragment>(nameof(TestInput));
			ExpectedOutput = parameters.GetValueOrDefault<RenderFragment>(nameof(ExpectedOutput));
			return base.SetParametersAsync(parameters);
		}

		/// <inheritdoc/>
		public override void Validate()
		{
			base.Validate();
			if (TestInput is null)
				throw new ArgumentException($"No {nameof(TestInput)} specified in the {nameof(SnapshotTest)} component.");
			if (ExpectedOutput is null)
				throw new ArgumentException($"No {nameof(ExpectedOutput)} specified in the {nameof(SnapshotTest)} component.");
		}
	}
}
