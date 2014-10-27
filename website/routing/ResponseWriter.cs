using evefifo.website.routing.responses;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class ResponseWriter
    {
        static ResponseWriter()
        {
            Razor.Compile(GetTemplateFile("Layout").Result, "Layout");
        }

        private readonly IDictionary<string, object> m_Environment;

        public ResponseWriter(IDictionary<string, object> environment)
        {
            m_Environment = environment;
        }

        protected IDictionary<string, string[]> GetHeaders(IDictionary<string, object> environment)
        {
            return (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
        }

        public async Task Write(HtmlFoundResponse response) 
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.OK;
            await WriteHtml(response.Html);
        }

        private async Task WriteHtml(string html)
        {
            var headers = GetHeaders(m_Environment);
            headers["Content-Type"] = new[] { "text/html" };
            var stream = (Stream)m_Environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(html);
            }
        }

        public async Task Write(SeeOtherResponse response)
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.SeeOther;
            var headers = GetHeaders(m_Environment);
            headers["Location"] = new[] { response.Location };
        }

        internal async Task Write(NotFoundResponse response)
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.NotFound;
        }

        internal async Task Write(ViewResponse response)
        {
            await WriteHtml(await CompileView(response.ViewName, response.Model));
        }

        private static async Task<string> CompileView(string viewName, object model)
        {
            var template = await GetTemplateFile(viewName);

            string result;
            try
            {
                result = Razor.Parse(template, model);
            }
            catch (TemplateCompilationException e)
            {
                // Workaround for an issue where RazorEngine will just include the first message -
                // this may be a warning instead of the error we want
                // see http://forum.ncrunch.net/yaf_postst273_Tests-that-use-RazorEngine-get-odd-exception-when-NCrunch-is-the-runner.aspx
                throw new Exception(String.Join("\n", e.Errors), e);
            }

            return result;
        }

        private static async Task<string> GetTemplateFile(string viewName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("evefifo.website.views." + viewName + ".cshtml"))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
