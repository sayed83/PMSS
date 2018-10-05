using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PMSMVC.Models;
using System.IO;

using PMSMVC.CustomAuth;

namespace PMSMVC.Controllers
{
    public class DoctorController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();

        // GET: Doctor
        public ActionResult Index()
        {
            var doctors = db.Doctors.Include(d => d.Qualification).Include(d => d.Speciality);
            return View(doctors.ToList());
        }

        // GET: Doctor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        // GET: Doctor/Create
        public ActionResult Create()
        {
            ViewBag.QualificationID = new SelectList(db.Qualifications, "QualificationID", "QualificationName");
            ViewBag.SpecialityID = new SelectList(db.Specialities, "SpecialityID", "SpecialityTag");
            return View();
        }

        // POST: Doctor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( Doctor doctor)
        {
            if (ModelState.IsValid)
            {
                if (doctor.ImageUpload != null)
                {
                    string filename = Path.GetFileNameWithoutExtension(doctor.ImageUpload.FileName);
                    string extension = Path.GetExtension(doctor.ImageUpload.FileName);
                    filename = filename +"_"+DateTime.Now.ToString("yymmssfff") + extension;
                    doctor.DoctorAvatar = filename;
                    doctor.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images/DoctorsImg"), filename));
                    db.Doctors.Add(doctor);
                    db.SaveChanges();
                }
            }

            ViewBag.QualificationID = new SelectList(db.Qualifications, "QualificationID", "QualificationName", doctor.QualificationID);
            ViewBag.SpecialityID = new SelectList(db.Specialities, "SpecialityID", "SpecialityTag", doctor.SpecialityID);
            var result = "Doctor successfully Added!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        // GET: Doctor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            ViewBag.QualificationID = new SelectList(db.Qualifications, "QualificationID", "QualificationName", doctor.QualificationID);
            ViewBag.SpecialityID = new SelectList(db.Specialities, "SpecialityID", "SpecialityTag", doctor.SpecialityID);
            return View(doctor);
        }

        // POST: Doctor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Doctor doctor)
        {
            var result=false;
            if (ModelState.IsValid)
            {
                var doctors = db.Doctors.Where(d => d.DoctorID == doctor.DoctorID).FirstOrDefault();
                if (doctor.ImageUpload.ContentLength > 0)
                {
                    
                    // Delete exiting file
                    //System.IO.File.Delete(Path.Combine(Server.MapPath("~/AppFile/Images/DoctorsImg"), doctor.DoctorAvatar));
                    
                    // Save new file
                    string filename = Path.GetFileNameWithoutExtension(doctor.ImageUpload.FileName);
                    string extension = Path.GetExtension(doctor.ImageUpload.FileName);
                    filename = filename + "_" + DateTime.Now.ToString("yymmssfff") + extension;
                    doctors.DoctorAvatar = filename;
                    doctor.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/AppFile/Images/DoctorsImg"), filename));
                    
                    db.Entry(doctors).State = EntityState.Modified;
                    db.SaveChanges();
                    result = true;
                    return RedirectToAction("Index","Doctor");
                }
            }
            ViewBag.QualificationID = new SelectList(db.Qualifications, "QualificationID", "QualificationName", doctor.QualificationID);
            ViewBag.SpecialityID = new SelectList(db.Specialities, "SpecialityID", "SpecialityTag", doctor.SpecialityID);
           
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Doctor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctor doctor = db.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        [Authorize(Roles ="Administrator")]
        // POST: Doctor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Doctor doctor = db.Doctors.Find(id);
            db.Doctors.Remove(doctor);
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
