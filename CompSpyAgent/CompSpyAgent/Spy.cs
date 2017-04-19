using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;

namespace CompSpyAgent
{
    class Spy
    {
        public Bitmap bmp;
        public Graphics zrzut;

        public Process[] listaProcesow;
        public List<String> listaStron;


            public Spy()
        {
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            zrzut = Graphics.FromImage(bmp);

            listaStron = new List<String>();
       
        }     
        
        public void Aktualizacja()
        {
            

            zrzut.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            listaProcesow = Process.GetProcesses();

            //aktualizacja otwartych stron

            foreach(var p in listaProcesow)
            {
                if (Convert.ToString(p.ProcessName) == "chrome")
                {
                    listaStron.Add("[chrome] " + p.MainWindowTitle);
                }
            }





        }   

        public void getZrzut(PictureBox p)
        {
            p.Image = bmp;
        }

        public void getListaProcesow(ListView lw)
        {
            lw.Items.Clear();

            foreach(var p in listaProcesow)
            {
                lw.Items.Add(p.ProcessName);
            }
        }

        public void getListaStron(ListView lw)
        {
            lw.Items.Clear();
            foreach(var p in listaStron)
            {
                lw.Items.Add(p);
            }
        }
    }
}
