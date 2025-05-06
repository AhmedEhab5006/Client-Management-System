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
        public Task<ApplicationUser?> GetByIdAsync(string userId);
        public Task<string> UpdateAsync(ApplicationUser user);
        public Task<string> DeleteAsync(ApplicationUser user);
        public Task <string> AddAsync(string password , ApplicationUser user);
        public Task <IQueryable<ApplicationUser>> GetAllAsync();
        public Task <string> GetByUserName (string userName);
        public Task<ApplicationUser> GetByEmail(string email);
        public Task<string> CheckPassword (string password  , ApplicationUser user);
        public Task<IList<Claim>> GetClaims (ApplicationUser user);   
    }
}
