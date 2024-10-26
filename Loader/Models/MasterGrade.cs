using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class MasterGrade
    {
        [Key]
        public int MgId { get; set; }
        public string GradeName { get; set; }
        public int StatusId { get; set; }
        //public string StatusName { get; set; }
        public int PostedBy { get; set; }
        public DateTime PostedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModofiedOn { get; set; } 
    }
}