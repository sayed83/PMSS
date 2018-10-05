using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//
using PMSMVC.Models;
using PMSMVC.ViewModels;

namespace PMSMVC.Controllers
{
    public class PatientViewModelController : Controller
    {
        private PmsmvcDBEntities db = new PmsmvcDBEntities();
        // GET: PatientViewModel
        public ActionResult Index()
        {
            //for cascading dropdown list
            List<RoomCategory> CategoryList = db.RoomCategories.ToList();
            ViewBag.CategoryList = new SelectList(CategoryList, "RoomCategoryID", "RoomCategory1");

            return View();
        }

        public JsonResult GetRoomNo(int categoryID)
        {
            //Result change base on cascading dropdown
            db.Configuration.ProxyCreationEnabled = false;
            List<Room> RoomList = db.Rooms.Where(c => c.RoomCategoryID == categoryID && c.IsActive == true).ToList();
            return Json(RoomList, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SavePatient(PatientVm patient)
        {
            bool result = false;
            if (ModelState.IsValid)
            {
                if(patient.RoomCategoryID != 0 && patient.RoomCategory1 ==null)
                {
                    Patient newPatient = new Patient();
                    newPatient.PatientName = patient.PatientName;
                    newPatient.Age = patient.Age;
                    newPatient.Gender = patient.Gender;
                    newPatient.ContactNumber = patient.ContactNumber;
                    newPatient.AdmissionDate = DateTime.Now;
                    newPatient.RoomID = patient.RoomID;
                    newPatient.IsDeleted = true;

                    db.Patients.Add(newPatient);
                    db.SaveChanges();
                }
                
            }

            result = true;
            
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}