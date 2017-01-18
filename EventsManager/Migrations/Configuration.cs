namespace EventsManager.Migrations
{
    using EventsManager.IdentityModel;
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<EventsManager.IdentityModel.EventsIdentityDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(EventsManager.IdentityModel.EventsIdentityDb context)
        {
            var hashPassword = new PasswordHasher();
            string pass = hashPassword.HashPassword("@Pp$d3v");
            context.Users.AddOrUpdate(u => u.UserName, new Users
            {
                UserName = "@dministrator",
                PasswordHash = pass
            });
        }
    }
}
