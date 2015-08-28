using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LeaveManager.Models
{
    public class LeaveManagerContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public LeaveManagerContext() : base("name=LeaveManagerContext")
        {
        }

        public System.Data.Entity.DbSet<LeaveManager.Models.Employee> Employees { get; set; }

        public System.Data.Entity.DbSet<LeaveManager.Models.Role> Roles { get; set; }
        
        public System.Data.Entity.DbSet<LeaveManager.Models.EmployeeRole> EmployeeRoles { get; set; }

        public System.Data.Entity.DbSet<LeaveManager.Models.LeaveReason> LeaveReasons { get; set; }

        public System.Data.Entity.DbSet<LeaveManager.Models.RequestStatus> RequestStatus { get; set; }

        public System.Data.Entity.DbSet<LeaveManager.Models.LeaveRequest> LeaveRequests { get; set; }

        public System.Data.Entity.DbSet<LeaveManager.Models.DeliveryManagerLeaveRequestViewModel> DeliveryManagerLeaveRequestViewModels { get; set; }

        // public System.Data.Entity.DbSet<LeaveManager.Models.DeliveryManagerLeaveRequestViewModel> DeliveryManagerLeaveRequestViewModels { get; set; }





    }
}
