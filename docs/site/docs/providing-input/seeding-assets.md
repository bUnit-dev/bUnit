---
uid: seeding-assets
title: Seeding static assets (Assets)
---

# Seeding static assets (`Assets`)

This article explains how to seed the `Assets` property of components under test in bUnit. This is supported for .NET 9 and later.

Since .NET 9, components can access static assets mapped by [`MapStaticAssets`](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/map-static-files?view=aspnetcore-9.0) through the [`ComponentBase.Assets`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.componentbase.assets?view=aspnetcore-9.0) property, e.g. to resolve fingerprinted URLs:

```razor
<img src="@Assets["img.png"]" />
```

By default, bUnit's renderer returns an empty [`ResourceAssetCollection`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.components.resourceassetcollection?view=aspnetcore-9.0), which matches an app that does not use `MapStaticAssets`. The indexer then returns the passed-in key unchanged, i.e. `Assets["img.png"]` returns `"img.png"`, and iterating over `Assets` yields no items.

## Adding assets

Use the `AddAsset` method on `BunitContext` to add assets before rendering a component. Passing a `label` maps the stable asset key to its (fingerprinted) URL:

```csharp
[Fact]
public void Image_uses_fingerprinted_url()
{
    AddAsset("img.abc123.png", label: "img.png");

    var cut = Render<ImageComponent>();

    cut.MarkupMatches(@"<img src=""img.abc123.png"" />");
}
```

Assets can also carry additional properties, which components can read when iterating the collection:

```csharp
[Fact]
public void Component_lists_subresources()
{
    AddAsset("css/app.abc123.css", label: "css/app.css");
    AddAsset("js/app.def456.js", label: "js/app.js", new ResourceAssetProperty("integrity", "sha256-..."));

    var cut = Render<SubresourceListingComponent>();

    // component iterates Assets and reads each asset's "label" property
    cut.MarkupMatches(@"<span>css/app.css</span><span>js/app.js</span>");
}
```

> [!NOTE]
> The `"label"` property is the convention `ResourceAssetCollection` uses to build its key → URL mapping, just like `MapStaticAssets` does in production. An asset added without a label is part of the collection when iterating, but the indexer will not map any key to it.
