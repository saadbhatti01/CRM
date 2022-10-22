
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class PanelController : Controller
    {
        DBCon con = new DBCon();
        // GET: Panel
        [CheckSession]
        public ActionResult StdPaidFee()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 3)
                {
                    int Id = Convert.ToInt32(Session["ID"]);
                    var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
                    if (getperId != null)
                    {
                        var getStdId = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();


                        if (getStdId != null)
                        {
                            //Get Paid Fee Logic//
                            var getStdTotalPaidFee = con.stdfee.Where(s => s.stdId == getStdId.stdId && s.classId == getStdId.classId && s.sesId == getStdId.sesId && s.secId == getStdId.secId).ToList();
                            if (getStdTotalPaidFee.Count != 0)
                            {
                                var TotalFee = getStdTotalPaidFee.Sum(s => s.feeAmount);
                                TempData["Fee"] = TotalFee;
                                TempData["Name"] = getStdId.pr.perName;
                            }
                            else
                            {
                                TempData["Fee"] = 0;
                            }
                            //// End Paid Fee Logic/////
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }
        [CheckSession]
        public ActionResult StdFeePkg()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 3)
                {
                    int Id = Convert.ToInt32(Session["ID"]);
                    var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
                    if (getperId != null)
                    {
                        var getStdId = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();


                        if (getStdId != null)
                        {
                            //Fee Package Logic//
                            var GetStdFeePkg = con.stdfpkg.Where(c => c.classId == getStdId.classId && c.sfpstdId == getStdId.stdId && c.sesId == getStdId.sesId).ToList();
                            if (GetStdFeePkg.Count != 0)
                            {
                                TempData["StdFeePackage"] = GetStdFeePkg;
                                TempData["Name"] = getStdId.pr.perName;
                            }
                            else
                            {
                                var GetFeePkg = con.clfpkg.Where(c => c.classId == getStdId.classId && c.sesId == getStdId.sesId).ToList();
                                if (GetFeePkg.Count != 0)
                                {
                                    TempData["ClassFeePackage"] = GetFeePkg;
                                    TempData["Name"] = getStdId.pr.perName;
                                }

                            }
                            ////End Fee Package Logic/////  
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }
        [CheckSession]
        public ActionResult StdUpcomingFee()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 3)
                {
                    //Upcomming Fee Logic//
                    TempData["UpcomingFee"] = 0;
                    ////End Upcomming Fee////
                }
                else
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();

        }
        [CheckSession]
        public ActionResult StdResult()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 3)
                {
                    PopulatExamType();
                    PopulateStdSes();
                }
                else
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }
        public ActionResult StdAllResult()
        {
            try
            {
                int roleId = Convert.ToInt32(Session["RoleId"]);
                if (roleId == 3)
                {

                }
                else
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Dashboard", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }
        [CheckSession]
        public ActionResult ParentChildDetail(int id)
        {
            try
            {
                var getStdId = con.std.Where(s => s.stdId == id).FirstOrDefault();
                if (getStdId != null)
                {
                    TempData["Name"] = getStdId.pr.perName;
                    TempData["Class"] = getStdId.cls.classname;
                    TempData["Session"] = getStdId.ses.sesName;
                    TempData["Image"] = getStdId.pr.perImage;
                    //Fee Package Logic//
                    var GetStdFeePkg = con.stdfpkg.Where(c => c.classId == getStdId.classId && c.sfpstdId == getStdId.stdId && c.sesId == getStdId.sesId).ToList();
                    if (GetStdFeePkg.Count != 0)
                    {
                        TempData["FeePackage"] = GetStdFeePkg;
                    }
                    else
                    {
                        var GetFeePkg = con.clfpkg.Where(c => c.classId == getStdId.classId && c.sesId == getStdId.sesId).ToList();
                        TempData["FeePackage"] = GetFeePkg;
                    }
                    ////End Fee Package Logic/////  

                    //Get Paid Fee Logic//
                    var getStdTotalFee = con.stdfee.Where(s => s.stdId == getStdId.stdId && s.classId == getStdId.classId && s.sesId == getStdId.sesId).ToList();
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


                    //Child Name///

                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
        }
        [CheckSession]
        public ActionResult ParentFeePkg()
        {
            try
            {
                int LoginId = LoginInfo.UserID;
                var GetRelation = con.rel.Where(r => r.id == LoginId).ToList();
                if (GetRelation.Count != 0)
                {
                    List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                    List<StdFeeDetail> StdList1 = new List<StdFeeDetail>();
                    foreach (var i in GetRelation)
                    {


                        var GetStd = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                        if (GetStd != null)
                        {
                            StdFeeDetail sd = new StdFeeDetail();
                            sd.stdId = GetStd.stdId;
                            sd.Stdname = GetStd.pr.perName;
                            var GetStdFeePkg = con.stdfpkg.Where(s => s.sfpstdId == GetStd.stdId && s.classId == GetStd.classId && s.sesId == GetStd.sesId).ToList();
                            if (GetStdFeePkg.Count != 0)
                            {
                                foreach (var s in GetStdFeePkg)
                                {
                                    StdFeeDetail sf = new StdFeeDetail();
                                    sf.Stdname = GetStd.pr.perName;
                                    sf.stdId = GetStd.stdId;
                                    sf.Fee = s.ft.feeTypeName;
                                    sf.FeeAmt = s.sfpAmt;
                                    sf.Dis = s.sfpDis;
                                    StdList.Add(sf);
                                }
                            }
                            StdList1.Add(sd);
                        }
                    }
                    TempData["FeePackage"] = StdList;
                    TempData["StdId"] = StdList1;
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }

        [CheckSession]
        public ActionResult ParentPaidFee()
        {
            try
            {
                int LoginId = LoginInfo.UserID;
                var GetRelation = con.rel.Where(r => r.id == LoginId).ToList();
                if (GetRelation.Count != 0)
                {
                    List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                    List<StdFeeDetail> StdList1 = new List<StdFeeDetail>();
                    List<StdFeeDetail> StdDetailList = new List<StdFeeDetail>();
                    foreach (var i in GetRelation)
                    {
                        var GetStd = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                        if (GetStd != null)
                        {
                            double TotalFee = 0;
                            StdFeeDetail sd = new StdFeeDetail();
                            sd.stdId = GetStd.stdId;
                            sd.Stdname = GetStd.pr.perName;
                            var GetStdPaidFee = con.stdfee.Where(s => s.stdId == GetStd.stdId && s.classId == GetStd.classId && s.sesId == GetStd.sesId).ToList();
                            if (GetStdPaidFee.Count != 0)
                            {
                                StdFeeDetail sf = new StdFeeDetail();
                                sf.Stdname = GetStd.pr.perName;
                                sf.stdId = GetStd.stdId;
                                foreach (var s in GetStdPaidFee)
                                {
                                    TotalFee = TotalFee + s.feeAmount;
                                    sf.FeeAmt = TotalFee;
                                }
                                StdList.Add(sf);
                                foreach (var s in GetStdPaidFee)
                                {
                                    StdFeeDetail st = new StdFeeDetail();
                                    st.Stdname = GetStd.pr.perName;
                                    st.stdId = GetStd.stdId;
                                    st.FeeName = s.ft.feeTypeName;
                                    st.PaidAmt = s.feeAmount;
                                    st.Panding = s.PandingAmount;
                                    if (st.Panding != 0)
                                    {
                                        ViewBag.Pending = "Fill";
                                    }
                                    st.FeeDate = s.paidDate;
                                    st.FeeStatus = s.feeStatus;
                                    TotalFee = TotalFee + s.feeAmount;
                                    st.FeeAmt = TotalFee;
                                    StdDetailList.Add(st);
                                }

                            }
                            StdList1.Add(sd);
                        }
                    }
                    TempData["Fee"] = StdList;
                    TempData["DetailFee"] = StdDetailList;
                    TempData["StdId"] = StdList1;
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }

        [CheckSession]
        public ActionResult ParentViewResult()
        {
            try
            {
                PopulatExamType();
                PopulatSes();
                int LoginId = LoginInfo.UserID;
                var GetRelation = con.rel.Where(r => r.id == LoginId).ToList();
                if (GetRelation.Count != 0)
                {
                    List<StdFeeDetail> StdList = new List<StdFeeDetail>();

                    foreach (var i in GetRelation)
                    {
                        var GetStd = con.std.Where(s => s.perId == i.perId).FirstOrDefault();
                        if (GetStd != null)
                        {
                            StdFeeDetail sd = new StdFeeDetail();
                            sd.stdId = GetStd.stdId;
                            sd.Stdname = GetStd.pr.perName;
                            StdList.Add(sd);

                        }
                    }
                    TempData["StdId"] = StdList;
                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult ParentViewResult(int SesId, int EtId, int StdId)
        {
            try
            {
                PopulatExamType();
                PopulatSes();
                var StdData = con.std.Where(s => s.stdId == StdId).FirstOrDefault();
                if (StdData != null)
                {
                    var GetDetails = con.stdObtmark.Where(s => s.sesId == SesId && s.classId == StdData.classId && s.stdId == StdData.stdId && s.etId == EtId).ToList();
                    if (GetDetails.Count != 0)
                    {
                        TempData["StdMarks"] = GetDetails;
                        ViewBag.Name = StdData.pr.perName;
                        ViewBag.StdId = StdData.stdId;
                        ViewBag.Ses = StdData.sesId;
                        ViewBag.Sec = StdData.secId;
                        ViewBag.Class = StdData.classId;
                        return View("ViewResult", TempData["StdMarks"]);
                    }
                    else
                    {
                        ViewBag.Name = StdData.pr.perName;
                        TempData["Empty"] = "No Record found";
                        return View("ViewResult", TempData["Empty"]);
                    }

                }

                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
        }

        [CheckSession]
        public ActionResult ViewResult()
        {
            return View();
        }
        [CheckSession]
        public ActionResult _GetStdResult(int SesId, int EtId, string StdId)
        {
            try
            {

                int Id = Convert.ToInt32(Session["ID"]);
                var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
                if (getperId != null)
                {
                    var StdData = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();
                    if (StdData != null)
                    {
                        var GetDetails = con.stdObtmark.Where(s => s.sesId == SesId && s.classId == StdData.classId && s.stdId == StdData.stdId && s.etId == EtId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            TempData["StdMarks"] = GetDetails;
                            ViewBag.StdId = StdData.stdId;
                            ViewBag.Ses = StdData.sesId;
                            ViewBag.Sec = StdData.secId;
                            ViewBag.Class = StdData.classId;
                            return PartialView(TempData["StdMarks"]);
                        }
                        else
                        {
                            TempData["No"] = "No Record found";
                        }
                    }
                }
                return PartialView(TempData["StdMarks"]);

            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error";
                return PartialView(TempData["StdMarks"]);
            }

        }

        [CheckSession]

        public ActionResult MyDocuments()
        {
            try
            {
                PopulateStdSes();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to your soft support";
                return View();
            }
        }
        [CheckSession]
        public ActionResult _MyDoc(int SesId)
        {
            int Id = Convert.ToInt32(Session["ID"]);
            var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
            if (getperId != null)
            {
                var StdData = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();
                if (StdData != null)
                {
                    var getDocs = con.documents.Where(d => d.sesId == SesId && d.classId == StdData.classId
                    && d.secId == StdData.secId && d.isVisible == true && d.ExpiryDate > DateTime.Now).ToList();
                    if (getDocs.Count != 0)
                    {
                        TempData["Docs"] = getDocs;
                        return PartialView("_MyDoc");
                    }
                    else
                    {
                        TempData["No"] = "No Record found";
                    }
                }
            }
            return PartialView("_MyDoc");
        }

        [CheckSession]
        public ActionResult LogInDetail()
        {
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult LogInDetail(string RollNo)
        {
            try
            {
                if (RollNo != "" && RollNo != null)
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        //For Correct Notification
                        ViewBag.Post = "Post";
                        var getLogin = (from p in con.person
                                        join s in con.std on p.perId equals s.perId
                                        join l in con.login on p.id equals l.id
                                        where p.perId == getStd.perId
                                        select new
                                        {
                                            p.perName,
                                            p.perImage,
                                            l.usrLogin,
                                            l.usrPassword
                                        }).FirstOrDefault();
                        if (getLogin != null)
                        {
                            StdFeeDetail  Std = new StdFeeDetail();
                            Std.Stdname = getLogin.perName;
                            Std.LogIn = getLogin.usrLogin;
                            Std.Password = getLogin.usrPassword;
                            Std.Image = getLogin.perImage;
                            TempData["Std"] = Std;

                            var Parent = (from r in con.rel
                                          join p in con.person on r.id equals p.id
                                          join l in con.login on r.id equals l.id
                                          where r.perId == getStd.perId
                                          select new
                                          {
                                              p.perName,
                                              p.perCode,
                                              l.usrLogin,
                                              l.usrName,
                                              l.usrPassword,
                                              p.perImage
                                          }).FirstOrDefault();
                            if(Parent != null)
                            {
                                StdFeeDetail parent = new StdFeeDetail();
                                parent.FatherName = Parent.perName;
                                parent.FatherCode = Parent.perCode;
                                parent.LogIn = Parent.usrLogin;
                                parent.Password = Parent.usrPassword;
                                parent.Image = Parent.perImage;
                                TempData["Parent"] = parent;
                            }

                        }
                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Roll Number. Please Enter correct Roll Number";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }
            return View();
        }

        public void PopulatExamType()
        {
            //Populating the dropdown for Exam
            SelectList sl = new SelectList(con.examtype.Where(s => s.etStatus == true).ToList(), "etId", "etname");
            ViewData["ExamType"] = sl;
        }

        public void PopulateStdSes()
        {
            try
            {
                List<InstSession> SesList = new List<InstSession>();
                SelectList list = new SelectList(SesList);
                int Id = Convert.ToInt32(Session["ID"]);
                var getperId = con.person.Where(p => p.id == Id).FirstOrDefault();
                if (getperId != null)
                {
                    var getStdId = con.std.Where(s => s.perId == getperId.perId).FirstOrDefault();

                    var getSesId = con.stdObtmark.Where(s => s.stdId == getStdId.stdId).ToList();

                    if (getSesId.Count != 0)
                    {
                        foreach (var i in getSesId)
                        {
                            var getSes = con.InstSes.Where(s => s.sesId == i.sesId).FirstOrDefault();
                            InstSession ses = new InstSession();

                            var chkSes = SesList.Where(s => s.sesId == i.sesId).FirstOrDefault();
                            if (chkSes == null)
                            {
                                ses.sesId = getSes.sesId;
                                ses.sesName = getSes.sesName;
                                SesList.Add(ses);
                            }
                        }
                        //Current Student Session
                        var chkSes1 = SesList.Where(s => s.sesId == getStdId.sesId).FirstOrDefault();
                        if (chkSes1 == null)
                        {
                            InstSession ses1 = new InstSession();
                            ses1.sesId = getStdId.sesId;
                            ses1.sesName = getStdId.ses.sesName;
                            SesList.Add(ses1);
                        }

                        list = new SelectList(SesList, "sesId", "sesName");
                        ViewData["Session"] = list;
                    }

                }
                ViewData["Session"] = list;
            }
            catch (Exception ex)
            {

            }
        }

        public void PopulatSes()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.InstSes.Where(s => s.sesStatus == "Open").ToList(), "sesId", "sesName");
            ViewData["Session"] = sl;
        }
    }
}