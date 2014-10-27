using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing.responses
{
    public class ViewResponse : Response
    {
        public readonly string ViewName;
        public readonly dynamic Model;

        public ViewResponse(string viewName, dynamic model, HttpStatusCode statusCode = HttpStatusCode.OK)
            : base(statusCode)
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
