using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using System.Data.Linq;
using PMSMVC.Models;
using PMSMVC.ViewModels;
using System.Data.Linq.SqlClient;

namespace PMSMVC.Controllers
{
    public class SearchDoctorController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();
        public ActionResult Index()
        {
           
            return View();
        }
        // GET: SearchDoctor
        //Search Doctor By Speciality and Name
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult SearchDoctor(int specialityID, string doctorName)
        {
            List<SearchDoctorVm> Doctors = new List<SearchDoctorVm>();
            var doctorList = (
                            from d in db.Doctors
                            join sp in db.Specialities
                            on d.SpecialityID equals sp.SpecialityID
                            join q in db.Qualifications
                            on d.QualificationID equals q.QualificationID
                            select new { d.DoctorID, d.DoctorName, d.Gender, d.ContactNumber, d.DoctorAvatar, sp.SpecialityID, sp.SpecialityTag, q.QualificationID, q.QualificationName }
                           );

            if (!String.IsNullOrEmpty(doctorName))
            {
                doctorList = doctorList.Where(s => s.DoctorName.ToUpper().Trim().Contains(doctorName.ToUpper().Trim()));
            }

            if (specialityID != 0)
            {
                doctorList = doctorList.Where(s => s.SpecialityID == specialityID);
            }

            foreach (var item in doctorList)
            {
                SearchDoctorVm doctor = new SearchDoctorVm();
                doctor.DoctorID = item.DoctorID;
                doctor.DoctorName = item.DoctorName;
                doctor.Gender = item.Gender;
                doctor.ContactNumber = item.ContactNumber;
                doctor.DoctorAvatar = item.DoctorAvatar;
                doctor.SpecialityID = item.SpecialityID;
                doctor.SpecialityTag = item.SpecialityTag;
                doctor.QualificationID = item.QualificationID;
                doctor.QualificationName = item.QualificationName;
                Doctors.Add(doctor);
            }

            return Json(Doctors, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BookAppointment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment(Appointment appointment)
        {
            if (ModelState.IsValid)
            {
                Appointment newApmnt = new Appointment();
                newApmnt.ApmntName = appointment.ApmntName;
                newApmnt.ApmntPhone = appointment.ApmntPhone;
                newApmnt.ApmntPayment = appointment.ApmntPayment;
                newApmnt.ApmntDate = appointment.ApmntDate;
                newApmnt.DoctorID = appointment.DoctorID;
                newApmnt.ApmntMakeDate = DateTime.Now;
                newApmnt.Approved = null;
                db.Appointments.Add(newApmnt);
                db.SaveChanges();
            }
            var result = "Appointment Successull";
            return Json(result, JsonRequestBehavior.AllowGet);
        }



    }
}