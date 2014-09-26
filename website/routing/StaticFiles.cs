using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace evefifo.website.routing
{
    public class StaticFiles
    {
        private readonly AppFunc m_Next;

        public StaticFiles(AppFunc next)
        {
            m_Next = next;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            string path = (string)environment["owin.RequestPath"];
            if (ContainsStaticFile(path))
            {
                var mimeType = GetMimeType(Path.GetExtension(path));
                var headers = (IDictionary<string, string[]>)environment["owin.ResponseHeaders"];
                headers["Content-Type"] = new [] { mimeType };
                using (var fileStream = File.OpenRead(StaticFilePath(path)))
                {
                    var responseStream = (Stream)environment["owin.ResponseBody"];
                    await fileStream.CopyToAsync(responseStream);
                }
                environment["owin.ResponseCode"] = HttpStatusCode.OK;
                return;
            }

            await m_Next(environment);
        }

        private string GetMimeType(string extension)
        {
            switch (extension)
            {
                case ".css": return "text/css";
                case ".js":  return "application/javascript";
                default:     return "text/plain";
            }
        }

        private bool ContainsStaticFile(string filePath)
        {
            var fullPath = StaticFilePath(filePath);
            return File.Exists(fullPath);
        }

        private string StaticFilePath(string path)
        {
            return Path.GetFullPath(Path.Combine("static", path.TrimStart('/')));
        }
    }

    public static class AppBuilderExtensionsForStaticFiles
    {
        public static void UseStaticFiles(this IAppBuilder app)
        {
            app.Use<StaticFiles>();
        }
    }
}
