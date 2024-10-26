using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Loader.Service
{
    public class UserVSBranchService
    {
        private GenericUnitOfWork uow = null;


        public UserVSBranchService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<UserVSBranch> GetAll()
        {
            return uow.Repository<UserVSBranch>().GetAll().ToList();
        }
        public List<Company> GetAllCompany()
        {
            return uow.Repository<Company>().GetAll().ToList();
        }

        public List<UserVSBranch> GetAllOfParent(int parentId)
        {


            return uow.Repository<UserVSBranch>().GetAll().Where(x => x.Id == parentId).ToList();
        }

        public List<ApplicationUser> GetAllUsers()
        {
            return uow.Repository<Models.ApplicationUser>().GetAll().ToList();
        }

        public string WithHeirarchy()
        {
            var dValue = uow.Repository<ParamValue>().GetSingle(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.DepartmentinHeirarchy).PValue;
            return dValue;
        }

        public UserVSBranch GetSingle(int UserVSBranchId)
        {
            UserVSBranch UserVSBranch = uow.Repository<UserVSBranch>().GetSingle(c => c.Id == UserVSBranchId);
            return UserVSBranch;
        }
        public string GetEmployeeName(int empId)
        {
            string employeeName = "";
            var empObj = uow.Repository<Models.Employee>().GetSingle(x => x.EmployeeId == empId);
            if (empObj != null)
            {
                employeeName = empObj.EmployeeName;
            }
            return employeeName;
        }

    
        public string GetUserName(int userId)
        {
            string userName = "";
            var userObj = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
            if(userObj!=null)
            {
                userName = userObj.UserName;
            }
            return userName;
        }

        public List<SelectListItem> GetCompanyList()
        {
            List<SelectListItem> ls = new List<SelectListItem>();
            var companyList = uow.Repository<Company>().SqlQuery("select * from dbo.FGetCompanyList() order by CompanyId").ToList();
            foreach (var item in companyList)
            {
                //var parentId = companyList.Where(x => x.ParentId == item.CompanyId).FirstOrDefault();
                //if (parentId != null)
                //{
                //    var parentName = parentId.BranchName;
                //    ls.Add(new SelectListItem { Text = item.BranchName + "(" + parentName + ")", Value = item.CompanyId.ToString() });
                //}
                //else
                //{
                //    ls.Add(new SelectListItem { Text = item.BranchName, Value = item.CompanyId.ToString() });
                //}
                ls.Add(new SelectListItem { Text = GetAddressInCompany(item.CompanyId), Value = item.CompanyId.ToString() });
                
            }
            return ls;
        }

        public Company GetSingleCompany(int companyId)
        {
            //return uow.Repository<Company>().SqlQuery("select * from dbo.FGetCompanyList() order by CompanyId").ToList().Where(x => x.CompanyId == companyId).FirstOrDefault();
            return uow.Repository<Company>().SqlQuery("select * from [LG].[Company] order by CompanyId").ToList().Where(x => x.CompanyId == companyId).FirstOrDefault();
        }
        public string GetAddressInCompany(int BranchSetupId)
        {
            string result = "";

            if (BranchSetupId != 0)
            {
                Company mnu = new Company();
                mnu = GetSingleCompany(BranchSetupId);

                List<string> lst = new List<string>();


                while (mnu != null)
                {
                    lst.Add(mnu.BranchName);
                    mnu = GetSingleCompany(mnu.ParentId);
                };

                var sorted = lst.Select((x, i) => new KeyValuePair<string, int>(x, i)).OrderByDescending(x => x.Value).ToList();

                foreach (var item in sorted)
                {
                    if (result == "")
                    {
                        result = result + item.Key;
                    }
                    else
                    {
                        result = result + "-" + item.Key;
                    }

                }
            }
            else
            {
                result = "ChannakyaSoft";
            }
            return result;
        }

        public string GetCurrentBranchString(int userId)
        {
            string branchName = "";
            var hasBranchsAssigned = uow.Repository<UserVSBranch>().FindBy(x => x.UserId == userId).ToList();
            if (hasBranchsAssigned.Count()>0)
            {
                branchName= GetBranchName(hasBranchsAssigned.LastOrDefault().BranchId);
            }
            else
            {
                var userObj = uow.Repository<Models.ApplicationUser>().GetSingle(x => x.Id == userId);
                if (userObj != null)
                {
                    int empId =(int) userObj.EmployeeId;

                    int branchId =Convert.ToInt32(uow.Repository<Models.Employee>().GetSingle(x => x.EmployeeId == empId).BranchId);
                    branchName= GetBranchName(branchId);
                }
               
            }
            return branchName;
        }

        public string GetBranchName(int branchId)
        {
            string branchName = "";
            var branchObj = uow.Repository<Models.Company>().GetSingle(x => x.CompanyId == branchId);
            if (branchObj != null)
            {
                branchName = branchObj.BranchName;
            }
            return branchName;
        }


        public int GetBranchRoleId(int branchId,int userId)
        {
            int roleId = 0;
            var branchObj = uow.Repository<Models.UserVSBranch>().FindBy(x => x.BranchId == branchId && x.UserId==userId).LastOrDefault();
            if (branchObj != null)
            {
                roleId = branchObj.RoleId;
            }
            else
            {
                var empObj = uow.Repository<Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();
                if(empObj!=null)
                {
                    roleId = empObj.MTId;
                }
            }
            
            return roleId;
        }

        public void Save(UserVSBranch UserVSBranch)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<UserVSBranch>().GetAll().Where(x => x.Id != UserVSBranch.Id && x.BranchId == UserVSBranch.BranchId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Already Mapped User and Branch");
            }
            if (UserVSBranch.Id == 0)
            {
                if(UserVSBranch.IsPermanent==true)
                {
                    UserVSBranch.To = null;
                }
                UserVSBranch.PostedBy = Loader.Models.Global.UserId;
                UserVSBranch.PostedOn = DateTime.Now;
                uow.Repository<UserVSBranch>().Add(UserVSBranch);
            }
            else
            {

                uow.Repository<UserVSBranch>().Edit(UserVSBranch);
            }
            uow.Commit();
        }

        public int GetCurrentBranchInt(int userId)
        {
            int currentBranch = 0;
            var branchObj = uow.Repository<Models.ApplicationUser>().FindBy(x => x.Id == userId).FirstOrDefault();
            if (branchObj != null)
            {
                var employeeObj = uow.Repository<Models.Employee>().FindBy(x => x.EmployeeId == branchObj.EmployeeId).FirstOrDefault();
                if (employeeObj != null)
                {
                    if (employeeObj.BranchId != null && employeeObj.BranchId != 0)
                    {
                        currentBranch = Convert.ToInt32(employeeObj.BranchId);
                    }
                }
            }
            return currentBranch;
        }

        public UserBranchViewModel HasAnotherRole(int userId)
        {
            UserBranchViewModel _usrBrnchViewModel = new UserBranchViewModel();
            var allbranchList = uow.Repository<Models.UserVSBranch>().FindBy(x => x.UserId == userId).Where(x => x.To > DateTime.Now || x.To == null && x.IsEnable==true).ToList();

            List<Branch> BranchList = new List<Branch>();
            foreach (var item in allbranchList)
            {
                BranchList.Add(new Branch { BranchId=item.BranchId,BranchName=GetBranchName(item.BranchId)});
            }
            _usrBrnchViewModel.Branch=BranchList;
            return _usrBrnchViewModel;
        }

        public bool Delete(int UserVSBranchId)
        {
            UserVSBranch UserVSBranch = this.GetSingle(UserVSBranchId);

            if (UserVSBranch != null)
            {
                uow.Repository<UserVSBranch>().Delete(UserVSBranch);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }



    }
}