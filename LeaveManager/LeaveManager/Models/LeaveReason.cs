using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace LeaveManager.Models
{
    public class LeaveReason
    {
        [Display(Name = "Leave Reason")]
        public int leaveReasonID { get; set; }
        [Display(Name ="Leave Reason")]
        public string leaveReasonName { get; set; }

    }
}