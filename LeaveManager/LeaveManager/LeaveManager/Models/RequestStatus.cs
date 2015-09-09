using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class RequestStatus
    {
        public int requestStatusID { get; set; }
        [Display(Name = "Request Status")]
        public string requestStatusName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}