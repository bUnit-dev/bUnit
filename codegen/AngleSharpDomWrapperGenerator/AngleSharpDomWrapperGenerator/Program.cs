using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using AngleSharp.Common;
using AngleSharp.Dom;
using AngleSharp.Svg.Dom;
using AngleSharp.Text;

namespace AngleSharpDomWrapperGenerator
{
    class Program
    {
        static void Main()
        {
            var _ = new Program();
        }

        private const char space = ' ';

        private Dictionary<string, Type> WrappedTypes { get; }

        public Program()
        {
            var inodeType = typeof(INode);
            var enumerableType = typeof(System.Collections.IEnumerable);
            var iattrType = typeof(IAttr);
            var allInterfaces = inodeType.Assembly.GetTypes().Where(x => x.IsInterface).ToList();

            WrappedTypes = allInterfaces.Where(x => inodeType.IsAssignableFrom(x) || enumerableType.IsAssignableFrom(x)).ToDictionary(x => GetTypeKey(x), x => x);

            WrappedTypes.Add(iattrType.FullName!, iattrType);

            foreach (var item in WrappedTypes.Values)
            {
                GenerateWrapper(item);
            }

            GenerateGetOrWrapMethods(WrappedTypes.Values);
            GenerateFactoryMethods(WrappedTypes.Values);
        }

        // public static partial class WrapperFactoryFactory
        // {
        //     static WrapperFactoryFactory()
        //     {
        //         Wrappers.Add(typeof(IElement), (WrapperFactory<IElement>)CreateElementWrapper);
        //     }
        //        
        //     /// <summary>
        //     /// Creates a wrapper for an <see cref="IElement"/>.
        //     /// </summary>
        //     /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        //     /// <returns>The <see cref="ElementWrapper"/>.</returns>
        //     public static ElementWrapper CreateElementWrapper(Func<IElement?> objectQuery) => new ElementWrapper(objectQuery);
        // }
        private void GenerateFactoryMethods(IEnumerable<Type> types)
        {
            var output = new StringBuilder();
            var usings = new List<Type>(types) { typeof(Func<>), typeof(Dictionary<,>) };

            output.AppendLine();
            output.AppendLine("namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers");
            output.AppendLine("{");
            output.AppendLine($"{space,4}#nullable enable");
            output.AppendLine($"{space,4}public static partial class WrapperFactoryFactory");
            output.AppendLine($"{space,4}{{");
            output.AppendLine($"{space,8}static WrapperFactoryFactory()");
            output.AppendLine($"{space,8}{{");
            foreach (var type in types.Where(x => !x.IsGenericType))
            {
                output.AppendLine($"{space,12}Wrappers.Add(typeof({type.Name}), (WrapperFactory<{type.Name}>)Create{GetWrapperClassName(type)});");
            }
            output.AppendLine($"{space,8}}}");
            foreach (var type in types.Where(x => !x.IsGenericType))
            {
                var wrapperClassName = GetWrapperClassName(type);
                output.AppendLine();
                output.AppendLine($"{space,8}");
                output.AppendLine($"{space,8}/// <summary>");
                output.AppendLine($"{space,8}/// Creates a wrapper for an <see cref=\"{type.Name}\"/>.");
                output.AppendLine($"{space,8}/// </summary>");
                output.AppendLine($"{space,8}/// <param name=\"objectQuery\">A function that can be used to retrieve a new instance of the wrapped type.</param>");
                output.AppendLine($"{space,8}/// <returns>The <see cref=\"{wrapperClassName}\"/>.</returns>");
                output.AppendLine($"{space,8}public static {wrapperClassName} Create{wrapperClassName}(Func<{type.Name}?> objectQuery) => new {wrapperClassName}(objectQuery);");
            }
            output.AppendLine($"{space,4}}}");
            output.AppendLine("}");


            AddUsingStatements(usings, output);

            Console.WriteLine($"Writing WrapperFactoryFactory.g.cs");

            File.WriteAllText($"WrapperFactoryFactory.g.cs", output.ToString());
        }

