using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Loader.DataAccess
{
    public class Paging
    {
        public int NoOfRecords { get; set; }

        public int PageSize { get; set; }

        public int NoOfPages
        {
            get
            {
                if (PageSize == 0)
                {
                    return 1;
                }

                return Convert.ToInt32(Math.Ceiling(double.Parse(NoOfRecords.ToString()) / double.Parse(PageSize.ToString())));
            }
        }

        public int CurrentPageNo { get; set; }

        public Paging()
        {
            PageSize = 10;
        }
    }
}