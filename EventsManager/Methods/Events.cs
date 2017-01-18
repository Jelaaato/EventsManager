using EventsManager.IdentityModel;
using EventsManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace EventsManager.Methods
{
    public class Events
    {
        private static EventsOrganizerEntities db = new EventsOrganizerEntities();
        private static EventsIdentityDb idb = new EventsIdentityDb();

        private static string currentuser = HttpContext.Current.User.Identity.Name;
        private static string createdby = idb.Users.Where(a => a.UserName == currentuser).Select(a => a.Id).First();

        public static int GeneratePasscode()
        {
            Random random = new Random();

            int passcode = random.Next(1000, 9999);

            return passcode;
        }

        public static int CountParticipants(Guid event_id)
        {
            var count = db.Participants.Where(p => p.event_id == event_id).Count();

            return count;
        }

        public static string CreatedBy()
        {
            return createdby;
        }

        public static byte[] ConvertToStream(Image img)
        {
            MemoryStream mst = new MemoryStream();
            img.Save(mst, ImageFormat.Jpeg);
            return mst.ToArray();
        }

        public static Guid GetEventId(string name)
        {
            var event_id = db.Events.Where(a => a.event_name == name).Select(a => a.event_id).FirstOrDefault();

            return event_id;
        }

        public static bool CheckImageSize(Image img)
        {
            if (img.Width == 1416 && img.Height == 446)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void UpdateParticipantCount(Guid event_id)
        {
            var count = db.Participants.Where(a => a.event_id == event_id).Count();
            var currentCount = db.Events.Where(a => a.event_id == event_id).Select(a => a.participant_count).First();

            if (currentCount == 0)
            {
                Event updatecount = db.Events.First(a => a.event_id == event_id);
                updatecount.participant_count = count;
                db.SaveChanges();
            }
            else
            {
                Event updatecount = db.Events.First(a => a.event_id == event_id);
                updatecount.participant_count = currentCount + count;
                db.SaveChanges();
            }
        }
    }
}