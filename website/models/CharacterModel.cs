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
        private readonly DateTime m_Now = DateTime.Now;

        public CharacterModel(Character character, DateTime? now = null)
        {
            m_Character = character;
            m_Now = now ?? m_Now;
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
                return (latestSkill.EndTime.ToLocalTime() - m_Now).ToReadableString();
            }
        }
    }
}
