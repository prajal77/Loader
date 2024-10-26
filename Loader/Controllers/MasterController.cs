using Loader.Service;
using Loader.ViewModel.Grade;
using Newtonsoft.Json;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace Loader.Controllers
{
    public class MasterController : Controller
    {
        private readonly MasterSetupService _masterService = null;

        public MasterController()
        {
            _masterService = new MasterSetupService();
        }

    //-------------------------------------------------------For GradeSetup -------------------------------------------------

        public ActionResult GradeSetup()
        {
            var res = _masterService.GetGradeList();
            return View(res);
        }

        public ActionResult AddMasterGrade()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddMasterGrade(MasterGradeVM dto)
        {
            try
            {
                _masterService.SaveMasterGrade(dto);
                var res = _masterService.GetGradeList();
                TempData["Message"] = "Grade Added Successfully..";
                return PartialView("GradeSetup", res);
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
                return PartialView("_Error");
            }
        }

        public ActionResult UpdateMasterGrade(int id)
        {
            MasterGradeVM obj = _masterService.GetGradeById(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult UpdateMasterGrade(MasterGradeVM obj)
        {
            if (ModelState.IsValid)
            {
                _masterService.SaveMasterGrade(obj);
                var res = _masterService.GetGradeList();
                TempData["Message"] = "Grade Updated Successfully..";
                return RedirectToAction("GradeSetup", "Master");
            }
            return View();
        }

        //-------------------------------------------------------For FacultySetup -------------------------------------------------


        public ActionResult FacultySetup()
        {
            List<FacultyVM> obj = _masterService.GetAllFacultyList();
            return View(obj);
        }

        [HttpGet]
        public ActionResult AddFaculty()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddFaculty(FacultyVM dto)
        {
            if (ModelState.IsValid) {
                _masterService.UpsertFaculty(dto);
                TempData["Message"] = "Inserted Successfully!";
                return RedirectToAction("FacultySetup", "Master");            
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateFaculty(int id)
        {
            FacultyVM obj = _masterService.GetFacultyById(id);
            return View(obj);
        }


        [HttpPost]
        public ActionResult UpdateFaculty(FacultyVM obj)
        {
            if (ModelState.IsValid)
            {
                _masterService.UpsertFaculty(obj);
                TempData["Message"] = "Updated Successfully..";
                return RedirectToAction("FacultySetup", "Master");
            }
            return View();
        }


        //-------------------------------------------------------For SubjectSetup -------------------------------------------------
        public ActionResult SubjectSetup()
        {
            List<SubjectCreateVM> sublist = _masterService.GetAllSubjectList();
            return View(sublist);
        }

        public ActionResult AddSubject()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSubject(SubjectCreateVM dto)
        {
            if (ModelState.IsValid)
            {
                _masterService.CreateSubject(dto);
                return RedirectToAction("SubjectSetup", "Master");
            }
            return View();
        }

        
        //----------------------------Class(Grade)--------------------------------
       
      public ActionResult GradeExam()
        {
            List<ClassSetupVM> list = _masterService.GetClassGrade();
            return View(list);
        }

        public ActionResult AddClass(int? id)
        {
          ClassSetupVM classSetup = new ClassSetupVM();

            if(id.HasValue)
            {
               ClassSetupVM model = _masterService.GetClassById(id.Value);
                ViewBag.GradeLists = _masterService.GetGrade();
                return View(model);
            }
          
            ViewBag.GradeLists = _masterService.GetGrade();
            return View(classSetup);
        }

        [HttpPost]
        public ActionResult AddClass(ClassSetupVM model)
        {
            if (ModelState.IsValid)
            {
                _masterService.UpsertClass(model);
                return RedirectToAction("GradeExam");
            }
            ViewBag.GradeLists = _masterService.GetGrade();
            return View("model");
        }

        public JsonResult GetSectionDropList(int id)
        {
            var sectionlist = _masterService.GetSectionLists(id);
            return Json(sectionlist,JsonRequestBehavior.AllowGet);
        }

        //------------------------------------Faculty Group Vs Subject List Setup-------------------------

        public ActionResult FacultyGroupSetup()
        {
            List<FacultyGroupSetupVM> faculty = _masterService.GetAllFacultyGroup();
            return View(faculty);
        }

        public ActionResult AddFacultyGroup()
        {
            var model = new FacultyGroupSetupVM();
    
            return View(model);
        }

        [HttpPost]
        public ActionResult AddFacultyGroup(List<FacultyGroupSetupVM> dataArray)
        {
            foreach (var item in dataArray)
            {
                if (ModelState.IsValid)
                {
                    _masterService.CreateFaculty(item);
                }
            }
            //return Json(new { success = true, message = "Data saved successfully" });
            return RedirectToAction("FacultyGroupSetup");

        }

    }
}