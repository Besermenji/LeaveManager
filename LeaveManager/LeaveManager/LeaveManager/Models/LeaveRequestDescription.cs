using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LeaveRequestDescription
    {

        public int LeaveRequestDescriptionID { get; set; }
    
        public virtual LeaveRequestInfo LeaveRequestInfo { get; set; }
        
        public bool AllDayEvent { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

     
        public virtual LeaveReason LeaveReason { get; set; }
        public string Description { get; set; }
        
        public virtual Employee DeliveryManager { get; set; }
        public virtual RequestStatus DeliveryManagerStatus { get; set; }
        public string DeliveryManagerComment { get; set; }


        public virtual Employee DepartmentManager { get; set; }
        public virtual RequestStatus DepartmentManagerStatus { get; set; }
        public string DepartmentManagerComment { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}