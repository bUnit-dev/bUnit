using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using AngleSharp;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="INodeList"/> type.
    /// </summary>
    public partial class NodeListWrapper : Wrapper<INodeList>, INodeList, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="NodeListWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public NodeListWrapper(Func<INodeList?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public INode this[Int32 index] { get => GetOrWrap(HashCode.Combine("this+System.Int32", index), () => WrappedObject[index]); }

        /// <inheritdoc/>
        public Int32 Length { get => WrappedObject.Length; }

        /// <inheritdoc/>
        public void ToHtml(TextWriter writer, IMarkupFormatter formatter)
            => WrappedObject.ToHtml(writer, formatter);

        /// <inheritdoc/>
        public IEnumerator<INode> GetEnumerator()
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
