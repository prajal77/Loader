using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class CategoryMaster
    {
        [Key]
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public int Status { get; set; }
        public int Paid { get; set; }
        public int Lvl { get; set; }
        public string CategoryCode { get; set; }
    }
}