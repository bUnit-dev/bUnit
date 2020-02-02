using System;
using System.Collections.Generic;
using AngleSharp.Css.Dom;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Io.Dom;
using AngleSharp.Media.Dom;
using AngleSharp.Svg.Dom;
namespace AngleSharpWrappers
{
    #nullable enable
    public static partial class WrapperFactoryFactory
    {
        static WrapperFactoryFactory()
        {
            Wrappers.Add(typeof(ISvgCircleElement), (WrapperFactory<ISvgCircleElement>)CreateSvgCircleElementWrapper);
            Wrappers.Add(typeof(ISvgDescriptionElement), (WrapperFactory<ISvgDescriptionElement>)CreateSvgDescriptionElementWrapper);
            Wrappers.Add(typeof(ISvgElement), (WrapperFactory<ISvgElement>)CreateSvgElementWrapper);
            Wrappers.Add(typeof(ISvgForeignObjectElement), (WrapperFactory<ISvgForeignObjectElement>)CreateSvgForeignObjectElementWrapper);
            Wrappers.Add(typeof(ISvgSvgElement), (WrapperFactory<ISvgSvgElement>)CreateSvgSvgElementWrapper);
            Wrappers.Add(typeof(ISvgTitleElement), (WrapperFactory<ISvgTitleElement>)CreateSvgTitleElementWrapper);
            Wrappers.Add(typeof(IAudioTrackList), (WrapperFactory<IAudioTrackList>)CreateAudioTrackListWrapper);
            Wrappers.Add(typeof(ITextTrackCueList), (WrapperFactory<ITextTrackCueList>)CreateTextTrackCueListWrapper);
            Wrappers.Add(typeof(ITextTrackList), (WrapperFactory<ITextTrackList>)CreateTextTrackListWrapper);
            Wrappers.Add(typeof(IVideoTrackList), (WrapperFactory<IVideoTrackList>)CreateVideoTrackListWrapper);
            Wrappers.Add(typeof(IFileList), (WrapperFactory<IFileList>)CreateFileListWrapper);
            Wrappers.Add(typeof(IHtmlAnchorElement), (WrapperFactory<IHtmlAnchorElement>)CreateHtmlAnchorElementWrapper);
            Wrappers.Add(typeof(IHtmlAreaElement), (WrapperFactory<IHtmlAreaElement>)CreateHtmlAreaElementWrapper);
            Wrappers.Add(typeof(IHtmlAudioElement), (WrapperFactory<IHtmlAudioElement>)CreateHtmlAudioElementWrapper);
            Wrappers.Add(typeof(IHtmlBaseElement), (WrapperFactory<IHtmlBaseElement>)CreateHtmlBaseElementWrapper);
            Wrappers.Add(typeof(IHtmlBodyElement), (WrapperFactory<IHtmlBodyElement>)CreateHtmlBodyElementWrapper);
            Wrappers.Add(typeof(IHtmlBreakRowElement), (WrapperFactory<IHtmlBreakRowElement>)CreateHtmlBreakRowElementWrapper);
            Wrappers.Add(typeof(IHtmlButtonElement), (WrapperFactory<IHtmlButtonElement>)CreateHtmlButtonElementWrapper);
            Wrappers.Add(typeof(IHtmlCanvasElement), (WrapperFactory<IHtmlCanvasElement>)CreateHtmlCanvasElementWrapper);
            Wrappers.Add(typeof(IHtmlCommandElement), (WrapperFactory<IHtmlCommandElement>)CreateHtmlCommandElementWrapper);
            Wrappers.Add(typeof(IHtmlDataElement), (WrapperFactory<IHtmlDataElement>)CreateHtmlDataElementWrapper);
            Wrappers.Add(typeof(IHtmlDataListElement), (WrapperFactory<IHtmlDataListElement>)CreateHtmlDataListElementWrapper);
            Wrappers.Add(typeof(IHtmlDetailsElement), (WrapperFactory<IHtmlDetailsElement>)CreateHtmlDetailsElementWrapper);
            Wrappers.Add(typeof(IHtmlDialogElement), (WrapperFactory<IHtmlDialogElement>)CreateHtmlDialogElementWrapper);
            Wrappers.Add(typeof(IHtmlDivElement), (WrapperFactory<IHtmlDivElement>)CreateHtmlDivElementWrapper);
            Wrappers.Add(typeof(IHtmlDocument), (WrapperFactory<IHtmlDocument>)CreateHtmlDocumentWrapper);
            Wrappers.Add(typeof(IHtmlElement), (WrapperFactory<IHtmlElement>)CreateHtmlElementWrapper);
            Wrappers.Add(typeof(IHtmlEmbedElement), (WrapperFactory<IHtmlEmbedElement>)CreateHtmlEmbedElementWrapper);
            Wrappers.Add(typeof(IHtmlFieldSetElement), (WrapperFactory<IHtmlFieldSetElement>)CreateHtmlFieldSetElementWrapper);
            Wrappers.Add(typeof(IHtmlFormControlsCollection), (WrapperFactory<IHtmlFormControlsCollection>)CreateHtmlFormControlsCollectionWrapper);
            Wrappers.Add(typeof(IHtmlFormElement), (WrapperFactory<IHtmlFormElement>)CreateHtmlFormElementWrapper);
            Wrappers.Add(typeof(IHtmlHeadElement), (WrapperFactory<IHtmlHeadElement>)CreateHtmlHeadElementWrapper);
            Wrappers.Add(typeof(IHtmlHeadingElement), (WrapperFactory<IHtmlHeadingElement>)CreateHtmlHeadingElementWrapper);
            Wrappers.Add(typeof(IHtmlHrElement), (WrapperFactory<IHtmlHrElement>)CreateHtmlHrElementWrapper);
            Wrappers.Add(typeof(IHtmlHtmlElement), (WrapperFactory<IHtmlHtmlElement>)CreateHtmlHtmlElementWrapper);
            Wrappers.Add(typeof(IHtmlImageElement), (WrapperFactory<IHtmlImageElement>)CreateHtmlImageElementWrapper);
            Wrappers.Add(typeof(IHtmlInlineFrameElement), (WrapperFactory<IHtmlInlineFrameElement>)CreateHtmlInlineFrameElementWrapper);
            Wrappers.Add(typeof(IHtmlInputElement), (WrapperFactory<IHtmlInputElement>)CreateHtmlInputElementWrapper);
            Wrappers.Add(typeof(IHtmlKeygenElement), (WrapperFactory<IHtmlKeygenElement>)CreateHtmlKeygenElementWrapper);
            Wrappers.Add(typeof(IHtmlLabelElement), (WrapperFactory<IHtmlLabelElement>)CreateHtmlLabelElementWrapper);
            Wrappers.Add(typeof(IHtmlLegendElement), (WrapperFactory<IHtmlLegendElement>)CreateHtmlLegendElementWrapper);
            Wrappers.Add(typeof(IHtmlLinkElement), (WrapperFactory<IHtmlLinkElement>)CreateHtmlLinkElementWrapper);
            Wrappers.Add(typeof(IHtmlListItemElement), (WrapperFactory<IHtmlListItemElement>)CreateHtmlListItemElementWrapper);
            Wrappers.Add(typeof(IHtmlMapElement), (WrapperFactory<IHtmlMapElement>)CreateHtmlMapElementWrapper);
            Wrappers.Add(typeof(IHtmlMarqueeElement), (WrapperFactory<IHtmlMarqueeElement>)CreateHtmlMarqueeElementWrapper);
            Wrappers.Add(typeof(IHtmlMediaElement), (WrapperFactory<IHtmlMediaElement>)CreateHtmlMediaElementWrapper);
            Wrappers.Add(typeof(IHtmlMenuElement), (WrapperFactory<IHtmlMenuElement>)CreateHtmlMenuElementWrapper);
            Wrappers.Add(typeof(IHtmlMenuItemElement), (WrapperFactory<IHtmlMenuItemElement>)CreateHtmlMenuItemElementWrapper);
            Wrappers.Add(typeof(IHtmlMetaElement), (WrapperFactory<IHtmlMetaElement>)CreateHtmlMetaElementWrapper);
            Wrappers.Add(typeof(IHtmlMeterElement), (WrapperFactory<IHtmlMeterElement>)CreateHtmlMeterElementWrapper);
            Wrappers.Add(typeof(IHtmlModElement), (WrapperFactory<IHtmlModElement>)CreateHtmlModElementWrapper);
            Wrappers.Add(typeof(IHtmlObjectElement), (WrapperFactory<IHtmlObjectElement>)CreateHtmlObjectElementWrapper);
            Wrappers.Add(typeof(IHtmlOptionElement), (WrapperFactory<IHtmlOptionElement>)CreateHtmlOptionElementWrapper);
            Wrappers.Add(typeof(IHtmlOptionsCollection), (WrapperFactory<IHtmlOptionsCollection>)CreateHtmlOptionsCollectionWrapper);
            Wrappers.Add(typeof(IHtmlOptionsGroupElement), (WrapperFactory<IHtmlOptionsGroupElement>)CreateHtmlOptionsGroupElementWrapper);
            Wrappers.Add(typeof(IHtmlOrderedListElement), (WrapperFactory<IHtmlOrderedListElement>)CreateHtmlOrderedListElementWrapper);
            Wrappers.Add(typeof(IHtmlOutputElement), (WrapperFactory<IHtmlOutputElement>)CreateHtmlOutputElementWrapper);
            Wrappers.Add(typeof(IHtmlParagraphElement), (WrapperFactory<IHtmlParagraphElement>)CreateHtmlParagraphElementWrapper);
            Wrappers.Add(typeof(IHtmlParamElement), (WrapperFactory<IHtmlParamElement>)CreateHtmlParamElementWrapper);
            Wrappers.Add(typeof(IHtmlPreElement), (WrapperFactory<IHtmlPreElement>)CreateHtmlPreElementWrapper);
            Wrappers.Add(typeof(IHtmlProgressElement), (WrapperFactory<IHtmlProgressElement>)CreateHtmlProgressElementWrapper);
            Wrappers.Add(typeof(IHtmlQuoteElement), (WrapperFactory<IHtmlQuoteElement>)CreateHtmlQuoteElementWrapper);
            Wrappers.Add(typeof(IHtmlScriptElement), (WrapperFactory<IHtmlScriptElement>)CreateHtmlScriptElementWrapper);
            Wrappers.Add(typeof(IHtmlSelectElement), (WrapperFactory<IHtmlSelectElement>)CreateHtmlSelectElementWrapper);
            Wrappers.Add(typeof(IHtmlSlotElement), (WrapperFactory<IHtmlSlotElement>)CreateHtmlSlotElementWrapper);
            Wrappers.Add(typeof(IHtmlSourceElement), (WrapperFactory<IHtmlSourceElement>)CreateHtmlSourceElementWrapper);
            Wrappers.Add(typeof(IHtmlSpanElement), (WrapperFactory<IHtmlSpanElement>)CreateHtmlSpanElementWrapper);
            Wrappers.Add(typeof(IHtmlStyleElement), (WrapperFactory<IHtmlStyleElement>)CreateHtmlStyleElementWrapper);
            Wrappers.Add(typeof(IHtmlTableCaptionElement), (WrapperFactory<IHtmlTableCaptionElement>)CreateHtmlTableCaptionElementWrapper);
            Wrappers.Add(typeof(IHtmlTableCellElement), (WrapperFactory<IHtmlTableCellElement>)CreateHtmlTableCellElementWrapper);
            Wrappers.Add(typeof(IHtmlTableColumnElement), (WrapperFactory<IHtmlTableColumnElement>)CreateHtmlTableColumnElementWrapper);
            Wrappers.Add(typeof(IHtmlTableDataCellElement), (WrapperFactory<IHtmlTableDataCellElement>)CreateHtmlTableDataCellElementWrapper);
            Wrappers.Add(typeof(IHtmlTableElement), (WrapperFactory<IHtmlTableElement>)CreateHtmlTableElementWrapper);
            Wrappers.Add(typeof(IHtmlTableHeaderCellElement), (WrapperFactory<IHtmlTableHeaderCellElement>)CreateHtmlTableHeaderCellElementWrapper);
            Wrappers.Add(typeof(IHtmlTableRowElement), (WrapperFactory<IHtmlTableRowElement>)CreateHtmlTableRowElementWrapper);
            Wrappers.Add(typeof(IHtmlTableSectionElement), (WrapperFactory<IHtmlTableSectionElement>)CreateHtmlTableSectionElementWrapper);
            Wrappers.Add(typeof(IHtmlTemplateElement), (WrapperFactory<IHtmlTemplateElement>)CreateHtmlTemplateElementWrapper);
            Wrappers.Add(typeof(IHtmlTextAreaElement), (WrapperFactory<IHtmlTextAreaElement>)CreateHtmlTextAreaElementWrapper);
            Wrappers.Add(typeof(IHtmlTimeElement), (WrapperFactory<IHtmlTimeElement>)CreateHtmlTimeElementWrapper);
            Wrappers.Add(typeof(IHtmlTitleElement), (WrapperFactory<IHtmlTitleElement>)CreateHtmlTitleElementWrapper);
            Wrappers.Add(typeof(IHtmlTrackElement), (WrapperFactory<IHtmlTrackElement>)CreateHtmlTrackElementWrapper);
            Wrappers.Add(typeof(IHtmlUnknownElement), (WrapperFactory<IHtmlUnknownElement>)CreateHtmlUnknownElementWrapper);
            Wrappers.Add(typeof(IHtmlUnorderedListElement), (WrapperFactory<IHtmlUnorderedListElement>)CreateHtmlUnorderedListElementWrapper);
            Wrappers.Add(typeof(IHtmlVideoElement), (WrapperFactory<IHtmlVideoElement>)CreateHtmlVideoElementWrapper);
            Wrappers.Add(typeof(ICharacterData), (WrapperFactory<ICharacterData>)CreateCharacterDataWrapper);
            Wrappers.Add(typeof(IComment), (WrapperFactory<IComment>)CreateCommentWrapper);
            Wrappers.Add(typeof(IDocument), (WrapperFactory<IDocument>)CreateDocumentWrapper);
            Wrappers.Add(typeof(IDocumentFragment), (WrapperFactory<IDocumentFragment>)CreateDocumentFragmentWrapper);
            Wrappers.Add(typeof(IDocumentType), (WrapperFactory<IDocumentType>)CreateDocumentTypeWrapper);
            Wrappers.Add(typeof(IElement), (WrapperFactory<IElement>)CreateElementWrapper);
            Wrappers.Add(typeof(IHtmlAllCollection), (WrapperFactory<IHtmlAllCollection>)CreateHtmlAllCollectionWrapper);
            Wrappers.Add(typeof(INamedNodeMap), (WrapperFactory<INamedNodeMap>)CreateNamedNodeMapWrapper);
            Wrappers.Add(typeof(INode), (WrapperFactory<INode>)CreateNodeWrapper);
            Wrappers.Add(typeof(INodeList), (WrapperFactory<INodeList>)CreateNodeListWrapper);
            Wrappers.Add(typeof(IProcessingInstruction), (WrapperFactory<IProcessingInstruction>)CreateProcessingInstructionWrapper);
            Wrappers.Add(typeof(IPseudoElement), (WrapperFactory<IPseudoElement>)CreatePseudoElementWrapper);
            Wrappers.Add(typeof(ISettableTokenList), (WrapperFactory<ISettableTokenList>)CreateSettableTokenListWrapper);
            Wrappers.Add(typeof(IShadowRoot), (WrapperFactory<IShadowRoot>)CreateShadowRootWrapper);
            Wrappers.Add(typeof(IStringList), (WrapperFactory<IStringList>)CreateStringListWrapper);
            Wrappers.Add(typeof(IStringMap), (WrapperFactory<IStringMap>)CreateStringMapWrapper);
            Wrappers.Add(typeof(IStyleSheetList), (WrapperFactory<IStyleSheetList>)CreateStyleSheetListWrapper);
            Wrappers.Add(typeof(IText), (WrapperFactory<IText>)CreateTextWrapper);
            Wrappers.Add(typeof(ITokenList), (WrapperFactory<ITokenList>)CreateTokenListWrapper);
            Wrappers.Add(typeof(IMediaList), (WrapperFactory<IMediaList>)CreateMediaListWrapper);
            Wrappers.Add(typeof(IAttr), (WrapperFactory<IAttr>)CreateAttrWrapper);
        }

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgCircleElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgCircleElementWrapper"/>.</returns>
        public static SvgCircleElementWrapper CreateSvgCircleElementWrapper(Func<ISvgCircleElement?> objectQuery) => new SvgCircleElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgDescriptionElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgDescriptionElementWrapper"/>.</returns>
        public static SvgDescriptionElementWrapper CreateSvgDescriptionElementWrapper(Func<ISvgDescriptionElement?> objectQuery) => new SvgDescriptionElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgElementWrapper"/>.</returns>
        public static SvgElementWrapper CreateSvgElementWrapper(Func<ISvgElement?> objectQuery) => new SvgElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgForeignObjectElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgForeignObjectElementWrapper"/>.</returns>
        public static SvgForeignObjectElementWrapper CreateSvgForeignObjectElementWrapper(Func<ISvgForeignObjectElement?> objectQuery) => new SvgForeignObjectElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgSvgElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgSvgElementWrapper"/>.</returns>
        public static SvgSvgElementWrapper CreateSvgSvgElementWrapper(Func<ISvgSvgElement?> objectQuery) => new SvgSvgElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISvgTitleElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SvgTitleElementWrapper"/>.</returns>
        public static SvgTitleElementWrapper CreateSvgTitleElementWrapper(Func<ISvgTitleElement?> objectQuery) => new SvgTitleElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IAudioTrackList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="AudioTrackListWrapper"/>.</returns>
        public static AudioTrackListWrapper CreateAudioTrackListWrapper(Func<IAudioTrackList?> objectQuery) => new AudioTrackListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ITextTrackCueList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="TextTrackCueListWrapper"/>.</returns>
        public static TextTrackCueListWrapper CreateTextTrackCueListWrapper(Func<ITextTrackCueList?> objectQuery) => new TextTrackCueListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ITextTrackList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="TextTrackListWrapper"/>.</returns>
        public static TextTrackListWrapper CreateTextTrackListWrapper(Func<ITextTrackList?> objectQuery) => new TextTrackListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IVideoTrackList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="VideoTrackListWrapper"/>.</returns>
        public static VideoTrackListWrapper CreateVideoTrackListWrapper(Func<IVideoTrackList?> objectQuery) => new VideoTrackListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IFileList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="FileListWrapper"/>.</returns>
        public static FileListWrapper CreateFileListWrapper(Func<IFileList?> objectQuery) => new FileListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlAnchorElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlAnchorElementWrapper"/>.</returns>
        public static HtmlAnchorElementWrapper CreateHtmlAnchorElementWrapper(Func<IHtmlAnchorElement?> objectQuery) => new HtmlAnchorElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlAreaElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlAreaElementWrapper"/>.</returns>
        public static HtmlAreaElementWrapper CreateHtmlAreaElementWrapper(Func<IHtmlAreaElement?> objectQuery) => new HtmlAreaElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlAudioElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlAudioElementWrapper"/>.</returns>
        public static HtmlAudioElementWrapper CreateHtmlAudioElementWrapper(Func<IHtmlAudioElement?> objectQuery) => new HtmlAudioElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlBaseElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlBaseElementWrapper"/>.</returns>
        public static HtmlBaseElementWrapper CreateHtmlBaseElementWrapper(Func<IHtmlBaseElement?> objectQuery) => new HtmlBaseElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlBodyElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlBodyElementWrapper"/>.</returns>
        public static HtmlBodyElementWrapper CreateHtmlBodyElementWrapper(Func<IHtmlBodyElement?> objectQuery) => new HtmlBodyElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlBreakRowElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlBreakRowElementWrapper"/>.</returns>
        public static HtmlBreakRowElementWrapper CreateHtmlBreakRowElementWrapper(Func<IHtmlBreakRowElement?> objectQuery) => new HtmlBreakRowElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlButtonElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlButtonElementWrapper"/>.</returns>
        public static HtmlButtonElementWrapper CreateHtmlButtonElementWrapper(Func<IHtmlButtonElement?> objectQuery) => new HtmlButtonElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlCanvasElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlCanvasElementWrapper"/>.</returns>
        public static HtmlCanvasElementWrapper CreateHtmlCanvasElementWrapper(Func<IHtmlCanvasElement?> objectQuery) => new HtmlCanvasElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlCommandElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlCommandElementWrapper"/>.</returns>
        public static HtmlCommandElementWrapper CreateHtmlCommandElementWrapper(Func<IHtmlCommandElement?> objectQuery) => new HtmlCommandElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDataElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDataElementWrapper"/>.</returns>
        public static HtmlDataElementWrapper CreateHtmlDataElementWrapper(Func<IHtmlDataElement?> objectQuery) => new HtmlDataElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDataListElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDataListElementWrapper"/>.</returns>
        public static HtmlDataListElementWrapper CreateHtmlDataListElementWrapper(Func<IHtmlDataListElement?> objectQuery) => new HtmlDataListElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDetailsElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDetailsElementWrapper"/>.</returns>
        public static HtmlDetailsElementWrapper CreateHtmlDetailsElementWrapper(Func<IHtmlDetailsElement?> objectQuery) => new HtmlDetailsElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDialogElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDialogElementWrapper"/>.</returns>
        public static HtmlDialogElementWrapper CreateHtmlDialogElementWrapper(Func<IHtmlDialogElement?> objectQuery) => new HtmlDialogElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDivElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDivElementWrapper"/>.</returns>
        public static HtmlDivElementWrapper CreateHtmlDivElementWrapper(Func<IHtmlDivElement?> objectQuery) => new HtmlDivElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlDocument"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlDocumentWrapper"/>.</returns>
        public static HtmlDocumentWrapper CreateHtmlDocumentWrapper(Func<IHtmlDocument?> objectQuery) => new HtmlDocumentWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlElementWrapper"/>.</returns>
        public static HtmlElementWrapper CreateHtmlElementWrapper(Func<IHtmlElement?> objectQuery) => new HtmlElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlEmbedElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlEmbedElementWrapper"/>.</returns>
        public static HtmlEmbedElementWrapper CreateHtmlEmbedElementWrapper(Func<IHtmlEmbedElement?> objectQuery) => new HtmlEmbedElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlFieldSetElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlFieldSetElementWrapper"/>.</returns>
        public static HtmlFieldSetElementWrapper CreateHtmlFieldSetElementWrapper(Func<IHtmlFieldSetElement?> objectQuery) => new HtmlFieldSetElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlFormControlsCollection"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlFormControlsCollectionWrapper"/>.</returns>
        public static HtmlFormControlsCollectionWrapper CreateHtmlFormControlsCollectionWrapper(Func<IHtmlFormControlsCollection?> objectQuery) => new HtmlFormControlsCollectionWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlFormElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlFormElementWrapper"/>.</returns>
        public static HtmlFormElementWrapper CreateHtmlFormElementWrapper(Func<IHtmlFormElement?> objectQuery) => new HtmlFormElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlHeadElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlHeadElementWrapper"/>.</returns>
        public static HtmlHeadElementWrapper CreateHtmlHeadElementWrapper(Func<IHtmlHeadElement?> objectQuery) => new HtmlHeadElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlHeadingElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlHeadingElementWrapper"/>.</returns>
        public static HtmlHeadingElementWrapper CreateHtmlHeadingElementWrapper(Func<IHtmlHeadingElement?> objectQuery) => new HtmlHeadingElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlHrElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlHrElementWrapper"/>.</returns>
        public static HtmlHrElementWrapper CreateHtmlHrElementWrapper(Func<IHtmlHrElement?> objectQuery) => new HtmlHrElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlHtmlElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlHtmlElementWrapper"/>.</returns>
        public static HtmlHtmlElementWrapper CreateHtmlHtmlElementWrapper(Func<IHtmlHtmlElement?> objectQuery) => new HtmlHtmlElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlImageElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlImageElementWrapper"/>.</returns>
        public static HtmlImageElementWrapper CreateHtmlImageElementWrapper(Func<IHtmlImageElement?> objectQuery) => new HtmlImageElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlInlineFrameElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlInlineFrameElementWrapper"/>.</returns>
        public static HtmlInlineFrameElementWrapper CreateHtmlInlineFrameElementWrapper(Func<IHtmlInlineFrameElement?> objectQuery) => new HtmlInlineFrameElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlInputElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlInputElementWrapper"/>.</returns>
        public static HtmlInputElementWrapper CreateHtmlInputElementWrapper(Func<IHtmlInputElement?> objectQuery) => new HtmlInputElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlKeygenElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlKeygenElementWrapper"/>.</returns>
        public static HtmlKeygenElementWrapper CreateHtmlKeygenElementWrapper(Func<IHtmlKeygenElement?> objectQuery) => new HtmlKeygenElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlLabelElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlLabelElementWrapper"/>.</returns>
        public static HtmlLabelElementWrapper CreateHtmlLabelElementWrapper(Func<IHtmlLabelElement?> objectQuery) => new HtmlLabelElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlLegendElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlLegendElementWrapper"/>.</returns>
        public static HtmlLegendElementWrapper CreateHtmlLegendElementWrapper(Func<IHtmlLegendElement?> objectQuery) => new HtmlLegendElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlLinkElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlLinkElementWrapper"/>.</returns>
        public static HtmlLinkElementWrapper CreateHtmlLinkElementWrapper(Func<IHtmlLinkElement?> objectQuery) => new HtmlLinkElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlListItemElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlListItemElementWrapper"/>.</returns>
        public static HtmlListItemElementWrapper CreateHtmlListItemElementWrapper(Func<IHtmlListItemElement?> objectQuery) => new HtmlListItemElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMapElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMapElementWrapper"/>.</returns>
        public static HtmlMapElementWrapper CreateHtmlMapElementWrapper(Func<IHtmlMapElement?> objectQuery) => new HtmlMapElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMarqueeElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMarqueeElementWrapper"/>.</returns>
        public static HtmlMarqueeElementWrapper CreateHtmlMarqueeElementWrapper(Func<IHtmlMarqueeElement?> objectQuery) => new HtmlMarqueeElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMediaElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMediaElementWrapper"/>.</returns>
        public static HtmlMediaElementWrapper CreateHtmlMediaElementWrapper(Func<IHtmlMediaElement?> objectQuery) => new HtmlMediaElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMenuElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMenuElementWrapper"/>.</returns>
        public static HtmlMenuElementWrapper CreateHtmlMenuElementWrapper(Func<IHtmlMenuElement?> objectQuery) => new HtmlMenuElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMenuItemElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMenuItemElementWrapper"/>.</returns>
        public static HtmlMenuItemElementWrapper CreateHtmlMenuItemElementWrapper(Func<IHtmlMenuItemElement?> objectQuery) => new HtmlMenuItemElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMetaElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMetaElementWrapper"/>.</returns>
        public static HtmlMetaElementWrapper CreateHtmlMetaElementWrapper(Func<IHtmlMetaElement?> objectQuery) => new HtmlMetaElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlMeterElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlMeterElementWrapper"/>.</returns>
        public static HtmlMeterElementWrapper CreateHtmlMeterElementWrapper(Func<IHtmlMeterElement?> objectQuery) => new HtmlMeterElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlModElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlModElementWrapper"/>.</returns>
        public static HtmlModElementWrapper CreateHtmlModElementWrapper(Func<IHtmlModElement?> objectQuery) => new HtmlModElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlObjectElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlObjectElementWrapper"/>.</returns>
        public static HtmlObjectElementWrapper CreateHtmlObjectElementWrapper(Func<IHtmlObjectElement?> objectQuery) => new HtmlObjectElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlOptionElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlOptionElementWrapper"/>.</returns>
        public static HtmlOptionElementWrapper CreateHtmlOptionElementWrapper(Func<IHtmlOptionElement?> objectQuery) => new HtmlOptionElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlOptionsCollection"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlOptionsCollectionWrapper"/>.</returns>
        public static HtmlOptionsCollectionWrapper CreateHtmlOptionsCollectionWrapper(Func<IHtmlOptionsCollection?> objectQuery) => new HtmlOptionsCollectionWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlOptionsGroupElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlOptionsGroupElementWrapper"/>.</returns>
        public static HtmlOptionsGroupElementWrapper CreateHtmlOptionsGroupElementWrapper(Func<IHtmlOptionsGroupElement?> objectQuery) => new HtmlOptionsGroupElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlOrderedListElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlOrderedListElementWrapper"/>.</returns>
        public static HtmlOrderedListElementWrapper CreateHtmlOrderedListElementWrapper(Func<IHtmlOrderedListElement?> objectQuery) => new HtmlOrderedListElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlOutputElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlOutputElementWrapper"/>.</returns>
        public static HtmlOutputElementWrapper CreateHtmlOutputElementWrapper(Func<IHtmlOutputElement?> objectQuery) => new HtmlOutputElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlParagraphElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlParagraphElementWrapper"/>.</returns>
        public static HtmlParagraphElementWrapper CreateHtmlParagraphElementWrapper(Func<IHtmlParagraphElement?> objectQuery) => new HtmlParagraphElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlParamElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlParamElementWrapper"/>.</returns>
        public static HtmlParamElementWrapper CreateHtmlParamElementWrapper(Func<IHtmlParamElement?> objectQuery) => new HtmlParamElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlPreElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlPreElementWrapper"/>.</returns>
        public static HtmlPreElementWrapper CreateHtmlPreElementWrapper(Func<IHtmlPreElement?> objectQuery) => new HtmlPreElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlProgressElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlProgressElementWrapper"/>.</returns>
        public static HtmlProgressElementWrapper CreateHtmlProgressElementWrapper(Func<IHtmlProgressElement?> objectQuery) => new HtmlProgressElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlQuoteElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlQuoteElementWrapper"/>.</returns>
        public static HtmlQuoteElementWrapper CreateHtmlQuoteElementWrapper(Func<IHtmlQuoteElement?> objectQuery) => new HtmlQuoteElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlScriptElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlScriptElementWrapper"/>.</returns>
        public static HtmlScriptElementWrapper CreateHtmlScriptElementWrapper(Func<IHtmlScriptElement?> objectQuery) => new HtmlScriptElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlSelectElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlSelectElementWrapper"/>.</returns>
        public static HtmlSelectElementWrapper CreateHtmlSelectElementWrapper(Func<IHtmlSelectElement?> objectQuery) => new HtmlSelectElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlSlotElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlSlotElementWrapper"/>.</returns>
        public static HtmlSlotElementWrapper CreateHtmlSlotElementWrapper(Func<IHtmlSlotElement?> objectQuery) => new HtmlSlotElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlSourceElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlSourceElementWrapper"/>.</returns>
        public static HtmlSourceElementWrapper CreateHtmlSourceElementWrapper(Func<IHtmlSourceElement?> objectQuery) => new HtmlSourceElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlSpanElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlSpanElementWrapper"/>.</returns>
        public static HtmlSpanElementWrapper CreateHtmlSpanElementWrapper(Func<IHtmlSpanElement?> objectQuery) => new HtmlSpanElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlStyleElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlStyleElementWrapper"/>.</returns>
        public static HtmlStyleElementWrapper CreateHtmlStyleElementWrapper(Func<IHtmlStyleElement?> objectQuery) => new HtmlStyleElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableCaptionElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableCaptionElementWrapper"/>.</returns>
        public static HtmlTableCaptionElementWrapper CreateHtmlTableCaptionElementWrapper(Func<IHtmlTableCaptionElement?> objectQuery) => new HtmlTableCaptionElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableCellElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableCellElementWrapper"/>.</returns>
        public static HtmlTableCellElementWrapper CreateHtmlTableCellElementWrapper(Func<IHtmlTableCellElement?> objectQuery) => new HtmlTableCellElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableColumnElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableColumnElementWrapper"/>.</returns>
        public static HtmlTableColumnElementWrapper CreateHtmlTableColumnElementWrapper(Func<IHtmlTableColumnElement?> objectQuery) => new HtmlTableColumnElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableDataCellElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableDataCellElementWrapper"/>.</returns>
        public static HtmlTableDataCellElementWrapper CreateHtmlTableDataCellElementWrapper(Func<IHtmlTableDataCellElement?> objectQuery) => new HtmlTableDataCellElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableElementWrapper"/>.</returns>
        public static HtmlTableElementWrapper CreateHtmlTableElementWrapper(Func<IHtmlTableElement?> objectQuery) => new HtmlTableElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableHeaderCellElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableHeaderCellElementWrapper"/>.</returns>
        public static HtmlTableHeaderCellElementWrapper CreateHtmlTableHeaderCellElementWrapper(Func<IHtmlTableHeaderCellElement?> objectQuery) => new HtmlTableHeaderCellElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableRowElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableRowElementWrapper"/>.</returns>
        public static HtmlTableRowElementWrapper CreateHtmlTableRowElementWrapper(Func<IHtmlTableRowElement?> objectQuery) => new HtmlTableRowElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTableSectionElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTableSectionElementWrapper"/>.</returns>
        public static HtmlTableSectionElementWrapper CreateHtmlTableSectionElementWrapper(Func<IHtmlTableSectionElement?> objectQuery) => new HtmlTableSectionElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTemplateElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTemplateElementWrapper"/>.</returns>
        public static HtmlTemplateElementWrapper CreateHtmlTemplateElementWrapper(Func<IHtmlTemplateElement?> objectQuery) => new HtmlTemplateElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTextAreaElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTextAreaElementWrapper"/>.</returns>
        public static HtmlTextAreaElementWrapper CreateHtmlTextAreaElementWrapper(Func<IHtmlTextAreaElement?> objectQuery) => new HtmlTextAreaElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTimeElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTimeElementWrapper"/>.</returns>
        public static HtmlTimeElementWrapper CreateHtmlTimeElementWrapper(Func<IHtmlTimeElement?> objectQuery) => new HtmlTimeElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTitleElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTitleElementWrapper"/>.</returns>
        public static HtmlTitleElementWrapper CreateHtmlTitleElementWrapper(Func<IHtmlTitleElement?> objectQuery) => new HtmlTitleElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlTrackElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlTrackElementWrapper"/>.</returns>
        public static HtmlTrackElementWrapper CreateHtmlTrackElementWrapper(Func<IHtmlTrackElement?> objectQuery) => new HtmlTrackElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlUnknownElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlUnknownElementWrapper"/>.</returns>
        public static HtmlUnknownElementWrapper CreateHtmlUnknownElementWrapper(Func<IHtmlUnknownElement?> objectQuery) => new HtmlUnknownElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlUnorderedListElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlUnorderedListElementWrapper"/>.</returns>
        public static HtmlUnorderedListElementWrapper CreateHtmlUnorderedListElementWrapper(Func<IHtmlUnorderedListElement?> objectQuery) => new HtmlUnorderedListElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlVideoElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlVideoElementWrapper"/>.</returns>
        public static HtmlVideoElementWrapper CreateHtmlVideoElementWrapper(Func<IHtmlVideoElement?> objectQuery) => new HtmlVideoElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ICharacterData"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="CharacterDataWrapper"/>.</returns>
        public static CharacterDataWrapper CreateCharacterDataWrapper(Func<ICharacterData?> objectQuery) => new CharacterDataWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IComment"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="CommentWrapper"/>.</returns>
        public static CommentWrapper CreateCommentWrapper(Func<IComment?> objectQuery) => new CommentWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IDocument"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="DocumentWrapper"/>.</returns>
        public static DocumentWrapper CreateDocumentWrapper(Func<IDocument?> objectQuery) => new DocumentWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IDocumentFragment"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="DocumentFragmentWrapper"/>.</returns>
        public static DocumentFragmentWrapper CreateDocumentFragmentWrapper(Func<IDocumentFragment?> objectQuery) => new DocumentFragmentWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IDocumentType"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="DocumentTypeWrapper"/>.</returns>
        public static DocumentTypeWrapper CreateDocumentTypeWrapper(Func<IDocumentType?> objectQuery) => new DocumentTypeWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="ElementWrapper"/>.</returns>
        public static ElementWrapper CreateElementWrapper(Func<IElement?> objectQuery) => new ElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IHtmlAllCollection"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="HtmlAllCollectionWrapper"/>.</returns>
        public static HtmlAllCollectionWrapper CreateHtmlAllCollectionWrapper(Func<IHtmlAllCollection?> objectQuery) => new HtmlAllCollectionWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="INamedNodeMap"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="NamedNodeMapWrapper"/>.</returns>
        public static NamedNodeMapWrapper CreateNamedNodeMapWrapper(Func<INamedNodeMap?> objectQuery) => new NamedNodeMapWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="INode"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="NodeWrapper"/>.</returns>
        public static NodeWrapper CreateNodeWrapper(Func<INode?> objectQuery) => new NodeWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="INodeList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="NodeListWrapper"/>.</returns>
        public static NodeListWrapper CreateNodeListWrapper(Func<INodeList?> objectQuery) => new NodeListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IProcessingInstruction"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="ProcessingInstructionWrapper"/>.</returns>
        public static ProcessingInstructionWrapper CreateProcessingInstructionWrapper(Func<IProcessingInstruction?> objectQuery) => new ProcessingInstructionWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IPseudoElement"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="PseudoElementWrapper"/>.</returns>
        public static PseudoElementWrapper CreatePseudoElementWrapper(Func<IPseudoElement?> objectQuery) => new PseudoElementWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ISettableTokenList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="SettableTokenListWrapper"/>.</returns>
        public static SettableTokenListWrapper CreateSettableTokenListWrapper(Func<ISettableTokenList?> objectQuery) => new SettableTokenListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IShadowRoot"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="ShadowRootWrapper"/>.</returns>
        public static ShadowRootWrapper CreateShadowRootWrapper(Func<IShadowRoot?> objectQuery) => new ShadowRootWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IStringList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="StringListWrapper"/>.</returns>
        public static StringListWrapper CreateStringListWrapper(Func<IStringList?> objectQuery) => new StringListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IStringMap"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="StringMapWrapper"/>.</returns>
        public static StringMapWrapper CreateStringMapWrapper(Func<IStringMap?> objectQuery) => new StringMapWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IStyleSheetList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="StyleSheetListWrapper"/>.</returns>
        public static StyleSheetListWrapper CreateStyleSheetListWrapper(Func<IStyleSheetList?> objectQuery) => new StyleSheetListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IText"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="TextWrapper"/>.</returns>
        public static TextWrapper CreateTextWrapper(Func<IText?> objectQuery) => new TextWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="ITokenList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="TokenListWrapper"/>.</returns>
        public static TokenListWrapper CreateTokenListWrapper(Func<ITokenList?> objectQuery) => new TokenListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IMediaList"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="MediaListWrapper"/>.</returns>
        public static MediaListWrapper CreateMediaListWrapper(Func<IMediaList?> objectQuery) => new MediaListWrapper(objectQuery);

        
        /// <summary>
        /// Creates a wrapper for an <see cref="IAttr"/>.
        /// </summary>
        /// <param name="objectQuery">A function that can be used to retrieve a new instance of the wrapped type.</param>
        /// <returns>The <see cref="AttrWrapper"/>.</returns>
        public static AttrWrapper CreateAttrWrapper(Func<IAttr?> objectQuery) => new AttrWrapper(objectQuery);
    }
}
