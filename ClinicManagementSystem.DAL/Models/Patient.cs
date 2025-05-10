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
        public Reservation? appointment { get; set; }
        public int? doctorId { get; set; }
        public Doctor? doctor { get; set; }
        public ICollection<MedicalHistory>? medicalHistory { get; set; }



    }
}
