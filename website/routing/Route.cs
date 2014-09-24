using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class Route
    {
        private readonly string m_Match;
        private readonly Action<IDictionary<string, object>> m_Action;

        public Route(string match, Action<IDictionary<string, object>> action)
        {
            m_Match = match;
            m_Action = action;
        }

        public string Match { get { return m_Match; } }

        public Action<IDictionary<string, object>> Action { get { return m_Action; } }
    }
}
