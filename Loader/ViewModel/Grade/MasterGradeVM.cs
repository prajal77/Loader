using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.ViewModel.Grade
{
    public class MasterGradeVM
    {
        public int Mgid { get; set; }
        public string GradeName { get; set; }
        public int StatusId { get; set; }
        public string StatusName { get; set; }
        public int rowId { get; set; }
    }
}