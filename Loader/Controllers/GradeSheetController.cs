using Loader.Service;
using Loader.ViewModel.GenerateGradeSheet;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Loader.Controllers
{
    public class GradeSheetController : Controller
    {
        private readonly GradeSheetService _gradeSheetService;
        public GradeSheetController()
        {
            _gradeSheetService=new GradeSheetService();
        }
        // GET: GradeSheet
        [HttpGet]
        public ActionResult GradeGenerator(int? gradeId=null, int? sectionId=null, int? examId=null, int? fiscalyear=null, int? studentId=null)
        {
            //var gradeSheet = new List<GradeSheetSetupVM>();
            ViewBag.GetGrade = _gradeSheetService.GetGradeLists();
            ViewBag.GetCompanyDetails = _gradeSheetService.GetCompanyDetails();
            if (gradeId.HasValue && sectionId.HasValue && examId.HasValue && fiscalyear.HasValue && studentId.HasValue)
            {
                var studentDetail = _gradeSheetService.GetGradeSheetByStudent(gradeId.Value, sectionId.Value, examId.Value, fiscalyear.Value, studentId.Value).ToList();
                return View(studentDetail);
            }
            return View(new List<GradeSheetSetupVM>());
        }

        /* public ActionResult GradePDFGenerator(int gradeId, int sectionId, int examId, int fiscalyear, int studentId)
         {
             var studentDetails = _gradeSheetService.GetGradeSheetByStudent(gradeId, sectionId, examId, fiscalyear, studentId).ToList();
             var companyInfo = _gradeSheetService.GetCompanyInfo();

             MemoryStream stream = _gradeSheetService.GenerateGradeSheetPdf(studentDetails, companyInfo);
             stream.Position = 0;

             return new FileStreamResult(stream, "application/pdf")
             {
                 FileDownloadName = $"Grade_Sheet.pdf"
             };

         }*/

        /* public ActionResult GradePDFGenerator(int gradeId, int sectionId, int examId, int fiscalyear, int studentId)
         {
             try
             {
                 var studentDetails = _gradeSheetService.GetGradeSheetByStudent(gradeId, sectionId, examId, fiscalyear, studentId)?.ToList();

                 var companyInfo = _gradeSheetService.GetCompanyInfo()?.ToList();

                 MemoryStream stream = _gradeSheetService.GenerateGradeSheetPdf(studentDetails, companyInfo);
                 stream.Position = 0;
                 return new FileStreamResult(stream, "application/pdf")
                 {
                     FileDownloadName = $"Grade_Sheet.pdf"
                 };
             }
             catch (Exception ex)
             {
                 // Log the error here
                 return new HttpStatusCodeResult(500, "An error occurred while generating the PDF");
             }
         }*/
     
        public ActionResult GradePDFGenerator(int gradeId, int sectionId, int examId, int fiscalyear, int studentId)
        {
            try
            {
                var gradeSheetData = _gradeSheetService.GetGradeSheetByStudent(gradeId, sectionId, examId, fiscalyear, studentId)?.ToList();

                var companyInfo = _gradeSheetService.GetCompanyInfo().FirstOrDefault();

                //Create the PDF using your existing view
                return new ActionAsPdf("GradeSheetPDF", new
                {
                    model = gradeSheetData,
                    companyInfo = companyInfo
                })
                {
                    FileName = $"GradeSheet.pdf",
                    PageSize = Rotativa.Options.Size.A4,
                    PageOrientation = Rotativa.Options.Orientation.Portrait,
                    CustomSwitches = "--margin-top 10 --margin-right 10 --margin-bottom 10 --margin-left 10",
                    PageMargins = new Rotativa.Options.Margins(10, 10, 10, 10)
                };
            }
            catch (Exception ex)
            {
                // Log the error here
                return new HttpStatusCodeResult(500, "An error occurred while generating the PDF");
            }
        }
        // PDF Template Action
        [NonAction]
        public ActionResult GradeSheetPDF(List<GradeSheetSetupVM> model,CompanyInfo companyInfo)
        {
            if (model == null || !model.Any())
            {
                return new HttpStatusCodeResult(400, "Invalid model data");
            }
            ViewBag.companyInfo = companyInfo ;
            
            return View(model);
        }

        public JsonResult GetSectionDropList(int id)
        {

            var sectionlist = _gradeSheetService.GetSectionsLists(id);
            return Json(sectionlist, JsonRequestBehavior.AllowGet);
           
        }

        public JsonResult GetStudentDropList(int id)
        {
            var studentList = _gradeSheetService.GetStudentName(id);
            return Json(studentList, JsonRequestBehavior.AllowGet);
            
        }
    }
}