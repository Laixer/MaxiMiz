namespace Maximiz.Model
{

    /// <summary>
    /// Represents the type of action our backend wants our poller to execute.
    /// </summary>
    public enum CrudAction
    {
        /// <summary>
        /// Create a new entity.
        /// </summary>
        Create,

        /// <summary>
        /// Read an entity.
        /// </summary>
        Read,

        /// <summary>
        /// Update an entity.
        /// </summary>
        Update,

        /// <summary>
        /// Delete an entity.
        /// </summary>
        Delete,

        /// <summary>
        /// Syncback some information from external API's for asap information
        /// display.
        /// </summary>
        Syncback
    }
}
