using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Svg.Dom;
using AngleSharpWrappers;

namespace Bunit;

internal static class ElementWrapperFactory
{
	public static IElement CreateByLabelText(IElement element, IRenderedFragment renderedFragment, string labelText)
	{
		return element switch
		{
			IHtmlAnchorElement htmlAnchorElement => new HtmlAnchorElementWrapper(
				new ByLabelTextElementFactory<IHtmlAnchorElement>(renderedFragment, htmlAnchorElement, labelText)),
			IHtmlAreaElement htmlAreaElement => new HtmlAreaElementWrapper(
				new ByLabelTextElementFactory<IHtmlAreaElement>(renderedFragment, htmlAreaElement, labelText)),
			IHtmlAudioElement htmlAudioElement => new HtmlAudioElementWrapper(
				new ByLabelTextElementFactory<IHtmlAudioElement>(renderedFragment, htmlAudioElement, labelText)),
			IHtmlBaseElement htmlBaseElement => new HtmlBaseElementWrapper(
				new ByLabelTextElementFactory<IHtmlBaseElement>(renderedFragment, htmlBaseElement, labelText)),
			IHtmlBodyElement htmlBodyElement => new HtmlBodyElementWrapper(
				new ByLabelTextElementFactory<IHtmlBodyElement>(renderedFragment, htmlBodyElement, labelText)),
			IHtmlBreakRowElement htmlBreakRowElement => new HtmlBreakRowElementWrapper(
				new ByLabelTextElementFactory<IHtmlBreakRowElement>(renderedFragment, htmlBreakRowElement, labelText)),
			IHtmlButtonElement htmlButtonElement => new HtmlButtonElementWrapper(
				new ByLabelTextElementFactory<IHtmlButtonElement>(renderedFragment, htmlButtonElement, labelText)),
			IHtmlCanvasElement htmlCanvasElement => new HtmlCanvasElementWrapper(
				new ByLabelTextElementFactory<IHtmlCanvasElement>(renderedFragment, htmlCanvasElement, labelText)),
			IHtmlCommandElement htmlCommandElement => new HtmlCommandElementWrapper(
				new ByLabelTextElementFactory<IHtmlCommandElement>(renderedFragment, htmlCommandElement, labelText)),
			IHtmlDataElement htmlDataElement => new HtmlDataElementWrapper(
				new ByLabelTextElementFactory<IHtmlDataElement>(renderedFragment, htmlDataElement, labelText)),
			IHtmlDataListElement htmlDataListElement => new HtmlDataListElementWrapper(
				new ByLabelTextElementFactory<IHtmlDataListElement>(renderedFragment, htmlDataListElement, labelText)),
			IHtmlDetailsElement htmlDetailsElement => new HtmlDetailsElementWrapper(
				new ByLabelTextElementFactory<IHtmlDetailsElement>(renderedFragment, htmlDetailsElement, labelText)),
			IHtmlDialogElement htmlDialogElement => new HtmlDialogElementWrapper(
				new ByLabelTextElementFactory<IHtmlDialogElement>(renderedFragment, htmlDialogElement, labelText)),
			IHtmlDivElement htmlDivElement => new HtmlDivElementWrapper(
				new ByLabelTextElementFactory<IHtmlDivElement>(renderedFragment, htmlDivElement, labelText)),
			IHtmlEmbedElement htmlEmbedElement => new HtmlEmbedElementWrapper(
				new ByLabelTextElementFactory<IHtmlEmbedElement>(renderedFragment, htmlEmbedElement, labelText)),
			IHtmlFieldSetElement htmlFieldSetElement => new HtmlFieldSetElementWrapper(
				new ByLabelTextElementFactory<IHtmlFieldSetElement>(renderedFragment, htmlFieldSetElement, labelText)),
			IHtmlFormElement htmlFormElement => new HtmlFormElementWrapper(
				new ByLabelTextElementFactory<IHtmlFormElement>(renderedFragment, htmlFormElement, labelText)),
			IHtmlHeadElement htmlHeadElement => new HtmlHeadElementWrapper(
				new ByLabelTextElementFactory<IHtmlHeadElement>(renderedFragment, htmlHeadElement, labelText)),
			IHtmlHeadingElement htmlHeadingElement => new HtmlHeadingElementWrapper(
				new ByLabelTextElementFactory<IHtmlHeadingElement>(renderedFragment, htmlHeadingElement, labelText)),
			IHtmlHrElement htmlHrElement => new HtmlHrElementWrapper(
				new ByLabelTextElementFactory<IHtmlHrElement>(renderedFragment, htmlHrElement, labelText)),
			IHtmlHtmlElement htmlHtmlElement => new HtmlHtmlElementWrapper(
				new ByLabelTextElementFactory<IHtmlHtmlElement>(renderedFragment, htmlHtmlElement, labelText)),
			IHtmlImageElement htmlImageElement => new HtmlImageElementWrapper(
				new ByLabelTextElementFactory<IHtmlImageElement>(renderedFragment, htmlImageElement, labelText)),
			IHtmlInlineFrameElement htmlInlineFrameElement => new HtmlInlineFrameElementWrapper(
				new ByLabelTextElementFactory<IHtmlInlineFrameElement>(renderedFragment, htmlInlineFrameElement, labelText)),
			IHtmlInputElement htmlInputElement => new HtmlInputElementWrapper(
				new ByLabelTextElementFactory<IHtmlInputElement>(renderedFragment, htmlInputElement, labelText)),
			IHtmlKeygenElement htmlKeygenElement => new HtmlKeygenElementWrapper(
				new ByLabelTextElementFactory<IHtmlKeygenElement>(renderedFragment, htmlKeygenElement, labelText)),
			IHtmlLabelElement htmlLabelElement => new HtmlLabelElementWrapper(
				new ByLabelTextElementFactory<IHtmlLabelElement>(renderedFragment, htmlLabelElement, labelText)),
			IHtmlLegendElement htmlLegendElement => new HtmlLegendElementWrapper(
				new ByLabelTextElementFactory<IHtmlLegendElement>(renderedFragment, htmlLegendElement, labelText)),
			IHtmlLinkElement htmlLinkElement => new HtmlLinkElementWrapper(
				new ByLabelTextElementFactory<IHtmlLinkElement>(renderedFragment, htmlLinkElement, labelText)),
			IHtmlListItemElement htmlListItemElement => new HtmlListItemElementWrapper(
				new ByLabelTextElementFactory<IHtmlListItemElement>(renderedFragment, htmlListItemElement, labelText)),
			IHtmlMapElement htmlMapElement => new HtmlMapElementWrapper(
				new ByLabelTextElementFactory<IHtmlMapElement>(renderedFragment, htmlMapElement, labelText)),
			IHtmlMarqueeElement htmlMarqueeElement => new HtmlMarqueeElementWrapper(
				new ByLabelTextElementFactory<IHtmlMarqueeElement>(renderedFragment, htmlMarqueeElement, labelText)),
			IHtmlMenuElement htmlMenuElement => new HtmlMenuElementWrapper(
				new ByLabelTextElementFactory<IHtmlMenuElement>(renderedFragment, htmlMenuElement, labelText)),
			IHtmlMenuItemElement htmlMenuItemElement => new HtmlMenuItemElementWrapper(
				new ByLabelTextElementFactory<IHtmlMenuItemElement>(renderedFragment, htmlMenuItemElement, labelText)),
			IHtmlMetaElement htmlMetaElement => new HtmlMetaElementWrapper(
				new ByLabelTextElementFactory<IHtmlMetaElement>(renderedFragment, htmlMetaElement, labelText)),
			IHtmlMeterElement htmlMeterElement => new HtmlMeterElementWrapper(
				new ByLabelTextElementFactory<IHtmlMeterElement>(renderedFragment, htmlMeterElement, labelText)),
			IHtmlModElement htmlModElement => new HtmlModElementWrapper(
				new ByLabelTextElementFactory<IHtmlModElement>(renderedFragment, htmlModElement, labelText)),
			IHtmlObjectElement htmlObjectElement => new HtmlObjectElementWrapper(
				new ByLabelTextElementFactory<IHtmlObjectElement>(renderedFragment, htmlObjectElement, labelText)),
			IHtmlOrderedListElement htmlOrderedListElement => new HtmlOrderedListElementWrapper(
				new ByLabelTextElementFactory<IHtmlOrderedListElement>(renderedFragment, htmlOrderedListElement, labelText)),
			IHtmlParagraphElement htmlParagraphElement => new HtmlParagraphElementWrapper(
				new ByLabelTextElementFactory<IHtmlParagraphElement>(renderedFragment, htmlParagraphElement, labelText)),
			IHtmlParamElement htmlParamElement => new HtmlParamElementWrapper(
				new ByLabelTextElementFactory<IHtmlParamElement>(renderedFragment, htmlParamElement, labelText)),
			IHtmlPreElement htmlPreElement => new HtmlPreElementWrapper(
				new ByLabelTextElementFactory<IHtmlPreElement>(renderedFragment, htmlPreElement, labelText)),
			IHtmlProgressElement htmlProgressElement => new HtmlProgressElementWrapper(
				new ByLabelTextElementFactory<IHtmlProgressElement>(renderedFragment, htmlProgressElement, labelText)),
			IHtmlQuoteElement htmlQuoteElement => new HtmlQuoteElementWrapper(
				new ByLabelTextElementFactory<IHtmlQuoteElement>(renderedFragment, htmlQuoteElement, labelText)),
			IHtmlScriptElement htmlScriptElement => new HtmlScriptElementWrapper(
				new ByLabelTextElementFactory<IHtmlScriptElement>(renderedFragment, htmlScriptElement, labelText)),
			IHtmlSelectElement htmlSelectElement => new HtmlSelectElementWrapper(
				new ByLabelTextElementFactory<IHtmlSelectElement>(renderedFragment, htmlSelectElement, labelText)),
			IHtmlSourceElement htmlSourceElement => new HtmlSourceElementWrapper(
				new ByLabelTextElementFactory<IHtmlSourceElement>(renderedFragment, htmlSourceElement, labelText)),
			IHtmlSpanElement htmlSpanElement => new HtmlSpanElementWrapper(
				new ByLabelTextElementFactory<IHtmlSpanElement>(renderedFragment, htmlSpanElement, labelText)),
			IHtmlStyleElement htmlStyleElement => new HtmlStyleElementWrapper(
				new ByLabelTextElementFactory<IHtmlStyleElement>(renderedFragment, htmlStyleElement, labelText)),
			IHtmlTableCaptionElement htmlTableCaptionElement => new HtmlTableCaptionElementWrapper(
				new ByLabelTextElementFactory<IHtmlTableCaptionElement>(renderedFragment, htmlTableCaptionElement, labelText)),
			IHtmlTableCellElement htmlTableCellElement => new HtmlTableCellElementWrapper(
				new ByLabelTextElementFactory<IHtmlTableCellElement>(renderedFragment, htmlTableCellElement, labelText)),
			IHtmlTableElement htmlTableElement => new HtmlTableElementWrapper(
				new ByLabelTextElementFactory<IHtmlTableElement>(renderedFragment, htmlTableElement, labelText)),
			IHtmlTableRowElement htmlTableRowElement => new HtmlTableRowElementWrapper(
				new ByLabelTextElementFactory<IHtmlTableRowElement>(renderedFragment, htmlTableRowElement, labelText)),
			IHtmlTableSectionElement htmlTableSectionElement => new HtmlTableSectionElementWrapper(
				new ByLabelTextElementFactory<IHtmlTableSectionElement>(renderedFragment, htmlTableSectionElement, labelText)),
			IHtmlTemplateElement htmlTemplateElement => new HtmlTemplateElementWrapper(
				new ByLabelTextElementFactory<IHtmlTemplateElement>(renderedFragment, htmlTemplateElement, labelText)),
			IHtmlTextAreaElement htmlTextAreaElement => new HtmlTextAreaElementWrapper(
				new ByLabelTextElementFactory<IHtmlTextAreaElement>(renderedFragment, htmlTextAreaElement, labelText)),
			IHtmlTimeElement htmlTimeElement => new HtmlTimeElementWrapper(
				new ByLabelTextElementFactory<IHtmlTimeElement>(renderedFragment, htmlTimeElement, labelText)),
			IHtmlTitleElement htmlTitleElement => new HtmlTitleElementWrapper(
				new ByLabelTextElementFactory<IHtmlTitleElement>(renderedFragment, htmlTitleElement, labelText)),
			IHtmlTrackElement htmlTrackElement => new HtmlTrackElementWrapper(
				new ByLabelTextElementFactory<IHtmlTrackElement>(renderedFragment, htmlTrackElement, labelText)),
			IHtmlUnknownElement htmlUnknownElement => new HtmlUnknownElementWrapper(
				new ByLabelTextElementFactory<IHtmlUnknownElement>(renderedFragment, htmlUnknownElement, labelText)),
			IHtmlVideoElement htmlVideoElement => new HtmlVideoElementWrapper(
				new ByLabelTextElementFactory<IHtmlVideoElement>(renderedFragment, htmlVideoElement, labelText)),
			IHtmlMediaElement htmlMediaElement => new HtmlMediaElementWrapper(
				new ByLabelTextElementFactory<IHtmlMediaElement>(renderedFragment, htmlMediaElement, labelText)),
			IPseudoElement pseudoElement => new PseudoElementWrapper(
				new ByLabelTextElementFactory<IPseudoElement>(renderedFragment, pseudoElement, labelText)),
			ISvgCircleElement svgCircleElement => new SvgCircleElementWrapper(
				new ByLabelTextElementFactory<ISvgCircleElement>(renderedFragment, svgCircleElement, labelText)),
			ISvgDescriptionElement svgDescriptionElement => new SvgDescriptionElementWrapper(
				new ByLabelTextElementFactory<ISvgDescriptionElement>(renderedFragment, svgDescriptionElement, labelText)),
			ISvgForeignObjectElement svgForeignObjectElement => new SvgForeignObjectElementWrapper(
				new ByLabelTextElementFactory<ISvgForeignObjectElement>(renderedFragment, svgForeignObjectElement, labelText)),
			ISvgSvgElement svgSvgElement => new SvgSvgElementWrapper(
				new ByLabelTextElementFactory<ISvgSvgElement>(renderedFragment, svgSvgElement, labelText)),
			ISvgTitleElement svgTitleElement => new SvgTitleElementWrapper(
				new ByLabelTextElementFactory<ISvgTitleElement>(renderedFragment, svgTitleElement, labelText)),
			ISvgElement svgElement => new SvgElementWrapper(
				new ByLabelTextElementFactory<ISvgElement>(renderedFragment, svgElement, labelText)),
			IHtmlElement htmlElement => new HtmlElementWrapper(
				new ByLabelTextElementFactory<IHtmlElement>(renderedFragment, htmlElement, labelText)),
			_ => new ElementWrapper(
				new ByLabelTextElementFactory<IElement>(renderedFragment, element, labelText)),
		};
	}
}
