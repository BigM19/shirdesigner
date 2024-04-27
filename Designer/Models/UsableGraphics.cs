using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;

namespace Designer.Dnn.Designer.Models
{
    [TableName("UsableGraphics")]
    [PrimaryKey("GraphicId", AutoIncrement = true)]
    [Cacheable("UsableGraphics", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class UsableGraphics
    {
        public int GraphicId { get; set; }
        public string GraphicPic { get; set; }
    }
}