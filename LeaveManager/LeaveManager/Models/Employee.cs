using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class Employee
    {
        [Display(Name = "Employee")]
        public int employeeID { get; set; }
        public string employeeName
        {
            get
            {
                return string.Format("{0} {1}", employeeFirstName, employeeLastName);
            }
        }
        public string employeeFirstName { get; set; }
        public string employeeLastName { get; set; }
        
        public string employeeEmail { get; set; }
    }
}