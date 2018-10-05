using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PMSMVC.Models;

namespace PMSMVC.ViewModels
{
    public class SearchDoctorVm
    {
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public string DoctorAvatar { get; set; }
        public int SpecialityID { get; set; }
        public string SpecialityTag { get; set; }
        public int QualificationID { get; set; }
        public string QualificationName { get; set; }
    }
}