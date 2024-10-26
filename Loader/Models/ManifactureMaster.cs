using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class ManifactureMaster
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Mid { get; set; }
        public string ManifatcureName { get; set; }
        public string ManifactureCode { get; set; }
        public int Status { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set;}
    }
}