using eZet.EveLib.Modules;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.api_pull
{
    class Repository : IRepository
    {
        private readonly model.EvefifoContext m_Db;

        public Repository(model.EvefifoContext db)
        {
            m_Db = db;
        }

        public Task<List<model.Character>> Characters
        {
            get
            {
                return m_Db.Characters.ToListAsync();
            }
        }

        public async Task<model.Character> CharacterFromApi(CharacterKey charKey, int charId)
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

        public void Replace(model.Character character, model.Character updatedChar)
        {
            m_Db.Entry(character).CurrentValues.SetValues(updatedChar);
        }
    }
}
