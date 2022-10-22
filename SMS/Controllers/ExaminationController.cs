using Newtonsoft.Json;

using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class ExaminationController : Controller
    {
        DBCon con = new DBCon();
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
                    return View();
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
                            return View();
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
                return View();
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
                    getData.Marks = ps.Marks;
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Passing Marks updated successfully";
                    return RedirectToAction("AddPassingMarks");
                }
                return View(getData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Passing Marks not Entered.";
                return View();
            }
        }


        // GET: Examination
        [CheckSession]
        public ActionResult ExamType()
        {
            TempData["ExamType"] = con.examtype.Where(e => e.IsDeleted == false).ToList().OrderByDescending(p => p.etId);
            return View(TempData["ExamType"]);
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ExamType(ExamType e)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Chk = con.examtype.Where(x => x.etname == e.etname && x.IsDeleted == false).FirstOrDefault();
                    if (Chk != null)
                    {
                        TempData["Error"] = "This ExamType name is already exist";
                        return RedirectToAction("ExamType");
                    }
                    else
                    {
                        e.CreatedBy = LoginInfo.UserID;
                        e.CreatedDate = DateTime.Now;
                        e.UpdatedBy = 0;
                        con.examtype.Add(e);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "ExamType Added successfully";
                        RedirectToAction("ExamType");

                    }
                }
                else
                {
                    TempData["Error"] = "ExamType not added";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
            }

            return RedirectToAction("ExamType");
        }
        [CheckSession]
        public ActionResult DelExamType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExamType s = con.examtype.Single(b => b.etId == id);
                    con.examtype.Remove(s);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "ExamType deleted Successfully";
                    return RedirectToAction("ExamType");
                }
                else
                {
                    TempData["Error"] = "ExamType not deleted";
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "ExamType cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "ExamType not deleted please contact to soft support";
                }
            }
            return RedirectToAction("ExamType");
        }
        [CheckSession]
        public ActionResult UpdateExamType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ExamType cl = con.examtype.Single(b => b.etId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult UpdateExamType(int id, ExamType cl)
        {
            try
            {
                var Oldsub = con.examtype.FirstOrDefault(b => b.etId == id);
                if (Oldsub != null)
                {
                    var chkName = con.examtype.Where(c => c.etname == cl.etname && c.etId != Oldsub.etId && c.IsDeleted == false).Any();
                    if (chkName == false)
                    {
                        Oldsub.etname = cl.etname;
                        Oldsub.etStatus = cl.etStatus;
                        Oldsub.UpdatedDate = DateTime.Now;
                        Oldsub.UpdatedBy = LoginInfo.UserID;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "ExamType Updated Successfully";
                        return RedirectToAction("ExamType");
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
                TempData["Error"] = "ExamType not Updated";

            }
            return RedirectToAction("ExamType");
        }
        [CheckSession]
        public ActionResult SubjectMarkType()
        {
            TempData["SubJectMarkType"] = con.submark.Where(s => s.IsDeleted == false).ToList();
            return View(TempData["SubJectMarkType"]);
        }
        [CheckSession]
        public ActionResult UpdateSubjectMarkType(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    SubMarkType cl = con.submark.Single(b => b.smtId == id);
                    return View(cl);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult UpdateSubjectMarkType(int id, SubMarkType cl)
        {
            try
            {
                var Oldsub = con.submark.Single(b => b.smtId == id);
                Oldsub.smtName = cl.smtName;
                Oldsub.smtStatus = cl.smtStatus;
                Oldsub.UpdatedDate = DateTime.Now;
                Oldsub.UpdatedBy = LoginInfo.UserID;
                con.SaveChanges();
                TempData["SuccessMessage"] = "ExamType Updated Successfully";
                return RedirectToAction("SubjectMarkType");


            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                TempData["Error"] = "ExamType not Updated";

            }
            return RedirectToAction("SubjectMarkType");
        }
        [CheckSession]
        public ActionResult StdSingleObt(string RollNo)
        {
            try
            {
                List<StdObtDetail> SubList = new List<StdObtDetail>();
                PopulatExamType();
                PopulatSubJect();
                if (RollNo != "" && RollNo != null)
                {
                    var StdData = (from s in con.std
                                   join p in con.person on s.perId equals p.perId
                                   join ss in con.InstSes on s.sesId equals ss.sesId
                                   join sc in con.InstSec on s.secId equals sc.secId
                                   join cl in con.cls on s.classId equals cl.classId
                                   where s.stdRollNo == RollNo && s.IsDeleted == false && s.stdStatus == "Active"
                                   select new
                                   {
                                       s.stdId,
                                       s.stdRollNo,
                                       p.perName,
                                       ss.sesId,
                                       ss.sesName,
                                       sc.secId,
                                       sc.secName,
                                       cl.classId,
                                       cl.classname,
                                   }).FirstOrDefault();
                    if (StdData == null)
                    {
                        TempData["Error"] = "No record found";
                        return View();
                    }
                    else
                    {
                        //Find if Student is related to current Teacher?
                        int id = Convert.ToInt32(Session["ID"]);
                        var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
                        if (getPerId != null)
                        {
                            var chkStudent = con.teachers.Where(t => t.classId == StdData.classId && t.secId == StdData.secId).Any();
                            if (chkStudent == true)
                            {
                                ViewBag.StdRoll = StdData.stdRollNo;
                                ViewBag.StdName = StdData.perName;
                                ViewBag.StdClass = StdData.classname;
                                ViewBag.StdSec = StdData.secName;
                                ViewBag.StdSes = StdData.sesName;
                                PopulatExamType();
                                PopulatSubJect();
                                var SubName = (from sb in con.submark
                                               select new
                                               {
                                                   sb.smtId,
                                                   sb.smtName
                                               }).ToList();
                                foreach (var i in SubName)
                                {
                                    StdObtDetail sub = new StdObtDetail();
                                    sub.sm1 = i.smtName;
                                    SubList.Add(sub);
                                }
                                TempData["SUbName"] = SubList;
                                var GetStdMarks = con.stdObtmark.Where(s => s.stdId == StdData.stdId && s.sesId == StdData.sesId && s.secId == StdData.secId && s.classId == StdData.classId).OrderByDescending(s => s.somID).ToList();
                                if (GetStdMarks.Count != 0)
                                {
                                    ViewBag.StdId = StdData.stdId;
                                    ViewBag.Ses = StdData.sesId;
                                    ViewBag.Sec = StdData.secId;
                                    ViewBag.Class = StdData.classId;
                                    TempData["StdMarks"] = GetStdMarks;
                                }

                                return View();
                            }
                            else
                            {
                                TempData["Info"] = "This Student is not belongs to You";
                                return View();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please contact to soft support";
            }

            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult StdSingleObt(StdObtDetail st, string StdRoll, string submitButton, string RollNo, string OutOf)
        {
            PopulatExamType();
            PopulatSubJect();
            List<StdObtDetail> StdList = new List<StdObtDetail>();
            List<StdObtDetail> SubList = new List<StdObtDetail>();
            try
            {
                if (submitButton == "Show")
                {
                    var StdData = (from s in con.std
                                   join p in con.person on s.perId equals p.perId
                                   join ss in con.InstSes on s.sesId equals ss.sesId
                                   join sc in con.InstSec on s.secId equals sc.secId
                                   join cl in con.cls on s.classId equals cl.classId
                                   where s.stdRollNo == StdRoll && s.IsDeleted == false && s.stdStatus == "Active"
                                   select new
                                   {
                                       s.stdId,
                                       s.stdRollNo,
                                       p.perName,
                                       ss.sesId,
                                       ss.sesName,
                                       sc.secId,
                                       sc.secName,
                                       cl.classId,
                                       cl.classname,
                                   }).FirstOrDefault();
                    if (StdData == null)
                    {
                        TempData["Error"] = "No record found";
                        return View();
                    }
                    else
                    {

                        ViewBag.StdRoll = StdData.stdRollNo;
                        ViewBag.StdName = StdData.perName;
                        ViewBag.StdClass = StdData.classname;
                        ViewBag.StdSec = StdData.secName;
                        ViewBag.StdSes = StdData.sesName;
                        PopulatExamType();
                        PopulatSubJect();
                        var SubName = (from sb in con.submark
                                       select new
                                       {
                                           sb.smtId,
                                           sb.smtName
                                       }).ToList();
                        foreach (var i in SubName)
                        {
                            StdObtDetail sub = new StdObtDetail();
                            sub.sm1 = i.smtName;
                            SubList.Add(sub);
                        }
                        TempData["SUbName"] = SubList;
                        var GetStdMarks = con.stdObtmark.Where(s => s.stdId == StdData.stdId && s.sesId == StdData.sesId && s.secId == StdData.secId && s.classId == StdData.classId).OrderByDescending(s => s.somID).ToList();
                        if (GetStdMarks.Count != 0)
                        {
                            ViewBag.StdId = StdData.stdId;
                            ViewBag.Ses = StdData.sesId;
                            ViewBag.Sec = StdData.secId;
                            ViewBag.Class = StdData.classId;
                            TempData["StdMarks"] = GetStdMarks;
                        }

                        return View();
                    }
                }
                else
                {
                    StdObtainMarks sm = new StdObtainMarks();
                    var GetDtl = (from s in con.std
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
                    if (GetDtl == null)
                    {
                        TempData["Error"] = "No record found";
                        return View();
                    }
                    else
                    {
                        var ChkStdMarks = con.stdObtmark.Where(s => s.stdId == GetDtl.stdId && s.etId == st.etId && s.subId == st.subId && s.sesId == GetDtl.sesId && s.classId == GetDtl.classId && s.secId == GetDtl.secId).FirstOrDefault();
                        if (ChkStdMarks != null)
                        {
                            ChkStdMarks.smt1 = st.smt1;
                            ChkStdMarks.smt2 = st.smt2;
                            ChkStdMarks.smt3 = st.smt3;
                            ChkStdMarks.subTotalMarks = st.subTotalMarks;

                            ChkStdMarks.totalObtainMarks = st.smt1 + st.smt2 + st.smt3;
                            if (ChkStdMarks.totalObtainMarks > ChkStdMarks.subTotalMarks)
                            {
                                TempData["Error"] = "Obtain Marks sould be less than or equal to Total Marks.";
                                return RedirectToAction("StdSingleObt", new { RollNo });
                            }
                            else
                            {
                                ChkStdMarks.UpdatedBy = 1;
                                ChkStdMarks.UpdatedDate = DateTime.Now;
                                con.SaveChanges();
                                TempData["SuccessMessage"] = "Student Marks Updated Successfully";
                                return RedirectToAction("StdSingleObt", new { RollNo });
                            }
                        }
                        else
                        {
                            sm.stdId = GetDtl.stdId;
                            sm.classId = GetDtl.classId;
                            sm.secId = GetDtl.secId;
                            sm.sesId = GetDtl.sesId;
                            sm.etId = st.etId;
                            sm.subId = st.subId;
                            sm.smt1 = st.smt1;
                            sm.smt2 = st.smt2;
                            sm.smt3 = st.smt3;
                            sm.subTotalMarks = st.subTotalMarks;
                            sm.totalObtainMarks = sm.smt1 + sm.smt2 + sm.smt3;
                            if (sm.totalObtainMarks > sm.subTotalMarks)
                            {
                                TempData["Error"] = "Obtain Marks sould be less than or equal to Total Marks.";
                                return RedirectToAction("StdSingleObt", new { RollNo });
                            }
                            else
                            {
                                sm.CreatedBy = LoginInfo.UserID;
                                sm.CreatedDate = DateTime.Now;
                                sm.UpdatedBy = 0;
                                sm.IsDeleted = false;
                                sm.DeletedBy = 0;
                                con.stdObtmark.Add(sm);
                                con.SaveChanges();
                                TempData["SuccessMessage"] = "Marks entered successfully";
                                return RedirectToAction("StdSingleObt", new { RollNo });
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                TempData["Error"] = "There is some error occured please try again";
            }
            return View();
        }
        [CheckSession]
        public ActionResult StdObtMark()
        {
            PopulatClass();
            PopulatExamType();
            PopulatSec();
            PopulatSes();
            PopulatSubJect();
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult StdObtMark(StdObtDetail st, string submitButton, List<StdObtDetail> StdSubList)
        {
            PopulatClass();
            PopulatExamType();
            PopulatSec();
            PopulatSes();
            PopulatSubJect();
            List<StdObtDetail> StdList = new List<StdObtDetail>();
            List<StdObtDetail> SubList = new List<StdObtDetail>();
            try
            {
                if (submitButton == "Marking")
                {
                    var getStd = con.std.Where(s => s.classId == st.classId && s.sesId == st.sesId && s.secId == st.secId && s.IsDeleted == false && s.stdStatus == "Active").ToList();
                    if (getStd.Count != 0)
                    {
                        var StdId = (from sa in con.std
                                     where !con.stdObtmark
                                              .Any(o => o.stdId == sa.stdId && o.etId == st.etId && o.subId == st.subId && o.sesId == sa.sesId && o.classId == sa.classId && o.secId == sa.secId)
                                     where sa.classId == st.classId && sa.sesId == st.sesId && sa.secId == st.secId && sa.IsDeleted == false

                                     select new
                                     {
                                         sa.stdId,
                                         sa.classId,
                                         sa.sesId,
                                         sa.secId,
                                         sa.stdRollNo,
                                         sa.pr.perName,
                                     }).ToList();

                        if (StdId.Count == 0)
                        {
                            TempData["Info"] = "All Students marks entered in the system.";
                            return View();
                        }
                        else
                        {
                            foreach (var i in StdId)
                            {
                                StdObtDetail std = new StdObtDetail();
                                std.stdId = i.stdId;
                                std.classId = i.classId;
                                std.secId = i.secId;
                                std.sesId = i.sesId;
                                std.stdRollNo = i.stdRollNo;
                                std.stdName = i.perName;
                                std.etId = st.etId;
                                std.subId = st.subId;
                                std.subTotalMarks = st.subTotalMarks;
                                StdList.Add(std);
                            }
                            TempData["StdObtMark"] = StdList;
                            var SubName = (from sb in con.submark
                                           select new
                                           {
                                               sb.smtId,
                                               sb.smtName
                                           }).ToList();
                            foreach (var i in SubName)
                            {
                                StdObtDetail sub = new StdObtDetail();
                                sub.sm1 = i.smtName;
                                SubList.Add(sub);
                            }
                            TempData["Outof"] = st.subTotalMarks;
                            TempData["SUbName"] = SubList;
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Info"] = "No Student Record found";
                        return View();
                    }


                   
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
            }
            return View();
        }

        [HttpPost]
        [CheckSession]
        public ActionResult SubMarks(int? t, int? tss, string ts)
        {
            var ss = 0;
            if (t != null)
            {

                var StdMarks = (from s in con.stdObtmark
                                join sd in con.std on s.stdId equals sd.stdId
                                join e in con.examtype on s.etId equals e.etId
                                where s.subId == t && s.etId == tss && sd.stdRollNo == ts
                                select new
                                {
                                    s.smt1,
                                    s.smt2,
                                    s.smt3,
                                    s.subTotalMarks
                                }).FirstOrDefault();

                if (StdMarks != null)
                {
                    var s1 = StdMarks.smt1;
                    var s2 = StdMarks.smt2;
                    var s3 = StdMarks.smt3;
                    var Total = StdMarks.subTotalMarks;

                    return Json(new { Success = "true", Data = new { s1, s2, s3, Total, ss } });
                }
                else
                {
                    return Json(new { Success = "false", Data = new { ss } });
                }
            }
            return Json(new { Success = "false", Data = new { ss } });
        }
        [CheckSession]
        public ActionResult AddStdMarks(string Data, string AccDetails)
        {
            try
            {

                if (Data != null)
                {
                    if (AccDetails != null)
                    {
                        List<StdObtainMarks> StdList = new List<StdObtainMarks>();

                        var ObjDetail = JsonConvert.DeserializeObject<List<StdObtainMarks>>(AccDetails);
                        var ObjData = JsonConvert.DeserializeObject<StdObtainMarks>(Data);
                        foreach (var st in ObjDetail)
                        {
                            var ChkRecord = con.stdObtmark.Where(s => s.stdId == st.stdId && s.sesId == ObjData.sesId && s.secId == ObjData.secId && s.classId == ObjData.classId && s.etId == ObjData.etId && s.subId == ObjData.subId).FirstOrDefault();
                            if (ChkRecord != null)
                            {

                            }
                            else
                            {
                                StdObtainMarks sm = new StdObtainMarks();
                                sm.stdId = st.stdId;
                                sm.classId = ObjData.classId;
                                sm.secId = ObjData.secId;
                                sm.sesId = ObjData.sesId;
                                sm.etId = ObjData.etId;
                                sm.subId = ObjData.subId;
                                sm.smt1 = st.smt1;
                                sm.smt2 = st.smt2;
                                sm.smt3 = st.smt3;
                                sm.subTotalMarks = ObjData.subTotalMarks;
                                sm.totalObtainMarks = sm.smt1 + sm.smt2 + sm.smt3;
                                if (sm.totalObtainMarks > sm.subTotalMarks)
                                {
                                    return Json(new { success = false, responseText = "Obtain Marks sould be less than or equal to Total Marks." }, JsonRequestBehavior.AllowGet);
                                }
                                else
                                {
                                    sm.CreatedBy = LoginInfo.UserID;
                                    sm.CreatedDate = DateTime.Now;
                                    sm.UpdatedBy = 0;
                                    sm.IsDeleted = false;
                                    sm.DeletedBy = 0;
                                    con.stdObtmark.Add(sm);
                                    con.SaveChanges();
                                }
                            }
                        }
                        return Json(new { success = true, responseText = "All Student Data Entered Successfully." }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        return Json(new { success = false, responseText = "Please insert All Student Record." }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { success = false, responseText = "Please insert All Student Record." }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, responseText = "Please insert All Student Record." }, JsonRequestBehavior.AllowGet);
            }
        }
        [CheckSession]
        public ActionResult ShowStdMarks()
        {
            PopulatExamType();
            PopulatAllSes();
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult ShowStdMarks(StdObtDetail sd, string StdRoll)
        {
            try
            {
                PopulatExamType();
                PopulatAllSes();
                PopulatExamType();
                if (StdRoll != "")
                {
                    var GetStdId = con.std.Where(s => s.stdRollNo == StdRoll).FirstOrDefault();
                    if (GetStdId != null)
                    {
                        var getStdDetail = con.stdObtmark.Where(s => s.stdId == GetStdId.stdId && s.etId == sd.etId && s.sesId == sd.sesId).ToList();
                        if (getStdDetail.Count == 0)
                        {
                            TempData["Error"] = "No Record Found";
                        }
                        else
                        {
                            TempData["StdMarks"] = getStdDetail;
                            foreach (var i in getStdDetail)
                            {
                                ViewBag.Ses = i.sesId;
                                ViewBag.Sec = i.secId;
                                ViewBag.Class = i.classId;
                            }
                            ViewBag.StdId = GetStdId.stdId;
                            ViewBag.Exam = sd.etId;
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Roll Number!";
                    }
                }
            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error while getting info!";
            }
            PopulatExamType();
            return View();
        }

        [CheckSession]
        public ActionResult PrintStdMarkSheet(string stId, string ssId, string scId, string clId, string etId)
        {
            try
            {
                int StdId, SecId, SesId, ClassId, EtId;
                StdId = Convert.ToInt32(stId);
                SesId = Convert.ToInt32(ssId);
                SecId = Convert.ToInt32(scId);
                ClassId = Convert.ToInt32(clId);
                if (etId != "" && etId != null)
                {
                    EtId = Convert.ToInt32(etId);
                    var getExamTye = con.examtype.Where(e => e.etId == EtId).FirstOrDefault();
                    var GetStdMarks = con.stdObtmark.Where(s => s.stdId == StdId && s.sesId == SesId && s.secId == SecId && s.classId == ClassId && s.etId == EtId).ToList();
                    if (GetStdMarks.Count != 0)
                    {
                        Double obtTotal = 0, total = 0;
                        foreach (var i in GetStdMarks)
                        {
                            obtTotal = obtTotal + i.totalObtainMarks;
                            total = total + i.subTotalMarks;
                        }
                        ViewBag.Total = total;
                        ViewBag.ObtTotal = obtTotal;
                        var getStdDetail = con.std.Where(s => s.stdId == StdId).FirstOrDefault();
                        TempData["Std"] = getStdDetail;
                        TempData["StdMarks"] = GetStdMarks;
                        TempData["Exam"] = getExamTye.etname;
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("StdSingleObt");
                    }
                }
                else
                {
                    var GetStdMarks = con.stdObtmark.Where(s => s.stdId == StdId && s.sesId == SesId && s.secId == SecId && s.classId == ClassId).ToList();
                    if (GetStdMarks.Count != 0)
                    {
                        Double obtTotal = 0, total = 0;
                        foreach (var i in GetStdMarks)
                        {
                            obtTotal = obtTotal + i.totalObtainMarks;
                            total = total + i.subTotalMarks;
                        }
                        ViewBag.Total = total;
                        ViewBag.ObtTotal = obtTotal;
                        var getStdDetail = con.std.Where(s => s.stdId == StdId).FirstOrDefault();
                        TempData["Std"] = getStdDetail;
                        TempData["StdMarks"] = GetStdMarks;
                        TempData["Exam"] = "All";
                        return View();
                    }
                    else
                    {
                        return RedirectToAction("StdSingleObt");
                    }
                }


            }
            catch (Exception ex)
            {
                return RedirectToAction("ShowStdMarks");
            }

        }
        [CheckSession]
        public ActionResult EditMarks(int id)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatExamType();
                PopulatSubJect();

                var GetMarks = con.stdObtmark.Where(s => s.somID == id).FirstOrDefault();
                return View(GetMarks);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
                return RedirectToAction("ShowStdMarks");
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult EditMarks(int id, StdObtainMarks sd)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatExamType();
                PopulatSubJect();
                var GetMarks = con.stdObtmark.Where(s => s.somID == id).FirstOrDefault();
                GetMarks.smt1 = sd.smt1;
                GetMarks.smt2 = sd.smt2;
                GetMarks.smt3 = sd.smt3;
                GetMarks.UpdatedBy = 1;
                GetMarks.UpdatedDate = DateTime.Now;
                GetMarks.totalObtainMarks = sd.smt1 + sd.smt2 + sd.smt3;
                con.SaveChanges();
                TempData["SuccessMessage"] = "Student Marks Update Sucessfully";
                return RedirectToAction("ShowStdMarks");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error occured please try again";
                return RedirectToAction("ShowStdMarks");
            }
        }

        public ActionResult DelStdMarks(int id)
        {
            try
            {
                var GetMarks = con.stdObtmark.Where(s => s.somID == id).FirstOrDefault();
                con.stdObtmark.Remove(GetMarks);
                con.SaveChanges();
                TempData["SuccessMessage"] = "Student Marks Deleted Sucessfully";
                return RedirectToAction("ShowStdMarks");
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "Student Marks cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Student Marks not deleted please contact to soft support";
                }
                return RedirectToAction("ShowStdMarks");
            }
        }


        [CheckSession]
        public ActionResult PromoteStudent()
        {
            PopulatAllSes();
            PopulatClass();
            PopulatExamType();
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult PromoteStudent(int ToSes, int FromSes, int FromClass, int ToClass, int ExamId)
        {
            try
            {
                List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                List<StdFeeDetail> StdFailed = new List<StdFeeDetail>();
                List<StdFeeDetail> Promoted = new List<StdFeeDetail>();
                List<StdFeeDetail> Failed = new List<StdFeeDetail>();
                //List<StdFeeDetail> StdPassed = new List<StdFeeDetail>();
                PopulatAllSes();
                PopulatClass();
                PopulatExamType();
                var GetPassingPercent = con.passing.Where(p => p.classId == FromClass).FirstOrDefault();
                if(GetPassingPercent != null)
                {
                    var GetPromoStd = con.stdObtmark.Where(s => s.sesId == FromSes && s.classId == FromClass && s.etId == ExamId).ToList();
                    if (GetPromoStd.Count != 0)
                    {
                        foreach (var i in GetPromoStd)
                        {
                            double ObtainMarksPercentage = (i.totalObtainMarks * 100) / i.subTotalMarks;
                            if (ObtainMarksPercentage < GetPassingPercent.Marks)
                            {
                                StdFeeDetail std = new StdFeeDetail();
                                StdFeeDetail stdfailed = new StdFeeDetail();

                                var chkFailed = StdFailed.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                if (chkFailed == null)
                                {
                                    var StdDetial = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                    stdfailed.stdId = i.stdId;
                                    stdfailed.Stdname = StdDetial.pr.perName;
                                    stdfailed.RollNo = StdDetial.stdRollNo;
                                    stdfailed.Phone = StdDetial.pr.perContactOne;
                                    stdfailed.Image = StdDetial.pr.perImage;
                                    stdfailed.CNIC = StdDetial.pr.perCNIC;
                                    StdFailed.Add(stdfailed);
                                    Failed.Add(stdfailed);
                                }
                                TempData["Failed"] = Failed;
                            }
                            else
                            {
                                StdFeeDetail std = new StdFeeDetail();
                                var ChkStdID = StdList.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                if (ChkStdID == null)
                                {
                                    std.stdId = i.stdId;
                                    StdList.Add(std);
                                }
                            }
                        }

                        var StdId = (from sa in StdList
                                     where !StdFailed
                                              .Any(o => o.stdId == sa.stdId)
                                     select new
                                     {
                                         sa.stdId,
                                     }).ToList();
                        if (StdId.Count != 0)
                        {
                            foreach (var i in StdId)
                            {
                                StdFeeDetail std = new StdFeeDetail();
                                var StdDetial = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                var GetStdforPromote = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();

                                //Promote
                                GetStdforPromote.sesId = ToSes;
                                GetStdforPromote.classId = ToClass;
                                con.SaveChanges();
                                ////End Promote////

                                //Show Promoted//
                                std.stdId = i.stdId;
                                std.Stdname = StdDetial.pr.perName;
                                std.RollNo = StdDetial.stdRollNo;
                                std.Phone = StdDetial.pr.perContactOne;
                                std.Image = StdDetial.pr.perImage;
                                std.CNIC = StdDetial.pr.perCNIC;
                                Promoted.Add(std);
                            }
                            TempData["Promoted"] = Promoted;
                            var GetFromClassName = con.cls.Where(c => c.classId == FromClass).FirstOrDefault();
                            var GetToClassName = con.cls.Where(c => c.classId == ToClass).FirstOrDefault();
                            TempData["SuccessMessage"] = "Students of Class " + GetFromClassName.classname + " (who has passing Marks) Promoted to  Class " + GetToClassName.classname + " and New Session Successfully.";
                            return View();
                        }
                        else
                        {
                            TempData["Error"] = "No Sudent Prometed To the Next Class.";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "No Record found.";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Passing marks of this class is not entered. Please enter passing marks and try again";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "This is some error! Student not promoted.";
                return View();
            }
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

    }
}