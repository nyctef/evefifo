using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class NotFoundResponse : Response
    {
        public NotFoundResponse() : base(HttpStatusCode.NotFound) { }

        public override async Task WriteTo(ResponseWriter writer)
        {
            await writer.Write(this);
        }
    }
}
