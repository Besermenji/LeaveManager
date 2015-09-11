using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class DepartmentManagerLeaveRequestViewModel
    {

        public int LeaveRequestID { get; set; }

        [Display(Name = "Employee Name")]
        public int EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public virtual Employee Employee { get; set; }

        [Display(Name = "All Day Event?")]
        public bool AllDayEvent { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }

        [Display(Name = "Leave Reason")]
        public int leaveReasonID { get; set; }
        public virtual LeaveReason LeaveReason { get; set; }

        [Display(Name = "Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Department Manager")]
        public int DepartmentManagerID { get; set; }
        [Display(Name = "Department Manager")]
        public virtual Employee DepartmentManager { get; set; }

        [Display(Name = "Department Manager Status")]
        public int DepartmentManagerStatusID { get; set; }
        [Display(Name = "Department Manager Status")]
        public virtual RequestStatus DepartmentManagerStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Department Manager Comment")]
        public string DepartmentManagerComment { get; set; }







    }
}