using evefifo.model;
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

namespace evefifo.website.controllers
{
    public abstract class ControllerBase
    {
        public ControllerBase()
        {
            Razor.Compile(GetTemplateFile("Layout").Result, "Layout");
        }

        public int GetIntParameter(IDictionary<string, object> environment, string paramName)
        {
            var parameters = (IDictionary<string, string>)environment["evefifo.RequestParameters"];
            return Convert.ToInt32(parameters[paramName]);
        }

        protected IRepository GetRepository(IDictionary<string, object> environment)
        {
            return (IRepository)environment["evefifo.Repository"];
        }

        protected async Task<string> CompileView(string modelName, object model)
        {
            var template = await GetTemplateFile(modelName);

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

        protected IDictionary<string, string[]> GetHeaders(IDictionary<string, object> environment)
        {
            return (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
        }

        protected async Task WriteResponse(IDictionary<string, object> environment, string result)
        {
            environment["owin.ResponseStatusCode"] = HttpStatusCode.OK;
            var headers = GetHeaders(environment);
            headers["Content-Type"] = new[] { "text/html" };
            var stream = (Stream)environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(result);
            }
        }

        protected async Task Write404(IDictionary<string, object> environment)
        {
            environment["owin.ResponseStatusCode"] = HttpStatusCode.NotFound;
        }

        protected async Task Write303(IDictionary<string, object> environment, string redirectPath)
        {
            environment["owin.ResponseStatusCode"] = HttpStatusCode.SeeOther;
            var headers = GetHeaders(environment);
            headers["Location"] = new[] { redirectPath };
        }

        protected async Task<string> GetTemplateFile(string viewName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("evefifo.website.views." + viewName + ".cshtml"))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
