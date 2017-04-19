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
    public partial class Podglad : Form
    {
        Spy s;

        public Podglad()
        {
            InitializeComponent();
            s = new CompSpyAgent.Spy();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            s.Aktualizacja();
            s.getZrzut(pictureBox2);
            s.getListaProcesow(listView1);
            s.getListaStron(listView2);
        }
    }
}
