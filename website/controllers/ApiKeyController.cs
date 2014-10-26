using evefifo.model;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.controllers
{
    public class ApiKeyController : ControllerBase
    {
        public ApiKeyController() { }

        public async Task List(IDictionary<string, object> environment)
        {
            var repo = GetRepository(environment);
            var model = new { ApiKeys = await repo.ApiKeys };
            string result = await CompileView("ApiKeyList", model);

            await WriteResponse(environment, result);
        }

        public async Task Show(IDictionary<string, object> environment)
        {
            
        }

        public async Task Add(IDictionary<string, object> environment)
        {
            var path = (string)environment["owin.RequestPath"];
            var repo = GetRepository(environment);
            var postData = (Stream)environment["owin.RequestBody"];
            var json = await new StreamReader(postData).ReadToEndAsync();
            dynamic data = JObject.Parse(json);

            repo.AddApiKey(new ApiKey { Id = data.id, Secret = data.secret });

            await Write303(environment, path);
        }
    }
}
