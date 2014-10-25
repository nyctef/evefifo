using evefifo.model;
using evefifo.website.models;
using RazorEngine;
using RazorEngine.Configuration;
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
    class CharacterController
    {
        public CharacterController()
        {
            Razor.Compile(GetTemplateFile("Layout").Result, "Layout");
        }

        public async Task List(IDictionary<string, object> environment)
        {
            var repo = GetRepository(environment);
            var characterEntries = (await repo.Characters).Select(x => new CharacterListModel.CharacterEntry { Character = x, NumNotifications = x.Notifications.Count });
            var model = new CharacterListModel(characterEntries.ToList());
            string result = await CompileView("CharacterList", model);

            await WriteResponse(environment, result);
        }

        public async Task Show(IDictionary<string, object> environment)
        {
            var repo = GetRepository(environment);
            var parameters = (IDictionary<string, string>)environment["evefifo.RequestParameters"];
            var characterId = Convert.ToInt32(parameters["id"]);
            var character = await repo.Character(characterId);
            if (character == null)
            {
                await Write404(environment);
                return;
            }
            var model = new CharacterModel(character);
            string result = await CompileView("Character", model);

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
            var headers = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
            headers["Content-Type"] = new[] { "text/html" };
            var stream = (Stream)environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(result);
            }
        }

        private async Task Write404(IDictionary<string, object> environment)
        {
            environment["owin.ResponseStatusCode"] = HttpStatusCode.NotFound;
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
