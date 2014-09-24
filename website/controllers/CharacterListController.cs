using RazorEngine;
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
    class CharacterListController
    {
        public async Task Invoke(IDictionary<string, object> environment)
        {
            var template = await GetTemplateFile("CharacterList");
            string result = Razor.Parse(template, new { Model = "asdf" });

            environment["owin.ResponseStatusCode"] = HttpStatusCode.OK;
            var stream = (Stream)environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(result);
            }
        }

        public async Task<string> GetTemplateFile(string viewName)
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("evefifo.website.views." + viewName + ".cshtml"))
            using (var reader = new StreamReader(stream))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
