using System;
using System.Collections;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IStringMap"/> type.
    /// </summary>
    public partial class StringMapWrapper : Wrapper<IStringMap>, IStringMap, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="StringMapWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public StringMapWrapper(Func<IStringMap?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public String this[String name] { get => WrappedObject[name]; set => WrappedObject[name] = value;}

        /// <inheritdoc/>
        public void Remove(String name)
            => WrappedObject.Remove(name);

        /// <inheritdoc/>
        public IEnumerator<KeyValuePair<String,String>> GetEnumerator() => WrappedObject.GetEnumerator();

        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => WrappedObject.GetEnumerator();
    }
}
