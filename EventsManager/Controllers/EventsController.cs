using EventsManager.IdentityModel;
using EventsManager.Methods;
using EventsManager.Models;
using EventsManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EventsManager.Controllers
{
    public class EventsController : Controller
    {
        // GET: Events
        private EventsOrganizerEntities db = new EventsOrganizerEntities();
        private EventsIdentityDb idb = new EventsIdentityDb();

        [Authorize]
        public ActionResult CreateEvent()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult CreateEvent(EventsManagerModel.CreateEventModel model)
        {
            if (ModelState.IsValid)
            {
                Event _event = new Event()
                {
                    event_id = Guid.NewGuid(),
                    event_name = model.event_name,
                    event_location = model.event_location,
                    start_datetime = model.event_startdate,
                    end_datetime = model.event_enddate,
                    passcode = Events.GeneratePasscode(),
                    delete_flag = false,
                    deleted_datetime = null,
                    date_created = DateTime.Today,
                    hasRaffle = model.hasRaffle,
                    registration_req = model.registration_req,
                    closed_flag = false,
                    closed_datetime = null,
                    reopen_flag = false,
                    reopen_datetime = null,
                    registration_type = (short)model.registration_type,
                    participant_count = 0,
                    created_by = Events.CreatedBy()
                };

                db.Events.Add(_event);

                db.SaveChanges();

                Session["event_id"] = Events.GetEventId(model.event_name);
                Guid event_id = new Guid(Session["event_id"].ToString());

                if (model.img != null)
                {
                    Image convertToImg = Image.FromStream(model.img.InputStream);
                    if (convertToImg.Width == 1416 && convertToImg.Height == 446)
                    {

                        byte[] imgdata = Events.ConvertToStream(convertToImg);
                        Event eventimg = db.Events.First(a => a.event_id == event_id);
                        {
                            eventimg.event_banner = imgdata;
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image should have a resolution of 1416 x 446");
                        return View(model);
                    }
                }
                return RedirectToAction("AddParticipants");
            }
            return View(model);  
        }

        [Authorize]
        public ActionResult AddParticipants()
        {
            return View();
        }
        [Authorize]
        public ActionResult AddParticipantManually()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddParticipantManually(EventsManagerModel.ParticipantManualModel model)
        {
            return View();
        }

        [Authorize]
        public ActionResult ManageEvents()
        {
            var currentuser = HttpContext.User.Identity.Name;
            var currentuserId = idb.Users.Where(a => a.UserName == currentuser).Select(a => a.Id).First();

            var events = db.Events.Where(a => a.created_by == currentuserId).Select(a => a.event_name).ToList();

            return View(events);
        }

        //[HttpPost]
        //public ActionResult AddParticipants()
        //{
        //    return View();
        //}
    }
}