using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChessPlaying.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult View(Guid id, [FromQuery(Name = "nickname")] string nickname)
        {
            ViewBag.SessionName = id.ToString();
            ViewBag.PlayerNickname = nickname;
            return View();
        }
    }
}
