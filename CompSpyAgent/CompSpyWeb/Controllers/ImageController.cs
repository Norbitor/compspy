using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CompSpyWeb.DAL;

namespace CompSpyWeb.Controllers
{
    public class ImageController : Controller
    {
        private static int cnt = 0;

        [HttpPost]
        public JsonResult Upload(string stationDiscr, string quality, HttpPostedFileBase uploadFile)
        {
            DeleteOldImages();
            JsonResult returnResult = null;
            if (uploadFile.ContentLength > 0)
            {

                string uniqueFileName = stationDiscr + '_' + quality + '_' + DateTime.Now.ToFileTimeUtc().ToString();
                string uploadRoot = GetUploadsDirectory();

                string filePath = Path.Combine(uploadRoot, uniqueFileName + ".png");

                uploadFile.SaveAs(filePath);
                cnt++;

                returnResult = new JsonResult
                {
                    Data = Path.GetFileNameWithoutExtension(filePath),
                    ContentType = "application/json",
                    ContentEncoding = System.Text.Encoding.UTF8
                };
            }
            return returnResult;
        }

        public ActionResult DisplayPhoto(string filename)
        {
            if (filename == null) return HttpNotFound();
            string uploadRoot = GetUploadsDirectory();
            return new ImageResult { Image = Bitmap.FromFile(Path.Combine(uploadRoot, filename + ".png")), ImageFormat = ImageFormat.Png };
        }

        private static string GetUploadsDirectory()
        {
            string websiteLocation = System.Web.HttpContext.Current.Server.MapPath("~");
            string uploadRoot = Directory.GetParent(websiteLocation).Parent.FullName;
            uploadRoot = Path.Combine(uploadRoot, "Uploads");
            return uploadRoot;
        }

        private void DeleteOldImages()
        {
            List<string> protectedFiles;
            using (var ctx = new CompSpyContext())
            {
                protectedFiles = ctx.Abuses.Where(a => !a.Read).Select(abuse => abuse.ScreenPath).ToList();
            }
            var di = new DirectoryInfo(GetUploadsDirectory());
            foreach(FileInfo file in di.GetFiles())
            {
                if (!protectedFiles.Contains(file.Name.Substring(0, file.Name.Length - 4)) &&
                    DateTime.Compare(file.CreationTime, DateTime.Now.AddSeconds(-30)) < 0)
                {
                    file.Delete();
                }
            }
        }
    }

    public class ImageResult : ActionResult
    {
        public ImageResult() { }
        public System.Drawing.Image Image { get; set; }
        public ImageFormat ImageFormat { get; set; }
        public override void ExecuteResult(ControllerContext context)
        {
            // verify properties 
            if (Image == null)
            {
                throw new ArgumentNullException("Image");
            }
            if (ImageFormat == null)
            {
                throw new ArgumentNullException("ImageFormat");
            }
            // output 
            context.HttpContext.Response.Clear();
            if (ImageFormat.Equals(ImageFormat.Bmp)) context.HttpContext.Response.ContentType = "image/bmp";
            if (ImageFormat.Equals(ImageFormat.Gif)) context.HttpContext.Response.ContentType = "image/gif";
            if (ImageFormat.Equals(ImageFormat.Icon)) context.HttpContext.Response.ContentType = "image/vnd.microsoft.icon";
            if (ImageFormat.Equals(ImageFormat.Jpeg)) context.HttpContext.Response.ContentType = "image/jpeg";
            if (ImageFormat.Equals(ImageFormat.Png)) context.HttpContext.Response.ContentType = "image/png";
            if (ImageFormat.Equals(ImageFormat.Tiff)) context.HttpContext.Response.ContentType = "image/tiff";
            if (ImageFormat.Equals(ImageFormat.Wmf)) context.HttpContext.Response.ContentType = "image/wmf";
            Image.Save(context.HttpContext.Response.OutputStream, ImageFormat);
        }
    }
}