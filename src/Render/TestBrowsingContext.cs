using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Browser;
using AngleSharp.Browser.Dom;
using AngleSharp.Dom;

namespace Egil.RazorComponents.Testing.Render
{
    internal sealed class TestBrowsingContext : EventTarget, IBrowsingContext, IDisposable
    {
        private readonly List<object> _services = new List<Object>();
        private readonly Dictionary<string, WeakReference<IBrowsingContext>> _children;

        /// <summary>
        /// Gets or sets the currently active document.
        /// </summary>
        public IDocument? Active { get; set; }

        /// <summary>
        /// Gets the document that created the current context, if any. The
        /// creator is the active document of the parent at the time of
        /// creation.
        /// </summary>
        public IDocument? Creator { get; }

        /// <summary>
        /// Gets the original services for the given browsing context.
        /// </summary>
        public IEnumerable<object> OriginalServices { get; }

        /// <summary>
        /// Gets the current window proxy.
        /// </summary>
        public IWindow? Current => Active?.DefaultView;

        /// <summary>
        /// Gets the parent of the current context, if any. If a parent is
        /// available, then the current context contains only embedded
        /// documents.
        /// </summary>
        public IBrowsingContext? Parent { get; }

        /// <summary>
        /// Gets the session history of the given browsing context, if any.
        /// </summary>
        public IHistory? SessionHistory { get; }

        /// <summary>
        /// Gets the sandboxing flag of the context.
        /// </summary>
        public Sandboxes Security { get; } = Sandboxes.None;
        
        public TestRenderer Renderer { get; }

        internal TestBrowsingContext(TestRenderer renderer) : this(renderer, Configuration.Default.Services)
        {
        }

        internal TestBrowsingContext(TestRenderer renderer, IEnumerable<object> services)
        {
            _children = new Dictionary<string, WeakReference<IBrowsingContext>>();
            _services.AddRange(services);
            OriginalServices = services;
            SessionHistory = GetService<IHistory>();
            Renderer = renderer;
        }

        private TestBrowsingContext(TestBrowsingContext parent) : this(parent.Renderer, parent.OriginalServices)
        {
            Parent = parent;
            Creator = Parent.Active;
        }

        /// <summary>
        /// Gets an instance of the given service.
        /// </summary>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        /// <returns>The instance of the service or null.</returns>
        public T? GetService<T>() where T : class
        {
            var count = _services.Count;

            for (var i = 0; i < count; i++)
            {
                var service = _services[i];
                var instance = service as T;

                if (instance == null)
                {
                    if (service is Func<IBrowsingContext, T> creator)
                    {
                        instance = creator.Invoke(this);
                        _services[i] = instance;
                    }
                    else
                    {
                        continue;
                    }
                }

                return instance;
            }

            return null;
        }

        /// <summary>
        /// Gets all registered instances of the given service.
        /// </summary>
        /// <typeparam name="T">The type of service to resolve.</typeparam>
        /// <returns>An enumerable with all service instances.</returns>
        public IEnumerable<T> GetServices<T>() where T : class
        {
            var count = _services.Count;

            for (var i = 0; i < count; i++)
            {
                var service = _services[i];
                var instance = service as T;

                if (instance == null)
                {
                    if (service is Func<IBrowsingContext, T> creator)
                    {
                        instance = creator.Invoke(this);
                        _services[i] = instance;
                    }
                    else
                    {
                        continue;
                    }
                }

                yield return instance;
            }
        }

        /// <summary>
        /// Creates a new named browsing context as child of the given parent.
        /// </summary>
        /// <param name="name">The name of the child context, if any.</param>
        /// <param name="security">The security flags to apply.</param>
        /// <returns></returns>
        public IBrowsingContext CreateChild(string name, Sandboxes security)
        {
            var context = new TestBrowsingContext(this);

            if (!string.IsNullOrEmpty(name))
            {
                _children[name] = new WeakReference<IBrowsingContext>(context);
            }

            return context;
        }

        /// <summary>
        /// Finds a named browsing context.
        /// </summary>
        /// <param name="name">The name of the browsing context.</param>
        /// <returns>The found instance, if any.</returns>
        public IBrowsingContext? FindChild(string name)
        {
            var context = default(IBrowsingContext);

            if (!string.IsNullOrEmpty(name) && _children.TryGetValue(name, out var reference))
            {
                reference.TryGetTarget(out context);
            }

            return context;
        }

        public void Dispose()
        {
            Active?.Dispose();
            Active = null;
        }
    }
}
