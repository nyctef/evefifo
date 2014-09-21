using System;
using System.Data.Entity;
using evefifo.model;
using eZet.EveLib.Modules;
using System.Linq;
using System.Threading.Tasks;

namespace evefifo.api_pull
{
    public static class ModelCharacter
    {
        public static async Task UpdateExisting(IRepository repo)
        {
            foreach (var character in await repo.Characters)
            {
                var updatedChar = await repo.CharacterFromApi(character.ApiKey, (int)character.Id);
                CheckNotifications(repo, updatedChar);
                repo.Replace(character, updatedChar);
            }
        }

        public static async Task AddNew(IRepository repo, model.ApiKey apiKey, int charId)
        {
            var character = await repo.CharacterFromApi(apiKey, charId);
            repo.AddCharacter(character);
        }

        public static void CheckNotifications(IRepository repo, model.Character character)
        {
            if (character.SkillQueue.HasSpace())
            {
                repo.AddNotification(new model.Notification
                {
                    Character = character,
                    NotificationText = String.Format("{0} has free space in their skill queue.", character.Name),
                    NotificationType = "queue-space",
                });
            }
        }
    }
}