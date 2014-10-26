using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Net;
using Moq.Language.Flow;

namespace evefifo.tests
{
    public static class Utils
    {
        public static HtmlDocument GetHtml(string text)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(text);
            return doc;
        }

        internal static async Task<HtmlDocument> GetHtml(HttpResponseMessage response)
        {
            return GetHtml(await response.Content.ReadAsStringAsync());
        }

        internal static async Task<HtmlNode> GetBody(HttpResponseMessage response)
        {
            var html = await GetHtml(response);
            return html.DocumentNode.SelectSingleNode("/html/body");
        }

        internal static async Task<string> GetTitle(HttpResponseMessage response)
        {
            var html = await GetHtml(response);
            var titleElement = html.DocumentNode.SelectSingleNode("/html/head/title");
            Assert.NotNull(titleElement, "Should find element /html/head/title in returned webpage");
            var title = titleElement.InnerText;
            return title;
        }

        internal static void HasStatusCode(this HttpResponseMessage response, HttpStatusCode statusCode)
        {
            Assert.AreEqual(statusCode, response.StatusCode);
        }

        internal static async Task<HttpResponseMessage> HasStatusCode(this Task<HttpResponseMessage> response, HttpStatusCode statusCode)
        {
            (await response).HasStatusCode(statusCode);
            return await response;
        }

        internal static void HasTitle(this HttpResponseMessage response, string title)
        {
            Assert.AreEqual(title, GetTitle(response).Result);
        }

        internal static async Task<HttpResponseMessage> HasTitle(this Task<HttpResponseMessage> response, string title)
        {
            Assert.AreEqual(title, await GetTitle(await response));
            return await response;
        }

        internal static void HasHeader(this HttpResponseMessage response, string headerName, string headerValue)
        {
            var header = response.Headers.GetValues(headerName).Single();
            Assert.AreEqual(headerValue, header);
        }

        internal static void ReturnsAsync<TMock, TResult>(this ISetupGetter<TMock, Task<TResult>> mock, TResult result) where TMock:class
        {
            mock.Returns(Task.FromResult(result));
        }
    }
}
