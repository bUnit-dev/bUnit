using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.RenderTree;
using Moq;
using Shouldly;
using Xunit;

namespace Bunit.Rendering
{
    public class RenderEventPubSubTest
    {
        [Fact(DisplayName = "When a subscriber subscribes to a publisher it can receives events from publisher")]
        public void Test001()
        {
            var pub = new RenderEventPublisher();
            var sub = new RenderEventSubscriber(pub);

            pub.OnRender(new RenderEvent(new RenderBatch(), null!));

            sub.RenderCount.ShouldBe(1);
            sub.LatestRenderEvent.ShouldNotBeNull();
        }

        [Fact(DisplayName = "Publisher cannot call OnRender after OnComplete")]
        public void Test002()
        {
            var pub = new RenderEventPublisher();

            pub.OnCompleted();

            Should.Throw<InvalidOperationException>(
                () => pub.OnRender(new RenderEvent(new RenderBatch(), null!))
            );
        }

        [Fact(DisplayName = "Calling Unsubscribe on subscriber unsubscribes it from publisher")]
        public void Test003()
        {
            var pub = new RenderEventPublisher();
            var sub = new RenderEventSubscriber(pub);

            sub.Unsubscribe();

            pub.OnRender(new RenderEvent(new RenderBatch(), null!));

            sub.RenderCount.ShouldBe(0);
            sub.LatestRenderEvent.ShouldBeNull();
        }
    }
}