        private void GenerateGetOrWrapMethods(IEnumerable<Type> wrappedTypes)
        {
            var output = new StringBuilder();
            var usings = new List<Type>() { typeof(Func<>) };

            output.AppendLine();
            output.AppendLine("namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers");
            output.AppendLine("{");
            output.AppendLine($"{space,4}#nullable enable");
            output.AppendLine($"{space,4}public abstract partial class Wrapper<T> : IWrapper where T : class");
            output.AppendLine($"{space,4}{{");

            var getOrWrapTypes = wrappedTypes.Where(x => x.FullName is { }).OrderBy(x => x.Name).ToDictionary(x => x.FullName, x => x);

            foreach (var type in getOrWrapTypes.Values.Where(x => !x.IsGenericType))
            {
                var wrapperName = $"{type.Name[1..]}Wrapper";
                output.AppendLine($"{space,8}/// <summary>");
                output.AppendLine($"{space,8}/// Gets (or wraps) the requested nested object.");
                output.AppendLine($"{space,8}/// </summary>");
                output.AppendLine($"{space,8}/// <param name=\"key\">Key to look up the requested wrapped object.</param>");
                output.AppendLine($"{space,8}/// <param name=\"objectQuery\">A function that can be used to retrieve a new instance of the wrapped type.</param>");
                output.AppendLine($"{space,8}/// <returns>The object wrapped in a wrapper.</returns>");
                output.AppendLine($"{space,8}protected {type.Name} GetOrWrap(int key, Func<{type.Name}?> objectQuery)");
                output.AppendLine($"{space,8}{{");
                output.AppendLine($"{space,12}if (!Wrappers.TryGetValue(key, out var result))");
                output.AppendLine($"{space,12}{{");
                output.AppendLine($"{space,16}result = new {wrapperName}(objectQuery);");
                output.AppendLine($"{space,16}Wrappers.Add(key, result);");
                output.AppendLine($"{space,12}}}");
                output.AppendLine($"{space,12}return ({type.Name})result;");
                output.AppendLine($"{space,8}}}");
                usings.Add(type);
                usings.AddRange(type.GetGenericArguments());
            }

            // /// <summary>
            // /// Gets (or wraps) the requested nested object.
            // /// </summary>
            // /// <param name="key">Key to look up the requested wrapped object.</param>
            // /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
            // /// <returns>The object wrapped in a wrapper.</returns>
            // protected IHtmlCollection<TWrapped> GetOrWrap<TWrapped>(int key, Func<IHtmlCollection<TWrapped>?> objectQuery) where TWrapped : class, IElement
            // {
            //     if (!Wrappers.TryGetValue(key, out var result))
            //     {
            //         result = new HtmlCollectionWrapper<TWrapped>(objectQuery);
            //         Wrappers.Add(key, result);
            //     }
            //     return (IHtmlCollection<TWrapped>)result;
            // }
            foreach (var type in getOrWrapTypes.Values.Where(x => x.IsGenericType && x.GetGenericArguments().Any(y => y.IsGenericParameter)))
            {
                var typeName = GetInterfaceTypeName(type);
                var paramType = type.GetGenericArguments().Single().GetGenericParameterConstraints().Single();
                var wrapperName = GetWrapperClassName(type);
                output.AppendLine($"{space,8}/// <summary>");
                output.AppendLine($"{space,8}/// Gets (or wraps) the requested nested object.");
                output.AppendLine($"{space,8}/// </summary>");
                output.AppendLine($"{space,8}/// <param name=\"key\">Key to look up the requested wrapped object.</param>");
                output.AppendLine($"{space,8}/// <param name=\"objectQuery\">A function that can be used to retrieve a new instance of the wrapped type.</param>");
                output.AppendLine($"{space,8}/// <returns>The object wrapped in a wrapper.</returns>");
                output.AppendLine($"{space,8}protected {typeName}<TWrapped> GetOrWrap<TWrapped>(int key, Func<{typeName}<TWrapped>?> objectQuery) where TWrapped : class, {paramType.Name}");
                output.AppendLine($"{space,8}{{");
                output.AppendLine($"{space,12}if (!Wrappers.TryGetValue(key, out var result))");
                output.AppendLine($"{space,12}{{");
                output.AppendLine($"{space,16}result = new {wrapperName}<TWrapped>(objectQuery);");
                output.AppendLine($"{space,16}Wrappers.Add(key, result);");
                output.AppendLine($"{space,12}}}");
                output.AppendLine($"{space,12}return ({typeName}<TWrapped>)result;");
                output.AppendLine($"{space,8}}}");
                usings.Add(type);
                usings.AddRange(type.GetGenericArguments());
            }


            output.AppendLine($"{space,4}}}");
            output.AppendLine("}");

            AddUsingStatements(usings, output);

            Console.WriteLine($"Writing Wrapper.g.cs");

            File.WriteAllText($"Wrapper.g.cs", output.ToString());
        }

