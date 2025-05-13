using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string text { get; set; }
        public DateTime date { get; set; }
        public int senderId { get; set; }
        public int recieverId { get; set; }
    }
}
