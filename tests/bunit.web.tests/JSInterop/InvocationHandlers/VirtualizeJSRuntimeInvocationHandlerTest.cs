#if NET5_0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Shouldly;
using Xunit;

namespace Bunit.JSInterop.ComponentSupport
{
	public class VirtualizeJSRuntimeInvocationHandlerTest : TestContext
	{
		public static readonly IEnumerable<object[]> ItemsInCollection = new object[][]
		{
			new object[] { 0 }, new object[] { 7 }, new object[] { 30 }, new object[] { 60 }, new object[] { 100 }, new object[] { 300 }, new object[] { 500 },
		};

		[Theory(DisplayName = "Can render component using <Virtualize Items> with ChildContent")]
		[MemberData(nameof(ItemsInCollection))]
		public void Test001(int itemsInDataSource)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.Items, CreateItems(itemsInDataSource))
				.Add(p => p.ChildContent, item => $"<p>{item}</p>"));

			cut.FindAll("p").Count.ShouldBe(itemsInDataSource);
		}

		[Theory(DisplayName = "Can render component using <Virtualize Items> with ItemContent")]
		[MemberData(nameof(ItemsInCollection))]
		public void Test002(int itemsInDataSource)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.Items, CreateItems(itemsInDataSource))
				.Add(p => p.ItemContent, item => $"<p>{item}</p>"));

			cut.FindAll("p").Count.ShouldBe(itemsInDataSource);
		}

		[Theory(DisplayName = "Can render component using <Virtualize ItemsProvider> with ChildContent")]
		[MemberData(nameof(ItemsInCollection))]
		public void Test010(int itemsInDataSource)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.ItemsProvider, CreateItemsProvider(itemsInDataSource))
				.Add(p => p.ChildContent, item => $"<p>{item}</p>"));

			cut.FindAll("p").Count.ShouldBe(itemsInDataSource);
		}

		[Theory(DisplayName = "Can render component using <Virtualize ItemsProvider> with ItemContent")]
		[MemberData(nameof(ItemsInCollection))]
		public void Test011(int itemsInDataSource)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.ItemsProvider, CreateItemsProvider(itemsInDataSource))
				.Add(p => p.ItemContent, item => $"<p>{item}</p>"));

			cut.FindAll("p").Count.ShouldBe(itemsInDataSource);
		}

		public static readonly IEnumerable<object[]> ItemCountItemSizeOverscanCount =
			ItemsInCollection.Select(x => new object[][]
			{
				new object[] { x[0], 1, 3 },
				new object[] { x[0], 1_000_000, 3 },
				new object[] { x[0], 50, 1 },
				new object[] { x[0], 50, 1_000_000 },
				new object[] { x[0], 1, 1 },
				new object[] { x[0], 1_000_000, 1_000_000 },
			}).SelectMany(x => x);

		[Theory(DisplayName = "Can render component using <Virtualize Items> and different ItemSize and OverscanCount")]
		[MemberData(nameof(ItemCountItemSizeOverscanCount))]
		public void Test030(int itemsInDataSource, float itemSize, int overscanCount)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.ItemsProvider, CreateItemsProvider(itemsInDataSource))
				.Add(p => p.ItemContent, item => $"<p>{item}</p>")
				.Add(p => p.ItemSize, itemSize)
				.Add(p => p.OverscanCount, overscanCount));

			cut.FindAll("p").Count.ShouldBe(itemsInDataSource);
		}

		[Theory(DisplayName = "Can render placeholder from <Virtualize ItemsProvider, Placeholder>")]
		[MemberData(nameof(ItemsInCollection))]
		public void Test040(int itemsInDataSource)
		{
			var cut = RenderComponent<Virtualize<string>>(ps => ps
				.Add(p => p.ItemsProvider, _ => ValueTask.FromResult(new ItemsProviderResult<string>(Array.Empty<string>(), itemsInDataSource)))
				.Add(p => p.ItemContent, item => @$"<p class=""item"">{item}</p>")
				.Add(p => p.Placeholder, item => @"<p class=""placeholder"" />"));

			cut.FindAll(".placeholder").Count.ShouldBe(itemsInDataSource);
			cut.FindAll(".item").Count.ShouldBe(0);
		}

		private static ICollection<string> CreateItems(int itemsToCreate)
			=> Enumerable.Range(0, itemsToCreate).Select(x => Guid.NewGuid().ToString()).ToArray();

		private static ItemsProviderDelegate<string> CreateItemsProvider(int itemsInCollection)
		{
			return request =>
			{
				// Count can be very big, so avoid creating unnecessary items by taking the smallest of them.
				var itemsToCreate = Math.Min(request.Count, itemsInCollection);
				var result = CreateItems(itemsToCreate);
				return ValueTask.FromResult(new ItemsProviderResult<string>(result, itemsInCollection));
			};
		}
	}
}

#endif
