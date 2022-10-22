using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class TeacherPanelController : Controller
    {
        DBCon con = new DBCon();

        // GET: TeacherPanel
        public ActionResult AssignTeacherToClass(string Code)
        {
            try
            {
                if (Code != "" && Code != null)
                {
                    PopulatSec();
                    PopulatSes();
                    PopulatClass();
                    PopulatAllSubJect();
                    int code = Convert.ToInt32(Code);
                    var getPerson = con.person.Where(s => s.perCode == code).FirstOrDefault();
                    if (getPerson != null)
                    {
                        if (getPerson.roleId == 5)
                        {
                            TempData["Person"] = getPerson;
                            ViewBag.Code = Code;
                            //ViewBag.roleId = reg.roleId;
                            ViewBag.perId = getPerson.perId;
                            var getData = con.teachers.Where(p => p.perId == getPerson.perId).ToList();
                            if (getData.Count != 0)
                            {
                                TempData["TeachersData"] = getData;
                            }

                            return View();
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter the Teacher Code Number. This Code number belongs to an " + getPerson.role.name + " role.";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Code Number";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Issue Please contact to Sof Support";
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult AssignTeacherToClass(string Code, string submitButton, TeacherClass teach)
        {
            try
            {
                PopulatSec();
                PopulatSes();
                PopulatClass();
                PopulatAllSubJect();
                if (submitButton == "Show")
                {
                    if (Code != "" && Code != null)
                    {
                        int code = Convert.ToInt32(Code);
                        var getPerson = con.person.Where(s => s.perCode == code).FirstOrDefault();
                        if (getPerson != null)
                        {
                            if (getPerson.roleId == 5)
                            {
                                TempData["Person"] = getPerson;
                                ViewBag.Code = Code;
                                //ViewBag.roleId = reg.roleId;
                                ViewBag.perId = getPerson.perId;

                                var getData = con.teachers.Where(p => p.perId == getPerson.perId).ToList();
                                if (getData.Count != 0)
                                {
                                    TempData["TeachersData"] = getData;
                                }

                                return View();
                            }
                            else
                            {
                                TempData["Error"] = "Please Enter the Teacher Code Number. This Code number belongs to an " + getPerson.role.name + " role.";
                                return View();
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Incorrect Code Number";
                            return View();
                        }



                    }
                }
                else
                {
                    if (teach.sesId != 0 && teach.classId != 0 && teach.secId != 0 && teach.perId != 0 && teach.subId != 0)
                    {
                        var chkEntry = con.teachers.Where(t => t.sesId == teach.sesId && t.classId == teach.classId && t.secId == teach.secId && t.perId == teach.perId && t.subId == teach.subId).Any();
                        if (chkEntry == true)
                        {
                            TempData["Error"] = "This Class and Subject is already assign to this teacher";
                            return RedirectToAction("AssignTeacherToClass", new { Code });
                        }
                        else
                        {

                            var chkEntry1 = con.teachers.Where(t => t.sesId == teach.sesId && t.classId == teach.classId && t.secId == teach.secId && t.subId == teach.subId).Any();
                            if (chkEntry1 == true)
                            {
                                TempData["Error"] = "This Class and Subject is already assign an other teacher";
                                return RedirectToAction("AssignTeacherToClass", new { Code });
                            }
                            else
                            {
                                con.teachers.Add(teach);
                                con.SaveChanges();
                                TempData["Success"] = "Class and Subject assign to this teacher successfully";
                                return RedirectToAction("AssignTeacherToClass", new { Code });
                            }

                        }
                    }
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Issue Please contact to Sof Support";
                return View();
            }
        }

        public ActionResult EditAssignTeacherToClass(int id)
        {
            try
            {
                PopulatSec();
                PopulatSes();
                PopulatClass();
                PopulatAllSubJect();
                var getData = con.teachers.Find(id);
                if (getData != null)
                {
                    return View(getData);
                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AssignTeacherToClass");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Issue Please contact to Sof Support";
                return RedirectToAction("AssignTeacherToClass");
            }
        }

        [HttpPost]
        public ActionResult EditAssignTeacherToClass(int id, TeacherClass teach)
        {
            try
            {
                PopulatSec();
                PopulatSes();
                PopulatClass();
                PopulatAllSubJect();
                var getData = con.teachers.Find(id);
                if (getData != null)
                {
                    var Code = getData.person.perCode;
                    var chkEntry = con.teachers.Where(t => t.sesId == teach.sesId && t.classId == teach.classId && t.secId == teach.secId && t.subId == teach.subId && t.perId == teach.perId && t.TeachClassId != id).Any();
                    if (chkEntry == true)
                    {
                        TempData["Error"] = "This Class and Subject is already assign to this teacher";
                        return View(getData);
                    }
                    else
                    {
                        var chkEntry1 = con.teachers.Where(t => t.sesId == teach.sesId && t.classId == teach.classId && t.secId == teach.secId && t.subId == teach.subId && t.TeachClassId != id).Any();
                        if (chkEntry1 == true)
                        {
                            TempData["Error"] = "This Class and Subject is already assign to this teacher";
                            return View(getData);
                        }
                        else
                        {
                            getData.secId = teach.secId;
                            getData.sesId = teach.sesId;
                            getData.classId = teach.classId;
                            getData.subId = teach.subId;
                            con.SaveChanges();
                            TempData["Success"] = "Entry Updated Successfully";
                            return RedirectToAction("AssignTeacherToClass", new { Code });
                        }
                    }

                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AssignTeacherToClass");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Issue Please contact to Sof Support";
                return RedirectToAction("AssignTeacherToClass");
            }
        }

        public ActionResult DelAssignTeacherToClass(int id)
        {
            try
            {
                var getData = con.teachers.Find(id);
                if (getData != null)
                {
                    var Code = getData.person.perCode;
                    con.teachers.Remove(getData);
                    con.SaveChanges();
                    TempData["Success"] = "Entry Deleted Successfully";
                    return RedirectToAction("AssignTeacherToClass", new { Code });
                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AssignTeacherToClass");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Issue Please contact to Sof Support";
                return RedirectToAction("AssignTeacherToClass");
            }
        }


        public ActionResult ClassMarking(int id)
        {
            try
            {
                PopulatSubJect();
                PopulatExamType();
                var getData = con.teachers.Where(t => t.TeachClassId == id).FirstOrDefault();
                if (getData != null)
                {
                    TempData["Data"] = getData;

                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error pelase contact to soft support";
                return View();
            }
        }
        [HttpPost]
        public ActionResult ClassMarking(StdObtDetail st, string submitButton, int id)
        {
            try
            {
                PopulatSubJect();
                PopulatExamType();
                List<StdObtDetail> StdList = new List<StdObtDetail>();
                List<StdObtDetail> SubList = new List<StdObtDetail>();
                //var getData = con.teachers.Where(t => t.TeachClassId == id).FirstOrDefault();
                //if (getData != null)
                //{
                //    TempData["Data"] = getData;

                //}
                if (submitButton == "Marking")
                {
                    if (id != null && id != 0)
                    {
                        var getData = con.teachers.Where(t => t.TeachClassId == id).FirstOrDefault();
                        if (getData != null)
                        {
                            TempData["Data"] = getData;

                        }
                    }


                    var getStd = con.std.Where(s => s.classId == st.classId && s.sesId == st.sesId && s.secId == st.secId && s.IsDeleted == false && s.stdStatus == "Active").ToList();
                    if (getStd.Count != 0)
                    {
                        var StdId = (from sa in con.std
                                     where !con.stdObtmark
                                              .Any(o => o.stdId == sa.stdId && o.etId == st.etId && o.subId == st.subId && o.sesId == sa.sesId && o.classId == sa.classId && o.secId == sa.secId)
                                     where sa.classId == st.classId && sa.sesId == st.sesId && sa.secId == st.secId && sa.IsDeleted == false && sa.stdStatus == "Active"

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


                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error pelase contact to soft support";
                return View();
            }
        }

        //Single Obtain Marks

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
                        //Find if Student is related to current Teacher?
                        int id = Convert.ToInt32(Session["ID"]);
                        var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
                        if (getPerId != null)
                        {
                            var chkStudent = con.teachers.Where(t => t.classId == StdData.classId && t.secId == StdData.secId).Any();
                            if(chkStudent == true)
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
                                return RedirectToAction("StdSingleObt", new { RollNo});
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

        public ActionResult StdExamReport()
        {
            PopulateSpecificSes();
            PopulateSpecificClass();
            PopulateSpecificSec();
            PopulatExamType();
            PopulatSubJect();
            return View();
        }

        public ActionResult _StdExamReport(int SesId, int ClassId, int SecId, int SubId, int EType)
        {
            try
            {
                var getStdDetail = con.stdObtmark.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId 
                && s.subId == SubId && s.etId == EType && s.std.stdStatus == "Active").OrderBy(s => s.std.stdRollNo).ToList();
                if (getStdDetail.Count != 0)
                {
                    TempData["StdMarks"] = getStdDetail;

                    ViewBag.ReportNo = 1;
                    var Session = getStdDetail.Where(s => s.sesId == SesId).Select(a => a.ses.sesName).FirstOrDefault();
                    TempData["Session"] = Session;
                    var Class = getStdDetail.Where(s => s.classId == ClassId).Select(a => a.cls.classname).FirstOrDefault();
                    TempData["ClassName"] = Class;
                    var Section = getStdDetail.Where(s => s.secId == SecId).Select(a => a.sec.secName).FirstOrDefault();
                    TempData["Section"] = Section;
                    var Subject = getStdDetail.Where(s => s.subId == SubId).Select(a => a.sb.subName).FirstOrDefault();
                    TempData["Subject"] = Subject;
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
        
        public void PopulateSpecificSes()
        {
            List<InstSession> List = new List<InstSession>();
            int id = Convert.ToInt32(Session["ID"]);
            var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
            if (getPerId != null)
            {
                var getTeachSub = con.teachers.Where(t => t.perId == getPerId.perId).ToList();
                if (getTeachSub.Count != 0)
                {
                    foreach (var i in getTeachSub)
                    {
                        InstSession sub = new InstSession();
                        var chkSub = List.Where(s => s.sesId == i.sesId).Any();
                        if (chkSub == false)
                        {
                            var Getist = con.InstSes.Where(s => s.sesId == i.sesId).Any();
                            if (Getist == true)
                            {
                                sub.sesId = i.sesId;
                                sub.sesName = i.ses.sesName;
                                List.Add(sub);
                            }

                        }


                    }

                }

            }
            SelectList sl = new SelectList(List, "sesId", "sesName");
            ViewData["Session"] = sl;
        }
        public void PopulateSpecificClass()
        {
            List<Class> List = new List<Class>();
            int id = Convert.ToInt32(Session["ID"]);
            var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
            if (getPerId != null)
            {
                var getTeachSub = con.teachers.Where(t => t.perId == getPerId.perId).ToList();
                if (getTeachSub.Count != 0)
                {
                    foreach (var i in getTeachSub)
                    {
                        Class sub = new Class();
                        var chkSub = List.Where(s => s.classId == i.classId).Any();
                        if (chkSub == false)
                        {
                            var Getist = con.cls.Where(s => s.classId == i.classId).Any();
                            if (Getist == true)
                            {
                                sub.classId = i.classId;
                                sub.classname = i.cls.classname;
                                List.Add(sub);
                            }

                        }


                    }

                }

            }
            SelectList sl = new SelectList(List, "classId", "classname");
            ViewData["Class"] = sl;
        }
        public void PopulateSpecificSec()
        {
            List<InstSection> List = new List<InstSection>();
            int id = Convert.ToInt32(Session["ID"]);
            var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
            if (getPerId != null)
            {
                var getTeachSub = con.teachers.Where(t => t.perId == getPerId.perId).ToList();
                if (getTeachSub.Count != 0)
                {
                    foreach (var i in getTeachSub)
                    {
                        InstSection sub = new InstSection();
                        var chkSub = List.Where(s => s.secId == i.secId).Any();
                        if (chkSub == false)
                        {
                            var Getist = con.InstSec.Where(s => s.secId == i.secId).Any();
                            if (Getist == true)
                            {
                                sub.secId = i.secId;
                                sub.secName = i.sec.secName;
                                List.Add(sub);
                            }

                        }


                    }

                }

            }
            SelectList sl = new SelectList(List, "secId", "secName");
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
        public void PopulatSec()
        {
            //Populating the dropdown for Section
            SelectList sl = new SelectList(con.InstSec.ToList(), "secId", "secName");
            ViewData["Section"] = sl;
        }
        public void PopulatSubJect()
        {
            List<Subject> SubjectList = new List<Subject>();
            int id = Convert.ToInt32(Session["ID"]);
            var getPerId = con.person.Where(p => p.id == id).FirstOrDefault();
            if (getPerId != null)
            {
                var getTeachSub = con.teachers.Where(t => t.perId == getPerId.perId).ToList();
                if (getTeachSub.Count != 0)
                {
                    foreach (var i in getTeachSub)
                    {
                        Subject sub = new Subject();
                        var chkSub = SubjectList.Where(s => s.subId == i.subId).Any();
                        if (chkSub == false)
                        {
                            var Getist = con.sub.Where(s => s.subId == i.subId && s.isVisible == true && s.IsDeleted == false).Any();
                            if (Getist == true)
                            {
                                sub.subId = i.subId;
                                sub.subName = i.subject.subName + "-" + i.subject.subCode;
                                SubjectList.Add(sub);
                            }

                        }


                    }

                }

            }
            SelectList sl = new SelectList(SubjectList, "subId", "subName");
            ViewData["SubJect"] = sl;
            //Populating the dropdown for Session

            //var GetList = con.sub.Where(s => s.isVisible == true && s.IsDeleted == false).ToList();
            //if (GetList.Count != 0)
            //{
            //    foreach (var i in GetList)
            //    {
            //        Subject sub = new Subject();
            //        var chkSub = SubjectList.Where(s => s.subId == i.subId).FirstOrDefault();
            //        if (chkSub == null)
            //        {
            //            sub.subId = i.subId;
            //            sub.subName = i.subName + "-" + i.subCode;
            //            SubjectList.Add(sub);
            //        }
            //    }

            //}

        }

        public void PopulatAllSubJect()
        {
            List<Subject> SubjectList = new List<Subject>();
            //Populating the dropdown for Session
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