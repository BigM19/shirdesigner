using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace Designer.Dnn.Designer.Models
{
    [TableName("DesignableProduct")]
    [PrimaryKey("ItemId", AutoIncrement = true)]
    [Cacheable("DesignableProduct", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class DesignableProducts
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public string ItemSize { get; set; }
        public int ItemPrice { get; set; }
        public string ItemPic { get; set; }
    }
}