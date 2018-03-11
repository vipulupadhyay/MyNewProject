using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeForm.Models
{
    public class jQueryDataTableParamModel
    {
        public string sEcho { get; set; }

        public string sSearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }

        public int? iColumns { get; set; }

        public int? iSortingCols { get; set; }
        public string sColumns { get; set; }

        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }

        public string sSearch_0 { get; set; }
        public string sSearch_1 { get; set; }
        public string sSearch_2 { get; set; }
        public string sSearch_3 { get; set; }

    }

    public class DataTableProperty
    {
        public int PageNo { get; set; }

        public int RecordPerPage { get; set; }

        public string SortField { get; set; }

        public string SortOrder { get; set; }

        public string Filter { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }

        
    }

}