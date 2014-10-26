using evefifo.website.routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.tests.website
{
    public class RouteTests
    {
        [Test]
        public void CanMatchPlainText()
        {
            var route = new Route("/asdf", null);
            Assert.True(route.Matches(HttpMethod.Get, "/asdf").Success);
            Assert.AreEqual(0, route.Matches(HttpMethod.Get, "/asdf").Parameters.Count);

            Assert.False(route.Matches(HttpMethod.Get, "/").Success);
            Assert.AreEqual(0, route.Matches(HttpMethod.Get, "/").Parameters.Count);
        }

        [Test]
        public void CanMatchParameter()
        {
            var route = new Route("/asdf/{id}", null);
            var match = route.Matches(HttpMethod.Get, "/asdf/1");
            Assert.True(match.Success);
            Assert.AreEqual(1, match.Parameters.Count);
            Assert.AreEqual("id", match.Parameters.Single().Key);
            Assert.AreEqual("1", match.Parameters.Single().Value);
        }

        [Test]
        public void DoesNotMatchDifferentHttpMethod()
        {
            var route = new Route("/asdf/{1234}", null, HttpMethod.Get);
            var match = route.Matches(HttpMethod.Post, "/asdf/1");
            Assert.False(match.Success);
        }
    }
}
