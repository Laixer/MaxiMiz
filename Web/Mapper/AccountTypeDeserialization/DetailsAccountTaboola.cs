
namespace Maximiz.Mapper.AccountTypeDeserialization
{

    /// <summary>
    /// Model for deserializing the details string of a Taboola account.
    /// </summary>
    internal sealed class DetailsAccountTaboola
    {

        /// <summary>
        /// Array containing the type of account.
        /// TODO What if we are both publisher and advertiser?
        /// </summary>
        //public AccountTypeTaboola[] PartnerTypes { get; set; }
        public string[] PartnerTypes { get; set; }

    }

}
