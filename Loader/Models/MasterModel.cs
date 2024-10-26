using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class MasterModel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Mid { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public int Status { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set;}
    }
}