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
        private readonly Action m_Result;

        public Route(string match, Action result)
        {
            m_Match = match;
            m_Result = result;
        }

        public string Match { get { return m_Match; } }

        public Action Result { get { return m_Result; } }
    }
}
