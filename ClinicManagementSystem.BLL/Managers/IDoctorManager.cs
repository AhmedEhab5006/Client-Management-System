using ClinicManagementSystem.BLL.Dto_s.DocDto;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public interface IDoctorManager
    {
        public IEnumerable<PatientReadDto> GetPatients(int docId);
        public IEnumerable<AppointmentReadDto> GetAppointments(int docId);
        public IEnumerable<MedicalHistoryReadDto> GetFullMedicalHistory(int patientId);
        public bool AddMedicalHistory (MedicalHistoryAddDto addDto);
        
    }
}
