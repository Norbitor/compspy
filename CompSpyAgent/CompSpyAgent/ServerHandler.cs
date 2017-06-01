using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Forms;
using System.Net.Http;

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
                { "stationId", "PC1" },
                { "secret", "fafafa" }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(hubAddr + "/Computers/Connect", content);
            var responseStr = await response.Content.ReadAsStringAsync();
            if (responseStr.Contains("SUCCESS"))
            {
                appForm.ShowTrayMessage("Connection to server established");
                appForm.SetConnectionStateLabel("Connected");
                await computerHub.Invoke("Connect", "PC1");
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
                { "stationId", "PC1" },
                { "secret", "fafafa" }
            };
            var content = new FormUrlEncodedContent(parameters);
            var response = await client.PostAsync(hubAddr + "/Computers/Disconnect", content);
            await computerHub.Invoke("Disconnect", "PC1");
        }

        private void ConfigureRPCHandlers()
        {
            computerHub.On<string>("BroadcastMessageReceived", x =>
            {
                appForm.ShowTrayMessage(x);
            });
        }

        public void StartListening()
        {
            hubConnection.Start().Wait();
        }
    }
}
