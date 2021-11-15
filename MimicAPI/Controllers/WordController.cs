using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MimicAPI.Controllers
{
    public class WordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
