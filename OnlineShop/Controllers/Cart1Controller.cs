using OnlineShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Dao;
using PagedList;
using System.Web.Script.Serialization;
using Model.EF;
using System.Configuration;
using Common;

namespace OnlineShop.Controllers
{
    public class Cart1Controller : Controller
    {
        private const string CartCoockies = "CartCoockies";
        // GET: Cart1
        public ActionResult Index()
        {
            var list = new List<CartItem>();
            /*int pageSize = 5;
            int pageNumber = (page ?? 1);*/
            var cart = Request.Cookies[CartCoockies];
            if (cart != null)
            {
                var it = cart.Value.Split(',').ToList();

                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }

                }

            }
            return View(list);
        }
        public JsonResult AddToCart(long productId, int quantity)
        {
            var cart = Request.Cookies[CartCoockies];
            if (cart != null)
            {


                var list = new List<CartItem>();
                var it = cart.Value.Split(',').ToList();

                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }


                }
                if (list.Exists(x => x.Product.ID == productId))
                {
                    Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(-1);
                    Response.SetCookie(Response.Cookies[CartCoockies]);
                    foreach (var item in list)
                    {
                        if (item.Product.ID == productId)
                        {
                            item.Quantity += quantity;
                        }

                        Response.Cookies[CartCoockies].Value = Response.Cookies[CartCoockies].Value + "," + item.Product.ID + "-" + item.Quantity;
                        Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(1);
                        Response.SetCookie(Response.Cookies[CartCoockies]);
                    }
                }
                else
                {

                    Response.Cookies[CartCoockies].Value = Request.Cookies[CartCoockies].Value + "," + productId + "-" + quantity;
                    Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(1);
                    Response.SetCookie(Response.Cookies[CartCoockies]);
                }

            }
            else
            {
                Response.Cookies[CartCoockies].Value = productId + "-" + quantity;
                Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(1);
                Response.SetCookie(Response.Cookies[CartCoockies]);
            }
            //return RedirectToAction("Index");
            return Json(new { status = true, message = "Thêm thành công!" });
        }

        
        public JsonResult Delete(long id)
        {
            var cart = Request.Cookies[CartCoockies];
            if (cart != null)
            {
                var it = cart.Value.Split(',');
                var list = new List<CartItem>();
                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();//đối với mỗi sản phẩm ta sẽ cắt chuỗi tại "-" để lấy ra id và số lượng
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }


                }
                Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(-1);
                Response.SetCookie(Response.Cookies[CartCoockies]);
                list.RemoveAll(x => x.Product.ID == id);
                foreach (var item in list)
                {
                    Response.Cookies[CartCoockies].Value = Response.Cookies[CartCoockies].Value + "," + item.Product.ID + "-" + item.Quantity;
                    Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(1);
                    Response.SetCookie(Response.Cookies[CartCoockies]);

                }
            }
            return Json(new { status = true, message = "Xóa thành công!" });
        }

        [HttpDelete]
        public JsonResult DeleteAll()
        {
            Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(-1);
            Response.SetCookie(Response.Cookies[CartCoockies]);
            return Json(new { status = true, message = "Xóa thành công!" });
        }

       
        public JsonResult UpdateCart(string contentJson)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(contentJson);
            var cart = Request.Cookies[CartCoockies];
            if (cart != null)
            {
                var it = cart.Value.Split(',');
                var list = new List<CartItem>();
                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }


                }
                Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(-1);
                Response.SetCookie(Response.Cookies[CartCoockies]);
                foreach (var item in list)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.Product.ID == item.Product.ID);
                    if (jsonItem != null)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                    Response.Cookies[CartCoockies].Value = Response.Cookies[CartCoockies].Value + "," + item.Product.ID + "-" + item.Quantity;
                    Response.Cookies[CartCoockies].Expires = DateTime.Now.AddYears(1);
                    Response.SetCookie(Response.Cookies[CartCoockies]);

                }
            }
            return Json(new { status = true, message = "Cập nhật thành công!" });
        }
        [HttpGet]
        public ActionResult Payment()
        {
            var list = new List<CartItem>();
            var cart = Request.Cookies[CartCoockies];
            var user = Request.Cookies["username"];
            ViewBag.shipName = "";
            ViewBag.mobile = "";
            ViewBag.address = "";
            ViewBag.email = "";
            if (cart != null)
            {
                var it = cart.Value.Split(',').ToList();

                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }

                }
            }
            if (user != null)
            {
                var a = new UserDao().GetById(user.Value);
                var usermodel = a;
                ViewBag.shipName = usermodel.Name;
                ViewBag.mobile = usermodel.Phone;
                ViewBag.address = usermodel.Address;
                ViewBag.email = usermodel.Email;
            }
            return View(list);
        }
        public ActionResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipEmail = email;
            order.ShipName = shipName;
            try
            {
                var id = new OrderDao().Insert(order);
                var cart = Request.Cookies[CartCoockies];
                var list = new List<CartItem>();
                var it = cart.Value.Split(',').ToList();//Số sản phẩm có trong cart được cắt bởi dấu ","

                for (int i = 0; i < it.Count(); i++)
                {
                    if (it[i] != "")
                    {
                        var a = it[i].Split('-').ToList();//đối với mỗi sản phẩm ta sẽ cắt chuỗi tại "-" để lấy ra id và số lượng
                        var product = new ProductDao().ViewDetail(long.Parse(a[0]));
                        var item = new CartItem();
                        item.Product = product;
                        item.Quantity = int.Parse(a[1]);
                        list.Add(item);
                    }

                }
                var detailDao = new Model.Dao.OrderDetailDao();
                decimal total = 0;
                foreach (var item in list)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    total += item.Product.PromotionPrice.HasValue ? (item.Product.PromotionPrice.Value * item.Quantity) : (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("/Assets/Client/template/neworder.html"));
                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ LearningWeb", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ LearningWeb", content);
            }
            catch (Exception)
            {
                //ghi log

                return RedirectToAction("Success");

            }
            return RedirectToAction("Fail");
        }

        public ActionResult Success()
        {
            return View();
        }

        public ActionResult Fail()
        {
            return View();
        }
    }
}