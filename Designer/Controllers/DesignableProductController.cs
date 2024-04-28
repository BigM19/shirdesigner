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
            DesignableProduct product = null;
            string connectionString = "Data Source=NAGYMATE\\SQLEXPRESS;Initial Catalog=MyDNNDatabase;Integrated Security=True"; // Update this with your actual connection string
            string query = "SELECT * FROM DesignableProducts WHERE ItemId = @ItemId";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ItemId", id);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        product = new DesignableProduct
                        {
                            ItemId = (int)reader["ItemId"],
                            ItemName = reader["ItemName"].ToString(),
                            ItemDescription = reader["ItemDescription"].ToString(),
                            ItemSize = reader["ItemSize"].ToString(),
                            ItemPrice = (int)reader["ItemPrice"],
                            ItemPic = reader["ItemPic"].ToString()
                        };
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new HttpStatusCodeResult(500, "Internal Server Error"); // Return a 500 error in case of exception
                }
            }

            if (product == null)
            {
                return HttpNotFound(); // Returns a 404 error if no product is found
            }

            return View(product);
        }

    }
}