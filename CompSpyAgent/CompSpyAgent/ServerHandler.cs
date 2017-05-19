using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Forms;

namespace CompSpyAgent
{
    class ServerHandler
    {
        private HubConnection hubConnection;
        private IHubProxy computerHub;
        private NotifyIcon trayIcon;

        public ServerHandler(string hubAddr, NotifyIcon tray)
        {
            hubConnection = new HubConnection(hubAddr);
            computerHub = hubConnection.CreateHubProxy("ComputerHub");
            ConfigureRPCHandlers();
            trayIcon = tray;
        }

        private void ConfigureRPCHandlers()
        {
            computerHub.On<string>("BroadcastMessageReceived", x =>
            {
                trayIcon.BalloonTipTitle = "CompSpy Agent 1.0";
                trayIcon.BalloonTipText = x;
                trayIcon.ShowBalloonTip(500);
            });
        }

        public void StartListening()
        {
            hubConnection.Start().Wait();
        }

    }
}
