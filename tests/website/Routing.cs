using evefifo.website;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using System.Net;

namespace evefifo.tests.website
{
    public class Routing
    {
        [Test]
        public async void RootPathReturns200()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            }
        }
    }
}
