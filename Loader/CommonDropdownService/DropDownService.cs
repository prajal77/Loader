using ChannakyaCustomeDatePicker.Repository;
using Loader.EntityDataModel;
using Loader.Models;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Loader.CommonDropdownService
{
    public static class DropDownService
    {
        public static List<SelectListItem> DepartmentDropDown()
        {
            using(GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from [LG].[Department]";
                var res = uow.Repository<Department>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.DepartmentId.ToString(), Text = x.DeptName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> DesignationDropDown()
        {
            using(GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from [LG].[Designation]";
                var res = uow.Repository<Designation>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.DesignationId.ToString(), Text = x.DGName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> EmployeeDropDown()
        {
            using(GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select EmployeeId, EmployeeName from [LG].[Employees] where EmployeeId not in (select EmployeeId from [LG].[User] )";
                var res = uow.Repository<Employee>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.EmployeeId.ToString(), Text = x.EmployeeName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> MenuTemplateDropDown()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetMenuTemplateInfo()";
                var res = uow.Repository<FgetMenuTemplateInfo_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.MTId.ToString(), Text = x.MTName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> BranchDepartmentDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from fgetLevelAuthorityType()";
                var res = uow.Repository<fgetLevelAuthorityType_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.TypeId.ToString(), Text=x.LauthorityType}).ToList();
                return res;
            }
        }

        public static List<SelectListItem> GetEmployeeInfoDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetEmployeeInfoTB() where EmployeeId not in(select EmployeeId from StaffAuthorization)";
                var res = uow.Repository<FgetEmployeeInfoTB_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.EmployeeId.ToString(), Text=x.EmployeeName}).ToList();
                return res;
            }
        }

        public static List<SelectListItem> BranchDetailDD()
        {
            using(GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from BranchDetails";
                var res = uow.Repository<BranchDetails>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.BranchId.ToString(), Text = x.BranchName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> LevelAuthorityDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from LevelAuthority";
                var res = uow.Repository<LevelAuthority>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.LvlId.ToString(), Text = x.LvlName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> SupervisorListDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetEmployeeSupervisorlist()";
                var res = uow.Repository<FgetEmployeeSupervisorlist_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.EmployeeId.ToString(), Text = x.EmployeeName }).ToList();
                return res;
            }
        }

        public static List<SelectListItem> ItemTypeDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from [dbo].[ItemType]";
                var result = uow.Repository<ItemType>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value=x.ItemTypeId.ToString(), Text=x.ItemTypeName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> MasterUnitOfMeasureDD()
        {
            using(GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetUnitOfMeasure()";
                var result = uow.Repository<FgetUnitOfMeasure_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value= x.UId.ToString(), Text=x.UnitName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ManufactureDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetMaifacture()";
                var result = uow.Repository<FgetMaifacture_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.Mid.ToString(), Text = x.ManifactureName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> BrandDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetBrand()";
                var result = uow.Repository<FgetBrand_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.Bid.ToString(), Text = x.BrandName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ModelDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from fgetMasterModel()";
                var result = uow.Repository<fgetMasterModel_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.Mid.ToString(), Text = x.ModelName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> TaxMasterDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetTaxMaster()";
                var result = uow.Repository<FgetTaxMaster_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.TId.ToString(), Text = x.TaxName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> MasterLocationDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetMasterLocation()";
                var result = uow.Repository<FgetMasterLocation_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.Lid.ToString(), Text = x.LocationName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ItemMasterDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetItemMasterList()";
                var result = uow.Repository<FgetItemMasterList_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.Itemid.ToString(), Text= x.ItemName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> BUnitDetailsDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from BusinessUnitDetails";
                var result = uow.Repository<BusinessUnitDetails>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.BUnitId.ToString(), Text = x.BUnitName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> UnitOfMeasureDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from FgetUnitOfMeasure()";
                var result = uow.Repository<FgetUnitOfMeasure_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.UId.ToString(), Text = x.UnitName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> PurposeTypeDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from PurposeTypeMaster";
                var result = uow.Repository<PurposeTypeMaster>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.PurposeTypeId.ToString(), Text = x.PurposeType }).ToList();
                return result;
            }
        }


        public static List<SelectListItem> InvestmentCodeDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select * from InvestmentCodeDetails";
                var result = uow.Repository<InvestmentCodeDetails>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.InvestmentCodeId.ToString(), Text = x.InvestmentCodeName }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ExistingTypeDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select distinct Type from FgetPurchaseSerialNolist()";
                var result = uow.Repository<FgetPurchaseSerialNolist_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.PdId.ToString(), Text = x.Type }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ExistingSerialNoDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select SerialNo from FgetPurchaseSerialNolist()";
                var result = uow.Repository<FgetPurchaseSerialNolist_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.PdId.ToString(), Text = x.SerialNo }).ToList();
                return result;
            }
        }

        public static List<SelectListItem> ExistingMacNoDD()
        {
            using (GenericUnitOfWork uow = new GenericUnitOfWork())
            {
                string query = "select MacNo from FgetPurchaseSerialNolist()\r\n";
                var result = uow.Repository<FgetPurchaseSerialNolist_Result>().ExecWithStoreProcedure(query).Select(x => new SelectListItem() { Value = x.PdId.ToString(), Text = x.MacNo }).ToList();
                return result;
            }
        }

    }
}