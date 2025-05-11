using ClinicManagementSystem.DAL.Database;
using ClinicManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClinicManagementSystem.DAL.Repository
{
    public class ChatRoomRepository : IChatRoomRepository
    {
        private ProgramContext _context;

        public ChatRoomRepository(ProgramContext context) {
            _context = context;
        }
        
        public void AddRoom(ChatRoom room)
        {
            _context.ChatRooms.Add(room);
            _context.SaveChanges();
        }

        public async Task<List<string>> GetAllRoomNamesAsync()
        {
            return await _context.ChatRooms.Select(r => r.Name).ToListAsync();
        }

        public async Task<bool> RoomExistsAsync(string roomName)
        {
            return await _context.ChatRooms.AnyAsync(r => r.Name == roomName);
        }
    }
}
