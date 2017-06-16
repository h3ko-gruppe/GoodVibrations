using System;

namespace GoodVibrations.SignalRTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            const string url = "http://goodvibrations-dev.azurewebsites.net/";
            const string url2 = "http://localhost:6169//signalr";
            while (true)
            {
                Console.WriteLine("SignalR Testclient.");
                Console.WriteLine($"Connect To: {url}");
               
                try
                {
                    var client = new SignalRClient(url);
                    client.ConnectToSignalRHub().GetAwaiter().GetResult();

                }
                catch (Exception e)
                {
                    Console.WriteLine($"{DateTime.Now:O}\tError: {e}");
                    Console.Write("\r\n\r\nPress ENTER to retry. Press CTRL+C to exit the application!\r\n\r\n");
                    Console.ReadLine();
                    continue;
                }

                Console.Write("\r\n\r\nPress CTRL+C to exit the application!\r\n\r\n");
                //run forever 
                while (true)
                    Console.ReadLine();
            }
        }
    }
}
