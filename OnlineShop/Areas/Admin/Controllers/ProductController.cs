using OnlineShop.Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using System.Net.Http;
using PagedList;

namespace OnlineShop.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        // GET: Admin/Product
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            IEnumerable<Product> products;
            var respone = GlobalVariables.WebApiClient.GetAsync("Products?search=" + searchString);
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultRecord = result.Content.ReadAsAsync<IEnumerable<Product>>();
                resultRecord.Wait();
                products = resultRecord.Result.ToPagedList(page, pageSize);
                ViewBag.SearchString = searchString;
            }
            else
            {
                products = Enumerable.Empty<Product>();
                ModelState.AddModelError(string.Empty, "No record found!");
            }
            return View(products);
        }

        [HttpGet]
        public ActionResult Create()
        {
            SetViewBag();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Product model)
        {
            if (ModelState.IsValid)
            {
                var session = (UserLogin)Session[CommonConstants.USER_SESSION];
                model.CreatedBy = session.UserName;
                var culture = Session[CommonConstants.CurrentCulture];
                model.CreatedDate = DateTime.Now;
                model.Quantity = 0;
                model.IncludedVAT = false;

                var respone = GlobalVariables.WebApiClient.PostAsJsonAsync("Products", model);
                respone.Wait();
                var result = respone.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            SetViewBag();
            return View();
        }
        [HttpGet]
        public ActionResult Edit(long id)
        {
            Product model = null;
            var respone = GlobalVariables.WebApiClient.GetAsync("Products/" + id.ToString());
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode)
            {
                var resultRecord = result.Content.ReadAsAsync<Product>();
                resultRecord.Wait();
                model = resultRecord.Result;
            }
            SetViewBag(model.CategoryID);
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Product model)
        {
            if (ModelState.IsValid)
            {
                var respone = GlobalVariables.WebApiClient.PutAsJsonAsync("Products/" + model.ID, model);
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
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    ModelState.AddModelError("", "Cập Nhật Sản Phẩm K Thành Công");
                }
            }
            SetViewBag(model.CategoryID);
            return View();
        }
        [HttpDelete]
        public ActionResult Delete(long id)
        {
            var respone = GlobalVariables.WebApiClient.DeleteAsync("Products/" + id.ToString());
            respone.Wait();
            var result = respone.Result;
            if (result.IsSuccessStatusCode) return RedirectToAction("Index");
            else return Content("Not success");
        }
        [HttpPost]
        public JsonResult ChangeStatus(long id)
        {
            var result = new ProductDao().ChangeStatus(id);
            // trả ra đối tg có thuộc tính status
            return Json(new
            {
                status = result
            });
        }
        public JsonResult LoadImages(long id)
        {
            ProductDao dao = new ProductDao();
            var product = dao.ViewDetail(id);
            var images = product.MoreImages;
            // handling moreImages = null
            List<string> listImagesReturn = new List<string>();
            if (images == null)
            {
                listImagesReturn = null;
            }
            else
            {
                XElement xImages = XElement.Parse(images);
                foreach (XElement element in xImages.Elements())
                {
                    listImagesReturn.Add(element.Value);
                }
            }
            return Json(new
            {
                data = listImagesReturn
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SaveImages(long id, string images)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var listImages = serializer.Deserialize<List<string>>(images);

            XElement xElement = new XElement("Images");

            foreach (var item in listImages)
            {
                var subStringItem = item.Substring(23);
                xElement.Add(new XElement("Image", subStringItem));
            }
            ProductDao dao = new ProductDao();
            try
            {
                dao.UpdateImages(id, xElement.ToString());
                return Json(new
                {
                    status = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    status = false
                });
            }

        }
        public void SetViewBag(long? selectedId = null)
        {
            var dao = new ProductCategoryDao();
            ViewBag.CategoryID = new SelectList(dao.ListAll(), "ID", "Name", selectedId);
        }

    }
}