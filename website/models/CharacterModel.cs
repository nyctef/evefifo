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

        public string TotalQueueLength
        {
            get
            {
                var skillQueue = m_Character.SkillQueue;
                if (skillQueue.IsEmpty()) return "Empty";
                var latestSkill = skillQueue.Entries.OrderByDescending(x => x.EndTime).First();
                return (latestSkill.EndTime.ToLocalTime() - DateTime.Now).ToReadableString();
            }
        }
    }
}
