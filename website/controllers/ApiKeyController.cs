using evefifo.model;
using evefifo.website.routing;
using evefifo.website.routing.responses;
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

        public async Task<Response> List(Request request)
        {
            var repo = request.Repository;
            return new ViewResponse("ApiKeyList", new { ApiKeys = await repo.ApiKeys });
        }

        public async Task<Response> Show(Request request)
        {
            return new NotFoundResponse();
        }

        public async Task<Response> Add(Request request)
        {
            var path = request.Path;
            var repo = request.Repository;
            var postData = request.Body;
            var json = await new StreamReader(postData).ReadToEndAsync();
            dynamic data = JObject.Parse(json);

            repo.AddApiKey(new ApiKey { Id = data.id, Secret = data.secret });

            return new SeeOtherResponse(request.Path);
        }
    }
}
