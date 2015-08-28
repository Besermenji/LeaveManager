using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LeaveRequest
    {
        public int leaveRequestID { get; set; }

        [Display(Name = "Employee Name")]
        public virtual Employee employee { get; set; }

        [Display(Name = "All Day Event?")]
        public bool allDayEvent { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime startTime { get; set; }

        [Display(Name = "End Date:")]
        [DataType(DataType.Date)]
        public DateTime endTime { get; set; }

        [Display(Name = "Leave Reason")]
        public virtual LeaveReason leaveReason { get; set; }

        [Display(Name ="Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Display(Name ="Delivery Manager")]
        public virtual Employee deliveryManager { get; set; }

        [Display(Name = "Delivery Manager Status")]
        public virtual RequestStatus deliveryManagerStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Manager Comment")]
        public string deliveryManagerComment { get; set; }

        [Display(Name = "Department Manager")]
        public virtual Employee departmentManager { get; set; }


        [Display(Name = "Department Manager Status")]
        public virtual RequestStatus departmentManagerStatus { get; set; }


        [Display(Name = "Delivery Manager Comment")]
        public string departmentManagerComment { get; set; }

       

    }
}