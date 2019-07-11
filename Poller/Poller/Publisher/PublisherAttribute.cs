using System;

namespace Poller.Publisher
{
    /// <summary>
    /// Annotation for publishers.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public sealed class PublisherAttribute : Attribute
    {
        /// <summary>
        /// The name of a publisher.
        /// </summary>
        public string Name { get; set; }

        public PublisherAttribute(string name)
        {
            Name = name;
        }
    }
}
