using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// This is the format in which the Taboola API 
    /// returns their lists. This servers as our
    /// deserialization template.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    [DataContract]
    internal class EntityList<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// The items in this result.
        /// </summary>
        [DataMember(Name = "results")]
        public IEnumerable<TEntity> Items { get; set; }
    }
}
