using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using bakaJzspiG.ShirtDesigner.ShirtDesigner.Models;
using bakaJzspiG.ShirtDesigner.ShirtDesigner.Components;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace bakaJzspiG.ShirtDesigner.ShirtDesigner.Controllers
{
    public class ProductGridController: DnnController
    {
        
        [HttpGet]
        public ActionResult Settings(Models.DesignableProduct product)
        {
            List<DesignableProduct> products = product.ToList();
            return View(products);
            ShirtDesigner.
        }

    }
}