using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class MasterLocation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Lid { get; set; }
        public string LocationName { get; set; }
        public string LocationCode { get; set; }
        public int StatusId { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set;}
    }
}