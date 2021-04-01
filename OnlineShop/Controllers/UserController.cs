using Model.Dao;
using Model.EF;
using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BotDetect.Web.Mvc;
using OnlineShop.Common;
using Facebook;
using System.Configuration;
using System.Xml.Linq;

namespace OnlineShop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Login()
        {
            var cku = new LoginModel();
            if (Request.Cookies["username"] != null && Request.Cookies["password"] != null)
            {
                cku.UserName = Request.Cookies["username"].Value;
                ViewBag.Password = Request.Cookies["password"].Value;
            }
            return View(cku);
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                    Response.Cookies["CartCoockies"].Expires = DateTime.Now.AddYears(-1);
                    Response.SetCookie(Response.Cookies["CartCoockies"]);
                    if (model.RememberMe == true)
                    {
                        Response.Cookies["username"].Value = model.UserName;
                        Response.Cookies["password"].Value = model.Password;
                        Response.Cookies["username"].Expires = DateTime.Now.AddYears(1);
                        Response.Cookies["password"].Expires = DateTime.Now.AddYears(1);
                        Response.SetCookie(Response.Cookies["username"]);
                        Response.SetCookie(Response.Cookies["password"]);
                        
                    }
                    return RedirectToAction("Index", "Home");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài Khoản Không Tồn Tại");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài Khoản Đang Bị Khóa");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật Khẩu Không Đúng");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng");
                }
            }
            return View(model);
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên Đăng Nhập Đã Tồn Tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email Đã Tồn Tại");
                }
                else
                {
                    var user = new User();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreatedDate = DateTime.Now;
                    user.Status = true;
                    if (!string.IsNullOrEmpty(model.ProvinceID))
                    {
                        user.ProvinceID = int.Parse(model.ProvinceID);
                        user.DistrictID = int.Parse(model.DistrictID);
                    }
                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng Kí Thành Công";
                        // reset toàn bộ lại model rồi trả về
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng Kí Không Thành Công");
                    }
                }
            }
            return View(model);
        }
        public JsonResult LoadProvince()
        {
            //láy ra 1 ptu có tên "Root" r ms lấy nhiều
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Where(x => x.Attribute("type").Value == "province");
            var list = new List<ProvinceModel>();
            ProvinceModel province = null;
            foreach (var item in xElements)
            {
                province = new ProvinceModel();
                province.ID = int.Parse(item.Attribute("id").Value);
                province.Name = item.Attribute("value").Value;
                list.Add(province);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public JsonResult LoadDistrict(int provinceID)
        {
            //láy ra 1 ptu có tên "Root" r ms lấy nhiều
            var xmlDoc = XDocument.Load(Server.MapPath(@"~/Assets/Client/data/Provinces_Data.xml"));
            var xElements = xmlDoc.Element("Root").Elements("Item").Single(x => x.Attribute("type").Value == "province"
            && int.Parse(x.Attribute("id").Value) == provinceID);
            // tỉnh 

            var list = new List<DistrictModel>();
            DistrictModel district = null;
            foreach (var item in xElements.Elements("Item").Where(x => x.Attribute("type").Value == "district"))
            {
                district = new DistrictModel();
                district.ID = int.Parse(item.Attribute("id").Value);
                district.Name = item.Attribute("value").Value;
                district.ProvinceID = int.Parse(xElements.Attribute("id").Value);
                list.Add(district);
            }
            return Json(new
            {
                data = list,
                status = true
            });
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return Redirect("/");
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email"
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;
                var user = new User();
                user.Email = email;
                user.UserName = userName;
                user.Status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreatedDate = DateTime.Now;
                var resultInsert = new UserDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }
    }
}