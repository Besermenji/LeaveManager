using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class MailSettings
    {
        public int MailSettingsID { get; set; }
        [Required]
        public string Host { get; set; }
        [Required]
        public int Port { get; set; }
        [Required]
        //[RegularExpression(@"^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$", ErrorMessage = "Please Enter valid e-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}