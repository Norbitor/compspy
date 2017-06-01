﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using static CompSpyWeb.Controllers.Hubs.ComputerHub;

namespace CompSpyWeb.Controllers.Hubs
{
    public class SuirvelanceHub : Hub<SuirvelanceHub.ISuirvelanceHubModel>
    {
        public void Connect(string classroom)
        {
            var chub = GlobalHost.ConnectionManager.GetHubContext<ComputerHub, IComputerHubModel>();
            chub.Clients.Group(classroom).BroadcastMessageReceived("Hello!");
        }

        public interface ISuirvelanceHubModel
        {
            void ComputerDataReceived(string jsonData);
            void ComputerConnected(string stationDiscr);
            void ComputerDisconnected(string stationDiscr);
        }
    }
}