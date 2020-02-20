using System;

namespace Maximiz.Model.Protocol
{
    /// <summary>
    /// Represents messages for 'internal-mq' event queue.
    /// </summary>
    [Serializable]
    public sealed class CreateOrUpdateObjectsMessage
    {
        /// <summary>
        /// Protocol constants.
        /// </summary>
        public static class Protocol
        {
            public static ushort[] Header = new ushort[] { 0x12, 0xe7 };
            public static byte Version = 0x2;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public CreateOrUpdateObjectsMessage() { }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="entity">Input entity.</param>
        /// <param name="action">Action on entity.</param>
        public CreateOrUpdateObjectsMessage(Entity.Entity entity, CrudAction action)
        {
            Header = Protocol.Header;
            Version = Protocol.Version;
            Entity = new Entity.Entity[] { entity };
            EntityAction = action;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="entities">Input entities.</param>
        /// <param name="action">Action on entity.</param>
        public CreateOrUpdateObjectsMessage(Entity.Entity[] entities, CrudAction action)
        {
            Header = Protocol.Header;
            Version = Protocol.Version;
            Entity = entities;
            EntityAction = action;
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
        public CrudAction EntityAction { get; set; }

        /// <summary>
        /// Entity to operate on.
        /// </summary>
        public Entity.Entity[] Entity { get; set; }

        /// <summary>
        /// Length of the entity array.
        /// </summary>
        public int EntityLength { get; set; }
    }
}
