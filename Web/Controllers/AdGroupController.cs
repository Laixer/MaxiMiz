using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Maximiz.Model.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Maximiz.Controllers
{
    public class AdGroupController : Controller
    {
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(AdGroup model)
        {
            // Test
            var x = model.Name;

            return View();
        }
    }
}