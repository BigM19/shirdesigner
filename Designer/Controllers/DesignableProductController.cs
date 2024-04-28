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
using System.Linq;
using System.Web.Mvc;

namespace Designer.Dnn.Designer.Controllers
{
    public class DesignableProductController : DnnController
    {
        public ActionResult Index()
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<DesignableProduct>();
            var products = rep
                .Find("")
                .ToArray();
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

    }
}