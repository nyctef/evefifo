using evefifo.model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace evefifo.website
{
    public interface IRepository
    {
        Task<List<Character>> Characters { get; }
    }
}