        private void GenerateWrapper(Type type)
        {
            var usings = new List<Type>();
            var output = new StringBuilder();

            usings.AddRange(CreateClassStart(type, output));
            usings.AddRange(CreateEvents(type, output));
            usings.AddRange(CreatePropertiesAndIndexers(type, output));
            usings.AddRange(CreateMethods(type, output));
            usings.AddRange(CreateEnumerableMethods(type, output));
            CreateClassEnd(output);
            AddUsingStatements(usings, output);

            WriteClassToFile(type, output);
        }

        private static void AddUsingStatements(List<Type> usings, StringBuilder output) => output.Insert(0, GenerateUsings(usings));

        private static void WriteClassToFile(Type type, StringBuilder output)
        {
            var wrapperName = GetWrapperClassName(type);
            Console.WriteLine($"Writing {wrapperName}.g.cs");
            File.WriteAllText($"{wrapperName}.g.cs", output.ToString());
        }

        private static IEnumerable<Type> CreateClassStart(Type type, StringBuilder output)
        {
            var name = GetInterfaceTypeName(type);
            var wrapperName = GetWrapperClassName(type);
            var genericArgs = GetGenericArgsList(type);
            var genericConstraints = GetGenericTypeConstraints(type);

            output.AppendLine();
            output.AppendLine();
            output.AppendLine("namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers");
            output.AppendLine("{");
            output.AppendLine($"{space,4}#nullable enable");

            output.AppendLine($"{space,4}/// <summary>");
            output.AppendLine($"{space,4}/// Represents a wrapper class around <see cref=\"{name}{genericArgs.Replace('<', '{').Replace('>', '}')}\"/> type.");
            output.AppendLine($"{space,4}/// </summary>");
            output.AppendLine($"{space,4}public partial class {wrapperName}{genericArgs} : Wrapper<{name}{genericArgs}>, {name}{genericArgs}, IWrapper");
            if (!string.IsNullOrEmpty(genericConstraints))
                output.AppendLine($"{space,8}{genericConstraints}");
            output.AppendLine($"{space,4}{{");

            if (type.IsGenericType && type.GetGenericArguments().Any(x => x.IsGenericParameter))
            {
                var genericParam = type.GetGenericArguments().Single(x => x.IsGenericParameter);
                output.AppendLine($"{space,8}private static readonly WrapperFactory<{genericParam.Name}> ItemWrapper = WrapperFactoryFactory.Create<{genericParam.Name}>();");
                output.AppendLine();
            }

            output.AppendLine($"{space,8}/// <summary>");
            output.AppendLine($"{space,8}/// Creates an instance of the <see cref=\"{wrapperName}{genericArgs.Replace('<', '{').Replace('>', '}')}\"/> type;");
            output.AppendLine($"{space,8}/// </summary>");
            output.AppendLine($"{space,8}/// <param name=\"getObject\">A function that can be used to retrieve a new instance of the wrapped type.</param>");
            output.AppendLine($"{space,8}public {wrapperName}(Func<{name}{genericArgs}?> getObject) : base(getObject) {{ }}");

            yield return type;
            foreach (var t in type.GetGenericArguments()) yield return t;
            foreach (var t in type.GetGenericArguments().SelectMany(x => x.GetGenericParameterConstraints())) yield return t;
        }

