//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Loader.EntityDataModel
{
    using System;
    
    public partial class FgetStoreTransferItemPendingDetails_Result
    {
        public int RdId { get; set; }
        public int RMID { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public Nullable<int> UnitOfMeasureId { get; set; }
        public string UnitofMeasure { get; set; }
        public string CategoryName { get; set; }
        public string Modelname { get; set; }
        public string ManifactureName { get; set; }
        public int ReqQnty { get; set; }
        public int TrQnty { get; set; }
        public Nullable<int> RemQntyToTransfer { get; set; }
        public Nullable<int> Balance { get; set; }
        public Nullable<int> PurposeTypeId { get; set; }
        public string PurposeType { get; set; }
        public Nullable<int> InvestmentCodeId { get; set; }
        public string InvestmentCodeName { get; set; }
        public byte Status { get; set; }
        public string StatusName { get; set; }
        public Nullable<int> BusinessUnitId { get; set; }
        public string BUnitName { get; set; }
    }
}
