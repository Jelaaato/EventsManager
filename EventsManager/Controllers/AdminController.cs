using EventsManager.IdentityModel;
using EventsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;

namespace EventsManager.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        private EventsIdentityDb db = new EventsIdentityDb();

        [Authorize]
        public ActionResult Index()
        {
            var users = db.Users.ToList();
            return View(users);
        }

        [Authorize]
        public ActionResult CreateUser()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateUser(AdminModel.CreateUserModel model)
        {
            if (ModelState.IsValid)
            {
                Users user = new Users { UserName = model.username };

                IdentityResult result = UserManager.Create(user, model.password);
                if (result.Succeeded)
                {
                    var currentuser = UserManager.FindByName(user.UserName);
                    UserManager.AddToRole(currentuser.Id, "User");
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
            
        }

        #region Helpers

        private UsersManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        #endregion

    }
}