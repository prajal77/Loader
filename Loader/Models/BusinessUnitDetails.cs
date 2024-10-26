using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class BusinessUnitDetails
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BUnitId { get; set; }
        public string BUnitName { get; set; }
        public string BUnitCode { get; set; }
    }
}