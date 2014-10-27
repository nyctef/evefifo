using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public abstract class Response
    {
        public readonly HttpStatusCode StatusCode;

        protected Response(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }

        public abstract Task WriteTo(ResponseWriter writer);
    }
}
