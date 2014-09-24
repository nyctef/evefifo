using evefifo.website.controllers;
using evefifo.website.routing;
using Owin;
using System;
using System.Net;

namespace evefifo.website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration(new Repository())(app);
        }

        public static Action<IAppBuilder> Configuration(IRepository repository)
        {
            return app => {
                // TODO: Route should probably just be able to handle async things being passed into it (or be async by default)
                app.UseRoutes(new Route("/", async env => { await new CharacterListController().Invoke(env); }));
            };
        }
    }
}