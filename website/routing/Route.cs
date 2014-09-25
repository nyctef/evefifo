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
        private readonly Func<IDictionary<string, object>, Task> m_Action;

        public Route(string match, Func<IDictionary<string, object>, Task> action)
        {
            m_Match = match;
            m_Action = action;
        }

        public string Match { get { return m_Match; } }

        public Func<IDictionary<string, object>, Task> Action { get { return m_Action; } }
    }
}
