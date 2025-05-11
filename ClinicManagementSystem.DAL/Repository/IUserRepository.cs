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
    public interface IUserRepository
    {
        public ApplicationUser GetById (int userId);
        public Task<string> UpdateAsync(ApplicationUser user);
        public Task<string> DeleteAsync(ApplicationUser user);
        public Task <int> AddAsync(ApplicationUser user);
        public IQueryable<ApplicationUser> GetAll();
        public ApplicationUser GetByUserName (string userName);
        public Task <ApplicationUser> GetByEmail(string email);
        public Task <byte[]> GetPassword (string email);
    }
}
