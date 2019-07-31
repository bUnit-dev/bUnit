using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;

namespace Egil.RazorComponents.Testing
{
    public class FragmentBuilder
    {
        private readonly Type _componentType;
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private RenderFragment? _childContent;

        public FragmentBuilder(Type componentType)
        {
            _componentType = componentType;
        }

        public FragmentBuilder WithChildContent(string markupContent)
        {
            _childContent = (builder) => builder.AddMarkupContent(1, markupContent);
            return this;
        }

        public FragmentBuilder WithChildContent(RenderFragment childContent)
        {
            _childContent = childContent;
            return this;
        }

        public FragmentBuilder WithChildContent(params RenderFragment[] childContents)
        {
            _childContent = ToFragment(childContents);
            return this;
        }

        public FragmentBuilder WithParams(params (string name, object value)[] paramValues)
        {
            foreach (var (name, value) in paramValues) _parameters[name] = value;
            return this;
        }

        public RenderFragment Build()
        {
            return builder =>
            {
                builder.OpenComponent(1, _componentType);
                if (!(_childContent is null)) builder.AddAttribute(2, RenderTreeBuilder.ChildContent, _childContent);
                foreach (var (name, value) in _parameters)
                {
                    builder.AddAttribute(3, name, value);
                }
                builder.CloseComponent();
            };
        }

        private static RenderFragment ToFragment(RenderFragment[] childContents)
        {
            return builder =>
            {
                for (int i = 0; i < childContents.Length; i++)
                {
                    builder.AddContent(i, childContents[i]);
                }
            };
        }

        public static implicit operator RenderFragment(FragmentBuilder builder)
        {
            return builder.Build();
        }
    }

    public class FragmentBuilder<TComponent> : FragmentBuilder
        where TComponent : ComponentBase
    {
        public FragmentBuilder() : base(typeof(TComponent))
        {

        }
    }
}
