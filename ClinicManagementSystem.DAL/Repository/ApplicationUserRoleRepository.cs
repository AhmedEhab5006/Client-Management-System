using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class ApplicationUserRoleRepository : IApplicationUserRoleRepository
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<ApplicationUserRole> _roleManager;

        public ApplicationUserRoleRepository(UserManager<ApplicationUser> userManager , RoleManager<ApplicationUserRole> roleManager) {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task<string> Add(string role , ApplicationUser user)
        {
            var found = await _roleManager.FindByNameAsync(role);
            if (found == null)
            {
                await _roleManager.CreateAsync(new ApplicationUserRole
                {
                    Name = role,
                    NormalizedName = role.ToUpper()
                });
            }
            
            var result = await _userManager.AddToRoleAsync(user , role);
            if (result.Succeeded)
            {
                return "done";
            }
            return null;
        }

        public Task<string> Delete(IdentityRole role)
        {
            throw new NotImplementedException();
        }

        public Task<string> Update(IdentityRole role)
        {
            throw new NotImplementedException();
        }
    }
}
