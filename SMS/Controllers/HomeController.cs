using PRMS.Models;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class HomeController : Controller
    {
        DBCon con = new DBCon();

        public ActionResult LogInUser()
        {
            try
            {
                if (CheckConnectivity())
                {
                    var GetInsInfo = con.InstInfo.Where(i => i.intId == 1).FirstOrDefault();
                    if (GetInsInfo != null)
                    {
                        //For LoginView
                        TempData["LogIn"] = "LogIn";
                        ////
                        Session["InsName"] = GetInsInfo.intName;
                        string imageBase64 = Convert.ToBase64String(GetInsInfo.intLogo);
                        string imageSrc = string.Format("data:image/gif;base64,{0}", imageBase64);
                        Session["InsImage"] = imageSrc;
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please check your internet connection!";
                    return View();
                }

            }
            catch (Exception ex)
            {

            }


            return View();
        }
        [HttpPost]

        public ActionResult LogInUser(Login lgin)
        {
            try
            {
                if (CheckConnectivity())
                {
                    var dbUsr = con.login.Where(u => u.usrLogin == lgin.usrLogin && u.usrPassword == lgin.usrPassword && u.usrStatus == "Active").FirstOrDefault();
                    if (dbUsr != null)
                    {
                        var GetInsInfo = con.InstInfo.Where(i => i.intId == 1).FirstOrDefault();
                        Session["InsName"] = GetInsInfo.intName;
                        string imageBase64 = Convert.ToBase64String(GetInsInfo.intLogo);
                        string imageSrc = string.Format("data:image/gif;base64,{0}", imageBase64);
                        Session["InsImage"] = imageSrc;
                        Session["ContactNo"] = GetInsInfo.intPhone;
                        LoginLogs login = new LoginLogs();
                        login.id = dbUsr.id;
                        login.logDateTime = DateTime.Now;
                        con.loginlogs.Add(login);
                        con.SaveChanges();
                        if (dbUsr.roleId == 10)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;
                            return RedirectToAction("Index");
                        }
                        else if (dbUsr.roleId == 1)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;
                            return RedirectToAction("Index");
                        }
                        else if (dbUsr.roleId == 2)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;
                            return RedirectToAction("Index");
                        }
                        else if (dbUsr.roleId == 3)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;
                            return RedirectToAction("Index");
                        }
                        else if (dbUsr.roleId == 4)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;
                            var GetRelation = con.rel.Where(r => r.id == dbUsr.id).ToList();
                            if (GetRelation.Count != 0)
                            {
                                List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                                foreach (var i in GetRelation)
                                {
                                    var GetStd = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                                    if (GetStd != null)
                                    {
                                        StdFeeDetail sf = new StdFeeDetail();
                                        sf.Stdname = GetStd.pr.perName;
                                        sf.stdId = GetStd.stdId;
                                        StdList.Add(sf);
                                    }
                                }
                                TempData["Kids"] = StdList;
                            }

                            return RedirectToAction("Index");
                        }
                        else if (dbUsr.roleId == 5)
                        {
                            Session["ID"] = dbUsr.id.ToString();
                            LoginInfo.UserID = dbUsr.id;
                            Session["RoleId"] = dbUsr.roleId;
                            Session["RoleName"] = dbUsr.role.name;
                            Session["Name"] = dbUsr.usrName;

                            //Get Teacher Person ID
                            var getPerId = con.person.Where(p => p.id == dbUsr.id).FirstOrDefault();
                            if (getPerId != null)
                            {
                                //Get Teacher Class Detail
                                var getClasses = con.teachers.Where(t => t.perId == getPerId.perId).ToList();
                                if (getClasses.Count != 0)
                                {
                                    TempData["TeacherClasses"] = getClasses;
                                }
                            }



                            return RedirectToAction("Index");
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Invalid user Name or Password";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please check your internet connection!";
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Please check your internet connection!";
                return View();
            }

            return View();
        }


        [CheckSession]
        public ActionResult UploadDoument(string From)
        {
            try
            {
                int Id = Convert.ToInt32(Session["ID"]);
                List<StdFeeDetail> List = new List<StdFeeDetail>();
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatSubJect();
                ViewBag.From = From;
                if (From != null && From == "1")
                {
                    var GetDocs = con.documents.ToList();

                    if (GetDocs.Count != 0)
                    {
                        foreach (var i in GetDocs)
                        {
                            StdFeeDetail s = new StdFeeDetail();
                            s.DocId = i.DocId;
                            s.Title = i.DocTitle;
                            s.Class = i.cls.classname;
                            s.Ses = i.ses.sesName;
                            s.Sec = i.sec.secName;
                            s.Subject = i.subject.subName + "-" + i.subject.subCode;
                            s.Note = i.Note;
                            s.PersonalNote = i.PersonalNote;
                            var getCreatedBy = con.person.Where(p => p.id == i.CreatedBy).FirstOrDefault();
                            s.CreatedBy = getCreatedBy.perName + "(" + getCreatedBy.role.name + ")";
                            s.CreatedDate = i.CreatedDate;
                            s.FilePath = i.FilePath;
                            s.Expiry = i.ExpiryDate;
                            if (i.isVisible == true)
                            {
                                s.Status = "Visible";
                            }
                            else
                            {
                                s.Status = "InVisible";
                            }

                            List.Add(s);
                        }
                        TempData["Docs"] = List;

                    }
                }
                else if (From != null && From == "2")
                {
                    var GetDocs = con.documents.Where(d => d.CreatedBy == Id).ToList();

                    if (GetDocs.Count != 0)
                    {
                        foreach (var i in GetDocs)
                        {
                            StdFeeDetail s = new StdFeeDetail();
                            s.DocId = i.DocId;
                            s.Title = i.DocTitle;
                            s.Class = i.cls.classname;
                            s.Ses = i.ses.sesName;
                            s.Sec = i.sec.secName;
                            s.Subject = i.subject.subName + "-" + i.subject.subCode;
                            s.Note = i.PersonalNote;
                            var getCreatedBy = con.person.Where(p => p.id == i.CreatedBy).FirstOrDefault();
                            s.CreatedBy = getCreatedBy.perName + "(" + getCreatedBy.role.name + ")";
                            s.CreatedDate = i.CreatedDate;
                            s.FilePath = i.FilePath;
                            s.Expiry = i.ExpiryDate;
                            if (i.isVisible == true)
                            {
                                s.Status = "Visible";
                            }
                            else
                            {
                                s.Status = "InVisible";
                            }
                            List.Add(s);
                        }
                        TempData["Docs"] = List;

                    }
                }


                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error pelase contact to soft support";
                return View();
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult UploadDoument(Documents doc, HttpPostedFileBase file, string From)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatSubJect();
                if (ModelState.IsValid)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var allowedExtensions = new[] { ".JPG", ".png", ".PNG", ".jpg", ".jpeg", ".JPEG", ".doc", ".docx", ".pdf" };

                        var ext = Path.GetExtension(file.FileName);
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            string PhysicalPath = Path.Combine(Server.MapPath("~/Documents/"), fileName);
                            file.SaveAs(PhysicalPath);
                            doc.FilePath = fileName;
                            if (doc.Note == "")
                            {
                                doc.Note = "Not Entered";
                            }
                            if (doc.PersonalNote == "")
                            {
                                doc.PersonalNote = "Not Entered";
                            }
                            doc.camId = 0;
                            doc.CreatedBy = LoginInfo.UserID;
                            doc.CreatedDate = DateTime.Now;
                            con.documents.Add(doc);
                            con.SaveChanges();
                            TempData["Success"] = "Document Uploaded successfully";
                            return RedirectToAction("UploadDoument", new { From });
                        }
                        else
                        {
                            TempData["Error"] = "Please Updload a valid file type Like .JPG, .png, .PNG, .jpg, .jpeg, .JPEG, .doc, .docx, .pdf";
                            return View("UploadDoument", new { From });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Updload a valid file";
                        return View("UploadDoument", new { From });
                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error pelase contact to soft support";
                return View("UploadDoument", new { From });
            }
        }

        [CheckSession]
        public ActionResult EditDocument(int id, string From)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatSubJect();
                ViewBag.From = From;
                var getDoc = con.documents.Where(d => d.DocId == id).FirstOrDefault();
                if (getDoc != null)
                {
                    return View(getDoc);
                }
                else
                {
                    TempData["Error"] = "No record found";
                    return RedirectToAction("UploadDoument", new { From });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Document not deleted pelase contact to soft support";
                return RedirectToAction("UploadDoument", new { From });
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult EditDocument(int id, Documents doc, HttpPostedFileBase file, string From)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatSubJect();
                ViewBag.From = From;
                var getDoc = con.documents.Where(d => d.DocId == id).FirstOrDefault();
                if (getDoc != null)
                {
                    if (file != null)
                    {
                        var allowedExtensions = new[] { ".JPG", ".png", ".PNG", ".jpg", ".jpeg", ".JPEG", ".doc", ".docx", ".pdf" };

                        var ext = Path.GetExtension(file.FileName);
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            string PhysicalPath = Path.Combine(Server.MapPath("~/Documents/"), fileName);
                            file.SaveAs(PhysicalPath);
                            getDoc.FilePath = fileName;
                            getDoc.sesId = doc.sesId;
                            getDoc.secId = doc.secId;
                            getDoc.classId = doc.classId;
                            getDoc.subId = doc.subId;
                            getDoc.DocTitle = doc.DocTitle;
                            getDoc.PersonalNote = doc.PersonalNote;
                            getDoc.isVisible = doc.isVisible;
                            getDoc.ExpiryDate = doc.ExpiryDate;
                            getDoc.Note = doc.Note;
                            con.SaveChanges();
                            TempData["Success"] = "Doucment Updated Successfully";
                            return RedirectToAction("UploadDoument", new { From });
                        }
                        else
                        {
                            TempData["Error"] = "Please Updload a valid file type Like .JPG, .png, .PNG, .jpg, .jpeg, .JPEG, .doc, .docx, .pdf";
                            return View(getDoc);
                        }
                    }
                    else
                    {
                        getDoc.sesId = doc.sesId;
                        getDoc.secId = doc.secId;
                        getDoc.classId = doc.classId;
                        getDoc.subId = doc.subId;
                        getDoc.DocTitle = doc.DocTitle;
                        getDoc.PersonalNote = doc.PersonalNote;
                        getDoc.isVisible = doc.isVisible;
                        getDoc.ExpiryDate = doc.ExpiryDate;
                        con.SaveChanges();
                        TempData["Success"] = "Doucment Updated Successfully";
                        return RedirectToAction("UploadDoument", new { From });
                    }

                }
                else
                {
                    TempData["Error"] = "No record found";
                    return RedirectToAction("UploadDoument", new { From });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Document not deleted pelase contact to soft support";
                return RedirectToAction("UploadDoument", new { From });
            }
        }


        [CheckSession]
        public ActionResult DelDocument(int id, string From)
        {
            try
            {
                var getDoc = con.documents.Where(d => d.DocId == id).FirstOrDefault();
                if (getDoc != null)
                {
                    con.documents.Remove(getDoc);
                    con.SaveChanges();
                    TempData["Success"] = "Docment Deleted successfully";
                    return RedirectToAction("UploadDoument", new { From });
                }
                else
                {
                    TempData["Error"] = "No record found";
                    return RedirectToAction("UploadDoument", new { From });
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Document not deleted pelase contact to soft support";
                return RedirectToAction("UploadDoument", new { From });
            }
        }


        // GET: Home
        [CheckSession]
        public ActionResult Index()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 10)
                {
                    var GetStdCount = con.std.Where(s => s.IsDeleted == false && s.stdStatus == "Active").ToList().Count();
                    var GetClassCount = con.cls.ToList().Count();
                    var GetSecCount = con.InstSec.ToList().Count();
                    var GetMonth = DateTime.Now.Month;
                    var GetYear = DateTime.Now.Year;
                    var GetFee = con.stdfee.Where(p => p.paidDate.Month == GetMonth).ToList();
                    if (GetFee.Count != 0)
                    {
                        var GetMonthlyFee = GetFee.Sum(p => p.feeAmount);
                        var GetMonthlyPendingFee = GetFee.Sum(p => p.PandingAmount);
                        ViewBag.Fee = GetMonthlyFee;
                        ViewBag.Pending = GetMonthlyPendingFee;
                        ViewBag.Month = GetMonth;
                        ViewBag.Year = GetYear;
                    }
                    ViewBag.Month = GetMonth;
                    ViewBag.Year = GetYear;
                    var getSes = con.InstSes.Where(s => s.sesStatus == "Open").FirstOrDefault();
                    if (getSes != null)
                    {
                        var GetMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                        if (GetMarks.Count != 0)
                        {
                            var SumOfTotalMarks = GetMarks.Sum(p => p.subTotalMarks);
                            var GetObtMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                            if (GetObtMarks.Count != 0)
                            {
                                var SumOfObtainMarks = GetObtMarks.Sum(p => p.totalObtainMarks);
                                var AvgResult = SumOfObtainMarks * 100 / SumOfTotalMarks;
                                ViewBag.Performance = AvgResult;
                                ViewBag.Session = getSes.sesName;
                            }
                        }
                        ViewBag.Session = getSes.sesName;
                    }
                    ViewBag.Std = GetStdCount;
                    ViewBag.Class = GetClassCount;
                    ViewBag.Section = GetSecCount;
                }

                else if (roleId == 1)
                {
                    var GetStdCount = con.std.Where(s => s.IsDeleted == false).ToList().Count();
                    var GetClassCount = con.cls.ToList().Count();
                    var GetSecCount = con.InstSec.ToList().Count();
                    var GetMonth = DateTime.Now.Month;
                    var GetYear = DateTime.Now.Year;
                    var GetFee = con.stdfee.Where(p => p.paidDate.Month == GetMonth).ToList();
                    if (GetFee.Count != 0)
                    {
                        var GetMonthlyFee = GetFee.Sum(p => p.feeAmount);
                        var GetMonthlyPendingFee = GetFee.Sum(p => p.PandingAmount);
                        ViewBag.Fee = GetMonthlyFee;
                        ViewBag.Pending = GetMonthlyPendingFee;
                        ViewBag.Month = GetMonth;
                        ViewBag.Year = GetYear;
                    }
                    ViewBag.Month = GetMonth;
                    ViewBag.Year = GetYear;
                    var getSes = con.InstSes.Where(s => s.sesStatus == "Open").FirstOrDefault();
                    if (getSes != null)
                    {
                        var GetMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                        if (GetMarks.Count != 0)
                        {
                            var SumOfTotalMarks = GetMarks.Sum(p => p.subTotalMarks);
                            var GetObtMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                            if (GetObtMarks.Count != 0)
                            {
                                var SumOfObtainMarks = GetObtMarks.Sum(p => p.totalObtainMarks);
                                var AvgResult = SumOfObtainMarks * 100 / SumOfTotalMarks;
                                ViewBag.Performance = AvgResult;
                                ViewBag.Session = getSes.sesName;
                            }
                        }
                        ViewBag.Session = getSes.sesName;
                    }
                    ViewBag.Std = GetStdCount;
                    ViewBag.Class = GetClassCount;
                    ViewBag.Section = GetSecCount;

                }

                else if (roleId == 2)
                {
                    var GetStdCount = con.std.ToList().Count();
                    var GetClassCount = con.cls.ToList().Count();
                    var GetSecCount = con.InstSec.ToList().Count();
                    var GetMonth = DateTime.Now.Month;
                    var GetYear = DateTime.Now.Year;
                    var GetFee = con.stdfee.Where(p => p.paidDate.Month == GetMonth).ToList();
                    if (GetFee.Count != 0)
                    {
                        var GetMonthlyFee = GetFee.Sum(p => p.feeAmount);
                        var GetMonthlyPendingFee = GetFee.Sum(p => p.PandingAmount);
                        ViewBag.Fee = GetMonthlyFee;
                        ViewBag.Pending = GetMonthlyPendingFee;
                        ViewBag.Month = GetMonth;
                        ViewBag.Year = GetYear;
                    }
                    ViewBag.Month = GetMonth;
                    ViewBag.Year = GetYear;
                    var getSes = con.InstSes.Where(s => s.sesStatus == "Open").FirstOrDefault();
                    if (getSes != null)
                    {
                        var GetMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                        if (GetMarks.Count != 0)
                        {
                            var SumOfTotalMarks = GetMarks.Sum(p => p.subTotalMarks);
                            var GetObtMarks = con.stdObtmark.Where(s => s.sesId == getSes.sesId).ToList();
                            if (GetObtMarks.Count != 0)
                            {
                                var SumOfObtainMarks = GetObtMarks.Sum(p => p.totalObtainMarks);
                                var AvgResult = SumOfObtainMarks * 100 / SumOfTotalMarks;
                                ViewBag.Performance = AvgResult;
                                ViewBag.Session = getSes.sesName;
                            }
                        }
                        ViewBag.Session = getSes.sesName;
                    }
                    ViewBag.Std = GetStdCount;
                    ViewBag.Class = GetClassCount;
                    ViewBag.Section = GetSecCount;
                }
                else if (roleId == 3)
                {
                    int Id = Convert.ToInt32(Session["ID"]);
                    var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
                    if (getperId != null)
                    {
                        var getStdId = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();


                        if (getStdId != null)
                        {
                            //Fee Package Logic//
                            var GetStdFeePkg = con.stdfpkg.Where(c => c.classId == getStdId.classId && c.sfpstdId == getStdId.stdId && c.secId == getStdId.secId).ToList();
                            if (GetStdFeePkg.Count != 0)
                            {
                                TempData["FeePackage"] = GetStdFeePkg;
                            }
                            else
                            {
                                var GetFeePkg = con.clfpkg.Where(c => c.classId == getStdId.classId && c.secId == getStdId.secId).ToList();
                                TempData["FeePackage"] = GetFeePkg;
                            }
                            ////End Fee Package Logic/////  

                            //Get Paid Fee Logic//
                            var getStdTotalFee = con.stdfee.Where(s => s.stdId == getStdId.stdId).ToList();
                            if (getStdTotalFee.Count != 0)
                            {
                                var TotalFee = getStdTotalFee.Sum(s => s.feeAmount);
                                TempData["Fee"] = TotalFee;
                            }
                            else
                            {
                                TempData["Fee"] = 0;
                            }
                            //// End Paid Fee Logic/////

                            //Get Pending Fee Logic//
                            //var getStdTotalPendingFee = con.stdfee.Where(s => s.stdId == getStdId.stdId && s.feeStatus == "Partial").ToList();
                            if (getStdTotalFee.Count != 0)
                            {
                                var GetPendingFee = getStdTotalFee.Sum(s => s.PandingAmount);
                                TempData["PendingFee"] = GetPendingFee;
                            }
                            else
                            {
                                TempData["PendingFee"] = 0;
                            }
                            //




                            //Upcomming Fee Logic//
                            TempData["UpcomingFee"] = 0;
                            ////End Upcomming Fee////
                        }
                    }
                }
                else if (roleId == 4)
                {
                    int LoginId = LoginInfo.UserID;
                    var GetRelation = con.rel.Where(r => r.id == LoginId).ToList();
                    if (GetRelation.Count != 0)
                    {
                        double TotalPaidFee = 0;
                        double TotalPendingFee = 0;
                        foreach (var i in GetRelation)
                        {
                            var GetStd = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                            if (GetStd != null)
                            {
                                var GetStdPaidFee = con.stdfee.Where(s => s.stdId == GetStd.stdId).ToList();

                                if (GetStdPaidFee.Count != 0)
                                {
                                    var GetTotalPaidFee = GetStdPaidFee.Sum(s => s.feeAmount);
                                    TotalPaidFee = TotalPaidFee + GetTotalPaidFee;

                                    var GetTotalPendingFee = GetStdPaidFee.Sum(s => s.PandingAmount);
                                    TotalPendingFee = TotalPendingFee + GetTotalPendingFee;
                                }
                                TempData["Fee"] = TotalPaidFee;

                                TempData["PendingFee"] = TotalPendingFee;

                                //Upcomming Fee Logic//
                                TempData["UpcomingFee"] = 0;
                                ////End Upcomming Fee////

                            }
                        }

                    }
                }
                else if (roleId == 5)
                {
                    //Get Teacher Person ID
                    int id = Convert.ToInt32(Session["ID"]);
                    var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
                    if (getPerId != null)
                    {
                        //Get Teacher Class Detail
                        List<int> classes = new List<int>();
                        List<int> Sections = new List<int>();
                        List<TeacherClass> StudentList = new List<TeacherClass>();
                        int StudList = 0;
                        var getClasses = con.teachers.Where(t => t.perId == getPerId.perId).ToList();

                        foreach (var s in getClasses)
                        {
                            var chkId = StudentList.Where(st => st.sesId == s.sesId && st.classId == s.classId && st.secId == s.secId).Any();
                            if (chkId == false)
                            {
                                TeacherClass t = new TeacherClass();
                                t.sesId = s.sesId;
                                t.classId = s.classId;
                                t.secId = s.secId;
                                var getStudents = con.std.Where(ss => ss.sesId == s.sesId && ss.secId == s.secId && ss.classId == s.classId && ss.stdStatus == "Active").ToList();
                                StudList = StudList + getStudents.Count();
                                StudentList.Add(t);
                            }


                        }



                        foreach (var i in getClasses)
                        {
                            if (classes.Contains(i.classId))
                            {

                            }
                            else
                            {
                                classes.Add(i.classId);
                            }

                            if (Sections.Contains(i.secId))
                            {

                            }
                            else
                            {
                                Sections.Add(i.secId);
                            }

                        }

                        var TotalClasses = classes.Count();
                        var TotalSections = Sections.Count();
                        var TotalStudents = StudList;
                        TempData["TotalClasses"] = TotalClasses;
                        TempData["TotalSections"] = TotalSections;
                        TempData["TotalStudents"] = TotalStudents;
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [CheckSession]
        public ActionResult InsituteInfo()
        {
            TempData["InsituteInfo"] = con.InstInfo.ToList().OrderByDescending(p => p.intId);
            return View(TempData["InsituteInfo"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult InsituteInfo(InstituteInfo info, HttpPostedFileBase logo)
        {
            FileUpload fu = new FileUpload();

            try
            {
                if (ModelState.IsValid)
                {
                    var path = "";
                    string paths;
                    var allowedExtensions = new[] { ".JPG", ".png", ".PNG", ".jpg", ".jpeg", ".JPEG" };
                    var fileName = Path.GetFileName(logo.FileName);
                    var ext = Path.GetExtension(logo.FileName);
                    if (allowedExtensions.Contains(ext)) //check what type of extension  
                    {
                        path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                        logo.SaveAs(path);
                        paths = Path.Combine(Server.MapPath("~/Images/"));

                        fu.makeLarge(300, 300, paths, fileName);
                        Stream fileStream = System.IO.File.OpenRead(paths + "Large_" + fileName);
                        var mStreamer = new MemoryStream();
                        mStreamer.SetLength(fileStream.Length);
                        fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
                        mStreamer.Seek(0, SeekOrigin.Begin);
                        byte[] fileBytes = mStreamer.GetBuffer();
                        info.intLogo = fileBytes;
                        con.InstInfo.Add(info);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Institue Address Added successfully";
                        return RedirectToAction("InsituteInfo");
                    }
                    else
                    {
                        TempData["Error"] = "Institute Address not added";
                    }



                }
                else
                {
                    TempData["Error"] = "Institute Address not added";
                }
            }
            catch (Exception e)
            {

            }
            return RedirectToAction("InsituteInfo");
        }

        [CheckSession]
        //public ActionResult DelInsituteInfo(int id)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            InstituteInfo s = con.InstInfo.Single(b => b.intId == id);
        //            con.InstInfo.Remove(s);
        //            con.SaveChanges();
        //            TempData["SuccessMessage"] = "Institute info  deleted Successfully";
        //            return RedirectToAction("InsituteInfo");
        //        }
        //        else
        //        {
        //            TempData["Error"] = "Insitute Info not deleted";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        string InnerException = ex.InnerException.ToString();
        //        string value = "DELETE statement conflicted with the REFERENCE constraint";
        //        if (InnerException.Contains(value))
        //        {
        //            TempData["Error"] = "This Insitute cannot deleted because it is linked with other information";
        //        }
        //        else
        //        {
        //            TempData["Error"] = "Insitute not deleted please contact to soft support";
        //        }
        //    }
        //    return RedirectToAction("InsituteInfo");
        //}
        [HttpGet]
        [CheckSession]
        public ActionResult UpdateInsituteInfo(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InstituteInfo ses = con.InstInfo.Single(b => b.intId == id);
                    return View(ses);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult UpdateInsituteInfo(int id, InstituteInfo info, HttpPostedFileBase logo)
        {
            try
            {
                var OldInfo = con.InstInfo.Single(b => b.intId == id);
                FileUpload fu = new FileUpload();
                if (ModelState.IsValid)
                {
                    if (logo == null)
                    {
                        OldInfo.intName = info.intName;
                        OldInfo.intAddress = info.intAddress;
                        OldInfo.intPhone = info.intPhone;
                        OldInfo.intLogo = OldInfo.intLogo;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Insitute Info Updated Successfully";
                        return RedirectToAction("InsituteInfo");
                    }
                    else
                    {
                        var path = "";
                        string paths;
                        var allowedExtensions = new[] { ".JPG", ".png", ".PNG", ".jpg", ".jpeg", ".JPEG" };
                        var fileName = Path.GetFileName(logo.FileName);
                        var ext = Path.GetExtension(logo.FileName);
                        if (allowedExtensions.Contains(ext)) //check what type of extension  
                        {
                            path = Path.Combine(Server.MapPath("~/Images/"), fileName);
                            logo.SaveAs(path);
                            paths = Path.Combine(Server.MapPath("~/Images/"));

                            fu.makeLarge(300, 300, paths, fileName);
                            Stream fileStream = System.IO.File.OpenRead(paths + "Large_" + fileName);
                            var mStreamer = new MemoryStream();
                            mStreamer.SetLength(fileStream.Length);
                            fileStream.Read(mStreamer.GetBuffer(), 0, (int)fileStream.Length);
                            mStreamer.Seek(0, SeekOrigin.Begin);
                            byte[] fileBytes = mStreamer.GetBuffer();
                            info.intLogo = fileBytes;
                            OldInfo.intName = info.intName;
                            OldInfo.intAddress = info.intAddress;
                            OldInfo.intPhone = info.intPhone;
                            OldInfo.intLogo = info.intLogo;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Insitute Info Updated Successfully";
                            return RedirectToAction("InsituteInfo");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Insitute Info not Updated";

            }
            return RedirectToAction("InsituteInfo");
        }
        [CheckSession]
        public ActionResult City()
        {
            TempData["City"] = con.city.ToList().OrderByDescending(p => p.CityId);
            return View(TempData["City"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult City(City cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.city.Where(x => x.CityName == cm.CityName).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This City name is already exist";
                        return RedirectToAction("City");
                    }
                    else
                    {
                        con.city.Add(cm);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "City Added successfully";
                        RedirectToAction("City");

                    }
                }
                else
                {
                    TempData["Error"] = "City not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("City");
        }
        [CheckSession]
        public ActionResult DelCity(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    City s = con.city.Single(b => b.CityId == id);
                    con.city.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "City deleted Successfully";
                    return RedirectToAction("City");
                }
                else
                {
                    TempData["Error"] = "City cannot deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This city cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "City cannot deleted please contact to soft support";
                }
            }
            return RedirectToAction("City");
        }
        [CheckSession]
        public ActionResult UpdateCity(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    City cl = con.city.Single(b => b.CityId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateCity(int id, City cl)
        {
            try
            {
                var Oldcam = con.city.Single(b => b.CityId == id);
                if (Oldcam != null)
                {
                    var chkName = con.city.Where(c => c.CityName == cl.CityName && c.CityId != Oldcam.CityId).Any();
                    if (chkName == false)
                    {
                        Oldcam.CityName = cl.CityName;

                        con.SaveChanges();
                        TempData["SuccessMessage"] = "City Updated Successfully";
                        return RedirectToAction("City");
                    }
                    else
                    {
                        TempData["Error"] = "This Name is already exist";
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "City not Updated";

            }
            return RedirectToAction("City");
        }
        [CheckSession]
        public ActionResult Area()
        {
            PopulatCity();
            TempData["Area"] = con.area.ToList().OrderByDescending(p => p.AreaId);
            return View(TempData["Area"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Area(Area cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.area.Where(x => x.AreaName == cm.AreaName && x.CityId == cm.CityId).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This Area name is already exist";
                        return RedirectToAction("Area");
                    }
                    else
                    {
                        con.area.Add(cm);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Area Added successfully";
                        RedirectToAction("Area");

                    }
                }
                else
                {
                    TempData["Error"] = "Area not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Area");
        }
        [CheckSession]
        public ActionResult DelArea(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Area s = con.area.Single(b => b.AreaId == id);
                    con.area.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Area deleted Successfully";
                    return RedirectToAction("Area");
                }
                else
                {
                    TempData["Error"] = "Area not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Area cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Area cannot deleted please contact to soft support";
                }
            }
            return RedirectToAction("Area");
        }
        [CheckSession]
        public ActionResult UpdateArea(int id)
        {
            PopulatCity();
            try
            {
                if (ModelState.IsValid)
                {
                    Area cl = con.area.Single(b => b.AreaId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateArea(int id, Area cl)
        {
            try
            {
                var Oldcam = con.area.Single(b => b.AreaId == id);
                if (Oldcam != null)
                {
                    var chkName = con.area.Where(c => c.AreaName == cl.AreaName && c.CityId == cl.CityId && c.AreaId != cl.AreaId).Any();
                    if (chkName == false)
                    {
                        Oldcam.AreaName = cl.AreaName;
                        Oldcam.CityId = cl.CityId;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Area Updated Successfully";
                        return RedirectToAction("Area");
                    }
                    else
                    {
                        TempData["Error"] = "This Area Name is already exist on same city";
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Area not Updated";

            }
            return RedirectToAction("Area");
        }
        [CheckSession]

        public ActionResult Sessions()
        {
            TempData["Session"] = con.InstSes.ToList().OrderByDescending(p => p.sesId);
            return View(TempData["Session"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Sessions(InstSession Ses)
        {
            try
            {
                var Chk = con.InstSes.Where(x => x.sesName == Ses.sesName).Any();
                if (Chk != false)
                {
                    TempData["Error"] = "This Session Name is already exist";
                    return RedirectToAction("Sessions");
                }
                else
                {
                    var ChkCode = con.InstSes.Where(x => x.sesCode == Ses.sesCode).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This Session Code is already exist please choose a different code";
                        return RedirectToAction("Sessions");
                    }
                    else
                    {
                        var chk = con.InstSes.Where(s => s.sesStatus == "Open").FirstOrDefault();
                        if (Ses.sesStatus == "Open" && chk != null)
                        {
                            TempData["Error"] = "There is already one Open Session";
                            return RedirectToAction("Sessions");
                        }
                        else
                        {
                            Ses.createDate = DateTime.Now;
                            Ses.createdBy = LoginInfo.UserID;
                            Ses.updatedDate = DateTime.Now;
                            Ses.updatedBy = 0;
                            con.InstSes.Add(Ses);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Session Added successfully";
                            return RedirectToAction("Sessions");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "There is some error please contact to sof support";
                return RedirectToAction("Sessions");
            }
        }
        [CheckSession]
        public ActionResult DelSession(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InstSession s = con.InstSes.Single(b => b.sesId == id);
                    con.InstSes.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Session deleted Successfully";
                    return RedirectToAction("Sessions");
                }
                else
                {
                    TempData["Error"] = "Session not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Session cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Session not deleted please contact to soft support";
                }

            }
            return RedirectToAction("Sessions");
        }
        [HttpGet]
        [CheckSession]
        public ActionResult UpdateSession(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InstSession ses = con.InstSes.Single(b => b.sesId == id);

                    return View(ses);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateSession(int id, InstSession Ses)
        {
            try
            {
                var OldSes = con.InstSes.FirstOrDefault(b => b.sesId == id);
                var chk = con.InstSes.Where(s => s.sesStatus == "Open").FirstOrDefault();
                if (Ses.sesStatus == "Open" && chk != null)
                {
                    //if (OldSes.sesName != Ses.sesName && OldSes.sesId != id)
                    //{
                    var chName = con.InstSes.Where(s => s.sesName == Ses.sesName && s.sesId != id).Any();
                    if (chName == false)
                    {
                        var chkCode = con.InstSes.Where(s => s.sesCode == Ses.sesCode && s.sesId != id).Any();
                        if (chkCode == false)
                        {
                            OldSes.sesName = Ses.sesName;
                            OldSes.sesCode = Ses.sesCode;
                            OldSes.updatedDate = DateTime.Now;
                            OldSes.updatedBy = LoginInfo.UserID;
                            con.SaveChanges();
                            TempData["Info"] = "There is already Open Session But Name & Code Updated";
                        }
                        else
                        {
                            TempData["Error"] = "This Session Code is already exist";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "This Session Name is already exist";
                    }

                    //}
                    //else
                    //{
                    //    TempData["Error"] = "There is already Open Session";
                    //}
                }
                else
                {
                    var chName = con.InstSes.Where(s => s.sesName == Ses.sesName && s.sesId != OldSes.sesId).Any();
                    if (chName == false)
                    {
                        var chkCode = con.InstSes.Where(s => s.sesCode == Ses.sesCode && s.sesId != OldSes.sesId).Any();
                        if (chkCode == false)
                        {
                            OldSes.sesName = Ses.sesName;
                            OldSes.sesCode = Ses.sesCode;
                            if (Ses.sesStartDate.Year != 1)
                            {
                                OldSes.sesStartDate = Ses.sesStartDate;
                            }
                            if (Ses.sesEndDate.Year != 1)
                            {
                                OldSes.sesEndDate = Ses.sesEndDate;
                            }
                            OldSes.sesStatus = Ses.sesStatus;
                            OldSes.updatedDate = DateTime.Now;
                            OldSes.updatedBy = LoginInfo.UserID;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Session Detail Updated Successfully";
                            return RedirectToAction("Sessions");
                        }
                        else
                        {
                            TempData["Error"] = "This Session Code is already exist";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "This Session Name is already exist";
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Session not Updated";

            }
            return RedirectToAction("Sessions");
        }
        [CheckSession]
        public ActionResult Classes()
        {
            TempData["Class"] = con.cls.ToList().OrderByDescending(p => p.classId);
            return View(TempData["Class"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Classes(Class cls)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.cls.Where(x => x.classname == cls.classname).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This Class Name is already exist";
                        return RedirectToAction("Classes");
                    }
                    else
                    {
                        var Check = con.cls.Where(x => x.classCode == cls.classCode).Any();
                        if (Check != false)
                        {
                            TempData["Error"] = "This Class Code is already exist please choose a different class code";
                            return RedirectToAction("Classes");
                        }
                        else
                        {
                            cls.createDate = DateTime.Now;
                            cls.createdBy = LoginInfo.UserID;
                            //cls.updatedDate = DateTime.Now;
                            //cls.updatedBy = 1;
                            con.cls.Add(cls);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Class Added successfully";
                            RedirectToAction("Classes");
                        }


                    }
                }
                else
                {
                    TempData["Error"] = "Session not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Classes");
        }
        [CheckSession]
        public ActionResult DelClass(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Class s = con.cls.Single(b => b.classId == id);
                    con.cls.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Class deleted Successfully";
                    return RedirectToAction("Classes");
                }
                else
                {
                    TempData["Error"] = "Class cannot deleted please contact to soft support";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This class cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Class cannot deleted please contact to soft support";
                }

            }
            return RedirectToAction("Classes");
        }
        [CheckSession]
        public ActionResult UpdateClass(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Class cl = con.cls.Single(b => b.classId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateClass(int id, Class cl)
        {
            try
            {
                var Oldclass = con.cls.Single(b => b.classId == id);
                var chk = con.cls.Where(s => s.classname == cl.classname && s.classId != Oldclass.classId).Any();
                if (chk == true)
                {
                    TempData["Error"] = "This Class Name is already exist";
                    return RedirectToAction("Classes");
                }
                else
                {
                    var chckk = con.cls.Where(s => s.classCode == cl.classCode && s.classId != Oldclass.classId).Any();
                    if (chckk == true)
                    {
                        TempData["Error"] = "This Class Code is already exist";
                        return RedirectToAction("Classes");
                    }
                    else
                    {
                        Oldclass.classname = cl.classname;
                        Oldclass.classCode = cl.classCode;

                        //Oldclass.createDate = cl.createDate;
                        Oldclass.createdBy = LoginInfo.UserID;
                        //Oldclass.updatedDate = DateTime.Now;
                        //Oldclass.updatedBy = 1;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Class Updated Successfully";
                        return RedirectToAction("Classes");
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Class not Updated";

            }
            return RedirectToAction("Classes");
        }

        [CheckSession]
        public ActionResult Sections()
        {
            TempData["Section"] = con.InstSec.ToList().OrderByDescending(p => p.secId);
            return View(TempData["Section"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Sections(InstSection sec)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.InstSec.Where(x => x.secName == sec.secName).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This Section is already exist";
                        return RedirectToAction("Sections");
                    }
                    else
                    {
                        var ChkCode = con.InstSec.Where(x => x.secCode == sec.secCode).Any();
                        if (ChkCode != false)
                        {
                            TempData["Error"] = "This Section Code is already exist";
                            return RedirectToAction("Sections");
                        }
                        else
                        {
                            sec.createDate = DateTime.Now;
                            sec.createdBy = LoginInfo.UserID;
                            //sec.updatedDate = DateTime.Now;
                            //sec.updatedBy = 1;
                            con.InstSec.Add(sec);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Section Added successfully";
                            return RedirectToAction("Sections");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Section not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Sections");
        }
        [CheckSession]
        public ActionResult DelSection(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InstSection s = con.InstSec.Single(b => b.secId == id);
                    con.InstSec.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Section deleted Successfully";
                    return RedirectToAction("Sections");
                }
                else
                {
                    TempData["Error"] = "Section not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Section cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Section not deleted please contact to soft support";
                }
            }
            return RedirectToAction("Sections");
        }
        [CheckSession]
        public ActionResult UpdateSection(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    InstSection cl = con.InstSec.Single(b => b.secId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateSection(int id, InstSection cl)
        {
            try
            {
                var Oldsec = con.InstSec.Single(b => b.secId == id);
                var chk = con.InstSec.Where(s => s.secName == cl.secName && s.secId != Oldsec.secId).Any();
                if (chk != false)
                {
                    TempData["Error"] = "This Section Name is already exist";
                    return RedirectToAction("Sections");
                }
                else
                {
                    var chkCode = con.InstSec.Where(s => s.secCode == cl.secCode && s.secId != Oldsec.secId).Any();
                    if (chkCode != false)
                    {
                        TempData["Error"] = "This Section Code is already exist";
                        return RedirectToAction("Sections");
                    }
                    else
                    {
                        Oldsec.secName = cl.secName;
                        Oldsec.secCode = cl.secCode;
                        Oldsec.updatedDate = DateTime.Now;
                        Oldsec.updatedBy = 1;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Section Updated Successfully";
                        return RedirectToAction("Sections");
                    }


                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Section not Updated";

            }
            return RedirectToAction("Sections");
        }
        [CheckSession]
        public ActionResult Campus()
        {
            TempData["Campus"] = con.camp.ToList().OrderByDescending(p => p.camId);
            return View(TempData["Campus"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Campus(Campus cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.camp.Where(x => x.campusname == cm.campusname).Any();
                    if (Chk != false)
                    {
                        TempData["Error"] = "This Campus name is already exist";
                        return RedirectToAction("Campus");
                    }
                    else
                    {
                        var ChkCode = con.camp.Where(x => x.CampCode == cm.CampCode).Any();
                        if (ChkCode != false)
                        {
                            TempData["Error"] = "This Campus Code is already exist";
                            return RedirectToAction("Campus");
                        }
                        else
                        {
                            con.camp.Add(cm);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Campus Added successfully";
                            RedirectToAction("Campus");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Campus not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Campus");
        }
        [CheckSession]
        public ActionResult DelCampus(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Campus s = con.camp.Single(b => b.camId == id);
                    con.camp.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Campus deleted Successfully";
                    return RedirectToAction("Campus");
                }
                else
                {
                    TempData["Error"] = "Campus not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Campus cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Campus not deleted please contact to soft support";
                }

            }
            return RedirectToAction("Campus");
        }
        [CheckSession]
        public ActionResult UpdateCampus(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Campus cl = con.camp.Single(b => b.camId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateCampus(int id, Campus cl)
        {
            try
            {
                var Oldcam = con.camp.Single(b => b.camId == id);
                if (Oldcam != null)
                {
                    var chkName = con.camp.Where(c => c.campusname == cl.campusname && c.camId != Oldcam.camId).Any();
                    if (chkName == false)
                    {
                        var chkCode = con.camp.Where(c => c.CampCode == cl.CampCode && c.camId != Oldcam.camId).Any();
                        if (chkCode == false)
                        {
                            Oldcam.campusname = cl.campusname;
                            Oldcam.CampCode = cl.CampCode;
                            Oldcam.Address = cl.Address;
                            Oldcam.PhoneNo = cl.PhoneNo;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Campus Updated Successfully";
                            return RedirectToAction("Campus");
                        }
                        else
                        {
                            TempData["Error"] = "This Code is already exist";
                        }

                    }
                    else
                    {
                        TempData["Error"] = "This Name is already exist";
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Campus not Updated";

            }
            return RedirectToAction("Campus");
        }

        [CheckSession]
        public ActionResult Term()
        {
            TempData["Term"] = con.term.ToList().OrderByDescending(p => p.termId);
            return View(TempData["Term"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Term(Term t)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.term.Where(x => x.termName == t.termName).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This Term name is already exist";
                        return RedirectToAction("Term");
                    }
                    else
                    {
                        con.term.Add(t);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Term Added successfully";
                        RedirectToAction("Term");

                    }
                }
                else
                {
                    TempData["Error"] = "Term not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Term");
        }

        [CheckSession]
        public ActionResult DelTerm(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Term s = con.term.Single(b => b.termId == id);
                    con.term.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Term deleted Successfully";
                    return RedirectToAction("Term");
                }
                else
                {
                    TempData["Error"] = "Term not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Term cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Term not deleted please contact to soft support";
                }

            }
            return RedirectToAction("Campus");
        }
        [CheckSession]
        public ActionResult UpdateTerm(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Term cl = con.term.Single(b => b.termId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateTerm(int id, Term cl)
        {
            try
            {
                var Oldterm = con.term.FirstOrDefault(b => b.termId == id);
                if (Oldterm != null)
                {
                    var chkName = con.term.Where(t => t.termName == cl.termName && t.termId != Oldterm.termId).Any();
                    if (chkName == false)
                    {
                        Oldterm.termName = cl.termName;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Term Updated Successfully";
                        return RedirectToAction("Term");
                    }
                    else
                    {
                        TempData["Error"] = "This term Name is already exist!";
                        return RedirectToAction("Term");
                    }

                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("Term");
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Term not Updated Please contact to soft support";

            }
            return RedirectToAction("Term");
        }
        [CheckSession]
        public ActionResult Subject()
        {
            TempData["Subject"] = con.sub.Where(s => s.IsDeleted == false).ToList().OrderByDescending(p => p.subId);
            return View(TempData["Subject"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Subject(Subject s, string subCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.sub.Where(x => x.subName == s.subName && x.IsDeleted == false).Any();
                    if (Chk == true)
                    {
                        TempData["Error"] = "This Subject name is already exist";
                        return RedirectToAction("Subject");
                    }
                    else
                    {
                        if (isAlphaNumeric(subCode))
                        {
                            var chkCode = con.sub.Where(sub => sub.subCode == subCode).Any();
                            if (chkCode == true)
                            {
                                TempData["Error"] = "Subject Code already exist please try again";
                                RedirectToAction("Subject");
                            }
                            else
                            {
                                s.CreatedBy = LoginInfo.UserID;
                                s.CreatedDate = DateTime.Now;
                                s.UpdatedBy = 0;
                                con.sub.Add(s);
                                con.SaveChanges();
                                TempData["SuccessMessage"] = "Subject Added successfully";
                                RedirectToAction("Subject");
                            }
                            //if (IsSubjectCodeExist(subCode))
                            //{
                            //    s.CreatedBy = LoginInfo.UserID;
                            //    s.CreatedDate = DateTime.Now;
                            //    s.UpdatedBy = 0;
                            //    con.sub.Add(s);
                            //    con.SaveChanges();
                            //    TempData["SuccessMessage"] = "Subject Added successfully";
                            //    RedirectToAction("Subject");
                            //}
                            //else
                            //{
                            //    TempData["Error"] = "Subject Code already exist please try again";
                            //    RedirectToAction("Subject");
                            //}
                        }
                        else
                        {
                            TempData["Error"] = "Subject Code must be Alphanumaric please try again";
                            RedirectToAction("Subject");
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Subject not added";
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = "There is some issue Please contact to soft support. " + e + "";
            }

            return RedirectToAction("Subject");
        }
        [CheckSession]
        public ActionResult DelSubject(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Subject s = con.sub.FirstOrDefault(b => b.subId == id);
                    if (s != null)
                    {
                        con.sub.Remove(s);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Subject deleted Successfully";
                        return RedirectToAction("Subject");

                    }
                    else
                    {
                        TempData["Error"] = "No Record found.";
                        return RedirectToAction("Subject");
                    }

                }
                else
                {
                    TempData["Error"] = "Subject not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Subject cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Subject not deleted please contact to soft support";
                }
            }
            return RedirectToAction("Subject");
        }
        [CheckSession]
        public ActionResult UpdateSubject(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Subject cl = con.sub.Single(b => b.subId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateSubject(int id, Subject cl)
        {
            try
            {
                var Oldsub = con.sub.FirstOrDefault(b => b.subId == id && b.IsDeleted == false);
                if (Oldsub != null)
                {
                    var chkName = con.sub.Where(c => c.subName == cl.subName && c.subId != Oldsub.subId && c.IsDeleted == false).Any();
                    if (chkName == false)
                    {
                        var chkCode = con.sub.Where(c => c.subCode == cl.subCode && c.subId != Oldsub.subId && c.IsDeleted == false).Any();
                        if (chkCode == false)
                        {
                            Oldsub.subName = cl.subName;
                            Oldsub.subCode = cl.subCode;
                            Oldsub.isVisible = cl.isVisible;
                            Oldsub.UpdatedDate = DateTime.Now;
                            Oldsub.UpdatedBy = LoginInfo.UserID;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Subject Updated Successfully";
                            return RedirectToAction("Subject");

                        }
                        else
                        {
                            TempData["Error"] = "This Subject Code is already exist";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "This Subject Name is already exist";
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Subject not Updated Please contact to soft support";

            }
            return RedirectToAction("Subject");
        }


        [CheckSession]
        public ActionResult FeeType()
        {
            TempData["FeeType"] = con.feetype.Where(f => f.isDeleted == false).ToList().OrderByDescending(p => p.feeTypeId);
            return View(TempData["FeeType"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult FeeType(FeeType fee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.feetype.Where(x => x.feeTypeName == fee.feeTypeName && x.isDeleted == false).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This FeeType is already exist";
                        return RedirectToAction("FeeType");
                    }
                    else
                    {

                        fee.createdDate = DateTime.Now;
                        fee.createdBy = LoginInfo.UserID;
                        con.feetype.Add(fee);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "FeeType Added successfully";
                        RedirectToAction("FeeType");

                    }
                }
                else
                {
                    TempData["Error"] = "FeeType not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("FeeType");
        }
        [CheckSession]
        public ActionResult DelFeeType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var chkFeeType = con.clfpkg.Where(f => f.feeTypeId == id).Any();
                    if(chkFeeType == false)
                    {
                        FeeType s = con.feetype.Single(b => b.feeTypeId == id);
                        s.isDeleted = true;
                        s.deletedBy = 1;
                        s.deletedDate = DateTime.Now;
                        con.feetype.Remove(s);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "FeeType deleted Successfully";
                        return RedirectToAction("FeeType");
                    }
                    else
                    {
                        TempData["Error"] = "This FeeType cannot deleted because it is linked with other information";
                    }
                    
                }
                else
                {
                    TempData["Error"] = "FeeType not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This FeeType cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "FeeType not deleted please contact to soft support";
                }
            }
            return RedirectToAction("FeeType");
        }
        [CheckSession]
        public ActionResult UpdateFeeType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    FeeType cl = con.feetype.Single(b => b.feeTypeId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateFeeType(int id, FeeType cl)
        {
            try
            {
                var Oldfee = con.feetype.Single(b => b.feeTypeId == id);
                var chk = con.feetype.Where(s => s.feeTypeName == cl.feeTypeName && s.feeTypeId != Oldfee.feeTypeId && s.isDeleted == false).Any();
                if (chk == true)
                {
                    TempData["Error"] = "This FeeType Name is already exist";
                    return RedirectToAction("FeeType");
                }
                else
                {
                    Oldfee.feeTypeName = cl.feeTypeName;
                    Oldfee.feeTypeStatus = cl.feeTypeStatus;
                    Oldfee.createdDate = cl.createdDate;
                    Oldfee.updatedBy = LoginInfo.UserID;
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "FeeType Updated Successfully";
                    return RedirectToAction("FeeType");

                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "FeeType not Updated";

            }
            return RedirectToAction("FeeType");
        }

        [CheckSession]
        public ActionResult Role()
        {
            TempData["Role"] = con.role.Where(r => r.roleId != 10).ToList();
            return View(TempData["Role"]);
        }
        [HttpPost]
        public ActionResult Role(Role cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.role.Where(x => x.name == cm.name).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This Role name is already exist";
                        return RedirectToAction("Role");
                    }
                    else
                    {
                        con.role.Add(cm);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Role Added successfully";
                        RedirectToAction("Role");

                    }
                }
                else
                {
                    TempData["Error"] = "Role not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Role");
        }
        [CheckSession]
        //public ActionResult DelRole(int id)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            var chk = con.login.Where(b => b.roleId == id).Select(b => b.roleId).FirstOrDefault();
        //            if (chk > 0)
        //            {
        //                TempData["Error"] = "This Role Cannot be Deleted because its associate with a User informaton.";
        //            }
        //            else
        //            {
        //                Role s = con.role.Single(b => b.roleId == id);
        //                con.role.Remove(s);
        //                con.SaveChanges();
        //                TempData["SuccessMessage"] = "Role deleted Successfully";
        //                return RedirectToAction("Role");
        //            }

        //        }
        //        else
        //        {
        //            TempData["Error"] = "Role not deleted";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.Message);
        //    }
        //    return RedirectToAction("Role");
        //}
        [CheckSession]
        public ActionResult UpdateRole(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Role cl = con.role.Single(b => b.roleId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateRole(int id, Role cl)
        {
            try
            {
                var Oldcam = con.role.Single(b => b.roleId == id);
                Oldcam.name = cl.name;
                con.SaveChanges();
                TempData["SuccessMessage"] = "Role Updated Successfully";
                return RedirectToAction("Role");


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "Role not Updated";

            }
            return RedirectToAction("Role");
        }


        [CheckSession]
        public ActionResult ChangePassword()
        {
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult ChangePassword(string cPassword, string nPassword, string rPassword)
        {
            try
            {
                int ID = Convert.ToInt32(Session["ID"]);
                var getUsr = con.login.Where(u => u.id == ID).FirstOrDefault();
                if (getUsr.usrPassword == cPassword)
                {
                    if (nPassword == rPassword && nPassword != "" && rPassword != "")
                    {
                        getUsr.usrPassword = nPassword;
                        con.Entry(getUsr).State = System.Data.Entity.EntityState.Modified;
                        con.SaveChanges();
                        TempData["Success"] = "Password has been changed successfully. Please relogin with new password.";
                        Session.Clear();
                        return RedirectToAction("LogInUser", "Home");
                    }
                    else
                    {
                        TempData["Error"] = "new password must be same.";
                    }
                }
                else
                {
                    TempData["Error"] = "Current password is not valid.";
                }
            }
            catch (Exception)
            {
                throw;
            }

            return View();
        }




        [CheckSession]
        public ActionResult Logs()
        {
            return View();
        }

        public ActionResult AddNewItems()
        {
            return View();
        }

        public ActionResult UpdateItems()
        {
            return View();
        }

        public ActionResult UpdateStock()
        {
            return View();
        }
        public ActionResult Payable()
        {
            return View();
        }
        public ActionResult Receiveable()
        {
            return View();
        }

        public ActionResult StudentRegistration()
        {
            return View();
        }
        public ActionResult UpdateStudent()
        {
            return View();
        }
        public ActionResult StudentFee()
        {
            return View();
        }
        public ActionResult UpdateStudentFee()
        {
            return View();
        }
        public ActionResult Reports()
        {
            return View();
        }

        public ActionResult Attendance()
        {
            return View();
        }

        public ActionResult PaidFee()
        {
            return View();
        }

        public ActionResult UnpaidFee()
        {
            return View();
        }

        public ActionResult ActiveStudents()
        {
            return View();
        }

        public ActionResult SuspendStudents()
        {
            return View();
        }

        public ActionResult LeftStudents()
        {
            return View();
        }

        public ActionResult Settings()
        {
            return View();
        }

        public ActionResult Applicationlogs()
        {
            return View();
        }

        public ActionResult ApplicationSettings()
        {
            return View();
        }

        [CheckSession]
        public ActionResult Users()
        {
            PopulatRole();
            TempData["User"] = con.login.Where(l => l.roleId != 10).ToList().OrderByDescending(p => p.id);
            return View(TempData["User"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult Users(Login cm)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.login.Where(x => x.usrLogin == cm.usrLogin).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This usrname is already exist";
                        return RedirectToAction("Users");
                    }
                    else
                    {

                        con.login.Add(cm);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "User Added successfully";
                        RedirectToAction("Users");

                    }
                }
                else
                {
                    TempData["Error"] = "User not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("Users");
        }
        [CheckSession]
        public ActionResult DelUser(int id)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                //    Login s = con.login.Single(b => b.id == id);
                //    con.login.Remove(s);
                //    con.SaveChanges();
                //    TempData["SuccessMessage"] = "Users deleted Successfully";
                //    return RedirectToAction("Users");


                //}
                //else
                //{
                //    TempData["Error"] = "User not deleted";
                //}
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return RedirectToAction("Users");
        }
        [CheckSession]
        public ActionResult UpdateUser(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PopulatRole();
                    Login cl = con.login.Single(b => b.id == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateUser(int id, Login cl)
        {
            try
            {
                var Oldcam = con.login.Single(b => b.id == id);
                if (Oldcam != null)
                {
                    var chkName = con.login.Where(c => c.usrLogin == cl.usrLogin && c.id != Oldcam.id).Any();
                    if (chkName == false)
                    {

                        Oldcam.usrName = cl.usrName;
                        Oldcam.usrLogin = cl.usrLogin;
                        Oldcam.usrPassword = cl.usrPassword;
                        Oldcam.usrStatus = cl.usrStatus;

                        var getPeron = con.person.Where(p => p.id == id).FirstOrDefault();

                        var chkParent = con.rel.Where(r => r.id == id).Any();
                        if (chkParent == true)
                        {
                            if (Oldcam.roleId != cl.roleId)
                            {
                                TempData["Info"] = "You cannot update his Role because this person is registered as a parent in Make Relation";
                            }
                        }
                        else
                        {
                            if (Oldcam.roleId == 3)
                            {
                                if (Oldcam.roleId != cl.roleId)
                                {
                                    TempData["Info"] = "Student Role cannot be updated";
                                }
                            }
                            else
                            {
                                if (Oldcam.roleId != cl.roleId)
                                {
                                    Oldcam.roleId = cl.roleId;
                                    if (getPeron != null)
                                    {
                                        var getperReg = con.perReg.Where(p => p.perId == getPeron.perId).FirstOrDefault();
                                        if (getperReg != null)
                                        {
                                            getperReg.roleId = cl.roleId;
                                        }
                                        getPeron.roleId = cl.roleId;
                                    }
                                }
                            }
                        }

                        if (getPeron != null)
                        {
                            getPeron.perName = cl.usrName;
                        }
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "User Updated Successfully";
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        TempData["Error"] = "This UserName is already exist";
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                }

            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "User not Updated";

            }
            return RedirectToAction("Users");
        }




        /// <summary>
        /// Passing Marks
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        /// 
        [CheckSession]
        public ActionResult AddPassingMarks()
        {
            try
            {
                var GetAllMarks = con.passing.ToList();
                TempData["Marks"] = GetAllMarks;
                PopulatClass();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "there is some issue";
                return View();
            }

        }
        [CheckSession]
        [HttpPost]
        public ActionResult AddPassingMarks(PassingMarks ps)
        {
            try
            {
                PopulatClass();
                if (ps.Marks > 100)
                {
                    TempData["Error"] = "Percentage(%) must be less than or equal to 100";
                    return RedirectToAction("AddPassingMarks");
                }
                else
                {
                    if (ps.classId > 0)
                    {
                        var chkMarks = con.passing.Where(c => c.classId == ps.classId).FirstOrDefault();
                        if (chkMarks == null)
                        {
                            con.passing.Add(ps);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Passing Marks Entered successfully";
                            return RedirectToAction("AddPassingMarks");
                        }
                        else
                        {
                            TempData["Error"] = "This class Passing Marks is already exist.";
                            return RedirectToAction("AddPassingMarks");
                        }
                    }
                    else
                    {
                        var ClassId = (from cl in con.cls
                                       where !con.passing
                                                .Any(o => o.classId == cl.classId)
                                       select new
                                       {
                                           cl.classId
                                       }).ToList();
                        foreach (var i in ClassId)
                        {
                            PassingMarks pas = new PassingMarks();
                            pas.classId = i.classId;
                            pas.Marks = ps.Marks;
                            con.passing.Add(pas);
                            con.SaveChanges();
                        }
                        TempData["SuccessMessage"] = "All classes Passing Marks Entered successfully";
                        return RedirectToAction("AddPassingMarks");
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Passing Marks not Entered.";
                return RedirectToAction("AddPassingMarks");
            }
        }

        [CheckSession]
        public ActionResult EditPassingMarks(int id)
        {
            try
            {
                PopulatClass();
                var getData = con.passing.Where(p => p.PassingId == id).FirstOrDefault();
                return View(getData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Passing Marks not Entered.";
                return View();
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult EditPassingMarks(PassingMarks ps, int id)
        {
            try
            {
                PopulatClass();
                var getData = con.passing.Where(p => p.PassingId == id).FirstOrDefault();
                if (getData != null)
                {
                    if (ps.Marks > 100)
                    {
                        TempData["Error"] = "Percentage(%) must be less than or equal to 100";
                        return View();
                    }
                    else
                    {
                        getData.Marks = ps.Marks;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Passing Marks updated successfully";
                        return RedirectToAction("AddPassingMarks");
                    }

                }
                return View(getData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Passing Marks not Entered.";
                return View();
            }
        }

        public bool IsSubjectCodeExist(string barcode)
        {
            if (barcode != null)
            {
                var Pre = con.sub.Where(a => a.subCode == barcode.ToString() && a.IsDeleted == false).Any();
                if (Pre == false)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            return false;
        }

        public ActionResult isSubCodeExist(string val)
        {
            if (val != null)
            {
                var Pre = con.sub.Where(a => a.subCode == val.ToString()).FirstOrDefault();
                if (Pre == null)
                {
                    return Json(new { Success = "true", Data = new { Pre } });

                }
                else
                {
                    return Json(new { Success = "false" });
                }

            }
            return Json(new { Success = "false" });
        }

        public static Boolean isAlphaNumeric(string val)
        {
            Regex rg = new Regex(@"^[A-Z0-9\s,]*$");
            return rg.IsMatch(val);
        }

        [HttpPost]
        public ActionResult CheckisAlphaNumeric(string val)
        {
            if (val != null)
            {
                // var Pre = con.vocher.OrderByDescending(a => a.VchId).Where(t => t.PartyId == val).Select(v => v.VchRemainingBlnc).Take(1);
                val = isAlphaNumeric(val).ToString();
                if (val == "True")
                {
                    return Json(new { Success = "true", Data = new { val } });
                }
                else
                {
                    return Json(new { Success = "False" });
                }


            }
            return Json(new { Success = "false" });
        }

        public ActionResult Download(string submitButton)
        {
            if (submitButton == "Web")
            {
                string file = HostingEnvironment.MapPath("~/UserManual/SMS_WEB_UM_V1.pdf");
                string contentType = "application/pdf";
                return File(file, contentType, Path.GetFileName(file));
            }
            else if (submitButton == "Desktop")
            {
                string file = HostingEnvironment.MapPath("~/UserManual/SMS_DES_UM_V1.pdf");
                string contentType = "application/pdf";
                return File(file, contentType, Path.GetFileName(file));
            }
            else
            {
                string file = HostingEnvironment.MapPath("~/UserManual/sms_desktop.rar");
                string contentType = "application/rar";
                return File(file, contentType, Path.GetFileName(file));
            }

        }
        public ActionResult DownloadDocument(int id)
        {
            var getFile = con.documents.Where(d => d.DocId == id).FirstOrDefault();
            string file = HostingEnvironment.MapPath("~/Documents/" + getFile.FilePath + "");
            //string contentType = "application/rar";
            return File(file, Path.GetFileName(file));

        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("LogInUser");
        }

        public void PopulatRole()
        {
            //Populating the dropdown for Role
            SelectList sl = new SelectList(con.role.Where(r => r.roleId != 10).ToList(), "roleId", "name");
            ViewData["name"] = sl;
        }

        public void PopulatCity()
        {
            //Populating the dropdown for City
            SelectList sl = new SelectList(con.city.ToList(), "CityId", "CityName");
            ViewData["City"] = sl;
        }
        public void PopulatClass()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.cls.ToList(), "classId", "classname");
            ViewData["Class"] = sl;
        }

        public void PopulatSec()
        {
            //Populating the dropdown for Section
            SelectList sl = new SelectList(con.InstSec.ToList(), "secId", "secName");
            ViewData["Section"] = sl;
        }
        public void PopulatSes()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.InstSes.Where(s => s.sesStatus == "Open").ToList(), "sesId", "sesName");
            ViewData["Session"] = sl;
        }
        public void PopulatAllSes()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.InstSes.ToList(), "sesId", "sesName");
            ViewData["Session"] = sl;
        }

        public void PopulatSubJect()
        {
            //Populating the dropdown for Session
            List<Subject> SubjectList = new List<Subject>();
            var GetList = con.sub.Where(s => s.isVisible == true && s.IsDeleted == false).ToList();
            if (GetList.Count != 0)
            {
                foreach (var i in GetList)
                {
                    Subject sub = new Subject();
                    var chkSub = SubjectList.Where(s => s.subId == i.subId).FirstOrDefault();
                    if (chkSub == null)
                    {
                        sub.subId = i.subId;
                        sub.subName = i.subName + "-" + i.subCode;
                        SubjectList.Add(sub);
                    }
                }

            }
            SelectList sl = new SelectList(SubjectList, "subId", "subName");
            ViewData["SubJect"] = sl;
        }
        public void PopulatExamType()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.examtype.Where(s => s.etStatus == true && s.IsDeleted == false).ToList(), "etId", "etname");
            ViewData["ExamType"] = sl;
        }

        private bool CheckConnectivity()
        {
            try
            {
                con.Database.Connection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


    }
}