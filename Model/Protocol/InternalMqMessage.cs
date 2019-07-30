using System;

namespace Maximiz.Model.Protocol
{
    /// <summary>
    /// Represents messages for 'internal-mq' event queue.
    /// </summary>
    [Serializable]
    public sealed class InternalMqMessage
    {
        public static class Protocol
        {
            public static ushort[] Header = new ushort[] { 0x12, 0xe7 };
            public static byte Version = 0x2;
        }

        public enum Action
        {
            Create,
            Update,
            Delete,
            Syncback,
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        public InternalMqMessage() { }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="action"></param>
        public InternalMqMessage(Entity.Entity entity, Action action)
        {
            Header = Protocol.Header;
            Version = Protocol.Version;
            Entity = entity;
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
        public Action EntityAction { get; set; }

        /// <summary>
        /// Entity to operate on.
        /// </summary>
        public Entity.Entity Entity { get; set; }
    }
}
