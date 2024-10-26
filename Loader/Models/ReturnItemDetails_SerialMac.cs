using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public partial class ReturnItemDetails_SerialMac
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReMC { get; set; }
        public Nullable<int> RdId { get; set; }
        public string SerialNo { get; set; }
        public string MacNo { get; set; }
        public int IsTransferred { get; set; }
    }
}