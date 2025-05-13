using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [InverseProperty(nameof(Message.senderId))]
        public ICollection<Message> SentMessages { get; set; }
        [InverseProperty(nameof(Message.recieverId))]
        public ICollection<Message> RecievedMessages { get; set; }

    }
}
