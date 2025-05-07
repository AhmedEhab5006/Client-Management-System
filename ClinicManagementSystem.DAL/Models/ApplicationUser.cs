using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class ApplicationUser
    {
        public int id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public byte [] password { get; set; }
        public string role { get; set; }
        public string phoneNumber { get; set; }
        public Doctor? doctor { get; set; }
        public Patient? patient { get; set; }

    }
}
