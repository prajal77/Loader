using System.Linq;
using System.Web.Mvc;
using Loader.Models;
using System;
using System.Web;

namespace Loader.Controllers
{
    [MyAuthorize]
    public class DepartmentController : Controller
    {

        private Service.DepartmentService DepartmentService = null;

        public DepartmentController()
        {
            DepartmentService = new Service.DepartmentService();
        }
        public ActionResult Index()
        {
            if (Request.IsAjaxRequest() == true)
            {
                ViewModel.TreeView tree = DepartmentService.GetDepartmentGroupTree();
                ViewBag.treeview = tree;
                ViewBag.Address = DepartmentService.GetAddress(0);
                ViewBag.ViewType = 1;
                ViewBag.ParentDepartmentId = 0;

                var withHeirarchy = DepartmentService.WithHeirarchy();
                ViewBag.Heirarchy = 0;
                if (withHeirarchy == "true")
                {
                    ViewBag.Heirarchy = 1;
                }
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, 0, "Available Department Group");

                return View(DepartmentService.GetAllOfParent(0).ToList());
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult _Details(int parentId, bool allowSelectGroup, int ViewType = 1)
        {
            ViewBag.Address = DepartmentService.GetAddress(parentId);
            ViewBag.ParentDepartmentId = parentId;
            ViewBag.ViewType = ViewType;
            return PartialView(DepartmentService.GetAllOfParent(parentId).ToList());
        }

        public ActionResult Details(int id = 0)
        {
            Department Department = DepartmentService.GetSingle(id);
            if (Department == null)
            {
                return HttpNotFound();
            }
            return View(Department);
        }

        [HttpGet]
        public ActionResult Create(string activeText, int activeId, int DepartmentId = 0)
        {
            if (Request.IsAjaxRequest())
            {
                Department DepartmentDTO = new Department();
                if (DepartmentId != 0)
                {

                    DepartmentDTO = new Loader.Repository.GenericUnitOfWork().Repository<Department>().GetSingle(x => x.DepartmentId == DepartmentId);

                }
                else
                {
                    DepartmentDTO.PDeptId = activeId;
                }
                var withHeirarchy = DepartmentService.WithHeirarchy();
                ViewBag.Heirarchy = 0;
                if (withHeirarchy == "true")
                {
                    ViewBag.Heirarchy = 1;
                }
                ViewBag.ActiveText = activeText;
                ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, false, 0, 0, "Available Department Group");
                return PartialView(DepartmentDTO);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpPost]
        public ActionResult Create(Department Department, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    if (Department.DepartmentId == 0)
                    {
                        DepartmentService.Save(Department);
                        return RedirectToAction("Create", new { activeText = "Root", activeId = 0 });
                    }
                    else
                    {
                        DepartmentService.Save(Department);
                        var parentNode = new Loader.Repository.GenericUnitOfWork().Repository<Department>().GetSingle(x => x.DepartmentId == Department.PDeptId);
                        if (parentNode==null)
                        {
                            return RedirectToAction("Create", new { activeText ="Root", activeId = Department.PDeptId });
                        }
                        

                        return RedirectToAction("Create", new { activeText = parentNode.DeptName, activeId = parentNode.DepartmentId });
                    }


                }
                catch (Exception ex)
                {
                    return JavaScript(ex.Message);
                }


            }
            return View(Department);

        }

        [HttpGet]
        public ActionResult _UpdateDepartmentTree(int selectedNode, bool allowSelectGroupNode, bool withImageIcon, bool withCheckBox, int withOutMe = 0, int rootNode = -1)
        {
            ViewData["AllowSelectGroup"] = allowSelectGroupNode;
            ViewData["WithImageIcon"] = withImageIcon;
            ViewData["WithCheckBox"] = withCheckBox;
            ViewData["param"] = new Loader.ViewModel.TreeViewParam(false, true, true, 0, selectedNode, "Available Department Group");

            ViewModel.TreeView tree = new ViewModel.TreeView();
            if (rootNode < 0)
            {
                tree = DepartmentService.GetDepartmentGroupTree(withOutMe);

            }
            else
            {
                tree = DepartmentService.GetDepartmentGroupTree(rootNode, withOutMe);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult _CreateAction()
        {
            return PartialView();
        }


        [HttpPost]
        public ActionResult _GetDepartmentTreePopup(Loader.ViewModel.TreeViewParam param)
        {

            ViewData["param"] = param;

            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = DepartmentService.GetDepartmentGroupTree(filter);
            }
            else
            {
                tree = DepartmentService.GetDepartmentGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewPopup", tree);

        }

        [HttpPost]
        public ActionResult _GetDepartmentTree(Loader.ViewModel.TreeViewParam param)
        {
            ViewData["param"] = param;
            var filter = param.Filter == null ? "" : param.Filter;

            ViewModel.TreeView tree = new ViewModel.TreeView();

            if (param.WithOutMe == 0)
            {
                tree = DepartmentService.GetDepartmentGroupTree(filter);
            }
            else
            {
                tree = DepartmentService.GetDepartmentGroupTree(param.WithOutMe, filter);
            }
            return PartialView("_TreeViewBody", tree);
        }

        public ActionResult Edit(int id = 0)
        {
            Department Department = DepartmentService.GetSingle(id);
            if (Department == null)
            {
                return HttpNotFound();
            }
            return View(Department);
        }


        [HttpPost]
        public ActionResult Edit(Department Department)
        {
            if (ModelState.IsValid)
            {
                DepartmentService.Save(Department);

                return RedirectToAction("Index");
            }
            return View(Department);
        }

        [HttpGet]
        public JsonResult Delete(int id = 0)
        {
            Department Department = new Loader.Repository.GenericUnitOfWork().Repository<Department>().GetSingle(x => x.PDeptId == id);
            bool deleteConfirm = false;
           
            var checkinEmployee = new Loader.Repository.GenericUnitOfWork().Repository<Employee>().GetSingle(x => x.DeptId == id);
            if (Department == null && checkinEmployee==null)
            {
                deleteConfirm = true;
            }
            return Json(deleteConfirm, JsonRequestBehavior.AllowGet);
            //return View(Department);
        }
        [HttpPost]
        public ActionResult DeleteConfirm(int DepartmentId)
        {
            DepartmentService.Delete(DepartmentId);
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
