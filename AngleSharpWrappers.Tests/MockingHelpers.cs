using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Moq;

namespace AngleSharpWrappers
{
    /// <summary>
    /// Helper methods for creating mocks.
    /// </summary>
    public static class MockingHelpers
    {
        private static readonly MethodInfo MockOfInfo = typeof(Mock)
            .GetMethods()
            .Where(x => x.Name == nameof(Mock.Of))
            .First(x => x.GetParameters().Length == 0);

        private static readonly Type DelegateType = typeof(MulticastDelegate);
        private static readonly Type StringType = typeof(string);

        /// <summary>
        /// Creates a mock instance of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to create a mock of.</param>
        /// <returns>An instance of <paramref name="type"/>.</returns>
        public static object ToMockInstance(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if (type.IsMockable())
            {
                var result = MockOfInfo.MakeGenericMethod(type).Invoke(null, Array.Empty<object>());

                if (result is null)
                    throw new NotSupportedException($"Cannot create an mock of {type.FullName}.");

                return result;
            }
            else if (type.Equals(StringType))
            {
                return new Mock<string>();
            }
            else
            {
                throw new NotSupportedException($"Cannot create an mock of {type.FullName}. Type to mock must be an interface, a delegate, or a non-sealed, non-static class.");
            }
        }

        /// <summary>
        /// Creates a mock instance of <paramref name="type"/>.
        /// </summary>
        /// <param name="type">Type to create a mock of.</param>
        /// <returns>An instance of <paramref name="type"/>.</returns>
        public static object ToMock(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if (type.IsMockable())
            {
                var constructed = typeof(Mock<>).MakeGenericType(type);
                var result = Activator.CreateInstance(constructed);

                if (result is null)
                    throw new NotSupportedException($"Cannot create an mock of {type.FullName}.");

                return result;
            }
            else if (type.Equals(StringType))
            {
                return new Mock<string>();
            }
            else
            {
                throw new NotSupportedException($"Cannot create an mock of {type.FullName}. Type to mock must be an interface, a delegate, or a non-sealed, non-static class.");
            }
        }

        /// <summary>
        /// Gets whether a type is mockable by <see cref="Moq"/>.
        /// </summary>
        public static bool IsMockable(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            if (type.IsSealed)
                return type.IsDelegateType();
            return true;
        }

        /// <summary>
        /// Gets whether a type is a delegate type.
        /// </summary>
        public static bool IsDelegateType(this Type type)
        {
            return Equals(type, DelegateType);
        }

        /// <summary>
        /// Create a default instance of the <paramref name="type"/>.
        /// </summary>
        /// <returns>The default value</returns>
        public static object? GetDefault(this Type type)
        {
            if (type.GetTypeInfo().IsValueType)
                return Activator.CreateInstance(type);
            return null;
        }

        /// <summary>
        /// Gets all methods (and property methods) of an interface, and any interfaces it implements.
        /// </summary>
        public static List<MethodInfo> GetInterfaceMethods(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new List<MethodInfo>();

            foreach (var mi in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                result.Add(mi);
            }

            foreach (var baseType in type.GetInterfaces())
            {
                result.AddRange(baseType.GetInterfaceMethods());
            }

            return result;
        }

        public static List<PropertyInfo> GetInterfaceProperties(this Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new List<PropertyInfo>();

            foreach (var mi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                result.Add(mi);
            }

            foreach (var baseType in type.GetInterfaces())
            {
                result.AddRange(GetInterfaceProperties(baseType));
            }

            return result;
        }

        /// <summary>
        /// Creates a parameter array containing a default or mocked instance of all parameters a method takes when invoked.
        /// </summary>
        public static object?[] CreateMethodArguments(this MethodInfo method)
        {
            if (method is null) throw new ArgumentNullException(nameof(method));

            var parameters = method.GetParameters();
            return parameters.Length == 0
                ? Array.Empty<object?>()
                : parameters.Select(p => p.ParameterType.IsMockable() ? p.ParameterType.ToMockInstance() : p.ParameterType.GetDefault()).ToArray();
        }

        public static bool IsIndexerPropertyMethod(this MethodInfo method)
        {
            if (method.DeclaringType is null) return false;
            var indexerProperty = GetIndexerProperty(method.DeclaringType);

            if (indexerProperty is null) return false;

            return method == indexerProperty.GetMethod || method == indexerProperty.SetMethod;
        }

        private static PropertyInfo? GetIndexerProperty(this Type type)
        {
            var defaultPropertyAttribute = type.GetCustomAttributes<DefaultMemberAttribute>().FirstOrDefault();

            if (defaultPropertyAttribute is null) return null;

            //return type.GetProperties(defaultPropertyAttribute.MemberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return type.GetProperties().FirstOrDefault(x => x.Name == defaultPropertyAttribute.MemberName);
        }
    }
}
