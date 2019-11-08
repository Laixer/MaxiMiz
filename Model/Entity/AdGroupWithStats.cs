using System;

namespace Maximiz.Model.Entity
{
    /// <summary>
    /// Represents a <see cref="AdGroup"/> along with some calculated statistics.
    /// This entity is obtained from one of our database views.
    /// </summary>
    [Serializable]
    public class AdGroupWithStats : AdGroup
    { 

        /// <summary>
        /// Total amount of ad items present in this ad group.
        /// </summary>
        public int AdItemCount { get; set; }

    }
}