        private static void CreateClassEnd(StringBuilder output)
        {
            output.AppendLine($"{space,4}}}");
            output.AppendLine("}");
        }

        private static IEnumerable<Type> CreateEvents(Type type, StringBuilder output)
        {
            var events = GetEvents(type).GroupBy(x => x.Name).OrderBy(x => x.Key);
            foreach (var evtGroup in events)
            {
                var evt = evtGroup.First();
                output.AppendLine();
                var evtType = GetReturnType(evt.EventHandlerType);

                var castType = evtGroup.Count() > 1
                    ? $"(({evt.DeclaringType!.Name})WrappedObject)"
                    : "WrappedObject";

                output.AppendLine($"{space,8}/// <inheritdoc/>");
                output.AppendLine($"{space,8}public event {evtType} {evt.Name} {{ add => {castType}.{evt.Name} += value; remove => {castType}.{evt.Name} -= value; }}");

                if (evt.DeclaringType is { })
                    yield return evt.DeclaringType;
                if (evt.EventHandlerType is { })
                    yield return evt.EventHandlerType;
            }
        }

        private IEnumerable<Type> CreatePropertiesAndIndexers(Type type, StringBuilder output)
        {
            var properties = GetProperties(type);
            var indexerUsings = CreateIndexerProperties();
            var propUsings = CreateProperties();
            return indexerUsings.Concat(propUsings).ToList();

            IEnumerable<Type> CreateIndexerProperties()
            {
                foreach (var prop in properties.Where(x => x.GetIndexParameters().Length > 0).OrderBy(x => x.Name))
                {
                    output.AppendLine();
                    var indexParam = prop.GetIndexParameters().Single();
                    var genericReturnType = GetGenericArgsList(prop.PropertyType);
                    var paramType = prop.PropertyType.IsGenericParameter
                        ? prop.PropertyType.GetGenericParameterConstraints().Single()
                        : prop.PropertyType;

                    var getter = " ";
                    var setter = prop.CanWrite
                        ? IsWrappedType(paramType)
                            ? $" set {{ WrappedObject[{indexParam.Name}] = value; MarkAsStale(); }}"
                            : $" set => WrappedObject[{indexParam.Name}] = value;"
                        : " ";

                    if (IsNonWrappedType(paramType) && prop.CanRead)
                    {
                        getter = $" get => WrappedObject[{indexParam.Name}];";
                    }
                    else if (prop.CanRead && prop.PropertyType.IsGenericParameter)
                    {
                        getter = $" get => GetOrWrap<{GetReturnType(prop.PropertyType)}>(HashCode.Combine(\"this+{indexParam.ParameterType}\", {indexParam.Name}), ItemWrapper, () => WrappedObject[{indexParam.Name}]);";
                    }
                    else if (prop.CanRead)
                    {
                        getter = $" get => GetOrWrap(HashCode.Combine(\"this+{indexParam.ParameterType}\", {indexParam.Name}), () => WrappedObject[{indexParam.Name}]);";
                    }
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public {GetReturnType(prop.PropertyType)} this[{GetReturnType(indexParam.ParameterType)} {indexParam.Name}] {{{getter}{setter}}}");

                    yield return indexParam.ParameterType;
                }
            }

            IEnumerable<Type> CreateProperties()
            {
                foreach (var prop in properties.Where(x => x.GetIndexParameters().Length == 0).OrderBy(x => x.Name))
                {
                    output.AppendLine();
                    var propType = GetReturnType(prop.PropertyType);
                    var genericReturnType = prop.PropertyType.IsGenericType
                        ? $"<{string.Join(",", prop.PropertyType.GetGenericArguments().Select(x => x.Name))}>"
                        : string.Empty;

                    var paramType = prop.PropertyType.IsGenericType
                        ? prop.PropertyType.GetGenericArguments().Single()
                        : prop.PropertyType;

                    var getter = " ";
                    var setter = prop.CanWrite
                        ? IsWrappedType(paramType)
                            ? $" set {{ WrappedObject.{prop.Name} = value; MarkAsStale(); }}"
                            : $" set => WrappedObject.{prop.Name} = value;"
                        : " ";

                    if (IsNonWrappedType(paramType) && prop.CanRead)
                    {
                        getter = $" get => WrappedObject.{prop.Name};";
                    }
                    else if (prop.CanRead)
                    {
                        var wrapKey = $"{prop.Name}Key";
                        output.AppendLine($"{space,8}private static readonly int {wrapKey} = HashCode.Combine(nameof({prop.Name}));");
                        getter = $" get => GetOrWrap({wrapKey}, () => WrappedObject.{prop.Name});";
                    }

                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public {propType} {prop.Name} {{{getter}{setter}}}");

                    yield return prop.PropertyType;
                    foreach (var t in prop.PropertyType.GetGenericArguments()) yield return t;
                }
            }
        }

