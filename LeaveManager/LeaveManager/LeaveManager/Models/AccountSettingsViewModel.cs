using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class AccountSettingsViewModel
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
       
        public string EmployeeFirstName { get; set; }
        [Display(Name = "Last Name")]
      
        public string EmployeeLastName { get; set; }
        [Display(Name = "Employee E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string EmployeeEmail { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [Required]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{7,15}$",ErrorMessage = " Password must be at least 7 characters, no more than 15 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        [Required]
        public string ConfirmPassword { get; set; }

    }
}