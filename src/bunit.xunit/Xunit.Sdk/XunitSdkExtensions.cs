using System;
using Xunit.Abstractions;

namespace Bunit
{
	internal static class XunitSdkExtensions
    {
        public static Type GetTestComponentType(this ITestMethod testMethod) => testMethod.TestClass.Class.ToRuntimeType();
    }
}
