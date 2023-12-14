using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thesis.Inventory.MobileApp.Configurations;
using Thesis.Inventory.MobileApp.Extensions;
using Thesis.Inventory.MobileApp.Model;
using Thesis.Inventory.Shared.Models;

namespace Thesis.Inventory.MobileApp.ViewModel
{
    public partial class ChatViewModel : ObservableObject
    {
        private readonly HttpClient _httpClient;
        private HubConnection _hubConnection;
        public ChatViewModel(HttpClient httpClient, APIConfiguration configuration)
        {
            _httpClient = httpClient;
            this.ConnectToChat(configuration);
            this.GetAllOldMessages(configuration.AdminUserName);
        }

        [ObservableProperty]
        string myUserId;

        [ObservableProperty]
        string myUsername;

        [ObservableProperty]
        string adminId;

        [ObservableProperty]
        string message;

        [ObservableProperty]
        ObservableCollection<ChatModel> messages = new ObservableCollection<ChatModel>();

        [ObservableProperty]
        string chat;

        [ObservableProperty]
        string connectionId;

        private async void GetAllOldMessages(string adminusername)
        {
            var myusername = await SecureStorage.GetAsync("username");
            var response = await _httpClient.GetAsync<ChatRoomMessageModel[]>($"Chat/GetMessages?user={myusername}");
            if (response.Succeeded)
            {
                response.Data.ToList().ForEach(x =>
                {
                    var msg = JsonSerializer.Deserialize<ChatModel>(x.Message);
                    msg.isYou = msg.User == this.MyUsername;

                    this.Messages.Add(msg);
                }
                );
            }
            else
            {

            }
        }
        private async void ConnectToChat(APIConfiguration configuration)
        {
            this.MyUsername = await SecureStorage.GetAsync("username");
            var token = await SecureStorage.GetAsync("token");
            _hubConnection = new HubConnectionBuilder().WithUrl($"{configuration.HuBConnection}?access_token={token}").Build();
            _hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var model = JsonSerializer.Deserialize<ChatModel>(message);
                model.isYou = model.User == this.MyUsername;
                this.Messages.Add(model);
                this.Message += $"\n{message}";
                this.ConnectionId = user;
            });

            await _hubConnection.StartAsync();
        }

        [RelayCommand]
        async Task SendMessage()
        {
            if(this.Chat == string.Empty)
            {
                return;
            }
            var chatmodel = new ChatModel
            {
                Message = this.Chat,
                User = this.MyUsername,
            };

            this.Chat = string.Empty;
            await this._hubConnection.SendAsync("SendMessageToAdmin", JsonSerializer.Serialize(chatmodel));

        }
    }
}
