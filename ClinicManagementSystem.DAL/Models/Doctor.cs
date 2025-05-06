using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Doctor : ApplicationUser
    {
        public bool firstTimeSlot {  get; set; }
        public bool secondTimeSlot { get; set; }
        public bool thirdTimeSlot {  get; set; }
        public string status { get; set; }
        public DateOnly? appointmentDate {  get; set; }
        public ICollection<Patient>? patients { get; set; }
        public int appointmentId { get; set; }
        public Appointment? appointment { get; set; }
        public bool isDeleted { get; set; }

    }
}
