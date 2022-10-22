
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class ShortCourseController : Controller
    {
        DBCon con = new DBCon();
        // GET: ShortCourse
        public ActionResult AddShortCourseSubject()
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
                    PopulatClass();
                    var GetData = con.scs.OrderByDescending(s => s.scsId).Where(s => s.IsDeleted == false).ToList();
                    if (GetData != null)
                    {
                        TempData["Subject"] = GetData;
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddShortCourseSubject(short_course_Subject sub)
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
                    
                    var ChkName = con.scs.Where(s => s.scsName == sub.scsName && s.IsDeleted == false).Any();
                    if (ChkName == false)
                    {
                        sub.CreatedBy = LoginInfo.UserID;
                        sub.CreatedDate = DateTime.Now;
                        con.scs.Add(sub);
                        con.SaveChanges();
                        PopulatClass();
                        TempData["Success"] = "Short Course Subject Added Successfully";
                        
                        return RedirectToAction("AddShortCourseSubject");
                    }
                    else
                    {
                        TempData["Error"] = "This Course is already exist please choose a different one";
                        return RedirectToAction("AddShortCourseSubject");
                    }

                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public ActionResult UpdateShortCourse(int id)
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
                    var getData = con.scs.Where(s => s.scsId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        return View(getData);
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddShortCourseSubject");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddShortCourseSubject");
            }
        }

        [HttpPost]
        public ActionResult UpdateShortCourse(int id, short_course_Subject sc)
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
                    var getData = con.scs.Where(s => s.scsId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        var chkName = con.scs.Where(s => s.scsName == sc.scsName && s.scsId != getData.scsId && s.IsDeleted == false).Any();
                        if (chkName == true)
                        {
                            TempData["Error"] = "This Course Name is already exist please choose a different name";
                            return View();
                        }
                        else
                        {
                            getData.scsName = sc.scsName;
                            getData.scsDuration = sc.scsDuration;
                            getData.scsFee = sc.scsFee;
                            getData.UpdatedBy = LoginInfo.UserID;
                            getData.UpdatedDate = DateTime.Now;
                            con.SaveChanges();
                            TempData["Success"] = "Course Name Updated successfully";
                            return RedirectToAction("AddShortCourseSubject");
                        }

                    }
                    else
                    {
                        return RedirectToAction("AddShortCourseSubject");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddShortCourseSubject");
            }
        }

        public ActionResult DeleteShortCourse(int id)
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
                    var getData = con.scs.Where(s => s.scsId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        con.scs.Remove(getData);
                        con.SaveChanges();
                        TempData["Success"] = "Course Name deleted successfully";
                        return RedirectToAction("AddShortCourseSubject");
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddShortCourseSubject");
                    }
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "Course Name cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Course Name not deleted please contact to soft support";
                }
                return RedirectToAction("AddShortCourseSubject");
            }
        }

        public ActionResult AddStdToShortCourse(string RollNo, string btn)
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
                    PopulatClassForCourse();
                    if (btn == "Show")
                    {
                        if (RollNo != null && RollNo != "")
                        {
                            var StdRoll = RollNo;
                            var getStd = con.std.Where(s => s.stdRollNo == StdRoll).FirstOrDefault();
                            if (getStd != null)
                            {
                                if (getStd.stdStatus == "Active")
                                {
                                    ViewBag.StdId = getStd.stdId;
                                    ViewBag.RollNo = getStd.stdRollNo;
                                    TempData["Std"] = getStd;

                                    var chkFee = con.scr.Where(s => s.stdId == getStd.stdId && s.IsDeleted == false).OrderByDescending(o => o.scrId).ToList();
                                    if (chkFee.Count != 0)
                                    {
                                        TempData["StdFee"] = chkFee;
                                    }
                                    return View();
                                }
                                else
                                {
                                    TempData["Error"] = "You cannot enroll in a course because your Status is " + getStd.stdStatus + ".";
                                }
                            }
                            else
                            {
                                TempData["Error"] = "No Record found. Please Enter the correct Roll Number!";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll Number";
                        }
                    }
                    return View();
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddStdToShortCourse(short_course_Registration Reg, string submitButton, string StdRoll, string RollNo)
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
                    PopulatClassForCourse();
                    if (submitButton == "Show")
                    {
                        if (StdRoll != null && StdRoll != "")
                        {
                            var getStd = con.std.Where(s => s.stdRollNo == StdRoll).FirstOrDefault();
                            if (getStd != null)
                            {
                                if (getStd.stdStatus == "Active")
                                {
                                    ViewBag.StdId = getStd.stdId;
                                    ViewBag.RollNo = getStd.stdRollNo;
                                    TempData["Std"] = getStd;

                                    var chkFee = con.scr.Where(s => s.stdId == getStd.stdId && s.IsDeleted == false).OrderByDescending(o => o.scrId).ToList();
                                    if (chkFee.Count != 0)
                                    {
                                        TempData["StdFee"] = chkFee;
                                    }
                                    return View();
                                }
                                else
                                {
                                    TempData["Error"] = "You cannot enroll in a course because your Status is " + getStd.stdStatus + ".";
                                }
                            }
                            else
                            {
                                TempData["Error"] = "No Record found. Please Enter the correct Roll Number!";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll Number";
                        }
                    }
                    else
                    {
                        var btn = "Show";
                        if (Reg.scsId != 0)
                        {
                            var ChkFee = con.scr.Where(s => s.classId == Reg.classId && s.scsId == Reg.scsId && s.stdId == Reg.stdId && s.IsDeleted == false).FirstOrDefault();

                            if (ChkFee != null)
                            {
                                if (ChkFee.scPendingFee == 0)
                                {
                                    TempData["Error"] = "" + ChkFee.scSub.scsName + " course fee is already received against this " + ChkFee.std.stdRollNo + " Roll number!";
                                    return RedirectToAction("AddStdToShortCourse", new { btn, RollNo } );
                                }
                                else
                                {
                                    var getFee = ChkFee.scSub.scsFee;
                                    if (Reg.scReceivedFee > getFee)
                                    {
                                        TempData["Error"] = "Received Fee must be less than or equal to total fee";
                                        return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                    }
                                    else
                                    {

                                        if (Reg.scDiscount != 0)
                                        {
                                            if (ChkFee.scDiscount > 0)
                                            {
                                                TempData["Error"] = "Discount has already given to this student.";
                                                return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }
                                            else
                                            {
                                                if (Reg.scReceivedFee + (ChkFee.scReceivedFee + ChkFee.scDiscount) - Reg.scDiscount > getFee)
                                                {
                                                    TempData["Error"] = "Received Fee must be less than or equal to total fee. Please Enter the discount carefully";
                                                    return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                                }
                                                else if (Reg.scReceivedFee + (ChkFee.scReceivedFee + ChkFee.scDiscount) - Reg.scDiscount < getFee)
                                                {
                                                    ChkFee.scReceivedFee = Reg.scReceivedFee + ChkFee.scReceivedFee;
                                                    ChkFee.scPendingFee = ChkFee.scPendingFee - Reg.scReceivedFee - Reg.scDiscount;
                                                    ChkFee.scDiscount = Reg.scDiscount;
                                                    if(Reg.scPendingDate.Year != 1)
                                                    {
                                                        ChkFee.scPendingDate = Reg.scPendingDate;
                                                    }
                                                    
                                                    con.SaveChanges();
                                                    TempData["Success"] = "Pending Fee received successfully";
                                                    return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                                }
                                                else
                                                {
                                                    ChkFee.scReceivedFee = Reg.scReceivedFee + ChkFee.scReceivedFee;
                                                    ChkFee.scPendingFee = ChkFee.scPendingFee - Reg.scReceivedFee - Reg.scDiscount;
                                                    ChkFee.scDiscount = Reg.scDiscount;
                                                    if (Reg.scPendingDate.Year != 1)
                                                    {
                                                        ChkFee.scPendingDate = Reg.scPendingDate;
                                                    }

                                                    con.SaveChanges();
                                                    TempData["Success"] = "Pending Fee received successfully";
                                                    return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                                }

                                            }

                                        }
                                        else
                                        {
                                            if (Reg.scReceivedFee + (ChkFee.scReceivedFee + ChkFee.scDiscount) > getFee)
                                            {
                                                TempData["Error"] = "Received Fee must be less than or equal to total fee";
                                                return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }
                                            else if (Reg.scReceivedFee + (ChkFee.scReceivedFee + ChkFee.scDiscount) < getFee)
                                            {
                                                ChkFee.scReceivedFee = Reg.scReceivedFee + ChkFee.scReceivedFee;
                                                ChkFee.scPendingFee = ChkFee.scPendingFee - Reg.scReceivedFee;
                                                if (Reg.scPendingDate.Year != 1)
                                                {
                                                    ChkFee.scPendingDate = Reg.scPendingDate;
                                                }

                                                con.SaveChanges();
                                                TempData["Success"] = "Pending Fee received successfully";
                                                return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }
                                            else
                                            {
                                                ChkFee.scReceivedFee = Reg.scReceivedFee + ChkFee.scReceivedFee;
                                                ChkFee.scPendingFee = ChkFee.scPendingFee - Reg.scReceivedFee;
                                                if (Reg.scPendingDate.Year != 1)
                                                {
                                                    ChkFee.scPendingDate = Reg.scPendingDate;
                                                }

                                                con.SaveChanges();
                                                TempData["Success"] = "Pending Fee received successfully";
                                                return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }
                                        }
                                    }

                                }
                            }
                            if (Reg.scReceivedFee != 0)
                            {
                                var getFee = con.scs.Where(s => s.scsId == Reg.scsId).FirstOrDefault();
                                if (Reg.scReceivedFee > getFee.scsFee)
                                {
                                    TempData["Error"] = "Received Fee must be less than or equal to total fee";
                                    return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                }
                                else
                                {
                                    if (Reg.scDiscount != 0)
                                    {
                                        if (Reg.scDiscount + Reg.scReceivedFee == getFee.scsFee)
                                        {
                                            Reg.scStatus = "Continue";
                                            Reg.CreatedBy = LoginInfo.UserID;
                                            Reg.CreatedDate = DateTime.Now;
                                            con.scr.Add(Reg);
                                            con.SaveChanges();
                                            TempData["Success"] = "Student Short Course Fee & Registration Completed Successfully";
                                            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                        }
                                        else if (Reg.scDiscount + Reg.scReceivedFee > getFee.scsFee)
                                        {
                                            TempData["Error"] = "Received Fee must be less than or equal to total fee. Please Enter the discount carefully";
                                            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                        }
                                        else if (Reg.scDiscount + Reg.scReceivedFee < getFee.scsFee)
                                        {
                                            Reg.scPendingFee = getFee.scsFee - (Reg.scReceivedFee + Reg.scDiscount);
                                            Reg.scStatus = "Continue";
                                            Reg.CreatedBy = LoginInfo.UserID;
                                            Reg.CreatedDate = DateTime.Now;
                                            con.scr.Add(Reg);
                                            con.SaveChanges();
                                            TempData["Success"] = "Student Short Course Fee & Registration Completed Successfully";
                                            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                        }
                                    }
                                    else
                                    {
                                        if (Reg.scfPaidDate.Year == 1)
                                        {
                                            Reg.scfPaidDate = DateTime.Now;
                                        }
                                        if (Reg.scPendingDate.Year == 1)
                                        {
                                            Reg.scPendingDate = DateTime.Now;
                                        }
                                        if (Reg.scPendingFee == 0)
                                        {
                                            Reg.scPendingFee = getFee.scsFee - Reg.scReceivedFee;
                                        }
                                        Reg.scStatus = "Continue";
                                        Reg.CreatedBy = LoginInfo.UserID;
                                        Reg.CreatedDate = DateTime.Now;
                                        con.scr.Add(Reg);
                                        con.SaveChanges();
                                        TempData["Success"] = "Student Short Course Fee & Registration Completed Successfully";
                                        return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                    }
                                }
                            }
                            else
                            {
                                TempData["Error"] = "Please Enter Received Fee";
                                return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Please Select the Course";
                            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                        }
                    }

                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some issue Please contact to soft support";
            }
            return View();
        }


        public ActionResult UpdateShortCourseReceivedFee(int id, string RollNo)
        {
            try
            {
                PopulatShortCourse();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var btn = "Show";
                    var getData = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        var getTotalFee = con.scs.Where(sc => sc.scsId == getData.scsId).FirstOrDefault();
                        int Fee = getTotalFee.scsFee;
                        TempData["TotalFee"] = Fee;

                        var getRoll = con.std.Where(s => s.stdId == getData.stdId).FirstOrDefault();
                        TempData["RollNo"] = getRoll.stdRollNo;
                        return View(getData);
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddStdToShortCourse");
            }
            return RedirectToAction("AddStdToShortCourse");
        }

        [HttpPost]
        public ActionResult UpdateShortCourseReceivedFee(int id, short_course_Registration Reg, string StdRoll, string RollNo)
        {
            var btn = "Show";
            try
            {
                
                PopulatShortCourse();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var ChkFee = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                    if (ChkFee != null)
                    {
                        if(ChkFee.scStatus == "Completed")
                        {
                            if (ChkFee != null)
                            {

                                var getFee = ChkFee.scSub.scsFee;
                                if (Reg.scReceivedFee > getFee)
                                {
                                    TempData["Error"] = "Received Fee must be less than or equal to total fee";
                                    return RedirectToAction("UpdateShortCourseReceivedFee", new { btn, RollNo });
                                }
                                else
                                {

                                    if (Reg.scDiscount != 0)
                                    {


                                        if (Reg.scReceivedFee - Reg.scDiscount > getFee)
                                        {
                                            TempData["Error"] = "Received Fee must be less than or equal to total fee. Please Enter the discount carefully";
                                            return RedirectToAction("UpdateShortCourseReceivedFee", new { btn, RollNo });
                                        }
                                        else if (Reg.scReceivedFee - Reg.scDiscount < getFee)
                                        {
                                            if (Reg.scReceivedFee + Reg.scDiscount > getFee)
                                            {
                                                TempData["Error"] = "Received Fee must be less than or equal to total fee. Please Enter the discount carefully";
                                                return RedirectToAction("UpdateShortCourseReceivedFee", new { btn, RollNo });
                                            }
                                            else
                                            {
                                                ChkFee.scReceivedFee = Reg.scReceivedFee;
                                                ChkFee.scPendingFee = getFee - Reg.scReceivedFee - Reg.scDiscount;
                                                ChkFee.scDiscount = Reg.scDiscount;
                                                con.SaveChanges();
                                                TempData["Success"] = "Fee updated successfully";
                                                RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }

                                        }
                                        else
                                        {
                                            if (Reg.scReceivedFee + Reg.scDiscount > getFee)
                                            {
                                                TempData["Error"] = "Received Fee must be less than or equal to total fee. Please Enter the discount carefully";
                                                return RedirectToAction("UpdateShortCourseReceivedFee", new { btn, RollNo });
                                            }
                                            else
                                            {
                                                ChkFee.scReceivedFee = Reg.scReceivedFee;
                                                ChkFee.scPendingFee = getFee - Reg.scReceivedFee - Reg.scDiscount;
                                                ChkFee.scDiscount = Reg.scDiscount;
                                                con.SaveChanges();
                                                TempData["Success"] = "Fee updated successfully";
                                                RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Reg.scReceivedFee > getFee)
                                        {
                                            TempData["Error"] = "Received Fee must be less than or equal to total fee";
                                            return RedirectToAction("UpdateShortCourseReceivedFee", new { btn, RollNo });
                                        }
                                        else if (Reg.scReceivedFee < getFee)
                                        {
                                            ChkFee.scReceivedFee = Reg.scReceivedFee;
                                            ChkFee.scPendingFee = getFee - Reg.scReceivedFee;
                                            ChkFee.scDiscount = Reg.scDiscount;
                                            con.SaveChanges();
                                            TempData["Success"] = "Fee updated successfully";
                                            RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                        }
                                        else
                                        {
                                            ChkFee.scReceivedFee = Reg.scReceivedFee;
                                            ChkFee.scPendingFee = getFee - Reg.scReceivedFee;
                                            ChkFee.scDiscount = Reg.scDiscount;
                                            con.SaveChanges();
                                            TempData["Success"] = "Fee updated successfully";
                                            RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "You cannot update this short course because it's status is completed.";
                            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                        }
                        
                       
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddStdToShortCourse");
            }
            return RedirectToAction("AddStdToShortCourse", new { btn, RollNo });
        }

        public ActionResult UpdateShortCourseStatus(int id)
        {
            try
            {
                PopulatShortCourse();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getData = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        return View(getData);
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddStdToShortCourse");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddStdToShortCourse");
            }
        }

        [HttpPost]
        public ActionResult UpdateShortCourseStatus(int id, short_course_Registration sr, string StdRoll)
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
                    var btn = "Show";
                    var getData = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.scStatus = sr.scStatus;
                        getData.UpdatedBy = LoginInfo.UserID;
                        getData.UpdatedDate = DateTime.Now;

                        con.SaveChanges();
                        TempData["Success"] = "Short Course status updated successfully";
                        RedirectToAction("AddStdToShortCourse", new { btn, StdRoll });
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        RedirectToAction("AddStdToShortCourse", new { btn, StdRoll });
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("AddStdToShortCourse");
            }
            return RedirectToAction("AddStdToShortCourse");
        }
        public ActionResult DeleteShortCourseReceivedFee(int id, string StdRollNo)
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
                    var btn = "Show";
                    var getData = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        con.scr.Remove(getData);
                        con.SaveChanges();
                        TempData["Success"] = "Course Received fee deleted successfully";
                        return RedirectToAction("AddStdToShortCourse", new { btn , StdRollNo});
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddStdToShortCourse", new { btn, StdRollNo });
                    }
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "Course Received fee cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Course Received fee not deleted please contact to soft support";
                }
                return RedirectToAction("AddStdToShortCourse");
            }
        }
        public ActionResult _ShortSub(int SubId)
        {
            try
            {
                var getSubject = con.scs.Where(s => s.scsId == SubId).FirstOrDefault();
                if (getSubject != null)
                {
                    TempData["Course"] = getSubject;
                    return PartialView();
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView();
        }

        public ActionResult DownloadFeeReceipt(int id)
        {
            try
            {
                var getData = con.scr.Where(s => s.scrId == id).FirstOrDefault();
                if(getData != null)
                {
                    TempData["Data"] = getData;
                    return View();
                }
                else
                {
                    TempData["Error"] = "No record found";
                    return RedirectToAction("AddStdToShortCourse");
                }
                
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public void PopulatClass()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.cls.ToList(), "classId", "classname");
            ViewData["Class"] = sl;
        }
        public void PopulatClassForCourse()
        {
            //Populating the dropdown for Class
            List<Class> clsList = new List<Class>();
            var getClasses = con.cls.ToList();
            foreach(var i in getClasses)
            {
                SMS.Models.Class cls = new Class();
                var getCourseClass = con.scs.Where(c => c.classId == i.classId).FirstOrDefault();
                if(getCourseClass != null)
                {
                    cls.classId = i.classId;
                    cls.classname = i.classname;
                    clsList.Add(cls);
                }
               
            }
            SelectList sl = new SelectList(clsList, "classId", "classname");
            ViewData["Class"] = sl;
        }

        public ActionResult PopulatCourse(int ClassId)
        {
            try
            {
                //Populating the dropdown for Locations
                SelectList objcourse = new SelectList(con.scs.Where(p => p.classId == ClassId && p.IsDeleted == false).ToList(), "scsId", "scsName");
                return Json(objcourse);
            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        public void PopulatShortCourse()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.scs.ToList(), "scsId", "scsName");
            ViewData["Course"] = sl;
        }

        //Check if Discount givern or not
        [CheckSession]
        public ActionResult DiscountCheck(int? val, int? sId)
        {
            if (val != null && val != 0 && sId != null && sId != 0)
            {
                var chkDis = con.scr.Where(s => s.stdId == sId && s.scsId == val && s.IsDeleted == false).FirstOrDefault();
                if(chkDis != null)
                {
                    if(chkDis.scDiscount > 0)
                    {
                        return Json(new { Success = "true" });
                    }
                }
            }
            else
            {
                return Json(new { Success = "false" });
            }
            return Json(new { Success = "false" });
        }
    }
}