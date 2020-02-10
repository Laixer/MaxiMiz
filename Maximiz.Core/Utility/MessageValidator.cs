using Maximiz.Model.Protocol;
using System;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Used to validate an <see cref="OperationMessage"/>.
    /// </summary>
    public static class MessageValidator
    {

        /// <summary>
        /// Validates a <see cref="OperationMessage"/> and throws an
        /// <see cref="ArgumentNullException"/> if it's invalid.
        /// </summary>
        /// <param name="message"><see cref="OperationMessage"/></param>
        public static void Validate(OperationMessage message)
        {
            if (message == null) { throw new ArgumentNullException("Message can't be null"); }
            if (message.Entity == null) { throw new ArgumentNullException("Message entity can't be null"); }
            ValidateGuid(message.Entity.Id, "Entity");
            ValidateGuid(message.OperationId, "Operation");
        }

        /// <summary>
        /// Validates a <see cref="Guid"/> and throws an 
        /// <see cref="ArgumentNullException"/> if it's null or empty.
        /// </summary>
        /// <param name="guid"><see cref="Guid"/></param>
        /// <param name="name">The optional name to display in the error message</param>
        private static void ValidateGuid(Guid guid, string name = null)
        {
            if (guid == null || guid == Guid.Empty) { throw new ArgumentNullException($"{(!string.IsNullOrEmpty(name) ? $"{name} " : "")}Guid can't be null or empty"); }
        }

    }
}
