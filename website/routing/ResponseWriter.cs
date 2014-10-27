using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class ResponseWriter
    {
        private readonly IDictionary<string, object> m_Environment;

        public ResponseWriter(IDictionary<string, object> environment)
        {
            m_Environment = environment;
        }

        protected IDictionary<string, string[]> GetHeaders(IDictionary<string, object> environment)
        {
            return (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
        }

        public async Task Write(HtmlFoundResponse response) 
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.OK;
            var headers = GetHeaders(m_Environment);
            headers["Content-Type"] = new[] { "text/html" };
            var stream = (Stream)m_Environment["owin.ResponseBody"];
            using (var writer = new StreamWriter(stream, System.Text.Encoding.UTF8, 1024, true))
            {
                await writer.WriteAsync(response.Html);
            }
        }

        public async Task Write(SeeOtherResponse response)
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.SeeOther;
            var headers = GetHeaders(m_Environment);
            headers["Location"] = new[] { response.Location };
        }

        internal async Task Write(NotFoundResponse response)
        {
            m_Environment["owin.ResponseStatusCode"] = HttpStatusCode.NotFound;
        }
    }
}
