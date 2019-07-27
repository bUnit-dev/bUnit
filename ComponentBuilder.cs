using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using System.Runtime.ExceptionServices;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Encodings.Web;
using Moq;
using System.Linq;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.JSInterop;
using Shouldly;

namespace Egil.RazorComponents.Testing
{
    public class ComponentBuilder<TComponent> where TComponent : ComponentBase
    {
        private readonly Func<string, string> _encoder = (t) => HtmlEncoder.Default.Encode(t);
        private readonly IDispatcher _dispatcher = Renderer.CreateDefaultDispatcher();

        protected ServiceCollection Services { get; } = new ServiceCollection();
        protected Dictionary<string, object> Parameters { get; } = new Dictionary<string, object>();

        public ComponentBuilder()
        {
            Services.AddSingleton(x => Mock.Of<IUriHelper>());
            Services.AddSingleton(x => Mock.Of<IJSRuntime>());
            Services.AddSingleton<TComponent>();
        }

        public ComponentBuilder<TComponent> WithServices(Action<IServiceCollection> addServices)
        {
            addServices(Services);
            return this;
        }

        public ComponentBuilder<TComponent> WithParams(params (string name, object value)[] paramValues)
        {
            foreach (var (name, value) in paramValues) Parameters[name] = value;
            return this;
        }

        public ComponentBuilder<TComponent> WithChildContent(RenderFragment renderFragment)
        {
            Parameters[RenderTreeBuilder.ChildContent] = renderFragment;
            return this;
        }

        public ComponentBuilder<TComponent> WithChildContent(params RenderFragment[] childContents)
        {
            Parameters[RenderTreeBuilder.ChildContent] = ToFragment(childContents);
            return this;
        }

        public ComponentBuilder<TComponent> WithChildContent(string childMarkupContent)
        {
            return WithChildContent(b => b.AddMarkupContent(0, childMarkupContent));
        }

        public ComponentRenderedText Render()
        {
            var serviceProvider = Services.BuildServiceProvider();
            var paramCollection = Parameters.Count > 0 ? ParameterCollection.FromDictionary(Parameters) : ParameterCollection.Empty;
            using var htmlRenderer = new HtmlRenderer(serviceProvider, _encoder, _dispatcher);
            return GetResult(_dispatcher.InvokeAsync(() => htmlRenderer.RenderComponentAsync<TComponent>(paramCollection)));
        }

        private ComponentRenderedText GetResult(Task<ComponentRenderedText> task)
        {
            task.IsCompleted.ShouldBeTrue();
            if (task.IsCompletedSuccessfully)
            {
                return task.Result;
            }
            else
            {
                ExceptionDispatchInfo.Capture(task.Exception!).Throw();
                throw new InvalidOperationException("We will never hit this line");
            }
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
    }

    public class ComponentBuilder<TComponent, TItem> : ComponentBuilder<TComponent>
        where TComponent : ComponentBase
    {
        public new ComponentBuilder<TComponent, TItem> WithServices(Action<IServiceCollection> addServices)
        {
            addServices(Services);
            return this;
        }


        public new ComponentBuilder<TComponent, TItem> WithParams(params (string name, object value)[] paramValues)
        {
            foreach (var (name, value) in paramValues) Parameters[name] = value;
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithItems(IEnumerable<TItem> items)
        {
            Parameters["Items"] = items;
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithItems(params TItem[] items)
        {
            Parameters["Items"] = items;
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithItems(int itemCount, Func<int, TItem> itemGenerator)
        {
            Parameters["Items"] = Enumerable.Range(1, itemCount).Select(itemGenerator).ToArray();
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithDefaultItems(int itemCount)
        {
            return WithItems(itemCount, _ => default!);
        }

        public ComponentBuilder<TComponent, TItem> WithTemplate(RenderFragment<TItem> renderTemplate)
        {
            Parameters[RenderTreeBuilder.ChildContent] = renderTemplate;
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithTemplate(Func<TemplateBuilder<TItem>, RenderFragment<TItem>> templateBuilder)
        {
            Parameters[RenderTreeBuilder.ChildContent] = templateBuilder(new TemplateBuilder<TItem>());
            return this;
        }

        public ComponentBuilder<TComponent, TItem> WithTemplate(params RenderFragment[] renderFragments)
        {
            Parameters[RenderTreeBuilder.ChildContent] = ToFragment<TItem>(renderFragments);
            return this;
        }

        private static RenderFragment<TItem> ToFragment<ITtem>(RenderFragment[] childContents)
        {
            return (item) => builder =>
            {
                for (int i = 0; i < childContents.Length; i++)
                {
                    builder.AddContent(i, childContents[i]);
                }
            };
        }

    }
}
