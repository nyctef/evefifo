using System;
using System.Data.Entity;
using evefifo.model;
using eZet.EveLib.Modules;
using System.Linq;
using System.Threading.Tasks;
using Character = eZet.EveLib.Modules.Character;

namespace evefifo.api_pull
{
    public static class ModelCharacter
    {
        

        public static async Task UpdateExisting(IRepository repo)
        {
            foreach (var character in await repo.Characters)
            {
                var updatedChar = await repo.CharacterFromApi(character.ApiKey, (int)character.Id);
                repo.Replace(character, updatedChar);
            }
        }
    }
}