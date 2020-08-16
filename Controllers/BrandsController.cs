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
    public class BrandsController : Controller
    {
        private LaptopsLLCEntities db = new LaptopsLLCEntities();

        // GET: Brands
        public ActionResult Index()
        {
            return View(db.Brands.ToList());
        }

        // GET: Brands/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // GET: Brands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Brands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Brands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }

        // POST: Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            db.Brands.Remove(brand);
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
        public ActionResult BrandsAndLaptops()
        {
            return View(db.Brands.ToList());
        }
        public ActionResult LaptopsInStockforBrand(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            var result = brand.Laptops.Where(l => l.QuantityAvailable > 0).ToList();
            if(result.Count == 0)
            {
                ViewBag.Message = "No laptop of this brand left.";
            }
            return View(result);
        }
        public ActionResult LaptopsInBrand()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult LaptopsInBrand(int BrandId)
        {
            Brand brand = db.Brands.Find(BrandId);
            if (brand == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name", brand.Id);
            return RedirectToAction("LaptopsInABrand", new { id = brand.Id});
        }
        public ActionResult LaptopsInABrand(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            return View(brand);
        }
        public ActionResult BuyThisLaptop(int? id)
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
            ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Email");
            return View(laptop);
        }
        [HttpPost]
        public ActionResult BuyThisLaptop(int id, int CustomerId,int Quantity)
        {
            Customer customer = db.Customers.Find(CustomerId);
            Laptop laptop = db.Laptops.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }

            var customerlaptop = customer.CustomerLaptops.SingleOrDefault(cl => cl.LaptopId == id);
            if (customerlaptop == null)
            {
                customerlaptop = new CustomerLaptop();
                customerlaptop.LaptopId = id;
                customerlaptop.CustomerId = CustomerId;
                customerlaptop.Quantity = Quantity;
            }
            else
            {
                customerlaptop.Quantity += Quantity;
            }
            customerlaptop.Cost = customerlaptop.Quantity * laptop.DollarValue;
            if (customer.Wallet < customerlaptop.Cost)
            {
                ViewBag.Message = "The customer does not have enough money";
                ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Email");
                return View(laptop);
            }
            else if (customerlaptop.Quantity > laptop.QuantityAvailable)
            {
                ViewBag.Message = "The laptop does not have enough stock";
                ViewBag.CustomerId = new SelectList(db.Customers, "Id", "Email");
                return View(laptop);
            }
            else
            {
                customer.Wallet -= customerlaptop.Cost;
                laptop.QuantityAvailable -= customerlaptop.Quantity;
                db.CustomerLaptops.Add(customerlaptop);
                db.SaveChanges();
                return RedirectToAction("PurchaseResult", customerlaptop);
            }
        }


        public ActionResult PurchaseResult(CustomerLaptop customerlaptop)
        {

            return View(db.CustomerLaptops.Find(customerlaptop.Id));
        }
    }
}
