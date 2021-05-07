using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class paging
    {
        [JsonProperty("cursors")]
        public cursors cursors { get; set; }
        [JsonProperty("next")]
        public string next { get; set; }
        [JsonProperty("previous")]
        public string previous { get; set; }
    }
}