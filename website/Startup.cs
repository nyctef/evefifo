using evefifo.model;
using evefifo.website.controllers;
using evefifo.website.routing;
using Owin;
using System;
using System.Net;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace evefifo.website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration(CreateRepository)(app);
        }

        private static IRepository CreateRepository(EvefifoContext context)
        {
            return new Repository(context);
        }

        public static Action<IAppBuilder> Configuration(Func<EvefifoContext, IRepository> createRepository)
        {
            return app => {
                app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    using (var context = new EvefifoContext())
                    {
                        var repository = createRepository(context);
                        env["evefifo.Repository"] = repository;
                        await next.Invoke(env);
                    }
                })));
                app.UseRoutes(new Route("/", new CharacterListController().Invoke));
            };
        }
    }
}