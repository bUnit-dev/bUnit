using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="INamedNodeMap"/> type.
    /// </summary>
    public partial class NamedNodeMapWrapper : Wrapper<INamedNodeMap>, INamedNodeMap, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="NamedNodeMapWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public NamedNodeMapWrapper(Func<INamedNodeMap?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public IAttr this[Int32 index] { get => GetOrWrap(HashCode.Combine("this+System.Int32", index), () => WrappedObject[index]); }

        /// <inheritdoc/>
        public IAttr this[String name] { get => GetOrWrap(HashCode.Combine("this+System.String", name), () => WrappedObject[name]); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public IAttr GetNamedItem(String name)
            => GetOrWrap(HashCode.Combine(nameof(GetNamedItem), name), () => WrappedObject.GetNamedItem(name));

        /// <inheritdoc/>
        public IAttr GetNamedItem(String namespaceUri, String localName)
            => GetOrWrap(HashCode.Combine(nameof(GetNamedItem), namespaceUri, localName), () => WrappedObject.GetNamedItem(namespaceUri, localName));

        /// <inheritdoc/>
        public IAttr RemoveNamedItem(String name)
            => GetOrWrap(HashCode.Combine(nameof(RemoveNamedItem), name), () => WrappedObject.RemoveNamedItem(name));

        /// <inheritdoc/>
        public IAttr RemoveNamedItem(String namespaceUri, String localName)
            => GetOrWrap(HashCode.Combine(nameof(RemoveNamedItem), namespaceUri, localName), () => WrappedObject.RemoveNamedItem(namespaceUri, localName));

        /// <inheritdoc/>
        public IAttr SetNamedItem(IAttr item)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(SetNamedItem), item), () => WrappedObject.SetNamedItem(item));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public IAttr SetNamedItemWithNamespaceUri(IAttr item)
        {
            var result = GetOrWrap(HashCode.Combine(nameof(SetNamedItemWithNamespaceUri), item), () => WrappedObject.SetNamedItemWithNamespaceUri(item));
            MarkAsStale();
            return result;
        }

        /// <inheritdoc/>
        public IEnumerator<IAttr> GetEnumerator()
        {
            for (int i = 0; i < Length; i++)
            {
                yield return this[i];
            }
        }

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
