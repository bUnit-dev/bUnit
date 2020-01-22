using RichardSzalay.MockHttp;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;

namespace Egil.RazorComponents.Testing
{
    /// <summary>
    /// Helper methods for adding a Mock HTTP client to a service provider.
    /// </summary>
    public static class MockHttpExtensions
    {
        /// <summary>
        /// Create a <see cref="MockHttpMessageHandler"/> and adds it to the 
        /// <paramref name="serviceProvider"/> as a <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns>The <see cref="MockHttpMessageHandler"/>.</returns>
        public static MockHttpMessageHandler AddMockHttp(this TestServiceProvider serviceProvider)
        {
            if (serviceProvider is null) throw new ArgumentNullException(nameof(serviceProvider));

            var mockHttp = new MockHttpMessageHandler();
            var httpClient = mockHttp.ToHttpClient();
            httpClient.BaseAddress = new Uri("http://example.com");
            serviceProvider.AddSingleton(httpClient);
            return mockHttp;
        }

        /// <summary>
        /// Configure a <see cref="MockHttpMessageHandler"/> to capture requests to 
        /// a <paramref name="url"/>.
        /// </summary>
        /// <param name="handler"></param>
        /// <param name="url">Url of requests to capture.</param>
        /// <returns>A <see cref="TaskCompletionSource{TResult}"/> that can be used to send a response to a captured request.</returns>
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
