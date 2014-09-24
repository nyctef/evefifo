using evefifo.website.models;
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
    class CharacterListController
    {
        public async Task Invoke(IDictionary<string, object> environment)
        {
            var repo = (IRepository)environment["evefifo.Repository"];
            var characters = await repo.Characters;

            var template = await GetTemplateFile("CharacterList");
            var model = new CharacterListModel(characters);
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
