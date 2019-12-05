using Maximiz.Model.Entity;
using System;
using System.Collections.Generic;

namespace Maximiz.Core.Operations
{

    /// <summary>
    /// Contract for an operation. 
    /// </summary>
    public sealed class Operation : Entity<Guid>
    {

        /// <summary>
        /// All entities that are affected by this operation.
        /// </summary>
        public IEnumerable<Entity> Entities { get; set; }

        /// <summary>
        /// Indicates the amount of times we have reached the failure state.
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// The <see cref="DateTime"/> at which this operation was started.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 
        /// The <see cref="DateTime"/> at which this operation was ended.
        /// </summary>
        public DateTime EndDate { get; set; }

    }
}
