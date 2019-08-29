using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Poller.Taboola.Model
{

    /// <summary>
    /// Object used by Taboola to indicate which items 
    /// should be targeted and which items should not be 
    /// targeted. Items can be any country, OS, etc. 
    /// This is used as an abstraction because this 
    /// target class is returned in two different ways.
    /// </summary>
    internal interface ITarget {
        /// <summary>
        /// The value string array can either be
        /// include, exclude or all.
        /// </summary>
        [DataMember(Name = "type")]
        TargetType? Type { get; set; }

        /// <summary>
        /// If the value list is too long, this link
        /// contains an API call to retrieve all our
        /// values.
        /// </summary>
        [DataMember(Name = "href")]
        string Href { get; set; }
    }



    /// <summary>
    /// Base abstraction with unconverted Json object.
    /// </summary>
    internal class TargetBase : ITarget
    {
        /// <summary>
        /// The value string array can either be
        /// include, exclude or all.
        /// </summary>
        [DataMember(Name = "type")]
        public TargetType? Type { get; set; }

        /// <summary>
        /// Object which is to be converted.
        /// </summary>
        [DataMember(Name = "value")]
        public object Value { get; set; }

        /// <summary>
        /// If the value list is too long, this link
        /// contains an API call to retrieve all our
        /// values.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }
    }



    /// <summary>
    /// Externds the target base. This is our most 
    /// common result.
    /// </summary>
    internal class TargetDefault : TargetBase
    {
        /// <summary>
        /// The value string array can either be
        /// include, exclude or all.
        /// </summary>
        [DataMember(Name = "type")]
        public TargetType? Type { get; set; }

        /// <summary>
        /// If the value list is too long, this link
        /// contains an API call to retrieve all our
        /// values.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }

        /// <summary>
        /// String array containing relevant string 
        /// identifiers.
        /// </summary>
        [DataMember(Name="value")]
        new public string[] Value { get; set; }
    }



    /// <summary>
    /// Extends the target base. This only occurs
    /// when we target specific iOS families.
    /// </summary>
    internal class TargetOsFamily : TargetBase
    {
        /// <summary>
        /// Hide.
        /// </summary>
        new private object Value;

        /// <summary>
        /// The value string array can either be
        /// include, exclude or all.
        /// </summary>
        [DataMember(Name = "type")]
        public TargetType? Type { get; set; }

        /// <summary>
        /// If the value list is too long, this link
        /// contains an API call to retrieve all our
        /// values.
        /// </summary>
        [DataMember(Name = "href")]
        public string Href { get; set; }

        /// <summary>
        /// OS family.
        /// </summary>
        [DataMember(Name = "os_family")]
        public string OsFamily { get; set; }

        /// <summary>
        /// OS family subtypes.
        /// </summary>
        [DataMember(Name = "sub_categories")]
        public string[] SubCategories { get; set; }
    }
}
