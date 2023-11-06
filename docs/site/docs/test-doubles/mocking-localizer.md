---
uid: mocking-localizer
title: Mocking Localization via IStringLocalizer
---

<p>This is very simple. First in your setup add the following:</p>
<code>TestContext.Services.AddLocalization();</code>
<p>Then in your test code, when you need the localized string to compare, you write the following:</p>
<code>var localizer = ctx.Services.GetService<IStringLocalizer<SharedStrings>>();</code>
<p>Where SharedStrings.cs (you can name this anything you want) that has the resource files such as `SharedStrings.en.resx`</p>
