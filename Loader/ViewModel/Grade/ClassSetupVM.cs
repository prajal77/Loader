using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel.Grade
{
    public class ClassSetupVM
    {
        public int CGsId { get; set; }
        public int GradeId { get; set; }
        public int Mgid { get; set; }
        public int SecId { get; set; }
        public int ETermId { get; set; }
        public string TermName { get; set; }
        public string GradeName { get; set; }
        public int SectionId { get; set; }
        public string SectionName { get; set; }
        public string ExamName { get; set; }
        public int StatusId { get; set; }
        public int PostedBy { get; set; }

    }
}