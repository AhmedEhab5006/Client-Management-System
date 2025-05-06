using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Patient : ApplicationUser
    {
        public string bookedAppointment { get; set; }
        public Guid doctorId { get; set; }
        public Doctor? doctor { get; set; }
        public int appointmentId { get; set; }
        public Appointment? appointment { get; set; }
        public ICollection<MedicalHistory>? medicalHistory { get; set; }
        public bool isDeleted { get; set; }
    }
}
