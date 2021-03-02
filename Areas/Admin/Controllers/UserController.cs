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

namespace OnlineShop.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        // GET: Admin/User
        [HasCredential(RoleID = "VIEW_USER")]
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var dao = new UserDao();
            var model = dao.ListAllPaging(searchString, page, pageSize);
            // giữ thông tin seach và phân trang theo kết quả search nếu đã search ra
            // Ex :Admin,admin1,giang tìm theo admin có 2 kqua và phân trang theo 2 kqua đó
            // sdung viewBag để thêm tham số sẽ phân trang bên view
            ViewBag.SearchString = searchString;
            return View(model);
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
            var model = new UserDao().ViewDetail(id);
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
                long id = dao.Insert(u);
                if (id > 0)
                {
                    SetAlert("Thêm User Thành Công","success");
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
                var result = dao.Update(u);
                if (result)
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
            new UserDao().Delete(id);
            return RedirectToAction("Index");
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
            }) ;
        }
    }
}