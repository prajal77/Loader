using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;
using System.Collections.Generic;
using Loader.ViewModel;
using Loader.Service;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Data.Entity;
using PagedList;
using System.Net;
using Loader.Helper;

namespace Loader.Controllers
{
    //[Authorize]
    [MyAuthorize]
    public class UsersController : Controller
    {
        private Service.UsersService UsersService = null;
        private ApplicationUserManager _userManager;


        public UsersController()
        {
            UsersService = new Service.UsersService();
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                //ViewModel.SearchViewModel employee = UsersService.GetEmployeeGroupTree();
                //ViewBag.EmployeeTree = employee;
                //ViewModel.TreeView menuTemplate = UsersService.GetMenuTemplateGroupTree();
                //ViewBag.MenuTemplateTree = menuTemplate;
                ViewBag.Id = 0;
                ViewBag.WithEmployee = UsersService.WithEmployee();
                ViewBag.ViewType = 1;
                var list = UserManager.Users.AsQueryable();
                int? page = 1;
                const int pageSize = 14;
                //var list = new Loader.Repository.GenericUnitOfWork().Repository<Models.Users>().GetAll().AsQueryable();
                var dataList = list.OrderBy(m => m.Id);
                var pagedList = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize);
                ViewBag.CountPager = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
                ViewBag.ActivePager = 1;

