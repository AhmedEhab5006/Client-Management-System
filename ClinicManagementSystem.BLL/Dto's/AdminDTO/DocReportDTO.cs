using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class DocReportDTO
    {
        public int id { get; set; }
        public string DocUserName { get; set; }
        public string major { get; set; }
        public int appointmentId { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly appointmentStart { get; set; }
        public int patientId { get; set; }
        public string PatientUserName { get; set; }
    }
}
