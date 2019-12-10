using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;

namespace Egil.RazorComponents.Testing
{
    public static class MockHttpExtensions
    {
        public static MockHttpMessageHandler AddMockHttp(this TestServiceProvider serviceProvider)
        {
            if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));

            var mockHttp = new MockHttpMessageHandler();
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://example.com");
            serviceProvider.AddService(httpClient);
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
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StringContent(JsonSerializer.Serialize(task.Result))
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return response;
                });
            });

            return tcs;
        }
    }
}
