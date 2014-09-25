using evefifo.website.models;
using RazorEngine;
using RazorEngine.Templating;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            IRepository repo = GetRepository(environment);
            var characterEntries = (await repo.Characters).Select(x => new CharacterListModel.CharacterEntry { Character = x, NumNotifications = x.Notifications.Count });
            var model = new CharacterListModel(characterEntries.ToList());
            string result = await CompileView("CharacterList", model);

            await WriteResponse(environment, result);
        }

        private IRepository GetRepository(IDictionary<string, object> environment)
        {
            return (IRepository)environment["evefifo.Repository"];
        }

        private async Task<string> CompileView(string modelName, object model)
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

        private async Task WriteResponse(IDictionary<string, object> environment, string result)
        {
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
