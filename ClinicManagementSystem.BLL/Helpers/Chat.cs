using ClinicManagementSystem.BLL.Helpers;
using ClinicManagementSystem.DAL.Repository;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Chat : Hub
{
    private readonly SharedDb _sharedDb;
    private readonly IChatRoomRepository _chatRoomService;
    private const int MaxUsersPerRoom = 2;

    public Chat(SharedDb sharedDb, IChatRoomRepository chatRoomService)
    {
        _sharedDb = sharedDb;
        _chatRoomService = chatRoomService;
    }

    public override async Task OnConnectedAsync()
    {
        // Do not join default room directly anymore
        await base.OnConnectedAsync();
    }

    //public override async Task OnDisconnectedAsync(Exception? exception)
    //{
    //    if (_sharedDb.Connection.TryGetValue(Context.ConnectionId, out var connection))
    //    {
    //        await Clients.Group(connection.Room).SendAsync("ReceiveMessage", "admin", $"{connection.User} left {connection.Room}");
    //        _sharedDb.Connection.Remove(Context.ConnectionId);
    //    }

    //    await base.OnDisconnectedAsync(exception);
    //}

    public async Task JoinChatRoom(string userName, string chatRoom)
    {
        // Check room validity from database
        if (!await _chatRoomService.RoomExistsAsync(chatRoom))
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "admin", $"Room '{chatRoom}' does not exist.");
            return;
        }

        // Check number of current users in the room
        var usersInRoom = _sharedDb.Connection.Values.Count(c => c.Room == chatRoom);
        if (usersInRoom >= MaxUsersPerRoom)
        {
            await Clients.Caller.SendAsync("ReceiveMessage", "admin", $"Room '{chatRoom}' is full.");
            return;
        }

        // Join the room
        await Groups.AddToGroupAsync(Context.ConnectionId, chatRoom);
        _sharedDb.Connection[Context.ConnectionId] = new UserConnection { User = userName, Room = chatRoom };

        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", "admin", $"{userName} has joined the chat room {chatRoom}");
    }

    public async Task SendMessage(string currentUserName, string message, string chatRoom)
    {
        await Clients.Group(chatRoom).SendAsync("ReceiveMessage", currentUserName, message);
    }


}
