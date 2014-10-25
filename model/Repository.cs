using eZet.EveLib.Modules;
using eZet.EveLib.Modules.Models.Character;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApiCharacter = eZet.EveLib.Modules.Character;
using ApiSkillQueue = eZet.EveLib.Modules.Models.Character.SkillQueue;

namespace evefifo.model
{
    public class Repository : IRepository
    {
        public static IRepository Create()
        {
            return new Repository(new model.EvefifoContext());
        }

        private readonly model.EvefifoContext m_Db;

        internal Repository(model.EvefifoContext db)
        {
            m_Db = db;
        }

        public Task<List<model.ApiKey>> ApiKeys
        {
            get
            {
                return m_Db.ApiKeys.ToListAsync();
            }
        }

        public Task<List<model.Character>> Characters
        {
            get
            {
                return m_Db.Characters.ToListAsync();
            }
        }

        public Task<List<model.Notification>> Notifications
        {
            get
            {
                return m_Db.Notifications.ToListAsync();
            }
        }

        public async Task<model.Character> CharacterFromApi(model.ApiKey apiKey, int charId)
        {
            var charKey = new CharacterKey(apiKey.Id, apiKey.Secret);
            await charKey.InitAsync();
            var apiChar = new ApiCharacter(charKey, charId);
            var charInfo = (await apiChar.GetCharacterInfoAsync()).Result;
            var charSheet = (await apiChar.GetCharacterSheetAsync()).Result;
            var skillQueue = (await apiChar.GetSkillQueueAsync()).Result;
            return new model.Character
            {
                Id = charId,
                Name = charInfo.CharacterName,
                CorpName = charInfo.CorporationName,
                CloneName = charSheet.CloneName,
                CloneSP = charSheet.CloneSkillPoints,
                SP = charInfo.SkillPoints,
                SecStatus = charInfo.SecurityStatus,
                SkillQueue = SkillQueue(skillQueue),
                ApiKey = new model.ApiKey { Id = charKey.KeyId, Secret = charKey.VCode }
            };
        }

        public Task<Character> Character(int id)
        {
            return m_Db.Characters.FindAsync(id);
        }

        public model.SkillQueue SkillQueue(ApiSkillQueue queue)
        {
            var queueEntries = queue.Queue.Select(x => new model.SkillQueue.Entry(x.TypeId, "unknown", (byte)x.Level, x.StartSp, x.EndSp, x.StartTime, x.EndTime));
            return new model.SkillQueue(queueEntries.ToList());
        }

        public void Replace(model.Character character, model.Character updatedChar)
        {
            m_Db.Entry(character).CurrentValues.SetValues(updatedChar);
        }

        public void AddApiKey(model.ApiKey apiKey)
        {
            m_Db.ApiKeys.Add(apiKey);
        }

        public void AddCharacter(model.Character character)
        {
            m_Db.Characters.Add(character);
        }

        public void AddNotification(model.Notification notification)
        {
            m_Db.Notifications.Add(notification);
        }

        public void RemoveNotification(model.Notification notification)
        {
            m_Db.Notifications.Remove(notification);
        }

        public void Dispose()
        {
            m_Db.SaveChanges();
            m_Db.Dispose();
        }
    }
}
