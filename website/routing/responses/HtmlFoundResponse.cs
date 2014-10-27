using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class HtmlFoundResponse : Response
    {
        public readonly string Html;

        public HtmlFoundResponse(string html)
            : base(HttpStatusCode.Found)
        {
            Html = html;
        }

        public override async Task WriteTo(ResponseWriter writer) 
        {
            writer.Write(this);
        }
    }
}
