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
            listaZPrzegladarki("Firefox");
            //listaZPrzegladarki("firefox");
            listaZPrzegladarki("Chrome");





        } 
        public void listaZPrzegladarki(string przegladarka)
        {
            try
            {
                DdeClient dde = new DdeClient(przegladarka, "WWW_GetWindowInfo");
                dde.Connect();
                string url = dde.Request("URL", int.MaxValue);
                string[] urls = url.Split(new string[] { "\",\"" }, StringSplitOptions.RemoveEmptyEntries);
                dde.Disconnect();
                foreach(var u in urls)
                {
                    listaStron.Add(u);
                }
            }
            catch
            {

            }
        }

        public void getZrzut(PictureBox p)
        {
            p.Image = bmp;
        }

        public String getHQScreen()
        {
           Bitmap img = new Bitmap(bmp, new Size(960, 540)); // 1/2 zdjecia
            String ret;

            using(System.Drawing.Image img2 = img)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    img2.Save(m, img.RawFormat);
                    byte[] bytes = m.ToArray();
                    ret = Convert.ToBase64String(bytes);
                }
            }
            return ret;
        }

        public Image getLQScreen()
        {
            Image img = new Bitmap(bmp, new Size(240, 135)); // 1/8 zdjecia
            String ret;

            using (System.Drawing.Image img2 = img)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    img2.Save(m, img.RawFormat);
                    byte[] bytes = m.ToArray();
                    ret = Convert.ToBase64String(bytes);
                }
            }

            return ret;
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
