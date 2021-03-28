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
        // GET: Cart1
        public ActionResult Index(int? page)
        {
            var cart = Session[Common.CommonConstants.CART_SESSION];
            var list = new List<CartItem>();

            int pageSize = 5;
            int pageNumber = (page ?? 1);

            if(cart != null)
            {
                list = cart as List<CartItem>;
            }
            return View(list.ToPagedList(pageNumber, pageSize));
        }
 
        public JsonResult AddToCart(long id, int quantity)
        {
            var cart = Session[Common.CommonConstants.CART_SESSION];
            var list = new List<CartItem>();
            if (cart != null)
            {
                list = cart as List<CartItem>;
                CartItem item = list.Where(x => x.Product.ID == id).FirstOrDefault();
                if(item == null)
                {
                    CartItem cartItem = new CartItem();
                    cartItem.Product = new ProductDao().ViewDetail(id);
                    cartItem.Quantity = quantity;
                    list.Add(cartItem);
                }
                else
                {
                    item.Quantity += quantity;
                }
                Session[Common.CommonConstants.CART_SESSION] = list;
            }
            else
            {
                CartItem cartItem = new CartItem();
                cartItem.Product = new ProductDao().ViewDetail(id);
                cartItem.Quantity = quantity;
                list.Add(cartItem); 
                Session[Common.CommonConstants.CART_SESSION] = list;
            }
            return Json(new { status = true, message = "Thêm thành công!" });

        }

        [HttpGet]
        public JsonResult Delete(long id)
        {
            var cart = Session[Common.CommonConstants.CART_SESSION] as List<CartItem>;
            cart.RemoveAll(x => x.Product.ID == id);
            Session[Common.CommonConstants.CART_SESSION] = cart;
            return Json(new {status=true, message="Xóa thành công!"}, JsonRequestBehavior.AllowGet);
        }

        [HttpDelete]
        public JsonResult DeleteAll()
        {
            Session[Common.CommonConstants.CART_SESSION] = null;
            return Json(new { status = true}, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateCart(string contentJson)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(contentJson);
            var sessionCart = (List<CartItem>)Session[Common.CommonConstants.CART_SESSION];
            if(sessionCart != null)
            {
                foreach(var cartItem in sessionCart)
                {
                    var item = jsonCart.FirstOrDefault(x => x.Product.ID == cartItem.Product.ID);
                    if(item != null)
                    {
                        cartItem.Quantity = item.Quantity;
                    }
                }
            }
            Session[Common.CommonConstants.CART_SESSION] = sessionCart;
            return Json(new { status=true,message="Cập nhật thành công!" });
        }

        [HttpPost]
        public JsonResult Payment(string shipName, string mobile, string address, string email)
        {
            var order = new Order();
            order.CreatedDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipMobile = mobile;
            order.ShipName = shipName;
            order.ShipEmail = email;

            try
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[Common.CommonConstants.CART_SESSION];
                var detailDao = new OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderDetail = new OrderDetail();
                    orderDetail.ProductID = item.Product.ID;
                    orderDetail.OrderID = id;
                    orderDetail.Price = item.Product.Price;
                    // tính theo promotion price khi code đã đầy đủ
                    orderDetail.Quantity = item.Quantity;
                    detailDao.Insert(orderDetail);
                    total += (item.Product.Price.GetValueOrDefault(0) * item.Quantity);
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/Assets/Client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipName);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                // email quản trị
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

              /*  new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);*/
            }
            catch (Exception ex)
            {
                string t = ex.Message;
                return Json(new { status = false, message = t } , JsonRequestBehavior.AllowGet);
            }
            return Json(new { status = true, message = "Đặt hàng thành công!" }, JsonRequestBehavior.AllowGet);
        }
    }
}