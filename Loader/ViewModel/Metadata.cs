using Loader.EntityDataModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Loader.ViewModel
{
    public class Metadata
    {
        public class FgetStaffAuthorization_ResultMetaData
        {
            public int Said { get; set; }
            public Nullable<int> Employeeid { get; set; }
            public int userid { get; set; }
            public string EmployeeName { get; set; }
            public Nullable<int> LvlId { get; set; }
            public string LvlName { get; set; }
            public Nullable<int> EmpBranchId { get; set; }
            //public int BranchId { get; set; }
            public string BranchName { get; set; }
            public Nullable<int> DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public string BranchCode { get; set; }
            public Nullable<int> SupervisorId { get; set; }
            public string SupervisorName { get; set; }
            public Nullable<int> Status { get; set; }
            public string StatusName { get; set; }
            public int TypeId { get; set; }
            public string TypeName { get; set; }
            public int LocationId { get; set; }
            public string LocationName { get; set; }
        }

        public class FgetItemMasterList_ResultMetaData
        {
            public int ItemTypeId { get; set; }
            public string ItemTypeName { get; set; }
        }

        public class fgetCategoryMaster_ResultMetaData
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public Nullable<int> PAId { get; set; }
            public string FullCategoryName { get; set; }
            public Nullable<byte> LVL { get; set; }
            public Nullable<int> ParentLvl { get; set; }
            public string LVLdetails { get; set; }
            public string StatusName { get; set; }
            public string CategoryCode { get; set; }

            public List<fgetCategoryMaster_ResultMetaData> Children { get; set; }
        }

        public class StoreTransferDetailsMetaData
        {
            [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int STDId { get; set; }
            public int RMId { get; set; }
            public int STMId { get; set; }
            public int RDId { get; set; }
            public int Qnty { get; set; }
            public int StoreTrQnty { get; set; }
            public int PurpostypeId { get; set; }
            public int InvestmentTypeId { get; set; }
            public int BusinessUnitId { get; set; }
            public int TransferQnty { get; set; }
            public decimal Rate { get; set; }
            public string StoreRemarks { get; set; }
            public virtual RequestDetails RequestDetails { get; set; }
            public virtual ICollection<StoreTransferItemDetails> StoreTransferItemDetails { get; set; }

        }

        //public class RequestDetailsMetadata
        //{
        //    public int RDId { get; set; }
        //    public int RMId { get; set; }
        //    public int ItemId { get; set; }
        //    public int BranchId { get; set; }
        //    public int ReqQnty { get; set; }
        //    public int PurposeTypeId { get; set; }
        //    public int InvestmentCodeId { get; set; }
        //    public byte Status { get; set; }
        //    public int TRQnty { get; set; }
        //    public decimal Rate { get; set; }
        //    public int BusinessUnitId { get; set; }
        //    public string ItemName { get; set; }
        //    public string BUnitName { get; set; }
        //    public string StoreRemarks { get; set; }
        //    //public bool StockOut { get; set; }
        //}

        public class FgetStoreTransferItemPening_ResultMetadata
        {
            public int RdId { get; set; }
            public int RMID { get; set; }
            public int ItemId { get; set; }
            //public int ItemId { get; set; }
            public int UnitOfMeasureId { get; set; }
            public string ItemName { get; set; }
            public string UnitofMeasure { get; set; }
            public string CategoryName { get; set; }
            public string Modelname { get; set; }
            public string ManifactureName { get; set; }
            public int ReqQnty { get; set; }
            public int TrQnty { get; set; }
            public Nullable<int> RemQntyToTransfer { get; set; }
            public int Balance { get; set; }
            public Nullable<int> PurposeTypeId { get; set; }
            public string PurposeType { get; set; }
            public Nullable<int> InvestmentCodeId { get; set; }
            public string InvestmentCodeName { get; set; }
            public byte Status { get; set; }
            public string StatusName { get; set; }
            public Nullable<int> BusinessUnitId { get; set; }
            public string BUnitName { get; set; }
          
            public string StoreRemarks { get; set; }
           
        }

        public class DatePickerCalander
        {
            public DateTime startDate { get; set; }
            public DateTime endDate { get; set; }
        }

        public partial class ReturnItemDetails_SerialMacMetaData
        {
            public int ReMC { get; set; }
            public Nullable<int> RdId { get; set; }
            public string SerialNo { get; set; }
            public string MacNo { get; set; }
            public int IsTransferred { get; set; }
            public int ItemId { get; set; }
        }

        public partial class FgetPurchaseSerialNolist_ResultMetaData
        {
            public string Type { get; set; }
            public int PdId { get; set; }
            public string SerialNo { get; set; }
            public string MacNo { get; set; }
            public int Status { get; set; }
            public string StatusName { get; set; }
            public int ItemId { get; set; }


        }
            public partial class RequestMainMetaData
            {
                public int RMId { get; set; }
                public string ReqNo { get; set; }
                public int BranchId { get; set; }
                public string BranchName { get; set; }
                public int DepartmentId { get; set; }
                public string DepartmentName { get; set; }
                public int LocationId { get; set; }
                public string LocationName { get; set; }
                public int Status { get; set; }
                public string Remarks { get; set; }
                public int RequesteBy { get; set; }
                public DateTime PostedOn { get; set; }
                public int PostedBy { get; set; }
                public int ApprovedBy { get; set; }
                public DateTime ApprovedOn { get; set; }
                public int RejectedBy { get; set; }
                public DateTime RejectedOn { get; set; }
                public string RejectRemarks { get; set; }
                public int FyId { get; set; }
                public virtual ICollection<RequestDetails> RequestDetails { get; set; }
                public virtual ICollection<StoreTransferMain> StoreTransferMain { get; set; }

            }
            public partial class RequestDetailsMetaData
            {
                public int RDId { get; set; }
                public int RMId { get; set; }
                public int ItemId { get; set; }
                public int BranchId { get; set; }
                public int ReqQnty { get; set; }
                public int PurposeTypeId { get; set; }
                public int InvestmentCodeId { get; set; }
                public byte Status { get; set; }
                public int TRQnty { get; set; }
                public decimal Rate { get; set; }
                public int BusinessUnitId { get; set; }
                public string StoreRemarks { get; set; }
                public bool IsStockOut { get; set; }
            }
            public class DateMetaData
            {
                public DateTime From { get; set; }
                public DateTime To { get; set; }
            }

        public partial class StoreTransfer
        {
            public int SNo { get; set; }
            public string ItemName { get; set; }

            public string ItemCode { get; set; }
            public int Qnty { get; set; }
            public string StoreRemarks { get; set; }
            public int STDId { get; set; }
            public virtual ICollection<StoreTransferItemDetails> macSerialDetails { get; set; }
        }

        public partial class BasicTransferDetails
        {
            public string ReqNo { get; set; }
            public DateTime RequestedDate { get; set; }
            public DateTime TransferedDate { get; set; }
            public string RequestedBy { get; set; }
            public string TransferedBy { get; set; }
        }

        public partial class fgetInvSummary_ResultMetaData
        {
            public string ItemName { get; set; }
            public string CategoryName { get; set; }
            public string UnitName { get; set; }
            public Nullable<int> ReorderPoint { get; set; }
            public Nullable<int> InQnty { get; set; }
            public Nullable<int> OutQnty { get; set; }
            public Nullable<int> Balance { get; set; }
            public string InvStatus { get; set; }
            public Nullable<int> ItemId { get; set; }
            public Nullable<int> MaxStocklvl { get; set; }
            public Nullable<int> MinStocklvl { get; set; }

            public int TransferTo { get; set; }
            public int DepartmentId { get; set; }
            public int EmployeeId { get; set; }
            public string TransferToName { get; set; }
            public int Qnty { get; set; }
            public string Remarks { get; set; }

        }

        public partial class directConsumption_ResultMetaData
        {
            public int STMID { get; set; }
            public int StDid { get; set; }
            public int Stidid { get; set; }
            //public Nullable<int> ItemId { get; set; }
            public int ItemId { get; set; }
            public string ItemName { get; set; }
            public int Qnty { get; set; }
            public int Balance { get; set; }
            public decimal Rate { get; set; }
            public int TransferTo { get; set; }
            public int DepartmentId { get; set; }
            public int EmployeeId { get; set; }
            public string TransferToName { get; set; }
            public string Remarks { get; set; }
            public int postedby { get; set; }
            public int PdId { get; set; }
        }

        public class directConsumptionList_ResultMetaData
        {
            public int ItemId { get; set; }
            public int Balance { get; set; }
            public int TransferTo { get; set; }
            public int DepartmentId { get; set; }
            public int EmployeeId { get; set; }
            public string TransferToName { get; set; }
            public decimal Rate { get; set; }
            public string Remarks { get; set; } = null;
            public int Qnty { get; set; }
            public int PdId { get; set; }

            public List<directConsumption_ResultMetaData> directConsumptionList { get ; set; }
           
        }

    }
}