using System;
using System.Reflection;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Bunit.RazorTesting
{
	public static class Mocks
	{
		public static TestMethod TestMethod(Type type, string methodName, ITestCollection? collection = null)
		{
			var @class = TestClass(type, collection);
			var methodInfo = type.GetMethod(methodName);
			if (methodInfo == null)
				throw new Exception($"Unknown method: {type.FullName}.{methodName}");

			return new TestMethod(@class, Reflector.Wrap(methodInfo));
		}

		public static TestClass TestClass(Type type, ITestCollection? collection = null)
		{
			if (collection == null)
				collection = TestCollection(type.GetTypeInfo().Assembly);

			return new TestClass(collection, Reflector.Wrap(type));
		}

		public static TestCollection TestCollection(Assembly? assembly = null, ITypeInfo? definition = null, string? displayName = null)
		{
			if (assembly == null)
				assembly = typeof(Mocks).GetTypeInfo().Assembly;
			if (displayName == null)
				displayName = "Mock test collection for " + assembly.Location;

			return new TestCollection(TestAssembly(assembly), definition, displayName);
		}

		public static TestAssembly TestAssembly(Assembly? assembly = null, string? configFileName = null)
		{
			return new TestAssembly(Reflector.Wrap(assembly ?? typeof(Mocks).GetTypeInfo().Assembly), configFileName);
		}
	}
}
