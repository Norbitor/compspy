using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CompSpyWeb.DAL;
using System.Data.Entity;
using static CompSpyWeb.Controllers.Hubs.SuirvelanceHub;

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
                    comp.ConnectionID = Context.ConnectionId;
                    ctx.Entry(comp).State = EntityState.Modified;
                    ctx.SaveChanges();

                    Groups.Add(Context.ConnectionId, comp.Classroom.Name);
                    Clients.Client(Context.ConnectionId).ConnectionFeedback(true);
                } else
                {
                    Clients.Client(Context.ConnectionId).ConnectionFeedback(false);
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
                    comp.ConnectionID = null;
                    ctx.Entry(comp).State = EntityState.Modified;
                    ctx.SaveChanges();
                    Groups.Remove(Context.ConnectionId, comp.Classroom.Name);
                }
            }
        }

        public void ReceiveData(string data)
        {
            var suirvelanceHub = GlobalHost.ConnectionManager.GetHubContext<SuirvelanceHub, ISuirvelanceHubModel>();
            suirvelanceHub.Clients.All.ComputerDataReceived(data);
        }

        public interface IComputerHubModel
        {
            void BroadcastMessageReceived(string msg);
            void ConnectionFeedback(bool success);

            void StartLowQualityTransmission();
            void StopLowQualityTransmission();

            void StartHighQualityTransmission();
            void StopHighQualityTransmission();
        }
    }
}
