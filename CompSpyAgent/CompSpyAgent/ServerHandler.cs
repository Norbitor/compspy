﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR.Client;
using System.Windows.Forms;
using System.Net.Http;
using System.Configuration;
using System.Timers;

namespace CompSpyAgent
{
    class ServerHandler
    {
        private static readonly HttpClient client = new HttpClient();
        private HubConnection hubConnection;
        private IHubProxy computerHub;
        private ApplicationForm appForm;
        private string hubAddr;

        private System.Timers.Timer lqTimer;
        private System.Timers.Timer hqTimer;
        private Spy spy;
         

        public ServerHandler(string hubAddr, ApplicationForm appForm)
        {
            this.hubAddr = hubAddr;
            this.appForm = appForm;

            spy = new Spy();
            lqTimer = new System.Timers.Timer();
            lqTimer.Elapsed += new ElapsedEventHandler(OnLqTimerEvent);
            lqTimer.Interval = 3000;

            hqTimer = new System.Timers.Timer();
            hqTimer.Elapsed += new ElapsedEventHandler(OnHqTimerEvent);
            hqTimer.Interval = 1000;

            hubConnection = new HubConnection(hubAddr);
            computerHub = hubConnection.CreateHubProxy("ComputerHub");
            ConfigureRPCHandlers();
        }

        private void OnLqTimerEvent(object o, ElapsedEventArgs e)
        {
            spy.Aktualizacja();
            var data = spy.serializacja(false);
            computerHub.Invoke("ReceiveData", data);
            Console.WriteLine("[INFO] LQ Screen done");
        }

        private void OnHqTimerEvent(object o, ElapsedEventArgs e)
        {
            spy.Aktualizacja();
            var data = spy.serializacja(true);
            computerHub.Invoke("ReceiveData", data);
            Console.WriteLine("[INFO] HQ Screen done");
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
                lqTimer.Start();
            });
            computerHub.On("StopLowQualityTransmission", () =>
            {
                lqTimer.Stop();
            });
            computerHub.On("StartHighQualityTransmission", () =>
            {
                hqTimer.Start();
            });
            computerHub.On("StopHighQualityTransmission", () =>
            {
                hqTimer.Stop();
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
