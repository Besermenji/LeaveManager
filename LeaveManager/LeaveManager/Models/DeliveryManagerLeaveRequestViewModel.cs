using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class DeliveryManagerLeaveRequestViewModel
    {

        public int DeliveryManagerLeaveRequestViewModelID { get; set; }

        [Display(Name = "Employee Name")]
        public int EmployeeID { get; set; }
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
        public int LeaveReasonID { get; set; }
        public virtual LeaveReason leaveReason { get; set; }

        [Display(Name = "Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Delivery Manager")]
        public int DeliveryManagerID { get; set; }
        public virtual Employee deliveryManager { get; set; }

        [Display(Name = "Delivery Manager Status")]
        public int deliveryManagerStatusID { get; set; }
        public virtual RequestStatus deliveryManagerStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Manager Comment")]
        public string deliveryManagerComment { get; set; }

        public int LeaveRequestId { get; set; }

    }
}