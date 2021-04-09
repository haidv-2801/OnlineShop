using System;
using System.Collections.Generic;
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
            string AccessToken = "EAApYW8RNvfQBACSeaFeCpZAIDJMAupTxdYIxZCXMpHZB1FVlOF8zprlH9PzM52D5Nva1nK1CksYwUFRnAKbHZC2oNbNSpM6NRHCz1Gy0Io1D4E0joQ1rdqdOWIeZCY3cnZBiorm1gvDDslSkzmjqopYp8gMO3go8ZAtUfYYKUZAXCC2pOhkL2qXiitwfNuHRxwvjFZBWpp5lPn20J2abQZBW87EFTEvayashDBPzxAWXYQBAZDZD";
            string PageId = "112730510914245";

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
                    String message =reader.ReadToEnd();
                    posts = JsonConvert.DeserializeObject<FBPostsModel>(reader.ReadToEnd());
                }
            }
            return View(posts);
        }
    }

    
}