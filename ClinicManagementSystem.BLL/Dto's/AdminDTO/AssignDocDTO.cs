using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class AssignDocDTO
    {
        public int doctorId { get; set; }
        public DateOnly date { get; set; }
        public TimeOnly appointmentStart { get; set; }

    }
}
