using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LeaveRequestInfo
    {

        public int LeaveRequestInfoID { get; set; }

        public virtual Employee Employee { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime UpdateDate { get; set; }



    }
}