                return View(pagedList);
            }
            return RedirectToAction("Index", "Home");

        }

        public ActionResult _Details(int ViewType = 1, string query = null, int? page = 1)
        {
            const int pageSize = 14;

            var list = new Loader.Repository.GenericUnitOfWork().Repository<ApplicationUser>().GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
                ViewBag.Query = query;
            }


            var dataList = list.OrderBy(m => m.Id);

            var pagedList = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize);

            //double total = (int)Math.Ceiling(list.Count() / (double)pageSize);

            ViewBag.CountPager = new Pagination<ApplicationUser>(dataList, page ?? 0, pageSize).TotalPages;
            //ViewBag.hasPrevious = new Pagination<Users>(dataList, page ?? 0, pageSize).HasPreviousPage;
            ViewBag.ViewType = ViewType;
            ViewBag.ActivePager = page;
            return PartialView("_Details", pagedList);
            //ViewBag.Address = UsersService.GetAddress(parentId);

        }

        //public ActionResult Details(int id = 0)
        //{
        //    Users Users = UsersService.GetSingle(id);
        //    if (Users == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(Users);
        //}

        [HttpGet]
        public ActionResult Create(int UsersId = 0)
        {
            //ViewModel.SearchViewModel employeeTree = UsersService.GetEmployeeGroupTree();
            //ViewBag.EmployeeTree = employeeTree;
            ViewBag.WithEmployee = UsersService.WithEmployee();
            ViewBag.Id = 0;
            if (Request.IsAjaxRequest())
            {

                RegisterViewModel createDTO = new RegisterViewModel();
                ApplicationUser usersDTO = new ApplicationUser();
                if (UsersId != 0)
                {
                    usersDTO = UserManager.FindById(UsersId);
                    createDTO.UserName = usersDTO.UserName;
                    createDTO.IsUnlimited = usersDTO.IsUnlimited;
                    if (createDTO.IsUnlimited == false)
                    {
                        createDTO.To = usersDTO.TillDate;
                        createDTO.From = usersDTO.EffDate;
                    }
                    createDTO.EmployeeId = usersDTO.EmployeeId;
                    createDTO.Email = usersDTO.Email;
                    createDTO.Password = usersDTO.PasswordHash;
                    createDTO.ConfirmPassword = usersDTO.PasswordHash;

                    createDTO.MTId = usersDTO.MTId;
               
                    ViewBag.Id = 1;

                    //ViewBag.MTText = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == usersDTO.MTId).MTName;

                    ViewBag.MTText = CommonDropdownService.DropDownService.MenuTemplateDropDown();

                    if (ViewBag.WithEmployee == true)
                    {
                        if (usersDTO.EmployeeId != 0)
                        {
                            //ViewBag.EmployeeText = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.EmployeeId == usersDTO.EmployeeId).EmployeeName;

                            ViewBag.EmployeeText = CommonDropdownService.DropDownService.EmployeeDropDown();
                        }
                    }
                }
                else
                {
                    usersDTO.IsUnlimited = true;
                }
                //var createObj = UserManager.Users.Where(x=>x.Id==UsersId) Select(x => new RegisterViewModel
                //{
                // UserName=x.UserName,
                // Email=x.Email,
                // EmployeeId=x.EmployeeId,
                // MTId=x.MTId,
                // From=x.From,
                // To=x.To,
                // IsUnlimited=x.IsUnlimited,

                //});


                //Users UsersDTO = new Users();
                //if (UsersId != 0)
                //{

                //    UsersDTO = new Loader.Repository.GenericUnitOfWork().Repository<Users>().GetSingle(x => x.UserId == UsersId);

                //    var MTName = new Loader.Repository.GenericUnitOfWork().Repository<MenuTemplate>().GetSingle(x => x.MTId == UsersDTO.MTId).MTName;
                //    //ViewBag.DeptText = deptName;
                //    //ViewBag.DesgText = designationName;
                //    if (ViewBag.WithEmployee == true)
                //    {
                //        var empName = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.EmployeeId == UsersDTO.EmployeeId).EmployeeName;
                //        ViewBag.EmployeeText = empName;
                //    }
                //    ViewBag.MTText = MTName;

                //    //if (UsersDTO.Photo != null)
                //    //{
                //    //    ViewBag.Image = Convert.ToBase64String(UsersDTO.Photo, Base64FormattingOptions.None);
                //    //}

                //}
                //else
                //{
                //    UsersDTO.IsUnlimited = true;
                //}
                ////ViewData["param"] = new Loader.ViewModel.TreeViewParam(true, true, false, 0, 0, "Available MenuTemplate Group");
                return PartialView(createDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult GetFromToDate()
        {
            RegisterViewModel userObj = new RegisterViewModel();
            return PartialView("_GetFromToDate", userObj);
        }

        public JsonResult CheckPassword(string ReEnterPassword, string Password)
        {
            if (ReEnterPassword == Password)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }


        //[Authorize]
        [HttpPost]
        public ActionResult Create(Users Users)
        {
            //if(Users.UserId!=0)
            //{

            //    Users.Password = "11111111";
            //    Users.ReEnterPassword = "11111111";
            //}
            if (ModelState.IsValid)
            {

                //if (file != null)
                //{
                //    using (var reader = new System.IO.BinaryReader(file.InputStream))
                //    {
                //        Users.Photo = reader.ReadBytes(file.ContentLength);
                //    }
                //}
                try
                {
                    if (Users.UserId == 0)
                    {
                        UsersService.Save(Users);
                        return RedirectToAction("Create", new { UsersId = 0 });
                    }
                    else
                    {
                        UsersService.Save(Users);
                        return RedirectToAction("Create", new { UsersId = Users.UserId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(Users);

        }


        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetEmployeeTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

              if (param.Title == "Select MenuTemplate")
                {
                    tree = UsersService.GetMenuTemplateGroupTree(filter);
                }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetEmployeeSearchPopup(Loader.ViewModel.SearchViewModel param,string query=null, int? page = 1)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;
            ViewModel.TreeView tree = new TreeView();

            ViewModel.SearchViewModel search = new ViewModel.SearchViewModel();

            const int pageSize = 10;
            if (param.Title == "Select Employee")
            {
                ApplicationDbContext ctx = new ApplicationDbContext();
                IPagedList<Employee> listEmp;
                if (!string.IsNullOrEmpty(query))
                {
                    listEmp = ctx.Employee.OrderBy(x => x.EmployeeName).Where(m => m.EmployeeName.ToUpper().Contains(query.ToUpper())).ToPagedList(page ?? 0, pageSize);
                    // ViewBag.Query = query;
                }
                else
                {
                    listEmp = ctx.Employee.OrderBy(x=>x.EmployeeName).ToPagedList(page ?? 0, pageSize);
                }

                int totalCount = listEmp.TotalItemCount;
                List<Employee> finalList = listEmp.ToList();
                search = UsersService.GetEmployeeGroupTree(filter, finalList);
                var list = search.SearchData.AsQueryable();
            
                if (!string.IsNullOrEmpty(query))
                {
                    list = list.Where(m => m.Text.Contains(query));
                   // ViewBag.Query = query;
                }
                var pagedList = new Pagination<SearchDTO>(list, page ?? 0, pageSize,totalCount);
                ViewBag.CountPager = new Pagination<SearchDTO>(list, page ?? 0, pageSize,totalCount).TotalPages;
                ViewBag.ActivePager = 1;
                search.SearchData = pagedList;
               
            }

            return PartialView("_SearchViewPopUp", search);
        }
        [HttpPost]
        public ActionResult _GetEmployeeSearchTree(Loader.ViewModel.TreeViewParam param, string query = null, int? page = 1)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;
            ViewModel.SearchViewModel search = new ViewModel.SearchViewModel();
            ViewModel.TreeView menutemplate = new TreeView();

            const int pageSize = 10;
            if (param.Title == "Select Employee")
            {
                ApplicationDbContext ctx = new ApplicationDbContext();
                IPagedList<Employee> listEmp;
                if (!string.IsNullOrEmpty(query))
                {
                    listEmp = ctx.Employee.OrderBy(x=>x.EmployeeName).Where(m => m.EmployeeName.ToUpper().Contains(query.ToUpper())).ToPagedList(page ?? 0, pageSize);
                    // ViewBag.Query = query;
                }
                else
                {
                    listEmp = ctx.Employee.OrderBy(x => x.EmployeeName).ToPagedList(page ?? 0, pageSize);
                }
                var totalCount = listEmp.TotalItemCount;
                List<Employee> treeList = listEmp.ToList();
                //int totalCount = new Loader.Repository.GenericUnitOfWork().Repository<Models.Employee>().GetAll().Count();
                search = UsersService.GetEmployeeGroupTree(filter, treeList);

                var list = search.SearchData.AsQueryable();

               

                var pagedList = new Pagination<SearchDTO>(list, page ?? 0, pageSize,totalCount);
                ViewBag.CountPager = new Pagination<SearchDTO>(list, page ?? 0, pageSize,totalCount).TotalPages;
                ViewBag.ActivePager = page;
                search.SearchData = pagedList;
             
            }
            return PartialView("_SearchViewBody", search);


        }
        public ActionResult PagingData(int id, int next = 0, int previous = 0)
        {
            int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Users>().GetAll().Count();
            var pagingCount = listCount % 10;
            List<Users> empList = UsersService.GetUsersList();
            return View();
        }
        //public ActionResult PagerIndex(int page = 1)
        //   {
        //   int listCount = new Loader.Repository.GenericUnitOfWork().Repository<Users>().GetAll().Count();
        //   const int PageSize = 1;
        //       var pageCount = (listCount + PageSize - 1) / PageSize;

        //       ViewData["page"] = page;
        //       ViewData["count"] = pageCount;

        //       return View(listCount((page - 1) * PageSize).Take(PageSize));
        //   }
        public ActionResult PagerIndex(String query, int? page)
        {
            const int pageSize = 4;

            var list = new Loader.Repository.GenericUnitOfWork().Repository<Users>().GetAll().AsQueryable();

            if (!string.IsNullOrEmpty(query))
            {
                list = list.Where(m => m.UserName.Contains(query));
            }

            var dataList = list.OrderByDescending(m => m.UserId);

            var pagedList = new Pagination<Users>(dataList, page ?? 0, pageSize);

            return View(pagedList);
        }


        //[HttpPost]
        //public ActionResult _GetEmployeeTree(Loader.ViewModel.TreeViewParam param)
        //{
        //    ViewData["param"] = param;
        //    var filter = param.Filter == null ? "" : param.Filter;

        //    ViewModel.TreeView tree = new ViewModel.TreeView();

        //    if (param.WithOutMe == 0)
        //    {
        //        tree = UsersService.GetEmployeeGroupTree(filter);
        //    }
        //    //else
        //    //{
        //    //    tree = UsersService.GetEmployeeGroupTree(param.WithOutMe, filter);
        //    //}
        //    return PartialView("_TreeViewBody", tree);
        //}

        public ActionResult Edit(int id = 0)
        {
            Users Users = UsersService.GetSingle(id);
            if (Users == null)
            {
                return HttpNotFound();
            }
            return View(Users);
        }


        [HttpPost]
        public ActionResult Edit(Users Users)
        {
            if (ModelState.IsValid)
            {
                UsersService.Save(Users);

                return RedirectToAction("Index");
            }
            return View(Users);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Users Users = new Loader.Repository.GenericUnitOfWork().Repository<Users>().GetSingle(x => x.UserId == id);
            bool deleteConfirm = true;

            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Users);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int UsersId)
        {
            UsersService.Delete(UsersId);
            return RedirectToAction("Index");
        }
        public ActionResult DisplayImage(HttpPostedFileBase file)
        {
            using (var reader = new System.IO.BinaryReader(file.InputStream))
            {
                byte[] ContentImage = reader.ReadBytes(file.ContentLength);
                var ImageContent = Convert.ToBase64String(ContentImage, Base64FormattingOptions.None);
                return Json(ImageContent, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult userpasswdReset()
        {
            var userList = UserManager.Users.AsQueryable();
            return PartialView(userList);
        }

        [HttpGet]
        public ActionResult _userpasswordReset(int userId=0)
        {
            if(userId == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userData = UserManager.FindById(userId);
            ViewBag.UserName = userData.UserName;
            return View(userData);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> _userpasswordReset(ApplicationUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await UserManager.FindByEmailAsync(model.Email);
            var removePassword = UserManager.RemovePassword(user.Id);
            if (removePassword.Succeeded)
            {
                //remove password success
                var addPassword = UserManager.AddPassword(user.Id, model.NewPassword);
                if (addPassword.Succeeded)
                {
                    return RedirectToAction("userpasswdReset", "Users");
                }
            }

            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}
