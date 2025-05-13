using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class ReservationReadDto
    {
        public int docId { get; set; }
        public TimeOnly start {  get; set; }
        public DateOnly date { get; set; }
        public string status { get; set; }
    }
}
