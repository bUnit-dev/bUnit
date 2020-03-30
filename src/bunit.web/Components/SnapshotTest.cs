using System;
using System.Threading.Tasks;
using Bunit.RazorTesting;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
	/// <summary>
	/// A component used to create snapshot tests.
	/// Snapshot tests takes two sets of child components, a <see cref="TestInput"/> component
	/// for declaring the test input, and an <see cref="ExpectedOutput"/> component for
	/// declaring the expected output, the test input should produce.
	/// </summary>
	public class SnapshotTest : RazorTest
	{
		private Action _setup = NoopTestMethod;
		private Func<Task> _setupAsync = NoopTestMethodAsync;	

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
		/// is rendered and compared.
		/// </summary>
		[Parameter] public Action Setup { get => _setup; set => _setup = value ?? NoopTestMethod; }

		/// <summary>
		/// Gets or sets the setup action to perform before the <see cref="TestInput"/> and <see cref="ExpectedOutput"/>
		/// is rendered and compared.
		/// </summary>
		[Parameter] public Func<Task> SetupAsync { get => _setupAsync; set => _setupAsync = value ?? NoopTestMethodAsync; }

		/// <inheritdoc/>
		public override Task RunTest() => throw new NotImplementedException();
	}

	/// <summary>
	/// Represents the test input in a snapshot test (<see cref="SnapshotTest"/>).
	/// </summary>
	public class TestInput : FragmentBase { }

	/// <summary>
	/// Represents the expected output in a snapshot test (<see cref="SnapshotTest"/>).
	/// </summary>
	public class ExpectedOutput : FragmentBase { }
}
