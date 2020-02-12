using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RichardSzalay.MockHttp;
using Shouldly;
using Xunit;

namespace Bunit
{
    public class MockHttpExtensionsTest
    {
        [Fact(DisplayName = "AddMockHttp throws if the service provider is null")]
        public void Test001()
        {
            TestServiceProvider provider = default!;

            Should.Throw<ArgumentNullException>(() => provider.AddMockHttp());
        }

        [Fact(DisplayName = "AddMockHttp registers a mock HttpClient in the service provider")]
        public void Test002()
        {
            using var provider = new TestServiceProvider();

            var mock = provider.AddMockHttp();

            provider.GetService<HttpClient>().ShouldNotBeNull();
        }

        [Fact(DisplayName = "Capture  throws if the handler is null")]
        public void Test003()
        {
            MockHttpMessageHandler handler = default!;

            Should.Throw<ArgumentNullException>(() => handler.Capture(""));
        }

        [Fact(DisplayName = "Capture returns a task, that when completed, " +
                            "provides a response to the captured url")]
        public async Task Test004()
        {
            using var provider = new TestServiceProvider();
            var mock = provider.AddMockHttp();
            var httpClient = provider.GetService<HttpClient>();
            var captured = mock.Capture("/ping");

            captured.SetResult("pong");

            var actual = await httpClient.GetStringAsync("/ping");
            actual.ShouldBe("\"pong\"");
        }
    }
}
