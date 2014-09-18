using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.model
{
    public class EvefifoContext : DbContext
    {
        public DbSet<Character> Characters { get; set; }
        public DbSet<ApiKey> ApiKeys { get; set; }
    }
}
