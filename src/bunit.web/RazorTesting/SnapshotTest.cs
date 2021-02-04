using System;
using System.Threading.Tasks;
using Bunit.Extensions;
using Bunit.RazorTesting;
using Bunit.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace Bunit
{
	/// <summary>
	/// A component used to create snapshot tests.
	/// Snapshot tests takes two child inputs, a TestInput section and a ExpectedOutput section.
	/// It then compares the result of rendering both using semantic HTML comparison.
	/// </summary>
	public class SnapshotTest : RazorTestBase
	{
		/// <summary>
		/// Gets bUnits JSInterop, that allows setting up handlers for <see cref="IJSRuntime.InvokeAsync{TValue}(string, object[])"/> invocations
		/// that components under tests will issue during testing. It also makes it possible to verify that the invocations has happened as expected.
		/// </summary>
		public BunitJSInterop JSInterop { get; } = new BunitJSInterop();

		/// <inheritdoc/>
		public override string? DisplayName => Description;

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
		/// is rendered and compared.
		/// </summary>
		[Parameter] public Action<SnapshotTest>? Setup { get; set; }

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
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

		/// <summary>
		/// Initializes a new instance of the <see cref="SnapshotTest"/> class.
		/// </summary>
		public SnapshotTest()
		{
			Services.AddDefaultTestContextServices(this, JSInterop);
		}

		/// <inheritdoc/>
		protected override async Task RunAsync()
		{
			Validate();

			if (Setup is not null)
				TryRun(Setup, this);
			if (SetupAsync is not null)
				await TryRunAsync(SetupAsync, this).ConfigureAwait(false);

			var renderedTestInput = this.RenderInsideRenderTree(TestInput!);
			var inputHtml = renderedTestInput.Markup;

			var renderedExpectedRender = this.RenderInsideRenderTree(ExpectedOutput!);
			var expectedHtml = renderedExpectedRender.Markup;

			VerifySnapshot(inputHtml, expectedHtml);
		}

		private void VerifySnapshot(string inputHtml, string expectedHtml)
		{
			var parser = Services.GetRequiredService<BunitHtmlParser>();
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
