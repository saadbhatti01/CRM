using SMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using LinqToExcel;

namespace SMS.Controllers
{
    [CheckSession]
    public class UploadStudentDataController : Controller
    {
        // GET: UploadStudentData
        DBCon con = new DBCon();
        public ActionResult DownloadFile()
        {
            string file = HostingEnvironment.MapPath("~/StudentData_Excel/SampleFile/studentsData.xlsx");
            string contentType = "application/xlsx";
            return File(file, contentType, Path.GetFileName(file));
        }
        public ActionResult UploadDataFile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadDataFile(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    var allowedExtensions = new[] { ".xlsx", ".xls" };

                    var ext = Path.GetExtension(file.FileName);
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        var getFile = con.studentDataFiles.Where(s => s.Status == "Pending").FirstOrDefault();
                        if (getFile != null)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            string[] split = fileName.Split('.');
                            string splitUrl = split[0];
                            string splitUr2 = split[1];
                            var Name = splitUrl + "" + DateTime.Now.ToFileTime() + "." + splitUr2 + "";
                            string PhysicalPath = Path.Combine(Server.MapPath("~/StudentData_Excel/UploadedFile/"), Name);
                            file.SaveAs(PhysicalPath);
                            getFile.DataFile = Name;
                            getFile.Status = "Pending";
                            getFile.UploadedDate = DateTime.Now;
                            con.SaveChanges();
                            TempData["Success"] = "Student Data File uploaded successfully";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Upload a valid Excel file Valid file formate (.xlsx, .xls)";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please Upload a file";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some issue please contact to soft support";
                return View();
            }
            return View();
        }

