using evefifo.website.routing;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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
            Assert.True(route.Matches("/asdf").Success);
            Assert.AreEqual(0, route.Matches("/asdf").Parameters.Count);

            Assert.False(route.Matches("/").Success);
            Assert.AreEqual(0, route.Matches("/").Parameters.Count);
        }

        [Test]
        public void CanMatchParameter()
        {
            var route = new Route("/asdf/{id}", null);
            var match = route.Matches("/asdf/1");
            Assert.True(match.Success);
            Assert.AreEqual(1, match.Parameters.Count);
            Assert.AreEqual("id", match.Parameters.Single().Key);
            Assert.AreEqual("1", match.Parameters.Single().Value);
        }
    }
}
