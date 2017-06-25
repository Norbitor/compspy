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
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization;
using NDde.Client;

namespace CompSpyAgent
{
    class Spy
    {
        public Bitmap bmp;
        public Graphics zrzut;

        public Process[] listaProcesow;
        public List<string> listaStron;
        public bool automatycznie;
        public int czasAutomatycznie;
        public DateTime czasOst;


        [DataContract]
        public class Komunikat
        {
            [DataMember]
            public string image { get; set; }

            [DataMember]
            public bool hq { get; set; }

            [DataMember]
            public List<string> listaProcesow { get; set; }

            [DataMember]
            public List<string> listaStron { get; set; }

            public Komunikat(string img, Process[] procesy, List<string> strony, bool hq)
            {
                image = img;
                this.hq = hq;
                listaProcesow = new List<string>();
                listaStron = new List<string>();

                foreach (var p in procesy)
                {
                    listaProcesow.Add(p.ProcessName);
                }

                foreach (var s in strony)
                {
                    listaStron.Add(s);
                }

            }

        }

        public string serializacja(string imageName, bool hq)
        {
            var komunikat = new Komunikat(imageName, listaProcesow, listaStron, hq);
            var json = new JavaScriptSerializer().Serialize(komunikat);

            return json;
        }

        public Spy()
        {
            bmp = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            zrzut = Graphics.FromImage(bmp);

            listaStron = new List<string>();

            automatycznie = false;
            czasAutomatycznie = 30;
            czasOst = DateTime.Now;
        }



        public void Aktualizacja()
        {


            zrzut.CopyFromScreen(0, 0, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);

            listaProcesow = Process.GetProcesses();

            //aktualizacja otwartych stron
            listaStron.Clear();
            getURLfirefox();

            foreach (var p in listaProcesow)
            {
                if (Convert.ToString(p.ProcessName) == "chrome")
                {
                    if (p.MainWindowTitle != "" || p.MainWindowTitle == " ")
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
        public string getImageBase64(Bitmap img)
        {

            Graphics g = Graphics.FromImage(img);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imageBytes = stream.ToArray();

            return Convert.ToBase64String(imageBytes); 
        }

        public byte[] getImageByteArray(Bitmap img)
        {
            Graphics g = Graphics.FromImage(img);

            System.IO.MemoryStream stream = new System.IO.MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] imageBytes = stream.ToArray();

            return imageBytes;
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
