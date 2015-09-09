using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class EmployeeRole
    {
        public int employeeRoleID { get; set; }
        public virtual int roleID { get; set; }
        public virtual Role role { get; set; }
        public virtual int employeeID { get; set; }
        public virtual Employee employee { get; set; }
    }
}