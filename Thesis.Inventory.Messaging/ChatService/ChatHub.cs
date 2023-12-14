using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thesis.Inventory.Domain.Entities;
using Thesis.Inventory.Infrastructure.UnitOfWork;
using Thesis.Inventory.Messaging.ChatHChatServiceub;

namespace Thesis.Inventory.Messaging.ChatService
{
    public sealed class ChatHub : Hub, IChatHub
    {
        private readonly IThesisUnitOfWork _unitOfWork;
        public ChatHub(IThesisUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public override async Task OnConnectedAsync()
        {
            var val = GetName();

            // Save the connection Id
            var user = await this._unitOfWork.Users.Entities.Where(x => x.Username == val).FirstOrDefaultAsync();
            if (user != null)
            {
                user.MessageConnectionId = Context.ConnectionId;
            }
            await _unitOfWork.Save();

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveMessage", Context.ConnectionId, $"{val}");
        }
        public async Task SendMessage(string targetUser, string message)
        {
            var val = GetName();
            var connectionId = "";
            var receiver = await this._unitOfWork.Users.Entities.Where(x=> x.Username == targetUser).FirstOrDefaultAsync();

            if(receiver != null)
            {
                connectionId = receiver.MessageConnectionId;
            }
            else
            {

            }

            var senderConenctionID = "";
            var sender = await this._unitOfWork.Users.Entities.Where(x => x.Username == val).FirstOrDefaultAsync();

            if(sender != null)
            {
                senderConenctionID = sender.MessageConnectionId;
            }
            
            var roomName = $"{targetUser}";
            await Groups.AddToGroupAsync(receiver.MessageConnectionId, roomName);
            await Groups.AddToGroupAsync(sender.MessageConnectionId, roomName);

            // add chat room in db
            var isAny = await this._unitOfWork.ChatRooms.Entities.AnyAsync(x => x.Name == roomName);
            if(!isAny)
            {
                var newroom = new ChatRoomEntity
                {
                    DateCreated = DateTime.Now,
                    Name = roomName,
                };
                await this._unitOfWork.ChatRooms.AddAsync(newroom);
               
                await this._unitOfWork.Save();


                var newmessage = new ChatMessageEntity
                {
                    ChatRoomEntityId = newroom.Id,
                    DateCreated = DateTime.Now,
                    Message = message
                };
                await this._unitOfWork.ChatRoomMessages.AddAsync(newmessage);
                await this._unitOfWork.Save();
            }
            else
            {
                var chatroom = await _unitOfWork.ChatRooms.Entities.Where(x=> x.Name ==roomName).FirstAsync();

                var newmessage = new ChatMessageEntity
                {
                    ChatRoomEntityId = chatroom.Id,
                    DateCreated = DateTime.Now,
                    Message = message
                };
                await this._unitOfWork.ChatRoomMessages.AddAsync(newmessage);
                await this._unitOfWork.Save();
            }
            await Clients.Groups(roomName).SendAsync("ReceiveMessage", val, message);

        }
        private string GetName()
        {
            var token = Context.GetHttpContext().Request.Query["access_token"];

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token);
            var tokenS = jsonToken as JwtSecurityToken;
            var val = tokenS.Claims.First(claim => claim.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name").Value;

            return val;
        }
        public async Task ReceiveMessage(string message)
        {
            await Task.CompletedTask;
        }

        public async Task SendMessageToAdmin(string message)
        {
            var val = GetName();
            var connectionId = "";

            var admins = await this._unitOfWork.Users.Entities.Where(x=> x.Role!= Shared.Enums.UserRoleType.Consumer).ToListAsync();

            var senderConenctionID = "";
            var sender = await this._unitOfWork.Users.Entities.Where(x => x.Username == val).FirstOrDefaultAsync();

            if (sender != null)
            {
                senderConenctionID = sender.MessageConnectionId;
            }
            var roomName = $"{sender.Username}";

            admins.ForEach(async x => await Groups.AddToGroupAsync(x.MessageConnectionId??"", roomName));
            
            await Groups.AddToGroupAsync(sender.MessageConnectionId, roomName);

            // add chat room in db
            var isAny = await this._unitOfWork.ChatRooms.Entities.AnyAsync(x => x.Name == roomName);
            if (!isAny)
            {
                var newroom = new ChatRoomEntity
                {
                    DateCreated = DateTime.Now,
                    Name = roomName,
                };
                await this._unitOfWork.ChatRooms.AddAsync(newroom);

                await this._unitOfWork.Save();


                var newmessage = new ChatMessageEntity
                {
                    ChatRoomEntityId = newroom.Id,
                    DateCreated = DateTime.Now,
                    Message = message
                };
                await this._unitOfWork.ChatRoomMessages.AddAsync(newmessage);
                await this._unitOfWork.Save();
            }
            else
            {
                var chatroom = await _unitOfWork.ChatRooms.Entities.Where(x => x.Name == roomName).FirstAsync();

                var newmessage = new ChatMessageEntity
                {
                    ChatRoomEntityId = chatroom.Id,
                    DateCreated = DateTime.Now,
                    Message = message
                };
                await this._unitOfWork.ChatRoomMessages.AddAsync(newmessage);
                await this._unitOfWork.Save();
            }
            await Clients.Groups(roomName).SendAsync("ReceiveMessage", val, message);
        }
    }
}
