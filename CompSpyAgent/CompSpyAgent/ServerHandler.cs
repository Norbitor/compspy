using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Forms;
using System.Net.Http;
using System.Configuration;

namespace CompSpyAgent
{
    class ServerHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private HubConnection hubConnection;
        private IHubProxy computerHub;
        private ApplicationForm appForm;
        private string hubAddr;

        public ServerHandler(string hubAddr, ApplicationForm appForm)
        {
            this.hubAddr = hubAddr;
            this.appForm = appForm;
            
            hubConnection = new HubConnection(hubAddr);
            computerHub = hubConnection.CreateHubProxy("ComputerHub");
            ConfigureRPCHandlers();
        }

        private void EstablishConnection()
        {
            var parameters = new Dictionary<string, string>
            {
                { "stationId", ConfigurationManager.AppSettings["stationDiscr"]},
                { "secret", ConfigurationManager.AppSettings["secret"] }
            };
            computerHub.Invoke("Connect", ConfigurationManager.AppSettings["stationDiscr"]);
        }

        public void CloseConnection()
        {
            var parameters = new Dictionary<string, string>
            {
                { "stationId", ConfigurationManager.AppSettings["stationDiscr"] },
                { "secret", ConfigurationManager.AppSettings["secret"] }
            };
            computerHub.Invoke("Disconnect", ConfigurationManager.AppSettings["stationDiscr"]);
        }

        private void ConfigureRPCHandlers()
        {
            computerHub.On<string>("BroadcastMessageReceived", msg =>
            {
                appForm.ShowTrayMessage(msg);
            });
            computerHub.On<bool>("ConnectionFeedback", isSuccessfull =>
            {
                if (isSuccessfull)
                {
                    appForm.ShowTrayMessage("Connected");
                    appForm.SetConnectionStateLabel("Connected");
                }
                else
                {
                    appForm.ShowTrayMessage("Not connected!");
                    appForm.SetConnectionStateLabel("Error");
                }
            });
            computerHub.On("StartLowQualityTransmission", () =>
            {
                Spy spy = new Spy();
                spy.Aktualizacja();
                var data = spy.serializacja(false);
                computerHub.Invoke("ReceiveData", data);
                appForm.ShowTrayMessage("odebrano żądanie śledzenia podstawowego");
            });
            computerHub.On("StopLowQualityTransmission", () =>
            {
                appForm.ShowTrayMessage("odebrano żądanie zakończenia śledzenia podst.");
            });
            computerHub.On("StartHighQualityTransmission", () =>
            {
                appForm.ShowTrayMessage("odebrano żądanie śledzenia hq");
            });
            computerHub.On("StopHighQualityTransmission", () =>
            {
                appForm.ShowTrayMessage("odebrano żądanie zakończenia śledzenia hq");
            });
        }

        public void StartListening()
        {
            hubConnection.Start().Wait();
            EstablishConnection();
        }

        public void SendData(String data)
        {
            hubConnection.Send(data);
        }
    }
}
