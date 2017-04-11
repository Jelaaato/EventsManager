using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EventsManager.ViewModels;
using EventsManager.IdentityModel;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.Owin;
using Owin;
using Microsoft.Owin.Security.Cookies;
using EventsManager.Methods;

namespace EventsManager.Controllers
{
    public class AccountController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(AccountModel.LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Users user = await UserManager.FindAsync(model.username, model.password);

                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
                else
                {
                    ClaimsIdentity claimsidentity = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    await SignInAsync(user, isPersistent: false);
                    return RedirectToLocal(returnUrl);
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(AccountModel.ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = UserManager.ChangePassword(User.Identity.GetUserId(), model.oldpassword, model.newpassword);

                if (result.Succeeded)
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());

                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }

                    return RedirectToAction("Logout");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(model);
        }

        [Authorize]
        public ActionResult Logout()
        {
            AuthenticationMgr.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #region Helpers

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private UsersManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }

        private IAuthenticationManager AuthenticationMgr
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private async Task SignInAsync(Users user, bool isPersistent)
        {
            AuthenticationMgr.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationMgr.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        #endregion
        
    }
}