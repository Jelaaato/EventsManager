using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using EventsManager.IdentityModel;
using Microsoft.Owin.Security.Cookies;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;
using Microsoft.AspNet.Identity.EntityFramework;

[assembly: OwinStartup(typeof(EventsManager.IdentityConfig))]

namespace EventsManager
{
    public class IdentityConfig
    {
        public void Configuration(IAppBuilder app)
        {
            app.CreatePerOwinContext<EventsIdentityDb>(EventsIdentityDb.Create);
            app.CreatePerOwinContext<UsersManager>(UsersManager.Create);
            app.CreatePerOwinContext<RolesManager>(RolesManager.Create);
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                ExpireTimeSpan = TimeSpan.FromMinutes(60),
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Index"),
            });
        }

        //public class EmailService : IIdentityMessageService
        //{
        //    public Task SendAsync(IdentityMessage message)
        //    {

        //        var username = "root";
        //        var pass = "7p$mv3r@";

        //        var sender = "amalga@asianhospital.com";
        //        System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("itworkssmtp.asianhospital.com");
        //        client.Port = 25;
        //        client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //        client.UseDefaultCredentials = false;

        //        System.Net.NetworkCredential cred = new System.Net.NetworkCredential(username, pass);

        //        //client.EnableSsl = true;
        //        client.Credentials = cred;

        //        var mail = new System.Net.Mail.MailMessage(sender, message.Destination);

        //        mail.Subject = message.Subject;
        //        mail.IsBodyHtml = true;
        //        mail.Body = message.Body;

        //        return client.SendMailAsync(mail);

        //    }
        //}


    }
}

