using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChessPlaying.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult View(Guid id)
        {
            ViewBag.SessionName = id.ToString();
            return View();
        }
    }
}
