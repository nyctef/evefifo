using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;
using System.Net;

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

        internal static async Task<HttpResponseMessage> HasStatusCode(this Task<HttpResponseMessage> response, HttpStatusCode statusCode)
        {
            Assert.AreEqual(statusCode, (await response).StatusCode);
            return await response;
        }

        internal static async Task<HttpResponseMessage> HasTitle(this Task<HttpResponseMessage> response, string title)
        {
            Assert.AreEqual(title, await GetTitle(await response));
            return await response;
        }
    }
}
