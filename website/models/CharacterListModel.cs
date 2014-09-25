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
        public class CharacterEntry
        {
            public Character Character { get; set; }
            public int NumNotifications { get; set; }
        }

        private readonly List<CharacterEntry> m_Characters;

        public CharacterListModel(List<CharacterEntry> characters)
        {
            m_Characters = characters;
        }

        public List<CharacterEntry> Characters { get { return m_Characters; } }
    }
}
