using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Moq;
using Shouldly;
using Xunit;

namespace AngleSharpWrappers
{
    public class WrapperTest
    {
        private static Type WrapperType = typeof(IWrapper);
        private static MethodInfo ObjectFuncMethod = typeof(WrapperTest).GetMethod(nameof(WrapperTest.GetObject))!;
        private static Dictionary<Type, MethodInfo> GetObjectFuncs = new Dictionary<Type, MethodInfo>();

        public static Func<T> GetObject<T>(T source) => () => source;

        private static MethodInfo GetObjectFuncMethod(Type type)
        {
            if (GetObjectFuncs.TryGetValue(type, out var result))
                return result;
            else
            {
                result = ObjectFuncMethod.MakeGenericMethod(type);
                GetObjectFuncs.Add(type, result);
            }
            return result;
        }

        private static ConstructorInfo GetConstructor(Type wrapper) => wrapper.GetConstructors().Single();

        private static object CreateSut(Type wrapper, Type wrappedType, dynamic wrapped)
        {
            var func = GetObjectFuncMethod(wrappedType).Invoke(null, new object[] { wrapped });
            var sut = GetConstructor(wrapper).Invoke(new object[] { func });
            return sut;
        }

        public static IEnumerable<object[]> GetWrapperMethods()
        {
            var allWrappers = WrapperType.Assembly.GetTypes().Where(x => WrapperType.IsAssignableFrom(x) && !x.IsGenericType && !x.IsInterface).ToList();

            foreach (var wrapper in allWrappers)
            {
                var wrappedTypeName = $"I{wrapper.Name.Replace("Wrapper", "", StringComparison.Ordinal)}";
                var wrappedType = wrapper.GetInterface(wrappedTypeName)!;

                var methods = wrappedType.GetInterfaceMethods().Where(x => !x.IsSpecialName && !x.Name.StartsWith("GetEnumerator") && !x.IsIndexerPropertyMethod()).ToArray();
                foreach (var method in methods)
                {
                    yield return new object[] { wrapper, wrappedType, method };
                }

                var properties = wrappedType.GetInterfaceProperties();
                foreach (var prop in properties)
                {
                    if (prop.CanRead)
                    {
                        yield return new object[] { wrapper, wrappedType, prop.GetGetMethod() };
                    }
                    if (prop.CanWrite)
                    {
                        yield return new object[] { wrapper, wrappedType, prop.GetSetMethod() };
                    }
                }

            }
        }

        [Theory]
        [MemberData(nameof(GetWrapperMethods))]
        public void ForwardMethodCalls(Type wrapper, Type wrappedType, MethodInfo method)
        {
            dynamic mockWrapped = wrappedType.ToMock();
            var wrapped = mockWrapped.Object;
            IInvocationList invocations = mockWrapped.Invocations;
            object sut = CreateSut(wrapper, wrappedType, wrapped);
            var args = method.CreateMethodArguments();

            method.Invoke(sut, args);

            invocations.Count.ShouldBe(1, $"Failed on {wrapper.Name} with {method.Name}");
            invocations[0].Method.ShouldBe(method);
            invocations[0].Arguments.ShouldBe(args);
        }
    }
}
