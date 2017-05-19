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
        ServerHandler srvhan;

        public ApplicationForm()
        {
            InitializeComponent();
            this.Icon = Resources.AppIcon;
            ConfigureTray();

            srvhan = new ServerHandler("http://{txbServerIP}", trayIcon);
            srvhan.StartListening();
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

        private void txbServerIP_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Podglad p = new CompSpyAgent.Podglad();
            p.ShowDialog();


        }
    }
}
