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
        public int leaveRequestID { get; set; }
        [Display(Name = "Employee")]
        public int employeeID { get; set; }
        [Display(Name = "Employee")]
        public string employeeName { get; set; }
        [Display(Name = "Employee")]
        public virtual Employee employee { get; set; }
        [Display(Name = "Is All Day Event")]
        public bool allDayEvent { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime startTime { get; set; }
        [Display(Name = "End Date")]
        public DateTime endTime { get; set; }

        [Display(Name = "Leave Reason")]
        public int leaveReasonID { get; set; }
        [Display(Name = "Leave Reason")]
        public virtual LeaveReason leaveReason { get; set; }

        public string Description { get; set; }
        [Display(Name = "Delivery Manager")]
        public int deliveryManagerID { get; set; }
        [Display(Name = "Delivery Manager")]
        public virtual Employee deliveryManager { get; set; }

        [Display(Name = "Delivery Manager")]
        public string deliveryManagerName { get; set; }
        [Display(Name = "Department Manager")]
        public int departmentManagerID { get; set; }
        [Display(Name = "Delivery Manager Comment")]
        public string deliveryManagerComment { get; set; }
        [Display(Name = "Department Manager")]
        public string departmentManagerName { get; set; }
        [Display(Name = "Department Manager")]
        public virtual Employee departmentManager { get; set; }
        [Display(Name = "Department Manager Comment")]
        public string departmentManagerComment { get; set; }

        [Display(Name = "Delivery Manager Status")]
        public virtual RequestStatus deliveryManagerStatus { get; set; }
        public virtual RequestStatus departmentManagerStatus { get; set; }

    }
}