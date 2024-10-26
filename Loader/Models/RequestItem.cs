using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class RequestItem
    {
        public int Rdid { get; set; }
        public int ItemId { get; set; }
        public int Rmid { get; set; }
        public int BranchId { get; set; }
        public int ReqQnty { get; set; }
        public string ItemName { get; set; }
        public string UnitOfMeasure { get; set; }
        public string BranchName { get; set; }
        public string StatusName { get; set; }
    }
}