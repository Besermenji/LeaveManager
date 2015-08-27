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
        [Display(Name = "Employe")]
        public virtual int employeeID { get; set; }
        [Display(Name = "Employe")]
        public virtual Employee employee { get; set; }
        [Display(Name = "Is All Day Event")]
        public bool allDayEvent { get; set; }
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Date")]
        public DateTime startTime { get; set; }
        [Display(Name = "End Date")]
        public DateTime endTime { get; set; }

        public virtual int leaveReasonID { get; set; }
        public virtual LeaveReason leaveReason { get; set; }

        public string Description { get; set; }

        [Display(Name = "Delivery Manager")]
        public virtual int deliveryManagerID { get; set; }
        [Display(Name = "Delivery Manager")]
        public virtual Employee deliveryManager { get; set; }

        [Display(Name = "Department Manager")]
        public virtual int departmentManagerID { get; set; }
        [Display(Name = "Department Manager")]
        public virtual Employee departmentManager { get; set; }

        public virtual int requestStatusID { get; set; }
        public virtual RequestStatus requestStatus { get; set; }

    }
}