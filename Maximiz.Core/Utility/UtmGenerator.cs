using Maximiz.Model.Entity;

namespace Maximiz.Core.Utility
{

    /// <summary>
    /// Contains functionality to generate UTM codes.
    /// </summary>
    public static class UtmGenerator
    {

        /// <summary>
        /// Generates an UTM code for a <see cref="Campaign"/> based on its
        /// parent <see cref="CampaignGroup"/>.
        /// </summary>
        /// <remarks>
        /// At the moment this just returns <see cref="null"/>, since we have not
        /// yet fully implemented the utm codes.
        /// </remarks>
        /// <returns><see cref="null"/></returns>
        public static string ForCampaignFromCampaignGroup(CampaignGroup campaignGroup)
            => null;

        /// <summary>
        /// TODO What to do with this?
        /// </summary>
        /// <returns></returns>
        public static string Default()
            => null;

    }
}
