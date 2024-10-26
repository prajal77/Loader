using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;
using Loader.ViewModel;
using PagedList;
using System.Collections.Generic;

namespace Loader.Controllers
{
    //[MyAuthorize]
    public class UserVSBranchController : Controller
    {

        private Service.UserVSBranchService UserVSBranchService = null;

        public UserVSBranchController()
        {
            UserVSBranchService = new Service.UserVSBranchService();
        }
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                
                ViewBag.ViewType = 1;
                return View(UserVSBranchService.GetAll().ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.ViewType = ViewType;
            return PartialView(UserVSBranchService.GetAll().ToList());
        }

        public ActionResult Details(int id = 0)
        {
            UserVSBranch UserVSBranch = UserVSBranchService.GetSingle(id);
            if (UserVSBranch == null)
            {
                return HttpNotFound();
            }
            return View(UserVSBranch);
        }

        public ActionResult GetUserDetails(string query = null, int? userId = 0, int? page = 1)
        {
            const int pageSize = 10;
            var userList = UserVSBranchService.GetAllUsers();
            ApplicationDbContext ctx = new ApplicationDbContext();
            IPagedList<ApplicationUser> listUser;
            if (!string.IsNullOrEmpty(query))
            {
                listUser = new Loader.Repository.GenericUnitOfWork().Repository<Models.ApplicationUser>().FindBy(m => m.UserName.ToUpper().Contains(query.ToUpper())).OrderBy(x => x.UserName).ToPagedList(page ?? 0, pageSize);
            }
            else
            {
                listUser = ctx.Users.OrderBy(x => x.UserName).ToPagedList(page ?? 0, pageSize);
            }

            int totalCount = listUser.TotalItemCount;
            List<ApplicationUser> finalList = listUser.ToList();
            
            var pagedList = new Pagination<ApplicationUser>(finalList.AsQueryable(), page ?? 0, pageSize, totalCount);
            ViewBag.CountPager = new Pagination<ApplicationUser>(finalList.AsQueryable(), page ?? 0, pageSize, totalCount).TotalPages;
            ViewBag.ActivePager = 1;


            return PartialView(pagedList);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int UserVSBranchId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                UserVSBranch UserVSBranchDTO = new UserVSBranch();
                
                if (UserVSBranchId != 0)
                {

                    UserVSBranchDTO = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == UserVSBranchId);

                }
                else
                {
                    UserVSBranchDTO.Id = activeId;
                }

                ViewBag.ActiveText = activeText;
                
                return PartialView(UserVSBranchDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(UserVSBranch UserVSBranch, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (UserVSBranch.Id == 0)
                    {
                        UserVSBranchService.Save(UserVSBranch);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        UserVSBranchService.Save(UserVSBranch);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == UserVSBranch.Id);
                        if (parentNode == null)
                        {
                            return RedirectToAction("Create", new { activeText = "Root" });
                        }


                        return RedirectToAction("Create", new {  activeId = parentNode.Id });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(UserVSBranch);

        }

    

        public ActionResult _CreateAction()
        {
            return PartialView();
        }


    
        public ActionResult Edit(int id = 0)
        {
            UserVSBranch UserVSBranch = UserVSBranchService.GetSingle(id);
            if (UserVSBranch == null)
            {
                return HttpNotFound();
            }
            return View(UserVSBranch);
        }


        [HttpPost]
        public ActionResult Edit(UserVSBranch UserVSBranch)
        {
            if (ModelState.IsValid)
            {
                UserVSBranchService.Save(UserVSBranch);

                return RedirectToAction("Index");
            }
            return View(UserVSBranch);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            UserVSBranch UserVSBranch = new Loader.Repository.GenericUnitOfWork().Repository<UserVSBranch>().GetSingle(x => x.Id == id);
            bool deleteConfirm = true;

            //var checkinEmployee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.Id == id);
            if (UserVSBranch == null)
            {
                deleteConfirm = false;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(UserVSBranch);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int UserVSBranchId)
        {
            UserVSBranchService.Delete(UserVSBranchId);
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

    }
}
