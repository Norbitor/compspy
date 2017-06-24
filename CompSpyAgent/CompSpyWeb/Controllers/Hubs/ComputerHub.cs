using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using CompSpyWeb.DAL;
using System.Data.Entity;
using static CompSpyWeb.Controllers.Hubs.SuirvelanceHub;
using System.Xml.Linq;

namespace CompSpyWeb.Controllers.Hubs
{
    public class ComputerHub : Hub<ComputerHub.IComputerHubModel>
    {
        private IHubContext<ISuirvelanceHubModel> suirvelanceHub;

        public ComputerHub()
        {
            suirvelanceHub = GlobalHost.ConnectionManager.GetHubContext<SuirvelanceHub, ISuirvelanceHubModel>();
        }

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
                    var groupsToInform = new List<string> { comp.Classroom.Name, Context.ConnectionId };
                    suirvelanceHub.Clients.Groups(groupsToInform).ComputerConnected(stationDiscr);
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
                    var groupsToInform = new List<string> { comp.Classroom.Name, Context.ConnectionId };
                    suirvelanceHub.Clients.Groups(groupsToInform).ComputerDisconnected(stationDiscr);
                }
            }
        }

        public void ReceiveData(string data)
        {
            Clients.Client(Context.ConnectionId).ACK();
            //// TODO: Determine station dicr and HQ/LQ by data
            using (var ctx = new CompSpyContext())
            {

                var comp = ctx.Computers.Where(c => c.ConnectionID == Context.ConnectionId).FirstOrDefault();
                if (comp != null)
                {
                    suirvelanceHub.Clients.Group(comp.Classroom.Name).ComputerDataReceived(data);
                }
            }

        }

        public interface IComputerHubModel
        {
            void BroadcastMessageReceived(string msg);
            void ConnectionFeedback(bool success);

            void StartLowQualityTransmission();
            void StopLowQualityTransmission();

            void StartHighQualityTransmission();
            void StopHighQualityTransmission();
            void ACK();
        }
    }
}
