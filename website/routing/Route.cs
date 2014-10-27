using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net.Http;

namespace evefifo.website.routing
{
    public class Route
    {
        private readonly Regex m_Regex;
        private readonly Func<Request, Task> m_Action;
        private readonly HttpMethod m_Method;

        public Route(string match, Func<Request, Task> action, HttpMethod method = null)
        {
            m_Regex = new Regex(CreateRegex(match), RegexOptions.Compiled | RegexOptions.ExplicitCapture, TimeSpan.FromSeconds(1));
            m_Action = action;
            m_Method = method ?? HttpMethod.Get;
        }

        public Func<Request, Task> Action { get { return m_Action; } }

        public RouteMatch Matches(HttpMethod method, string requestPath)
        {
            if (method != m_Method) return new RouteMatch(false, null);
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
