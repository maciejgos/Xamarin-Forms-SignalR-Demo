using Common.Dictionaries;
using Common.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Owin;
using System;
using System.Threading.Tasks;

namespace SignalRServer
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start(Servers.NotificationHubServer))
            {
                Console.WriteLine($"Server running on {Servers.NotificationHubServer}");
                Console.ReadLine();
            }
        }
    }

    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll);
            app.MapSignalR();
        }
    }

    public class NotificationHub : Hub
    {
        public void SendNotification(Notification notification)
        {
            Clients.All.sendNotification(notification);
        }

        public override Task OnConnected()
        {
            Console.WriteLine($"Connection with ID {Context.ConnectionId} connected");
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Console.WriteLine($"Connection with ID {Context.ConnectionId} disconnected");
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            Console.WriteLine($"Connection with ID {Context.ConnectionId} reconnected");
            return base.OnReconnected();
        }
    }
}
