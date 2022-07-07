#if NET5_0_OR_GREATER
using System;
using System.Diagnostics.CodeAnalysis;
using AutoFixture.Xunit2;
using Microsoft.JSInterop;
using Xunit;

namespace Bunit.JSInterop;

public partial class JSRuntimeUnhandledInvocationExceptionTest
{
	[Theory(DisplayName = "Message prints correctly when trying to import an unconfigured module")]
	[AutoData]
	public void Test036(string moduleName)
	{
		var identifier = "import";
		var returnType = typeof(IJSObjectReference);
		var invocationMethodName = "InvokeAsync";
		var expectedErrorMessage = CreateExpectedErrorMessage(
			$"{CodeIdent}{invocationMethodName}<{returnType.Name}>(\"{identifier}\", \"{moduleName}\")",
			$"{CodeIdent}SetupModule(\"{moduleName}\")");

		var sut = new JSRuntimeUnhandledInvocationException(new JSRuntimeInvocation(identifier, new object?[] { moduleName }, returnType, invocationMethodName));

		Assert.Equal(expectedErrorMessage, sut.Message);
	}
}
#endif
