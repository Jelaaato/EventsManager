﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsManager.ViewModels
{
    public class AccountModel
    {
        public class LoginModel
        {
            [Required]
            public string username { get; set; }
            [Required]
            public string password { get; set; }
        }

        public class ChangePasswordModel
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "current password")]
            public string oldpassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "new password")]
            public string newpassword { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "confirm new password")]
            [Compare("newpassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string confirmpassword { get; set; }
        }
    }
}