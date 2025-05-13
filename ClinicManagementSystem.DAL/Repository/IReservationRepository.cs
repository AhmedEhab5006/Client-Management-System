using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.DAL.Models;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IReservationRepository
    {
        void AddReservation(Reservation reservation);
        void DeleteReservation(Reservation reservation);
        void UpdateReservation(Reservation reservation);
        Reservation GetById(int id);
        IQueryable<Reservation> GetByPatientId(int patientId);
        public IQueryable<Reservation> GetAllPendingAppointments();
        public IQueryable<Reservation> GetAllApprovedAppointments();
        public IQueryable<Reservation> GetSimilar(int id);
    }
}
