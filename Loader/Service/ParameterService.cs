using Loader.Models;
using Loader.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Loader.Service
{
    public class ParameterService
    {
        private GenericUnitOfWork uow = null;


        public ParameterService()
        {
            uow = new GenericUnitOfWork();
        }

        public List<Param> GetAll()
        {
            return uow.Repository<Param>().GetAll().ToList();
        }

        public List<Param> GetAllOfParent(int parentId)
        {
            return uow.Repository<Param>().GetAll().Where(x => x.ParentId == parentId).ToList();
        }

        public Param GetSingle(int paramId)
        {
            Param param = uow.Repository<Param>().GetSingle(c => c.PId == paramId);
            return param;
        }
        public int GetCurrentFYID()
        {
            int currentFiscalYear = 0;
            

            var currentFY = uow.Repository<Models.FiscalYear>().FindBy(x => x.StartDt < DateTime.Now && x.EndDt > DateTime.Now).FirstOrDefault();
            if (currentFY != null)
            {
                currentFiscalYear = currentFY.FYID;
            }
            return currentFiscalYear;
        }

        public  string GetCurrentFiscalYear(int FYId=0)
        {
            var currentFiscalYear = DateTime.Now.ToShortDateString();
            if (FYId != 0)
            {
                var currentFY = uow.Repository<Models.FiscalYear>().FindBy(x => x.FYID == FYId).FirstOrDefault();
                if (currentFY != null)
                {
                    currentFiscalYear = currentFY.FyName;
                }

            }
        
            return currentFiscalYear;
        }
        public DateTime GetTransactionDate()
        {
            DateTime currentDate = new DateTime();
            var useCustomDateObj = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.UseCustomTransactionDate).FirstOrDefault();
            if(useCustomDateObj!=null)
            {
                bool useCurrentDate = Convert.ToBoolean(useCustomDateObj.PValue);
                if(useCurrentDate)
                {
                    var customDateObj = uow.Repository<Models.ParamValue>().FindBy(x => x.PId == (int)Loader.Helper.EnumValue.Parameter.TransactionDate).FirstOrDefault();
                    if(customDateObj!=null)
                    {
                        currentDate =Convert.ToDateTime(customDateObj.PValue);
                    }
                }

            }
            return currentDate;
        }

        public void Save(Param param)
        {
            GenericUnitOfWork editUOW = new GenericUnitOfWork();
            int checkExists = editUOW.Repository<Param>().GetAll().Where(x => x.PId != param.PId && x.PName == param.PName && x.ParentId == param.ParentId).Count();
            if (checkExists > 0)
            {
                throw new Exception("Duplicate Param Found. Param Caption Not Valid");
            }
            if (param.PId == 0)
            {
                uow.Repository<Param>().Add(param);
            }
            else
            {
                if (param.paramValue != null)
                {
                    if (param.paramValue.paramScript != null)
                    {
                        if (param.paramValue.paramScript.PId == 0)
                        {
                            param.paramValue.paramScript.PId = param.PId;
                            uow.Repository<ParamScript>().Add(param.paramValue.paramScript);
                        }
                        else
                        {
                            uow.Repository<ParamScript>().Edit(param.paramValue.paramScript);
                        }
                    }
                    uow.Repository<ParamValue>().Edit(param.paramValue);
                }

                uow.Repository<Param>().Edit(param);

            }
            uow.Commit();
        }

        //private bool CheckifExists(int paramId)
        //{

        //}

        public bool Delete(int paramId)
        {
            Param param = this.GetSingle(paramId);
            ParamValue paramValue = uow.Repository<ParamValue>().GetSingle(x => x.PId == paramId);
            ParamScript paramScript = uow.Repository<ParamScript>().GetSingle(x => x.PId == paramId);

            if (param != null)
            {
                if (paramScript != null)
                {
                    uow.Repository<ParamScript>().Delete(paramScript);
                }
                if (paramValue != null)
                {
                    uow.Repository<ParamValue>().Delete(paramValue);
                }

                uow.Repository<Param>().Delete(param);
                uow.Commit();
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SaveParamValues(int id, string value)
        {
            ParamValue paramValue = uow.Repository<ParamValue>().GetSingle(x => x.PId == id);
            paramValue.PValue = value;

            uow.Repository<ParamValue>().Edit(paramValue);

            uow.Commit();

        }

        //public string SelectedVal(int pId,string query)
        //{
        //    var getVal = uow.Repository<ParamValue>().GetSingle(x => x.PId == pId);

        //    var selectedText = GetOption(query);
        //}
        public List<SelectListItem> GetOption(string query, int selectedId = 0)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            string con = ConfigurationManager.ConnectionStrings["MainConnection"].ConnectionString;
            SqlDataAdapter sd = new SqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt32(dr[0]) == selectedId)
                {
                    lst.Add(new SelectListItem { Text = dr[1].ToString(), Value = dr[0].ToString(), Selected = true });
                }
                else
                {
                    lst.Add(new SelectListItem { Text = dr[1].ToString(), Value = dr[0].ToString() });
                }

            }
            return lst;


        }

        public string GetAddress(int paramId)
        {
            string result = "";

            if (paramId != 0)
            {
                Param param = new Param();
                param = GetSingle(paramId);

                List<string> lst = new List<string>();


                while (param != null)
                {
                    lst.Add(param.PName);
                    param = GetSingle(param.ParentId);
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
                        result = result + "/" + item.Key;
                    }

                }
            }
            else
            {
                result = "Root";
            }
            return result;
        }

        #region Tree

        private List<Param> FilterTree(List<Param> list, string filter)
        {
            bool lLoop = true;

            var filteredList = list.Where(x => x.PName.ToLower().Contains(filter.ToLower())).ToList();

            while (lLoop)
            {
                //select all parents of filtered list
                var allParent = (from mainList in list
                                 join selList in filteredList on mainList.PId equals selList.ParentId
                                 select mainList).ToList();

                //Select unique parent only
                var parentList = (from p in allParent
                                  join c in filteredList on p.PId equals c.PId into gj
                                  from uniqueParent in gj.DefaultIfEmpty()
                                  where uniqueParent == null
                                  select p).ToList();

                if (parentList.Count() == 0)
                {
                    lLoop = false;
                }

                filteredList = filteredList.Union(parentList).OrderBy(x => x.PId).ToList();
            }
            list = filteredList;
            return list;

        }
        public ViewModel.TreeView GetParamGroupTree(string filter = "")
        {
            var treelist = uow.Repository<Param>().GetAll().Where(x => x.IsGroup == true);
            List<Param> list = treelist.ToList();

            list.Add(new Param { PId = 0, IsGroup = true, PName = "Root", ParentId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }

        public ViewModel.TreeView GetParamGroupTree(int ParamIdExpect, string filter = "")
        {
            List<Param> list = uow.Repository<Param>().GetAll().Where(x => x.IsGroup == true).Where(x => x.PId != ParamIdExpect).ToList();
            list.Add(new Param { PId = 0, IsGroup = true, PName = "Root", ParentId = -1 });

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }
            ViewModel.TreeView tree = this.GenerateTree(list, -1);
            return tree;
        }


        public ViewModel.TreeView GetParamGroupTree(int parentId, int ParamIdExpect, string filter = "")
        {
            List<Param> list = uow.Repository<Param>().GetAll().Where(x => x.IsGroup == true).Where(x => x.PId != ParamIdExpect).ToList();

            if (filter.Trim() != "")
            {
                list = FilterTree(list, filter);
            }

            ViewModel.TreeView tree = this.GenerateTree(list, parentId);
            return tree;
        }

        private ViewModel.TreeView GenerateTree(List<Param> list, int? parentParamId)
        {

            var parent = list.Where(x => x.ParentId == parentParamId);
            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Param";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.PId,
                    PId = itm.ParentId,
                    Text = itm.PName,
                    IsGroup = itm.IsGroup,
                    //Image = itm.Image

                });
            }

            foreach (var itm in tree.TreeData)
            {
                itm.Children = GenerateTree(list, itm.Id).TreeData.ToList();
            }
            return tree;
        }

        private List<Param> GenerateChildList(List<Param> list, int? parentParamId, List<Param> newList = null)
        {

            var parent = list.Where(x => x.ParentId == parentParamId);

            if (newList == null)
            {
                newList = new List<Param>();
            }

            ViewModel.TreeView tree = new ViewModel.TreeView();
            tree.Title = "Param";
            foreach (var itm in parent)
            {
                tree.TreeData.Add(new ViewModel.TreeDTO
                {
                    Id = itm.PId,
                    PId = itm.ParentId,
                    Text = itm.PName,
                    IsGroup = itm.IsGroup,
                });
                if (itm.IsGroup == false)
                {
                    newList.Add(new Param { ParentId = itm.ParentId, PId = itm.PId, PName = itm.PName });
                }
            }

            foreach (var itm in tree.TreeData)
            {
                GenerateChildList(list, itm.Id, newList);
            }
            return newList;

        }
        public List<Param> ListAllParameters(int id)
        {

            List<Param> list = uow.Repository<Param>().GetAll().ToList();
            ViewModel.TreeView tree = this.GenerateTree(list, id);
            List<Param> paramLst = new List<Param>();
            var childList = GenerateChildList(list, id);

            foreach (var item in childList)
            {
                var listVal = uow.Repository<Param>().GetSingle(x => x.PId == item.PId);
                if (listVal.paramValue != null)
                {
                    if (listVal.paramValue.DTId == 5)
                    {
                        if (listVal.paramValue.PValue != null)
                        {
                            var pValue = GetOption(listVal.paramValue.paramScript.PScript);
                            pValue = pValue.Where(x => x.Value == listVal.paramValue.PValue).ToList();

                            listVal.paramValue.PValue = pValue[0].Text + " [" + listVal.paramValue.PValue + "]";
                        }

                    }
                }
                paramLst.Add(listVal);
            }


            return paramLst;

        }
        public ParamValue ParameterValues(int id)
        {
            ParamValue paramList = uow.Repository<ParamValue>().GetSingle(x => x.PId == id);
            var dataType = uow.Repository<Datatype>().GetSingle(x => x.DTId == paramList.DTId);
            paramList.DTId = dataType.DTId;
            if (dataType.DTId == 5)
            {
                var paramScript = uow.Repository<ParamScript>().GetSingle(x => x.PId == id);
                paramList.paramScript = paramScript;
            }
            return paramList;

        }

        #endregion
    }
}