using Common.Dictionaries;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BackEndClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var repository = new NotificationsRepository();

            var hubConnection = new HubConnection($"{Servers.NotificationHubServer}/signalr");
            var hubProxy = hubConnection.CreateHubProxy(Hubs.NotificationHubName);

            hubConnection.Start().Wait();

            var notifications = repository.GetNotifications();

            //sort from oldes to newest
            var sortedNotifications = notifications.OrderBy(n => n.CreationDate);
            foreach (var notification in sortedNotifications)
            {
                Thread.Sleep(10000);
                hubProxy.Invoke<Notification>(Actions.SendNotification, notification);
            }

            Console.ReadLine();
        }
    }

    class NotificationsRepository
    {
        public IEnumerable<Notification> GetNotifications()
        {
            return new List<Notification>
            {
                new Notification { CreationDate = DateTime.Now.AddHours(-1), Message = "Notification 1" },
                new Notification { CreationDate = DateTime.Now.AddHours(-2), Message = "Notification 2" },
                new Notification { CreationDate = DateTime.Now.AddHours(-3), Message = "Notification 3" },
                new Notification { CreationDate = DateTime.Now.AddHours(-4), Message = "Notification 4" },
                new Notification { CreationDate = DateTime.Now.AddHours(-5), Message = "Notification 5" },
                new Notification { CreationDate = DateTime.Now.AddHours(-6), Message = "Notification 6" },
                new Notification { CreationDate = DateTime.Now.AddHours(-7), Message = "Notification 7" },
                new Notification { CreationDate = DateTime.Now.AddHours(-8), Message = "Notification 8" },
                new Notification { CreationDate = DateTime.Now.AddHours(-9), Message = "Notification 9" },
                new Notification { CreationDate = DateTime.Now.AddHours(-10), Message = "Notification 10" }
            };
        }
    }
}
