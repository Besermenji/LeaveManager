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
        public virtual LeaveReason LeaveReason { get; set; }

        [Display(Name = "Leave Reason Description")]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Delivery Manager")]
        public virtual Employee DeliveryManager { get; set; }

        [Display(Name = "Delivery Manager Status")]
        public virtual RequestStatus DeliveryManagerStatus { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Delivery Manager Comment")]
        public string DeliveryManagerComment { get; set; }

        public int LeaveRequestInfoID { get; set; }

    }
}