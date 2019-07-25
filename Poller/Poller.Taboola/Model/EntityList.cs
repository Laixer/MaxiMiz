using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Poller.Taboola.Model
{
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
