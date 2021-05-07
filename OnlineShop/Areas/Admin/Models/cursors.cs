using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class cursors
    {
        [JsonProperty("before")]
        public string before { get; set; }
        [JsonProperty("after")]
        public string after { get; set; }
    }
}