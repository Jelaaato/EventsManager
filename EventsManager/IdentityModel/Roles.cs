﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventsManager.IdentityModel
{
    public class roles : IdentityRole
    {
        public roles() : base() { }
        public roles(string name) : base(name) { }
    }
}