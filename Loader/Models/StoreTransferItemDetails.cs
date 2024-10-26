using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class StoreTransferItemDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int STIDId { get; set; }
        public int STDId { get; set; }
        public int IsTransfered { get; set; }
        public string SerialNo { get; set; }
        public string MacNo { get; set; }
        public int PdIId { get; set; }
    }
}