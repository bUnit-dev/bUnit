using System;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using AngleSharp.Media.Dom;
using AngleSharp.Svg.Dom;
namespace AngleSharpWrappers
{
    #nullable enable
    public abstract partial class Wrapper<T> : IWrapper where T : class
    {
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IAttr GetOrWrap(int key, Func<IAttr?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new AttrWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IAttr)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IAudioTrackList GetOrWrap(int key, Func<IAudioTrackList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new AudioTrackListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IAudioTrackList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ICharacterData GetOrWrap(int key, Func<ICharacterData?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new CharacterDataWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ICharacterData)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IComment GetOrWrap(int key, Func<IComment?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new CommentWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IComment)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IDocument GetOrWrap(int key, Func<IDocument?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new DocumentWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IDocument)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IDocumentFragment GetOrWrap(int key, Func<IDocumentFragment?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new DocumentFragmentWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IDocumentFragment)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IDocumentType GetOrWrap(int key, Func<IDocumentType?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new DocumentTypeWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IDocumentType)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IElement GetOrWrap(int key, Func<IElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new ElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IFileList GetOrWrap(int key, Func<IFileList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new FileListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IFileList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlAllCollection GetOrWrap(int key, Func<IHtmlAllCollection?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlAllCollectionWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlAllCollection)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlAnchorElement GetOrWrap(int key, Func<IHtmlAnchorElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlAnchorElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlAnchorElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlAreaElement GetOrWrap(int key, Func<IHtmlAreaElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlAreaElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlAreaElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlAudioElement GetOrWrap(int key, Func<IHtmlAudioElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlAudioElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlAudioElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlBaseElement GetOrWrap(int key, Func<IHtmlBaseElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlBaseElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlBaseElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlBodyElement GetOrWrap(int key, Func<IHtmlBodyElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlBodyElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlBodyElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlBreakRowElement GetOrWrap(int key, Func<IHtmlBreakRowElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlBreakRowElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlBreakRowElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlButtonElement GetOrWrap(int key, Func<IHtmlButtonElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlButtonElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlButtonElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlCanvasElement GetOrWrap(int key, Func<IHtmlCanvasElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlCanvasElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlCanvasElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlCommandElement GetOrWrap(int key, Func<IHtmlCommandElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlCommandElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlCommandElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDataElement GetOrWrap(int key, Func<IHtmlDataElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDataElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDataElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDataListElement GetOrWrap(int key, Func<IHtmlDataListElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDataListElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDataListElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDetailsElement GetOrWrap(int key, Func<IHtmlDetailsElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDetailsElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDetailsElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDialogElement GetOrWrap(int key, Func<IHtmlDialogElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDialogElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDialogElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDivElement GetOrWrap(int key, Func<IHtmlDivElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDivElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDivElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlDocument GetOrWrap(int key, Func<IHtmlDocument?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlDocumentWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlDocument)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlElement GetOrWrap(int key, Func<IHtmlElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlEmbedElement GetOrWrap(int key, Func<IHtmlEmbedElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlEmbedElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlEmbedElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlFieldSetElement GetOrWrap(int key, Func<IHtmlFieldSetElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlFieldSetElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlFieldSetElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlFormControlsCollection GetOrWrap(int key, Func<IHtmlFormControlsCollection?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlFormControlsCollectionWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlFormControlsCollection)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlFormElement GetOrWrap(int key, Func<IHtmlFormElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlFormElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlFormElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlHeadElement GetOrWrap(int key, Func<IHtmlHeadElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlHeadElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlHeadElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlHeadingElement GetOrWrap(int key, Func<IHtmlHeadingElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlHeadingElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlHeadingElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlHrElement GetOrWrap(int key, Func<IHtmlHrElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlHrElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlHrElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlHtmlElement GetOrWrap(int key, Func<IHtmlHtmlElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlHtmlElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlHtmlElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlImageElement GetOrWrap(int key, Func<IHtmlImageElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlImageElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlImageElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlInlineFrameElement GetOrWrap(int key, Func<IHtmlInlineFrameElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlInlineFrameElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlInlineFrameElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlInputElement GetOrWrap(int key, Func<IHtmlInputElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlInputElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlInputElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlKeygenElement GetOrWrap(int key, Func<IHtmlKeygenElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlKeygenElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlKeygenElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlLabelElement GetOrWrap(int key, Func<IHtmlLabelElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlLabelElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlLabelElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlLegendElement GetOrWrap(int key, Func<IHtmlLegendElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlLegendElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlLegendElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlLinkElement GetOrWrap(int key, Func<IHtmlLinkElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlLinkElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlLinkElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlListItemElement GetOrWrap(int key, Func<IHtmlListItemElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlListItemElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlListItemElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMapElement GetOrWrap(int key, Func<IHtmlMapElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMapElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMapElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMarqueeElement GetOrWrap(int key, Func<IHtmlMarqueeElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMarqueeElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMarqueeElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMediaElement GetOrWrap(int key, Func<IHtmlMediaElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMediaElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMediaElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMenuElement GetOrWrap(int key, Func<IHtmlMenuElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMenuElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMenuElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMenuItemElement GetOrWrap(int key, Func<IHtmlMenuItemElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMenuItemElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMenuItemElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMetaElement GetOrWrap(int key, Func<IHtmlMetaElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMetaElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMetaElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlMeterElement GetOrWrap(int key, Func<IHtmlMeterElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlMeterElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlMeterElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlModElement GetOrWrap(int key, Func<IHtmlModElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlModElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlModElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlObjectElement GetOrWrap(int key, Func<IHtmlObjectElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlObjectElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlObjectElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlOptionElement GetOrWrap(int key, Func<IHtmlOptionElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlOptionElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlOptionElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlOptionsCollection GetOrWrap(int key, Func<IHtmlOptionsCollection?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlOptionsCollectionWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlOptionsCollection)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlOptionsGroupElement GetOrWrap(int key, Func<IHtmlOptionsGroupElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlOptionsGroupElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlOptionsGroupElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlOrderedListElement GetOrWrap(int key, Func<IHtmlOrderedListElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlOrderedListElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlOrderedListElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlOutputElement GetOrWrap(int key, Func<IHtmlOutputElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlOutputElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlOutputElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlParagraphElement GetOrWrap(int key, Func<IHtmlParagraphElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlParagraphElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlParagraphElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlParamElement GetOrWrap(int key, Func<IHtmlParamElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlParamElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlParamElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlPreElement GetOrWrap(int key, Func<IHtmlPreElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlPreElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlPreElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlProgressElement GetOrWrap(int key, Func<IHtmlProgressElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlProgressElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlProgressElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlQuoteElement GetOrWrap(int key, Func<IHtmlQuoteElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlQuoteElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlQuoteElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlScriptElement GetOrWrap(int key, Func<IHtmlScriptElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlScriptElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlScriptElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlSelectElement GetOrWrap(int key, Func<IHtmlSelectElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlSelectElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlSelectElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlSlotElement GetOrWrap(int key, Func<IHtmlSlotElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlSlotElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlSlotElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlSourceElement GetOrWrap(int key, Func<IHtmlSourceElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlSourceElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlSourceElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlSpanElement GetOrWrap(int key, Func<IHtmlSpanElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlSpanElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlSpanElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlStyleElement GetOrWrap(int key, Func<IHtmlStyleElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlStyleElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlStyleElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableCaptionElement GetOrWrap(int key, Func<IHtmlTableCaptionElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableCaptionElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableCaptionElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableCellElement GetOrWrap(int key, Func<IHtmlTableCellElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableCellElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableCellElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableColumnElement GetOrWrap(int key, Func<IHtmlTableColumnElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableColumnElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableColumnElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableDataCellElement GetOrWrap(int key, Func<IHtmlTableDataCellElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableDataCellElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableDataCellElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableElement GetOrWrap(int key, Func<IHtmlTableElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableHeaderCellElement GetOrWrap(int key, Func<IHtmlTableHeaderCellElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableHeaderCellElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableHeaderCellElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableRowElement GetOrWrap(int key, Func<IHtmlTableRowElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableRowElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableRowElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTableSectionElement GetOrWrap(int key, Func<IHtmlTableSectionElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTableSectionElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTableSectionElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTemplateElement GetOrWrap(int key, Func<IHtmlTemplateElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTemplateElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTemplateElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTextAreaElement GetOrWrap(int key, Func<IHtmlTextAreaElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTextAreaElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTextAreaElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTimeElement GetOrWrap(int key, Func<IHtmlTimeElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTimeElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTimeElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTitleElement GetOrWrap(int key, Func<IHtmlTitleElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTitleElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTitleElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlTrackElement GetOrWrap(int key, Func<IHtmlTrackElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlTrackElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlTrackElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlUnknownElement GetOrWrap(int key, Func<IHtmlUnknownElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlUnknownElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlUnknownElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlUnorderedListElement GetOrWrap(int key, Func<IHtmlUnorderedListElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlUnorderedListElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlUnorderedListElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlVideoElement GetOrWrap(int key, Func<IHtmlVideoElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlVideoElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlVideoElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IMediaList GetOrWrap(int key, Func<IMediaList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new MediaListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IMediaList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected INamedNodeMap GetOrWrap(int key, Func<INamedNodeMap?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new NamedNodeMapWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (INamedNodeMap)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected INode GetOrWrap(int key, Func<INode?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new NodeWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (INode)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected INodeList GetOrWrap(int key, Func<INodeList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new NodeListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (INodeList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IProcessingInstruction GetOrWrap(int key, Func<IProcessingInstruction?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new ProcessingInstructionWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IProcessingInstruction)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IPseudoElement GetOrWrap(int key, Func<IPseudoElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new PseudoElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IPseudoElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISettableTokenList GetOrWrap(int key, Func<ISettableTokenList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SettableTokenListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISettableTokenList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IShadowRoot GetOrWrap(int key, Func<IShadowRoot?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new ShadowRootWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IShadowRoot)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IStringList GetOrWrap(int key, Func<IStringList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new StringListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IStringList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IStringMap GetOrWrap(int key, Func<IStringMap?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new StringMapWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IStringMap)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IStyleSheetList GetOrWrap(int key, Func<IStyleSheetList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new StyleSheetListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IStyleSheetList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgCircleElement GetOrWrap(int key, Func<ISvgCircleElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgCircleElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgCircleElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgDescriptionElement GetOrWrap(int key, Func<ISvgDescriptionElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgDescriptionElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgDescriptionElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgElement GetOrWrap(int key, Func<ISvgElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgForeignObjectElement GetOrWrap(int key, Func<ISvgForeignObjectElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgForeignObjectElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgForeignObjectElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgSvgElement GetOrWrap(int key, Func<ISvgSvgElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgSvgElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgSvgElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ISvgTitleElement GetOrWrap(int key, Func<ISvgTitleElement?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new SvgTitleElementWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ISvgTitleElement)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IText GetOrWrap(int key, Func<IText?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new TextWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IText)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ITextTrackCueList GetOrWrap(int key, Func<ITextTrackCueList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new TextTrackCueListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ITextTrackCueList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ITextTrackList GetOrWrap(int key, Func<ITextTrackList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new TextTrackListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ITextTrackList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected ITokenList GetOrWrap(int key, Func<ITokenList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new TokenListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (ITokenList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IVideoTrackList GetOrWrap(int key, Func<IVideoTrackList?> objectQuery)
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new VideoTrackListWrapper(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IVideoTrackList)result;
        }
        /// <summary>
        /// Gets (or wraps) the requested nested object.
        /// </summary>
        /// <param name="key">Key to look up the requested wrapped object.</param>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The object wrapped in a wrapper.</returns>
        protected IHtmlCollection<TWrapped> GetOrWrap<TWrapped>(int key, Func<IHtmlCollection<TWrapped>?> objectQuery) where TWrapped : class, IElement
        {
            if (!Wrappers.TryGetValue(key, out var result))
            {
                result = new HtmlCollectionWrapper<TWrapped>(objectQuery);
                Wrappers.Add(key, result);
            }
            return (IHtmlCollection<TWrapped>)result;
        }
    }
}
