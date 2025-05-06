using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IApplicationUserRoleRepository
    {
        public Task<string> Add(string role , ApplicationUser user);
        public Task<string> Update(IdentityRole role);
        public Task<string> Delete(IdentityRole role);
    }
}