        public ActionResult DataFile()
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            if (RoleId == 10)
            {
                return View();
            }
            else
            {
                TempData["Error"] = "You are not allowed to enter this page";
                return RedirectToAction("Logout", "Home");
            }

        }
        

        [HttpPost]

        public ActionResult DataFile(HttpPostedFileBase FileUpload)
        {
            try
            {
                List<string> data = new List<string>();
                if (FileUpload != null)
                {
                    if (FileUpload.ContentType == "application/vnd.ms-excel" ||
                        FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        string excelPath = Server.MapPath("~/StudentData_Excel/") + Path.GetFileName(FileUpload.FileName);
                        FileUpload.SaveAs(excelPath);

                        string fileName = FileUpload.FileName;
                        var db = "";
                        if (fileName.EndsWith(".xls"))
                        {
                            db = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;",
                                excelPath);
                        }
                        else if (fileName.EndsWith(".xlsx"))
                        {
                            db = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";",
                                excelPath);
                        }
                        string sheetName = "Student Data";
                        var excelFile = new ExcelQueryFactory(excelPath);
                        var artistAlbums = from a in excelFile.Worksheet<StudentBulkData>(sheetName) select a;
                        var adapter = new OleDbDataAdapter("SELECT * FROM [Student Data$]", db);
                        var ds = new DataSet();
                        adapter.Fill(ds, "ExcelTable");
                        DataTable dtable = ds.Tables["ExcelTable"];
                        foreach (var reg in artistAlbums)
                        {
                            int loginId = 0;
                            int PersonId = 0;
                            int StudentId = 0;
                            int perRegIdId = 0;
                            try
                            {
                                if (reg.Name != "" && reg.FatherName != "" && reg.ContactOne != "" && reg.Name != null && reg.FatherName != null && reg.ContactOne != null)
                                {
                                    var CheckROll = con.std.Where(r => r.stdRollNo == reg.RollNumber).Any();
                                    if (CheckROll == false)
                                    {
                                        Person per = new Person();
                                        Student std = new Student();
                                        //For Person
                                        per.perName = reg.Name;
                                        per.perGarName = reg.FatherName;

                                        if (reg.CNIC == "" || reg.CNIC == null)
                                        {
                                            per.perCNIC = "12345";
                                        }
                                        else
                                        {
                                            per.perCNIC = reg.CNIC;
                                        }
                                        if (reg.Email == "" || reg.Email == null)
                                        {
                                            per.perEmail = "a@a.a";
                                        }
                                        else
                                        {
                                            per.perEmail = reg.Email;
                                        }
                                        if (reg.DOB == "" || reg.DOB == null)
                                        {
                                            per.perDOB = "2020-01-01";
                                        }
                                        else
                                        {
                                            per.perDOB = reg.DOB;
                                        }
                                        if (reg.BloodGroup == "" || reg.BloodGroup == null)
                                        {
                                            per.perBloodGrp = "notEntered";
                                        }
                                        else
                                        {
                                            per.perBloodGrp = reg.BloodGroup;
                                        }

                                        if (reg.ContactOne == "" || reg.ContactOne == null)
                                        {
                                            per.perContactOne = "0000";
                                        }
                                        else
                                        {
                                            per.perContactOne = reg.ContactOne;
                                        }

                                        if (reg.ContactTwo == "" || reg.ContactTwo == null)
                                        {
                                            per.perContactTwo = "0000";
                                        }
                                        else
                                        {
                                            per.perContactTwo = reg.ContactTwo;
                                        }

                                        if (reg.CurrentAddress == "" || reg.CurrentAddress == null)
                                        {
                                            per.perCurrentAdrs = "notEntered";
                                        }
                                        else
                                        {
                                            per.perCurrentAdrs = reg.CurrentAddress;
                                        }
                                        if (reg.PermanentAddress == "" || reg.PermanentAddress == null)
                                        {
                                            per.perPermanantAdrs = "notEntered";
                                        }
                                        else
                                        {
                                            per.perPermanantAdrs = reg.PermanentAddress;
                                        }

                                        if (reg.Cast == "" || reg.Cast == null)
                                        {
                                            per.perCast = "notEntered";
                                        }
                                        else
                                        {
                                            per.perCast = reg.Cast;
                                        }

                                        if (reg.Religion == "" || reg.Religion == null)
                                        {
                                            per.perReligion = "notEntered";
                                        }
                                        else
                                        {
                                            per.perReligion = reg.Religion;
                                        }
                                        per.perImage = "avatar.jpg";


                                        var getCityId = con.city.Where(c => c.CityName == reg.City).FirstOrDefault();
                                        if (getCityId != null)
                                        {
                                            per.CityId = getCityId.CityId;
                                            var getAreaId = con.area.Where(c => c.AreaName == reg.Location).FirstOrDefault();
                                            if (getAreaId != null)
                                            {
                                                per.AreaId = getAreaId.AreaId;
                                                per.CityId = getCityId.CityId;
                                            }

                                        }

                                        Login log = new Login();
                                        log.usrName = reg.Name;
                                        log.usrLogin = reg.RollNumber;
                                        log.usrPassword = "abc123";
                                        log.roleId = 3;
                                        log.usrStatus = "Active";
                                        con.login.Add(log);
                                        con.SaveChanges();
                                        loginId = log.id;
                                        per.roleId = 3;
                                        var getmaxCode = con.person.Where(p => p.roleId == 3).OrderByDescending(p => p.perId).Select(s => s.perCode).FirstOrDefault();
                                        if (getmaxCode != 0)
                                        {
                                            per.perCode = Convert.ToInt32(getmaxCode) + 1;
                                        }
                                        else
                                        {
                                            per.perCode = 100001;
                                        }

                                        per.id = log.id;
                                        per.CreatedBy = LoginInfo.UserID;
                                        per.CreatedDate = DateTime.Now;
                                        per.UpdatedBy = 0;
                                        //per.UpdatedDate = DateTime.Now;
                                        per.IsDeleted = false;
                                        per.DeletedBy = 0;
                                        //per.DeletedDate = DateTime.Now;
                                        con.person.Add(per);
                                        con.SaveChanges();
                                        PersonId = per.perId;



                                        std.perId = per.perId;

                                        var getSesId = con.InstSes.Where(c => c.sesName == reg.Session).FirstOrDefault();
                                        if (getSesId != null)
                                        {
                                            std.sesId = getSesId.sesId;
                                        }

                                        var getSecId = con.InstSec.Where(c => c.secName == reg.Section).FirstOrDefault();
                                        if (getSecId != null)
                                        {
                                            std.secId = getSecId.secId;
                                        }

                                        var getClassId = con.cls.Where(c => c.classname == reg.Class).FirstOrDefault();
                                        if (getClassId != null)
                                        {
                                            std.classId = getClassId.classId;
                                        }

                                        var getCampusId = con.camp.Where(c => c.campusname == reg.Campus).FirstOrDefault();
                                        if (getCampusId != null)
                                        {
                                            std.camId = getCampusId.camId;
                                        }

                                        std.stdRollNo = reg.RollNumber;
                                        std.stdStatus = reg.StudentStatus;
                                        std.CreatedBy = LoginInfo.UserID;
                                        std.CreatedDate = DateTime.Now;
                                        std.UpdatedBy = 0;
                                        //std.UpdatedDate = DateTime.Now;
                                        std.IsDeleted = false;
                                        std.DeletedBy = 0;
                                        //std.DeletedDate = DateTime.Now;
                                        con.std.Add(std);

                                        //for Attendance RollNo as barcode
                                        PersonRegNo pr = new PersonRegNo();
                                        pr.prBarcode = std.stdRollNo;
                                        pr.roleId = per.roleId;
                                        pr.perId = per.perId;
                                        con.perReg.Add(pr);
                                        con.SaveChanges();

                                        StudentId = std.stdId;
                                        perRegIdId = pr.prId;

                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                if (perRegIdId != 0)
                                {
                                    var chk = con.perReg.Where(i => i.prId == perRegIdId).FirstOrDefault();
                                    if (chk != null)
                                    {
                                        con.perReg.Remove(chk);
                                        con.SaveChanges();
                                    }
                                }
                                if (StudentId != 0)
                                {
                                    var chk = con.std.Where(i => i.stdId == StudentId).FirstOrDefault();
                                    if (chk != null)
                                    {
                                        con.std.Remove(chk);
                                        con.SaveChanges();
                                    }
                                }

                                if (loginId != 0)
                                {
                                    var chk = con.login.Where(i => i.id == loginId).FirstOrDefault();
                                    if (chk != null)
                                    {
                                        con.login.Remove(chk);
                                        con.SaveChanges();
                                    }
                                }

                                if (PersonId != 0)
                                {
                                    var chk = con.person.Where(i => i.perId == PersonId).FirstOrDefault();
                                    if (chk != null)
                                    {
                                        con.person.Remove(chk);
                                        con.SaveChanges();
                                    }
                                }

                            }
                        }
                        TempData["Success"] = "Student Bulk Data Uploaded successfully";
                        return RedirectToAction("DataFile");

                    }

                    else
                    {
                        TempData["Error"] = "Only Excel file format is allowed";
                        return RedirectToAction("DataFile");
                    }
                }
                else
                {
                    TempData["Error"] = "Please upload Excel file for Uploading Bulk Student Data";
                    return RedirectToAction("DataFile");
                }
            }
            catch (Exception ex)
            {
                TempData["Info"] = "" + ex + "";
                TempData["Error"] = "There is some error please contact to soft support";
                return RedirectToAction("DataFile");
            }
        }
    }
}