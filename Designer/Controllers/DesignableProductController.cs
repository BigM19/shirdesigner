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
using System.Net.Mail;
using Hotcakes.Modules.Core.Admin.Configuration;
using System.Drawing.Imaging;
using System.Net;
using Hotcakes.Modules.Core.Models;
using Hotcakes.Commerce.Orders;
using Hotcakes.Commerce.Urls;
using Hotcakes.Commerce;
using Hotcakes.Commerce.Extensions;


namespace Designer.Dnn.Designer.Controllers
{
    public class DesignableProductController : DnnController
    {
        public ActionResult Index(string search = "")
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<DesignableProduct>();
            var products = rep.Get().Where(p => p.ItemName.ToLower().Contains(search.ToLower()));
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
        public ActionResult Detail(HttpPostedFileBase graphicFile, int Id)
        {
            IDataContext ctx = DataContext.Instance();

            if (graphicFile != null && graphicFile.ContentLength > 0)
            {
                var rep = ctx.GetRepository<UsableGraphics>();

                var fileName = Path.GetFileName(graphicFile.FileName);

                var path = Path.Combine(@"C:\DNN\DesktopModules\MVC\shirtdesigner\Designer\Assets\UsableGraphics\", fileName);

                graphicFile.SaveAs(path);

                UsableGraphics graphic = new UsableGraphics
                {
                    GraphicPic = fileName
                };

                try
                {
                    rep.Insert(graphic);
                    ctx.Commit();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }

            }

            var rep2 = ctx.GetRepository<DesignableProduct>();
            var product = rep2.GetById(Id);

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

        public string ConvertImageToBase64(string imagePath)
        {
            // Ensure the image file exists
            if (!System.IO.File.Exists(imagePath))
            {
                throw new FileNotFoundException("Image file not found.", imagePath);
            }

            byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return base64String;
        }

        [HttpPost]
        public ActionResult CreateOrder(int id, string imageData, string email, string selectedSize )
        {
            IDataContext ctx = DataContext.Instance();
            var rep = ctx.GetRepository<ProductOrder>();

            string fileName = Guid.NewGuid().ToString() + ".png";
            string folderPath = @"C:\DNN\DesktopModules\MVC\shirtdesigner\Designer\Assets\CreatedDesigns\";
            string filePath = Path.Combine(folderPath, fileName);

            // Converting base64 data to bytes
            if (!String.IsNullOrEmpty(imageData))
            {
                imageData = imageData.Substring(imageData.IndexOf(",") + 1); // Remove the header part of the data URL
                byte[] imageBytes = Convert.FromBase64String(imageData);

                System.IO.File.WriteAllBytes(filePath, imageBytes); // Save the image

                ProductOrder order = new ProductOrder
                {
                    ItemId = id,
                    Email = email,
                    SelectedSize = selectedSize,
                    DesignImg = fileName
                };

                try
                {
                    rep.Insert(order);
                    ctx.Commit();
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = ex.Message;
                }

                try
                {
                    var rep2 = ctx.GetRepository<DesignableProduct>();
                    var designedProduct = rep2.GetById(id);

                    var shippingCost = 1050;

                    string body = $@"
                        <html>
                        <head>
                            <title>Rendelés Részletei</title>
                        </head>
                        <body style='font-size: 16px;'>
                            <h2 style='font-size: 22px;'>Köszönjük megrendelésed!</h2>
                            <p>A termék a legyártást követően üzletünkben átvehető.</p>

                            <table style='width: 80%; border-collapse: collapse; background-color: #E4F0D0; border-radius: 5px;'>
                                <tr style='background-color: #C2D8B9; border: 1px solid #ddd; border-radius: 5px 5px 0 0;'>
                                    <td style='border-bottom: 1px solid #ddd; padding: 8px; border-radius: 5px 0 0 0; font-weight: bold;'>Részletek:</th>
                                    <td style='border-bottom: 1px solid #ddd; padding: 8px; border-radius: 0 5px 0 0;'> </td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px;'>Termék neve:</td>
                                    <td style='padding: 8px;'>{designedProduct.ItemName}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px;'>Termék mérete:</td>
                                    <td style='padding: 8px;'>{selectedSize}</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px;'>Termék ára:</td>
                                    <td style='padding: 8px;'>{designedProduct.ItemPrice} Ft</td>
                                </tr>
                                <tr>
                                    <td style='padding: 8px;'>Szállítási költség:</td>
                                    <td style='padding: 8px;'>{shippingCost} Ft</td>
                                </tr>
                                <tr style='background-color: #C2D8B9; border: 1px solid #ddd; border-radius: 0 0 5px 5px;'>
                                    <td style='padding: 8px; font-weight: bold; border-radius: 0 0 0 5px;'>Teljes ár:</td>
                                    <td style='padding: 8px; font-weight: bold; border-radius: 0 0 5px 0;'>{designedProduct.ItemPrice + shippingCost} Ft</td>
                                </tr>
                            </table>

                            <p>Rendelésed az alábbi email címen 24 órán belül visszamondható: shirtcraftbce@gmail.com</p>
                        </body>
                        </html>";


                    MailAddress from = new MailAddress("shirtcraftbce@gmail.com");
                    MailAddress to = new MailAddress(email);
                    MailMessage mail = new MailMessage(from, to);
                    mail.Subject = "Order Confirmation";
                    mail.IsBodyHtml = true;
                    mail.Body = body;
                    Attachment attachment = new Attachment(filePath);
                    mail.Attachments.Add(attachment);


                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com"; // Or read from web.config
                    smtp.Port = 587;                // Or read from web.config
                    smtp.EnableSsl = true;          // Or read from web.config
                    smtp.Credentials = new System.Net.NetworkCredential("shirtcraftbce@gmail.com", "ycwz djdr wxho gsxr"); // Or read from web.config

                    smtp.Send(mail);
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    ViewBag.Message = "Could not send email: " + ex.Message;
                }
            }

            return RedirectToAction("Index", "DesignableProduct");
        }

        //public void AddProductToCart(object sender, EventArgs e)
        //{

        //    // create a reference to the Hotcakes store
        //    var HccApp = HccAppHelper.InitHccApp();
        //    // get an instance of the product to add
        //    var p = HccApp.CatalogServices.Products.FindBySku("YOUR-SKU-TO-LOOK-UP");

        //    // set the quantity
        //    int qty = 1;

        //    // create a reference to the current shopping cart
        //    Order currentCart = HccApp.OrderServices.EnsureShoppingCart();

        //    // create a line item for the cart using the product
        //    LineItem li = HccApp.CatalogServices.ConvertProductToLineItem(p, new OptionSelections(), qty, HccApp);

        //    // add the line item to the current cart
        //    HccApp.AddToOrderWithCalculateAndSave(currentCart, li);

        //    // send the customer to the shopping cart page
        //    Response.Redirect(HccUrlBuilder.RouteHccUrl(HccRoute.Cart));

        //}

    }
}