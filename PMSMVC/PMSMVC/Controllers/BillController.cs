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
    public class BillController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();

        // GET: Bill
        public ActionResult Index()
        {
            var bills = db.Bills.Include(b => b.Patient).Include(b => b.Staff);
            return View(bills.ToList());
        }

        // GET: Bill/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // GET: Bill/Create
        public ActionResult Create()
        {
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID");
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffCardID");
            return View();
        }

        // POST: Bill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Bill bill)
        {
            if (ModelState.IsValid)
            {
                bill.PrepareDate = DateTime.Now;
                db.Bills.Add(bill);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //var availablePatient = (from ap in db.Patients
            //                        where 
            //                        )
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", bill.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffCardID", bill.StaffID);
            return View(bill);
        }

        [Authorize(Roles ="Administrator")]
        // GET: Bill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", bill.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffCardID", bill.StaffID);
            return View(bill);
        }

        // POST: Bill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "BillID,BillNO,StaffID,PatientID,PrepareDate,DischargeDate,PathologyFee,DoctorFee,OtherFees,Discount")] Bill bill)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", bill.PatientID);
            ViewBag.StaffID = new SelectList(db.Staffs, "StaffID", "StaffCardID", bill.StaffID);
            return View(bill);
        }


        [Authorize(Roles = "Administrator")]
        // GET: Bill/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Bill bill = db.Bills.Find(id);
            if (bill == null)
            {
                return HttpNotFound();
            }
            return View(bill);
        }

        // POST: Bill/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Bill bill = db.Bills.Find(id);
            db.Bills.Remove(bill);
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
