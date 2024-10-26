using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class RequestDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RDId { get; set; }
        public int RMId { get; set; }
        public int ItemId { get; set; }
        public int BranchId { get; set; }
        public int ReqQnty { get; set; }
        public int PurposeTypeId { get; set; }
        public int InvestmentCodeId { get; set; }
        public byte Status { get; set; }
        public int TRQnty { get; set; }
        public decimal Rate { get; set; }
        public int BusinessUnitId { get; set; }

    }
}