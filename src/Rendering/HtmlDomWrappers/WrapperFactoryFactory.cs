using System;
using System.Collections.Generic;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class WrapperFactoryFactory
    {
        private static Dictionary<Type, object> Wrappers = new Dictionary<Type, object>();

        /// <summary>
        /// Creates a wrapper for type <typeparamref name="T"/>.
        /// </summary>        
        /// <returns>The <see cref="HtmlDomWrappers.WrapperFactory{T}"/></returns>
        public static WrapperFactory<T> Create<T>() where T : class
        {
            var type = typeof(T);
            var result = Wrappers[type];
            return (WrapperFactory<T>)result;
        }
    }
}
