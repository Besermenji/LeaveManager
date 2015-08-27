using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class DeliveryManagerLeaveRequestViewModel
    {
        [Key]
        public int lID { get; set; }

        public virtual int deliveryManagerID { get; set; }
        public virtual Employee deliveryManager { get; set; }
        public bool deliveryManagerApproved { get; set; }
        public string deliveryManagerComment { get; set; }

    }
}