using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Egil.RazorComponents.Testing
{
    public interface ITestRenderingContext
    {
        List<(int Id, IComponent Component)> GetComponents();
        List<(int Id, T Component)> GetComponents<T>();
        string GetHtml(int componentId);
        void RenderComponentUnderTest(RenderFragment renderFragment);
        void DispatchAndAssertNoSynchronousErrors(Action dispatchAction);
        void WaitForNextRender(Action trigger);
    }
}