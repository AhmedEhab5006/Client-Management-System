using ClinicManagementSystem.BLL.Dto_s.DocDto;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Managers
{
    public class DoctorManager : IDoctorManager
    {
        private IPatientRepository _patientRepository;
        private IAppointmentRepository _appointmentRepository;
        private IMedicalHistoryRepository _medicalHistoryRepository;

        public DoctorManager (IPatientRepository patientRepository , IAppointmentRepository appointmentRepository , IMedicalHistoryRepository medicalHistoryRepository)
        {
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _medicalHistoryRepository = medicalHistoryRepository;
        }

        public bool AddMedicalHistory(MedicalHistoryAddDto addDto)
        {

            var foundPatient = _patientRepository.GetPatientById(addDto.patientId);


            if (foundPatient != null)
            {
                _medicalHistoryRepository.AddMedicalHistory(new MedicalHistory
                {
                    describtion = addDto.describtion,
                    doctorId = addDto.doctorId,
                    note = addDto.note,
                    patientId = addDto.patientId,
                });

                return true;
            }

            return false;
        }

        public IEnumerable<AppointmentReadDto> GetAppointments(int docId)
        {
            var foundModel = _appointmentRepository.Get(docId).ToList();
            if (foundModel != null)
            {
                var found = foundModel.Select(a => new AppointmentReadDto
                {
                    status = a.status,
                    appointmentStart = a.appointmentStart,
                    appointmentEnd = a.appointmentEnd,
                    date = a.date,
                    duration = a.duration

                    
                });

                return found;

            }

            return null;
        }

        public IEnumerable<MedicalHistoryReadDto> GetFullMedicalHistory(int patientId)
        {
            var foundModel = _medicalHistoryRepository.GetFullHistory(patientId).ToList();

            if (foundModel.Count() > 0)
            {
                var found = foundModel.Select(a => new MedicalHistoryReadDto
                {
                    describtion = a.describtion,
                    id = a.id,
                    patientId = patientId,
                    notes = a.note
                });

                return found;
            }

            return null;
        }

        public IEnumerable<PatientReadDto> GetPatients(int docId)
        {
            var foundModel = _patientRepository.GetSpecificDoctorPatients(docId).ToList();

            if (foundModel.Count() > 0)
            {
                var found = foundModel.Select(a => new PatientReadDto
                {
                    firstName = a.user.firstName,
                    lastName = a.user.lastName,
                    issue = a.medicalHistory.Select(a => a.describtion).LastOrDefault() ?? "There is no history for this patient",
                    notes = a.medicalHistory.Select(a => a.note).LastOrDefault() ?? "There is no history for this patient",

                    phoneNumber = a.user.phoneNumber
                });

                return found;
            }

            return null;
        }
    }
}
