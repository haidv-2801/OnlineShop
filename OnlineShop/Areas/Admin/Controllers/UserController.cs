using AutoMapper;
using Model.Dao;
using Model.EF;
using OnlineShop.Areas.Admin.Models;
using OnlineShop.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Net.Http;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            //var dao = new UserDao();
            IEnumerable<User> users;
            var respone = GlobalVariables.WebApiClient.GetAsync("Users?search=" + searchString);
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultRecord = result.Content.ReadAsAsync<IEnumerable<User>>();
                resultRecord.Wait();
                users = resultRecord.Result.ToPagedList(page, pageSize);
                ViewBag.SearchString = searchString;
            }
            else
            {
                users = Enumerable.Empty<User>();
                ModelState.AddModelError(string.Empty, "No record found!");
            }
            return View(users);
        }
        [HttpGet]   // đưa thông tin 
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create()
        {
            return View();
        }
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(int id)
        {
            User model = null;
            //var model = new UserDao().ViewDetail(id);
            var respone = GlobalVariables.WebApiClient.GetAsync("Users/" + id.ToString());
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultRecord = result.Content.ReadAsAsync<User>();
                resultRecord.Wait();
                model = resultRecord.Result;
            }
            else
            {
                ModelState.AddModelError(string.Empty, "No record found!");
            }
            return View(model);
        }
        [HttpPost]  // thông tin mới
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "ADD_USER")]
        public ActionResult Create(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                user.Password = encryptedMd5Pas;
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserModel, User>();
                });
                User u = config.CreateMapper().Map<User>(user);
                u.CreatedDate = DateTime.Now;
                //long id = dao.Insert(u);
                //
                var respone = GlobalVariables.WebApiClient.PostAsJsonAsync("Users", u);
                respone.Wait();
                var result = respone.Result;
                int id = 0;
                if (result.IsSuccessStatusCode)
                {
                    var resultRecord = result.Content.ReadAsAsync<int>();
                    resultRecord.Wait();
                    id = resultRecord.Result;
                }
                //
                if (id > 0)
                {
                    SetAlert("Thêm User Thành Công", "success");
                    return RedirectToAction("Index", "User");
                }
                else if (id == -1)
                {
                    ModelState.AddModelError("", "Trùng User");
                }
                else
                {
                    ModelState.AddModelError("", "Thêm User K Thành Công");
                }
            }
            return View("Index");
        }
        // EDIT USER HTTP POST
        [HttpPost]  // thông tin mới
        [ValidateAntiForgeryToken]
        [HasCredential(RoleID = "EDIT_USER")]
        public ActionResult Edit(UserModel user)
        {
            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                var dao = new UserDao();
                if (!string.IsNullOrEmpty(user.Password))
                {
                    var encryptedMd5Pas = Encryptor.MD5Hash(user.Password);
                    user.Password = encryptedMd5Pas;
                }
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<UserModel, User>();
                });
                User u = config.CreateMapper().Map<User>(user);
                //var result = dao.Update(u);

                var respone = GlobalVariables.WebApiClient.PutAsJsonAsync("Users/" + u.ID, u);
                respone.Wait();
                var result = respone.Result;
                bool stt = false;
                if (result.IsSuccessStatusCode)
                {
                    var resultRecord = result.Content.ReadAsAsync<bool>();
                    resultRecord.Wait();
                    stt = resultRecord.Result;
                }
                if (stt)
                {
                    SetAlert("Sửa User Thành Công", "success");
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhật User K Thành Công");
                }
            }
            return View("Index");
        }
        [HttpDelete]
        [HasCredential(RoleID = "DELETE_USER")]
        public ActionResult Delete(int id)
        {
            var respone = GlobalVariables.WebApiClient.DeleteAsync("Users/" + id.ToString());
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            else return Content("Not success");
        }
        [HttpPost]
        [HasCredential(RoleID = "EDIT_USER")]
        public JsonResult ChangeStatus(long id)
        {
            var result = new UserDao().ChangeStatus(id);
            // trả ra đối tg có thuộc tính status
            return Json(new
            {
                status = result
            });
        }
    }
}