using evefifo.model;
using evefifo.website.controllers;
using evefifo.website.routing;
using Microsoft.Owin;
using Owin;
using System;
using System.Net;
using System.Net.Http;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;



[assembly:OwinStartup(typeof(evefifo.website.Startup))]



namespace evefifo.website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration(Repository.Create)(app);
        }

        public static Action<IAppBuilder> Configuration(Func<IRepository> createRepository)
        {
            return app => {
                app.UseErrorPage();
                app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    try
                    {
                        await next.Invoke(env);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                })));
                app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
                {
                    using (var repository = createRepository())
                    {
                        env["evefifo.Repository"] = repository;
                        await next.Invoke(env);
                    }
                })));

                var characterController = new CharacterController();
                var apiKeyController = new ApiKeyController();
                app.UseStaticFiles();
                app.UseRoutes(
                    new Route("/", characterController.List),
                    new Route("/characters/{id}", characterController.Show),
                    new Route("/apikeys", apiKeyController.List),
                    new Route("/apikeys", apiKeyController.Add, HttpMethod.Post)
                );
            };
        }
    }
}