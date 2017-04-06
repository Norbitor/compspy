using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Management;

namespace CSATest
{
    public partial class Form1 : Form
    {
        private Bitmap _bmpScreenCapture;
        public Form1()
        {
            InitializeComponent();
            _bmpScreenCapture = new Bitmap(Screen.PrimaryScreen.Bounds.Width,
                                            Screen.PrimaryScreen.Bounds.Height);

            listView1.View = View.Details;
            listView1.Columns.Add("ProcName");
            listView1.Columns.Add("PID");
            listView1.Columns.Add("WT");
            listView1.Columns.Add("US");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (Graphics g = Graphics.FromImage(_bmpScreenCapture))
            {
                g.CopyFromScreen(Screen.PrimaryScreen.Bounds.X,
                                    Screen.PrimaryScreen.Bounds.Y,
                                    0, 0,
                                    _bmpScreenCapture.Size,
                                    CopyPixelOperation.SourceCopy);
            }
            pictureBox1.Image = _bmpScreenCapture;

            Process[] procs = Process.GetProcesses();

            var x = from p in procs
                    
                    select p;

            listView1.Items.Clear();
            foreach(var Proc in x)
            {
                ListViewItem it = new ListViewItem(Proc.ProcessName);
                it.SubItems.Add(Proc.Id.ToString());
                it.SubItems.Add(Proc.MainWindowTitle);
                it.SubItems.Add(/*GetProcessOwner(Proc.Id)*/ "");
                listView1.Items.Add(it);
            }
            
        }

        private string GetProcessOwner(int processId)
        {
            string query = "Select * From Win32_Process Where ProcessID = " + processId;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(query);
            ManagementObjectCollection processList = searcher.Get();

            foreach (ManagementObject obj in processList)
            {
                string[] argList = new string[] { string.Empty, string.Empty };
                int returnVal = Convert.ToInt32(obj.InvokeMethod("GetOwner", argList));
                if (returnVal == 0)
                {
                    // return DOMAIN\user
                    return argList[1] + "\\" + argList[0];
                }
            }

            return "";
        }
    }
}
