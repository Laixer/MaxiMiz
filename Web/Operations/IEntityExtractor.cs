using Maximiz.Model.Operations;
using Maximiz.ViewModels.CampaignDetails;
using System.Threading.Tasks;

namespace Maximiz.Operations
{

    /// <summary>
    /// Contract for extracting a <see cref="EntityMap"/> from a form post.
    /// </summary>
    public interface IEntityExtractor
    {

        Task<EntityMap> Extract(FormCampaignDetailsViewModel form);

    }
}
