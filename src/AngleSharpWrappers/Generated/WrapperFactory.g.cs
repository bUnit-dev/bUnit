using System;
using System.Collections.Generic;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Svg.Dom;
namespace AngleSharpWrappers
{
    #nullable enable
    /// <summary>
    /// A factory for creating element wrappers.
    /// </summary>
    public static class WrapperFactory
    {
		/// <summary>
		/// Wraps an <see cref="IElement"/> in the wrapper specific to it.
		/// </summary>
		/// <typeparam name="T">The element type.</typeparam>
		/// <param name="factory">The factory that provides the wrapped element.</param>
		/// <returns>The wrapped <see cref="IElement"/>.</returns>
		public static T Create<T>(IElementFactory<T> factory) where T : class, INode
        {
            IElement result = factory switch
            {
                IElementFactory<IHtmlAnchorElement> htmlAnchorElementFactory => new HtmlAnchorElementWrapper(htmlAnchorElementFactory),
                IElementFactory<IHtmlAreaElement> htmlAreaElementFactory => new HtmlAreaElementWrapper(htmlAreaElementFactory),
                IElementFactory<IHtmlAudioElement> htmlAudioElementFactory => new HtmlAudioElementWrapper(htmlAudioElementFactory),
                IElementFactory<IHtmlBaseElement> htmlBaseElementFactory => new HtmlBaseElementWrapper(htmlBaseElementFactory),
                IElementFactory<IHtmlBodyElement> htmlBodyElementFactory => new HtmlBodyElementWrapper(htmlBodyElementFactory),
                IElementFactory<IHtmlBreakRowElement> htmlBreakRowElementFactory => new HtmlBreakRowElementWrapper(htmlBreakRowElementFactory),
                IElementFactory<IHtmlButtonElement> htmlButtonElementFactory => new HtmlButtonElementWrapper(htmlButtonElementFactory),
                IElementFactory<IHtmlCanvasElement> htmlCanvasElementFactory => new HtmlCanvasElementWrapper(htmlCanvasElementFactory),
                IElementFactory<IHtmlCommandElement> htmlCommandElementFactory => new HtmlCommandElementWrapper(htmlCommandElementFactory),
                IElementFactory<IHtmlDataElement> htmlDataElementFactory => new HtmlDataElementWrapper(htmlDataElementFactory),
                IElementFactory<IHtmlDataListElement> htmlDataListElementFactory => new HtmlDataListElementWrapper(htmlDataListElementFactory),
                IElementFactory<IHtmlDetailsElement> htmlDetailsElementFactory => new HtmlDetailsElementWrapper(htmlDetailsElementFactory),
                IElementFactory<IHtmlDialogElement> htmlDialogElementFactory => new HtmlDialogElementWrapper(htmlDialogElementFactory),
                IElementFactory<IHtmlDivElement> htmlDivElementFactory => new HtmlDivElementWrapper(htmlDivElementFactory),
                IElementFactory<IHtmlEmbedElement> htmlEmbedElementFactory => new HtmlEmbedElementWrapper(htmlEmbedElementFactory),
                IElementFactory<IHtmlFieldSetElement> htmlFieldSetElementFactory => new HtmlFieldSetElementWrapper(htmlFieldSetElementFactory),
                IElementFactory<IHtmlFormElement> htmlFormElementFactory => new HtmlFormElementWrapper(htmlFormElementFactory),
                IElementFactory<IHtmlHeadElement> htmlHeadElementFactory => new HtmlHeadElementWrapper(htmlHeadElementFactory),
                IElementFactory<IHtmlHeadingElement> htmlHeadingElementFactory => new HtmlHeadingElementWrapper(htmlHeadingElementFactory),
                IElementFactory<IHtmlHrElement> htmlHrElementFactory => new HtmlHrElementWrapper(htmlHrElementFactory),
                IElementFactory<IHtmlHtmlElement> htmlHtmlElementFactory => new HtmlHtmlElementWrapper(htmlHtmlElementFactory),
                IElementFactory<IHtmlImageElement> htmlImageElementFactory => new HtmlImageElementWrapper(htmlImageElementFactory),
                IElementFactory<IHtmlInlineFrameElement> htmlInlineFrameElementFactory => new HtmlInlineFrameElementWrapper(htmlInlineFrameElementFactory),
                IElementFactory<IHtmlInputElement> htmlInputElementFactory => new HtmlInputElementWrapper(htmlInputElementFactory),
                IElementFactory<IHtmlKeygenElement> htmlKeygenElementFactory => new HtmlKeygenElementWrapper(htmlKeygenElementFactory),
                IElementFactory<IHtmlLabelElement> htmlLabelElementFactory => new HtmlLabelElementWrapper(htmlLabelElementFactory),
                IElementFactory<IHtmlLegendElement> htmlLegendElementFactory => new HtmlLegendElementWrapper(htmlLegendElementFactory),
                IElementFactory<IHtmlLinkElement> htmlLinkElementFactory => new HtmlLinkElementWrapper(htmlLinkElementFactory),
                IElementFactory<IHtmlListItemElement> htmlListItemElementFactory => new HtmlListItemElementWrapper(htmlListItemElementFactory),
                IElementFactory<IHtmlMapElement> htmlMapElementFactory => new HtmlMapElementWrapper(htmlMapElementFactory),
                IElementFactory<IHtmlMarqueeElement> htmlMarqueeElementFactory => new HtmlMarqueeElementWrapper(htmlMarqueeElementFactory),
                IElementFactory<IHtmlMenuElement> htmlMenuElementFactory => new HtmlMenuElementWrapper(htmlMenuElementFactory),
                IElementFactory<IHtmlMenuItemElement> htmlMenuItemElementFactory => new HtmlMenuItemElementWrapper(htmlMenuItemElementFactory),
                IElementFactory<IHtmlMetaElement> htmlMetaElementFactory => new HtmlMetaElementWrapper(htmlMetaElementFactory),
                IElementFactory<IHtmlMeterElement> htmlMeterElementFactory => new HtmlMeterElementWrapper(htmlMeterElementFactory),
                IElementFactory<IHtmlModElement> htmlModElementFactory => new HtmlModElementWrapper(htmlModElementFactory),
                IElementFactory<IHtmlObjectElement> htmlObjectElementFactory => new HtmlObjectElementWrapper(htmlObjectElementFactory),
                IElementFactory<IHtmlOptionElement> htmlOptionElementFactory => new HtmlOptionElementWrapper(htmlOptionElementFactory),
                IElementFactory<IHtmlOptionsGroupElement> htmlOptionsGroupElementFactory => new HtmlOptionsGroupElementWrapper(htmlOptionsGroupElementFactory),
                IElementFactory<IHtmlOrderedListElement> htmlOrderedListElementFactory => new HtmlOrderedListElementWrapper(htmlOrderedListElementFactory),
                IElementFactory<IHtmlOutputElement> htmlOutputElementFactory => new HtmlOutputElementWrapper(htmlOutputElementFactory),
                IElementFactory<IHtmlParagraphElement> htmlParagraphElementFactory => new HtmlParagraphElementWrapper(htmlParagraphElementFactory),
                IElementFactory<IHtmlParamElement> htmlParamElementFactory => new HtmlParamElementWrapper(htmlParamElementFactory),
                IElementFactory<IHtmlPreElement> htmlPreElementFactory => new HtmlPreElementWrapper(htmlPreElementFactory),
                IElementFactory<IHtmlProgressElement> htmlProgressElementFactory => new HtmlProgressElementWrapper(htmlProgressElementFactory),
                IElementFactory<IHtmlQuoteElement> htmlQuoteElementFactory => new HtmlQuoteElementWrapper(htmlQuoteElementFactory),
                IElementFactory<IHtmlScriptElement> htmlScriptElementFactory => new HtmlScriptElementWrapper(htmlScriptElementFactory),
                IElementFactory<IHtmlSelectElement> htmlSelectElementFactory => new HtmlSelectElementWrapper(htmlSelectElementFactory),
                IElementFactory<IHtmlSlotElement> htmlSlotElementFactory => new HtmlSlotElementWrapper(htmlSlotElementFactory),
                IElementFactory<IHtmlSourceElement> htmlSourceElementFactory => new HtmlSourceElementWrapper(htmlSourceElementFactory),
                IElementFactory<IHtmlSpanElement> htmlSpanElementFactory => new HtmlSpanElementWrapper(htmlSpanElementFactory),
                IElementFactory<IHtmlStyleElement> htmlStyleElementFactory => new HtmlStyleElementWrapper(htmlStyleElementFactory),
                IElementFactory<IHtmlTableCaptionElement> htmlTableCaptionElementFactory => new HtmlTableCaptionElementWrapper(htmlTableCaptionElementFactory),
                IElementFactory<IHtmlTableColumnElement> htmlTableColumnElementFactory => new HtmlTableColumnElementWrapper(htmlTableColumnElementFactory),
                IElementFactory<IHtmlTableDataCellElement> htmlTableDataCellElementFactory => new HtmlTableDataCellElementWrapper(htmlTableDataCellElementFactory),
                IElementFactory<IHtmlTableElement> htmlTableElementFactory => new HtmlTableElementWrapper(htmlTableElementFactory),
                IElementFactory<IHtmlTableHeaderCellElement> htmlTableHeaderCellElementFactory => new HtmlTableHeaderCellElementWrapper(htmlTableHeaderCellElementFactory),
                IElementFactory<IHtmlTableRowElement> htmlTableRowElementFactory => new HtmlTableRowElementWrapper(htmlTableRowElementFactory),
                IElementFactory<IHtmlTableSectionElement> htmlTableSectionElementFactory => new HtmlTableSectionElementWrapper(htmlTableSectionElementFactory),
                IElementFactory<IHtmlTemplateElement> htmlTemplateElementFactory => new HtmlTemplateElementWrapper(htmlTemplateElementFactory),
                IElementFactory<IHtmlTextAreaElement> htmlTextAreaElementFactory => new HtmlTextAreaElementWrapper(htmlTextAreaElementFactory),
                IElementFactory<IHtmlTimeElement> htmlTimeElementFactory => new HtmlTimeElementWrapper(htmlTimeElementFactory),
                IElementFactory<IHtmlTitleElement> htmlTitleElementFactory => new HtmlTitleElementWrapper(htmlTitleElementFactory),
                IElementFactory<IHtmlTrackElement> htmlTrackElementFactory => new HtmlTrackElementWrapper(htmlTrackElementFactory),
                IElementFactory<IHtmlUnknownElement> htmlUnknownElementFactory => new HtmlUnknownElementWrapper(htmlUnknownElementFactory),
                IElementFactory<IHtmlUnorderedListElement> htmlUnorderedListElementFactory => new HtmlUnorderedListElementWrapper(htmlUnorderedListElementFactory),
                IElementFactory<IHtmlVideoElement> htmlVideoElementFactory => new HtmlVideoElementWrapper(htmlVideoElementFactory),
                IElementFactory<IPseudoElement> pseudoElementFactory => new PseudoElementWrapper(pseudoElementFactory),
                IElementFactory<ISvgCircleElement> svgCircleElementFactory => new SvgCircleElementWrapper(svgCircleElementFactory),
                IElementFactory<ISvgDescriptionElement> svgDescriptionElementFactory => new SvgDescriptionElementWrapper(svgDescriptionElementFactory),
                IElementFactory<ISvgForeignObjectElement> svgForeignObjectElementFactory => new SvgForeignObjectElementWrapper(svgForeignObjectElementFactory),
                IElementFactory<ISvgSvgElement> svgSvgElementFactory => new SvgSvgElementWrapper(svgSvgElementFactory),
                IElementFactory<ISvgTitleElement> svgTitleElementFactory => new SvgTitleElementWrapper(svgTitleElementFactory),
                IElementFactory<IHtmlMediaElement> htmlMediaElementFactory => new HtmlMediaElementWrapper(htmlMediaElementFactory),
                IElementFactory<IHtmlTableCellElement> htmlTableCellElementFactory => new HtmlTableCellElementWrapper(htmlTableCellElementFactory),
                IElementFactory<ISvgElement> svgElementFactory => new SvgElementWrapper(svgElementFactory),
                IElementFactory<IHtmlElement> htmlElementFactory => new HtmlElementWrapper(htmlElementFactory),
                IElementFactory<IElement> elementFactory => new ElementWrapper(elementFactory),
                _ => throw new InvalidOperationException($"Unknown type. Cannot create wrapper for {typeof(T)}"),
            };
            return (T)result;
        }
    }
}
