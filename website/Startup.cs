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
                app.UseRoutes(new Route("/", new CharacterListController().Invoke));
            };
        }
    }
}