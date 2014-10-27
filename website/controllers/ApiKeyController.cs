using evefifo.model;
using evefifo.website.routing;
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

        public async Task List(Request request)
        {
            var repo = request.Repository;
            var model = new { ApiKeys = await repo.ApiKeys };
            string result = await CompileView("ApiKeyList", model);

            await WriteResponse(request.Environment, result);
        }

        public async Task Show(Request request)
        {
            
        }

        public async Task Add(Request request)
        {
            var path = request.Path;
            var repo = request.Repository;
            var postData = request.Body;
            var json = await new StreamReader(postData).ReadToEndAsync();
            dynamic data = JObject.Parse(json);

            repo.AddApiKey(new ApiKey { Id = data.id, Secret = data.secret });

            await Write303(request.Environment, path);
        }
    }
}
