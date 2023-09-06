using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Svg.Dom;
using AngleSharpWrappers;

namespace Bunit;

internal static class ElementWrapperFactory
{
	public static IElement Create(IElement element, IRenderedFragment renderedFragment, string cssSelector)
	{
		return element switch
		{
			IHtmlAnchorElement htmlAnchorElement => new HtmlAnchorElementWrapper(
				new ElementFactory<IHtmlAnchorElement>(renderedFragment, htmlAnchorElement, cssSelector)),
			IHtmlAreaElement htmlAreaElement => new HtmlAreaElementWrapper(
				new ElementFactory<IHtmlAreaElement>(renderedFragment, htmlAreaElement, cssSelector)),
			IHtmlAudioElement htmlAudioElement => new HtmlAudioElementWrapper(
				new ElementFactory<IHtmlAudioElement>(renderedFragment, htmlAudioElement, cssSelector)),
			IHtmlBaseElement htmlBaseElement => new HtmlBaseElementWrapper(
				new ElementFactory<IHtmlBaseElement>(renderedFragment, htmlBaseElement, cssSelector)),
			IHtmlBodyElement htmlBodyElement => new HtmlBodyElementWrapper(
				new ElementFactory<IHtmlBodyElement>(renderedFragment, htmlBodyElement, cssSelector)),
			IHtmlBreakRowElement htmlBreakRowElement => new HtmlBreakRowElementWrapper(
				new ElementFactory<IHtmlBreakRowElement>(renderedFragment, htmlBreakRowElement, cssSelector)),
			IHtmlButtonElement htmlButtonElement => new HtmlButtonElementWrapper(
				new ElementFactory<IHtmlButtonElement>(renderedFragment, htmlButtonElement, cssSelector)),
			IHtmlCanvasElement htmlCanvasElement => new HtmlCanvasElementWrapper(
				new ElementFactory<IHtmlCanvasElement>(renderedFragment, htmlCanvasElement, cssSelector)),
			IHtmlCommandElement htmlCommandElement => new HtmlCommandElementWrapper(
				new ElementFactory<IHtmlCommandElement>(renderedFragment, htmlCommandElement, cssSelector)),
			IHtmlDataElement htmlDataElement => new HtmlDataElementWrapper(
				new ElementFactory<IHtmlDataElement>(renderedFragment, htmlDataElement, cssSelector)),
			IHtmlDataListElement htmlDataListElement => new HtmlDataListElementWrapper(
				new ElementFactory<IHtmlDataListElement>(renderedFragment, htmlDataListElement, cssSelector)),
			IHtmlDetailsElement htmlDetailsElement => new HtmlDetailsElementWrapper(
				new ElementFactory<IHtmlDetailsElement>(renderedFragment, htmlDetailsElement, cssSelector)),
			IHtmlDialogElement htmlDialogElement => new HtmlDialogElementWrapper(
				new ElementFactory<IHtmlDialogElement>(renderedFragment, htmlDialogElement, cssSelector)),
			IHtmlDivElement htmlDivElement => new HtmlDivElementWrapper(
				new ElementFactory<IHtmlDivElement>(renderedFragment, htmlDivElement, cssSelector)),
			IHtmlEmbedElement htmlEmbedElement => new HtmlEmbedElementWrapper(
				new ElementFactory<IHtmlEmbedElement>(renderedFragment, htmlEmbedElement, cssSelector)),
			IHtmlFieldSetElement htmlFieldSetElement => new HtmlFieldSetElementWrapper(
				new ElementFactory<IHtmlFieldSetElement>(renderedFragment, htmlFieldSetElement, cssSelector)),
			IHtmlFormElement htmlFormElement => new HtmlFormElementWrapper(
				new ElementFactory<IHtmlFormElement>(renderedFragment, htmlFormElement, cssSelector)),
			IHtmlHeadElement htmlHeadElement => new HtmlHeadElementWrapper(
				new ElementFactory<IHtmlHeadElement>(renderedFragment, htmlHeadElement, cssSelector)),
			IHtmlHeadingElement htmlHeadingElement => new HtmlHeadingElementWrapper(
				new ElementFactory<IHtmlHeadingElement>(renderedFragment, htmlHeadingElement, cssSelector)),
			IHtmlHrElement htmlHrElement => new HtmlHrElementWrapper(
				new ElementFactory<IHtmlHrElement>(renderedFragment, htmlHrElement, cssSelector)),
			IHtmlHtmlElement htmlHtmlElement => new HtmlHtmlElementWrapper(
				new ElementFactory<IHtmlHtmlElement>(renderedFragment, htmlHtmlElement, cssSelector)),
			IHtmlImageElement htmlImageElement => new HtmlImageElementWrapper(
				new ElementFactory<IHtmlImageElement>(renderedFragment, htmlImageElement, cssSelector)),
			IHtmlInlineFrameElement htmlInlineFrameElement => new HtmlInlineFrameElementWrapper(
				new ElementFactory<IHtmlInlineFrameElement>(renderedFragment, htmlInlineFrameElement, cssSelector)),
			IHtmlInputElement htmlInputElement => new HtmlInputElementWrapper(
				new ElementFactory<IHtmlInputElement>(renderedFragment, htmlInputElement, cssSelector)),
			IHtmlKeygenElement htmlKeygenElement => new HtmlKeygenElementWrapper(
				new ElementFactory<IHtmlKeygenElement>(renderedFragment, htmlKeygenElement, cssSelector)),
			IHtmlLabelElement htmlLabelElement => new HtmlLabelElementWrapper(
				new ElementFactory<IHtmlLabelElement>(renderedFragment, htmlLabelElement, cssSelector)),
			IHtmlLegendElement htmlLegendElement => new HtmlLegendElementWrapper(
				new ElementFactory<IHtmlLegendElement>(renderedFragment, htmlLegendElement, cssSelector)),
			IHtmlLinkElement htmlLinkElement => new HtmlLinkElementWrapper(
				new ElementFactory<IHtmlLinkElement>(renderedFragment, htmlLinkElement, cssSelector)),
			IHtmlListItemElement htmlListItemElement => new HtmlListItemElementWrapper(
				new ElementFactory<IHtmlListItemElement>(renderedFragment, htmlListItemElement, cssSelector)),
			IHtmlMapElement htmlMapElement => new HtmlMapElementWrapper(
				new ElementFactory<IHtmlMapElement>(renderedFragment, htmlMapElement, cssSelector)),
			IHtmlMarqueeElement htmlMarqueeElement => new HtmlMarqueeElementWrapper(
				new ElementFactory<IHtmlMarqueeElement>(renderedFragment, htmlMarqueeElement, cssSelector)),
			IHtmlMenuElement htmlMenuElement => new HtmlMenuElementWrapper(
				new ElementFactory<IHtmlMenuElement>(renderedFragment, htmlMenuElement, cssSelector)),
			IHtmlMenuItemElement htmlMenuItemElement => new HtmlMenuItemElementWrapper(
				new ElementFactory<IHtmlMenuItemElement>(renderedFragment, htmlMenuItemElement, cssSelector)),
			IHtmlMetaElement htmlMetaElement => new HtmlMetaElementWrapper(
				new ElementFactory<IHtmlMetaElement>(renderedFragment, htmlMetaElement, cssSelector)),
			IHtmlMeterElement htmlMeterElement => new HtmlMeterElementWrapper(
				new ElementFactory<IHtmlMeterElement>(renderedFragment, htmlMeterElement, cssSelector)),
			IHtmlModElement htmlModElement => new HtmlModElementWrapper(
				new ElementFactory<IHtmlModElement>(renderedFragment, htmlModElement, cssSelector)),
			IHtmlObjectElement htmlObjectElement => new HtmlObjectElementWrapper(
				new ElementFactory<IHtmlObjectElement>(renderedFragment, htmlObjectElement, cssSelector)),
			IHtmlOrderedListElement htmlOrderedListElement => new HtmlOrderedListElementWrapper(
				new ElementFactory<IHtmlOrderedListElement>(renderedFragment, htmlOrderedListElement, cssSelector)),
			IHtmlParagraphElement htmlParagraphElement => new HtmlParagraphElementWrapper(
				new ElementFactory<IHtmlParagraphElement>(renderedFragment, htmlParagraphElement, cssSelector)),
			IHtmlParamElement htmlParamElement => new HtmlParamElementWrapper(
				new ElementFactory<IHtmlParamElement>(renderedFragment, htmlParamElement, cssSelector)),
			IHtmlPreElement htmlPreElement => new HtmlPreElementWrapper(
				new ElementFactory<IHtmlPreElement>(renderedFragment, htmlPreElement, cssSelector)),
			IHtmlProgressElement htmlProgressElement => new HtmlProgressElementWrapper(
				new ElementFactory<IHtmlProgressElement>(renderedFragment, htmlProgressElement, cssSelector)),
			IHtmlQuoteElement htmlQuoteElement => new HtmlQuoteElementWrapper(
				new ElementFactory<IHtmlQuoteElement>(renderedFragment, htmlQuoteElement, cssSelector)),
			IHtmlScriptElement htmlScriptElement => new HtmlScriptElementWrapper(
				new ElementFactory<IHtmlScriptElement>(renderedFragment, htmlScriptElement, cssSelector)),
			IHtmlSelectElement htmlSelectElement => new HtmlSelectElementWrapper(
				new ElementFactory<IHtmlSelectElement>(renderedFragment, htmlSelectElement, cssSelector)),
			IHtmlSourceElement htmlSourceElement => new HtmlSourceElementWrapper(
				new ElementFactory<IHtmlSourceElement>(renderedFragment, htmlSourceElement, cssSelector)),
			IHtmlSpanElement htmlSpanElement => new HtmlSpanElementWrapper(
				new ElementFactory<IHtmlSpanElement>(renderedFragment, htmlSpanElement, cssSelector)),
			IHtmlStyleElement htmlStyleElement => new HtmlStyleElementWrapper(
				new ElementFactory<IHtmlStyleElement>(renderedFragment, htmlStyleElement, cssSelector)),
			IHtmlTableCaptionElement htmlTableCaptionElement => new HtmlTableCaptionElementWrapper(
				new ElementFactory<IHtmlTableCaptionElement>(renderedFragment, htmlTableCaptionElement, cssSelector)),
			IHtmlTableCellElement htmlTableCellElement => new HtmlTableCellElementWrapper(
				new ElementFactory<IHtmlTableCellElement>(renderedFragment, htmlTableCellElement, cssSelector)),
			IHtmlTableElement htmlTableElement => new HtmlTableElementWrapper(
				new ElementFactory<IHtmlTableElement>(renderedFragment, htmlTableElement, cssSelector)),
			IHtmlTableRowElement htmlTableRowElement => new HtmlTableRowElementWrapper(
				new ElementFactory<IHtmlTableRowElement>(renderedFragment, htmlTableRowElement, cssSelector)),
			IHtmlTableSectionElement htmlTableSectionElement => new HtmlTableSectionElementWrapper(
				new ElementFactory<IHtmlTableSectionElement>(renderedFragment, htmlTableSectionElement, cssSelector)),
			IHtmlTemplateElement htmlTemplateElement => new HtmlTemplateElementWrapper(
				new ElementFactory<IHtmlTemplateElement>(renderedFragment, htmlTemplateElement, cssSelector)),
			IHtmlTextAreaElement htmlTextAreaElement => new HtmlTextAreaElementWrapper(
				new ElementFactory<IHtmlTextAreaElement>(renderedFragment, htmlTextAreaElement, cssSelector)),
			IHtmlTimeElement htmlTimeElement => new HtmlTimeElementWrapper(
				new ElementFactory<IHtmlTimeElement>(renderedFragment, htmlTimeElement, cssSelector)),
			IHtmlTitleElement htmlTitleElement => new HtmlTitleElementWrapper(
				new ElementFactory<IHtmlTitleElement>(renderedFragment, htmlTitleElement, cssSelector)),
			IHtmlTrackElement htmlTrackElement => new HtmlTrackElementWrapper(
				new ElementFactory<IHtmlTrackElement>(renderedFragment, htmlTrackElement, cssSelector)),
			IHtmlUnknownElement htmlUnknownElement => new HtmlUnknownElementWrapper(
				new ElementFactory<IHtmlUnknownElement>(renderedFragment, htmlUnknownElement, cssSelector)),
			IHtmlVideoElement htmlVideoElement => new HtmlVideoElementWrapper(
				new ElementFactory<IHtmlVideoElement>(renderedFragment, htmlVideoElement, cssSelector)),
			IHtmlMediaElement htmlMediaElement => new HtmlMediaElementWrapper(
				new ElementFactory<IHtmlMediaElement>(renderedFragment, htmlMediaElement, cssSelector)),
			IPseudoElement pseudoElement => new PseudoElementWrapper(
				new ElementFactory<IPseudoElement>(renderedFragment, pseudoElement, cssSelector)),
			ISvgCircleElement svgCircleElement => new SvgCircleElementWrapper(
				new ElementFactory<ISvgCircleElement>(renderedFragment, svgCircleElement, cssSelector)),
			ISvgDescriptionElement svgDescriptionElement => new SvgDescriptionElementWrapper(
				new ElementFactory<ISvgDescriptionElement>(renderedFragment, svgDescriptionElement, cssSelector)),
			ISvgForeignObjectElement svgForeignObjectElement => new SvgForeignObjectElementWrapper(
				new ElementFactory<ISvgForeignObjectElement>(renderedFragment, svgForeignObjectElement, cssSelector)),
			ISvgSvgElement svgSvgElement => new SvgSvgElementWrapper(
				new ElementFactory<ISvgSvgElement>(renderedFragment, svgSvgElement, cssSelector)),
			ISvgTitleElement svgTitleElement => new SvgTitleElementWrapper(
				new ElementFactory<ISvgTitleElement>(renderedFragment, svgTitleElement, cssSelector)),
			ISvgElement svgElement => new SvgElementWrapper(
				new ElementFactory<ISvgElement>(renderedFragment, svgElement, cssSelector)),
			IHtmlElement htmlElement => new HtmlElementWrapper(
				new ElementFactory<IHtmlElement>(renderedFragment, htmlElement, cssSelector)),
			_ => new ElementWrapper(
				new ElementFactory<IElement>(renderedFragment, element, cssSelector)),
		};
	}
}
