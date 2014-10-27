using evefifo.model;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace evefifo.website.routing
{
    public class Routing
    {
        private readonly AppFunc m_Next;
        private readonly List<Route> m_Routes;

        public Routing(AppFunc next, IEnumerable<Route> routes)
        {
            m_Next = next;
            m_Routes = routes.ToList();
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            foreach (var route in m_Routes)
            {
                var path = (string)environment["owin.RequestPath"];
                var method = GetMethod((string)environment["owin.RequestMethod"]);
                RouteMatch routeMatch = route.Matches(method, path);
                if (routeMatch.Success)
                {
                    environment["evefifo.RequestParameters"] = routeMatch.Parameters;
                    var repo = (IRepository)environment["evefifo.Repository"];
                    var body = (Stream)environment["owin.RequestBody"];
                    var request = new Request(environment, method, path, routeMatch.Parameters, repo, body);
                    var response = await route.Action(request);
                    response.WriteTo(new ResponseWriter(environment));
                    return;
                }
            }

            await m_Next(environment);
        }

        private static HttpMethod GetMethod(string methodName)
        {
            switch (methodName)
            {
                case "GET": return HttpMethod.Get;
                case "POST": return HttpMethod.Post;
                case "PUT": return HttpMethod.Put;
                case "HEAD": return HttpMethod.Head;
                case "DELETE": return HttpMethod.Delete;
                case "OPTIONS": return HttpMethod.Options;
                case "TRACE": return HttpMethod.Trace;
                default: throw new ArgumentOutOfRangeException("methodName");
            }
        }
    }

    public static class AppBuilderExtensions
    {
        public static void UseRoutes(this IAppBuilder app, params Route[] routes)
        {
            app.Use<Routing>((IEnumerable<Route>)routes);
        }
    }
}
