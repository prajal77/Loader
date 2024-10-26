using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Configuration;

namespace Loader.ViewModel.GenerateGradeSheet
{
    public class GradeSheetSetupVM
    {
        public int FYId { get; set; }
        public long FiscalYear { get; set; }
        public int NYear { get; set; }
        public int Mgid { get; set; }
        public int SecId { get; set; }
        public int GradeId { get; set; }
        public string GradeName { get; set; }
        public int sectionId { get; set; }
        public string SectionName { get; set; }
        public int ETermId { get; set; }
        public string TermName { get; set; }
        public string RegistrationNo { get; set; }
        public int YearBS { get; set; }
        public int YearAD { get; set; }
        public string SubjectCode { get; set; }
        public string Subjects { get; set; }
        public string FacultyGroupName { get; set; }
        public DateTime DOBAD { get; set; }
        public string DOBBS { get; set; }

        //--------GradeSection---------
        public decimal CreditHour { get; set; }
        public decimal GradePoint { get; set; }
        public string Grade { get; set; }
        public string FinalGrade { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal GPA { get; set; }
        public int StudentId { get; set; }
        //-------------student detail--------
        public string StudentName { get; set; }
        public string RollNo { get; set; }
        public int EnrollFy { get; set; }
        public string FacultyName { get; set; }


      


    }
}