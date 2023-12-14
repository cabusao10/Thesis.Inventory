using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Thesis.Inventory.Messaging.ChatHChatServiceub
{
    public interface IChatHub
    {
        Task SendMessageToAdmin(string message);
        Task SendMessage(string sessionid,string message);
        Task ReceiveMessage(string message);
    }
}
