using Loader.EntityDataModel;
using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using static Loader.ViewModel.Metadata;

namespace Loader.Service
{
    public class EmployeeService
    {
        private GenericUnitOfWork uow = null;
        Loader.Service.UserVSBranchService usrBrnchService = new UserVSBranchService();


        public EmployeeService()
        {
            uow = new GenericUnitOfWork();
        }
        public bool AllowDepartmentInGroup()
        {
            var dValue =Convert.ToBoolean (uow.Repository<ParamValue>().GetSingle(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.AllowEmployeeinDepartmentGroup).PValue);
            return dValue;
        }
      
        public List<Employee> GetAll()
        {
            return uow.Repository<Employee>().GetAll().ToList();
        }

        public Employee GetSingle(int EmployeeId)
        {
            Employee Employee = uow.Repository<Employee>().GetSingle(c => c.EmployeeId == EmployeeId);
            return Employee;
        }
        public bool HasDeptChilds(int deptId)
        {
            var Employee = uow.Repository<Department>().FindBy(x => x.PDeptId == deptId);
            bool hasChilds = false;
            if(Employee!=null)
            {
                hasChilds= true;
            }
            return hasChilds;
        }
        public List<SelectListItem> GetBloodGroupOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<BloodGroup>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.BGName, Value = item.BGId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetReligionOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Religion>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.ReligionName, Value = item.RId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetNationalityOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Nationality>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.NName, Value = item.NId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetMaritialStatusOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<MaritialStatus>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.MSName, Value = item.MSId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetStatusOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Status>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.StatusName, Value = item.StatusId.ToString() });
            }
            return lst;
        }
        public List<SelectListItem> GetGenderOption()
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            var list = uow.Repository<Gender>().GetAll();
            foreach (var item in list)
            {
                lst.Add(new SelectListItem { Text = item.GenderName, Value = item.GenderId.ToString() });
            }
            return lst;
        }
        public void Save(Employee TemplateData)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            
            var withDesg = WithDesignation();
            var withDept = WithDepartment();
            if (TemplateData.EmployeeNo==null || TemplateData.EmployeeName==null ||(withDesg==true&&TemplateData.DGId==null) || (withDept==true&&TemplateData.DeptId==null) )
            {
                throw new Exception("The Required Feild is Empty");
            }

            if (TemplateData.EmployeeId == 0)
            {
                int checkExists = editUOW.Repository<Employee>().GetAll().Where(x => x.EmployeeNo == TemplateData.EmployeeNo || x.EmployeeId == TemplateData.EmployeeId).Count();
                if (checkExists > 0)
                {
                    throw new Exception("Duplicate Employee Found. Employee Number Not Valid");
                }
                TemplateData.DateOfJoin = Convert.ToDateTime(TemplateData.DateOfJoin);
                TemplateData.PostedOn = DateTime.Now;
                TemplateData.PostedBy = Global.UserId;
                TemplateData.ModifiedOn = DateTime.Now;
                uow.Repository<Employee>().Add(TemplateData);
            }
            else
            {
                if(TemplateData.Photo==null)
                {
                    byte[] ImageContent = editUOW.Repository<Employee>().GetSingle(x => x.EmployeeId == TemplateData.EmployeeId).Photo;
                    TemplateData.Photo = ImageContent;
                }
                
                
              
                TemplateData.ModifiedBy = Global.UserId;
                TemplateData.ModifiedOn = DateTime.Now;
                
                uow.Repository<Employee>().Edit(TemplateData);
            }
            uow.Commit();
        }
        public List<Employee> GetEmployeeList()
        {
            List<Employee> lstEmployee = new List<Employee>();
            return lstEmployee;
        }
        public bool Delete(int EmployeeId)
        {
            Employee Employee = this.GetSingle(EmployeeId);

            if (Employee != null)
            {
                uow.Repository<Employee>().Delete(Employee);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public string GetStatusName(int statusId)
        {
            //string name = uow.Repository<Status>().GetSingle(x => x.StatusId == statusId).StatusName;
            string name = uow.Repository<Status>().FindBy(x => x.StatusId == statusId).Select(x=>x.StatusName).FirstOrDefault();
            return name;
        }
        public string GetDepartmentName(int? deptId)
        {
            string name = uow.Repository<Department>().FindBy(x => x.DepartmentId == deptId).Select(x => x.DeptName).FirstOrDefault();
            return name;
        }
        public string GetDesignationName(int? DGId)
        {
            string name = uow.Repository<Designation>().FindBy(x => x.DesignationId == DGId).Select(x => x.DGName).FirstOrDefault();
            return name;
        }
        //public string GetAddress(int EmployeeId)
        //{
        //    string result = "";

        //    if (EmployeeId != 0)
        //    {
        //        Employee mnu = new Employee();
        //        mnu = GetSingle(EmployeeId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.EmployeeName);
        //            mnu = GetSingle(mnu.PEmployeeId);
        //        };

        //        var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

        //        foreach (var item in sorted)
        //        {
        //            if (result == "")
        //            {
        //                result = result + item.Key;
        //            }
        //            else
        //            {
        //                result = result + "/" + item.Key;
        //            }

        //        }
        //    }
        //    else
        //    {
        //        result = "Root";
        //    }
        //    return result;
        //}

        #region Employee Authorization
        public List<FgetStaffAuthorization_ResultMetaData> GetStaffAuthorizationList()
        {
            string query = "select * from FgetStaffAuthorization()";
            var staffAuthList = uow.Repository<FgetStaffAuthorization_ResultMetaData>().ExecWithStoreProcedure(query).ToList();
            return staffAuthList;
        } 

        public FgetStaffAuthorization_ResultMetaData GetStaffAuthorizationById(int Id)
        {
            try
            {
                string query = "select * from FgetStaffAuthorization()";
                var data = uow.Repository<FgetStaffAuthorization_ResultMetaData>().SqlQuery(query).Where(x => x.Said == Id).SingleOrDefault();
                return data;

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        public string SaveEmployeeAuthorization(FgetStaffAuthorization_ResultMetaData model)
        {
            string query = "";
            try
            {
                if(model.Said == 0)
                {
                    if (!model.SupervisorId.HasValue)
                    {
                        model.SupervisorId = model.Employeeid;
                    }

                    if(model.EmpBranchId.HasValue)
                    {
                        query = "PcreateStaffAuthorization 0 "+ ", "+model.Employeeid+", "+model.EmpBranchId+", "+ model.LocationId +", "+model.LvlId+", "+model.SupervisorId+", "+ Loader.Models.Global.UserId +", "+ model.Status +", "+model.TypeId+"";
                    }

                    if (model.DepartmentId.HasValue)
                    {
                        query = "PcreateStaffAuthorization 0 " + ", " + model.Employeeid + ", " + model.DepartmentId + ", "+ model.LocationId +", " + model.LvlId + ", " + model.SupervisorId + ", " + Loader.Models.Global.UserId + ", " + model.Status + ", " + model.TypeId + "";
                    }

                }
                else
                {
                    if (!model.SupervisorId.HasValue)
                    {
                        model.SupervisorId = model.Employeeid;
                    }

                    if (model.EmpBranchId.HasValue)
                    {
                        query = "PcreateStaffAuthorization " + model.Said + ", " + model.Employeeid + ", " + model.EmpBranchId + ", "+ model.LocationId +", " + model.LvlId + ", " + model.SupervisorId + ", " + Loader.Models.Global.UserId + ", " + model.Status + ", " + model.TypeId + "";
                    }

                    if (model.DepartmentId.HasValue)
                    {
                        query = "PcreateStaffAuthorization " + model.Said + ", " + model.Employeeid + ", " + model.DepartmentId + ", "+ model.LocationId +", " + model.LvlId + ", " + model.SupervisorId + ", " + Loader.Models.Global.UserId + ", " + model.Status + ", " + model.TypeId + "";
                    }
                }

                var empInfo = uow.ExecuteStoreProcedure(query);

                return "Staff Authorization Added Successfully!";

            }catch(Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Tree

        List<int> templateList = new List<int>();
        public List<int> CheckedEmployee(TreeView data)
        {

            foreach (var item in data.TreeData)
            {
                if (item.IsChecked == true)
                {
                    templateList.Add(item.Id);
                }
                if (item.Children != null)
                {
                    TemplateChild(item);
                }

            }
            return templateList;

        }
        private void TemplateChild(TreeDTO data)
        {
            foreach (var item in data.Children)
            {
                if (item.IsChecked == true)
                {
                    templateList.Add(item.Id);

                }
                if (item.Children != null)
                {
                    data.Children = item.Children;
                    TemplateChild(data);
                }
            }

        }

        private List<Company> FilterBranchTree(List<Company> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.BranchName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.CompanyId equals selList.ParentId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.CompanyId equals c.CompanyId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.CompanyId).ToList();
            }
            list = filteredList;
            return list;

        }
        private List<Department> FilterDepartmentTree(List<Department> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.DeptName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DepartmentId equals selList.PDeptId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.DepartmentId equals c.DepartmentId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.DepartmentId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetDepartmentGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Department>().GetAll();
            List<Department> list = treelist.ToList();

            //list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterDepartmentTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateDepartmentTree(list, 0);
            return tree;
        }

        public ViewModel.TreeView GetBranchGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Company>().GetAll();
            List<Company> list = treelist.ToList();

            //list.Add(new Department { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterBranchTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateBranchTree(list, 1);
            return tree;
        }
        private ViewModel.TreeView GenerateBranchTree(List<Company> list, int? parentDepartmentId)
        {

            var parent = list.Where(x => x.ParentId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Department";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.CompanyId,
                    PId = itm.ParentId,
                    Text = usrBrnchService.GetAddressInCompany(itm.CompanyId),

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateBranchTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        private ViewModel.TreeView GenerateDepartmentTree(List<Department> list, int? parentDepartmentId)
        {

            var parent = list.Where(x => x.PDeptId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Department";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.DepartmentId,
                    PId = itm.PDeptId,
                    Text = itm.DeptName,

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateDepartmentTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }


        private List<Designation> FilterDesignationTree(List<Designation> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.DGName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DesignationId equals selList.PDGId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.DesignationId equals c.DesignationId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.DesignationId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetDesignationGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Designation>().GetAll();
            List<Designation> list = treelist.ToList();

            //list.Add(new Designation { DGId = 0, DGName = "Root", PDGId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterDesignationTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateDesignationTree(list, 0);
            return tree;
        }
        private ViewModel.TreeView GenerateDesignationTree(List<Designation> list, int? parentDesignationId)
        {

            var parent = list.Where(x => x.PDGId == parentDesignationId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Designation";
            foreach (var itm in parent)
            {
                bool isGrp = false;
                int childs = IsGroup(itm.DesignationId, list);
                if (childs > 0)
                {
                    isGrp = true;
                }
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.DesignationId,
                    PId = itm.PDGId,
                    Text = itm.DGName,
                    IsGroup = isGrp

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateDesignationTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }
        private int IsGroup(int DGid, List<Designation> list)
        {
            int childs = list.Where(x => x.PDGId == DGid).Select(x => x.DesignationId).Count();
            return childs;
        }
        public bool WithPayrollParameters()
        {
            string withDes = uow.Repository<ParamValue>().GetSingle(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.WithPayrollParameters).PValue;
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
        }
        public bool WithDesignation()
        {
            string withDes = uow.Repository<ParamValue>().GetSingle(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.WithDesignation).PValue;
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
        }
        public bool WithDepartment()
        {
            string withGrp = uow.Repository<ParamValue>().GetSingle(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.WithDepartment).PValue;
            bool desc = withGrp.Equals("true") ? true : false;
            return desc;
        }


        #endregion
    }
}