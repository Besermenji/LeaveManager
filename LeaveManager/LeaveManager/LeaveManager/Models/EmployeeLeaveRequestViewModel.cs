using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class EmployeeLeaveRequestViewModel
    {
        [Key]
        public int LeaveRequestID { get; set; }
        [Display(Name = "Employee")]
        public int EmployeeID { get; set; }
        [Display(Name = "Employee")]
        public string EmployeeName { get; set; }
        [Display(Name = "Employee")]
        public virtual Employee Employee { get; set; }
        [Display(Name = "Is All Day Event")]
        public bool AllDayEvent { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Leave Reason")]
        public int LeaveReasonID { get; set; }
        [Display(Name = "Leave Reason")]
        public virtual LeaveReason LeaveReason { get; set; }

        public string Description { get; set; }
        [Display(Name = "Delivery Manager")]
        public int DeliveryManagerID { get; set; }
        [Display(Name = "Delivery Manager")]
        public virtual Employee DeliveryManager { get; set; }

        [Display(Name = "Delivery Manager")]
        public string DeliveryManagerName { get; set; }
        [Display(Name = "Department Manager")]
        public int DepartmentManagerID { get; set; }
        [Display(Name = "Delivery Manager Comment")]
        public string DeliveryManagerComment { get; set; }
        [Display(Name = "Department Manager")]
        public string DepartmentManagerName { get; set; }
        [Display(Name = "Department Manager")]
        public virtual Employee DepartmentManager { get; set; }
        [Display(Name = "Department Manager Comment")]
        public string DepartmentManagerComment { get; set; }

        [Display(Name = "Delivery Manager Status")]
        public virtual RequestStatus DeliveryManagerStatus { get; set; }
        public virtual RequestStatus DepartmentManagerStatus { get; set; }

    }
}