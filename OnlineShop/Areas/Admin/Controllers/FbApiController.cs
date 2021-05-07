using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using OnlineShop.Areas.Admin.Models;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class FbApiController : Controller
    {
        // GET: Admin/FbApi
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetMyFacebookPageFeeds()
        {
            var NumberofFeeds = 100;
            string AccessToken = ConfigurationManager.AppSettings["Access_Token"];
            string PageId = ConfigurationManager.AppSettings["PageId"];

            FBPostsModel posts;
            string FeedRequestUrl = string.Concat("https://graph.facebook.com/v10.0/" + PageId + "/feed?limit=", NumberofFeeds, "&fields=id,message,created_time&access_token=", AccessToken);
            HttpWebRequest feedRequest = (HttpWebRequest)WebRequest.Create(FeedRequestUrl);
            feedRequest.Method = "GET";
            feedRequest.Accept = "application/json";
            feedRequest.ContentType = "application/json; charset=utf-8";
            WebResponse feedResponse = (HttpWebResponse)feedRequest.GetResponse();
            using (feedResponse)
            {
                using (var reader = new StreamReader(feedResponse.GetResponseStream()))
                {
                    String message = reader.ReadToEnd();
                    posts = JsonConvert.DeserializeObject<FBPostsModel>(message);
                }
            }
            return View(posts);
        }
    }

    
}