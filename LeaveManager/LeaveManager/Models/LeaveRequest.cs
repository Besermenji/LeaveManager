using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LeaveRequest
    {
        public int leaveRequestID { get; set; }
        
        public virtual int employeeID { get; set; }
        public virtual Employee employee { get; set; }

        public bool allDayEvent { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }

        public virtual int leaveReasonID { get; set; }
        public virtual LeaveReason leaveReason { get; set; }

        public string Description { get; set; }
        
        public virtual int deliveryManagerID { get; set; }
        public virtual Employee deliveryManager { get; set; }
        public bool deliveryManagerApproved { get; set; }
        [DataType(DataType.MultilineText)]
        public string deliveryManagerComment { get; set; }

        public virtual int departmentManagerID { get; set; }
        public virtual Employee departmentManager { get; set; }
        public bool departmentManagerApproved { get; set; }
        public string departmentManagerComment { get; set; }


        public virtual int requestStatusID { get; set; }
        public virtual RequestStatus requestStatus { get; set; }


    }
}