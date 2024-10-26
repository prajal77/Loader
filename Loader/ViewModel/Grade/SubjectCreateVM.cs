using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel.Grade
{
    public class SubjectCreateVM
    {
        public int Sid { get; set; }
        public string ThSubjectCode { get; set; }
        public int SubCode { get; set; }
        public string ThSubjectName { get; set; }
        public decimal ThFullMarks { get; set; }
        public decimal ThCreditHR { get; set; }
        public string INSubCode { get; set; }
        public string INSubjectName { get; set; }
        public decimal INFullMarks { get; set; }
        public decimal INCreditHR { get; set; }
        public int postedby { get; set; }
        public decimal FullMarks { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
    }
}