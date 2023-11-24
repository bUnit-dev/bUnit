using Bunit.Web.AngleSharp;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using System.Reflection;

namespace Bunit;

[UsesVerify]
public class WrapperElementsGeneratorTest
{
	[Fact]
	public Task Generator()
	{
		var inputCompilation = CreateCompilation();
		var generator = new WrapperElementsGenerator();

		GeneratorDriver driver = CSharpGeneratorDriver.Create(generator);
		driver = driver.RunGenerators(inputCompilation);

		var settings = new VerifySettings();
		settings.AutoVerify();
		return Verifier.Verify(driver.GetRunResult(), settings);
	}

	private static Compilation CreateCompilation()
	{
		return CSharpCompilation.Create(
			assemblyName: "compilation",
			syntaxTrees: null,
			references: new[]
			{
				MetadataReference.CreateFromFile(typeof(Binder).GetTypeInfo().Assembly.Location),
				MetadataReference.CreateFromFile(typeof(AngleSharp.BrowsingContext).Assembly.Location),
			},
			options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
	}
}