        private IEnumerable<Type> CreateMethods(Type type, StringBuilder output)
        {
            var methods = GetMethods(type);
            var markAsStaleMethods = new string[] { "Add", "SetOptionAt", "AddOption" };

            foreach (var method in methods.Where(x => !x.IsSpecialName && x.Name != "GetEnumerator").OrderBy(x => x.Name))
            {
                var parameters = method.GetParameters();
                var paramStr = string.Join(", ", parameters.Select(x => $"{GetParamterType(x.ParameterType)} {x.Name}"));
                var argsStr = string.Join(", ", parameters.Select(x => x.Name));
                var returnType = GetReturnType(method.ReturnType);
                var genericReturnType = method.ReturnType.IsGenericType
                    ? $"<{string.Join(",", method.ReturnType.GetGenericArguments().Select(x => x.Name))}>"
                    : string.Empty;

                var shouldMarkAsStale = parameters.Any(x => IsWrappedType(x.ParameterType));

                output.AppendLine();

                if (IsNonWrappedType(method.ReturnType))
                {
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public {returnType} {method.Name}({paramStr})");

                    if (shouldMarkAsStale && markAsStaleMethods.Contains(method.Name, StringComparison.Ordinal))
                    {
                        if (method.ReturnType == VoidType)
                        {
                            output.AppendLine($"{space,8}{{");
                            output.AppendLine($"{space,12}WrappedObject.{method.Name}({argsStr});");
                            output.AppendLine($"{space,12}MarkAsStale();");
                            output.AppendLine($"{space,8}}}");
                        }
                        else
                        {
                            output.AppendLine($"{space,8}{{");
                            output.AppendLine($"{space,12}var result = WrappedObject.{method.Name}({argsStr});");
                            output.AppendLine($"{space,12}MarkAsStale();");
                            output.AppendLine($"{space,12}return result;");
                            output.AppendLine($"{space,8}}}");
                        }
                    }
                    else
                    {
                        output.AppendLine($"{space,12}=> WrappedObject.{method.Name}({argsStr});");
                    }
                }
                else
                {
                    var wrapKey = string.Empty;
                    if (parameters.Length == 0)
                    {
                        output.AppendLine($"{space,8}private static readonly int {method.Name}Key = HashCode.Combine(nameof({method.Name}));");
                        wrapKey = $"{method.Name}Key";
                    }
                    else
                    {
                        wrapKey = $"HashCode.Combine(nameof({method.Name}), {argsStr})";
                    }
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public {returnType} {method.Name}({paramStr})");
                    if (!shouldMarkAsStale)
                    {
                        output.AppendLine($"{space,12}=> GetOrWrap{genericReturnType}({wrapKey}, () => WrappedObject.{method.Name}({argsStr}));");
                    }
                    else if (shouldMarkAsStale && method.ReturnType == VoidType)
                    {
                        output.AppendLine($"{space,8}{{");
                        output.AppendLine($"{space,12}WrappedObject.{method.Name}({argsStr});");
                        output.AppendLine($"{space,12}MarkAsStale();");
                        output.AppendLine($"{space,8}}}");
                    }
                    else
                    {
                        output.AppendLine($"{space,8}{{");
                        output.AppendLine($"{space,12}var result = GetOrWrap{genericReturnType}({wrapKey}, () => WrappedObject.{method.Name}({argsStr}));");
                        output.AppendLine($"{space,12}MarkAsStale();");
                        output.AppendLine($"{space,12}return result;");
                        output.AppendLine($"{space,8}}}");
                    }
                }

                yield return method.ReturnType;
                foreach (var t in method.ReturnType.GetGenericArguments()) yield return t;
                foreach (var t in parameters.Select(x => x.ParameterType)) yield return t;
                foreach (var t in parameters.SelectMany(x => x.ParameterType.GetGenericArguments())) yield return t;
            }
        }

