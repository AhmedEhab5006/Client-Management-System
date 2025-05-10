using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Repository;
using ClinicManagementSystem.BLL.Dto_s;
using ClinicManagementSystem.BLL.Dto_s.PatientDto_s;

namespace ClinicManagementSystem.BLL.Managers
{
    public class PatientManager : IPatientManager
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IAppointmentRepository _appointmentRepository;

        public PatientManager(IReservationRepository reservationRepo, IAppointmentRepository appointmentRepo)
        {
            _appointmentRepository = appointmentRepo;
            _reservationRepository = reservationRepo;
        }

        public bool BookAppointment(int patientId, AppointmentBookingDto dto)
        {
            var appointment = _appointmentRepository.GetById(dto.AppointmentId);
            if(appointment == null || appointment.status == "Booked")
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
            _reservationRepository.DeleteReservation(reservation);
            return true;
        }

        public bool RescheduleAppointment(AppointmentRescheduleDto dto, int patientId)
        {
            var reservation = _reservationRepository.GetById(dto.ReservationId);
            if (reservation == null || reservation.patientId != patientId)
                return false;
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
    }
}
