using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

//Add by Developer
using PMSMVC.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace PMSMVC.ViewModels
{
    public class PatientVm
    {
        public int PatientID { get; set; }
        public string PatCardID { get; set; }
        public string PatientName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string ContactNumber { get; set; }
        public Nullable<System.DateTime> AdmissionDate { get; set; }
        public int RoomID { get; set; }
        public string RoomNO { get; set; }
        [NotMapped]
        public int RoomCategoryID { get; set; }
        [NotMapped]
        public string RoomCategory1 { get; set; }
    }
}