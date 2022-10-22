
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class ExtrasController : Controller
    {
        string btn = "Show";
        DBCon con = new DBCon();
        // GET: Extras
        [CheckSession]
        public ActionResult ExtraFeeType()
        {
            TempData["EFeeType"] = con.Efeetype.Where(f => f.IsDeleted == false).ToList().OrderByDescending(p => p.eftId);
            return View(TempData["EFeeType"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ExtraFeeType(ExtraFeeType fee)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.Efeetype.Where(x => x.eftName == fee.eftName && x.IsDeleted == false).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This ExtraFeeType is already exist";
                        return RedirectToAction("ExtraFeeType");
                    }
                    else
                    {

                        fee.CreatedDate = DateTime.Now;
                        fee.CreatedBy = LoginInfo.UserID;
                        con.Efeetype.Add(fee);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Extra FeeType Added successfully";
                        RedirectToAction("ExtraFeeType");
                    }
                }
                else
                {
                    TempData["Error"] = "Extra FeeType not added";
                }
            }
            catch (Exception e)
            {

            }

            return RedirectToAction("ExtraFeeType");
        }
        [CheckSession]
        public ActionResult DelExtraFeeType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExtraFeeType s = con.Efeetype.FirstOrDefault(b => b.eftId == id);
                    if (s != null)
                    {
                        con.Efeetype.Remove(s);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Extra FeeType deleted Successfully";
                        return RedirectToAction("ExtraFeeType");
                    }
                    else
                    {
                        TempData["Error"] = "No record found";
                    }

                }
                else
                {
                    TempData["Error"] = "Extra FeeType not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "Extra FeeType cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Extra FeeType not deleted please contact to soft support";
                }
            }
            return RedirectToAction("ExtraFeeType");
        }
        [CheckSession]
        public ActionResult UpdateExtraFeeType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExtraFeeType cl = con.Efeetype.Single(b => b.eftId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateExtraFeeType(int id, ExtraFeeType cl)
        {
            try
            {
                var Oldfee = con.Efeetype.Single(b => b.eftId == id);
                if (Oldfee != null)
                {
                    var chkName = con.Efeetype.Where(c => c.eftName == cl.eftName && c.eftId != Oldfee.eftId).Any();
                    if (chkName == false)
                    {
                        Oldfee.eftName = cl.eftName;
                        Oldfee.eftStatus = cl.eftStatus;
                        Oldfee.UpdatedDate = DateTime.Now;
                        Oldfee.UpdatedBy = LoginInfo.UserID;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Extra FeeType Updated Successfully";
                        return RedirectToAction("ExtraFeeType");
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
                TempData["Error"] = "Extra FeeType not Updated";

            }
            return RedirectToAction("ExtraFeeType");
        }
        [CheckSession]
        public ActionResult StdExtraFeeType(string RollNo, string btn)
        {
            try
            {
                List<ExtraFeeType> ExtraFee = new List<ExtraFeeType>();
                if (RollNo != null && RollNo != "")
                {
                    if (btn == "Show")
                    {
                        var StdRoll = RollNo;
                        var StdData = (from s in con.std
                                       join p in con.person on s.perId equals p.perId
                                       join ss in con.InstSes on s.sesId equals ss.sesId
                                       join sc in con.InstSec on s.secId equals sc.secId
                                       join cl in con.cls on s.classId equals cl.classId
                                       where s.stdRollNo == StdRoll && s.IsDeleted == false
                                       select new
                                       {
                                           s.stdId,
                                           s.classId,
                                           s.sesId,
                                           s.secId,
                                           s.stdRollNo,
                                           p.perName,
                                           ss.sesName,
                                           sc.secName,
                                           cl.classname
                                       }).FirstOrDefault();
                        if (StdData != null)
                        {

                            ViewBag.StdRoll = StdData.stdRollNo;
                            ViewBag.StdName = StdData.perName;
                            ViewBag.StdClass = StdData.classname;
                            ViewBag.StdSec = StdData.secName;
                            ViewBag.StdSes = StdData.sesName;
                            /// Populate Fee Extra type ////
                            PopulatExtraFeetype();
                            /// End Populate Extra Fee type ////

                            var FeeDtl = (from s in con.Efeetype
                                          where s.eftStatus == true && s.IsDeleted == false
                                          select new
                                          {
                                              s.eftName
                                          }).ToList();
                            foreach (var i in FeeDtl)
                            {
                                ExtraFeeType fee = new Models.ExtraFeeType();
                                fee.eftName = i.eftName;
                                ExtraFee.Add(fee);
                            }
                            ViewData["FeeDetail"] = ExtraFee;

                            var GetStdExtraFee = con.StudentExtraFee.Where(s => s.stdId == StdData.stdId && s.classId == StdData.classId && s.sesId == StdData.sesId).OrderByDescending(e => e.eftId).ToList();
                            if (GetStdExtraFee.Count != 0)
                            {
                                ViewData["ExtraFeeDetail"] = GetStdExtraFee;
                            }

                            return View();

                        }
                        else
                        {
                            TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                            return RedirectToAction("StdExtraFeeType");
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult StdExtraFeeType(StudentExtraFee sd, string submitButton, string StdRoll,
            string RollNo)
        {
            List<ExtraFeeType> ExtraFee = new List<ExtraFeeType>();
            try
            {
                if (submitButton == "Show")
                {
                    var StdData = (from s in con.std
                                   join p in con.person on s.perId equals p.perId
                                   join ss in con.InstSes on s.sesId equals ss.sesId
                                   join sc in con.InstSec on s.secId equals sc.secId
                                   join cl in con.cls on s.classId equals cl.classId
                                   where s.stdRollNo == StdRoll && s.IsDeleted == false
                                   select new
                                   {
                                       s.stdId,
                                       s.classId,
                                       s.sesId,
                                       s.secId,
                                       s.stdRollNo,
                                       p.perName,
                                       ss.sesName,
                                       sc.secName,
                                       cl.classname
                                   }).FirstOrDefault();
                    if (StdData != null)
                    {

                        ViewBag.StdRoll = StdData.stdRollNo;
                        ViewBag.StdName = StdData.perName;
                        ViewBag.StdClass = StdData.classname;
                        ViewBag.StdSec = StdData.secName;
                        ViewBag.StdSes = StdData.sesName;
                        /// Populate Fee Extra type ////
                        PopulatExtraFeetype();
                        /// End Populate Extra Fee type ////

                        var FeeDtl = (from s in con.Efeetype
                                      where s.eftStatus == true && s.IsDeleted == false
                                      select new
                                      {
                                          s.eftName
                                      }).ToList();
                        foreach (var i in FeeDtl)
                        {
                            ExtraFeeType fee = new Models.ExtraFeeType();
                            fee.eftName = i.eftName;
                            ExtraFee.Add(fee);
                        }
                        ViewData["FeeDetail"] = ExtraFee;

                        var GetStdExtraFee = con.StudentExtraFee.Where(s => s.stdId == StdData.stdId && s.classId == StdData.classId && s.sesId == StdData.sesId).OrderByDescending(e => e.eftId).ToList();
                        if (GetStdExtraFee.Count != 0)
                        {
                            ViewData["ExtraFeeDetail"] = GetStdExtraFee;
                        }

                        return View();

                    }
                    else
                    {
                        TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                        return RedirectToAction("StdExtraFeeType");
                    }
                }
                else
                {
                    StudentExtraFee AdFee = new StudentExtraFee();
                    var GetStdDtl = (from s in con.std
                                     join p in con.person on s.perId equals p.perId
                                     join ss in con.InstSes on s.sesId equals ss.sesId
                                     join sc in con.InstSec on s.secId equals sc.secId
                                     join cl in con.cls on s.classId equals cl.classId
                                     where s.stdRollNo == RollNo && s.IsDeleted == false
                                     select new
                                     {
                                         s.stdId,
                                         ss.sesId,
                                         sc.secId,
                                         cl.classId,
                                     }).FirstOrDefault();
                    var chkfee = (from s in con.std
                                  join se in con.StudentExtraFee on s.stdId equals se.stdId
                                  where se.eftId == sd.eftId && s.stdRollNo == RollNo && s.IsDeleted == false
                                  select new
                                  {
                                      se.sefId
                                  }).FirstOrDefault();
                    if (chkfee != null)
                    {
                        TempData["Error"] = "This Fees is already paid against this roll Number  '" + RollNo + "'";
                        return RedirectToAction("StdExtraFeeType", new { RollNo, btn });
                    }
                    else
                    {
                        if (GetStdDtl != null)
                        {
                            if (sd.Remarks == null)
                            {
                                sd.Remarks = "Fee Received";
                                AdFee.Remarks = sd.Remarks;
                                AdFee.secId = GetStdDtl.secId;
                                AdFee.sesId = GetStdDtl.sesId;
                                AdFee.classId = GetStdDtl.classId;
                                AdFee.stdId = GetStdDtl.stdId;
                                AdFee.eftId = sd.eftId;
                                AdFee.Remarks = sd.Remarks;
                                AdFee.eDate = sd.eDate;
                                AdFee.Amount = sd.Amount;
                                AdFee.CreatedBy = LoginInfo.UserID;
                                AdFee.CreatedDate = DateTime.Now;
                                AdFee.UpdatedBy = 0;
                                AdFee.IsDeleted = false;
                                AdFee.DeletedBy = 0;
                                con.StudentExtraFee.Add(AdFee);
                                con.SaveChanges();
                            }
                            else
                            {
                                AdFee.Remarks = sd.Remarks;
                                AdFee.secId = GetStdDtl.secId;
                                AdFee.sesId = GetStdDtl.sesId;
                                AdFee.classId = GetStdDtl.classId;
                                AdFee.stdId = GetStdDtl.stdId;
                                AdFee.eftId = sd.eftId;
                                AdFee.Remarks = sd.Remarks;
                                AdFee.eDate = sd.eDate;
                                AdFee.Amount = sd.Amount;
                                AdFee.CreatedBy = LoginInfo.UserID;
                                AdFee.CreatedDate = DateTime.Now;
                                AdFee.UpdatedBy = 0;
                                AdFee.IsDeleted = false;
                                AdFee.DeletedBy = 0;
                                con.StudentExtraFee.Add(AdFee);
                                con.SaveChanges();
                            }
                            TempData["SuccessMessage"] = "Student Extra Fee received successfully";
                            return RedirectToAction("StdExtraFeeType", new { RollNo, btn });
                        }
                        else
                        {
                            TempData["Error"] = "No record Found against this roll Number  '" + RollNo + "' and Fee type";
                            return RedirectToAction("StdExtraFeeType", new { RollNo, btn });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "' Please contact to soft support";
            }
            return View();
        }
        [CheckSession]
        public ActionResult BulkStdExtraFeeType()
        {
            PopulatExtraFeetype();
            PopulatClass();
            PopulatSec();
            PopulatSes();
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult BulkStdExtraFeeType(StudentExtraFee sfd)
        {
            try
            {
                PopulatExtraFeetype();
                PopulatClass();
                PopulatSec();
                PopulatSes();
                var StdId = (from sa in con.std
                             where !con.StudentExtraFee
                                      .Any(o => o.stdId == sa.stdId && o.eftId == sfd.eftId)
                             where sa.classId == sfd.classId && sa.sesId == sfd.sesId && sa.secId == sfd.secId && sa.IsDeleted == false
                             select new
                             {
                                 sa.stdId
                             }).ToList();

                if (StdId.Count == 0)
                {
                    TempData["Error"] = "No record found or Student Fee already paid against this fee type";
                    return View(TempData["FeeDetail"]);
                }
                else
                {
                    foreach (var i in StdId)
                    {
                        if (sfd.Remarks == null)
                        {
                            sfd.stdId = i.stdId;
                            sfd.Remarks = "Fee Received";
                            sfd.CreatedBy = LoginInfo.UserID;
                            sfd.CreatedDate = DateTime.Now;
                            sfd.UpdatedBy = 0;
                            sfd.IsDeleted = false;
                            sfd.DeletedBy = 0;
                            con.StudentExtraFee.Add(sfd);
                            con.SaveChanges();
                            TempData["Success"] = "Student Fee Extra received successfully";
                        }
                        else
                        {
                            sfd.stdId = i.stdId;
                            sfd.CreatedBy = LoginInfo.UserID;
                            sfd.CreatedDate = DateTime.Now;
                            sfd.UpdatedBy = 0;
                            sfd.IsDeleted = false;
                            sfd.DeletedBy = 0;
                            con.StudentExtraFee.Add(sfd);
                            con.SaveChanges();
                            TempData["Success"] = "Student Fee Extra received successfully";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
            }
            return RedirectToAction("BulkStdExtraFeeType");
        }

        public ActionResult ExtraFeeReport()
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            PopulatExtraFeetype();
            return View();
        }

        public ActionResult _GetExtraFeeStatus(int SesId, int SecId, int ClassId, int fType, string Status)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                if (Status == "Paid")
                {
                    var GetDetails = con.StudentExtraFee.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.eftId == fType).ToList();
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
                            sd.Fee = i.eft.eftName;
                            sd.FeeAmt = i.Amount;
                            sd.FeePaidStatus = "Paid";
                            sd.FeeDate = i.eDate;
                            sList.Add(sd);
                        }
                        //TempData["StdList"] = GetStdList;
                        TempData["Fee"] = sList;
                    }
                    else
                    {
                        TempData["No"] = "No Record Found";
                    }
                }
                else if (Status == "UnPaid")
                {
                    var GetStdId = (from s in con.std
                                    where s.sesId == SesId && s.secId == SecId && s.classId == ClassId &&
                                    !(from sf in con.StudentExtraFee
                                      where s.stdId == sf.stdId && sf.sesId == SesId && sf.secId == SecId && sf.classId == ClassId && sf.eftId == fType
                                      select sf.stdId).Contains(s.stdId)
                                    select new
                                    {
                                        s.stdId,
                                        s.stdRollNo,
                                    }).ToList();

                    if (GetStdId.Count != 0)
                    {
                        foreach (var i in GetStdId)
                        {
                            var GetStdDetails = con.std.Where(s => s.stdId == i.stdId && s.sesId == SesId && s.secId == SecId && s.classId == ClassId).FirstOrDefault();
                            var GetFee = con.Efeetype.Where(f => f.eftId == fType).FirstOrDefault();
                            StdFeeDetail sd = new StdFeeDetail();
                            sd.Stdname = GetStdDetails.pr.perName;
                            sd.RollNo = GetStdDetails.stdRollNo;
                            sd.Ses = GetStdDetails.ses.sesName;
                            sd.Sec = GetStdDetails.sec.secName;
                            sd.Class = GetStdDetails.cls.classname;
                            sd.Fee = GetFee.eftName;
                            sd.FeeAmt = 0000;
                            sd.FeePaidStatus = "UnPaid";
                            sList.Add(sd);
                            TempData["Fee"] = sList;
                        }
                    }
                    else
                    {
                        TempData["No"] = "No Record Found";
                    }


                }

                return PartialView(TempData["Fee"]);
            }
            catch (Exception ex)
            {

            }
            return PartialView(TempData["Fee"]);
        }
        public void PopulatExtraFeetype()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.Efeetype.Where(f => f.eftStatus == true && f.IsDeleted == false).ToList(), "eftId", "eftName");
            ViewData["EFeeType"] = sl;
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
        public void PopulatClass()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.cls.ToList(), "classId", "classname");
            ViewData["Class"] = sl;
        }

    }
}