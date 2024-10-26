using Loader.Repository;
using Loader.EntityDataModel;
using Loader.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static Loader.ViewModel.Metadata;
using Antlr.Runtime.Tree;
using System.Data.Entity.Validation;
using Loader.ViewModel;
using System.Data.SqlClient;
using BotDetect;
using System.IO;
using System.Web.UI;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Loader.Service
{
    public class TransferService
    {
        private GenericUnitOfWork uow = null;
        private bool IsRequestMainChanged = false;
        public TransferService()
        {
            uow = new GenericUnitOfWork();
        }

       public List<RequestMain> GetAll()
       {
            try
            {
                return uow.Repository<RequestMain>().GetAll().ToList();

            }catch (Exception)
            {
                throw;
            }
       }

        public RequestMain GetSingle(int RMId)
        {
            try
            {
                RequestMain requestMain = uow.Repository<RequestMain>().GetSingle(x => x.RMId == RMId);
                return requestMain;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmployeeId()
        {
            return uow.Repository<ApplicationUser>().GetSingle(x => x.Id == Loader.Models.Global.UserId).EmployeeId.ToString();
        }

        public FgetStaffAuthorization_Result GetStaffAuthorizationData(int? value)
        {
            string query = "select * from FgetStaffAuthorization()";
            var staffData = uow.Repository<FgetStaffAuthorization_Result>().SqlQuery(query).Where(x => x.Employeeid == value).SingleOrDefault();
            return staffData;
        }

        //Store Transfer List
        public IEnumerable<FgetPendinStoreTransfer_Result> GetAllStoreTransferList()
        { 
            try
            {
                var EmployeeId = GetEmployeeId();
                var staffAuth = GetStaffAuthorizationData((int?)Convert.ToUInt64(EmployeeId));
                var query = "select * from FgetPendinStoreTransfer("+ EmployeeId + ", "+ staffAuth.LvlId +", "+ staffAuth.TypeId +")";
                var queryList = uow.Repository<FgetPendinStoreTransfer_Result>().ExecWithStoreProcedure(query);
                return queryList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Store Transfer Pending List
        //public IEnumerable<FgetStoreTransferItemPening_Result> GetStoreTransferPendingList(int RMId)
        //{
        //    var query = "select * from [dbo].[FgetStoreTransferItemPening]("+ RMId +")";
        //    var storePendingList = uow.Repository<FgetStoreTransferItemPening_Result>().ExecWithStoreProcedure(query).ToList();
        //    return storePendingList;
        //}
        
        public IEnumerable<FgetStoreTransferItemPening_ResultMetadata> GetStoreTransferPendingList(int? RMId)
        {
            var query = "select * from [dbo].[FgetStoreTransferItemPening](" + RMId + ")";
            var storePendingList = uow.Repository<FgetStoreTransferItemPening_ResultMetadata>().ExecWithStoreProcedure(query).ToList();
            return storePendingList;
        }

        //List<FgetStoreTransferItemPening_ResultMetadata> RequestDetailsList, List<FgetPurchaseSerialNolist_ResultMetaData> itemDetailsList
        public IEnumerable<FgetStoreTransferItemPening_ResultMetadata> GetRquestDetails()
        {
            var query = "select * from [dbo].[FgetStoreTransferItemPendingDetails]()";
            var reqDetailsList = uow.Repository<FgetStoreTransferItemPening_ResultMetadata>().ExecWithStoreProcedure(query).ToList();
            return reqDetailsList;
        }

        //create store transfer

        public void SaveStoreTransfer(RequestMain reqMain)
        {
            var userId = Loader.Models.Global.UserId;
            string query = string.Empty;

            if (reqMain != null)
            {
                query = "exec PcreateSroreTransfermain " + reqMain.RMId + ", " + userId + ", '" + reqMain.Remarks + "'";
            }
            uow.ExecuteStoreProcedurecommand(query);
        }

        //
        public bool IsEditValid(int RMId)
        {
            return uow.Repository<StoreTransferMain>().FindBy(x => x.RMId == RMId).Any();
        }

        public List<StoreTransferDetailsMetaData> GetAllStoreTransfer(int RMId)
        {
            var ForRMId = uow.Repository<StoreTransferDetailsMetaData>().FindBy(x => x.RMId == RMId).Select(x => x.STMId);
            var deptTransferDetails = uow.Repository<StoreTransferDetailsMetaData>().FindBy(x => ForRMId.Contains(x.STMId) && x.StoreTrQnty < x.Qnty && x.RequestDetails.PurposeTypeId != 2).ToList();
            return deptTransferDetails;
        }

        public int GetStoreTransferQntyBySTDId(int STDId)
        {
            var ReqQnty = uow.Repository<StoreTransferDetailsMetaData>().FindBy(x => x.STDId == STDId && x.StoreTrQnty < x.Qnty && x.RequestDetails.PurposeTypeId != 2).ToList().FirstOrDefault().Qnty;
            var TrQnty = uow.Repository<StoreTransferDetailsMetaData>().FindBy(x => x.STDId == STDId && x.StoreTrQnty < x.Qnty && x.RequestDetails.PurposeTypeId != 2).ToList().FirstOrDefault().StoreTrQnty;
            if (TrQnty != null)
            {
                ReqQnty = ReqQnty - Convert.ToInt32(TrQnty);
            }
            return ReqQnty;
        }

        //for serial mac
        public List<StoreTransferItemDetails> GetAllMacSerialNo(int STDId)
        {
            return uow.Repository<StoreTransferItemDetails>().FindBy(x => x.STDId == STDId).ToList();
        }


        public List<StoreTransferItemDetails> GetAllSerialMacDetails(int STDId)
        {
            return uow.Repository<StoreTransferItemDetails>().FindBy(x => x.STDId == STDId).ToList();
        }


        public List<FgetPurchaseSerialNolist_Result> GetPurchaseSerialNoList()
        {
            return uow.Repository<FgetPurchaseSerialNolist_Result>().SqlQuery(" select * from FgetPurchaseSerialNolist()").ToList();
        }


        //store transfer reject
        public int StoreRejectItemRequestAndReturnRMId(int RdId)
        {
            var userId = Loader.Models.Global.UserId;
            var query = string.Empty;

            query = "exec PRejectPendingStoreTransferfromList " + RdId + ", " + userId + "";
            var res = uow.ExecuteStoreProcedure(query);
            return res;
        }

        public RequestMain GetSigle(int RMId)
        {
            RequestMain requestMain = uow.Repository<RequestMain>().GetSingle(x => x.RMId == RMId);
            //requestMain.BranchName= uow.Repository<BranchDetails>().GetSingle(x => x.BranchId == requestMain.BranchId).BranchName;
            //requestMain.DepartmentName = uow.Repository<Department>().GetSingle(x => x.DepartmentId == requestMain.DepartmentId).DeptName;
            //requestMain.LocationName = uow.Repository<MasterLocation>().GetSingle(x => x.Lid == requestMain.LocationId).LocationName;

            //foreach(var item in requestMain.RequestDetails)
            //{
            //    item.Item
            //item.ItemName = uow.Repository<Item>().GetSingle(x => x.ItemId == item.ItemId)?.ItemName;
            //}
            return requestMain;
        }


        public int IsValueChanged(int newInput, int old)
        {
            if(newInput == old)
            {
                return old;
            }
            else
            {
                IsRequestMainChanged = true;
                return newInput;
            }
        }

        public RequestMain IsRequestMainUpdated(RequestMain RM)
        {
            var dbRequestMain = GetSigle(RM.RMId);
            dbRequestMain.BranchId = IsValueChanged(Convert.ToInt32(RM.BranchId), Convert.ToInt32((int)dbRequestMain.BranchId));
            dbRequestMain.DepartmentId = IsValueChanged(Convert.ToInt32(RM.DepartmentId), Convert.ToInt32((int)dbRequestMain.DepartmentId));
            dbRequestMain.LocationId = IsValueChanged(Convert.ToInt32(RM.LocationId), Convert.ToInt32((int)dbRequestMain.LocationId));
            dbRequestMain.RequestedBy = IsValueChanged(RM.RequestedBy, dbRequestMain.RequestedBy);
            dbRequestMain.ApprovedBy = IsValueChanged(RM.ApprovedBy, dbRequestMain.ApprovedBy);

            //if(IsRequestMainChanged == true)
            //{
            //    dbRequestMain.ModifiedOn = DateTime.Now;
            //}

            IsRequestMainChanged = false;
            return dbRequestMain;
        }

        //RequestDetailsMetaData
        public RequestMain IsRequestDetailUpdated(FgetStoreTransferItemPening_Result RD, RequestMain RM)
        {
            var dbRequestDetail = uow.Repository<RequestDetails>().FindBy(x => x.RDId == RD.RdId).FirstOrDefault();

            RM.RequestDetails.Single(x => x.RDId == RD.RdId).ItemId = IsValueChanged(RD.ItemId, dbRequestDetail.ItemId);
            //RM.RequestDetails.Single(x => x.RDId == RD.RDId).BranchId = IsValueChanged(RD.BranchId, dbRequestDetail.BranchId);
            RM.RequestDetails.Single(x => x.RDId == RD.RdId).PurposeTypeId = IsValueChanged(RD.PurposeTypeId, dbRequestDetail.PurposeTypeId);
            RM.RequestDetails.Single(x => x.RDId == RD.RdId).BusinessUnitId = IsValueChanged(RD.BusinessUnitId, dbRequestDetail.BusinessUnitId);
            RM.RequestDetails.Single(x => x.RDId == RD.RdId).ReqQnty = IsValueChanged(RD.ReqQnty, dbRequestDetail.ReqQnty);
            RM.RequestDetails.Single(x => x.RDId == RD.RdId).TRQnty = IsValueChanged(RD.TrQnty, dbRequestDetail.TRQnty);

            return RM;
        }

        public bool IsStockSubmit(StoreTransfreViewModel viewModel)
        {
            if(viewModel == null)
            {
                return false;
            }

            foreach(var  item in viewModel.RequestDetailsList)
            {
                if(item == null || item.TrQnty != 0)
                {
                    return false;
                }
            }
            return true;
        }

        //create PcreateSroreTransfermain
        public int SaveStoreTransMain(FgetPendinStoreTransfer_Result storeTrans)
        {
            var EmployeeId = GetEmployeeId();
            var StaffInfo = GetStaffAuthorizationData(Convert.ToInt32(EmployeeId));

            string query = "[dbo].[PcreateSroreTransfermain] "+ storeTrans.RMId +", "+ StaffInfo.userid +", '"+ storeTrans.Remarks +"' ";

            int stmId = uow.ExecuteStoreProcedure(query);

            return stmId;
        }


        //public int SaveStoreTransferDetails(
        // FgetPendinStoreTransfer_Result storeTrans,
        // List<FgetStoreTransferItemPening_ResultMetadata> requestDetailsList,
        // List<FgetPurchaseSerialNolist_ResultMetaData> itemDetailsList)
        //{
        //    if (requestDetailsList == null || !requestDetailsList.Any())
        //        throw new ArgumentException("RequestDetailsList cannot be null or empty.");

        //    foreach (var requestDetail in requestDetailsList)
        //    {
        //        if (requestDetail == null)
        //            throw new InvalidOperationException("RequestDetailsList contains a null item.");

        //        if (requestDetail.TrQnty != 0)
        //        {
        //            var stmId = SaveStoreTransMain(storeTrans);
        //            if (stmId != 0)
        //            {
        //                string query = "[dbo].[PcreateStoreTransferDetails]";
        //                var parameters = new[]
        //                {
        //            new SqlParameter("@StmId", stmId),
        //            new SqlParameter("@RdId", requestDetail.RdId),
        //            new SqlParameter("@PurposeTypeId", requestDetail.PurposeTypeId),
        //            new SqlParameter("@InvestmentCodeId", requestDetail.InvestmentCodeId),
        //            new SqlParameter("@BusinessUnitId", requestDetail.BusinessUnitId),
        //            new SqlParameter("@TrQnty", requestDetail.TrQnty),
        //            new SqlParameter("@SomeValue", 0),
        //            new SqlParameter("@StoreRemarks", requestDetail.StoreRemarks)
        //        };
        //                int stdId = uow.ExecuteStoreProcedure(query, parameters);
        //                if (stdId != 0)
        //                {
        //                    foreach (var itemDetail in itemDetailsList)
        //                    {
        //                        string serialMacQuery = "[dbo].[PcreateStoreTransferItemDetails]";
        //                        var itemParameters = new[]
        //                        {
        //                    new SqlParameter("@StdId", stdId),
        //                    new SqlParameter("@PdId", itemDetail.PdId),
        //                    new SqlParameter("@SerialNo", itemDetail.SerialNo),
        //                    new SqlParameter("@MacNo", itemDetail.MacNo)
        //                };
        //                        var result = uow.ExecuteStoreProcedure(serialMacQuery, itemParameters);

        //                        if (result == 0)
        //                        {
        //                            // Handle the failure case if needed
        //                            return 0;
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return 1; // Return 1 if the process completes successfully
        //}

        //create StoreTransferDetails
        public int SaveStoreTransferDetails(FgetPendinStoreTransfer_Result storeTrans, FgetStoreTransferItemPening_ResultMetadata RequestDetailsList, FgetPurchaseSerialNolist_ResultMetaData itemDetail)
        {
            if (RequestDetailsList == null)
                throw new InvalidOperationException("RequestDetailsList contains a null item.");

            if (RequestDetailsList.TrQnty != 0)
            {
                var stmId = SaveStoreTransMain(storeTrans);
                if (stmId != 0)
                {
                    string query = "[dbo].[PcreateStoreTransferDetails] " + stmId + ", " + RequestDetailsList.RdId + ", " + RequestDetailsList.PurposeTypeId + ", " + RequestDetailsList.InvestmentCodeId + ", " + RequestDetailsList.BusinessUnitId + ", " + RequestDetailsList.TrQnty + ", " + 0 + ", '" + RequestDetailsList.StoreRemarks + "'";
                    int stdId = uow.ExecuteStoreProcedure(query);
                    if (stdId != 0)
                    {
                        string serialMacQuery = " [dbo].[PcreateStoreTransferItemDetails] " + stdId + ", " + itemDetail.PdId + ", '" + itemDetail.SerialNo + "', '" + itemDetail.MacNo + "' ";
                        var result = uow.ExecuteStoreProcedure(serialMacQuery);

                        return 1; // Assuming 1 indicates success
                    }
                }
            }

            return 0; // Return 0 if no transfers were processed

            //foreach (var requestDetail in RequestDetailsList)
            //{
            //    if (requestDetail == null)
            //        throw new InvalidOperationException("RequestDetailsList contains a null item.");

            //    if (requestDetail.TrQnty != 0)
            //    {
            //        var stmId = SaveStoreTransMain(storeTrans);
            //        if (stmId != 0)
            //        {
            //            string query = "[dbo].[PcreateStoreTransferDetails] " + stmId + ", " + requestDetail.RdId + ", " + requestDetail.PurposeTypeId + ", " + requestDetail.InvestmentCodeId + ", " + requestDetail.BusinessUnitId + ", " + requestDetail.TrQnty + ", " + 0 + ", '" + requestDetail.StoreRemarks + "'";
            //            int stdId = uow.ExecuteStoreProcedure(query);
            //            if (stdId != 0)
            //            {
            //                foreach (var itemDetail in itemDetailsList)
            //                {
            //                    string serialMacQuery = " [dbo].[PcreateStoreTransferItemDetails] " + stdId + ", " + itemDetail.PdId + ", '" + itemDetail.SerialNo + "', '" + itemDetail.MacNo + "' ";
            //                    var result = uow.ExecuteStoreProcedure(serialMacQuery);

            //                    //if (result == 0)
            //                    //{
            //                    //    // Handle the failure case if needed
            //                    //    return result;
            //                    //}
            //                }
            //                return 1; // Assuming 1 indicates success
            //            }
            //        }
            //    }
            //}

            //return 0; // Return 0 if no transfers were processed
        }


        //request details transfer
        //RequestDetailsMetaData
        //IEnumerable<FgetStoreTransferItemPening_Result> RequestDetailsList, IEnumerable<StoreTransferItemDetails>ItemDetailsList, RequestMain requestMain, StoreTransferMain storeTrnsMain
        //public int SaveStoreTransferDetails1(StoreTransfreViewModel viewModel)
        //{
        //    bool IsStockOutSub = IsStockSubmit((StoreTransfreViewModel)viewModel.RequestDetailsList);
        //    if (!IsStockOutSub)
        //    {
        //        var storeTransMain = new StoreTransferMain();

        //        var requestMain = IsRequestMainUpdated(viewModel.requestMain);

        //        foreach (var item in viewModel.RequestDetailsList)
        //        {
        //            //var storeTransferDetails = new StoreTransferDetails();
        //            var storeTransferDetails = new StoreTransferDetailsMetaData();

        //            requestMain = IsRequestDetailUpdated(item, requestMain);
        //            var reqDetails = uow.Repository<RequestDetails>().FindBy(x => x.RDId == item.RdId).FirstOrDefault();

        //            if (item.PurposeTypeId != reqDetails.PurposeTypeId)
        //            {
        //                requestMain.RequestDetails.Where(x => x.RDId == item.RdId).FirstOrDefault().PurposeTypeId = item.PurposeTypeId;
        //            }

        //            if (item.TrQnty != 0)
        //            {
        //                if (requestMain.StoreTransferMain.Count() == 0)
        //                {
        //                    requestMain.RequestDetails.Where(x => x.RDId == item.RdId).FirstOrDefault().ReqQnty = item.ReqQnty;
        //                    reqDetails.ReqQnty = item.ReqQnty;
        //                }
        //                else
        //                {
        //                    item.Status = 30;
        //                    requestMain.RequestDetails.Where(x => x.RDId == item.RdId).FirstOrDefault().Status = 30;
        //                }

        //                storeTransferDetails.STMId = item.STMId;
        //                storeTransferDetails.RDId = item.RdId;
        //                storeTransferDetails.Qnty = Convert.ToInt32(item.TrQnty);
        //                storeTransferDetails.StoreTrQnty = 0;
        //                storeTransferDetails.StoreRemarks = item.StoreRemarks;

        //                //store item details
        //                storeTransferDetails.PurpostypeId = item.PurposeTypeId;
        //                storeTransferDetails.InvestmentTypeId = item.InvestmentCodeId;
        //                storeTransferDetails.BusinessUnitId = item.BusinessUnitId;
        //                storeTransferDetails.TransferQnty = item.TrQnty;
        //                storeTransferDetails.Rate = item.Rate;


        //                if (viewModel.ItemDetailsList != null)
        //                {
        //                    foreach (var itemDetail in viewModel.ItemDetailsList)
        //                    {
        //                        if (item.RdId == itemDetail.STDId)
        //                        {
        //                            itemDetail.STDId = 0;
        //                            storeTransferDetails.StoreTransferItemDetails.Add(itemDetail);
        //                        }
        //                    }
        //                }

        //                storeTransMain.StoreTransferDetails.Add(storeTransferDetails);
        //            }

        //        }
        //        var storeTrnsMain = viewModel.storeTrnsMain;
        //        storeTrnsMain.RMId = requestMain.RMId;
        //        storeTrnsMain.TransferBy = Loader.Models.Global.UserId;
        //        storeTrnsMain.TransferOn = DateTime.Now;
        //        storeTrnsMain.Remarks = requestMain.Remarks;

        //        storeTrnsMain.AccBy = Loader.Models.Global.UserId;
        //        storeTrnsMain.AccDate = DateTime.Now;
        //        storeTrnsMain.AccRemarks = storeTrnsMain.AccRemarks;

        //        try
        //        {
        //            requestMain.StoreTransferMain.Add(storeTrnsMain);
        //            SaveRequestItem(requestMain);

        //            return requestMain.StoreTransferMain.FirstOrDefault().STMId;
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }

        //    }
        //    else
        //    {
        //        RequestMain RM = new RequestMain();
        //        RM = GetSigle(viewModel.RMID);

        //        foreach (var item in viewModel.RequestDetailsList)
        //        {
        //            var preTransQnty = Convert.ToInt32(uow.Repository<RequestDetailsMetaData>().FindBy(x => x.RDId == item.RdId).FirstOrDefault().TRQnty);
        //            if (item.IsStockOut == true && preTransQnty != 0)
        //            {
        //                RM.RequestDetails.Where(x => x.RDId == item.RdId).FirstOrDefault().Status = 50;
        //                RM.Status = 50;
        //            }
        //            else
        //            {
        //                if (preTransQnty != 0)
        //                {
        //                    RM.RequestDetails.Where(x => x.RDId == item.RdId).FirstOrDefault().Status = 60;
        //                    RM.Status = 60;
        //                }
        //            }
        //        }

        //        try
        //        {
        //            SaveRequestItem(RM);
        //            return 0;
        //        }
        //        catch (DbEntityValidationException e)
        //        {
        //            throw e;
        //        }
        //    }

        //}

        public void SaveRequestItem(RequestMain reqMain)
        {
            if(reqMain.RMId == 0)
            {
                uow.Repository<RequestMain>().Add(reqMain);
            }
            else
            {
                uow.Repository<RequestMain>().Edit(reqMain);
            }
            uow.Commit();
        }

        #region PDF Generate
        
        public List<FgetGetPassPrintreport_Result> GetPassList(int STMId)
        {
            return GetPassListFilter(STMId);
        }
        
        public List<FgetGetPassPrintreport_Result> GetPassListFilter(int STMId)
        {
            string query = "select * from [dbo].[FgetGetPassPrintreport]("+ STMId +")";

            var approvedList = uow.Repository<FgetGetPassPrintreport_Result>().ExecWithStoreProcedure(query).ToList();
            return approvedList;
        }
        #endregion


        public List<FgetCpmpanyInfo_Result> GetCompanyInfo()
        {
            var query = "select * from  [dbo].[FgetCpmpanyInfo]()";
            var companyList = uow.Repository<FgetCpmpanyInfo_Result>().ExecWithStoreProcedure(query).ToList();
            return companyList;
        }

        public StoreTransferMain GetTransferDetails(int STMId)
        {
            return uow.Repository<StoreTransferMain>().GetSingle(x => x.STMId == STMId);
        }

        public List<StoreTransfer> ToPrintTransferList(StoreTransferMain stm)
        {
            List<StoreTransfer> st = new List<StoreTransfer>();
            int Count = 0;
            foreach (var item in stm.StoreTransferDetails)
            {
                st.Add(new StoreTransfer
                {
                    SNo = ++Count,
                    ItemName = uow.Repository<ItemMaster>().GetSingle(x => x.ItemId == item.RequestDetails.ItemId).ItemName,
                    ItemCode = uow.Repository<ItemMaster>().GetSingle(x => x.ItemId == item.RequestDetails.ItemId).ItemCode,
                    Qnty = Convert.ToInt32(item.Qnty),
                    STDId = item.STDId,
                    StoreRemarks = item.StoreRemarks,
                    macSerialDetails = item.StoreTransferItemDetails
                });
            }
            return st;
        }

        public BasicTransferDetails GetBasicTransferDetails(StoreTransferMain STM)
        {
            BasicTransferDetails btd = new BasicTransferDetails();
            //var RequestedBy = STM.StoreTransferDetails.FirstOrDefault().RequestDetails.RequestMain.RequestedBy;
            //btd.RequestedBy = uow.Repository<Employees>().GetSingle(x => x.EmployeeId == RequestedBy).EmployeeName;
            //btd.RequestedDate = STM.StoreTransferDetails.FirstOrDefault().RequestDetails.RequestMain.RequestedDate;
            btd.TransferedDate = STM.TransferOn;
            //btd.TransferedBy = gs.IsHod().EmployeeName;
            //btd.ReqNo = STM.StoreTransferDetails.FirstOrDefault().RequestDetails.RequestMain.ReqNo;
            return btd;
        }

        public string getItemName(int? value)
        {
            if(value != null)
            {
                return uow.Repository<ItemMaster>().GetSingle(x => x.ItemId == value).ItemName;
            }
            else
            {
                return null;
            }
        }

        public string getBranchName(int? value)
        {
            if(value != null)
            {
                return uow.Repository<BranchDetails>().GetSingle(x => x.BranchId == value).BranchName;
            }
            else
            {
                return null;
            }
        }

    }
}