using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;
using Microsoft.EntityFrameworkCore;
using ClinicManagementSystem.BLL.Dto_s.DocDto;

namespace ClinicManagementSystem.BLL.Managers
{
    public class PatientManager : IPatientManager
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMedicalHistoryRepository _medicalHistoryRepository;
        private readonly IDoctorRepository _doctorRepository;

        public PatientManager(IReservationRepository reservationRepo, IAppointmentRepository appointmentRepo, IMedicalHistoryRepository medicalHistoryRepo , IDoctorRepository doctorRepository)
        {
            _appointmentRepository = appointmentRepo;
            _reservationRepository = reservationRepo;
            _medicalHistoryRepository = medicalHistoryRepo;
            _doctorRepository = doctorRepository;
        }

        public bool BookAppointment(int patientId, AppointmentBookingDto dto)
        {
            var appointment = _appointmentRepository.GetById(dto.AppointmentId);
            if (appointment == null || appointment.status == "Booked")
                return false;
            var reservation = new Reservation
            {
                appointmentId = dto.AppointmentId,
                patientId = patientId,
                status = "Pending"
            };
            _reservationRepository.AddReservation(reservation);
            return true;
        }

        public bool CancelAppointment(AppointmentCancelDto dto, int patientId)
        {
            var reservation = _reservationRepository.GetById(dto.ReservationId);
            if(reservation == null || reservation.patientId != patientId)
                return false;
            var appointment = _appointmentRepository.GetById(reservation.appointmentId);
            appointment.status = "Available";
            _reservationRepository.DeleteReservation(reservation);
            return true;
        }

        public bool RescheduleAppointment(AppointmentRescheduleDto dto, int patientId)
        {
            var reservation = _reservationRepository.GetById(dto.ReservationId);
            if (reservation == null || reservation.patientId != patientId)
                return false;
            var appointment = _appointmentRepository.GetById(reservation.appointmentId);
            appointment.status = "Available";
            reservation.appointmentId = dto.NewDoctorAppointmentId;
            reservation.status = "Pending";
            _reservationRepository.UpdateReservation(reservation);
            return true;
        }

        public IEnumerable<AppointmentGetDto> GetMyAppointments(int patientId)
        {
            var reservations = _reservationRepository.GetByPatientId(patientId);
            var result = reservations.Select(r => new AppointmentGetDto
            {
                ReservationId = r.id,
                AppointmentDate = r.appointment.date,
                AppointmentStart = r.appointment.appointmentStart,
                AppointmentEnd = r.appointment.appointmentEnd,
                Status = r.status,
                DoctorName = r.appointment.doctor.user.firstName + " " + r.appointment.doctor.user.lastName,
                DoctorSpecialization = r.appointment.doctor.major,
                DoctorLocation = r.appointment.doctor.location
            });
            return result;
        }

        public IEnumerable<MedicalHistoryGetDto> GetMyMedicalHistory(int  patientId)
        {
            try
            {
                var history = _medicalHistoryRepository.GetFullHistory(patientId).ToList();
                if (history == null || !history.Any())
                    return Enumerable.Empty<MedicalHistoryGetDto>();
                var result = history.Select(h => new MedicalHistoryGetDto
                {
                    id = h.id,
                    patientId = h.patientId,
                    Description = h.describtion,
                    Note = h.note,
                    DoctorName = h.doctor.user.firstName + " " + h.doctor.user.lastName
                });
                return result;
            }
            catch(Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving medical history.", ex);
            }
        }

        public IEnumerable<DoctorsReadDto> GetAllDoctors()
        {
            var foundModel = _doctorRepository.GetAll().Where(a => a.doctor.appointments.Count() > 0)
                                                       .Where(a => a.doctor.appointments.FirstOrDefault().status == "Available")
                                                       .ToList(); 

            if (foundModel != null)
            {
                var found = foundModel.Select(a => new DoctorsReadDto
                {
                    major = a.doctor.major,
                    Address = a.doctor.location,
                    Id = a.id,
                    Name = a.firstName + " " + a.lastName,
                    phone = a.phoneNumber
                }).ToList();

                return found;
            }

            return null;
        }

        public IEnumerable<AppointmentReadDto> GetDoctorAppointment(int doctorId)
        {
           var foundModel = _appointmentRepository.Get(doctorId).ToList();

           
            if(foundModel != null)
            {
                var found = foundModel.Select(a=> new AppointmentReadDto
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
