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
        public int EmployeeID { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName
        {
            get
            {
                return string.Format("{0} {1}", EmployeeFirstName, EmployeeLastName);
            }
        }
        [Display(Name = "First Name")]
        [Required]
        public string EmployeeFirstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string EmployeeLastName { get; set; }
        [Display(Name = "Employee E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmployeeEmail { get; set; }
        [Display(Name = "Password")]
       
        public string PasswordHash { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}