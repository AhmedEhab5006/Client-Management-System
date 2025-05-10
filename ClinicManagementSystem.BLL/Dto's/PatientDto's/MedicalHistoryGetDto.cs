using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.PatientDto_s
{
    public class MedicalHistoryGetDto
    {
        public int id { get; set; }
        public int patientId { get; set; }
        public string Description { get; set; }
        public string Note { get; set; }
        public string DoctorName { get; set; }
    }
}
