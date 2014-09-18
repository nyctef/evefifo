using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.model
{
    public class Character
    {
        public string CloneName { get; set; }
        public long CloneSP { get; set; }
        public string CorpName { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public double SecStatus { get; set; }
        public long SP { get; set; }
    }
}
