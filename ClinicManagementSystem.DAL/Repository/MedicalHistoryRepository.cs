using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class MedicalHistoryRepository : IMedicalHistoryRepository
    {
        private ProgramContext _context;

        public MedicalHistoryRepository(ProgramContext context) {
            _context = context;
        }

        public void AddMedicalHistory(MedicalHistory medicalHistory)
        {
            _context.MedicalHistories.Add(medicalHistory);
            _context.SaveChanges();
        }

        public IQueryable<MedicalHistory> GetFullHistory(int patientId)
        {
            var found = _context.MedicalHistories
                .Include(h => h.doctor)
                .ThenInclude(d => d.user)
                .Where(h => h.patientId == patientId);
                 return found;
        }
    }
}
