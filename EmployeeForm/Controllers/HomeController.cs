using EmployeeForm.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EmployeeForm.Controllers
{
    public class HomeController : Controller
    {
        AccessDB objAccessDB = new AccessDB();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetAllUserListJSON(jQueryDataTableParamModel param)
        {
            string sortOrder = string.Empty;
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            if (sortColumnIndex == 0)
            {
                sortOrder = "UserId";
            }

            else if (sortColumnIndex == 1)
            {
                sortOrder = "FullName";
            }
            else if (sortColumnIndex == 2)
            {
                sortOrder = "Email";
            }

            string search = "||"; //It's indicate blank filter

            if (!string.IsNullOrEmpty(param.sSearch))
                search = param.sSearch;

            //var sortDirection = Request["sSortDir_0"]; // asc or desc
            var sortDirection = "desc";
            int pageNo = 1;
            int recordPerPage = param.iDisplayLength;

            //Find page number from the logic
            if (param.iDisplayStart > 0)
            {
                pageNo = (param.iDisplayStart / recordPerPage) + 1;
            }

            DataTableProperty DataTableProperty = new DataTableProperty();
            DataTableProperty.PageNo = pageNo;
            DataTableProperty.RecordPerPage = recordPerPage;
            DataTableProperty.Filter = search == "||" ? null : search;
            DataTableProperty.SortField = sortOrder;
            DataTableProperty.SortOrder = sortDirection;
            if (param.sSearch_1 != null)
            {
                DataTableProperty.FullName = param.sSearch_1;
            }
            if (param.sSearch_2 != null)
            {
                DataTableProperty.Email = param.sSearch_2;
            }

            var result1 = objAccessDB.GetUserList(DataTableProperty);
            dynamic result = "";
            if (result1.EmployeeInfoList == null)
            {
                result = new List<EmployeeInfo>();
            }
            else
            {
                result = from c in result1.EmployeeInfoList
                         select new[] { c.UserId.ToString(), c.FullName.ToString(), c.Email.ToString(), c.ProfileImage.ToString() };
            }

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = result1.totalNumberofRecord,
                iTotalDisplayRecords = result1.filteredRecord,
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public ActionResult InsertUpdateData(Employee model)
        {
            bool IsSuccess = objAccessDB.AddEmployee(model);
            return Json(new { IsSuccess }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult UploadImageData()
        {
            string result = "";

            if (Request.Files.Count > 0)
            {
                try
                {
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                        string fname;

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fname = file.FileName;
                        }

                        // Get the complete folder path and store the file inside it.  
                        fname = Path.Combine(Server.MapPath("~/Uploads/"), fname);
                        file.SaveAs(fname);
                        return Json(new { data = file.FileName }, JsonRequestBehavior.AllowGet);

                    }
                    // Returns message that successfully uploaded  
                    return Json(new { data = result }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { data = result }, JsonRequestBehavior.AllowGet);

                }
            }
            else
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Edit(int? id)
        {
            string result = "";
            try
            {
                EmployeeInfo objEmployeeInfo = new EmployeeInfo();
                objEmployeeInfo = objAccessDB.GetUserByUserId(id);
                return Json(new { data = objEmployeeInfo }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);

            }

        }
        [HttpPost]
        public ActionResult Delete(int? id)
        {
            string result = "";
            try
            {
                bool IsDeleted = false;
                IsDeleted = objAccessDB.Delete(id);
                return Json(new { data = IsDeleted }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);

            }

        }
    }
}