using Loader.EntityDataModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class StoreTransfreViewModel
    {
        public int RMID { get; set; }
        public int RdId { get; set; }
        public int STMId { get; set; }
        public int UnitOfMeasureId { get; set; }
        public int PurposeType { get; set; }
        public int BusinessUnit { get; set; }
        public int InvestmentCodeId { get; set; }
        public int ReqQnty { get; set; }
        public int PdId { get; set; }
        public string SerialNo { get; set; }
        public string MacNo { get; set; }

        public IEnumerable<FgetStoreTransferItemPening_Result> RequestDetailsList { get; set; }
        public IEnumerable<StoreTransferItemDetails> ItemDetailsList { get; set; }
        public RequestMain requestMain { get; set; }
        public StoreTransferMain storeTrnsMain { get; set; }
    }
}