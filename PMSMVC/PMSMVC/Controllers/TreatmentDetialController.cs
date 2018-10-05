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
    public class TreatmentDetialController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();

        // GET: TreatmentDetial
        public ActionResult Index()
        {
            var treatmentDetials = db.TreatmentDetials.Include(t => t.Doctor).Include(t => t.Patient).Include(t => t.Treatment);
            return View(treatmentDetials.ToList());
        }

        // GET: TreatmentDetial/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentDetial treatmentDetial = db.TreatmentDetials.Find(id);
            if (treatmentDetial == null)
            {
                return HttpNotFound();
            }
            return View(treatmentDetial);
        }

        // GET: TreatmentDetial/Create
        public ActionResult Create()
        {
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorCardID");
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID");
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentName");
            return View();
        }

        // POST: TreatmentDetial/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TreatmentDetailID,TreatmentID,Description,DoctorID,PatientID")] TreatmentDetial treatmentDetial)
        {
            if (ModelState.IsValid)
            {
                db.TreatmentDetials.Add(treatmentDetial);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorCardID", treatmentDetial.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", treatmentDetial.PatientID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentName", treatmentDetial.TreatmentID);
            return View(treatmentDetial);
        }

        // GET: TreatmentDetial/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentDetial treatmentDetial = db.TreatmentDetials.Find(id);
            if (treatmentDetial == null)
            {
                return HttpNotFound();
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorCardID", treatmentDetial.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", treatmentDetial.PatientID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentName", treatmentDetial.TreatmentID);
            return View(treatmentDetial);
        }

        // POST: TreatmentDetial/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TreatmentDetailID,TreatmentID,Description,DoctorID,PatientID")] TreatmentDetial treatmentDetial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(treatmentDetial).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DoctorID = new SelectList(db.Doctors, "DoctorID", "DoctorCardID", treatmentDetial.DoctorID);
            ViewBag.PatientID = new SelectList(db.Patients, "PatientID", "PatCardID", treatmentDetial.PatientID);
            ViewBag.TreatmentID = new SelectList(db.Treatments, "TreatmentID", "TreatmentName", treatmentDetial.TreatmentID);
            return View(treatmentDetial);
        }

        // GET: TreatmentDetial/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TreatmentDetial treatmentDetial = db.TreatmentDetials.Find(id);
            if (treatmentDetial == null)
            {
                return HttpNotFound();
            }
            return View(treatmentDetial);
        }
        [Authorize(Roles ="Administrator")]
        // POST: TreatmentDetial/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TreatmentDetial treatmentDetial = db.TreatmentDetials.Find(id);
            db.TreatmentDetials.Remove(treatmentDetial);
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
