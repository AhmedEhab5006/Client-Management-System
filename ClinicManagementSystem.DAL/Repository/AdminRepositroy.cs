using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class AdminRepositroy
    {
        private readonly ProgramContext _context;

        public AdminRepositroy(ProgramContext context)
        {
            _context = context;
        }

        public bool CheckUserExistsByEmail(string email)
        {
            var check = _context.ApplicationUsers.Where(e => e.email == email).ToList();
            if (check.Count != 0)
            {
                return true;
            }
            return false;
        }

        public void AddingDoc<T>(T user)
        {
            _context.Add(user);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() > 0;
        }
        
        public ApplicationUser GettingUserByEmail(string email)
        {
            var docUser = _context.ApplicationUsers.Where(i => i.email == email).FirstOrDefault();
            return docUser;
        }


        //GetDocs

        public IEnumerable<ApplicationUser> GetUsers()
        {
            return _context.ApplicationUsers.ToList();
        }
        public IEnumerable<Doctor> GetDoctors()
        {
            return _context.Doctors.ToList();
        }
        //Editing Doctors

    }
}
