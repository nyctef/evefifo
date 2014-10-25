using eZet.EveLib.Modules;
using System.Threading.Tasks;
using System.Collections.Generic;
using evefifo.model;
using System;

namespace evefifo.model
{
    public interface IRepository : IDisposable
    {
        Task<List<model.ApiKey>> ApiKeys { get; }
        void AddApiKey(model.ApiKey apiKey);

        Task<List<model.Character>> Characters { get; }
        void AddCharacter(model.Character character);
        Task<model.Character> CharacterFromApi(model.ApiKey charKey, int charId);
        void Replace(model.Character character, model.Character updatedChar);
        Task<Character> Character(int id);

        Task<List<model.Notification>> Notifications { get; }
        void AddNotification(model.Notification notification);
        void RemoveNotification(model.Notification notification);
    }
}