using Loader.Models;
using Loader.Repository;
using Loader.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using PagedList;
using PagedList.Mvc;
using System.Web.Mvc;

namespace Loader.Service
{
    public class UsersService
    {
        private GenericUnitOfWork uow = null;
  

        public UsersService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Users> GetAll()
        {
            return uow.Repository<Users>().GetAll().ToList();
        }

        public Users GetSingle(int UsersId)
        {
            Users Users = uow.Repository<Users>().GetSingle(c => c.UserId == UsersId);
            return Users;
        }
        public string GetMenuTemplateName(int MTId)
        {
            var name = uow.Repository<MenuTemplate>().GetSingle(x => x.MTId == MTId).MTName;
            return name;

        }
        public string GetDepartmentName(int? Id)
        {
            var name = "";
            if (Id != 0)
            {
                name = uow.Repository<Department>().GetSingle(x => x.DepartmentId == Id).DeptName;
            }
            return name;

        }
        public string GetDesignationName(int? Id)
        {
            var name = "";
            if (Id != 0)
            {
                name = uow.Repository<Designation>().GetSingle(x => x.DesignationId == Id).DGName;
            }
            return name;

        }
        public void Save(Users userData)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Users>().GetAll().Where(x => x.UserId != userData.UserId && x.UserName == userData.UserName).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Users Found. Users Caption Not Valid");
            }
            if (userData.UserId == 0)
            {
                if (userData.IsUnlimited == true)
                {
                    userData.From = Convert.ToDateTime(null);
                    userData.To = Convert.ToDateTime(null);

                }
                RegisterViewModel regObj = new RegisterViewModel();
                regObj.Email = userData.Email;
                regObj.IsUnlimited = userData.IsUnlimited;
                regObj.Password = userData.Password;
                regObj.ConfirmPassword = userData.ReEnterPassword;
                //uow.Repository<Users>().Add(userData);
            }
            else
            {
                Users userEdit = new Users();
                var password = editUOW.Repository<Users>().GetSingle(x => x.UserId == userData.UserId).Password;
                userData.Password = password;
                userData.ReEnterPassword = password;
                uow.Repository<Users>().Edit(userData);
            }
            uow.Commit();
        }
        public List<Users> GetUsersList()
        {
            List<Users> lstUsers = new List<Users>();
            return lstUsers;
        }
        public bool Delete(int UsersId)
        {
            Users Users = this.GetSingle(UsersId);

            if (Users != null)
            {
                uow.Repository<Users>().Delete(Users);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }

        //public string GetAddress(int UsersId)
        //{
        //    string result = "";

        //    if (UsersId != 0)
        //    {
        //        Users mnu = new Users();
        //        mnu = GetSingle(UsersId);

        //        List<string> lst = new List<string>();


        //        while (mnu != null)
        //        {
        //            lst.Add(mnu.UsersName);
        //            mnu = GetSingle(mnu.PUsersId);
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


        #region Tree

        List<int> templateList = new List<int>();
        public List<int> CheckedUsers(TreeView data)
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
        private List<MenuTemplate> FilterMenuTemplateTree(List<MenuTemplate> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.MTName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.MTId equals selList.MTId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.MTId equals c.MTId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.MTId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetMenuTemplateGroupTree(string filter = "")
        {
            var treelist = uow.Repository<MenuTemplate>().GetAll();
            List<MenuTemplate> list = treelist.ToList();

            //list.Add(new MenuTemplate { DeptId = 0, DeptName = "Root", PDeptId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterMenuTemplateTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateMenuTemplateTree(list, -1);
            return tree;
        }
        private ViewModel.TreeView GenerateMenuTemplateTree(List<MenuTemplate> list, int? parentMenuTemplate)
        {

            //var parent = list.Where(x => x.MTId == parentDepartmentId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Menu Template";
            foreach (var itm in list)
            {
                if (itm.MTId == 1)
                {
                    if (Loader.Models.Global.UserId == 1)
                    {
                        tree.TreeData.Add(new ViewModel.TreeDTO
                        {
                            Id = itm.MTId,
                            Text = itm.MTName,

                        });
                    }
                }
                else
                {
                    tree.TreeData.Add(new ViewModel.TreeDTO
                    {
                        Id = itm.MTId,
                        Text = itm.MTName,

                    });
                }
                
            }

            return tree;
        }

        private List<Employee> FilterEmployeeTree(List<Employee> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.EmployeeName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.DGId equals selList.EmployeeId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.DGId equals c.DGId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.DGId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.SearchViewModel GetEmployeeGroupTree(string filter = "", List<Employee> list=null)
        {
            
          
            //var treelist = uow.Repository<Employee>().GetAll().Skip(pageFinal * PageSize).Take(PageSize);
          
            if (filter.Trim() != "")
            {
                list = FilterEmployeeTree(list, filter);
            }
            ViewModel.SearchViewModel tree = this.GenerateEmployeeTree(list, 2);
            return tree;
        }

        private ViewModel.SearchViewModel GenerateEmployeeTree(List<Employee> list, int? parentDesignationId)
        {
            ViewModel.SearchViewModel tree = new ViewModel.SearchViewModel();
            tree.Title = "Designation";

            foreach (var itm in list)
            {
               
                tree.SearchData.Add(new SearchDTO
                {
                    Address="",
                    Id=itm.EmployeeId,
                    EmpNo=itm.EmployeeNo,
                    PhoneNumber="",
                    Text=itm.EmployeeName,
               
                    DeptId =GetDepartmentName(itm.DeptId==null?0:itm.DeptId),
                    
                    DGId=GetDesignationName(itm.DGId == null ? 0 : itm.DGId)
                });

            }
            return tree;
        }

        public bool WithEmployee()
        {
            string withDes = uow.Repository<ParamValue>().GetAll().Where(x => x.PId == 21).Select(x => x.PValue).SingleOrDefault();
            bool desc = withDes.Equals("true") ? true : false;
            return desc;
        }

        #endregion
    }
}