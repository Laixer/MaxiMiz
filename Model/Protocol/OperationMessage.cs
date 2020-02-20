using Maximiz.Model.Entity;
using System;

namespace Maximiz.Model.Protocol
{
    /// <summary>
    /// Represents a message to be sent to the external processing unit.
    /// </summary>
    [Serializable]
    public sealed class OperationMessage
    {
        /// <summary>
        /// Protocol constants.
        /// </summary>
        public static class Protocol
        {
            public static ushort[] Header = new ushort[] { 0x12, 0xe7 };
            public static byte Version = 0x2;
        }

        public OperationMessage(Entity<Guid> entity, CrudAction crudAction, Guid operationId)
        {
            if (entity == null) { throw new ArgumentNullException(nameof(entity)); }
            if (entity.Id == null || entity.Id == Guid.Empty) { throw new ArgumentNullException("Entity id can't be null"); }
            if (operationId == null  || operationId == Guid.Empty) { throw new ArgumentNullException("Operation id can't be null"); }

            Header = Protocol.Header;
            Version = Protocol.Version;

            Entity = entity;
            CrudAction = crudAction;
            OperationId = operationId;
        }

        /// <summary>
        /// Message header.
        /// </summary>
        public ushort[] Header { get; set; }

        /// <summary>
        /// Protocol version.
        /// </summary>
        public byte Version { get; set; }

        /// <summary>
        /// What to do with the Entity.
        /// </summary>
        public CrudAction CrudAction { get; set; }

        /// <summary>
        /// Entity to operate on.
        /// </summary>
        /// <remarks>
        /// This should only be used for the type and for the id.
        /// </remarks>
        public Entity<Guid> Entity { get; set; }

        /// <summary>
        /// The id of the operation we are performing in.
        /// </summary>
        public Guid OperationId { get; set; }

    }
}
