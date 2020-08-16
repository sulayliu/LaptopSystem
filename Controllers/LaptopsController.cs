using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LaptopsLLC;

namespace LaptopsLLC.Controllers
{
    public class LaptopsController : Controller
    {
        private LaptopsLLCEntities db = new LaptopsLLCEntities();

        // GET: Laptops
        public ActionResult Index()
        {
            return View(db.Laptops.ToList());
        }

        // GET: Laptops/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }

        // GET: Laptops/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Laptops/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Model,DollarValue,QuantityAvailable")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                db.Laptops.Add(laptop);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(laptop);
        }

        // GET: Laptops/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }

        // POST: Laptops/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Model,DollarValue,QuantityAvailable")] Laptop laptop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(laptop).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(laptop);
        }

        // GET: Laptops/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }

        // POST: Laptops/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Laptop laptop = db.Laptops.Find(id);
            db.Laptops.Remove(laptop);
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
        public ActionResult EditMake(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }
        [HttpPost]
        public ActionResult EditMake(int? id, string Model)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            laptop.Model = Model;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult EditQuantity(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }
        [HttpPost]
        public ActionResult EditQuantity(int? id, int QuantityAvailable)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            laptop.QuantityAvailable = QuantityAvailable;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult AddToBrand(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }
        [HttpPost]
        public ActionResult AddToBrand(int? id, string BrandName)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }

            if (BrandName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Brand brand = db.Brands.FirstOrDefault(b =>b.Name == BrandName);
            if(brand == null)
            {
                brand = new Brand();
                brand.Name = BrandName;
                db.Brands.Add(brand);
            }
            laptop.BrandId = brand.Id;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // Delete a Laptop
        public ActionResult DeleteLaptop(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Laptop laptop = db.Laptops.Find(id);
            if (laptop == null)
            {
                return HttpNotFound();
            }
            return View(laptop);
        }
        [HttpPost]
        public ActionResult DeleteLaptop(int id)
        {
            Laptop laptop = db.Laptops.Find(id);
            db.Laptops.Remove(laptop);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult StockLaptops()
        {
            ViewBag.LaptopId = new SelectList(db.Laptops, "Id", "Model");
            return View();
        }
        [HttpPost]
        public ActionResult StockLaptops(int LaptopId, int QuantityAvailable)
        {
            Laptop laptop = db.Laptops.Find(LaptopId);
            if (laptop == null)
            {
                return HttpNotFound();
            }

            laptop.QuantityAvailable = QuantityAvailable;
            db.SaveChanges();
            ViewBag.LaptopId = new SelectList(db.Laptops, "Id", "Model", laptop.Id);
            return RedirectToAction("Details", laptop);
        }
        public ActionResult CreateNewLaptop()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult CreateNewLaptop(int BrandId, string Model, int DollarValue, int QuantityAvailable)
        {
            Brand brand = db.Brands.Find(BrandId);
            if (brand == null)
            {
                return HttpNotFound();
            }
            Laptop laptop = new Laptop();
            laptop.Model = Model;
            laptop.DollarValue = DollarValue;
            laptop.QuantityAvailable = QuantityAvailable;
            laptop.BrandId = BrandId;
            db.Laptops.Add(laptop);
            db.SaveChanges();
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", brand.Id);
            return RedirectToAction("Index");
        }
    }
}
