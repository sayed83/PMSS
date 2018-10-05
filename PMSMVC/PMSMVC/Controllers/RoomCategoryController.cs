using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMSMVC.Models;

namespace PMSMVC.Controllers
{
    public class RoomCategoryController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();

        // GET: RoomCategory
        public ActionResult Index()
        {
            return View(db.RoomCategories.ToList());
        }

        // GET: RoomCategory/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomCategory roomCategory = db.RoomCategories.Find(id);
            if (roomCategory == null)
            {
                return HttpNotFound();
            }
            return View(roomCategory);
        }

        // GET: RoomCategory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomCategory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RoomCategoryID,RoomCategory1,RoomFee")] RoomCategory roomCategory)
        {
            if (ModelState.IsValid)
            {
                db.RoomCategories.Add(roomCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomCategory);
        }

        // GET: RoomCategory/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomCategory roomCategory = db.RoomCategories.Find(id);
            if (roomCategory == null)
            {
                return HttpNotFound();
            }
            return View(roomCategory);
        }

        // POST: RoomCategory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RoomCategoryID,RoomCategory1,RoomFee")] RoomCategory roomCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomCategory);
        }

        // GET: RoomCategory/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomCategory roomCategory = db.RoomCategories.Find(id);
            if (roomCategory == null)
            {
                return HttpNotFound();
            }
            return View(roomCategory);
        }

        // POST: RoomCategory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomCategory roomCategory = db.RoomCategories.Find(id);
            db.RoomCategories.Remove(roomCategory);
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
    }
}
