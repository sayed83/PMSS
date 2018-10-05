using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using PMSMVC.Models;

namespace PMSMVC.Controllers
{
    public class HomeController : Controller
    {
        PmsmvcDBEntities db = new PmsmvcDBEntities();
        public ActionResult Index()
        {
            List<Speciality> SpecialityList = db.Specialities.ToList();
            ViewBag.SpecialityList = new SelectList(SpecialityList, "SpecialityID", "SpecialityTag");

            List<Doctor> DoctorList = db.Doctors.ToList();
            ViewBag.DoctorList = DoctorList;

            return View();
        }
        


    }
}