using Maximiz.Model.Entity;

namespace Maximiz.InputModels
{
    /// <summary>
    /// An model containing the user based input for a single <see cref="AdItem"></see>
    /// </summary>
    public class AdItemInputModel
    {
        /// <summary>
        /// Advertisement title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Advertisement content.
        /// </summary>
        public string Content { get; set; }
    }
}
