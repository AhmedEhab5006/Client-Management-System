using ClinicManagementSystem.BLL.Dto_s.ChatRoomDto;
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
        private IChatRoomRepository _chatRoomRepository;

        public DoctorManager (IPatientRepository patientRepository , IAppointmentRepository appointmentRepository , IMedicalHistoryRepository medicalHistoryRepository , IChatRoomRepository chatRoomRepository)
        {
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _medicalHistoryRepository = medicalHistoryRepository;
            _chatRoomRepository = chatRoomRepository;
        }

        public void AddChatRoom(ChatRoomAddDto chatRoomAddDto)
        {
            _chatRoomRepository.AddRoom(new ChatRoom { 
                doctorId = chatRoomAddDto.doctorId,
                Name = chatRoomAddDto.Name,
            });

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

        public IEnumerable<MedicalHistoryGetDto> GetFullMedicalHistory(int patientId)
        {
            var foundModel = _medicalHistoryRepository.GetFullHistory(patientId).ToList();

            if (foundModel != null)
            {
                var found = foundModel.Select(a => new MedicalHistoryGetDto
                {
                    Description = a.describtion,
                    id = a.id,
                    patientId = patientId,
                    Note = a.note,
                    DoctorName = a.doctor.user.firstName + " " + a.doctor.user.lastName
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
                    id = a.userId,
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
