using eZet.EveLib.Modules;
using System.Threading.Tasks;
using System.Collections.Generic;
using evefifo.model;

namespace evefifo.api_pull
{
    public interface IRepository
    {
        Task<List<model.Character>> Characters { get; }
        Task<model.Character> CharacterFromApi(model.ApiKey charKey, int charId);
        void Replace(model.Character character, model.Character updatedChar);
    }
}