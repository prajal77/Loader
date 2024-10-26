using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class StoreTransferReportViewModel
    {
        public string ReqNo { get; set; }
        public string RequestDateAD { get; set; }
        public string RequestDateBS { get; set; }
        public string RequestedBy { get; set; }
        public string TransferBy { get; set; }
        public string TransferOn { get; set; }
        public string ItemName { get; set; }
        public int Qnty { get; set; }
        public string BranchName { get; set; }
        public string StoreRemarks { get; set; }
    }
}