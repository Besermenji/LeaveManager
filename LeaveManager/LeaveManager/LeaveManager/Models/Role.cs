using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class Role
    {
        public int roleID { get; set; }
        public string roleName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}