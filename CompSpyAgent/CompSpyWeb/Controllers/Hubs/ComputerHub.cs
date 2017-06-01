using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CompSpyWeb.DAL;

namespace CompSpyWeb.Controllers.Hubs
{
    public class ComputerHub : Hub<ComputerHub.IComputerHubModel>
    {
        public void BroadcastMessage(string msg)
        {
            Clients.All.BroadcastMessageReceived(msg);
        }

        public void Connect(string stationDiscr)
        {        
            using (var ctx = new CompSpyContext())
            {
                var comp = ctx.Computers.Where(c => c.StationDiscriminant == stationDiscr).FirstOrDefault();
                if (comp != null)
                {
                    Groups.Add(Context.ConnectionId, comp.Classroom.Name);
                }
            }
        }

        public void Disconnect(string stationDiscr)
        {
            using (var ctx = new CompSpyContext())
            {
                var comp = ctx.Computers.Where(c => c.StationDiscriminant == stationDiscr).FirstOrDefault();
                if (comp != null)
                {
                    Groups.Remove(Context.ConnectionId, comp.Classroom.Name);
                }
            }
        }

        public void ReceiveData(string data)
        {

        }

        public interface IComputerHubModel
        {
            void BroadcastMessageReceived(string msg);
            void StartLowQualityTransmission();
            void StopLowQualityTransmission();

            void StartHighQualityTransmission();
            void StopHighQualityTransmission();
        }
    }
}
