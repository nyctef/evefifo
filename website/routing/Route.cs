using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class Route
    {
        private readonly Regex m_Regex;
        private readonly Func<IDictionary<string, object>, Task> m_Action;

        public Route(string match, Func<IDictionary<string, object>, Task> action)
        {
            m_Regex = new Regex(CreateRegex(match), RegexOptions.Compiled | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1));
            m_Action = action;
        }

        public Func<IDictionary<string, object>, Task> Action { get { return m_Action; } }

        public RouteMatch Matches(string requestPath)
        {
            var match = m_Regex.Match(requestPath);
            return new RouteMatch(match.Success, ParameterNames().Select(x => new KeyValuePair<string, string>(x, match.Groups[x].Value)).ToDictionary());
        }

        private IEnumerable<string> ParameterNames()
        {
            return m_Regex.GetGroupNames().Where(x => x != "0");
        }

        private static string CreateRegex(string route)
        {
            return "^" + route.Replace("{", "(?<")
                .Replace("}", ">[^/]+)") + "$";
        }
    }


}
