using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.PatientDto_s
{
    public class AppointmentGetDto
    {
        public int ReservationId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentStart { get; set; }
        public TimeOnly AppointmentEnd { get; set; }
        public string Status { get; set; }
        public string DoctorName { get; set; }
        public string DoctorSpecialization { get; set; }
        public string DoctorLocation { get; set; }
        public int docId { get; set; }

    }
}
