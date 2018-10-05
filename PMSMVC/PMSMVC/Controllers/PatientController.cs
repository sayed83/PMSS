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
    
    public class PatientController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();

        // GET: Patient
        public ActionResult Index()
        {
            var patients = db.Patients.Include(p => p.Room).Where(p=>p.IsDeleted!=true);
            return View(patients.ToList());
        }

        
        // GET: Patient/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            return View(patient);
        }

        // GET: Patient/Create
        public ActionResult Create()
        {
            var room = (from r in db.Rooms
                        where r.IsActive != false
                        select new { r.RoomID, r.RoomNO});

            ViewBag.RoomID = new SelectList(room, "RoomID", "RoomNO");
            return View();
        }

        // POST: Patient/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Patient patient)
        {
            
            if (ModelState.IsValid)
            {
                patient.AdmissionDate = DateTime.Now;
                db.Patients.Add(patient);
                db.SaveChanges();
            }

            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomNO", patient.RoomID);
            var result = "Save Successfully!";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Patient/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Patient patient = db.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomNO", patient.RoomID);
            return View(patient);
        }

        // POST: Patient/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PatientID,PatCardID,PatientName,Age,Gender,ContactNumber,AdmissionDate,RoomID")] Patient patient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(patient).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoomID = new SelectList(db.Rooms, "RoomID", "RoomNO", patient.RoomID);
            return View(patient);
        }

        [HttpPost]
        public ActionResult ApprovedAppointment(int AppointmentID)
        {
            bool result = false;
            Appointment apnmnt = db.Appointments.SingleOrDefault(a => a.AppoinmentID == AppointmentID);
            if(apnmnt != null)
            {
                apnmnt.Approved = true;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult RejectAppointment(int AppointmentID)
        {
            bool result = false;
            Appointment apnmnt = db.Appointments.SingleOrDefault(a => a.AppoinmentID == AppointmentID);
            if(apnmnt != null)
            {
                apnmnt.Approved = false;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeletePatient(int PatientID)
        {
            bool result = false;
            Patient pat = db.Patients.SingleOrDefault(p => p.PatientID == PatientID && p.IsDeleted == false);
           if(pat != null)
            {
                pat.IsDeleted = true;
                db.SaveChanges();
                result = true;
            }
            return Json(result, JsonRequestBehavior.AllowGet);
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
