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
       
        public string employeeFirstName { get; set; }
        [Display(Name = "Last Name")]
      
        public string employeeLastName { get; set; }
        [Display(Name = "Employee E-mail")]
        [DataType(DataType.EmailAddress)]
        [Required]
        public string employeeEmail { get; set; }

        [Display(Name = "Old Password")]
        [DataType(DataType.Password)]
        [Required]
        public string oldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{7,15}$",ErrorMessage = " Password must be at least 7 characters, no more than 15 characters, and must include at least one upper case letter, one lower case letter, and one numeric digit.")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string newPassword { get; set; }
        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare("newPassword")]
        [Required]
        public string confirmPassword { get; set; }

    }
}