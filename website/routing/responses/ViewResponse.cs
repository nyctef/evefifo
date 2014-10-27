using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing.responses
{
    class ViewResponse : Response
    {
        public readonly string ViewName;
        public readonly object Model;

        public ViewResponse(string viewName, object model, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode)
        {
            ViewName = viewName;
            Model = model;
        }

        public override async Task WriteTo(ResponseWriter writer)
        {
            await writer.Write(this);
        }
    }
}
