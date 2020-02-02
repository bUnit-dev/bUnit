using System;
using AngleSharp.Dom;

namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// Represents a wrapper class around <see cref="IAttr"/> type.
    /// </summary>
    public partial class AttrWrapper : Wrapper<IAttr>, IAttr, IWrapper
    {
        /// <summary>
        /// Creates an instance of the <see cref="AttrWrapper"/> type;
        /// </summary>
        /// <param name="getObject">A function that can be used to retrieve a new instance of the wrapped type.</param>
        public AttrWrapper(Func<IAttr?> getObject) : base(getObject) { }

        /// <inheritdoc/>
        public String LocalName { get => WrappedObject.LocalName; }

        /// <inheritdoc/>
        public String Name { get => WrappedObject.Name; }

        /// <inheritdoc/>
        public String NamespaceUri { get => WrappedObject.NamespaceUri; }

        /// <inheritdoc/>
        public String Prefix { get => WrappedObject.Prefix; }

        /// <inheritdoc/>
        public String Value { get => WrappedObject.Value; set => WrappedObject.Value = value;}

        /// <inheritdoc/>
        public Boolean Equals(IAttr other)
            => WrappedObject.Equals(other);
    }
}
