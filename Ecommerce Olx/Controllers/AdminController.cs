using Ecommerce_Olx.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ecommerce_Olx.Controllers
{
    public class AdminController : Controller
    {
        OlxEntities3 database = new OlxEntities3();
        // GET: Admin

        [HttpGet]
        public ActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Admin_Login(Table_Admin admin)
        {
            Table_Admin get = database.Table_Admin.Where(x => x.admin_EMAIL == admin.admin_EMAIL && x.admin_PASSWORD == admin.admin_PASSWORD).SingleOrDefault();
            if (get != null)
            {
                Session["admin_id"] = get.admin_ID; 
                Session["admin_name"] = get.admin_NAME;

                return RedirectToAction("Admin_Dashboard");


            }
            else
            {
                ViewBag.error = "Please Enter Valid Email Or Password";
            }
            return View();
        }

        public ActionResult Admin_Dashboard()
        {
            if (Session["admin_id"] != null)
            {

            }
            else
            {
                return RedirectToAction("Admin_Login");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add_Category()
        {
            if (Session["admin_id"] != null)
            {
            }
            else
            {
                return RedirectToAction("Admin_Login");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Add_Category(Table_Category user , HttpPostedFileBase file)
        {
            
                string path = UploadImg(file);
                if (path.Equals(-1))
                {
                    ViewBag.error = "Enter a Valid File";
                }
                else
                {
                    Table_Category get = new Table_Category();
                    get.category_NAME = user.category_NAME;
                    get.category_STATUS = 1;
                    get.category_IMAGE = path;
                    get.admin_ID_foriegn_key = Convert.ToInt32(Session["admin_id"].ToString());
                    database.Table_Category.Add(get);
                    database.SaveChanges();
                    return RedirectToAction("View_Category");
                }
            

            
            return View();
        }

        [HttpGet]
        public ActionResult View_Category(int ? page)
        {
            int pagesize = 6 , pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32 (page) : 1;
            var list = database.Table_Category.Where(x => x.category_STATUS == 1).OrderByDescending(x=> x.category_ID);
           IPagedList<Table_Category> cat = list.ToPagedList(pageindex, pagesize);
            
            return View(cat);
        }      
        [HttpPost]
        public ActionResult View_Category(int ? page , string search)
        {
            int pagesize = 6 , pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32 (page) : 1;
            var list = database.Table_Category.Where(x => x.category_NAME.Contains(search)).OrderByDescending(x=> x.category_ID);
           IPagedList<Table_Category> cat = list.ToPagedList(pageindex, pagesize);
            
            return View(cat);
        }  
        public ActionResult View_User()
        {
            if (Session["admin_id"] != null)
            {

            List<Table_User> list = database.Table_User.ToList();

            return View(list);
            }
            else
            {
                return RedirectToAction("Admin_Login");
            }



        }

        public ActionResult Display_Category_Records()
        {
            List<Table_Category> list = database.Table_Category.ToList();
            List<DisplayCategoryRecordsAdmin> fetchingdata = list.Select(x=> new DisplayCategoryRecordsAdmin
            {
             category_ID = x.category_ID,
             category_NAME = x.category_NAME,
             category_IMAGE = x.category_IMAGE,
             category_STATUS = x.category_STATUS,
             admin_ID = x.Table_Admin.admin_ID,
             admin_NAME = x.Table_Admin.admin_NAME,
              
            }).ToList();

            return View(fetchingdata);
        }


        [HttpGet]
        public ActionResult Cat_Edit(int ? id)
        {
            if (Session["admin_id"] != null)
            {
                Table_Category get = database.Table_Category.Where(x => x.category_ID == id).SingleOrDefault();
                return View(get);
            }
            else
            {
                return RedirectToAction("Admin_Login");

            }

        }
        [HttpPost]
        public ActionResult Cat_Edit(Table_Category user, int? id , HttpPostedFileBase file)
        {
            string path = UploadImg(file);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Enter Valid File";

            }
            else
            {
                    
           
                Table_Category get = database.Table_Category.Where(x => x.category_ID == id).SingleOrDefault();

                get.category_NAME = user.category_NAME;
                get.category_STATUS = user.category_STATUS;
                get.category_IMAGE = path;
                get.admin_ID_foriegn_key = Convert.ToInt32(Session["admin_id"].ToString());
                database.SaveChanges();
                return RedirectToAction("View_Category");
            }
            
            return View();
        }


      
        [HttpGet]
        public ActionResult Cat_Delete(int ? id)
        {
            Table_Category get = database.Table_Category.Where(x => x.category_ID == id).SingleOrDefault();
            return View(get);

        }
        [HttpPost]
        public ActionResult Cat_Delete( Table_User user ,int? id)
        {

            

                Table_Category get = database.Table_Category.Where(x => x.category_ID == id).SingleOrDefault();
                database.Table_Category.Remove(get);
                database.SaveChanges();
            return  RedirectToAction("View_Category");
            

        }

        public string UploadImg(HttpPostedFileBase file)
        {
            string path = "-1";
            Random r = new Random();
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {

                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".png") || extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".jfif"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Content/upload/") + random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);


                    }
                    catch (Exception ex)
                    {
                        ViewBag.error = ex;

                    }
                }
                else
                {

                    Response.Write("<script> alert('Choose Right Extension'); </script> ");
                }
            }
            else
            {
                Response.Write("<script> alert('Choose a File'); </script> ");
            }

            return path;
        }
    }
}