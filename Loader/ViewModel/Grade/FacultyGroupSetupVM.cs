using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel.Grade
{
    public class FacultyGroupSetupVM
    {
        public int FGSId { get; set; }
        public int FGId { get; set; }
        public int SubId { get; set; }
        public string SubjectType { get; set; }
        public string Prefix { get; set; }
        public string FacultyGroupName { get; set; }
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public decimal FullMarks { get; set; } 
        public decimal Passmarks { get; set; }
        public string Type { get; set; }
        public string ThSubCode { get; set; }
        public decimal ThFullMarks { get; set; } 
        public string ThSubjectName { get; set; }
        public decimal ThPassmarks { get; set; } 
        public string InSubjectCode { get; set; }
        public decimal InFullMarks { get; set; } 
        public string INSubjectName { get; set; }

        public decimal InPassmarks { get; set; } 
        public int PostedBy { get; set; }
    }
}