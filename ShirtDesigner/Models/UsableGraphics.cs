/*
' Copyright (c) 2024 bakaJzspiG
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Content;
using System;
using System.Web.Caching;
using System.Web.UI.WebControls;

namespace bakaJzspiG.ShirtDesigner.ShirtDesigner.Models
{
    [TableName("ProductOrder")]
    [PrimaryKey("GraphicId", AutoIncrement = true)]
    [Cacheable("ProductOrder", CacheItemPriority.Default, 20)]
    [Scope("ModuleId")]
    public class UsableGraphics
    {
        public int GraphicId { get; set; }

        public string GraphicPic { get; set; }

    }

}
