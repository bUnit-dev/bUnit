using System;
using System.Runtime.Serialization;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public class ComponentNotFoundException<TComponent> : Exception where TComponent : IComponent
    {
        public ComponentNotFoundException() : base($"Unable to find a component of type {typeof(TComponent)} in the render tree.")
        {
        }
    }
}