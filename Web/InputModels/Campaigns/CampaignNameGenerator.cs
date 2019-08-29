using Maximiz.Model.Entity;
using Maximiz.Model.Enums;
using System.Text;

namespace Maximiz.InputModels.Campaigns
{
    public static class CampaignNameGenerator
    {
        // TODO: Ensure this name will always be unique (currently not)
        //       Maybe add properties from the linked account.

        /// <summary>
        /// Generates a name for a given <see cref="Campaign"></see>.
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="language"></param>
        /// <param name="os"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public static string Generate(string groupName, string language, string location, OS os, Device device)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(language.ToUpper());
            sb.Append("_");
            sb.Append(os.GetEnumMemberName().ToUpper().Substring(0,2));
            sb.Append("_");
            sb.Append(device.GetEnumMemberName().ToUpper().Substring(0,3));
            sb.Append("_");
            sb.Append(groupName.ToUpper());
            sb.Append("_");
            sb.Append(location);

            return sb.ToString();
        }
    }
}
