using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LoginViewModel
    {
       
            [Required]
            [Display(Name = "Email")]
            [EmailAddress]
            public string email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string password { get; set; }



          
        

    }
}