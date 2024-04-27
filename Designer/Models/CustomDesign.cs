using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace Designer.Dnn.Designer.Models
{
    [TableName("CustomDesign")]
    [PrimaryKey("DesignId", AutoIncrement = true)]
    [Cacheable("CustomDesign", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class CustomDesign
    {
        public int DesignId { get; set; }
        public int OrderId { get; set; }
        public string DesignImg { get; set; }
    }
}