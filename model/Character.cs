using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.model
{
    public class Character
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public long Id { get; set; }
        public string CloneName { get; set; }
        public long CloneSP { get; set; }
        public string CorpName { get; set; }
        public string Name { get; set; }
        public double SecStatus { get; set; }
        public long SP { get; set; }
        public long ApiKeyId { get; set; }
        public virtual ApiKey ApiKey { get; set; }
    }
}
