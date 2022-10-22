
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class LogsController : Controller
    {
        DBCon con = new DBCon();
        // GET: Logs
        public ActionResult ReceivedFeeLogsByUser()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateRole();
                    return View();
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult _ReceivedFeeLogs(string ToDate, string FromDate, string UserId)
        {
            try
            {
                if (ToDate != null && FromDate != null)
                {
                    if (UserId != null)
                    {
                        int usrId = Convert.ToInt32(UserId);
                        var UserName = con.login.Where(l => l.id == usrId).FirstOrDefault();
                        DateTime tDate = Convert.ToDateTime(ToDate);
                        DateTime fDate = Convert.ToDateTime(FromDate);
                        List<StdFeeDetail> sList = new List<StdFeeDetail>();
                        var GetDetails = con.stdfee.Where(s => s.paidDate >= fDate && s.paidDate <= tDate && s.CreatedBy == usrId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            foreach (var i in GetDetails)
                            {
                                StdFeeDetail sd = new StdFeeDetail();
                                if (i.UpdatedBy != 0)
                                {
                                    var getUpdatedByName = con.login.Where(l => l.id == i.UpdatedBy).FirstOrDefault();
                                    if (getUpdatedByName != null)
                                    {
                                        sd.UpdatedBy = getUpdatedByName.usrName;
                                        sd.UpdateRoleName = getUpdatedByName.role.name;
                                    }
                                }
                                var getUserName = con.login.Where(l => l.id == i.CreatedBy).FirstOrDefault();
                                if (getUserName != null)
                                {
                                    sd.UpdatedById = i.UpdatedBy;
                                    sd.CreatedById = i.CreatedBy;
                                    sd.CreatedBy = getUserName.usrName;
                                    sd.CreateRoleName = getUserName.role.name;
                                    sd.Stdname = i.std.pr.perName;
                                    sd.RollNo = i.std.stdRollNo;
                                    sd.Ses = i.ses.sesName;
                                    sd.Sec = i.sec.secName;
                                    sd.Class = i.cls.classname;
                                    sd.Fee = i.ft.feeTypeName;
                                    sd.FeeAmt = i.feeAmount;
                                    sd.PandingAmt = i.PandingAmount;
                                    sd.FeePaidStatus = i.feeStatus;
                                    sd.FeeDate = i.paidDate;
                                    sd.PaidDate = i.paidDate;
                                    sd.CreatedDate = i.CreatedDate;
                                    sd.UpdatedDate = i.UpdatedDate;
                                    sList.Add(sd);
                                }

                            }
                            TempData["Fee"] = sList.OrderBy(s => s.CreatedDate).ToList();

                            ViewBag.ReportNo = 1;
                            TempData["User"] = UserName.usrName + "(" + UserName.role.name + ")";
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            return PartialView("_ReceivedFeeLogs");
                            //var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                            //TempData["Session"] = Session;
                            //var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                            //TempData["ClassName"] = Class;
                            //var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                            //TempData["Section"] = Section;
                            //var FeeType = GetDetails.Where(s => s.feeTypeId == fType).Select(a => a.ft.feeTypeName).FirstOrDefault();
                            //TempData["FeeType"] = FeeType;
                            //TempData["Status"] = Status;
                            //TempData["fDate"] = FromDate;
                            //TempData["tDate"] = ToDate;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }

                        return PartialView("_ReceivedFeeLogs");
                    }
                    else
                    {
                        TempData["No"] = "Please Select a User!";
                    }
                }
                else
                {
                    TempData["No"] = "Please Select Date!";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ReceivedFeeLogs");
        }
        public ActionResult ReceivedFeeLogsByDate()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult _ReceivedFeeLogsByDate(string ToDate, string FromDate)
        {
            try
            {
                if (ToDate != null && FromDate != null)
                {
                    DateTime tDate = Convert.ToDateTime(ToDate);
                    DateTime fDate = Convert.ToDateTime(FromDate);
                    List<StdFeeDetail> sList = new List<StdFeeDetail>();
                    var GetDetails = con.stdfee.Where(s => s.paidDate >= fDate && s.paidDate <= tDate).ToList();
                    if (GetDetails.Count != 0)
                    {
                        foreach (var i in GetDetails)
                        {
                            StdFeeDetail sd = new StdFeeDetail();
                            if (i.UpdatedBy != 0)
                            {
                                var getUpdatedByName = con.login.Where(l => l.id == i.UpdatedBy).FirstOrDefault();
                                if (getUpdatedByName != null)
                                {
                                    sd.UpdatedBy = getUpdatedByName.usrName;
                                    sd.UpdateRoleName = getUpdatedByName.role.name;
                                }
                            }
                            var getUserName = con.login.Where(l => l.id == i.CreatedBy).FirstOrDefault();
                            if (getUserName != null)
                            {
                                sd.UpdatedById = i.UpdatedBy;
                                sd.CreatedById = i.CreatedBy;
                                sd.CreatedBy = getUserName.usrName;
                                sd.CreateRoleName = getUserName.role.name;
                                sd.Stdname = i.std.pr.perName;
                                sd.RollNo = i.std.stdRollNo;
                                sd.Ses = i.ses.sesName;
                                sd.Sec = i.sec.secName;
                                sd.Class = i.cls.classname;
                                sd.Fee = i.ft.feeTypeName;
                                sd.FeeAmt = i.feeAmount;
                                sd.PandingAmt = i.PandingAmount;
                                sd.FeePaidStatus = i.feeStatus;
                                sd.FeeDate = i.paidDate;
                                sd.PaidDate = i.paidDate;
                                sd.CreatedDate = i.CreatedDate;
                                sd.UpdatedDate = i.UpdatedDate;
                                sList.Add(sd);
                            }

                        }
                        TempData["Fee"] = sList.OrderBy(s => s.CreatedDate).ToList();
                        ViewBag.ReportNo = 2;
                        TempData["User"] = "Date";
                        TempData["fDate"] = FromDate;
                        TempData["tDate"] = ToDate;
                        return PartialView("_ReceivedFeeLogs");
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }

                    return PartialView("_ReceivedFeeLogs");
                }
                else
                {
                    TempData["No"] = "Please Select Date!";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ReceivedFeeLogs");
        }
        public ActionResult ReceivedFeeLogsByStudent()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {

                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public ActionResult _ReceivedFeeLogsByStd(string ToDate, string FromDate, string RollNo)
        {
            try
            {
                if (ToDate != null && FromDate != null)
                {
                    if (RollNo != null)
                    {
                        var GetStudent = con.std.Where(l => l.stdRollNo == RollNo).FirstOrDefault();
                        if(GetStudent == null)
                        {
                            TempData["No"] = "Invalid Roll Number";
                            return PartialView("_ReceivedFeeLogs");
                        }
                        else
                        {
                            DateTime tDate = Convert.ToDateTime(ToDate);
                            DateTime fDate = Convert.ToDateTime(FromDate);
                            List<StdFeeDetail> sList = new List<StdFeeDetail>();
                            var GetDetails = con.stdfee.Where(s => s.paidDate >= fDate && s.paidDate <= tDate && s.stdId == GetStudent.stdId).ToList();
                            if (GetDetails.Count != 0)
                            {
                                foreach (var i in GetDetails)
                                {
                                    StdFeeDetail sd = new StdFeeDetail();
                                    if (i.UpdatedBy != 0)
                                    {
                                        var getUpdatedByName = con.login.Where(l => l.id == i.UpdatedBy).FirstOrDefault();
                                        if (getUpdatedByName != null)
                                        {
                                            sd.UpdatedBy = getUpdatedByName.usrName;
                                            sd.UpdateRoleName = getUpdatedByName.role.name;
                                        }
                                    }
                                    var getUserName = con.login.Where(l => l.id == i.CreatedBy).FirstOrDefault();
                                    if (getUserName != null)
                                    {
                                        sd.UpdatedById = i.UpdatedBy;
                                        sd.CreatedById = i.CreatedBy;
                                        sd.CreatedBy = getUserName.usrName;
                                        sd.CreateRoleName = getUserName.role.name;
                                        sd.Stdname = i.std.pr.perName;
                                        sd.RollNo = i.std.stdRollNo;
                                        sd.Ses = i.ses.sesName;
                                        sd.Sec = i.sec.secName;
                                        sd.Class = i.cls.classname;
                                        sd.Fee = i.ft.feeTypeName;
                                        sd.FeeAmt = i.feeAmount;
                                        sd.PandingAmt = i.PandingAmount;
                                        sd.FeePaidStatus = i.feeStatus;
                                        sd.FeeDate = i.paidDate;
                                        sd.PaidDate = i.paidDate;
                                        sd.CreatedDate = i.CreatedDate;
                                        sd.UpdatedDate = i.UpdatedDate;
                                        sList.Add(sd);
                                    }

                                }
                                TempData["Fee"] = sList.OrderBy(s => s.CreatedDate).ToList();

                                ViewBag.ReportNo = 3;
                                TempData["User"] = GetStudent.pr.perName + "(" + GetStudent.stdRollNo + ")";
                                TempData["fDate"] = FromDate;
                                TempData["tDate"] = ToDate;
                                return PartialView("_ReceivedFeeLogs");
                            }
                            else
                            {
                                TempData["No"] = "No Record found.";
                            }

                            return PartialView("_ReceivedFeeLogs");
                        }
                       
                        
                    }
                    else
                    {
                        TempData["No"] = "Please Enter a Roll Number!";
                    }
                }
                else
                {
                    TempData["No"] = "Please Select Date!";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ReceivedFeeLogs");
        }

        public ActionResult LoginLogs()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult _LoginLog(string ToDate, string FromDate)
        {
            try
            {
                if (ToDate != null && FromDate != null)
                {
                    DateTime tDate = Convert.ToDateTime(ToDate);
                    DateTime fDate = Convert.ToDateTime(FromDate);
                    DateTime tdate =  tDate.AddHours(23).AddMinutes(59);
                    var getLogs = con.loginlogs.Where(l => l.logDateTime >= fDate && l.logDateTime <= tdate && l.login.roleId != 10).OrderByDescending(s => s.logDateTime).ToList();
                    if(getLogs.Count != 0)
                    {
                        TempData["Logs"] = getLogs;
                        TempData["fDate"] = FromDate;
                        TempData["tDate"] = ToDate;
                        return PartialView("_LoginLog");
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }

                    return PartialView("_LoginLog");
                }
                else
                {
                    TempData["No"] = "Please Select Date!";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_LoginLog");
        }

        public void PopulateRole()
        {
            //Populating the dropdown for Campus
            SelectList sl = new SelectList(con.login.Where(r => r.roleId == 1 || r.roleId == 2).ToList(), "id", "usrName");
            ViewData["Role"] = sl;
        }
    }
}