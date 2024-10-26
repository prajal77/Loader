using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class StaffAuthorizationViewModel
    {
        public int Said { get; set; }
        public int Employeeid { get; set; }
        public int userid { get; set; }
        public string EmployeeName { get; set; }
        public int LvlId { get; set; }
        public string LvlName { get; set;}
        public int EmpBranchId { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set;}
        public int Status { get; set; }
        public string StatusName { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set;}
    }
}