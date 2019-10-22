using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Maximiz.Controllers
{
    public class SettingsController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Account()
        {
            return View("Account");
        }

        [HttpGet]
        public IActionResult Preferences()
        {
            return View("Preferences");
        }

        [HttpGet]
        public IActionResult Billing()
        {
            return View("Billing");
        }

        [HttpGet]
        public IActionResult Integrations()
        {
            return View("Integrations");
        }

        [HttpGet]
        public IActionResult Settings()
        {
            return View("Settings");
        }

    }
}
