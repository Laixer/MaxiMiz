
namespace Maximiz.Infrastructure.Repositories.Sql
{

    /// <summary>
    /// Contains sql statements for our <see cref="CampaignRepository"/>.
    /// </summary>
    internal static class SqlCampaignRepository
    {

        /// <summary>
        /// Sql statement to get a single campaign from our database based on its internal id.
        /// </summary>
        internal static string GetCampaign = @"SELECT 1 FROM public.campaign WHERE id = @Id";

        /// <summary>
        /// Sql statement to get a single campaign from our database based on its external id.
        /// </summary>
        internal static string GetCampaignFromExternalId = @"SELECT 1 FROM public.campaign WHERE id = @ExternalId"

        /// <summary>
        /// Sql statement to get all entries from the campaign table.
        /// </summary>
        internal static string GetAllCampaigns = "SELECT * FROM public.campaign";

    }
}
