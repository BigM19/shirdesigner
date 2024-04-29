using Designer.Dnn.Designer.Models;
using DotNetNuke.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Designer.Dnn.Designer.Controllers
{
    public class UsableGraphicsController : Controller
    {
        public ActionResult GetUsableGraphics()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<UsableGraphics>();
            var images = rep.Get();

            return PartialView(images);
        }
    }
}