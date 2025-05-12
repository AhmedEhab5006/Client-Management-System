using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Patient
    {
        public int userId { get; set; }
        public ApplicationUser? user { get; set; }
        public int approvedAppointments { get; set; }
        public int pendingAppointments { get; set; }
        public int rejectedAppointments { get; set; }
        public ICollection<Reservation>? reservations { get; set; }
        public ICollection<DoctorPatient>? DoctorPatients { get; set; } = new List<DoctorPatient>();
        public ICollection<MedicalHistory>? medicalHistory { get; set; }



    }
}
