using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AdminDTO
{
    public class PendingReadDto
    {
        public int id { get; set; }
        public string patientName { get; set; }
        public string doctorName { get; set; }
        public TimeOnly time {  get; set; }
        public DateOnly date {  get; set; }

    }
}
