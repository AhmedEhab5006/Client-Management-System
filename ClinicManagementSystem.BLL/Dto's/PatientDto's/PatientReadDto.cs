using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.PatientDto_s
{
    public class PatientReadDto
    {
        public int id {  get; set; }
        public string firstName {  get; set; }
        public string lastName { get; set; }
        public string phoneNumber { get; set; }
        public string issue {  get; set; }
        public string notes { get; set; }

    }
}
