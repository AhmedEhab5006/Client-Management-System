using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Appointment
    {
        public int id { get; set; }
        public DateTime date { get; set; }
        public Guid doctorId { get; set; }
        public Doctor? doctor { get; set; }
        public Guid patientId { get; set; }
        public Patient? patient { get; set; }
        public bool isDeleted { get; set; }
    }
}
