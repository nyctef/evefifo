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
        internal static async Task<model.Character> FromApi(CharacterKey charKey, int charId)
        {
            await charKey.InitAsync();
            var apiChar = new Character(charKey, charId);
            var charInfo = (await apiChar.GetCharacterInfoAsync()).Result;
            var charSheet = (await apiChar.GetCharacterSheetAsync()).Result;
            return new model.Character
            {
                Id = charId,
                Name = charInfo.CharacterName,
                CorpName = charInfo.CorporationName,
                CloneName = charSheet.CloneName,
                CloneSP = charSheet.CloneSkillPoints,
                SP = charInfo.SkillPoints,
                SecStatus = charInfo.SecurityStatus,
                ApiKey = new model.ApiKey { Id = charKey.KeyId, Secret = charKey.VCode }
            };
        }

        internal static async Task UpdateExisting()
        {
            using (var db = new EvefifoContext())
            {
                foreach (var character in await db.Characters.ToListAsync())
                {
                    var charKey = new CharacterKey(character.ApiKey.Id, character.ApiKey.Secret);
                    var updatedChar = await FromApi(charKey, (int) character.Id);
                    db.Entry(character).CurrentValues.SetValues(updatedChar);
                }

                db.SaveChanges();
            }
        }
    }
}