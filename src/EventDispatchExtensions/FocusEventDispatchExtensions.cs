using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Egil.RazorComponents.Testing
{
    public static class FocusEventDispatchExtensions
    {
        public static void Focus(this IElement element) => _ = FocusAsync(element);

        public static Task FocusAsync(this IElement element)
            => element.TriggerEventAsync("onfocus", new FocusEventArgs());

        public static void Blur(this IElement element) => _ = BlurAsync(element);

        public static Task BlurAsync(this IElement element)
            => element.TriggerEventAsync("onblur", new FocusEventArgs());

        public static void FocusIn(this IElement element) => _ = FocusInAsync(element);

        public static Task FocusInAsync(this IElement element)
            => element.TriggerEventAsync("onfocusin", new FocusEventArgs());

        public static void FocusOut(this IElement element) => _ = FocusOutAsync(element);

        public static Task FocusOutAsync(this IElement element)
            => element.TriggerEventAsync("onfocusout", new FocusEventArgs());
    }
}
