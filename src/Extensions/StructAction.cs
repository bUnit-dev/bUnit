using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing.Extensions
{
    /// <summary>
    /// Represents an Action delegate, that allows <c>readonly struct</c> to be passed using
    /// the <c>in</c> argument modifier.
    /// </summary>
    /// <typeparam name="T">The struct type.</typeparam>
    /// <param name="input">The input argument</param>
    public delegate void StructAction<T>(in T input) where T : struct;
}
