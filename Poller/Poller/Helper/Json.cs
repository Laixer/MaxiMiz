using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Poller.Helper
{
    public static class Json
    {
        public static T Deserialize<T>(Stream stream)
            where T : class
        {
            using (var streamReader = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(streamReader))
            {
                return new JsonSerializer().Deserialize<T>(jsonTextReader);
            }
        }

        public static async Task<T> DeserializeAsync<T>(HttpResponseMessage message)
            where T : class
        {
            using (var stream = await message.Content.ReadAsStreamAsync())
            {
                return Deserialize<T>(stream);
            }
        }
    }
}
