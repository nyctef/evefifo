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
    class CharacterController : ControllerBase
    {
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
    }
}
