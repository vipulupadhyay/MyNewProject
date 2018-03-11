using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeForm.Models
{
    public class Employee
    {
        public Bank Bank { get; set; }
        public Education Education { get; set; }
        public Sport Sport { get; set; }
        public Person Person { get; set; }
    }

    public class Person
    {
        public int? UserId { get; set; }
        public string FullName { get; set; }
        public string Gender { get; set; }
        public string ProfileImage { get; set; }
        public string Passward { get; set; }
        public string Email { get; set; }
    }

    public class Bank
    {
        public string BankName { get; set; }
    }
    public class Degree
    {
        public string DegreeName { get; set; }
    }
    public class Sport
    {
        public string SportName { get; set; }
    }
    public class Education
    {
        public int DegreeId { get; set; }
        
    }
}