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
using PagedList;
using PagedList.Mvc;

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
            var currentuser = HttpContext.User.Identity.Name;

            if (ModelState.IsValid)
            {
                if (model.img != null)
                {
                    if (ManageEvents.isFileTypeOK(model.img))
                    {
                        Image convertToImg = Image.FromStream(model.img.InputStream);
                        if (ManageEvents.isSizeOk(convertToImg))
                        {
                            byte[] imgdata = ManageEvents.ConvertToStream(convertToImg);

                            Event _event = new Event()
                            {
                                event_id = Guid.NewGuid(),
                                event_name = model.event_name,
                                event_location = model.event_location,
                                start_datetime = model.event_startdate,
                                end_datetime = model.event_enddate,
                                passcode = ManageEvents.GeneratePasscode(),
                                event_banner = imgdata,
                                delete_flag = false,
                                deleted_datetime = null,
                                date_created = DateTime.Now,
                                hasRaffle = model.hasRaffle,
                                registration_req = model.registration_req,
                                closed_flag = false,
                                closed_datetime = null,
                                reopen_flag = false,
                                reopen_datetime = null,
                                registration_type = (short)model.registration_type,
                                participant_count = 0,
                                created_by = ManageEvents.CreatedByLoginID(currentuser)
                            };

                            db.Events.Add(_event);
                            db.SaveChanges();

                            Session["event_id"] = ManageEvents.GetEventId(model.event_name);

                            return RedirectToAction("AddParticipants", "Participants");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Image should have a resolution of 1416px x 446px");
                            return View(model);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image should be .jpeg or .png only");
                        return View(model);
                    }
                }
                else
                {
                    Event _event = new Event()
                    {
                        event_id = Guid.NewGuid(),
                        event_name = model.event_name,
                        event_location = model.event_location,
                        start_datetime = model.event_startdate,
                        end_datetime = model.event_enddate,
                        passcode = ManageEvents.GeneratePasscode(),
                        event_banner = null,
                        delete_flag = false,
                        deleted_datetime = null,
                        date_created = DateTime.Now,
                        hasRaffle = model.hasRaffle,
                        registration_req = model.registration_req,
                        closed_flag = false,
                        closed_datetime = null,
                        reopen_flag = false,
                        reopen_datetime = null,
                        registration_type = (short)model.registration_type,
                        participant_count = 0,
                        created_by = ManageEvents.CreatedByLoginID(currentuser)
                    };

                    db.Events.Add(_event);
                    db.SaveChanges();

                    Session["event_id"] = ManageEvents.GetEventId(model.event_name);
                    return RedirectToAction("AddParticipants", "Participants");
                }
            }
            //ModelState.Clear();
            return View(model);  
        }

        [Authorize]
        public ActionResult MyEvents(Guid? id, int? page, string search, string currentfilter)
        {
           
            var currentuser = HttpContext.User.Identity.Name;
            var currentuserId = idb.Users.Where(a => a.UserName == currentuser).Select(a => a.Id).First();
            

            if (search != null)
            {
                page = 1;
            }
            else
            {
                search = currentfilter;
            }

            ViewBag.CurrentFilter = search;

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if (!String.IsNullOrEmpty(search))
            {
                if (User.IsInRole("Administrator"))
                {
                    var events = ManageEvents.GetEvents().Where(a => a.event_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1);
                    return View(events.OrderByDescending(a => a.date_created).ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    //var events = ManageEvents.GetEvents().Where(a => a.event_name.Contains(search));
                    //return View(events.OrderByDescending(a => a.date_created).ToPagedList(pageNumber, pageSize));
                    var events = ManageEvents.GetEvents().Where(a => a.event_name.IndexOf(search, StringComparison.OrdinalIgnoreCase) != -1);
                    return View(events.OrderByDescending(a => a.date_created).ToPagedList(pageNumber, pageSize));
                }
                
            }
            else
            {
                if (User.IsInRole("Administrator"))
                {
                    var events = ManageEvents.GetEvents();
                    ViewBag.EventsCount = ManageParticipants.CountParticipants(id);
                    return View(events.OrderByDescending(a => a.date_created).ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    var events = ManageEvents.GetMyEvents(currentuserId);
                    ViewBag.EventsCount = ManageParticipants.CountParticipants(id);
                    return View(events.OrderByDescending(a => a.date_created).ToPagedList(pageNumber, pageSize));
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult MyEvents(IEnumerable<Guid?> isSelectedEvent, string action)
        {
            if (action.Equals("Edit"))
            {
                Session["selectedEventID"] = isSelectedEvent;
                return RedirectToAction("EditEvent");
            }
            else if (action.Equals("Delete"))
            {
                Session["selectedEventID"] = isSelectedEvent;
                return RedirectToAction("DeleteEvent");
            }
            else if (action.Equals("Post"))
            {
                Session["selectedEventID"] = isSelectedEvent;
                return RedirectToAction("CloseEvent");
            }
            else if (action.Equals("Participants"))
            {
                Session["selectedEventID"] = isSelectedEvent;
                return RedirectToAction("AddParticipants", "Participants");
            }
            else if (action.Equals("Reports"))
            {
                Session["selectedEventID"] = isSelectedEvent;
                return RedirectToAction("GenerateReport", "Reports");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        public ActionResult LoadThumbnail(Guid id)
        {

            var eventBanner = db.Events.Where(e => e.event_id == id).Select(e => e.event_banner).First();
            return File(eventBanner, "image/jpeg");
            
            
        }

        [Authorize]
        public ActionResult EditEvent(Guid? id)
        {
            //var idList = Session["selectedEventID"] as IEnumerable<Guid?>;
            //if (idList == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}
            //foreach (var i in idList)
            //{
            //    id = i;
            //}
            if (id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var bannerExists = ManageEvents.bannerExists(id);
                if (bannerExists)
                {
                    ViewBag.Display = "Image";
                    var event_details = ManageEvents.ViewEventDetails(id);
                    return View(event_details);
                }
                else
                {
                    ViewBag.Display = "Form";
                    var event_details = ManageEvents.ViewEventDetails(id);
                    return View(event_details);
                }
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditEvent(Guid? id, EventsManagerModel.MyEventsModel model)
        {
            //var idList = Session["selectedEventID"] as IEnumerable<Guid?>;

            //if (idList == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //foreach (var i in idList)
            //{
            //    id = i;
            //}

            if (ModelState.IsValid)
            {
                Event _event = db.Events.First(e => e.event_id == id);

                if (model.img == null)
                {
                    _event.event_name = model.event_name;
                    _event.event_location = model.event_location;
                    _event.start_datetime = model.start_datetime;
                    _event.end_datetime = model.end_datetime;
                    _event.registration_type = (short)model.registration_type;
                    _event.hasRaffle = model.hasRaffle;
                    _event.registration_req = model.registration_req;
                    db.SaveChanges();
                }
                else
                {
                    Image convertToImg = Image.FromStream(model.img.InputStream);
                    if (ManageEvents.isSizeOk(convertToImg))
                    {
                        byte[] imgdata = ManageEvents.ConvertToStream(convertToImg);

                        _event.event_name = model.event_name;
                        _event.event_location = model.event_location;
                        _event.start_datetime = model.start_datetime;
                        _event.end_datetime = model.end_datetime;
                        _event.registration_type = (short)model.registration_type;
                        _event.event_banner = imgdata;

                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Image should have a resolution of 1416px x 446px");
                        return View(model);
                    }
                }
                return RedirectToAction("MyEvents");
            }
            else
            {
                return View(model);
            }  
        }

        [Authorize]
        public ActionResult DeleteEvent()
        {
            var idList = Session["selectedEventID"] as IEnumerable<Guid?>;

            if (idList == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var id in idList)
            {
                Event _event = db.Events.First(e => e.event_id == id);
                _event.delete_flag = true;
                _event.deleted_datetime = DateTime.Now;
            }
            db.SaveChanges();

            return RedirectToAction("MyEvents");
        }

        [Authorize]
        public ActionResult CloseEvent()
        {
            var idList = Session["selectedEventID"] as IEnumerable<Guid?>;

            if (idList == null)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var id in idList)
            {
                Event _event = db.Events.First(e => e.event_id == id);
                _event.closed_flag = true;
                _event.closed_datetime = DateTime.Now;
            }
            db.SaveChanges();
            return RedirectToAction("MyEvents");
        }
    }
}