using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Loader.ViewModel.Metadata;

namespace Loader.Models
{
    public class StoreTransferMain
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STMId { get; set; }
        public int RMId { get; set; }
        public int TransferBy { get; set; }
        public DateTime TransferOn { get; set; }
        public string AccRemarks { get; set; }
        public DateTime AccDate { get; set; }
        public int AccBy { get; set; }
        public string Remarks { get; set; }
        public string Email { get; set; }
        public virtual ICollection<StoreTransferDetailsMetaData> StoreTransferDetails { get; set; }
    }
}