using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class RequestStatus
    {
        public int RequestStatusID { get; set; }
        public string RequestStatusName { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}