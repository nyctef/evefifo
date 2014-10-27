using evefifo.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.routing
{
    public class Request
    {
        public readonly HttpMethod Method;
        public readonly string Path;
        public readonly IRepository Repository;
        public readonly IDictionary<string, string> Parameters;
        public readonly Stream Body;

        public Request(HttpMethod method, string path, IDictionary<string, string> parameters, IRepository repository, Stream body)
        {
            Method = method;
            Path = path;
            Parameters = parameters;
            Repository = repository;
            Body = body;
        }
    }
}
