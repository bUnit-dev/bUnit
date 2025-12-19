using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Xunit;
using VerifyCS = Microsoft.CodeAnalysis.CSharp.Testing.XUnit.AnalyzerVerifier<Bunit.Analyzers.PreferGenericFindAnalyzer>;

namespace Bunit.Analyzers.Tests;

public class PreferGenericFindAnalyzerTests
{
	[Fact]
	public async Task NoDiagnostic_WhenUsingGenericFind()
	{
		const string code = @"
namespace TestNamespace
{
	public class TestClass
	{
		public void TestMethod()
		{
			var cut = new TestHelper();
			var elem = cut.Find<string>(""a"");
		}
	}

	public class TestHelper
	{
		public T Find<T>(string selector) => default(T);
	}
}";

		await VerifyCS.VerifyAnalyzerAsync(code);
	}

	[Fact]
	public async Task NoDiagnostic_WhenCastingNonFindMethod()
	{
		const string code = @"
namespace TestNamespace
{
	public class TestClass
	{
		public void TestMethod()
		{
			var obj = new TestHelper();
			var elem = (string)obj.GetSomething();
		}
	}

	public class TestHelper
	{
		public object GetSomething() => null;
	}
}";

		await VerifyCS.VerifyAnalyzerAsync(code);
	}

	[Fact]
	public async Task NoDiagnostic_WhenFindIsNotFromRenderedFragment()
	{
		const string code = @"
namespace TestNamespace
{
	public class TestClass
	{
		public void TestMethod()
		{
			var helper = new UnrelatedHelper();
			var result = (string)helper.Find(""test"");
		}
	}

	public class UnrelatedHelper
	{
		public object Find(string selector) => null;
	}
}";

		await VerifyCS.VerifyAnalyzerAsync(code);
	}

	[Fact]
	public async Task Diagnostic_WhenCastingFindResultFromIRenderedFragment()
	{
		const string code = @"
namespace TestNamespace
{
	public interface IMyElement { }

	public class TestClass
	{
		public void TestMethod()
		{
			var cut = new MockRenderedFragment();
			IMyElement elem = {|#0:(IMyElement)cut.Find(""a"")|};
		}
	}

	public interface IRenderedFragment
	{
		object Find(string selector);
	}

	public class MockRenderedFragment : IRenderedFragment
	{
		public object Find(string selector) => null;
	}
}";

		var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.PreferGenericFind.Id)
			.WithLocation(0)
			.WithArguments("IMyElement", "\"a\"");

		await VerifyCS.VerifyAnalyzerAsync(code, expected);
	}

	[Fact]
	public async Task Diagnostic_WhenCastingFindResultFromRenderedComponent()
	{
		const string code = @"
namespace TestNamespace
{
	public interface IMyElement { }

	public class TestClass
	{
		public void TestMethod()
		{
			var cut = new MockRenderedComponent();
			var elem = {|#0:(IMyElement)cut.Find(""div"")|};
		}
	}

	public interface IRenderedComponent
	{
		object Find(string selector);
	}

	public class MockRenderedComponent : IRenderedComponent
	{
		public object Find(string selector) => null;
	}
}";

		var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.PreferGenericFind.Id)
			.WithLocation(0)
			.WithArguments("IMyElement", "\"div\"");

		await VerifyCS.VerifyAnalyzerAsync(code, expected);
	}

	[Fact]
	public async Task Diagnostic_WhenCastingFindResultFromRenderedFragmentType()
	{
		const string code = @"
namespace TestNamespace
{
	public interface IMyElement { }

	public class TestClass
	{
		public void TestMethod()
		{
			var cut = new RenderedFragment();
			var button = {|#0:(IMyElement)cut.Find(""button"")|};
		}
	}

	public class RenderedFragment
	{
		public object Find(string selector) => null;
	}
}";

		var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.PreferGenericFind.Id)
			.WithLocation(0)
			.WithArguments("IMyElement", "\"button\"");

		await VerifyCS.VerifyAnalyzerAsync(code, expected);
	}

	[Fact]
	public async Task Diagnostic_WithComplexSelector()
	{
		const string code = @"
namespace TestNamespace
{
	public interface IMyElement { }

	public class TestClass
	{
		public void TestMethod()
		{
			var cut = new MockRenderedFragment();
			var link = {|#0:(IMyElement)cut.Find(""a.nav-link[href='/home']"")|};
		}
	}

	public interface IRenderedFragment
	{
		object Find(string selector);
	}

	public class MockRenderedFragment : IRenderedFragment
	{
		public object Find(string selector) => null;
	}
}";

		var expected = VerifyCS.Diagnostic(DiagnosticDescriptors.PreferGenericFind.Id)
			.WithLocation(0)
			.WithArguments("IMyElement", "\"a.nav-link[href='/home']\"");

		await VerifyCS.VerifyAnalyzerAsync(code, expected);
	}
}
