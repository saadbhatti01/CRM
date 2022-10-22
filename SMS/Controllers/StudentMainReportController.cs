using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class StudentMainReportController : Controller
    {
        DBCon con = new DBCon();
        // GET: StudentMainReport
        public ActionResult MainReport()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MainReport(string RollNo)
        {
            try
            {
                if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            TempData["RollNo"] = RollNo;
                            return View();
                        }
                        else
                        {
                            TempData["Info"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number to proceed";
                    return View();
                }
            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }

        }

        public ActionResult AllSessionReport()
        {
            PopulatAllSes();
            return View();
        }

        [HttpPost]
        public ActionResult AllSessionReport(string RollNo, int? SesId)
        {
            try
            {
                PopulatAllSes();
                if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            TempData["RollNo"] = RollNo;
                            TempData["SesId"] = SesId;
                            return View();
                        }
                        else
                        {
                            TempData["Info"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number to proceed";
                    return View();
                }

            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }
        }

        public ActionResult _StdInfo(string RollNo)
        {
            try
            {
                if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            TempData["RollNo"] = RollNo;
                            var getInfo = con.stdEduInfos.Where(s => s.stdId == getStd.stdId).FirstOrDefault();
                            if (getInfo != null)
                            {
                                TempData["PreInfo"] = getInfo;
                            }
                            return PartialView("_StdInfo", getStd);
                        }
                        else
                        {
                            TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return PartialView();
                        }

                    }
                    else
                    {
                        TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return PartialView();
                    }
                }
                else
                {
                    TempData["No"] = "Please Enter Roll Number to proceed";
                    return PartialView();
                }


            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error please contact to soft support";
                return PartialView();
            }
        }

        public ActionResult _StdAccounts(string RollNo, int? SesId)
        {
            try
            {
                if (SesId != null)
                {
                    if (RollNo != "" && RollNo != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.stdId == getStd.stdId).ToList();
                                if (GetDetails.Count != 0)
                                {
                                    foreach (var i in GetDetails)
                                    {
                                        StdFeeDetail sd = new StdFeeDetail();
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
                                        sList.Add(sd);
                                    }
                                    TempData["Fee"] = sList;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                else
                                {
                                    TempData["No"] = "No Record found";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }

                                return PartialView("_StdAccounts");
                            }
                            else
                            {
                                TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                                return PartialView("_StdAccounts");
                            }

                        }
                        else
                        {
                            TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                            return PartialView("_StdAccounts");
                        }
                    }
                    else
                    {
                        TempData["No"] = "Please Enter Roll Number to proceed";
                        return PartialView("_StdAccounts");
                    }
                }
                else if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            List<StdFeeDetail> sList = new List<StdFeeDetail>();
                            var GetDetails = con.stdfee.Where(s => s.stdId == getStd.stdId).ToList();
                            if (GetDetails.Count != 0)
                            {
                                foreach (var i in GetDetails)
                                {
                                    StdFeeDetail sd = new StdFeeDetail();
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
                                    sList.Add(sd);
                                }
                                TempData["Fee"] = sList;
                            }
                            else
                            {
                                TempData["No"] = "No Record found";
                            }

                            return PartialView("_StdAccounts");
                        }
                        else
                        {
                            TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return PartialView("_StdAccounts");
                        }

                    }
                    else
                    {
                        TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return PartialView("_StdAccounts");
                    }
                }
                else
                {
                    TempData["No"] = "Please Enter Roll Number to proceed";
                    return PartialView("_StdAccounts");
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error please contact to soft support";
                return PartialView("_StdAccounts");
            }
        }
        public ActionResult _StdExamination(string RollNo, int? SesId)
        {
            try
            {
                if (SesId != null)
                {
                    if (RollNo != "" && RollNo != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.stdId == getStd.stdId).ToList();

                                if (getStdDetail.Count != 0)
                                {
                                    TempData["StdMarks"] = getStdDetail;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }

                                }
                                else
                                {
                                    TempData["No"] = "No Record found.";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                return PartialView("_StdExamination");
                            }
                            else
                            {
                                TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                                return PartialView("_StdExamination");
                            }

                        }
                        else
                        {
                            TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                            return PartialView("_StdExamination");
                        }
                    }
                    else
                    {
                        TempData["No"] = "Please Enter Roll Number to proceed";
                        return PartialView("_StdExamination");
                    }
                }
                else if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            var getStdDetail = con.stdObtmark.Where(s => s.stdId == getStd.stdId).ToList();

                            if (getStdDetail.Count != 0)
                            {
                                TempData["StdMarks"] = getStdDetail;
                            }
                            else
                            {
                                TempData["No"] = "No Record found.";
                            }
                            return PartialView("_StdExamination");
                        }
                        else
                        {
                            TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return PartialView("_StdExamination");
                        }

                    }
                    else
                    {
                        TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return PartialView("_StdExamination");
                    }
                }
                else
                {
                    TempData["No"] = "Please Enter Roll Number to proceed";
                    return PartialView("_StdExamination");
                }


            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error please contact to soft support";
                return PartialView("_StdExamination");
            }
        }

        public ActionResult _StdAttendance(string RollNo, int? SesId)
        {
            try
            {
                if (SesId != null)
                {
                    if (RollNo != "" && RollNo != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                var getAttendance = con.att.Where(a => a.perId == getStd.perId && a.sesId == SesId).ToList();
                                if (getAttendance.Count != 0)
                                {
                                    TempData["Attendance"] = getAttendance;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                else
                                {
                                    TempData["No"] = "No Record found";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                    return PartialView("_StdAttendance");
                                }
                                return PartialView("_StdAttendance");
                            }
                            else
                            {
                                TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                                return PartialView("_StdAttendance");
                            }

                        }
                        else
                        {
                            TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                            return PartialView("_StdAttendance");
                        }
                    }
                    else
                    {
                        TempData["No"] = "Please Enter Roll Number to proceed";
                        return PartialView("_StdAttendance");
                    }
                }
                else if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            var getAttendance = con.att.Where(a => a.perId == getStd.perId).ToList();
                            if (getAttendance.Count != 0)
                            {
                                TempData["Attendance"] = getAttendance;
                            }
                            else
                            {
                                TempData["No"] = "No Record found";
                                return PartialView("_StdAttendance");
                            }
                            return PartialView("_StdAttendance");
                        }
                        else
                        {
                            TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return PartialView("_StdAttendance");
                        }

                    }
                    else
                    {
                        TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return PartialView("_StdAttendance");
                    }
                }
                else
                {
                    TempData["No"] = "Please Enter Roll Number to proceed";
                    return PartialView("_StdAttendance");
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error please contact to soft support";
                return PartialView("_StdAttendance");
            }
        }

        public ActionResult _StdHistory(string RollNo, int? SesId)
        {
            try

            {
                if (SesId != null)
                {
                    if (RollNo != "" && RollNo != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                TempData["Std"] = getStd;
                                var getInfo = con.stdEduInfos.Where(s => s.stdId == getStd.stdId).FirstOrDefault();
                                if (getInfo != null)
                                {
                                    TempData["PreInfo"] = getInfo;
                                }

                                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.stdId == getStd.stdId).ToList();
                                if (GetDetails.Count != 0)
                                {
                                    foreach (var i in GetDetails)
                                    {
                                        StdFeeDetail sd = new StdFeeDetail();
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
                                        sList.Add(sd);
                                    }

                                    TempData["Fee"] = sList;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                else
                                {
                                    TempData["No"] = "No Record found for Accounts";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }


                                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.stdId == getStd.stdId).ToList();
                                if (getStdDetail.Count != 0)
                                {
                                    TempData["StdMarks"] = getStdDetail;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                else
                                {
                                    TempData["No"] = "No Record found for Examination";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                var getAttendance = con.att.Where(a => a.perId == getStd.perId && a.sesId == SesId).ToList();
                                if (getAttendance.Count != 0)
                                {
                                    TempData["Attendance"] = getAttendance;
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }
                                else
                                {
                                    TempData["No"] = "No Record found for Attendance";
                                    var GetSes = con.InstSes.Where(s => s.sesId == SesId).Select(s => s.sesName).FirstOrDefault();
                                    if (GetSes != null)
                                    {
                                        TempData["Session"] = GetSes;
                                    }
                                }

                                return PartialView("_StdHistory");
                            }
                            else
                            {
                                TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                                return PartialView("_StdHistory");
                            }

                        }
                        else
                        {
                            TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                            return PartialView("_StdHistory");
                        }
                    }
                    else
                    {
                        TempData["No"] = "Please Enter Roll Number to proceed";
                        return PartialView("_StdHistory");
                    }
                }

                else if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            TempData["Std"] = getStd;
                            var getInfo = con.stdEduInfos.Where(s => s.stdId == getStd.stdId).FirstOrDefault();
                            if (getInfo != null)
                            {
                                TempData["PreInfo"] = getInfo;
                            }
                            List<StdFeeDetail> sList = new List<StdFeeDetail>();
                            var GetDetails = con.stdfee.Where(s => s.stdId == getStd.stdId).ToList();
                            if (GetDetails.Count != 0)
                            {
                                foreach (var i in GetDetails)
                                {
                                    StdFeeDetail sd = new StdFeeDetail();
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
                                    sList.Add(sd);
                                }
                                TempData["Fee"] = sList;
                            }
                            else
                            {
                                TempData["No"] = "No Record found for Accounts";
                            }


                            var getStdDetail = con.stdObtmark.Where(s => s.stdId == getStd.stdId).ToList();
                            if (getStdDetail.Count != 0)
                            {
                                TempData["StdMarks"] = getStdDetail;
                            }
                            else
                            {
                                TempData["No"] = "No Record found for Examination";
                            }
                            var getAttendance = con.att.Where(a => a.perId == getStd.perId).ToList();
                            if (getAttendance.Count != 0)
                            {
                                TempData["Attendance"] = getAttendance;
                            }
                            else
                            {
                                TempData["No"] = "No Record found for Attendance";
                            }

                            return PartialView("_StdHistory");
                        }
                        else
                        {
                            TempData["No"] = "Student Status is " + getStd.stdStatus + ". You cannot get any info of this student";
                            return PartialView("_StdHistory");
                        }

                    }
                    else
                    {
                        TempData["No"] = "Incorrect Roll number. Please Enter correct Roll Number";
                        return PartialView("_StdHistory");
                    }
                }
                else
                {
                    TempData["No"] = "Please Enter Roll Number to proceed";
                    return PartialView("_StdHistory");
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error please contact to soft support";
                return PartialView("_StdHistory");
            }
        }

        //Student Performance Report

        public ActionResult Student_Performance()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Student_Performance(string submitButton, string StdRoll)
        {
            try
            {
                if (submitButton == "Show")

                {
                    if (StdRoll != "" && StdRoll != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == StdRoll).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                TempData["RollNo"] = getStd.stdRollNo;
                                TempData["StdId"] = getStd.stdId;
                                var getStdExam = con.stdObtmark.Where(s => s.stdId == getStd.stdId).ToList();
                                if (getStdExam.Count != 0)
                                {
                                    List<ExamType> ExamList = new List<ExamType>();
                                    foreach (var i in getStdExam)
                                    {
                                        ExamType exam = new ExamType();
                                        var chkExamId = ExamList.Where(e => e.etId == i.etId).Any();
                                        if (chkExamId == false)
                                        {
                                            exam.etId = i.etId;
                                            exam.etname = i.et.etname;
                                            ExamList.Add(exam);
                                            SelectList Examin = new SelectList(ExamList, "etId", "etname");
                                            ViewData["Exam"] = Examin;
                                        }

                                    }

                                }
                                else
                                {
                                    TempData["Info"] = "No Examination Record found";
                                }
                            }
                            else
                            {
                                TempData["Error"] = "This Student status is " + getStd.stdStatus + ". we cannot perform any action.";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter correct Roll Number to proceed";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Enter Roll Number to proceed";
                    }
                }
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult _StdPerformance(string StdId, string Exam1, string Exam2)
        {
            try
            {
                if (StdId != "" && StdId != null)
                {
                    int Id = Convert.ToInt32(StdId);
                    int e1 = Convert.ToInt32(Exam1);
                    int e2 = Convert.ToInt32(Exam2);
                    var getStdExam = con.stdObtmark.Where(s => s.stdId == Id && (s.etId == e1 || s.etId == e2)).ToList();
                    if (getStdExam.Count != 0)
                    {
                        List<PerformanceVM> PerformList = new List<PerformanceVM>();
                        List<PerformanceVM> PerformanceList = new List<PerformanceVM>();
                        List<ExamType> ExamList = new List<ExamType>();
                        foreach (var i in getStdExam)
                        {
                            PerformanceVM perform = new PerformanceVM();
                            var chkData = PerformList.Where(p => p.sub1 == i.subId || p.sub2 == i.subId).FirstOrDefault();
                            if (chkData == null)
                            {
                                if(e1 == i.etId)
                                {
                                    perform.etId = i.etId;
                                    perform.sub1 = i.subId;
                                    perform.subName1 = i.sb.subName;
                                    perform.exam = i.et.etname;
                                    perform.Obt1 = i.totalObtainMarks;
                                    perform.Total1 = i.subTotalMarks;
                                    perform.per1 = i.totalObtainMarks * 100 / i.subTotalMarks;
                                    PerformList.Add(perform);
                                }
                                else
                                {
                                    perform.et2 = i.etId;
                                    perform.sub2 = i.subId;
                                    perform.subName2 = i.sb.subName;
                                    perform.exam2 = i.et.etname;
                                    perform.Obt2 = i.totalObtainMarks;
                                    perform.Total2 = i.subTotalMarks;
                                    perform.per2 = i.totalObtainMarks * 100 / i.subTotalMarks;
                                    PerformList.Add(perform);
                                }
                            }
                            else
                            {
                                if(e2 == i.etId)
                                {
                                    chkData.et2 = i.etId;
                                    chkData.sub2 = i.subId;
                                    chkData.subName2 = i.sb.subName;
                                    chkData.exam2 = i.et.etname;
                                    chkData.Obt2 = i.totalObtainMarks;
                                    chkData.Total2 = i.subTotalMarks;
                                    chkData.per2 = i.totalObtainMarks * 100 / i.subTotalMarks;
                                    if (chkData.per1 != 0 && chkData.per2 != 0)
                                    {
                                        chkData.performance = chkData.per1 - chkData.per2;
                                    }
                                    chkData.ExamsName = chkData.exam + "           &          " + chkData.exam2;
                                }
                                else
                                {
                                    chkData.etId = i.etId;
                                    chkData.sub1 = i.subId;
                                    chkData.subName1 = i.sb.subName;
                                    chkData.exam = i.et.etname;
                                    chkData.Obt1 = i.totalObtainMarks;
                                    chkData.Total1 = i.subTotalMarks;
                                    chkData.per1 = i.totalObtainMarks * 100 / i.subTotalMarks;
                                    if (chkData.per1 != 0 && chkData.per2 != 0)
                                    {
                                        chkData.performance = chkData.per1 - chkData.per2;
                                    }
                                    chkData.ExamsName = chkData.exam + " & " + chkData.exam2;
                                }
                                
                                TempData["ExamName"] = chkData.ExamsName;
                            }

                            ExamType exam = new ExamType();
                            var chkExamId = ExamList.Where(e => e.etId == i.etId).Any();
                            if (chkExamId == false)
                            {
                                exam.etId = i.etId;
                                exam.etname = i.et.etname;
                                ExamList.Add(exam);
                                ViewData["Exam"] = ExamList;
                            }

                            TempData["StdInfo"] =  "Name: " + i.std.pr.perName + ",  Roll Number: " + i.std.stdRollNo + " ";
                            TempData["StdEduInfo"] =  "Session: " + i.std.ses.sesName + ",  Class: " + i.std.cls.classname + ", Section: " + i.std.sec.secName;
                        }

                        TempData["Examination"] = getStdExam;
                        
                        TempData["PErformance"] = PerformList;
                        return PartialView("_StdPerformance");
                    }
                    else
                    {
                        TempData["No"] = "No Examination Record found";
                        return PartialView("_StdPerformance");
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter correct Roll Number to proceed";
                    return PartialView("_StdPerformance");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return PartialView("_StdPerformance");
            }
        }
        public void PopulatAllSes()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.InstSes.ToList(), "sesId", "sesName");
            ViewData["Session"] = sl;
        }
    }
}