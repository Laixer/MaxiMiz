using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MaxiMiz.Poller.Helper
{

    static internal class Json
    {

        internal static T Deserialize<T>(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return new JsonSerializer().Deserialize<T>(jsonTextReader);
            }

        }

        internal static async Task<T> Deserialize<T>(HttpResponseMessage message)
        {
            using (var stream = await message.Content.ReadAsStreamAsync())
            {
                return Deserialize<T>(stream);
            }

        }
    }


}