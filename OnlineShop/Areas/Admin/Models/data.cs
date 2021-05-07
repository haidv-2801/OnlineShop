using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShop.Areas.Admin.Models
{
    public class data
    {

        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("message")]
        public string message { get; set; }

        [JsonProperty("created_time")]
        public string created_time { get; set; }   
    }
}