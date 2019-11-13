using System;
using System.Collections.Generic;
using AngleSharp.Diffing.Core;

namespace Egil.RazorComponents.Testing
{
    public interface IHtmlComparer : IDisposable
    {
        List<IDiff> Compare(string controlHtml, string testHtml);
    }
}