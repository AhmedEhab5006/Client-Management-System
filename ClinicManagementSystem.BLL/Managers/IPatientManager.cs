using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using ClinicManagementSystem.BLL.Dto_s.DocDto;



namespace ClinicManagementSystem.BLL.Managers
{
    public interface IPatientManager
    {
        bool BookAppointment(int patientId, AppointmentBookingDto dto);
        bool CancelAppointment(AppointmentCancelDto dto, int patientId);
        bool RescheduleAppointment(AppointmentRescheduleDto dto, int patientId);
        IEnumerable<AppointmentGetDto> GetMyAppointments(int patientId);
        IEnumerable<MedicalHistoryGetDto> GetMyMedicalHistory(int patientId);
        public IEnumerable<DoctorsReadDto> GetAllDoctors();
        public IEnumerable<AppointmentReadDto> GetDoctorAppointment(int doctorId);
    }
}
