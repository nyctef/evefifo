using eZet.EveLib.Modules;
using System.Threading.Tasks;
using System.Collections.Generic;
using evefifo.model;

namespace evefifo.api_pull
{
    internal interface IRepository
    {
        Task<List<model.Character>> Characters { get; }
        Task<model.Character> CharacterFromApi(CharacterKey charKey, int charId);
        void Replace(model.Character character, model.Character updatedChar);
    }
}