using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ALLINSHOP.Models;
using PagedList;
using PagedList.Mvc;
using Type = ALLINSHOP.Models.Type;

namespace ALLINSHOP.Controllers
{
    public class HomeController : Controller
    {
        private AllInShopEntities db = new AllInShopEntities();



        //--- Home Page ---//
        public ActionResult Index(string currentFilter, string searchString)
        {
            searchString = currentFilter;

            ViewBag.CurrentFilter = searchString;

            var item = db.Heroes.OrderBy(po => po.Hero_Name);
            return View(item);
        }
        //--- Home Page ---//



        //--- Admin Login ---//
        [HttpGet]
        public ActionResult Admin_Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin_Login(Admintrator admintrator)
        {

            admintrator = db.Admintrators.Where(x => x.Admin_Name == admintrator.Admin_Name && x.Admin_Pa == admintrator.Admin_Pa).FirstOrDefault();
            if (admintrator == null)
            {
                var response = "Username Or Password Please Try Again";
                return Json(response);
            }
            else
            {
                var CheckAdmin = db.Admintrators.Where(m => m.Admin_ID == admintrator.Admin_ID && m.Admin_Pa == admintrator.Admin_Pa).FirstOrDefault();
                if (CheckAdmin != null) //Username & Password correct
                {
                    Session["AdminID"] = CheckAdmin.Admin_ID.ToString();
                    Session["AdminName"] = CheckAdmin.Admin_Name.ToString();
                    return RedirectToAction("UserDashBoard");
                }

                else
                {
                    var response = "Username Or Password Please Try Again";
                    return Json(response);
                }
            }

        }

        public ActionResult UserDashBoard()
        {
            if (Session["AdminID"] != null)
            {
                return RedirectToAction("Index", "Products");
            }
            else
            {
                return RedirectToAction("Admin_Login");
            }
        }
        //--- Admin Login ---//



        //---   Product   ---//
        public ActionResult Product_cards(string sortOrder, string currentSort, string currentFilter, string searchString, int? page, string id)
        {
            if (id != null)
            {
                searchString = id;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.HeroSort = sortOrder == "Hero" ? "Hero_desc" : "Hero";
            ViewBag.TypeSort = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.PriceSort = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                TempData["HoldSearch"] = searchString;
                TempData.Keep();
            }
            else
            {
                searchString = (string)TempData["HoldSearch"];
                TempData.Keep();
            }
            ViewBag.CurrentFilter = searchString;
            var product = from po in db.Products select po;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(po => po.PO_Name.Contains(searchString) || po.Hero.Hero_Name.Contains(searchString) || po.Type.Type_Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name_desc":
                    product = product.OrderByDescending(po => po.PO_Name);
                    break;

                case "Hero_desc":
                    product = product.OrderByDescending(po => po.Hero.Hero_Name);
                    break;
                case "Hero":
                    product = product.OrderBy(po => po.Hero.Hero_Name);
                    break;
                case "Type_desc":
                    product = product.OrderByDescending(po => po.Type.Type_Name);
                    break;
                case "Type":
                    product = product.OrderBy(po => po.Type.Type_Name);
                    break;

                case "Price_desc":
                    product = product.OrderByDescending(po => po.PO_Price);
                    break;
                case "Price":
                    product = product.OrderBy(po => po.PO_Price);
                    break;
                default:
                    product = product.OrderBy(po => po.PO_Name);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);

            ViewBag.Hero_ID = new SelectList(db.Heroes, "Hero_ID", "Hero_Name");
            return View(product.ToPagedList(pageNumber, pageSize));
        }

        /*Product Detials*/
        public ActionResult Product_Detials(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.Hero_ID = new SelectList(db.Heroes, "Hero_ID", "Hero_Name", product.Hero_ID);
            ViewBag.Type_ID = new SelectList(db.Types, "Type_ID", "Type_Name", product.Type_ID);
            return View(product);
        }
        /*Product Detials*/

        /*Product Hero*/
        public ActionResult Product_hero(string sortOrder, string currentSort, string currentFilter, string searchString, int? page, string id)
        {
            if (id != null)
            {
                searchString = id;
            }

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.HeroSort = sortOrder == "Hero" ? "Hero_desc" : "Hero";
            ViewBag.TypeSort = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.PriceSort = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                TempData["HoldSearch"] = searchString;
                TempData.Keep();
            }
            else
            {
                searchString = (string)TempData["HoldSearch"];
                TempData.Keep();
            }

            var product = from po in db.Products select po;

            if (!String.IsNullOrEmpty(searchString))
            {
                product = product.Where(po => po.PO_Name.Contains(searchString) || po.Hero.Hero_Name.Contains(searchString) || po.Type.Type_Name.Contains(searchString));
            }
            ViewBag.CurrentFilter = searchString;

            switch (sortOrder)
            {
                case "Name_desc":
                    product = product.OrderByDescending(po => po.PO_Name);
                    break;
                case "Hero_desc":
                    product = product.OrderByDescending(po => po.Hero.Hero_Name);
                    break;
                case "Hero":
                    product = product.OrderBy(po => po.Hero.Hero_Name);
                    break;
                case "Type_desc":
                    product = product.OrderByDescending(po => po.Type.Type_Name);
                    break;
                case "Type":
                    product = product.OrderBy(po => po.Type.Type_Name);
                    break;
                case "Price_desc":
                    product = product.OrderByDescending(po => po.PO_Price);
                    break;
                case "Price":
                    product = product.OrderBy(po => po.PO_Price);
                    break;
                default:
                    product = product.OrderBy(po => po.PO_Name);
                    break;
            }

            int pageSize = 30;
            int pageNumber = (page ?? 1);

            ViewBag.Hero_ID = new SelectList(db.Heroes, "Hero_ID", "Hero_Name");
            return View(product.ToPagedList(pageNumber, pageSize));

        }
        /*Product Hero*/

        //---   Product   ---//



        //--  Hero Page --//
        public ActionResult Hero_Page(string sortOrder, string currentSort, string currentFilter, string searchString)
        {

            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";

            if (searchString != null)
            {
                ViewBag.CurrentSort = sortOrder;
            }

            ViewBag.CurrentFilter = searchString;

            var item = from po in db.Heroes select po;

            if (!String.IsNullOrEmpty(searchString))
            {
                item = item.Where(po => po.Hero_Name.Contains(searchString));

            }

            switch (sortOrder)
            {
                case "Name_desc":
                    item = item.OrderByDescending(po => po.Hero_Name);
                    break;
                default:
                    item = item.OrderBy(po => po.Hero_Name);
                    break;
            }

            return View(item.ToList());
        }
        //--  Hero Page --//

        //-- Forum Page --//
        public ActionResult Form_Page()
        {
            return View();
        }

        //-- Forum Page --//

    }
}