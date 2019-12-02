using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Microsoft.AspNetCore.Components.Web;

namespace Egil.RazorComponents.Testing
{
    public static class KeyboardEventDispatchExtensions
    {
        public static void Keydown(this IElement element,
                                   string key,
                                   string code = "",
                                   float location = 0,
                                   bool repeat = false,
                                   bool ctrlKey = false,
                                   bool shiftKey = false,
                                   bool altKey = false,
                                   bool metaKey = false)
            => _ = KeydownAsync(element, key, code, location, repeat, ctrlKey, shiftKey, altKey, metaKey);

        public static Task KeydownAsync(this IElement element,
                                        string key,
                                        string code = "",
                                        float location = 0,
                                        bool repeat = false,
                                        bool ctrlKey = false,
                                        bool shiftKey = false,
                                        bool altKey = false,
                                        bool metaKey = false)
            => element.TriggerEventAsync("onkeydown", new KeyboardEventArgs
            {
                Key = key,
                Code = code,
                Location = location,
                Repeat = repeat,
                CtrlKey = ctrlKey,
                ShiftKey = shiftKey,
                AltKey = altKey,
                MetaKey = metaKey
            });

        public static void Keyup(this IElement element,
                           string key,
                           string code = "",
                           float location = 0,
                           bool repeat = false,
                           bool ctrlKey = false,
                           bool shiftKey = false,
                           bool altKey = false,
                           bool metaKey = false)
            => _ = KeyupAsync(element, key, code, location, repeat, ctrlKey, shiftKey, altKey, metaKey);

        public static Task KeyupAsync(this IElement element,
                                        string key,
                                        string code = "",
                                        float location = 0,
                                        bool repeat = false,
                                        bool ctrlKey = false,
                                        bool shiftKey = false,
                                        bool altKey = false,
                                        bool metaKey = false)
            => element.TriggerEventAsync("onkeyup", new KeyboardEventArgs
            {
                Key = key,
                Code = code,
                Location = location,
                Repeat = repeat,
                CtrlKey = ctrlKey,
                ShiftKey = shiftKey,
                AltKey = altKey,
                MetaKey = metaKey
            });

        public static void Keypress(this IElement element,
                           string key,
                           string code = "",
                           float location = 0,
                           bool repeat = false,
                           bool ctrlKey = false,
                           bool shiftKey = false,
                           bool altKey = false,
                           bool metaKey = false)
            => _ = KeypressAsync(element, key, code, location, repeat, ctrlKey, shiftKey, altKey, metaKey);

        public static Task KeypressAsync(this IElement element,
                                        string key,
                                        string code = "",
                                        float location = 0,
                                        bool repeat = false,
                                        bool ctrlKey = false,
                                        bool shiftKey = false,
                                        bool altKey = false,
                                        bool metaKey = false)
            => element.TriggerEventAsync("onkeypress", new KeyboardEventArgs
            {
                Key = key,
                Code = code,
                Location = location,
                Repeat = repeat,
                CtrlKey = ctrlKey,
                ShiftKey = shiftKey,
                AltKey = altKey,
                MetaKey = metaKey
            });
    }
}
