using AngleSharp;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{

    // TODO: add support for all event types listed here:
    //       https://github.com/aspnet/AspNetCore/blob/master/src/Components/Web/src/Web/EventHandlers.cs
    public static class EventDispatchExtensions
    {
        public static void Click(this IElement element, int numberOfClicks = 1)
        {
            for (int i = 0; i < numberOfClicks; i++) _ = ClickAsync(element);
        }

        public static Task ClickAsync(this IElement element) => element.TriggerEventAsync("onclick", new MouseEventArgs());

        public static void Submit(this IElement element) => _ = SubmitAsync(element);

        public static Task SubmitAsync(this IElement element)
            => element.TriggerEventAsync("onsubmit", new EventArgs());

        public static void Change(this IElement element, string newValue)
            => _ = ChangeAsync(element, newValue);

        public static Task ChangeAsync(this IElement element, string newValue)
            => element.TriggerEventAsync("onchange", new ChangeEventArgs { Value = newValue });

        public static void Change(this IElement element, bool newValue)
        {
            _ = ChangeAsync(element, newValue);
        }

        public static Task ChangeAsync(this IElement element, bool newValue)
            => element.TriggerEventAsync("onchange", new ChangeEventArgs { Value = newValue });

        [SuppressMessage("Usage", "BL0006:Do not use RenderTree types", Justification = "<Pending>")]
        public static Task TriggerEventAsync(this IElement element, string attributeName, EventArgs eventArgs)
        {
            if (element is null) throw new ArgumentNullException(nameof(element));

            var eventHandlerIdString = element.GetAttribute($"{Htmlizer.EVENT_HANDLE_ID_ATTR_PREFIX}{attributeName}");

            if (string.IsNullOrEmpty(eventHandlerIdString))
                throw new ArgumentException($"The element does not have an event handler for the event '{attributeName}'.");

            var eventHandlerId = ulong.Parse(eventHandlerIdString, CultureInfo.InvariantCulture);

            var renderer = element.Owner.Context.GetService<TestRenderer>();
            return renderer.DispatchEventAsync(eventHandlerId,
                new EventFieldInfo(),
                eventArgs);
        }
    }
}
