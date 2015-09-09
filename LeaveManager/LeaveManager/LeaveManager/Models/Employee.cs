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
        [Display(Name = "Employee Name")]
        public string employeeName
        {
            get
            {
                return string.Format("{0} {1}", employeeFirstName, employeeLastName);
            }
        }
        [Display(Name = "First Name")]
        [Required]
        public string employeeFirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string employeeLastName { get; set; }
        [Display(Name = "Employee E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string employeeEmail { get; set; }
        [Display(Name = "Password")]
       
        public string passwordHash { get; set; }
        public DateTime CreateDate { get; set; }
    }
}