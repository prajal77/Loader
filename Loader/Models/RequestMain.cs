using Antlr.Runtime.Tree;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class RequestMain
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RMId { get; set; }
        public string ReqNo { get; set; }
        public int BranchId { get; set; }
        public int DepartmentId { get; set; }
        public int LocationId { get; set; }
        public int Status { get; set; }
        public string Remarks { get; set; }
        public int RequestedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int PostedBy { get; set; }
        public int ApprovedBy { get; set; }
        public DateTime ApprovedOn { get; set; }
        public Nullable<int> RejectedBy { get; set; }
        public Nullable<DateTime> RejectedOn { get; set; }
        public string RejectRemarks { get; set; }
        public int FyId { get; set; }
        public virtual ICollection<RequestDetails> RequestDetails { get; set; }
        public virtual ICollection<StoreTransferMain> StoreTransferMain { get; set; }
    }
}