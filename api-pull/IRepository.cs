using eZet.EveLib.Modules;
using System.Threading.Tasks;
using System.Collections.Generic;
using evefifo.model;

namespace evefifo.api_pull
{
    public interface IRepository
    {
        Task<List<model.Character>> Characters { get; }
        void AddCharacter(model.Character character);
        Task<model.Character> CharacterFromApi(model.ApiKey charKey, int charId);
        void Replace(model.Character character, model.Character updatedChar);

        Task<List<model.Notification>> Notifications { get; }
        void AddNotification(model.Notification notification);
        void RemoveNotification(model.Notification notification);
    }
}