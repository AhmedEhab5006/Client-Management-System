using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClinicManagementSystem.DAL.Models;
using ClinicManagementSystem.DAL.Database;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementSystem.DAL.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ProgramContext _context;
        public ReservationRepository(ProgramContext context)
        {
            _context = context;
        }
        public void AddReservation(Reservation reservation)
        {
            _context.Reservations.Add(reservation);
            _context.SaveChanges();
        }
        public void DeleteReservation(Reservation reservation)
        {
            _context.Reservations.Remove(reservation);
            _context.SaveChanges();
        }
        public void UpdateReservation(Reservation reservation)
        {
            _context.Reservations.Update(reservation);
            _context.SaveChanges();
        }
        public Reservation GetById(int id)
        {
            return _context.Reservations
                .Include(r => r.appointment)
                .ThenInclude(a => a.doctor)
                .ThenInclude(d => d.user)
                .FirstOrDefault(r => r.id == id);
        }
        public IQueryable<Reservation> GetByPatientId(int patientId)
        {
            return _context.Reservations
                .Include(r => r.appointment)
                .ThenInclude(a => a.doctor)
                .ThenInclude(d => d.user)
                .Where(r => r.patientId == patientId);
        }
    }
}
