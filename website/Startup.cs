using Owin;
using System;

namespace evefifo.website
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            Configuration()(app);
        }

        public static Action<IAppBuilder> Configuration()
        {
            return app => app.UseWelcomePage("/");
        }
    }
}