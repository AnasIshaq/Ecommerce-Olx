using Ecommerce_Olx.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;


namespace Ecommerce_Olx.Controllers
{
    public class USERController : Controller
    {
        OlxEntities3 database = new OlxEntities3();

        // GET: USER


        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        [HttpGet]
        public ActionResult User_Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult User_Signup( HttpPostedFileBase file , Table_User user)
        {
            string path = UploadImg(file);
            if (path.Equals("-1"))
            {
                ViewBag.error = "AD";
            }
            else
            {
                    
            Table_User get = new Table_User();
            get.userr_NAME = user.userr_NAME;
            get.userr_CONTACT = user.userr_CONTACT;
            get.userr_EMAIL = user.userr_EMAIL;
            get.userr_PASSWORD = user.userr_PASSWORD;
            get.userr_IMAGE = path;
            database.Table_User.Add(get);
            database.SaveChanges();
            return RedirectToAction("User_Login");
            }
            return View();

        }
        [HttpGet]
        public ActionResult User_Login()
        {
            return View();

        }
        [HttpPost]
        public ActionResult User_Login(Table_User user)
        { 
            Table_User get = database.Table_User.Where(x => x.userr_EMAIL == user.userr_EMAIL && x.userr_PASSWORD == user.userr_PASSWORD).SingleOrDefault();
            if (get != null)
            {
                Session["user_name"] = get.userr_NAME;
                Session["user_id"] = get.userr_ID;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Incorrect Password Or Email";
            }
            return View();

        }
        public ActionResult User_Logout()
        {
            Session.RemoveAll();
            Session.Abandon();
           return RedirectToAction("Index");


        }
        [HttpGet]
        public ActionResult View_Category(int ? page)
        {

            if (TempData["cart"] != null)
            {
                float x = 0;
                List<Cart> li = TempData["cart"] as List<Cart>;
                foreach (var item in li)
                {
                    x += Convert.ToInt32(item.order_Sub_Total);

                }
                TempData["Total"] = x;
                TempData.Keep();
            }


                int pagesize = 6, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = database.Table_Category.Where(x => x.category_STATUS == 1).OrderByDescending(x => x.category_ID).ToList();
                IPagedList<Table_Category> category = list.ToPagedList(pageindex, pagesize);

                return View(category);
         

        }

        [HttpPost]
        public ActionResult View_Category(int? page , string search)
        {
            
            
                int pagesize = 6, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = database.Table_Category.Where(x => x.category_NAME.Contains(search)).OrderByDescending(x => x.category_ID).ToList();
                IPagedList<Table_Category> category = list.ToPagedList(pageindex, pagesize);

                return View(category);
            }
           
        [HttpGet]
        public ActionResult Add_Product()
        {   
          

            if (Session["user_id"] != null)
            {
                List<Table_Category> list = database.Table_Category.ToList();
                ViewBag.Category = new SelectList(list, "category_ID", "category_NAME");
            }
            else
            {
                return RedirectToAction("User_Login");
            }

            return View();
        }

        [HttpPost]

        public ActionResult Add_Product(Table_Product user , HttpPostedFileBase file)
        {
            List<Table_Category> list = database.Table_Category.ToList();
            ViewBag.Category = new SelectList(list, "category_ID", "category_NAME");
            
           
                string path = ProductImage(file);
                if (path.Equals("-1"))
                {
                    ViewBag.error = "Please Choose a File";
                }
                else
                {
                   Table_Product get = new Table_Product();
                    get.product_NAME = user.product_NAME;
                    get.product_DESCRIPTION = user.product_DESCRIPTION; 
                    get.product_PRICE = user.product_PRICE;
                    get.product_IMAGE = path;
                    get.category_ID_foreign_key = user.category_ID_foreign_key;
                    get.userr_ID_foreign_key = Convert.ToInt32(Session["user_id"].ToString());
                    database.Table_Product.Add(get);
                    database.SaveChanges();
                TempData["message"] = "Sucessfully Inserted";
                TempData.Keep();
                return RedirectToAction("Add_Product");

                

            }
            
            
            return View();
        }
        [HttpGet]
        public ActionResult View_Products(int ? page , int ? id)
        {

            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
           List<Table_Product> list = database.Table_Product.Where(x => x.category_ID_foreign_key == id).OrderByDescending(x => x.product_ID).ToList();
            IPagedList<Table_Product> pro = list.ToPagedList(pageindex , pagesize);
            return View(pro);
        }


        [HttpPost]
        public ActionResult View_Products(int? page,int? id , string search)
        {

            int pagesize = 6, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            List<Table_Product> list = database.Table_Product.Where(x => x.product_NAME.Contains(search) && x.category_ID_foreign_key == id).OrderByDescending(x => x.product_ID).ToList();
            IPagedList<Table_Product> pro = list.ToPagedList(pageindex, pagesize);
            return View(pro);
        }

