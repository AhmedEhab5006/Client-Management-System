using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.PatientDto_s
{
    public class AppointmentRescheduleDto
    {
        public int ReservationId { get; set; }
        public int NewDoctorAppointmentId { get; set; }
    }
}
