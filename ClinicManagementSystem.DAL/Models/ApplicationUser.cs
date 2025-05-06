using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool isDeleted { get; set; }
    }
}
