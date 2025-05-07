using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class MedicalHistory
    {
        public int id {  get; set; }
        public int patientId { get; set; }
        public Patient? patient { get; set; }
        public string describtion { get; set; }
        public int doctorId { get; set; }
        public Doctor? doctor { get; set; }
    }
}
