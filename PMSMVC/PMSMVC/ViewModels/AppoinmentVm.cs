using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMSMVC.ViewModels
{
    public class AppoinmentVm
    {
        public int AppoinmentID { get; set; }
        public string ApmntName { get; set; }
        public string ApmntPhone { get; set; }
        public decimal ApmntPayment { get; set; }
        public System.DateTime ApmntDate { get; set; }
        public int DoctorID { get; set; }
        public string DoctorName { get; set; }
    }
}