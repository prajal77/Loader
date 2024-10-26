using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.Models
{
    public class ReturnRequestDTO
    {
        public class SerialMac
        {
            public string SerialNo { get; set; }
            public string MacNo { get; set; }
        }

        public class Item
        {
            public int ItemId { get; set; }
            public int ReqQnty { get; set; }
            public string Remarks { get; set; }
            public int ReturnTypeId { get; set; }
            public List<SerialMac> SerialMacList { get; set; }
        }

        public class ReturnRequest
        {
            public string MainRemarks { get; set; }
            public int Lid { get; set; }
            public List<Item> ItemList { get; set; }
        }
    }
}