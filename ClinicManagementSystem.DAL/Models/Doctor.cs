using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Doctor
    {
        public int userId { get; set; }
        public ApplicationUser? user { get; set; }
        public string major { get; set; }
        public string location { get; set; }
        public ICollection<DoctorAppointment>? appointments { get; set; }
        public ICollection<Patient>? patients { get; set; }
    }
}
