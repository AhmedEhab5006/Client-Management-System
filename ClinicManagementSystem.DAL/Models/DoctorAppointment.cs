using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class DoctorAppointment
    {
        public int Id { get; set; }
        public int doctorId { get; set; }
        public Doctor? doctor { get; set; }
        //public TimeSpan timeSlot { get; set; }
        public DateOnly date { get; set; }
        public string? status { get; set; }
        public TimeOnly appointmentStart { get; set; }
        public TimeOnly appointmentEnd { get; set; }
        public TimeSpan duration { get; set; }
    }
}
