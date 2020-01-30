using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Rendering.HtmlDomWrappers
{
    /// <summary>
    /// Represents a factory for a wrapper of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type this factory can wrap.</typeparam>
    /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
    /// <returns>A wrapper of type <typeparamref name="T"/>.</returns>
    public delegate Wrapper<T> WrapperFactory<T>(Func<T?> objectQuery) where T : class;
}