        public ActionResult ViewDetail_Product(int ? id)
        {
            ViewDetail_Product detail = new ViewDetail_Product();

            Table_Product pro = database.Table_Product.Where(x=> x.product_ID == id).SingleOrDefault();

            detail.product_ID = pro.product_ID;
            detail.product_NAME = pro.product_NAME;
            detail.product_DESCRIPTION = pro.product_DESCRIPTION;
            detail.product_PRICE = pro.product_PRICE;
            detail.product_IMAGE = pro.product_IMAGE;


            Table_Category cat = database.Table_Category.Where(x=> x.category_ID == pro.category_ID_foreign_key).SingleOrDefault();

            detail.category_NAME = cat.category_NAME;


            Table_User user = database.Table_User.Where(x => x.userr_ID == pro.userr_ID_foreign_key).SingleOrDefault();

            detail.userr_NAME = user.userr_NAME;
            detail.userr_IMAGE = user.userr_IMAGE;
            detail.userr_CONTACT = user.userr_CONTACT;
            detail.userr_ID = user.userr_ID;
            

            return View(detail);
        }
        [HttpGet]
        public ActionResult MyProduct()
        {
            if (Session["user_id"] != null)
            {

                int userID = (int)Session["user_id"];
                List<Table_Product> list = database.Table_Product.Where(x => x.userr_ID_foreign_key == userID).ToList();


                return View(list);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        }

        [HttpPost]
        public ActionResult MyProduct( string search)
        {
            

                int userID = (int)Session["user_id"];
                List<Table_Product> list = database.Table_Product.Where(x => x.product_NAME.Contains(search) && x.userr_ID_foreign_key == userID).ToList();


                return View(list);
            
        }

        public ActionResult Delete_Product(int ? id)
        {
            Table_Product prod = database.Table_Product.Where(x => x.product_ID == id).SingleOrDefault();
            database.Table_Product.Remove(prod);
            database.SaveChanges();
            return RedirectToAction("View_Category");

        }

        public ActionResult MyProfile()
        {
            if (Session["user_id"] != null)
            {

                int userID = (int)Session["user_id"];
                List<Table_User> list = database.Table_User.Where(x => x.userr_ID == userID).ToList();


                return View(list);
            }
            else
            {
                return RedirectToAction("User_Login");
            }
        }

        [HttpGet]

        public ActionResult Add_To_Cart(int ? id)
        {

            Table_Product product = database.Table_Product.Where(x => x.product_ID == id).SingleOrDefault();
            return View(product);
        }

        List<Cart> li = new List<Cart>();

        [HttpPost]
        public ActionResult Add_To_Cart(int ? id , string quantity)
        {

            Table_Product product = database.Table_Product.Where(x => x.product_ID == id).SingleOrDefault();

            Cart cart = new Cart();
            cart.product_ID = product.product_ID;
            cart.product_NAME = product.product_NAME;
            cart.product_IMAGE = product.product_IMAGE;
            cart.product_PRICE = product.product_PRICE;
            cart.order_quantity = Convert.ToInt32( quantity);
            cart.order_Sub_Total = Convert.ToInt32(product.product_PRICE) * cart.order_quantity;

            if (TempData["cart"] == null)
            {
                li.Add(cart);
                TempData["cart"] = li;




            }
            else
            {
            List<Cart> li2 = TempData["cart"] as  List<Cart>;
                li2.Add(cart);
                TempData["cart"] = li2;

            }

            TempData.Keep();
            return RedirectToAction("View_Category");
        }

        [HttpGet]
        public ActionResult CheckOut()
        {
            TempData.Keep();
            return View();
        }

        [HttpPost]

        public ActionResult CheckOut(Table_Order o)
        {
            List<Cart> li = TempData["cart"] as List<Cart>;

            Table_Invoice invoice = new Table_Invoice();
            invoice.invoice_userr_ID_foreign_key = Convert.ToInt32(Session["user_id"].ToString());
            invoice.inovice_date = System.DateTime.Now;
            invoice.invoice_bill = Convert.ToInt32(TempData["Total"]);
            database.Table_Invoice.Add(invoice);
            database.SaveChanges();

            foreach (var item in li)
            {
                Table_Order order = new Table_Order();
                order.order_user_fk = Convert.ToInt32(Session["user_id"].ToString());
                order.order_invoice_fk = invoice.invoice_id;
                order.order_product_fk = item.product_ID;
                order.order_unit_price = Convert.ToInt32(item.product_PRICE);
                order.order_date = System.DateTime.Now;
                order.order_quantity = item.order_quantity;
                order.order_bill = Convert.ToInt32(TempData["Total"]);
                database.Table_Order.Add(order);
                database.SaveChanges();
                


            }
            TempData.Remove("Total");
            TempData.Remove("Cart");
            TempData["sucsess"] = "Order Sucessfull";
            TempData.Keep();
            return RedirectToAction("View_Category");
        }
        public string ProductImage(HttpPostedFileBase file)
        {
            string path = "-1";
            Random r = new Random();
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".jfif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/productimage/") + random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/productimage/" + random + Path.GetFileName(file.FileName);

                    }
                    catch (Exception)
                    {
                        ViewBag.error = "ads";

                    }


                }
                else
                {
                    Response.Write("<script> alert('Select a file)'</script>");
                }
            }
            else
            {
                Response.Write("<script> alert('Select a Valid File')'</script>");
            }
            return path;
        }
        public string UploadImg(HttpPostedFileBase file)
        {
            string path = "-1";
            Random r = new Random();
            int random = r.Next();
            if (file != null && file.ContentLength > 0 )
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".jfif") )
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/userImg/") + random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/userImg/" + random + Path.GetFileName(file.FileName);

                    }
                    catch (Exception)
                    {
                        ViewBag.error = "ads";
                        
                    }
                    
                }
                else
                {
                    Response.Write("<script> alert('Select a file)'</script>");
                }
            }
            else
            {
                Response.Write("<script> alert('Select a Valid File')'</script>");
            }
            return path;
        }


    }
}