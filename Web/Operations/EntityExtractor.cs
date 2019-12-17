using Maximiz.Core.StateMachine.Abstraction;
using Maximiz.Model.Operations;
using Maximiz.ViewModels.CampaignDetails;
using System;
using System.Threading.Tasks;

namespace Maximiz.Operations
{

    /// <summary>
    /// Contains functionality for extracting the correct <see cref="EntityMap"/>
    /// based on a form post.
    /// </summary>
    internal sealed class EntityExtractor : IEntityExtractor
    {

        private readonly IStateManager _stateManager;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public EntityExtractor(IStateManager stateManager)
        {
            _stateManager = stateManager ?? throw new ArgumentNullException(nameof(stateManager));
        }

        public Task<EntityMap> Extract(FormCampaignDetailsViewModel form)
        {
            throw new NotImplementedException();
        }

    }
}
