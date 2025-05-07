using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Reservation
    {
        public int id { get; set; }
        public int appointmentId { get; set; }
        public DoctorAppointment? appointment { get; set; }
        public int patientId { get; set; }
        public Patient? patient { get; set; }
        public string status { get; set; }
    }
}
