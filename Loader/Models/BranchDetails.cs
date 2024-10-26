using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("BranchDetails", Schema = "LG")]
    public class BranchDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public int Status { get; set; }
    }
}