using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System.Text;

namespace Maximiz.InputModels.Campaigns
{
    /// <summary>
    /// Name generator for the <see cref="Campaign"></see> class.
    /// </summary>
    public static class CampaignNameGenerator
    {
        // TODO: Ensure this name will always be unique (currently not)
        //       Maybe add properties from the linked account.

        /// <summary>
        /// Generates a name for a given <see cref="Campaign"></see>.
        /// </summary>
        /// <param name="groupName">Name of the campaign's group</param>
        /// <param name="language">Language of the campaign</param>
        /// <param name="os">Targeted OS</param>
        /// <param name="device">Targeted device</param>
        /// <returns></returns>
        public static string Generate(string groupName, string language, string location, OS os, Device device)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(groupName.ToUpper());
            sb.Append("_");
            sb.Append(location);
            sb.Append("_");
            sb.Append(language.ToUpper());
            sb.Append("_");
            sb.Append(os.GetEnumMemberName().ToUpper().Substring(0, 2));
            sb.Append("_");
            sb.Append(device.GetEnumMemberName().ToUpper().Substring(0, 3));
            return sb.ToString();
        }
    }
}
