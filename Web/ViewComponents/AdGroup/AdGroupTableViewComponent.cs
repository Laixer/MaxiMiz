using Maximiz.Repositories.Abstraction;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maximiz.ViewComponents.AdGroup
{

    /// <summary>
    /// Gets ad groups from our data store and displays them as table rows.
    /// </summary>
    public sealed class AdGroupTableViewComponent : ViewComponent
    {

        /// <summary>
        /// Data store for <see cref="AdGroup"/>s.
        /// </summary>
        private readonly IAdGroupRepository _adGroupRepository;

        /// <summary>
        /// Constructor for dependency injection.
        /// </summary>
        public AdGroupTableViewComponent(IAdGroupRepository adGroupRepository)
        {
            _adGroupRepository = adGroupRepository;
        }

        public async Task<ViewComponentResult> InvokeAsync(AdGroupTableSettings settings)
        {
            throw new NotImplementedException();
        }

    }
}
