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
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace Designer.Dnn.Designer.Controllers
{
    public class DesignableProductController : DnnController
    {
        public int itemId;
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
            itemId = id;
            DesignableProduct product;
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<DesignableProduct>();
            product = rep.GetById(id);
            return View(product);
        }

        [HttpPost]
        public ActionResult Detail(HttpPostedFileBase graphicFile)
        {
            DesignableProduct product;
            IDataContext ctx = DataContext.Instance();

            if (graphicFile != null && graphicFile.ContentLength > 0)
            {
                var rep2 = ctx.GetRepository<UsableGraphics>();
                //int maxId = rep2.Get().Max(g => (int?)g.GraphicId) ?? 0; // Use nullable int to handle an empty table
                //int newId = maxId + 1;

                var fileName = Path.GetFileName(graphicFile.FileName);

                var path = Path.Combine(@"C:\DNN\DesktopModules\MVC\shirtdesigner\Designer\Assets\UsableGraphics\", fileName);

                graphicFile.SaveAs(path);

                UsableGraphics graphic = new UsableGraphics
                {
                    //GraphicId = newId,
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
            product = rep.GetById(itemId);
            
            return View(product);

        }

        [HttpGet]
        public ActionResult GetUsableGraphics()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<UsableGraphics>();
            var images = rep.Get();

            return PartialView("GetUsableGraphics",images);
        }
    }
}