using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                if (route.Match == (string)environment["owin.RequestPath"])
                {
                    route.Action(environment);
                    return;
                }
            }

            await m_Next(environment);
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
