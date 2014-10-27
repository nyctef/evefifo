using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing.responses
{
    public class SeeOtherResponse : Response
    {
        public readonly string Location;

        public SeeOtherResponse(string location) : base(HttpStatusCode.SeeOther)
        {
            Location = location;
        }

        public override async Task WriteTo(ResponseWriter writer)
        {
            await writer.Write(this);
        }
    }
}
