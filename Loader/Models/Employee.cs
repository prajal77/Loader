using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
   [Table("Employees",Schema ="LG")]
    public class Employee
    {

        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("EmployeeId")]
        [Display(Name = "Employee Select")]
        public int EmployeeId { get; set; }
        [Display(Name = "Employee Number")]
        public string EmployeeNo { get; set; }
        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Employee Name Is Required"), MaxLength(100), Column(TypeName = "nvarchar")]
        public string EmployeeName { get; set; }
        [Display(Name = "Designation")]
        public Nullable<int> DGId { get; set; }
        [Display(Name = "Department")]
        public Nullable<int> DeptId { get; set; }
        public int Religion { get; set; }
        public int Nationality { get; set; }
        [Display(Name = "Marital Status")]
        public Int16 MaritialStatus { get; set; }
        [Column(TypeName = "datetime2")]
        [Display(Name = "Date Of Join")]
        public DateTime DateOfJoin { get; set; }
        [Display(Name ="Branch")]
        public Nullable<int> BranchId { get; set; }
        public Int16 Gender { get; set; }
        public Int16 BloodGroup { get; set; }
        public byte[] Photo { get; set; }
        public int Initials { get; set; }
        public DateTime PostedOn { get; set; }
        public int PostedBy { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Int16 Status { get; set; }

    }
}