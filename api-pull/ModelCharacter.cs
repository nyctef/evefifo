using System;
using eZet.EveLib.Modules;
using System.Linq;
using System.Threading.Tasks;

namespace evefifo.api_pull
{
    internal class ModelCharacter
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
                Name = apiChar.CharacterName,
                CorpName = apiChar.CorporationName,
                CloneName = charSheet.CloneName,
                CloneSP = charSheet.CloneSkillPoints,
                SP = charInfo.SkillPoints,
                SecStatus = charInfo.SecurityStatus,
            };
        }
    }
}