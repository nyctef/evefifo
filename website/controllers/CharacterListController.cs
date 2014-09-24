using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.controllers
{
    class CharacterListController
    {
        public void Invoke(IDictionary<string, object> environment)
        {
            environment["owin.ResponseStatusCode"] = HttpStatusCode.OK;
        }
    }
}
