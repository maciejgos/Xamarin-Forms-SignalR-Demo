using Common.Dictionaries;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System;

namespace XamarinAndSignalRDemo
{
    public partial class MainPage : ContentPage
    {
        ObservableCollection<Notification> notifications = new ObservableCollection<Notification>();

        public MainPage()
        {
            InitializeComponent();
            ConnectToHub();

#if DEBUG
            CreateFakeData();
#endif

            NotificationsView.ItemsSource = notifications;
        }

        private void CreateFakeData()
        {
            notifications.Add(new Notification { CreationDate = DateTime.Now.AddHours(-2), Message = "Limit na karcie jest bliski wyczerpaniu. Powiększ limit." });
            notifications.Add(new Notification { CreationDate = DateTime.Now.AddHours(-1), Message = "Super oferta kredyciku" });
        }

        private async void ConnectToHub()
        {
            var hubConnection = new HubConnection($"{Servers.NotificationHubServer}/signalr");
            var hubProxy = hubConnection.CreateHubProxy(Hubs.NotificationHubName);

            await hubConnection.Start();

            hubProxy.On<Notification>(Actions.SendNotification, notification => DisplayNotification(notification));
        }

        private void DisplayNotification(Notification notification)
        {
            Device.BeginInvokeOnMainThread(() => notifications.Add(notification));
        }
    }
}
