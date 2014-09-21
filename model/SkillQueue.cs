using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.model
{
    public class SkillQueue
    {
        private readonly List<Entry> m_Entries;

        public class Entry
        {
            private readonly long m_EndSP;
            private readonly DateTime m_EndTime;
            private readonly long m_SkillId;
            private readonly byte m_SkillLevel;
            private readonly string m_SkillName;
            private readonly long m_StartSP;
            private readonly DateTime m_StartTime;

            public Entry(long skillId, string skillName, byte skillLevel, long startSP, long endSP, DateTime startTime, DateTime endTime)
            {
                m_SkillId = skillId;
                m_SkillName = skillName;
                m_SkillLevel = skillLevel;
                m_StartSP = startSP;
                m_EndSP = endSP;
                m_StartTime = startTime;
                m_EndTime = endTime;
            }

            public long EndSP { get { return m_EndSP; } } 
            public DateTime EndTime { get { return m_EndTime; } } 
            public long SkillId { get { return m_SkillId; } } 
            public byte SkillLevel { get { return m_SkillLevel; } } 
            public string SkillName { get { return m_SkillName; } } 
            public long StartSP { get { return m_StartSP; } } 
            public DateTime StartTime { get { return m_StartTime; } }
        }

        public SkillQueue(List<Entry> entries)
        {
            m_Entries = entries;
        }

        public List<Entry> Entries { get { return m_Entries; } }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static SkillQueue FromJSON(string json)
        {
            return JsonConvert.DeserializeObject<SkillQueue>(json);
        }
    }
}
