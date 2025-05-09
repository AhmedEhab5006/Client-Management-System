using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.DocDto
{
    public class AppointmentReadDto
    {
        public DateOnly date { get; set; }
        public string? status { get; set; }
        public TimeOnly appointmentStart { get; set; }
        public TimeOnly appointmentEnd { get; set; }
        public TimeSpan duration { get; set; }
    }
}
