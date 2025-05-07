using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.BLL.Dto_s.AuthDto
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string username { get; set; }
        [Required(ErrorMessage = "Email is required")]
        public string email { get; set; }
        [Required(ErrorMessage = "Firstname is required")]
        public string firstname { get; set; }
        [Required(ErrorMessage = "Lastname is required")]
        public string lastname { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 5 , ErrorMessage = "Password must be at least from 5 chars")]
        public string password { get; set; }
        [Required(ErrorMessage = "Phonenumber is required")]
        [StringLength(12, MinimumLength = 12 , ErrorMessage = "Phonenumber must be 12 digits")]
        public string phoneNumber { get; set; }

    }
}
