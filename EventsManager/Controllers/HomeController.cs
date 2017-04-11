using EventsManager.Methods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsManager.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        [Authorize]
        public ActionResult Index()
        {
            var currentuser = HttpContext.User.Identity.Name;
            var userid = ManageEvents.CreatedByLoginID(currentuser);
            var count = ManageEvents.GetMyEvents(userid).Count();

            if (count != 0)
            {
                return RedirectToAction("MyEvents", "Events");
            }
            else
            {
                return View();
            }
        }
    }
}