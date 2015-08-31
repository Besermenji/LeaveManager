using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class DepartmentManagerLeaveRequestViewModel
    {

        public int leaveRequestID { get; set; }

        [Display(Name = "Employee Name")]
        public int employeeID { get; set; }
        [Display(Name = "Employee Name")]
        public virtual Employee employee { get; set; }

        [Display(Name = "All Day Event?")]
        public bool allDayEvent { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime startTime { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime endTime { get; set; }

        [Display(Name = "Leave Reason")]
        public int leaveReasonID { get; set; }
        public virtual LeaveReason leaveReason { get; set; }

        [Display(Name = "Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }

        [Display(Name = "Department Manager")]
        public int departmentManagerID { get; set; }
        [Display(Name = "Department Manager")]
        public virtual Employee departmentManager { get; set; }

        [Display(Name = "Department Manager Status")]
        public int departmentManagerStatusID { get; set; }
        [Display(Name = "Department Manager Status")]
        public virtual RequestStatus departmentManagerStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Department Manager Comment")]
        public string departmentManagerComment { get; set; }







    }
}