using CompSpyAgent.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CompSpyAgent
{
    public partial class ApplicationForm : Form
    {
        private ServerHandler srvhan;

        public ApplicationForm()
        {
            InitializeComponent();
            this.Icon = Resources.AppIcon;
            ConfigureTray();
            EstablishConnection();            
        }

        public void ShowTrayMessage(string msg)
        {
            trayIcon.BalloonTipTitle = "CompSpy Agent 1.0";
            trayIcon.BalloonTipText = msg;
            trayIcon.ShowBalloonTip(500);
        }

        public void SetConnectionStateLabel(string msg)
        {
            lblStatus.Text = msg;
        }

        private void EstablishConnection()
        {
            try
            {
                srvhan = new ServerHandler("http://" + txbServerIP.Text, this);
                srvhan.StartListening();
            }
            catch (ServerConnectionException ex)
            {
                Console.Error.WriteLine("Received following response from server, which was unexpected!\n"
                    + ex.Message);
            }
        }

        private void ConfigureTray()
        {
            trayIcon.Icon = Resources.AppIcon;
            trayIcon.ContextMenu = new ContextMenu(new MenuItem[]
            {
                new MenuItem("&Show", (object sender, EventArgs e) =>
                {
                    Show();
                    WindowState = FormWindowState.Normal;
                    ShowInTaskbar = true;
                }),
                new MenuItem("&Exit", (object sender, EventArgs e) =>
                {
                    trayIcon.Visible = false;
                    Application.Exit();
                })                
            });
            trayIcon.Visible = true;
        }

        private void ApplicationForm_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                Hide();
            }
        }

        private void btnAdministrator_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Not implemented, yet.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void trayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Podglad p = new CompSpyAgent.Podglad();
            p.ShowDialog();


        }

        private void ApplicationForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            srvhan.CloseConnection();
        }
    }
}
