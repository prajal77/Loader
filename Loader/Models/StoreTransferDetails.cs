using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Drawing.Printing;

namespace Loader.Models
{
    public class StoreTransferDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STDId { get; set; }
        public int STMId { get; set; }
        public int RDId { get; set; }
        public int Qnty { get; set; }
        public int StoreTrQnty { get; set; }
        public string StoreRemarks { get; set; }
        public virtual ICollection<StoreTransferItemDetails> StoreTransferItemDetails { get; set; }
    }
}