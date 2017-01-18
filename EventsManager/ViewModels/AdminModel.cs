using EventsManager.IdentityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsManager.ViewModels
{
    public class AdminModel
    {
        public class CreateUserModel
        {
            [Required]
            public string username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "password")]
            public string password { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("password", ErrorMessage = "The password and confirmation password do not match.")]
            public string confirmpassword { get; set; }
        }

        public class RoleModel
        {
            public roles role { get; set; }
            public IEnumerable<Users> members { get; set; }
            public IEnumerable<Users> nonmembers { get; set; }
        }

        public class ModifyRoleUsersModel
        {
            public string rolename { get; set; }
            public string[] idstoadd { get; set; }
            public string[] idstodelete { get; set; }
        }
    }
}