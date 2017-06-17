using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using static CompSpyWeb.Controllers.Hubs.ComputerHub;

namespace CompSpyWeb.Controllers.Hubs
{
    public class SuirvelanceHub : Hub<SuirvelanceHub.ISuirvelanceHubModel>
    {
        IHubContext<IComputerHubModel> computerHub;

        public SuirvelanceHub()
        {
            computerHub = GlobalHost.ConnectionManager.GetHubContext<ComputerHub, IComputerHubModel>();
        }

        public void Connect(string classroom)
        {
            Groups.Add(Context.ConnectionId, classroom);
            computerHub.Clients.Group(classroom).StartLowQualityTransmission();
        }

        public void Disconnect(string classroom)
        {
            Groups.Remove(Context.ConnectionId, classroom);
            computerHub.Clients.Group(classroom).StopLowQualityTransmission();
        }

        public void ConnectHq(string connectionId)
        {
            Groups.Add(Context.ConnectionId, connectionId);
            computerHub.Clients.Client(connectionId).StartHighQualityTransmission();
        }

        public void DisconnectHq(string connectionId)
        {
            Groups.Remove(Context.ConnectionId, connectionId);
            computerHub.Clients.Client(connectionId).StopHighQualityTransmission();
        }

        public void SendMessageToClassroom(string classroom, string message)
        {
            computerHub.Clients.Group(classroom).BroadcastMessageReceived(message);
        }

        public void SendMessageToClient(string connectionId, string message)
        {
            computerHub.Clients.Client(connectionId).BroadcastMessageReceived(message);
        }

        public interface ISuirvelanceHubModel
        {
            void ComputerDataReceived(string jsonData);
            void ComputerConnected(string stationDiscr);
            void ComputerDisconnected(string stationDiscr);
        }
    }
}
