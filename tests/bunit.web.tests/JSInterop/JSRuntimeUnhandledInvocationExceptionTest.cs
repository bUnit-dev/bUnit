using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.Xunit2;
using Microsoft.JSInterop;
using Xunit;

namespace Bunit.JSInterop
{
	[SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter should not span multiple lines", Justification = "Makes error message easier to read.")]
	[SuppressMessage("Design", "CA1034:Nested types should not be visible", Justification = "Dummy types are visible for testing purposes.")]
	public class JSRuntimeUnhandledInvocationExceptionTest
	{
		private const string CodeIdent = "    ";

		private static string CreateExpectedErrorMessage(string invocationMethod, string suggestedSetup)
		{
			return $"bUnit's JSInterop has not been configured to handle the call:{Environment.NewLine}{Environment.NewLine}" +
				$"{invocationMethod}{Environment.NewLine}{Environment.NewLine}" +
				$"Configure bUnit's JSInterop to handle the call with following:{Environment.NewLine}{Environment.NewLine}" +
				$"{suggestedSetup}{Environment.NewLine}{Environment.NewLine}" +
				$"The setup methods are available on an instance of the BunitJSInterop or{Environment.NewLine}" +
				$"BunitJSModuleInterop type. The standard BunitJSInterop is available{Environment.NewLine}" +
				$"through the TestContext.JSInterop property, and a BunitJSModuleInterop{Environment.NewLine}" +
				$"instance is returned from calling SetupModule on a BunitJSInterop instance.{Environment.NewLine}";
		}

		[Theory(DisplayName = "Message prints correctly with void return type")]
		[InlineAutoData("InvokeVoidAsync")]
		[InlineAutoData("InvokeVoid")]
		public void Test001(string identifier, string invocationMethodName)
		{
			var exectedErrorMessage = CreateExpectedErrorMessage(
					$"{CodeIdent}{invocationMethodName}(\"{identifier}\")",
					$"{CodeIdent}SetupVoid(\"{identifier}\")");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, typeof(object), invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with non-primitive return type")]
		[InlineAutoData("InvokeAsync")]
		[InlineAutoData("InvokeUnmarshalled")]
		public void Test002(string identifier, string invocationMethodName)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var exectedErrorMessage = CreateExpectedErrorMessage(
					$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\")",
					$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\")");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with primitive return type")]
		[InlineData(typeof(bool), "bool")]
		[InlineData(typeof(byte), "byte")]
		[InlineData(typeof(sbyte), "sbyte")]
		[InlineData(typeof(short), "short")]
		[InlineData(typeof(ushort), "ushort")]
		[InlineData(typeof(int), "int")]
		[InlineData(typeof(uint), "uint")]
		[InlineData(typeof(long), "long")]
		[InlineData(typeof(ulong), "ulong")]
		[InlineData(typeof(char), "char")]
		[InlineData(typeof(double), "double")]
		[InlineData(typeof(float), "float")]
		public void Test003(Type returnType, string returnTypeName)
		{
			var identifier = "func";
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
					$"{CodeIdent}{invocationMethodName}<{returnTypeName}>(\"{identifier}\")",
					$"{CodeIdent}Setup<{returnTypeName}>(\"{identifier}\")");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and string argument")]
		[InlineAutoData("InvokeVoidAsync")]
		[InlineAutoData("InvokeVoid")]
		public void Test011(string identifier, string arg0, string invocationMethodName)
		{
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}(\"{identifier}\", \"{arg0}\")",
				$"{CodeIdent}SetupVoid(\"{identifier}\", \"{arg0}\"){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}SetupVoid(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0 }, typeof(object), invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and multiple string arguments")]
		[InlineAutoData("InvokeVoidAsync")]
		[InlineAutoData("InvokeVoid")]
		public void Test012(string identifier, string arg0, string arg1, string invocationMethodName)
		{
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}(\"{identifier}\", \"{arg0}\", \"{arg1}\")",
				$"{CodeIdent}SetupVoid(\"{identifier}\", \"{arg0}\", \"{arg1}\"){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}SetupVoid(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0, arg1 }, typeof(object), invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and multiple non-string arguments")]
		[InlineAutoData("InvokeVoidAsync")]
		[InlineAutoData("InvokeVoid")]
		public void Test013(string identifier, bool arg0, object arg1, string invocationMethodName)
		{
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}(\"{identifier}\", {arg0}, {arg1})",
				$"{CodeIdent}SetupVoid(\"{identifier}\", {arg0}, {arg1}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}SetupVoid(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0, arg1 }, typeof(object), invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and null argument")]
		[InlineAutoData("InvokeVoidAsync")]
		[InlineAutoData("InvokeVoid")]
		public void Test014(string identifier, string invocationMethodName)
		{
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}(\"{identifier}\", null)",
				$"{CodeIdent}SetupVoid(\"{identifier}\", null){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}SetupVoid(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { null }, typeof(object), invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with return type and string argument")]
		[AutoData]
		public void Test021(string identifier, string arg0)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", \"{arg0}\")",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", \"{arg0}\"){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and multiple string arguments")]
		[AutoData]
		public void Test022(string identifier, string arg0, string arg1)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", \"{arg0}\", \"{arg1}\")",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", \"{arg0}\", \"{arg1}\"){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0, arg1 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and multiple non-string arguments")]
		[AutoData]
		public void Test023(string identifier, bool arg0, object arg1)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", {arg0}, {arg1})",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", {arg0}, {arg1}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0, arg1 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly with void return type and null argument")]
		[AutoData]
		public void Test024(string identifier)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", null)",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", null){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { null }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints generic arguments of InvokeUnmarshalled with one argument correctly")]
		[AutoData]
		public void Test031(string identifier, Dummy1 arg0)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeUnmarshalled";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<Dummy1, {returnType.Name}>(\"{identifier}\", {arg0})",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", {arg0}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new[] { arg0 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints generic arguments of InvokeUnmarshalled with two arguments correctly")]
		[AutoData]
		public void Test032(string identifier, Dummy1 arg0, Dummy2 arg1)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeUnmarshalled";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<Dummy1, Dummy2, {returnType.Name}>(\"{identifier}\", {arg0}, {arg1})",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", {arg0}, {arg1}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object[] { arg0, arg1 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints generic arguments of InvokeUnmarshalled with three arguments correctly")]
		[AutoData]
		public void Test033(string identifier, Dummy1 arg0, Dummy2 arg1, Dummy3 arg2)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeUnmarshalled";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<Dummy1, Dummy2, Dummy3, {returnType.Name}>(\"{identifier}\", {arg0}, {arg1}, {arg2})",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", {arg0}, {arg1}, {arg2}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object[] { arg0, arg1, arg2 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints generic arguments of InvokeUnmarshalled with primitive arguments correctly")]
		[AutoData]
		public void Test034(string identifier, int arg0)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeUnmarshalled";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<int, {returnType.Name}>(\"{identifier}\", {arg0})",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", {arg0}){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { arg0 }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints generic arguments as '?' of InvokeUnmarshalled when matching argument is null")]
		[AutoData]
		public void Test035(string identifier)
		{
			var returnType = typeof(JSRuntimeUnhandledInvocationExceptionTest);
			var invocationMethodName = "InvokeUnmarshalled";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<?, {returnType.Name}>(\"{identifier}\", null)",
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", null){Environment.NewLine}" +
				$"or the following, to match any arguments:{Environment.NewLine}" +
				$"{CodeIdent}Setup<{returnType.Name}>(\"{identifier}\", _ => true)");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { null }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		[Theory(DisplayName = "Message prints correctly when trying to import an unconfigured module")]
		[AutoData]
		public void Test036(string moduleName)
		{
			var identifier = "import";
			var returnType = typeof(IJSObjectReference);
			var invocationMethodName = "InvokeAsync";
			var exectedErrorMessage = CreateExpectedErrorMessage(
				$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", \"{moduleName}\")",
				$"{CodeIdent}SetupModule(\"{moduleName}\")");

			var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { moduleName }, returnType, invocationMethodName));

			Assert.Equal(exectedErrorMessage, sut.Message);
		}

		public class Dummy1 { }
		public class Dummy2 { }
		public class Dummy3 { }
	}
}
