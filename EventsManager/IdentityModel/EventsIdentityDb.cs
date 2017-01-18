namespace EventsManager.IdentityModel
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class EventsIdentityDb : IdentityDbContext<Users>
    {
        public EventsIdentityDb()
            : base("EventsIdentityDb") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<IdentityUser>().ToTable("EventsUsers");
            modelBuilder.Entity<IdentityRole>().ToTable("EventsRoles");
            modelBuilder.Entity<IdentityUserRole>().ToTable("EventsUserRoles");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("EventsUserClaims");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("EventsUserLogin");
        }

        static EventsIdentityDb()
        {
            Database.SetInitializer<EventsIdentityDb>(new IdentityDbSeed());
        }

        public static EventsIdentityDb Create()
        {
            return new EventsIdentityDb();
        }
    }

    public class IdentityDbSeed : DropCreateDatabaseIfModelChanges<EventsIdentityDb>
    {
        protected override void Seed(EventsIdentityDb context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(EventsIdentityDb Context)
        {
            //UsersManager UserManager = new UsersManager(new UserStore<Users>(Context));
            //RolesManager RoleManager = new RolesManager(new RoleStore<roles>(Context));

            //string Name = "Administrator";
            //string UserName = "appAdmin";
            //string Password = "P0rt4l@dm!n";
            //string email = "elabetoria@asianhospital.com";


        }
    }
}
