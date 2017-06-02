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

            EstablishConnection();
            hubConnection = new HubConnection(hubAddr);
            computerHub = hubConnection.CreateHubProxy("ComputerHub");
            ConfigureRPCHandlers();
        }

        private async void EstablishConnection()
        {
            var parameters = new Dictionary<string, string>
            {
                { "stationId", ConfigurationManager.AppSettings["stationDiscr"]},
                { "secret", ConfigurationManager.AppSettings["secret"] }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(hubAddr + "/Computers/Connect", content);
            var responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("SUCCESS"))
            {
                appForm.ShowTrayMessage("Connection to server established");
                appForm.SetConnectionStateLabel("Connected");
                await computerHub.Invoke("Connect", ConfigurationManager.AppSettings["stationDiscr"]);
            } else
            {
                appForm.ShowTrayMessage("Connection to server failed");
                appForm.SetConnectionStateLabel("Failed");
                throw new ServerConnectionException(responseStr);
            }
        }

        public async void CloseConnection()
        {
            var parameters = new Dictionary<string, string>
            {
                { "stationId", ConfigurationManager.AppSettings["stationDiscr"] },
                { "secret", ConfigurationManager.AppSettings["secret"] }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(hubAddr + "/Computers/Disconnect", content);
            await computerHub.Invoke("Disconnect", ConfigurationManager.AppSettings["stationDiscr"]);
        }

        private void ConfigureRPCHandlers()
        {
            computerHub.On<string>("BroadcastMessageReceived", x =>
            {
                appForm.ShowTrayMessage(x);
            });
            computerHub.On("StartLowQualityTransmission", () =>
            {
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
        }
    }
}
