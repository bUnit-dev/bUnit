using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Moq;
using Shouldly;
using Xunit;

namespace AngleSharpWrappers
{
    public class WrapperTest
    {
        private static readonly Type WrapperType = typeof(IWrapper);
        private static readonly MethodInfo ObjectFuncMethod = typeof(WrapperTest).GetMethod(nameof(GetObject)) ?? throw new InvalidOperationException($"{nameof(GetObject)} not found on type");
        private static readonly MethodInfo TestEventHandlerMethod = typeof(WrapperTest).GetMethod(nameof(DummyDomEventHandler)) ?? throw new InvalidOperationException($"{nameof(DummyDomEventHandler)} not found on type");
        private static readonly Dictionary<Type, MethodInfo> GetObjectFuncs = new Dictionary<Type, MethodInfo>();
        private static readonly List<Type> AllWrapperTypes = WrapperType.Assembly.GetTypes().Where(x => WrapperType.IsAssignableFrom(x) && !x.IsGenericType && !x.IsInterface).ToList();

        public static void DummyDomEventHandler(object sender, AngleSharp.Dom.Events.Event ev) { }

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

        public static IEnumerable<object[]> GetWrapperMethods()
        {
            foreach (var wrapper in AllWrapperTypes)
            {
                var wrappedTypeName = $"I{wrapper.Name.Replace("Wrapper", "", StringComparison.Ordinal)}";
                var wrappedType = wrapper.GetInterface(wrappedTypeName)!;

                var methods = wrappedType.GetInterfaceMethods()
                    .Where(x => !x.IsSpecialName) // remove properties
                    .Where(x => !x.IsIndexerPropertyMethod()) // remove indexers
                    .Where(x => !x.Name.StartsWith("GetEnumerator", StringComparison.Ordinal))
                    .ToArray();

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

        public static IEnumerable<object[]> GetWrapperEvents()
        {
            foreach (var wrapper in AllWrapperTypes)
            {
                var wrappedTypeName = $"I{wrapper.Name.Replace("Wrapper", "", StringComparison.Ordinal)}";
                var wrappedType = wrapper.GetInterface(wrappedTypeName)!;
                var eventNames = wrappedType.GetInterfaceEvents().Select(x => x.Name).Distinct();
                foreach (var evt in eventNames)
                {
                    yield return new object[] { wrapper, wrappedType, evt };
                }
            }
        }

        private static object CreateSut(Type wrapper, Type wrappedType, dynamic wrapped)
        {
            var func = GetObjectFuncMethod(wrappedType).Invoke(null, new object[] { wrapped });
            var sut = GetConstructor(wrapper).Invoke(new object[] { func });
            return sut;
        }

        private static (object sut, IInvocationList invocations) SetupSut(Type wrapper, Type wrappedType)
        {
            dynamic mockWrapped = wrappedType.ToMock();
            var wrapped = mockWrapped.Object;
            IInvocationList invocations = mockWrapped.Invocations;
            var sut = CreateSut(wrapper, wrappedType, wrapped);
            return (sut, invocations);
        }

        [Theory]
        [MemberData(nameof(GetWrapperMethods))]
        public void ForwardMethodCalls(Type wrapper, Type wrappedType, MethodInfo method)
        {
            var (sut, invocations) = SetupSut(wrapper, wrappedType);
            var args = method.CreateMethodArguments();

            method.Invoke(sut, args);

            invocations.Count.ShouldBe(1, $"Failed on {wrapper.Name} with {method.Name}");
            invocations[0].Method.ShouldBe(method);
            invocations[0].Arguments.ShouldBe(args);
        }

        [Theory]
        [MemberData(nameof(GetWrapperEvents))]
        public void ForwardEventSubscriptionsAndUnsubscriptions(Type wrapper, Type wrappedType, string eventName)
        {
            var eventInfo = wrapper.GetEvent(eventName) ?? throw new InvalidOperationException($"Event {eventName} not found on {wrapper.Name}");
            dynamic mockWrapped = wrappedType.ToMock();
            var wrapped = mockWrapped.Object;
            mockWrapped.SetupAdd((Action<dynamic>)(x => AddSubscriptions(x, eventName)));
            mockWrapped.SetupRemove((Action<dynamic>)(x => RemoveSubscritpion(x, eventName)));
            var sut = CreateSut(wrapper, wrappedType, wrapped);

            SubscribeAndUnsubscribeToEvent(sut, eventInfo);

            mockWrapped.VerifyAdd((Action<dynamic>)(x => AddSubscriptions(x, eventName)));
            mockWrapped.VerifyRemove((Action<dynamic>)(x => RemoveSubscritpion(x, eventName)));

            void SubscribeAndUnsubscribeToEvent(dynamic wrapper, EventInfo eventInfo)
            {
                var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, TestEventHandlerMethod);
                eventInfo.AddEventHandler(wrapper, handler);
                eventInfo.RemoveEventHandler(wrapper, handler);
            }

            void AddSubscriptions(dynamic target, string eventName)
            {
                var eventInfo = target.GetType().GetEvent(eventName) ?? throw new InvalidOperationException($"{eventName} not found on type");
                var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, TestEventHandlerMethod);
                eventInfo.AddEventHandler(target, handler);
            }

            void RemoveSubscritpion(dynamic target, string eventName)
            {
                var eventInfo = target.GetType().GetEvent(eventName) ?? throw new InvalidOperationException($"{eventName} not found on type");
                var handler = Delegate.CreateDelegate(eventInfo.EventHandlerType, null, TestEventHandlerMethod);
                eventInfo.RemoveEventHandler(target, handler);
            }
        }


    }
}
