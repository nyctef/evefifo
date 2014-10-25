using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.website.controllers
{
    class ApiKeyController : ControllerBase
    {
        public async Task List(IDictionary<string, object> environment)
        {
            var repo = GetRepository(environment);
            var model = new { ApiKeys = await repo.ApiKeys };
            string result = await CompileView("ApiKeyList", model);

            await WriteResponse(environment, result);
        }

        public async Task Show(IDictionary<string, object> environment)
        {
            
        }
    }
}
