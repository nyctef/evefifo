using evefifo.website;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using System.Net;
using Moq;

namespace evefifo.tests.website
{
    public class Routing
    {
        private static TestServer GetServer()
        {
            var repo = new Mock<IRepository>();
            return TestServer.Create(Startup.Configuration(repo.Object));
        }

        [Test]
        public async void RootPathReturns200()
        {
            using (var server = GetServer())
            {
                var response = await server.HttpClient.GetAsync("/");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }

        [Test]
        public async void UnknownPathReturns404()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/asldkjfhalskdjfhlasdjfhas");
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            }
        }
    }
}