        private IEnumerable<Type> CreateEnumerableMethods(Type type, StringBuilder output)
        {
            var enumerableType = type.GetInterfaces().FirstOrDefault(x => x.Name.StartsWith("IEnumerable"));
            if (enumerableType is { })
            {
                output.AppendLine();
                var enumerableGeneric = enumerableType.GetGenericArguments().Single();
                var enumerableTargetType = enumerableGeneric;

                if (enumerableTargetType.IsGenericParameter)
                {
                    enumerableTargetType = enumerableTargetType.GetGenericParameterConstraints().Single();
                }

                if (IsNonWrappedType(enumerableTargetType))
                {
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public IEnumerator<{GetParamterType(enumerableGeneric)}> GetEnumerator() => WrappedObject.GetEnumerator();");
                    output.AppendLine();
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();");
                }
                else
                {
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}public IEnumerator<{GetParamterType(enumerableGeneric)}> GetEnumerator()");
                    output.AppendLine($"{space,8}{{");
                    output.AppendLine($"{space,12}for (int i = 0; i < Length; i++)");
                    output.AppendLine($"{space,12}{{");
                    output.AppendLine($"{space,16}yield return this[i];");
                    output.AppendLine($"{space,12}}}");
                    output.AppendLine($"{space,8}}}");
                    output.AppendLine();
                    output.AppendLine($"{space,8}/// <inheritdoc/>");
                    output.AppendLine($"{space,8}IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();");
                }
                yield return typeof(System.Collections.IEnumerable);
                yield return typeof(IEnumerable<>);
                yield return enumerableGeneric;
            }
        }

        private static string GenerateUsings(List<Type> usings)
        {
            var uniqueUsings = usings.Select(x => x.Namespace).Distinct();
            var systemUsings = uniqueUsings.Where(x => x!.StartsWith("System")).OrderBy(x => x);
            var others = uniqueUsings.Where(x => !x!.StartsWith("System")).OrderBy(x => x);
            return string.Join(Environment.NewLine, systemUsings.Concat(others).Select(x => $"using {x};"));
        }

        private bool IsWrappedType(Type type)
        {
            return !IsNonWrappedType(type);
        }

        private bool IsNonWrappedType(Type type)
        {
            return type.IsPrimitive || type == VoidType || type.IsGenericParameter || !WrappedTypes.ContainsKey(GetTypeKey(type));
        }

        private static string GetTypeKey(Type type) => $"{type.Namespace}.{type.Name}";

