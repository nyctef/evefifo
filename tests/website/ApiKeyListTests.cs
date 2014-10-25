using evefifo.model;
using evefifo.website;
using Microsoft.Owin.Testing;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    class ApiKeyListTests
    {
        private static TestServer GetServer(IRepository repo)
        {
            return TestServer.Create(Startup.Configuration(() => repo));
        }

        private static IRepository CreateRepo()
        {
            var repo = new Mock<IRepository>();
            repo.SetupGet(x => x.ApiKeys).ReturnsAsync(new List<ApiKey> {
                new ApiKey { Id = 1234, Secret = "api_secret_1" }
            });
            return repo.Object;
        }

        [Test]
        public async Task ApiKeyListContainsKeysInRepo()
        {
            using (var server = GetServer(CreateRepo()))
            {
                var body = await Utils.GetBody(await server.HttpClient.GetAsync("/apikeys"));
                StringAssert.Contains("1234", body.InnerText);
                StringAssert.Contains("api_secret_1", body.InnerText);
            }
        }
    }
}