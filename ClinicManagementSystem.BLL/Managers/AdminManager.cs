using ClinicManagementSystem.BLL.Dto_s.AdminDTO;
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
    public class AdminManager : IAdminManager
    {
        private IDoctorRepository _doctorRepository;
        private IPasswordHandlerManager _passwordHadnlerManager;
        private IReservationRepository _reservationRepository;
        private IPatientRepository _patientRepository;
        private IAppointmentRepository _appointmentRepository;

        public AdminManager(IDoctorRepository doctorRepository , IPasswordHandlerManager passwordHandlerManager , IReservationRepository reservationRepository , IPatientRepository patientRepository , IAppointmentRepository  appointmentRepository) {
            _doctorRepository = doctorRepository;
            _passwordHadnlerManager = passwordHandlerManager;
            _reservationRepository = reservationRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
        }

        public IEnumerable<PendingReadDto> GetAllApprovedAppointments()
        {
            var foundModel = _reservationRepository.GetAllApprovedAppointments().ToList();

            if (foundModel != null)
            {
                var found = foundModel.Select(a => new PendingReadDto
                {
                    date = a.appointment.date,
                    doctorName = a.appointment.doctor.user.firstName + " " + a.appointment.doctor.user.lastName,
                    id = a.id,
                    patientName = a.patient.user.firstName + " " + a.patient.user.lastName,
                    time = a.appointment.appointmentStart
                }).ToList();

                return found;
            }

            return null;
        }

        public IEnumerable<PatientGetDto> GetAllPatients()
        {
            var foundModel = _patientRepository.GetAll().ToList();

            if (foundModel != null)
            {
                var found = foundModel.Select(a=> new PatientGetDto
                {
                    email = a.user.email,
                    id = a.user.id,
                    phone = a.user.phoneNumber,
                    username = a.user.userName
                }).ToList();

                return (found);
            }

            return null;
        }

        public IEnumerable<PendingReadDto> GetAllPendingAppointments()
        {
            var foundModel = _reservationRepository.GetAllPendingAppointments().ToList();

            if (foundModel != null)
            {
                var found = foundModel.Select(a => new PendingReadDto
                {
                    date = a.appointment.date,
                    doctorName = a.appointment.doctor.user.firstName + " " + a.appointment.doctor.user.lastName,
                    id = a.id,
                    patientName = a.patient.user.firstName + " " + a.patient.user.lastName,
                    time = a.appointment.appointmentStart
                }).ToList();

                return found;
            }

            return null;

        }

        public DoctorReadDto GetDocById(int id)
        {
            var foundModel = _doctorRepository.GetById(id);
            if (foundModel != null)
            {
                var foundPassword = _passwordHadnlerManager.DecodePasswordFromByteArray(foundModel.password);
                var found = new DoctorReadDto { 
                    Id = foundModel.id,
                    firstName = foundModel.firstName,
                    lastName = foundModel.lastName,
                    userName = foundModel.userName,
                    email = foundModel.email,
                    location = foundModel.doctor.location,
                    major = foundModel.doctor.major,
                    phoneNumber = foundModel.phoneNumber,
                    password = foundPassword,
                };

                return found;
            }

            return null;
        }

        public IEnumerable<ReservationReadDto> GetSame(int id)
        {
            var foundModel = _reservationRepository.GetSimilar(id).ToList();
            if (foundModel != null)
            {
                var found = foundModel.Select(a => new ReservationReadDto
                {
                    start = a.appointment.appointmentStart,
                    date = a.appointment.date,
                    docId = a.appointment.doctorId,
                    status = a.appointment.status
                }).ToList();

                return found;
            }

            return null;
        }

        public IEnumerable<AppointmentReadDto> GetDoctorAppointment(int doctorId)
        {
            var foundModel = _appointmentRepository.Get(doctorId).Where(a => a.status == "Available").ToList();


            if (foundModel != null)
            {
                var found = foundModel.Select(a => new AppointmentReadDto
                {
                    status = a.status,
                    appointmentStart = a.appointmentStart,
                    appointmentEnd = a.appointmentEnd,
                    date = a.date,
                    duration = a.duration,
                    id = a.Id
                }).ToList();

                return found;
            }

            return null;

        }

    }
}
