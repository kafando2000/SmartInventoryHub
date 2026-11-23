using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using SharedModels.Models;
namespace HubClient.Services
{
    public class PopulateCategoriesService{
        private HubConnection _hubConnection;
        public  bool isServerUp() => _hubConnection.State == HubConnectionState.Connected;

        public event Action<List<CategoryResponseDto>>? OnMessageReceived;
        public PopulateCategoriesService()
        {
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5168/populate-categories")
                .WithAutomaticReconnect()
                .Build();
            // this establishes the connection with the server
            // ReceiveMessage 
            _hubConnection.On<List<CategoryResponseDto>>("ReceiveCategories", (categoriesResponse) =>
            {
                OnMessageReceived?.Invoke(categoriesResponse);
            });
            // error handling
            _hubConnection.Reconnecting += error =>
            {
                Console.WriteLine("Server unreachable. Attempting to reconnect...");
                return Task.CompletedTask;
            };
            // error handling
            _hubConnection.Closed += error =>
            {
                Console.WriteLine("Connection closed. Server is down or unreachable.");
                return Task.CompletedTask;
            };
        }

        public async Task CloseAsync() =>  await _hubConnection.StopAsync();
        public async Task StartAsync(){
            await _hubConnection.StartAsync();
        }

        // SendMessage : must be a registered method on server hub class
        public async Task SendAndReceiveCategories(List<CategoryResponseDto> categoriesResponse) => 
        
        await _hubConnection.SendAsync("PopulateCategories",categoriesResponse);

    }
}