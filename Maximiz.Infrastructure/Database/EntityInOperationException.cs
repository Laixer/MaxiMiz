using System;

namespace Maximiz.Infrastructure.Database
{

    /// <summary>
    /// Indicates an entity is in an operation and can't be modified.
    /// </summary>
    public class EntityInOperationException : Exception
    {

        public EntityInOperationException(string message)
            : base(message) { }

    }

}
