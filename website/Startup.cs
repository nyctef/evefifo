using Owin;
using System;

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
            return app => app.UseWelcomePage("/");
        }
    }
}