using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class EmployeeRole
    {
        public int EmployeeRoleID { get; set; }
        public virtual Role Role { get; set; }
        public virtual Employee Employee { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}