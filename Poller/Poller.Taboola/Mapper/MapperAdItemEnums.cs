using AdItemStatusExternal = Poller.Taboola.Model.CampaignItemStatus;
using AdItemStatusInternal = Maximiz.Model.Enums.AdItemStatus;

namespace Poller.Taboola.Mapper
{

    /// <summary>
    /// This partial section contains all explicit conversions for our enums.
    /// Taboola only accepts upper case strings, these functions help us with
    /// converting our enums to this accepted format.
    /// </summary>
    internal partial class MapperAdItem
    {

        /// <summary>
        /// Converts an internal status to an exteral upper case string
        /// that represents a <see cref="AdItemStatusExternal.PendingApproval"/>.
        /// </summary>
        /// <remarks>Default: <see cref="AdItemStatusExternal.PendingApproval"/>.</remarks>
        /// <param name="internalEnum">The internal status</param>
        /// <returns>Upper case string</returns>
        internal string AdItemStatusToString(AdItemStatusInternal internalEnum)
        {
            var externalEnum = AdItemStatusExternal.PendingApproval;

            switch (internalEnum)
            {
                case AdItemStatusInternal.Unknown:
                    externalEnum = AdItemStatusExternal.PendingApproval;
                    break;
                case AdItemStatusInternal.Running:
                    externalEnum = AdItemStatusExternal.Running;
                    break;
                case AdItemStatusInternal.Crawling:
                    externalEnum = AdItemStatusExternal.Crawling;
                    break;
                case AdItemStatusInternal.CrawlingError:
                    externalEnum = AdItemStatusExternal.CrawlingError;
                    break;
                case AdItemStatusInternal.NeedToEdit:
                    externalEnum = AdItemStatusExternal.NeedToEdit;
                    break;
                case AdItemStatusInternal.Paused:
                    externalEnum = AdItemStatusExternal.Paused;
                    break;
                case AdItemStatusInternal.Stopped:
                    externalEnum = AdItemStatusExternal.Stopped;
                    break;
                case AdItemStatusInternal.PendingApproval:
                    externalEnum = AdItemStatusExternal.PendingApproval;
                    break;
                case AdItemStatusInternal.Rejected:
                    externalEnum = AdItemStatusExternal.Rejected;
                    break;
            }

            return _utility.ToUpperString(externalEnum);
        }

        /// <summary>
        /// Converts an upper case string that represents an external
        /// <see cref="AdItemStatusExternal"/> to our internal model.
        /// </summary>
        /// <remarks>Default: <see cref="AdItemStatusInternal.Unknown"/>.</remarks>
        /// <param name="upperCaseString">Upper case string</param>
        /// <returns>Internal status</returns>
        internal AdItemStatusInternal AdItemStatusToInternal(string upperCaseString)
        {
            var externalEnum = _utility.FromUpperString(upperCaseString, AdItemStatusExternal.PendingApproval);
            switch (externalEnum)
            {
                case AdItemStatusExternal.Running:
                    return AdItemStatusInternal.Running;
                case AdItemStatusExternal.Crawling:
                    return AdItemStatusInternal.Crawling;
                case AdItemStatusExternal.CrawlingError:
                    return AdItemStatusInternal.CrawlingError;
                case AdItemStatusExternal.NeedToEdit:
                    return AdItemStatusInternal.NeedToEdit;
                case AdItemStatusExternal.Paused:
                    return AdItemStatusInternal.Paused;
                case AdItemStatusExternal.Stopped:
                    return AdItemStatusInternal.Stopped;
                case AdItemStatusExternal.PendingApproval:
                    return AdItemStatusInternal.PendingApproval;
                case AdItemStatusExternal.Rejected:
                    return AdItemStatusInternal.Rejected;
                default:
                    return AdItemStatusInternal.Unknown;
            }
        }

    }
}
