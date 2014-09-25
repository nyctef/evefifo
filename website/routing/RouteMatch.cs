using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class RouteMatch
    {
        private readonly IDictionary<string, string> m_Parameters;
        private readonly bool m_Success;

        public RouteMatch(bool success, IDictionary<string, string> parameters)
        {
            m_Success = success;
            m_Parameters = parameters;
        }

        public IDictionary<string, string> Parameters { get { return m_Parameters; } }

        public bool Success { get { return m_Success; } }
    }
}
