using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace Designer.Dnn.Designer.Models
{
    [TableName("UsableGraphics")]
    [PrimaryKey("OrderId", AutoIncrement = true)]
    [Cacheable("UsableGraphics", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class ProductOrder
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public string Email { get; set; }
        public string SelectedSize { get; set; }
        public string DesignImg { get; set; }
    }
}