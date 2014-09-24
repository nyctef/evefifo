using evefifo.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.models
{
    public class CharacterListModel
    {
        private readonly List<Character> m_Characters;

        public CharacterListModel(List<Character> characters)
        {
            m_Characters = characters;
        }

        public List<Character> Characters { get { return m_Characters; } }
    }
}
