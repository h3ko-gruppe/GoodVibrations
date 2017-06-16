using System;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;

namespace GoodVibrations.SignalRTestClient
{
    class SignalRClient
    {
        private readonly string _url;

        public SignalRClient(string url)
        {
            _url = url;
        }

        public async Task ConnectToSignalRHub()
        {
            var hubConnection = new HubConnection(_url);
            var locationPrody = hubConnection.CreateHubProxy("NotifyHub");
            locationPrody.On<string>("Notify", eventId =>
            {
                Console.WriteLine($"SignalR notification recieved: {eventId}");
            });

            //wire up all events
            hubConnection.StateChanged +=
                change => Console.WriteLine($"{DateTime.Now:O}\tState Changed! Old State: {change.OldState} New State: {change.NewState}");
            hubConnection.Closed += () => Console.WriteLine($"{DateTime.Now:O}\tConnection Closed");
            hubConnection.ConnectionSlow += () => Console.WriteLine($"{DateTime.Now:O}\tConnection Slow");
            hubConnection.Reconnected += () => Console.WriteLine($"{DateTime.Now:O}\tReconnected");
            hubConnection.Error += exception => Console.WriteLine($"{DateTime.Now:O}\tError: {exception}");
            hubConnection.Reconnecting += () => Console.WriteLine($"{DateTime.Now:O}\tReconnecting");
            //hubConnection.Received += s => Console.WriteLine($"{DateTime.Now:O}\tReceived: {s}");


            //start connection to hub
            await hubConnection.Start();
            Console.WriteLine($"{DateTime.Now:O}\tHubConnection Started!");
        }
    }
}
