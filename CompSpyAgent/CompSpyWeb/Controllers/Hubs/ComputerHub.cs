using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace CompSpyWeb.Controllers.Hubs
{
    public class ComputerHub : Hub<ComputerHub.IComputerHubModel>
    {
        public void BroadcastMessage(string msg)
        {
            Clients.All.BroadcastMessageReceived(msg);
        }

        public interface IComputerHubModel
        {
            void BroadcastMessageReceived(string msg);
        }
    }
}
