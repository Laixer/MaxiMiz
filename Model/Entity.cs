namespace Maximiz.Model
{
    /// <summary>
    /// Datastore entity.
    /// </summary>
    /// <typeparam name="TPrimary">Primary key.</typeparam>
    public abstract class Entity<TPrimary>
    {
        /// <summary>
        /// Entity identifier.
        /// </summary>
        public TPrimary Id { get; set; }
    }
}
