using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Devoir3_DesrosiersAlex;
using System.IO;

namespace Devoir3_DesrosiersAlex.Controllers
{
    public class ProductsController : Controller
    {
        private NorthwindEntities db = new NorthwindEntities();
        string imageFile = "~/Content/Images/";
        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryName = new SelectList(db.Categories, "CategoryName", "CategoryName");
            var products = db.Products.Include(p => p.Categories).Include(p => p.Suppliers);
            ViewBag.ImageFile = imageFile;
            return View(products.ToList());
        }

        public ActionResult Index(String categoryName)
        {
            if (Session["userSession"] == null)
            {

                return RedirectToAction("Index", "Home");
            }
            ViewBag.ImageFile = imageFile;
            ViewBag.CategoryName = new SelectList(db.Categories, "CategoryName", "CategoryName");

            var products = db.Products.Where(db => db.Categories.CategoryName.Contains(categoryName)).Include(p => p.Categories).Include(p => p.Suppliers);
            return View(products.ToList());
        }


        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {

                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.Image = products.Photo;
            return View(products);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName");
            return View();
        }

        // POST: Products/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(HttpPostedFileBase ChoosedFile, [Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Products products)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (ChoosedFile != null)
                {

                    var fileName = Path.GetFileName(ChoosedFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    ChoosedFile.SaveAs(path);
            
                    products.Photo = fileName;
                    db.Products.Add(products);
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                {
                    products.Photo = null;
                    db.Products.Add(products);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

            }

            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.ImageFile = imageFile;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
            ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }

        // POST: Products/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(HttpPostedFileBase ChoosedFile, [Bind(Include = "ProductID,ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Photo")] Products products)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                if (ChoosedFile != null)
                {
                    var fileName = Path.GetFileName(ChoosedFile.FileName);
                    var fileExtension = Path.GetExtension(ChoosedFile.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/"), fileName);
                    ChoosedFile.SaveAs(path);
                    Session["xml_name"] = fileName;
                    Session["xml_path"] = path;
                    products.Photo = fileName;
                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    products.Photo = null;
                    db.Entry(products).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }


            
        
        ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", products.CategoryID);
        ViewBag.SupplierID = new SelectList(db.Suppliers, "SupplierID", "CompanyName", products.SupplierID);
            return View(products);
        }


        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Products products = db.Products.Find(id);
            if (products == null)
            {
                return HttpNotFound();
            }
            return View(products);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (Session["userSession"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
           
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult productAutoComplete()
        {
            string term = Request.QueryString["term"].ToLower();
            var result = from p in db.Products
                         where p.Categories.CategoryName.ToLower().Contains(term)
                         select p.Categories.CategoryName;
            return Json(result, JsonRequestBehavior.AllowGet);
        }


    }
}
