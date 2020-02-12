using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Bunit
{
    /// <summary>
    /// Represents a component that can be added inside a <see cref="Fixture"/>,
    /// where a component under test can be defined as the child content.
    /// </summary>
    public class ComponentUnderTest : FragmentBase
    {
        /// <inheritdoc />
        public override Task SetParametersAsync(ParameterView parameters)
        {
            var result = base.SetParametersAsync(parameters);
            return result;
        }
    }
}
