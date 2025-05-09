using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.DocDto
{
    public class MedicalHistoryAddDto
    {
        [Required(ErrorMessage = "Missing patient id")]
        public int patientId { get; set; }
        [Required(ErrorMessage = "Missing note")]
        public string note { get; set; }
        [Required(ErrorMessage = "Missing describtion")]
        public string describtion { get; set; }
        [Required(ErrorMessage = "Missing doctor id")]
        public int doctorId { get; set; }
    }
}
