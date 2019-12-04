using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using Xunit.Abstractions;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    public static class MockHttpExtensions
    {
        public static MockHttpMessageHandler AddMockHttp(this TestContext host)
        {
            if(host is null) throw new ArgumentNullException(nameof(host));

            var mockHttp = new MockHttpMessageHandler();
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://example.com");
            host.AddService(httpClient);
            return mockHttp;
        }

        [SuppressMessage("Reliability", "CA2008:Do not create tasks without passing a TaskScheduler", Justification = "<Pending>")]
        [SuppressMessage("Design", "CA1054:Uri parameters should not be strings", Justification = "<Pending>")]
        public static TaskCompletionSource<object> Capture(this MockHttpMessageHandler handler, string url)
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));

            var tcs = new TaskCompletionSource<object>();

            handler.When(url).Respond(() =>
            {
                return tcs.Task.ContinueWith(task =>
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new StringContent(JsonSerializer.Serialize(task.Result));
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });
            });

            return tcs;
        }
    }
}
