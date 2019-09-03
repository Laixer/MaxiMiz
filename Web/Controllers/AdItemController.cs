using Maximiz.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Maximiz.Controllers
{
    /// <summary>
    /// Controller for requests related to Ad Items.
    /// </summary>
    public class AdItemController : Controller
    {
        private readonly IAdItemRepository _adItemRepo;

        public AdItemController(IAdItemRepository adItemRepository)
        {
            _adItemRepo = adItemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        // TODO Change method
        // GET: /Advertisements
        /// <summary>
        /// (Temporary) action for displaying an overview of advertisements.
        /// </summary>
        /// <returns></returns>
        [Route("Advertisements")]
        public async Task<IActionResult> Overview()
        {
            var ads = await _adItemRepo.GetAds();
            return View(ads);
        }

    }
}