using ClinicManagementSystem.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public interface IChatRoomRepository
    {
        public Task<List<string>> GetAllRoomNamesAsync();
        public Task<bool> RoomExistsAsync(string roomName);
        public void AddRoom(ChatRoom room);
    }
}
