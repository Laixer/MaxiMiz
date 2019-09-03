using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Maximiz.Repositories;
using Maximiz.Repositories.Interfaces;

namespace Maximiz.Controllers
{
    public class AdItemController : Controller
    {
        private readonly IAdItemRepository _adItemRepo;

        public AdItemController(IAdItemRepository adItemRepository) {
            _adItemRepo = adItemRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Advertisements")]
        public async Task<IActionResult> Overview()
        {
            var ads = await _adItemRepo.GetAds();
            return View(ads);
        }

    }
}