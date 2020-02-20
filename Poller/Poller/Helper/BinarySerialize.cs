using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Poller.Helper
{
    public static class BinarySerializer
    {
        private static readonly BinaryFormatter binaryFormatter = new BinaryFormatter();

        /// <summary>
        /// Deserialize stream into object.
        /// </summary>
        /// <typeparam name="T">Output type.</typeparam>
        /// <param name="stream">The stream containing the data.</param>
        /// <returns>The Derserialized instance of <see cref="T"/>.</returns>
        public static T Deserialize<T>(Stream stream)
            where T : class
        {
            return (T)binaryFormatter.Deserialize(stream);
        }

        public static byte[] Serialize(object obj)
        {
            using (var stream = new MemoryStream())
            {
                binaryFormatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }
    }
}
