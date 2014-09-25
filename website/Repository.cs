using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using evefifo.model;
using System.Data.Entity;

namespace evefifo.website
{
    class Repository : IRepository
    {
        private readonly EvefifoContext m_Db;

        public Repository(EvefifoContext db)
        {
            m_Db = db;
        }

        public Task<List<Character>> Characters
        {
            get
            {
                return m_Db.Characters.ToListAsync();
            }
        }

        public async Task<List<Notification>> NotificationsForCharacter(Character character)
        {
            return await m_Db.Notifications.Where(x => x.Character == character).ToListAsync();
        }
    }
}
