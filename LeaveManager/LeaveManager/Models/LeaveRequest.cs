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
        public virtual int employeeID { get; set; }
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
        public virtual int leaveReasonID { get; set; }
        public virtual LeaveReason leaveReason { get; set; }

        [Display(Name ="Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        
        [Display(Name ="Delivery Manager")]
        public virtual int deliveryManagerID { get; set; }
        public virtual Employee deliveryManager { get; set; }

        [Display(Name = "Approved by Delivery Manager?")]
        public bool deliveryManagerApproved { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Manager Comment")]
        public string deliveryManagerComment { get; set; }

        [Display(Name = "Department Manager")]
        public virtual int departmentManagerID { get; set; }
        public virtual Employee departmentManager { get; set; }

        [Display(Name = "Approved by Delivery Manager?")]
        public bool departmentManagerApproved { get; set; }

        [Display(Name = "Delivery Manager Comment")]
        public string departmentManagerComment { get; set; }

        [Display(Name = "Request Status")]
        public virtual int requestStatusID { get; set; }
        public virtual RequestStatus requestStatus { get; set; }


    }
}