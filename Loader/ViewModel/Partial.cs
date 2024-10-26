using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static Loader.ViewModel.Metadata;

namespace Loader.ViewModel
{
   
    [MetadataType(typeof(ReturnItemDetails_SerialMacMetaData))]
    public partial class ReturnItemDetails_SerialMac
    {
        public int ItemId { get; set; }
    }

}