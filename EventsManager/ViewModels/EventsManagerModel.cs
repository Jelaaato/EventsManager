using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventsManager.ViewModels
{
    public class EventsManagerModel
    {
        public class CreateEventModel
        {
            public DateTime DateCreated { get; set; }
            public int passcode { get; set; }
            [Required]
            public string event_name { get; set; }
            [Required]
            public string event_location { get; set; }
            [Required]
            public DateTime event_startdate { get; set; }
            [Required]
            public DateTime event_enddate { get; set; }
            [Required]
            public bool hasRaffle { get; set; }
            [Required]
            public bool registration_req { get; set; }
            [Required]
            public RegistrationType registration_type { get; set; }
            public HttpPostedFileBase img { get; set; }
            //public HttpPostedFileBase event_banner { get; set; }
            public Guid created_by { get; set; }
        }

        public enum RegistrationType
        { 
            Manual = 1,
            QRCode = 2,
            Both = 3
        }
        public enum Classification
        { 
            Employee = 1,
            Doctor = 2,
            Outsourced = 3,
            Consultant = 4,
            OnCall = 5,
            Others = 6
        }

        public class ParticipantManualModel
        {
            public Classification classification { get; set; }
            public string others { get; set; }
            public string employee_number { get; set; }
            public string last_name { get; set; }
            public string first_name { get; set; }
            public string middle_name { get; set; }
            public string position { get; set; }
            public string department { get; set; }
        }
    }
}