using eZet.EveLib.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace evefifo.api_pull
{
    class Program
    {
        static void Main(string[] args)
        {
            var charId = args[0];
            var apiKey = args[1];
            var apiSecret = args[2];

            PullCharacterInfo(Int32.Parse(apiKey), apiSecret, Int32.Parse(charId)).Wait();
        }

        static async Task PullCharacterInfo(int apiKey, string apiSecret, int charId)
        {
            var charKey = EveOnlineApi.CreateCharacterKey(apiKey, apiSecret);

            var character = await ModelCharacter.FromApi(charKey, charId);
        }
    }
}
