using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using System.Security.Claims;
using Microsoft.Owin.Security;
using System.ComponentModel.DataAnnotations;
using EventsManager.IdentityModel;
using EventsManager.ViewModels;

namespace EventsManager.Controllers
{
    public class RoleController : Controller
    {
        // GET: Role
        [Authorize]
        public ActionResult Index()
        {
            return View(RoleManager.Roles);
        }

        [Authorize]
        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult CreateRole([Required] string name)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = RoleManager.Create(new roles(name));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            return View(name);
        }

        public ActionResult AddToRole(string id)
        {
            roles role = RoleManager.FindById(id);
            string[] memberIDs = role.Users.Select(a => a.UserId).ToArray();
            IEnumerable<Users> members = UserManager.Users.Where(b => memberIDs.Any(c => c == b.Id));
            IEnumerable<Users> nonMembers = UserManager.Users.Except(members);
            return View(new AdminModel.RoleModel
            {
                role = role,
                members = members,
                nonmembers = nonMembers
            });
        }

        [HttpPost]
        [Authorize]
        public ActionResult AddToRole(AdminModel.ModifyRoleUsersModel model)
        {
            IdentityResult result;

            if (ModelState.IsValid)
            {
                foreach (string userId in model.idstoadd ?? new string[] { })
                {
                    result = UserManager.AddToRole(userId, model.rolename);

                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                }
                foreach (string userId in model.idstodelete ?? new string[] { })
                {
                    result = UserManager.RemoveFromRole(userId, model.rolename);
                    if (!result.Succeeded)
                    {
                        AddErrorsFromResult(result);
                    }
                }
                return RedirectToAction("Index");
            }
            return View(new string[] { "Role not found" });
        }

        #region Helpers

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (string error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private RolesManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<RolesManager>(); }
        }

        private UsersManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<UsersManager>(); }
        }

        #endregion
    }
}