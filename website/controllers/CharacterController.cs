using evefifo.model;
using evefifo.website.models;
using evefifo.website.routing;
using evefifo.website.routing.responses;
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
        public async Task<Response> List(Request request)
        {
            var repo = request.Repository;
            var characterEntries = (await repo.Characters).Select(x => new CharacterListModel.CharacterEntry { Character = x, NumNotifications = x.Notifications.Count });
            var model = new CharacterListModel(characterEntries.ToList());
            return new ViewResponse("CharacterList", model);
        }

        public async Task<Response> Show(Request request)
        {
            var repo = request.Repository;
            var characterId = Convert.ToInt32(request.Parameters["id"]);
            var character = await repo.Character(characterId);
            if (character == null)
            {
                return new NotFoundResponse();
            }
            var model = new CharacterModel(character);
            return new ViewResponse("Character", model);
        }
    }
}
