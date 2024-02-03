---
uid: mocking-localizer
title: Mocking Localization via IStringLocalizer
---

<p>There are just two steps. First in your setup add the following:</p>

```csharp
TestContext.Services.AddLocalization();
```

<p>Then in your test code, when you need the localized string to compare, you write the following:</p>

```csharp
var localizer = ctx.Services.GetService<IStringLocalizer<SharedStrings>>();
```

<p>Where SharedStrings.cs (you can name this anything you want) that has the resource files such as `SharedStrings.en.resx`</p>
