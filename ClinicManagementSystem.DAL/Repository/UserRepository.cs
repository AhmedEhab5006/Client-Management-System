using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class UserRepository : IUserRepository
    {
        private ProgramContext _context;

        public UserRepository(ProgramContext context)
        {
            _context = context;
        }

        public async Task<int> AddAsync(ApplicationUser user)
        {
            
            await _context.ApplicationUsers.AddAsync(user);
            var done = await _context.SaveChangesAsync();
            
            if (done > 0)
            {
                return user.id;
            }

            return 0;
        }


        public async Task<string> DeleteAsync(ApplicationUser user)
        {
            _context.ApplicationUsers.Remove(user);
            
            var done =  await _context.SaveChangesAsync();
            
            if (done > 0)
            {
                return "done";
            }
            return null;
        }

        public IQueryable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser GetByEmail(string email)
        {
            var found = _context.ApplicationUsers.Where(a=>a.email == email).FirstOrDefault();
            
            if (found != null)
            {
                return found;
            }
            return null;
        }

        public ApplicationUser GetById(int userId)
        {
            var found = _context.ApplicationUsers.Find(userId);

            if (found != null)
            {
                return found;
            }
            return null;
        }

        public ApplicationUser GetByUserName(string username)
        {
            var found = _context.ApplicationUsers.Where(a=>a.userName == username).FirstOrDefault();
           
            if (found != null)
            {
                return found;
            }
            return null;
        }

        public byte[] GetPassword(string email)
        {
            var found = _context.ApplicationUsers.Where(a => a.email == email).Select(a => a.password).FirstOrDefault();
            if (found != null)
            {
                return found;
            }

            else
            {
                return null;
            }
        }

        public async Task<string> UpdateAsync(ApplicationUser user)
        {
            _context.Update(user);
            var done = await _context.SaveChangesAsync();
            
            if (done > 0) 
            {
                return "done";
            }
            return null;
        }
    }
}
