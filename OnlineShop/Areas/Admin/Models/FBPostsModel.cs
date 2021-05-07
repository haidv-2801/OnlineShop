using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    [Serializable]
    public class FBPostsModel
    {
        
        [JsonProperty("data")]
        public List<data> data { get; set; }
        [JsonProperty("paging")]
        public paging paging { get; set; }
    }
}