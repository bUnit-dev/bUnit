using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    public readonly struct ComponentParameter : IEquatable<ComponentParameter>
    {
        public string Name { get; }
        public object? Value { get; }
        public bool IsCascadingValue { get; }

        public ComponentParameter(string name, object? value, bool isCascadingValue = false)
        {
            Name = name;
            Value = value;
            IsCascadingValue = isCascadingValue;
        }

        public static implicit operator ComponentParameter((string name, object? value) input)
        {
            return new ComponentParameter(input.name, input.value);
        }

        public static implicit operator ComponentParameter((string name, object? value, bool isCascadingValue) input)
        {
            return new ComponentParameter(input.name, input.value, input.isCascadingValue);
        }

        /// <inheritdoc/>
        public bool Equals(ComponentParameter other) => Name == other.Name && Value == other.Value && IsCascadingValue == other.IsCascadingValue;

        /// <inheritdoc/>
        public override bool Equals(object obj) => obj is ComponentParameter other && Equals(other);

        /// <inheritdoc/>
        public override int GetHashCode() => (Name, Value, IsCascadingValue).GetHashCode();

        /// <inheritdoc/>
        public static bool operator ==(ComponentParameter left, ComponentParameter right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(ComponentParameter left, ComponentParameter right)
        {
            return !(left == right);
        }
    }
}
