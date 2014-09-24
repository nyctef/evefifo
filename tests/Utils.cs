using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using NUnit.Framework;

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
    }
}
