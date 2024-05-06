using Designer.Dnn.Designer.Components;
using Designer.Dnn.Designer.Models;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using Hotcakes.Commerce.Catalog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Designer.Dnn.Designer.Controllers
{
    public class DesignableProductController : DnnController
    {
        public ActionResult Index()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<DesignableProduct>();
            var products = rep.Get();
            return View(products);
        }

        [HttpGet]
        public ActionResult Detail(int id)
        {
            DesignableProduct product;
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<DesignableProduct>();
            product = rep.GetById(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Detail(HttpPostedFileBase graphicFile, int id)
        {
            DesignableProduct product;
            IDataContext ctx = DataContext.Instance();

            if (graphicFile != null && graphicFile.ContentLength > 0)
            {
                var rep2 = ctx.GetRepository<UsableGraphics>();

                var fileName = Path.GetFileName(graphicFile.FileName);

                var path = Path.Combine(@"C:\DNN\DesktopModules\MVC\shirtdesigner\Designer\Assets\UsableGraphics\", fileName);

                graphicFile.SaveAs(path);

                UsableGraphics graphic = new UsableGraphics
                {
                    GraphicPic = fileName
                };

                try
                {
                    rep2.Insert(graphic);
                    ctx.Commit();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }

            }
            
            var rep = ctx.GetRepository<DesignableProduct>();
            product = rep.GetById(id);
            
            return View(product);

        }

        [HttpGet]
        public ActionResult GetUsableGraphics()
        {

            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<UsableGraphics>();
            var images = rep.Get();

            return PartialView("GetUsableGraphics", images);
        }

        [HttpPost]
        public ActionResult SaveDesign(string imageData, int id)
        {
            DesignableProduct product;
            IDataContext ctx = DataContext.Instance();

            string fileName = Guid.NewGuid().ToString() + ".png";
            string folderPath = @"C:\DNN\DesktopModules\MVC\shirtdesigner\Designer\Assets\CreatedDesigns\";
            string filePath = Path.Combine(folderPath, fileName);

            // Converting base64 data to bytes
            if (!String.IsNullOrEmpty(imageData))
            {
                imageData = imageData.Substring(imageData.IndexOf(",") + 1); // Remove the header part of the data URL
                byte[] imageBytes = Convert.FromBase64String(imageData);

                System.IO.File.WriteAllBytes(filePath, imageBytes); // Save the image

                var rep2 = ctx.GetRepository<UsableGraphics>();

                CustomDesign design = new CustomDesign
                {
                    DesignImg = imageData,
                }

            }

            var rep = ctx.GetRepository<DesignableProduct>();
            product = rep.GetById(id);

            return View("Detail", product);

        }

    }
}