using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ALLINSHOP.Models;
using PagedList;
using Type = ALLINSHOP.Models.Type;

namespace ALLINSHOP.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductsController : Controller
    {
        private AllInShopEntities db = new AllInShopEntities();



        //-- Index --//  
        // GET: Products
        [Route("/Products")]
        public ActionResult Index(string sortOrder, string currentSort, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSort = String.IsNullOrEmpty(sortOrder) ? "Name_desc" : "";
            ViewBag.HeroSort = sortOrder == "Hero" ? "Hero_desc" : "Hero";
            ViewBag.TypeSort = sortOrder == "Type" ? "Type_desc" : "Type";
            ViewBag.PriceSort = sortOrder == "Price" ? "Price_desc" : "Price";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
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

            int pageSize = 25;
            int pageNumber = (page ?? 1);

            return View(product.ToPagedList(pageNumber, pageSize));
        }
        //-- Index --//  



        //-- Admin --//
        /*
        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        */
        // Get: Account
        // -- Admin Register --//
        public ActionResult Admin_Regis()
        {
            ViewBag.Admin_ID = new SelectList(db.Admintrators.OrderBy(x => x.Admin_ID), "Admin_ID", "Admin_Name");
            return View();
        }
        [HttpPost]
        public ActionResult Admin_Regis(Admintrator admintrator)
        {

            if (ModelState.IsValid)
            {
                var Check = db.Admintrators.Where(x => x.Admin_Name == admintrator.Admin_Name).FirstOrDefault();
                if (Check == null)
                {
                    admintrator.Admin_ID = Guid.NewGuid();

                    db.Admintrators.Add(admintrator);
                    db.SaveChanges();
                    return Content(@"<body>
                       <script type='text/javascript'>
                        window.close();                  
                       </script>
                     </body> ");
                }
                else
                {
                    admintrator.errorMessage = "Error กรุณาลองใหม่";
                    return View("Admin_Regis", admintrator);
                }
            }
            else
            {
                admintrator.errorMessage = "Error กรุณาลองใหม่";
                return View("Admin_Regis", admintrator);
            }

        }
        //-- Admin Register --//
        //-- Admin --//



        //-- Product Detials --//
        public ActionResult Details(Guid? id)
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
            return View(product);
        }
        //-- Product Detials --//



        //--  Create Product  --//
        // GET: Products/Create
        public ActionResult Create()
        {
            ViewBag.Hero_ID = new SelectList(db.Heroes.OrderBy(x => x.Hero_Name), "Hero_ID", "Hero_Name");
            ViewBag.Type_ID = new SelectList(db.Types.OrderBy(x => x.Type_Name), "Type_ID", "Type_Name");
            return View();
        }
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase ImageFile)
        {
            if (ModelState.IsValid)
            {
                product.PO_ID = Guid.NewGuid();
                ImageFile.SaveAs(HttpContext.Server.MapPath("~/Images/") + product.PO_ID + ImageFile.FileName);
                product.PO_Pic = "~/Images/" + product.PO_ID + ImageFile.FileName; db.Products.Add(product);
                db.SaveChanges();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();                  
                       </script>
                     </body> ");
            }
            ViewBag.Hero_ID = new SelectList(db.Heroes.OrderBy(x => x.Hero_Name), "Hero_ID", "Hero_Name", product.Hero_ID);
            ViewBag.Type_ID = new SelectList(db.Types.OrderBy(x => x.Type_Name), "Type_ID", "Type_Name", product.Type_ID);
            db.SaveChanges();

            return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();                   
                       </script>
                     </body> ");
        }
        //--  Create Product  --//



        //--  Edit Product  --//
        // GET: Products/Edit/5
        public ActionResult Edit(Guid? id)
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
        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            using (var tran = db.Database.BeginTransaction())
            {

                var item = db.Products.Find(product.PO_ID);

                item.PO_Name = product.PO_Name;
                item.Hero_ID = product.Hero_ID;
                item.Type_ID = product.Type_ID;
                item.PO_Price = product.PO_Price;

                db.SaveChanges();
                tran.Commit();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();                   
                       </script>
                     </body> ");

            }
        }
        //--  Edit Product  --//



        //-- Delete Product --//
        public ActionResult Delete(Guid? id)
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
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(Guid id)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                var item = db.Products.Find(id);
                db.Products.Remove(item);
                db.SaveChanges();
                tran.Commit();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();                  
                       </script>
                     </body> ");
            }
        }
        // Delete Product //
        //-- Delete Product --//



        //-- Edit Hero --//
        // Hero List //
        public ActionResult EditHero(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Hero> heroes = null;
            heroes = db.Heroes.OrderBy(m => m.Hero_Name).ToPagedList(pageIndex, pageSize);

            return View(heroes);
        }
        [HttpPost]
        public ActionResult EditHero(Hero hero)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                var item = db.Heroes.Find(hero.Hero_ID);
                item.Hero_ID = hero.Hero_ID;
                item.Hero_Name = hero.Hero_Name;
                db.SaveChanges();
                tran.Commit();
                return RedirectToAction("EditHero", "Products");
            }
        }
        // Hero List //

        // Pop Edit Hero //
        public ActionResult PopHero(Guid? id)
        {

            using (AllInShopEntities db = new AllInShopEntities())
            {
                var hero = db.Heroes.Find(id);
                return View(hero);
            }

        }
        [HttpPost]
        public ActionResult PopHero(Hero hero)
        {
            using (var tran = db.Database.BeginTransaction())
            {

                var item = db.Heroes.Find(hero.Hero_ID);
                item.Hero_ID = hero.Hero_ID;
                item.Hero_Name = hero.Hero_Name;
                db.SaveChanges();
                tran.Commit();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();
                       </script>
                     </body> ");
            }
        }
        // Pop Edit Hero //

        // Delete Hero //
        public ActionResult DeleteHero(Guid? id)
        {
            Hero hero = db.Heroes.Find(id);

            return View(hero);
        }
        [HttpPost]
        public ActionResult DeleteHero(Guid id)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                var item = db.Heroes.Find(id);
                db.Heroes.Remove(item);
                db.SaveChanges();
                tran.Commit();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();                   
                       </script>
                     </body> ");
            }
        }
        // Delete Hero //
        // GET: Products/Delete/5

        // Create Hero //
        public ActionResult CreateHero()
        {
            ViewBag.Hero_ID = new SelectList(db.Heroes.OrderBy(x => x.Hero_Name), "Hero_ID", "Hero_Name");
            ViewBag.Type_ID = new SelectList(db.Types.OrderBy(x => x.Type_Name), "Type_ID", "Type_Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateHero(Hero hero, HttpPostedFileBase ImageFile)
        {

            if (ModelState.IsValid)
            {
                var CheckHero = db.Heroes.Where(x => x.Hero_Name == hero.Hero_Name || x.Hero_Pic == hero.Hero_Pic).FirstOrDefault();


                if (CheckHero == null)
                {
                    hero.Hero_ID = Guid.NewGuid();
                    ImageFile.SaveAs(HttpContext.Server.MapPath("~/Images/Hero-icon/") + ImageFile.FileName);
                    hero.Hero_Pic = "~/Images/Hero-icon/" + ImageFile.FileName;
                    db.Heroes.Add(hero);
                    db.SaveChanges();
                    return Content(@"<body>
                       <script type='text/javascript'>
                        window.close();                  
                       </script>
                     </body> ");
                }
                else
                {
                    hero.errorMessage = "ชื่อซ้ำ";
                    return View("CreateHero", hero);
                }
            }
            else
            {
                hero.errorMessage = "ชื่อซ้ำ";
                return View("CreateHero", hero);
            }

        }
        // Create Hero //
        //-- Edit Hero  --//



        //-- Edit Type --//
        // Type List //
        public ActionResult EditType(int? page)
        {
            int pageSize = 10;
            int pageIndex = 1;
            pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            IPagedList<Type> types = null;
            types = db.Types.OrderBy(m => m.Type_Name).ToPagedList(pageIndex, pageSize);

            return View(types);
        }
        [HttpPost]
        public ActionResult EditType(Type type)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                var item = db.Heroes.Find(type.Type_ID);
                item.Hero_ID = type.Type_ID;
                item.Hero_Name = type.Type_Name;
                db.SaveChanges();
                tran.Commit();
                return RedirectToAction("EditType", "Products");
            }
        }
        // Type List //

        // Create Type //
        public ActionResult CreateType()
        {
            ViewBag.Hero_ID = new SelectList(db.Heroes.OrderBy(x => x.Hero_Name), "Hero_ID", "Hero_Name");
            ViewBag.Type_ID = new SelectList(db.Types.OrderBy(x => x.Type_Name), "Type_ID", "Type_Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateType(Type type)
        {
            if (ModelState.IsValid)
            {
                var CheckType = db.Types.Where(x => x.Type_Name == type.Type_Name).FirstOrDefault();
                if (CheckType == null)
                {
                    type.Type_ID = Guid.NewGuid();
                    db.Types.Add(type);
                    db.SaveChanges();
                    return Content(@"<body>
                       <script type='text/javascript'>
                        window.close();                  
                       </script>
                     </body> ");
                }
                else
                {
                    type.errorMessage = "ชื่อซ้ำ";
                    return View("CreateType", type);
                }
            }
            else
            {
                type.errorMessage = "ชื่อซ้ำ";
                return View("CreateType", type);
            }
        }
        // Create Type //

        //  Pop Edit Type  //
        public ActionResult PopType(Guid? id)
        {

            using (AllInShopEntities db = new AllInShopEntities())
            {
                var type = db.Types.Find(id);
                return View(type);
            }

        }
        [HttpPost]
        public ActionResult PopType(Type type)
        {
            using (var tran = db.Database.BeginTransaction())
            {
                var item = db.Types.Find(type.Type_ID);
                item.Type_ID = type.Type_ID;
                item.Type_Name = type.Type_Name;
                db.SaveChanges();
                tran.Commit();
                return Content(@"<body>
                       <script type='text/javascript'>
                         window.close();
                       </script>
                     </body> ");
            }
        }
        //  Pop Edit Type  //
        // Edit Type //



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
