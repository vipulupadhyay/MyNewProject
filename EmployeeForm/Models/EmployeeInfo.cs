using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeForm.Models
{



    public class EmployeeManagementList
    {
        public List<EmployeeInfo> EmployeeInfoList { get; set; }


        public int totalNumberofRecord { get; set; }

        public int filteredRecord { get; set; }
    }

    public class EmployeeInfo
    {
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string ProfileImage { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Passward { get; set; }
        public string BankName { get; set; }
        public string SportName { get; set; }
        public string EducationId { get; set; }
        
    }

}