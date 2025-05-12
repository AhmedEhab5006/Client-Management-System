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
    public class DoctorRepository : IDoctorRepository
    {
        private ProgramContext _context;

        public DoctorRepository(ProgramContext context) {
            _context = context;
        }
        
        public IEnumerable<ApplicationUser> GetAll()
        {
            var found = _context.ApplicationUsers
                                    .Where(a=>a.role == "Doctor")
                                    .Include(a => a.doctor)
                                    .Include(a => a.doctor.appointments);

            return found;
        }

        public ApplicationUser GetById(int id)
        {
            var found = _context.ApplicationUsers.Where(a=>a.id == id)
                                                 .Include(a=>a.doctor)
                                                 .FirstOrDefault();

            return found;
        }
    }
}
