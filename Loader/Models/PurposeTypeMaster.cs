using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class PurposeTypeMaster
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurposeTypeId { get; set; }
        public string PurposeType { get; set; }
    }
}