using System;
using System.Data.Entity;
using evefifo.model;
using eZet.EveLib.Modules;
using System.Linq;
using System.Threading.Tasks;
using Character = eZet.EveLib.Modules.Character;

namespace evefifo.api_pull
{
    internal static class ModelCharacter
    {
        

        internal static async Task UpdateExisting(IRepository repo)
        {
            foreach (var character in await repo.Characters)
            {
                var charKey = new CharacterKey(character.ApiKey.Id, character.ApiKey.Secret);
                var updatedChar = await repo.CharacterFromApi(charKey, (int)character.Id);
                repo.Replace(character, updatedChar);
            }
        }
    }
}