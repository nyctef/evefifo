using evefifo.website;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    public class StaticFilesTests
    {
        [Test]
        public async void CanGetPureCssAsStaticFile()
        {
            using (var server = TestServer.Create<Startup>())
            {
                var response = await server.HttpClient.GetAsync("/pure.css");
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                StringAssert.AreEqualIgnoringCase("text/css", response.Content.Headers.GetValues("content-type").Single());
            }
        }
    }
}
