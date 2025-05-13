using ClinicManagementSystem.BLL.Dto_s.AdminDTO;
using ClinicManagementSystem.BLL.Dto_s.DocDto;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public interface IAdminManager 
    {
        public DoctorReadDto GetDocById (int id);
        public IEnumerable<PendingReadDto> GetAllPendingAppointments();
        public IEnumerable<PendingReadDto> GetAllApprovedAppointments();
        public IEnumerable<PatientGetDto> GetAllPatients();
        public IEnumerable<ReservationReadDto> GetSame(int id);
        public IEnumerable<AppointmentReadDto> GetDoctorAppointment(int doctorId);
        public void EditReservation(ReservationEditDto reservationEditDto);
    }
}
