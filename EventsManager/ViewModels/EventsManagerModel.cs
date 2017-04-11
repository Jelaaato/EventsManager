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

        public class ParticipantsModel
        {
            public HttpPostedFileBase file { get; set; }
            public Classification _classification { get; set; }

            public IEnumerable<MyParticipantsModel> participants { get; set; }
        }

        public class MyParticipantsModel
        {
            [Required(ErrorMessage="Classification cannot be left empty.")]
            public Classification _classification { get; set; }
            public System.Guid participant_id { get; set; }
            public Nullable<System.Guid> event_id { get; set; }
            //[Required]
            public string employee_number { get; set; }
            [Required(ErrorMessage="Last Name cannot be left empty.")]
            public string last_name { get; set; }
            [Required(ErrorMessage="First Name cannot be left empty.")]
            public string first_name { get; set; }
            public string middle_name { get; set; }
            public string department_name { get; set; }
            public string position_name { get; set; }
            public string display_name { get; set; }
            public short registration_type { get; set; }
            public Nullable<short> classification { get; set; }
            public Nullable<DateTime> registered_datetime { get; set; }

            public int reg_count { get; set; }
            //[Required]
            public string others { get; set; }
        }

        public class MyEventsModel
        {
            public bool isSelected { get; set; }
            public HttpPostedFileBase img { get; set; }
            public string search { get; set; }

            public System.Guid event_id { get; set; }
            [Required]
            public string event_name { get; set; }
            [Required]
            public string event_location { get; set; }
            [Required]
            public System.DateTime start_datetime { get; set; }
            [Required]
            public System.DateTime end_datetime { get; set; }
            public byte[] event_banner { get; set; }
            public bool delete_flag { get; set; }
            public Nullable<System.DateTime> deleted_datetime { get; set; }
            public System.DateTime date_created { get; set; }
            public int passcode { get; set; }
            public string email_address { get; set; }
            public bool hasRaffle { get; set; }
            public bool registration_req { get; set; }
            public Nullable<int> participant_count { get; set; }
            public Nullable<bool> closed_flag { get; set; }
            public Nullable<System.DateTime> closed_datetime { get; set; }
            public Nullable<bool> reopen_flag { get; set; }
            public Nullable<System.DateTime> reopen_datetime { get; set; }
            [Required]
            public RegistrationType registration_type { get; set; }
            public string created_by { get; set; }
        }
    }
}