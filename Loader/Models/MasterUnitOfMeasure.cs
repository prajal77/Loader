using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    [Table("MasterUnitOfMeasure", Schema = "LG")]
    public class MasterUnitOfMeasure
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UId { get; set; }
        public string UnitName { get; set; }
        public string USymbol { get; set; }
        public int Status { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set;}
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set;}
    }
}