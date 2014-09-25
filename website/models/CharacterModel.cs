using evefifo.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.models
{
    public class CharacterModel
    {
        private readonly Character m_Character;

        public CharacterModel(Character character)
        {
            m_Character = character;
        }

        public Character Character
        {
            get
            {
                return m_Character;
            }
        }
    }
}
