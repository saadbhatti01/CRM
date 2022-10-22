
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class AppReportsController : Controller
    {
        DBCon con = new DBCon();
        // GET: Reports
        [CheckSession]
        public ActionResult Index()
        {
            PopulatClass();
            PopulatSec();
            PopulatAllSes();
            PopulatFeetype();
            return View();
        }
        [CheckSession]
        public ActionResult _GetFeeStatus(int SesId, int SecId, int ClassId, int fType, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                if (Status == "Paid")
                {
                    var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.feeTypeId == fType && s.IsDeleted == false).ToList();
                    if (GetDetails.Count != 0)
                    {
                        // var GetStdList = con.std.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId).ToList();
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
                            sd.FeePaidStatus = i.feeStatus;
                            sd.FeeDate = i.paidDate;
                            sList.Add(sd);
                        }
                        //TempData["StdList"] = GetStdList;
                        TempData["Fee"] = sList;
                    }
                }
                else if (Status == "UnPaid")
                {
                    var GetStdId = (from s in con.std
                                    where s.sesId == SesId && s.secId == SecId && s.classId == ClassId &&
                                    !(from sf in con.stdfee
                                      where s.stdId == sf.stdId && sf.sesId == SesId && sf.secId == SecId && sf.classId == ClassId && sf.feeTypeId == fType
                                      select sf.stdId).Contains(s.stdId)
                                    select new
                                    {
                                        s.stdId,
                                        s.stdRollNo,
                                    }).ToList();

                    //var GetStd = con.std.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId).ToList();
                    foreach (var i in GetStdId)
                    {
                        var GetStdDetails = con.std.Where(s => s.stdId == i.stdId && s.sesId == SesId && s.secId == SecId && s.classId == ClassId).FirstOrDefault();
                        var GetFee = con.feetype.Where(f => f.feeTypeId == fType).FirstOrDefault();
                        StdFeeDetail sd = new StdFeeDetail();
                        sd.Stdname = GetStdDetails.pr.perName;
                        sd.RollNo = GetStdDetails.stdRollNo;
                        sd.Ses = GetStdDetails.ses.sesName;
                        sd.Sec = GetStdDetails.sec.secName;
                        sd.Class = GetStdDetails.cls.classname;
                        sd.Fee = GetFee.feeTypeName;
                        sd.FeeAmt = 0000;
                        sd.FeePaidStatus = "UnPaid";
                        //sd.FeeDate = Convert.ToDateTime("01/01/0000");
                        sList.Add(sd);
                        TempData["Fee"] = sList;
                    }

                }

                return PartialView(TempData["Fee"]);
            }
            catch (Exception ex)
            {

            }
            return PartialView(TempData["Fee"]);
        }


        [CheckSession]
        public ActionResult DetailStd(int id)
        {
            var i = (from p in con.person
                     join s in con.std on p.perId equals s.perId
                     join c in con.city on p.CityId equals c.CityId
                     join a in con.area on p.AreaId equals a.AreaId
                     join cl in con.cls on s.classId equals cl.classId
                     join sc in con.InstSec on s.secId equals sc.secId
                     join ss in con.InstSes on s.sesId equals ss.sesId
                     join cm in con.camp on s.camId equals cm.camId
                     where p.perId == id && p.IsDeleted == false
                     select new
                     {
                         p.perId,
                         p.perName,
                         p.perGarName,
                         p.perDOB,
                         p.perCurrentAdrs,
                         p.perPermanantAdrs,
                         p.perContactOne,
                         p.perContactTwo,
                         p.perCNIC,
                         p.perEmail,
                         p.perBloodGrp,
                         p.perImage,
                         c.CityName,
                         a.AreaName,
                         sc.secName,
                         ss.sesName,
                         cl.classname,
                         s.stdRollNo,
                         s.stdStatus,
                         cm.campusname
                     }).SingleOrDefault();
            if (i != null)
            {
                RegViewModel reg = new RegViewModel();
                reg.perId = i.perId;
                reg.perName = i.perName;
                reg.perGarName = i.perGarName;
                reg.perDOB = i.perDOB;
                reg.perCurrentAdrs = i.perCurrentAdrs;
                reg.perPermanantAdrs = i.perPermanantAdrs;
                reg.perContactOne = i.perContactOne;
                reg.perContactTwo = i.perContactTwo;
                reg.perCNIC = i.perCNIC;
                reg.perEmail = i.perEmail;
                reg.perBloodGrp = i.perBloodGrp;
                reg.perImage = i.perImage;
                reg.CityName = i.CityName;
                reg.AreaName = i.AreaName;
                reg.secName = i.secName;
                reg.sesName = i.sesName;
                reg.ClassName = i.classname;
                reg.campusname = i.campusname;
                reg.stdRollNo = i.stdRollNo;
                reg.stdStatus = i.stdStatus;
                return View(reg);
            }
            else
            {
                TempData["Error"] = "Student information not available please try again";
            }
            return View();
        }
        /// <summary>
        /// Start Student Info Reporting
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult RptSessionWise()
        {
            try
            {
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _PopulateSesWise(int SesId, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.IsDeleted == false).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 1;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
                else
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.IsDeleted == false && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 1;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }
        [CheckSession]
        public ActionResult RptClassWise()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _PopulateClassWise(int SesId, int ClassId, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.classId == ClassId && c.IsDeleted == false).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 2;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["Class"] = Class;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
                else
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.classId == ClassId && c.IsDeleted == false && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 2;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["Class"] = Class;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }
        [CheckSession]
        public ActionResult RptSectionWise()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _PopulateSecWise(int SesId, int SecId, int ClassId, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.secId == SecId && c.classId == ClassId && c.IsDeleted == false).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 3;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["Class"] = Class;
                        var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Section"] = Section;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
                else
                {
                    var GetDetails = con.std.Where(c => c.sesId == SesId && c.secId == SecId && c.classId == ClassId && c.IsDeleted == false && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 3;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["Class"] = Class;
                        var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Class"] = Section;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }
        [CheckSession]
        public ActionResult RptCampusWise()
        {
            try
            {
                PopulateCampus();
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _PopulateCampusWise(int Camp, int SesId, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    var GetDetails = con.std.Where(c => c.camId == Camp && c.sesId == SesId && c.IsDeleted == false).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 4;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Campus = GetDetails.Where(s => s.camId == Camp).Select(a => a.cam.campusname).FirstOrDefault();
                        TempData["Campus"] = Campus;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
                else
                {
                    var GetDetails = con.std.Where(c => c.camId == Camp && c.sesId == SesId && c.IsDeleted == false && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 4;
                        TempData["List"] = GetDetails;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Campus = GetDetails.Where(s => s.camId == Camp).Select(a => a.cam.campusname).FirstOrDefault();
                        TempData["Campus"] = Campus;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }
        [CheckSession]
        public ActionResult RptDateWise()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]

        public ActionResult _PopulateDateWise(string ToDate, string FromDate, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        var GetDetails = con.std.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To).OrderBy(c => c.stdRollNo).ToList();
                        if (GetDetails.Count != 0)
                        {
                            ViewBag.ReportNo = 5;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_PopulateSudent");
                    }
                }
                else
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        var GetDetails = con.std.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                        if (GetDetails.Count != 0)
                        {
                            ViewBag.ReportNo = 5;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_PopulateSudent");
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }
        [CheckSession]
        public ActionResult RptBloodGrpWise()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]

        public ActionResult _PopulatebloodWise(string Blood, string Status)
        {
            try
            {
                if (Status == "All")
                {
                    var GetDetails = con.std.Where(c => c.pr.perBloodGrp == Blood && c.IsDeleted == false).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 6;
                        TempData["List"] = GetDetails;
                        TempData["Blood"] = Blood;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }
                else
                {
                    var GetDetails = con.std.Where(c => c.pr.perBloodGrp == Blood && c.IsDeleted == false && c.stdStatus == Status).OrderBy(c => c.stdRollNo).ToList();
                    if (GetDetails.Count != 0)
                    {
                        ViewBag.ReportNo = 6;
                        TempData["List"] = GetDetails;
                        TempData["Blood"] = Blood;
                        TempData["Status"] = Status;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateSudent");
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateSudent");
        }

        /// <summary>
        /// Start Fee Package Reporting
        /// </summary>
        /// <returns></returns>
        /// 
        [CheckSession]
        public ActionResult RptClassFeePkg()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _ClassFeePkg(int SesId, int ClassId)
        {
            try
            {
                var GetDetails = con.clfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    ViewBag.ReportNo = 1;
                    TempData["Class"] = GetDetails;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_ClassFeeDetail");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClassFeeDetail");
        }
        [CheckSession]
        public ActionResult RptSecFeePkg()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _SecFeePkg(int SesId, int ClassId, int SecId)
        {
            try
            {
                var GetDetails = con.clfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.secId == SecId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    ViewBag.ReportNo = 2;
                    TempData["Class"] = GetDetails;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;

                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_ClassFeeDetail");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClassFeeDetail");
        }
        [CheckSession]
        public ActionResult RptFeeTypePkg()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _fTypeFeePkg(int SesId, int ClassId, int SecId, int fTypeId)
        {
            try
            {
                var GetDetails = con.clfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.secId == SecId && c.feeTypeId == fTypeId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    ViewBag.ReportNo = 3;
                    TempData["Class"] = GetDetails;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var FeeType = GetDetails.Where(s => s.feeTypeId == fTypeId).Select(a => a.ft.feeTypeName).FirstOrDefault();
                    TempData["FeeType"] = FeeType;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_ClassFeeDetail");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClassFeeDetail");
        }

        [CheckSession]
        public ActionResult RptMaxDiscountFeePkg()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _MaxDisFeePkg(int SesId)
        {
            try
            {
                //var GetDetails = con.clfpkg.Where(c => c.sesId == con.clfpkg.Where(s => s.sesId == SesId).Max(s => s.cfpDis)).FirstOrDefault();
                var GetDetails = con.clfpkg.Where(c => c.sesId == SesId).OrderByDescending(s => s.cfpDis).FirstOrDefault();
                if (GetDetails != null)
                {
                    ViewBag.ReportNo = 4;
                    TempData["Max"] = GetDetails;
                    var Session = GetDetails.ses.sesName;
                    TempData["Session"] = Session;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_ClassFeeDetail");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_ClassFeeDetail");
        }

        /// <summary>
        /// Start Srudent Fee Package Report
        /// </summary>
        /// <returns></returns>
        /// 

        [CheckSession]
        public ActionResult RptStdFeePkgSes()
        {
            try
            {
                PopulatAllSes();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdFeePkgSession(int SesId, int fTypeId)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfpkg.Where(c => c.sesId == SesId && c.feeTypeId == fTypeId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    foreach (var i in GetDetails)
                    {
                        var GetStd = con.std.Where(s => s.stdId == i.sfpstdId).FirstOrDefault();
                        StdFeeDetail std = new StdFeeDetail();
                        std.Stdname = GetStd.pr.perName;
                        std.RollNo = GetStd.stdRollNo;
                        std.Ses = GetStd.ses.sesName;
                        std.Sec = GetStd.sec.secName;
                        std.Class = GetStd.cls.classname;
                        std.FeeAmt = i.sfpAmt;
                        std.Dis = i.sfpDis;
                        std.Fee = i.ft.feeTypeName;
                        std.StdStatus = GetStd.stdStatus;
                        sList.Add(std);
                    }

                    TempData["FeePkgDetail"] = sList.Where(s => s.StdStatus == "Active").OrderBy(s => s.RollNo).ToList();
                    ViewBag.ReportNo = 1;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var FeeType = GetDetails.Where(s => s.feeTypeId == fTypeId).Select(a => a.ft.feeTypeName).FirstOrDefault();
                    TempData["FeeType"] = FeeType;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdFeePkg");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdFeePkg");
        }
        [CheckSession]
        public ActionResult RptStdFeePkgClass()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdFeePkgClass(int SesId, int ClassId)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    foreach (var i in GetDetails)
                    {
                        var GetStd = con.std.Where(s => s.stdId == i.sfpstdId).FirstOrDefault();
                        StdFeeDetail std = new StdFeeDetail();
                        std.Stdname = GetStd.pr.perName;
                        std.RollNo = GetStd.stdRollNo;
                        std.Ses = GetStd.ses.sesName;
                        std.Sec = GetStd.sec.secName;
                        std.Class = GetStd.cls.classname;
                        std.FeeAmt = i.sfpAmt;
                        std.Dis = i.sfpDis;
                        std.Fee = i.ft.feeTypeName;
                        std.StdStatus = GetStd.stdStatus;
                        sList.Add(std);
                    }
                    TempData["FeePkgDetail"] = sList.Where(s => s.StdStatus == "Active").OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 2;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdFeePkg");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdFeePkg");
        }
        [CheckSession]
        public ActionResult RptStdFeePkgSec()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdFeePkgSec(int SesId, int ClassId, int SecId)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.secId == SecId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    foreach (var i in GetDetails)
                    {
                        var GetStd = con.std.Where(s => s.stdId == i.sfpstdId).FirstOrDefault();
                        StdFeeDetail std = new StdFeeDetail();
                        std.Stdname = GetStd.pr.perName;
                        std.RollNo = GetStd.stdRollNo;
                        std.Ses = GetStd.ses.sesName;
                        std.Sec = GetStd.sec.secName;
                        std.Class = GetStd.cls.classname;
                        std.FeeAmt = i.sfpAmt;
                        std.Dis = i.sfpDis;
                        std.Fee = i.ft.feeTypeName;
                        std.StdStatus = GetStd.stdStatus;
                        sList.Add(std);
                    }
                    TempData["FeePkgDetail"] = sList.Where(s => s.StdStatus == "Active").OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 3;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdFeePkg");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdFeePkg");
        }
        [CheckSession]
        public ActionResult RptStdFeePkgType()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _StdFeePkgType(int SesId, int ClassId, int SecId, int fTypeId)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfpkg.Where(c => c.sesId == SesId && c.classId == ClassId && c.secId == SecId && c.feeTypeId == fTypeId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    foreach (var i in GetDetails)
                    {
                        var GetStd = con.std.Where(s => s.stdId == i.sfpstdId).FirstOrDefault();
                        StdFeeDetail std = new StdFeeDetail();
                        std.Stdname = GetStd.pr.perName;
                        std.RollNo = GetStd.stdRollNo;
                        std.Ses = GetStd.ses.sesName;
                        std.Sec = GetStd.sec.secName;
                        std.Class = GetStd.cls.classname;
                        std.FeeAmt = i.sfpAmt;
                        std.Dis = i.sfpDis;
                        std.Fee = i.ft.feeTypeName;
                        std.StdStatus = GetStd.stdStatus;
                        sList.Add(std);
                    }
                    TempData["FeePkgDetail"] = sList.Where(s => s.StdStatus == "Active").OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 4;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var FeeType = GetDetails.Where(s => s.feeTypeId == fTypeId).Select(a => a.ft.feeTypeName).FirstOrDefault();
                    TempData["FeeType"] = FeeType;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdFeePkg");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdFeePkg");
        }

        /// <summary>
        /// Start Received Fee Reporting
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult RptStdSesFee()
        {
            try
            {
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdSesFee(int SesId, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.feeStatus == Status && s.std.stdStatus == "Active" && s.IsDeleted == false).ToList();
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
                    TempData["Fee"] = sList.OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 1;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    TempData["Status"] = Status;
                }
                else
                {
                    TempData["No"] = "No Record found";
                }

                return PartialView("_GetFeeStatus");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_GetFeeStatus");
        }
        [CheckSession]
        public ActionResult RptStdClassFee()
        {
            try
            {
                PopulatClass();
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdClassFee(int SesId, int ClassId, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();


                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.classId == ClassId && s.feeStatus == Status && s.std.stdStatus == "Active" && s.IsDeleted == false).ToList();
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
                    TempData["Fee"] = sList.OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 2;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    TempData["Status"] = Status;
                }
                else
                {
                    TempData["No"] = "No Record found";
                }

                return PartialView("_GetFeeStatus");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_GetFeeStatus");
        }
        [CheckSession]
        public ActionResult RptStdSecFee()
        {
            try
            {
                PopulatClass();
                PopulatAllSes();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdSecFee(int SesId, int ClassId, int SecId, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();


                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.classId == ClassId && s.secId == SecId && s.feeStatus == Status && s.std.stdStatus == "Active" && s.IsDeleted == false).ToList();
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
                    TempData["Fee"] = sList.OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 3;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    TempData["Status"] = Status;
                }

                else
                {
                    TempData["No"] = "No Record found";
                }
                return PartialView("_GetFeeStatus");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_GetFeeStatus");
        }
        [CheckSession]
        public ActionResult RptStdTypeFee()
        {
            try
            {
                PopulatClass();
                PopulatAllSes();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdTypeFee(int SesId, int ClassId, int SecId, int fType, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.classId == ClassId && s.secId == SecId && s.feeTypeId == fType && s.feeStatus == Status && s.std.stdStatus == "Active" && s.IsDeleted == false).ToList();
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
                    TempData["Fee"] = sList.OrderBy(s => s.RollNo).ToList();

                    ViewBag.ReportNo = 4;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var FeeType = GetDetails.Where(s => s.feeTypeId == fType).Select(a => a.ft.feeTypeName).FirstOrDefault();
                    TempData["FeeType"] = FeeType;
                    TempData["Status"] = Status;
                }
                else
                {
                    TempData["No"] = "No Record found";
                }

                return PartialView("_GetFeeStatus");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_GetFeeStatus");
        }

        [CheckSession]
        public ActionResult RptStdDateWiseFee()
        {
            try
            {
                PopulatClass();
                PopulatAllSes();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _StdDateWiseFee(int SesId, int ClassId, int SecId, int fType, string ToDate, string FromDate, string Status)
        {
            try
            {
                if (ToDate != null && FromDate != null)
                {
                    DateTime tDate = Convert.ToDateTime(ToDate);
                    DateTime fDate = Convert.ToDateTime(FromDate);
                    List<StdFeeDetail> sList = new List<StdFeeDetail>();
                    var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.classId == ClassId && s.secId == SecId && s.feeTypeId == fType && s.feeStatus == Status && s.paidDate >= fDate && s.paidDate <= tDate && s.std.stdStatus == "Active" && s.IsDeleted == false).ToList();
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
                        TempData["Fee"] = sList.OrderBy(s => s.RollNo).ToList();

                        ViewBag.ReportNo = 5;
                        var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["ClassName"] = Class;
                        var Section = GetDetails.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Section"] = Section;
                        var FeeType = GetDetails.Where(s => s.feeTypeId == fType).Select(a => a.ft.feeTypeName).FirstOrDefault();
                        TempData["FeeType"] = FeeType;
                        TempData["Status"] = Status;
                        TempData["fDate"] = FromDate;
                        TempData["tDate"] = ToDate;

                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }

                    return PartialView("_GetFeeStatus");
                }
                else
                {
                    TempData["No"] = "Please Select Date!";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_GetFeeStatus");
        }

        /// <summary>
        /// End Received Fee Report///
        /// </summary>

        //Start Extra Fee Reporting
        [CheckSession]
        public ActionResult RptExtraFeeSes()
        {
            try
            {
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _ExtraFeeSes(int SesId)
        {
            try
            {
                var GetDetails = con.StudentExtraFee.Where(s => s.sesId == SesId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList().ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["Fee"] = GetDetails;

                    ViewBag.ReportNo = 1;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;

                    return PartialView("_ExtraFee");
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ExtraFee");
        }

        [CheckSession]
        public ActionResult RptExtraFeeClass()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _ExtraFeeClass(int SesId, int ClassId)
        {
            try
            {
                var GetDetails = con.StudentExtraFee.Where(s => s.sesId == SesId && s.classId == ClassId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["Fee"] = GetDetails;

                    ViewBag.ReportNo = 2;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;

                    return PartialView("_ExtraFee");
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ExtraFee");
        }
        [CheckSession]
        public ActionResult RptExtraFeeType()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatExtraFeetype();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _ExtraFeeType(int SesId, int ClassId, int EfId)
        {
            try
            {

                var GetDetails = con.StudentExtraFee.Where(s => s.sesId == SesId && s.classId == ClassId && s.eftId == EfId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["Fee"] = GetDetails;

                    ViewBag.ReportNo = 3;
                    var Session = GetDetails.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = GetDetails.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var ExtraFeeType = GetDetails.Where(s => s.eftId == EfId).Select(a => a.eft.eftName).FirstOrDefault();
                    TempData["ExtraFeeType"] = ExtraFeeType;

                    return PartialView("_ExtraFee");
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_ExtraFee");
        }

        /// <summary>
        /// Start Student Exam Report 
        /// </summary>
        /// <returns></returns>
        [CheckSession]
        public ActionResult RptExamClass()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdExamClass(int SesId, int ClassId)
        {
            try
            {
                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.classId == ClassId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (getStdDetail.Count != 0)
                {
                    TempData["StdMarks"] = getStdDetail;

                    ViewBag.ReportNo = 1;
                    var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdExam");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExam");
        }
        [CheckSession]
        public ActionResult RptExamSec()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdExamSection(int SesId, int ClassId, int SecId)
        {
            try
            {
                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (getStdDetail.Count != 0)
                {
                    TempData["StdMarks"] = getStdDetail;

                    ViewBag.ReportNo = 2;
                    var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;

                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdExam");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExam");
        }
        [CheckSession]
        public ActionResult RptExamSubject()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatSubJect();

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        public ActionResult _StdExamSubject(int SesId, int ClassId, int SecId, int SubId)
        {
            try
            {
                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.subId == SubId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (getStdDetail.Count != 0)
                {
                    TempData["StdMarks"] = getStdDetail;

                    ViewBag.ReportNo = 3;
                    var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var Subject = getStdDetail.Where(s => s.subId == SubId).Select(a => a.sb.subName).FirstOrDefault();
                    TempData["Subject"] = Subject;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdExam");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExam");
        }
        [CheckSession]
        public ActionResult RptExamType()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatExamType();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _StdExamType(int SesId, int ClassId, int SecId, int EType)
        {
            try
            {
                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.etId == EType && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (getStdDetail.Count != 0)
                {
                    TempData["StdMarks"] = getStdDetail;

                    ViewBag.ReportNo = 4;
                    var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var Exam = getStdDetail.Where(s => s.etId == EType).Select(a => a.et.etname).FirstOrDefault();
                    TempData["Exam"] = Exam;
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_StdExam");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExam");
        }

        public ActionResult RptStudentExam()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatExamType();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _RptStudentExam(int SesId, int ClassId, int SecId, int EType, string RollNo)
        {
            try
            {
                var getStdId = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                if (getStdId != null)
                {
                    var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.etId == EType && s.stdId == getStdId.stdId && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();

                    if (getStdDetail.Count != 0)
                    {
                        TempData["StdMarks"] = getStdDetail;

                        ViewBag.ReportNo = 4;
                        var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["ClassName"] = Class;
                        var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Section"] = Section;
                        var Exam = getStdDetail.Where(s => s.etId == EType).Select(a => a.et.etname).FirstOrDefault();
                        TempData["Exam"] = Exam;
                        TempData["Student"] = "" + getStdId.pr.perName + "";
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                }
                else
                {
                    TempData["No"] = "Incorrect Roll number.";
                }

                return PartialView("_StdExam");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExam");
        }


        [CheckSession]
        public ActionResult RptPassStudent()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatExamType();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _StdPassed(int SesId, int ClassId, int SecId, int EType)
        {
            try
            {
                List<StdObtainMarks> StdPassed = new List<StdObtainMarks>();
                List<StdObtainMarks> Stdfailed = new List<StdObtainMarks>();
                var getPassingPercent = con.passing.Where(p => p.classId == ClassId).FirstOrDefault();
                if (getPassingPercent != null)
                {
                    var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.etId == EType && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                    if (getStdDetail.Count != 0)
                    {
                        foreach (var i in getStdDetail)
                        {
                            double ObtainMarksPercentage = (i.totalObtainMarks * 100) / i.subTotalMarks;
                            if (ObtainMarksPercentage >= getPassingPercent.Marks)
                            {
                                var chk = StdPassed.Where(s => s.stdId == i.stdId).Any();
                                if (chk == false)
                                {
                                    StdPassed.Add(i);
                                }
                            }
                            else
                            {
                                var chk = Stdfailed.Where(s => s.stdId == i.stdId).Any();
                                if (chk == false)
                                {
                                    Stdfailed.Add(i);
                                }
                            }

                        }

                        var StdId = (from sa in StdPassed
                                     where !Stdfailed
                                              .Any(o => o.stdId == sa.stdId)
                                     select new
                                     {
                                         sa.stdId,
                                     }).ToList();
                        if (StdId.Count != 0)
                        {
                            List<Student> PassList = new List<Student>();
                            foreach (var i in StdId)
                            {
                                var getPassStudent = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                PassList.Add(getPassStudent);
                                TempData["List"] = PassList;
                            }
                        }
                        else
                        {
                            TempData["No"] = "No Pass Student Found.";
                        }





                        ViewBag.ReportNo = 1;
                        var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["ClassName"] = Class;
                        var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Section"] = Section;
                        var Exam = getStdDetail.Where(s => s.etId == EType).Select(a => a.et.etname).FirstOrDefault();
                        TempData["Exam"] = Exam;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                }
                else
                {
                    TempData["No"] = "Passing marks of this class is not entered. Please enter passing marks and try again";
                }

                return PartialView("_StdExamResult");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExamResult");
        }

        [CheckSession]
        public ActionResult RptFailStudent()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                PopulatExamType();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _StdFailed(int SesId, int ClassId, int SecId, int EType)
        {
            try
            {
                List<Student> StdFailed = new List<Student>();
                var getPassingPercent = con.passing.Where(p => p.classId == ClassId).FirstOrDefault();
                if (getPassingPercent != null)
                {
                    var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.etId == EType && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                    if (getStdDetail.Count != 0)
                    {
                        foreach (var i in getStdDetail)
                        {
                            double ObtainMarksPercentage = (i.totalObtainMarks * 100) / i.subTotalMarks;
                            if (ObtainMarksPercentage < getPassingPercent.Marks)
                            {
                                var chk = StdFailed.Where(s => s.stdId == i.stdId).Any();
                                if (chk == false)
                                {
                                    var getstd = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                    StdFailed.Add(getstd);
                                }
                            }
                            TempData["List"] = StdFailed;
                        }

                        ViewBag.ReportNo = 1;
                        var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                        TempData["ClassName"] = Class;
                        var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                        TempData["Section"] = Section;
                        var Exam = getStdDetail.Where(s => s.etId == EType).Select(a => a.et.etname).FirstOrDefault();
                        TempData["Exam"] = Exam;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                }
                else
                {
                    TempData["No"] = "Passing marks of this class is not entered. Please enter passing marks and try again";
                }

                return PartialView("_StdExamResult");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_StdExamResult");
        }


        //Start Short Course Report
        [CheckSession]
        public ActionResult RptShortCourseAll()
        {
            try
            {
                PopulatShortCourse();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Report Cannot generated";
                return View();
            }
        }

        [CheckSession]
        public ActionResult _PopulateShortCourse(int? ScsId)
        {
            try
            {
                if (ScsId == 0 || ScsId == null)
                {
                    var getCourseDetail = con.scr.OrderBy(s => s.std.stdRollNo).ToList();
                    if (getCourseDetail.Count != 0)
                    {
                        TempData["Course"] = getCourseDetail;
                        ViewBag.ReportNo = 1;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateShortCourse");
                }
                else
                {
                    var getCourseDetail = con.scr.Where(s => s.scsId == ScsId).OrderBy(s => s.std.stdRollNo).ToList();
                    if (getCourseDetail.Count != 0)
                    {
                        TempData["Course"] = getCourseDetail;
                        ViewBag.ReportNo = 2;

                        var Course = getCourseDetail.Where(s => s.scsId == ScsId).Select(a => a.scSub.scsName).FirstOrDefault();
                        TempData["CourseName"] = Course;

                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_PopulateShortCourse");
                }

            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulateShortCourse");
        }


        /// <summary>
        /// Start Attendance Report
        /// </summary>
        /// 

        [CheckSession]
        public ActionResult RptDateWiseStdAtten()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();

        }

        [CheckSession]
        public ActionResult _RptDateWiseStdAtten(int SesId, int ClassId, int SecId, string ToDate, string FromDate)
        {
            try
            {
                if (FromDate != null)
                {
                    List<TeacherAttendance> AttenList = new List<TeacherAttendance>();
                    //DateTime tDate = Convert.ToDateTime(ToDate);
                    DateTime fDate = Convert.ToDateTime(FromDate);
                    //DateTime tDate = Convert.ToDateTime(ToDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                    //DateTime fDate = Convert.ToDateTime(FromDate).AddHours(23).AddMinutes(59).AddSeconds(59);
                    var getAttn = con.att.Where(a => a.attenDate == fDate && a.sesId == SesId && a.classId == ClassId && a.secId == SecId).ToList();
                    if (getAttn.Count != 0)
                    {
                        foreach (var i in getAttn)
                        {
                            TeacherAttendance att = new TeacherAttendance();
                            var getRollNo = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                            if (getRollNo != null)
                            {
                                att.RollNo = getRollNo.stdRollNo;
                            }
                            att.Name = i.pr.perName;
                            att.Date = i.attenDate;
                            att.Time = i.attenTime;
                            att.Status = "Present";
                            AttenList.Add(att);
                            TempData["Attendance"] = AttenList;
                        }
                    }
                    var AbsentStdList = (from sa in con.std
                                         where !con.att
                                                  .Any(o => o.perId == sa.perId && o.classId == ClassId && o.sesId == SesId && o.secId == SecId
                                                  && o.attenDate == fDate)
                                         where sa.classId == ClassId && sa.sesId == SesId && sa.secId == SecId && sa.IsDeleted == false && sa.stdStatus == "Active"

                                         select new
                                         {
                                             sa.stdId
                                         }).ToList();
                    if (AbsentStdList.Count != 0)
                    {
                        foreach (var i in AbsentStdList)
                        {
                            TeacherAttendance att = new TeacherAttendance();
                            var getStd = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                            if (getStd != null)
                            {
                                att.Name = getStd.pr.perName;
                                att.RollNo = getStd.stdRollNo;
                                att.Status = "Absent";
                                AttenList.Add(att);
                                TempData["Attendance"] = AttenList;
                            }
                        }
                    }
                    var Session = getAttn.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getAttn.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getAttn.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    TempData["Date"] = "Student Attendance Report Dated: " + fDate.ToString("dd-MMM-yyyy") + "";
                    return PartialView("_RptDateWiseStdAtten");
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult RptAttenSession()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _RptAttenSession(int SesId, int ClassId, int SecId, int Year, int Month)
        {
            try
            {
                List<AttendenceRegister> aList = new List<AttendenceRegister>();
                List<Person> PerIdList = new List<Person>();
                List<Student> StudentList = new List<Student>();
                var getStdAttenDetail = con.att.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId
                && s.attenDate.Month == Month && s.attenDate.Year == Year
                ).OrderBy(s => s.attenDateTime).ToList();
                if (getStdAttenDetail.Count != 0)
                {
                    foreach (var i in getStdAttenDetail)
                    {
                        Person per = new Person();
                        per.perId = i.perId;
                        PerIdList.Add(per);
                    }
                    foreach (var p in PerIdList)
                    {
                        Student s = new Student();
                        var chkList = StudentList.Where(sa => sa.perId == p.perId).FirstOrDefault();
                        if (chkList == null)
                        {
                            var getStd = con.std.Where(st => st.perId == p.perId && st.stdStatus == "Active").FirstOrDefault();
                            if (getStd != null)
                            {
                                StudentList.Add(getStd);
                            }
                        }

                    }

                    //var getAllStd = con.std.Where(s => s.sesId == SesId && s.classId == ClassId && s.secId == SecId && s.stdStatus == "Active").ToList();
                    foreach (var i in StudentList)
                    {
                        AttendenceRegister att = new AttendenceRegister();

                        foreach (var s in getStdAttenDetail)
                        {
                            if (i.perId == s.perId)
                            {
                                att.RollNo = i.stdRollNo;
                                att.Name = i.pr.perName;
                                //Type = (drr["Status"]).ToString();
                                if (att.D1 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 1)
                                    {
                                        att.D1 = "P";
                                    }
                                    else
                                    {
                                        att.D1 = "A";
                                    }
                                }
                                if (att.D2 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 2)
                                    {
                                        att.D2 = "P";
                                    }
                                    else
                                    {
                                        att.D2 = "A";
                                    }
                                }
                                if (att.D3 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 3)
                                    {
                                        att.D3 = "P";
                                    }
                                    else
                                    {
                                        att.D3 = "A";
                                    }
                                }
                                if (att.D4 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 4)
                                    {
                                        att.D4 = "P";
                                    }
                                    else
                                    {
                                        att.D4 = "A";
                                    }
                                }
                                if (att.D5 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 5)
                                    {
                                        att.D5 = "P";
                                    }
                                    else
                                    {
                                        att.D5 = "A";
                                    }
                                }
                                if (att.D6 == "P")
                                {

                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 6)
                                    {
                                        att.D6 = "P";
                                    }
                                    else
                                    {
                                        att.D6 = "A";
                                    }
                                }
                                if (att.D7 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 7)
                                    {
                                        att.D7 = "P";
                                    }
                                    else
                                    {
                                        att.D7 = "A";
                                    }
                                }
                                if (att.D8 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 8)
                                    {
                                        att.D8 = "P";
                                    }
                                    else
                                    {
                                        att.D8 = "A";
                                    }
                                }
                                if (att.D9 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 9)
                                    {
                                        att.D9 = "P";
                                    }
                                    else
                                    {
                                        att.D9 = "A";
                                    }

                                }
                                if (att.D10 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 10)
                                    {
                                        att.D10 = "P";
                                    }
                                    else
                                    {
                                        att.D10 = "A";
                                    }
                                }
                                if (att.D11 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 11)
                                    {
                                        att.D11 = "P";
                                    }
                                    else
                                    {
                                        att.D11 = "A";
                                    }
                                }
                                if (att.D12 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 12)
                                    {
                                        att.D12 = "P";
                                    }
                                    else
                                    {
                                        att.D12 = "A";
                                    }
                                }
                                if (att.D13 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 13)
                                    {
                                        att.D13 = "P";
                                    }
                                    else
                                    {
                                        att.D13 = "A";
                                    }
                                }
                                if (att.D14 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 14)
                                    {
                                        att.D14 = "P";
                                    }
                                    else
                                    {
                                        att.D14 = "A";
                                    }
                                }
                                if (att.D15 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 15)
                                    {
                                        att.D15 = "P";
                                    }
                                    else
                                    {
                                        att.D15 = "A";
                                    }

                                }
                                if (att.D16 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 16)
                                    {
                                        att.D16 = "P";
                                    }
                                    else
                                    {
                                        att.D16 = "A";
                                    }
                                }
                                if (att.D17 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 17)
                                    {
                                        att.D17 = "P";
                                    }
                                    else
                                    {
                                        att.D17 = "A";
                                    }
                                }
                                if (att.D18 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 18)
                                    {
                                        att.D18 = "P";
                                    }
                                    else
                                    {
                                        att.D18 = "A";
                                    }
                                }
                                if (att.D19 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 19)
                                    {
                                        att.D19 = "P";
                                    }
                                    else
                                    {
                                        att.D19 = "A";
                                    }
                                }
                                if (att.D20 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 20)
                                    {
                                        att.D20 = "P";
                                    }
                                    else
                                    {
                                        att.D20 = "A";
                                    }
                                }
                                if (att.D21 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 21)
                                    {
                                        att.D21 = "P";
                                    }
                                    else
                                    {
                                        att.D21 = "A";
                                    }
                                }
                                if (att.D22 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 22)
                                    {
                                        att.D22 = "P";
                                    }
                                    else
                                    {
                                        att.D22 = "A";
                                    }
                                }
                                if (att.D23 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 23)
                                    {
                                        att.D23 = "P";
                                    }
                                    else
                                    {
                                        att.D23 = "A";
                                    }
                                }
                                if (att.D24 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 24)
                                    {
                                        att.D24 = "P";
                                    }
                                    else
                                    {
                                        att.D24 = "A";
                                    }
                                }
                                if (att.D25 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 25)
                                    {
                                        att.D25 = "P";
                                    }
                                    else
                                    {
                                        att.D25 = "A";
                                    }
                                }
                                if (att.D26 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 26)
                                    {
                                        att.D26 = "P";
                                    }
                                    else
                                    {
                                        att.D26 = "A";
                                    }
                                }
                                if (att.D27 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 28)
                                    {
                                        att.D27 = "P";
                                    }
                                    else
                                    {
                                        att.D27 = "A";
                                    }
                                }
                                if (att.D28 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 28)
                                    {
                                        att.D28 = "P";
                                    }
                                    else
                                    {
                                        att.D28 = "A";
                                    }
                                }
                                if (att.D29 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 29)
                                    {
                                        att.D29 = "P";
                                    }
                                    else
                                    {
                                        att.D29 = "A";
                                    }
                                }
                                if (att.D30 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 30)
                                    {
                                        att.D30 = "P";
                                    }
                                    else
                                    {
                                        att.D30 = "A";
                                    }
                                }
                                if (att.D31 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 31)
                                    {
                                        att.D31 = "P";
                                    }
                                    else
                                    {
                                        att.D31 = "A";
                                    }
                                }
                            }

                        }
                        aList.Add(att);
                    }

                    TempData["StdAttendance"] = aList;
                    ViewBag.ReportNo = 1;
                    var Session = getStdAttenDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdAttenDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getStdAttenDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    TempData["Month"] = "Class Wise Attendance Report Month(" + Month + "-" + Year + ")";
                }
                else
                {
                    TempData["No"] = "No Record found.";
                }
                return PartialView("_RptAttenSession");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_RptAttenSession");
        }

        public ActionResult RptAttenStd()
        {
            try
            {
                PopulatAllSes();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        public ActionResult _RptAttenStd(int SesId, int Year, int Month, string RollNo)
        {
            try
            {
                List<AttendenceRegister> aList = new List<AttendenceRegister>();
                var getperId = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                if (getperId != null)
                {
                    var getStdAtten = con.att.Where(s => s.sesId == SesId && s.perId == getperId.perId
                    && s.attenDate.Month == Month && s.attenDate.Year == Year
                    ).OrderBy(s => s.attenDateTime).ToList();
                    if (getStdAtten.Count != 0)
                    {
                        AttendenceRegister att = new AttendenceRegister();
                        foreach (var s in getStdAtten)
                        {
                            if (getperId.perId == s.perId)
                            {
                                att.RollNo = getperId.stdRollNo;
                                att.Name = getperId.pr.perName;
                                if (att.D1 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 1)
                                    {
                                        att.D1 = "P";
                                    }
                                    else
                                    {
                                        att.D1 = "A";
                                    }
                                }
                                if (att.D2 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 2)
                                    {
                                        att.D2 = "P";
                                    }
                                    else
                                    {
                                        att.D2 = "A";
                                    }
                                }
                                if (att.D3 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 3)
                                    {
                                        att.D3 = "P";
                                    }
                                    else
                                    {
                                        att.D3 = "A";
                                    }
                                }
                                if (att.D4 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 4)
                                    {
                                        att.D4 = "P";
                                    }
                                    else
                                    {
                                        att.D4 = "A";
                                    }
                                }
                                if (att.D5 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 5)
                                    {
                                        att.D5 = "P";
                                    }
                                    else
                                    {
                                        att.D5 = "A";
                                    }
                                }
                                if (att.D6 == "P")
                                {

                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 6)
                                    {
                                        att.D6 = "P";
                                    }
                                    else
                                    {
                                        att.D6 = "A";
                                    }
                                }
                                if (att.D7 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 7)
                                    {
                                        att.D7 = "P";
                                    }
                                    else
                                    {
                                        att.D7 = "A";
                                    }
                                }
                                if (att.D8 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 8)
                                    {
                                        att.D8 = "P";
                                    }
                                    else
                                    {
                                        att.D8 = "A";
                                    }
                                }
                                if (att.D9 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 9)
                                    {
                                        att.D9 = "P";
                                    }
                                    else
                                    {
                                        att.D9 = "A";
                                    }

                                }
                                if (att.D10 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 10)
                                    {
                                        att.D10 = "P";
                                    }
                                    else
                                    {
                                        att.D10 = "A";
                                    }
                                }
                                if (att.D11 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 11)
                                    {
                                        att.D11 = "P";
                                    }
                                    else
                                    {
                                        att.D11 = "A";
                                    }
                                }
                                if (att.D12 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 12)
                                    {
                                        att.D12 = "P";
                                    }
                                    else
                                    {
                                        att.D12 = "A";
                                    }
                                }
                                if (att.D13 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 13)
                                    {
                                        att.D13 = "P";
                                    }
                                    else
                                    {
                                        att.D13 = "A";
                                    }
                                }
                                if (att.D14 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 14)
                                    {
                                        att.D14 = "P";
                                    }
                                    else
                                    {
                                        att.D14 = "A";
                                    }
                                }
                                if (att.D15 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 15)
                                    {
                                        att.D15 = "P";
                                    }
                                    else
                                    {
                                        att.D15 = "A";
                                    }

                                }
                                if (att.D16 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 16)
                                    {
                                        att.D16 = "P";
                                    }
                                    else
                                    {
                                        att.D16 = "A";
                                    }
                                }
                                if (att.D17 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 17)
                                    {
                                        att.D17 = "P";
                                    }
                                    else
                                    {
                                        att.D17 = "A";
                                    }
                                }
                                if (att.D18 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 18)
                                    {
                                        att.D18 = "P";
                                    }
                                    else
                                    {
                                        att.D18 = "A";
                                    }
                                }
                                if (att.D19 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 19)
                                    {
                                        att.D19 = "P";
                                    }
                                    else
                                    {
                                        att.D19 = "A";
                                    }
                                }
                                if (att.D20 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 20)
                                    {
                                        att.D20 = "P";
                                    }
                                    else
                                    {
                                        att.D20 = "A";
                                    }
                                }
                                if (att.D21 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 21)
                                    {
                                        att.D21 = "P";
                                    }
                                    else
                                    {
                                        att.D21 = "A";
                                    }
                                }
                                if (att.D22 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 22)
                                    {
                                        att.D22 = "P";
                                    }
                                    else
                                    {
                                        att.D22 = "A";
                                    }
                                }
                                if (att.D23 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 23)
                                    {
                                        att.D23 = "P";
                                    }
                                    else
                                    {
                                        att.D23 = "A";
                                    }
                                }
                                if (att.D24 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 24)
                                    {
                                        att.D24 = "P";
                                    }
                                    else
                                    {
                                        att.D24 = "A";
                                    }
                                }
                                if (att.D25 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 25)
                                    {
                                        att.D25 = "P";
                                    }
                                    else
                                    {
                                        att.D25 = "A";
                                    }
                                }
                                if (att.D26 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 26)
                                    {
                                        att.D26 = "P";
                                    }
                                    else
                                    {
                                        att.D26 = "A";
                                    }
                                }
                                if (att.D27 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 28)
                                    {
                                        att.D27 = "P";
                                    }
                                    else
                                    {
                                        att.D27 = "A";
                                    }
                                }
                                if (att.D28 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 28)
                                    {
                                        att.D28 = "P";
                                    }
                                    else
                                    {
                                        att.D28 = "A";
                                    }
                                }
                                if (att.D29 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 29)
                                    {
                                        att.D29 = "P";
                                    }
                                    else
                                    {
                                        att.D29 = "A";
                                    }
                                }
                                if (att.D30 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 30)
                                    {
                                        att.D30 = "P";
                                    }
                                    else
                                    {
                                        att.D30 = "A";
                                    }
                                }
                                if (att.D31 == "P")
                                {
                                }
                                else
                                {
                                    if (Convert.ToInt32(s.attenDate.Day) == 31)
                                    {
                                        att.D31 = "P";
                                    }
                                    else
                                    {
                                        att.D31 = "A";
                                    }
                                }
                            }
                        }
                        aList.Add(att);


                        TempData["StdAttendance"] = aList;
                        ViewBag.ReportNo = 2;
                        var Session = getStdAtten.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                        TempData["Session"] = Session;
                        TempData["Month"] = "Student Wise Attendance Report [" + RollNo + "]-(" + Month + "-" + Year + ")";
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }

                }
                else
                {
                    TempData["No"] = "INvalid Roll Number. Please Enter correct roll number!";
                }

                return PartialView("_RptAttenSession");
            }
            catch (Exception ex)
            {

            }
            return PartialView("_RptAttenSession");
        }

        public ActionResult RptAttenTeacher()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
            return View();
        }

        public ActionResult _RptAttenTeacher(int Year, int Month, string Code)
        {
            try
            {
                var GetTeacherId = con.perReg.Where(p => p.prBarcode == Code || p.prRFID == Code).FirstOrDefault();
                if (GetTeacherId != null)
                {
                    var getAttendance = con.att.Where(a => a.perId == GetTeacherId.perId
                    && a.attenDate.Month == Month && a.attenDate.Year == Year).ToList();
                    if (getAttendance.Count != 0)
                    {
                        List<TeacherAttendance> tList = new List<TeacherAttendance>();
                        int Day = 0;
                        foreach (var i in getAttendance)
                        {
                            Day = i.attenDate.Day;

                            TeacherAttendance t = new TeacherAttendance();
                            foreach (var te in getAttendance)
                            {
                                if (Day == te.attenDate.Day)
                                {
                                    if (te.attenType == "IN")
                                    {
                                        t.IN = te.attenTime;
                                    }
                                    else if (te.attenType == "OUT")
                                    {
                                        t.OUT = te.attenTime;
                                    }
                                }
                            }
                            t.Date = i.attenDate;
                            tList.Add(t);
                        }
                        TempData["Teacher"] = tList;
                        //TempData["Teacher"] = getAttendance;
                        TempData["TeacherName"] = "" + GetTeacherId.pr.perName + "'s Attendance Report (" + Month + "-" + Year + ")";
                    }
                    else
                    {
                        TempData["No"] = "No Record found!";
                    }
                }
                else
                {
                    TempData["No"] = "Invalid Code please Enter correct code!";
                }
                return PartialView("_RptAttenTeacher");
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        public void PopulateCampus()
        {
            //Populating the dropdown for Campus
            SelectList sl = new SelectList(con.camp.ToList(), "camId", "campusname");
            ViewData["Campus"] = sl;
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
        public void PopulatClass()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.cls.ToList(), "classId", "classname");
            ViewData["Class"] = sl;
        }
        public void PopulatFeetype()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.feetype.ToList(), "feeTypeId", "feeTypeName");
            ViewData["FeeType"] = sl;
        }
        public void PopulatExtraFeetype()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.Efeetype.Where(e => e.IsDeleted == false).ToList(), "eftId", "eftName");
            ViewData["FeeType"] = sl;
        }
        public void PopulatExamType()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.examtype.ToList(), "etId", "etname");
            ViewData["ExamType"] = sl;
        }
        public void PopulatSubJect()
        {
            //Populating the dropdown for Session
            List<Subject> SubjectList = new List<Subject>();
            var GetList = con.sub.Where(s => s.isVisible == true).ToList();
            if (GetList.Count != 0)
            {
                foreach (var i in GetList)
                {
                    Subject sub = new Subject();
                    var chkSub = SubjectList.Where(s => s.subId == i.subId).FirstOrDefault();
                    if (chkSub == null)
                    {
                        sub.subId = i.subId;
                        sub.subName = i.subName + "/" + i.subCode;
                        SubjectList.Add(sub);
                    }
                }
                SelectList sl = new SelectList(SubjectList, "subId", "subName");
                ViewData["SubJect"] = sl;
            }
        }

        public void PopulatShortCourse()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.scs.ToList(), "scsId", "scsName");
            ViewData["Course"] = sl;
        }
    }
}