        /// <summary>
        /// Gets all methods (and property methods) of an interface, and any interfaces it implements.
        /// </summary>
        public static HashSet<MethodInfo> GetMethods(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new HashSet<MethodInfo>();

            foreach (var mi in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                result.Add(mi);
            }

            foreach (var baseType in type.GetInterfaces())
            {
                result.AddRange(GetMethods(baseType));
            }

            return result;
        }

        public static List<EventInfo> GetEvents(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new List<EventInfo>();

            foreach (var mi in type.GetEvents(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                result.Add(mi);
            }

            foreach (var baseType in type.GetInterfaces())
            {
                result.AddRange(GetEvents(baseType));
            }

            return result;
        }

        public static HashSet<PropertyInfo> GetProperties(Type type)
        {
            if (type is null) throw new ArgumentNullException(nameof(type));

            var result = new HashSet<PropertyInfo>();

            foreach (var mi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy))
            {
                result.Add(mi);
            }

            foreach (var baseType in type.GetInterfaces())
            {
                result.AddRange(GetProperties(baseType));
            }

            return result;
        }

        public static PropertyInfo[] GetPublicProperties(Type type)
        {
            if (type.IsInterface)
            {
                var propertyInfos = new HashSet<PropertyInfo>();

                var considered = new List<Type>();
                var queue = new Queue<Type>();
                considered.Add(type);
                queue.Enqueue(type);
                while (queue.Count > 0)
                {
                    var subType = queue.Dequeue();
                    foreach (var subInterface in subType.GetInterfaces())
                    {
                        if (considered.Contains(subInterface)) continue;

                        considered.Add(subInterface);
                        queue.Enqueue(subInterface);
                    }

                    var typeProperties = subType.GetProperties(
                        BindingFlags.FlattenHierarchy
                        | BindingFlags.Public
                        | BindingFlags.Instance);

                    var newPropertyInfos = typeProperties
                        .Where(x => !propertyInfos.Contains(x));

                    propertyInfos.AddRange(newPropertyInfos);
                }

                return propertyInfos.ToArray();
            }

            return type.GetProperties(BindingFlags.FlattenHierarchy
                | BindingFlags.Public | BindingFlags.Instance);
        }

        public static readonly Type VoidType = typeof(void);

        public static string GetReturnType(Type returnType)
        {
            if (returnType == VoidType) return "void";
            return GetParamterType(returnType);
        }

        private static string GetParamterType(Type parameterType)
        {
            var name = GetInterfaceTypeName(parameterType);
            var genericArgs = GetGenericArgsList(parameterType);
            return $"{name}{genericArgs}";
        }

        private static string GetGenericArgsList(Type type)
        {
            var genericArgs = type.GetGenericArguments();
            return genericArgs.Length > 0
                ? $"<{string.Join(",", type.GetGenericArguments().Select(x => x.Name))}>"
                : "";
        }

        private static string GetWrapperClassName(Type type) => $"{GetInterfaceTypeName(type)[1..]}Wrapper";

        private static string GetGenericClassArguments(Type type) => type.IsGenericType ? GetParamterType(type) : string.Empty;

        private static string GetGenericTypeConstraints(Type type)
        {
            return type.IsGenericType
                ? string.Join("", type.GetGenericArguments().SelectMany(x => x.GetGenericParameterConstraints().Select(y => $"where {x.Name} : class, {y.Name}")))
                : string.Empty;
        }

        private static string GetInterfaceTypeName(Type type)
        {
            return type.IsGenericType
                           ? $"{type.Name[0..type.Name.IndexOf('`', StringComparison.Ordinal)]}"
                           : $"{type.Name[0..]}";
        }
    }

    public static class Extensions
    {
        public static void AddRange<T>(this ISet<T> set, IEnumerable<T> items)
        {
            foreach (var item in items)
            {
                set.Add(item);
            }
        }
    }
}
