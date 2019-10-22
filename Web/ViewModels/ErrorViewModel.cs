
namespace Maximiz.ViewModels
{

    /// <summary>
    /// Viewmodel used to display errors.
    /// </summary>
    public sealed class ErrorViewModel
    {

        /// <summary>
        /// The request id that produced the error.
        /// </summary>
        public string RequestId { get; set; }

        /// <summary>
        /// Used to handle and prevent request id null pointer exceptions.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    }
}