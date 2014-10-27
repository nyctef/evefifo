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
