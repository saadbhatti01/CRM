
using Newtonsoft.Json;
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class StudentController : Controller
    {
        DBCon con = new DBCon();
        SendMessage sendMessage = new SendMessage();
        int ss = 10;
        // GET: Student
        [CheckSession]
        public ActionResult StdReg()
        {
            try
            {
                PopulatCity();
                PopulatArea();
                PopulatCampus();
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatDegree();
                PopulatIstitute();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
            }

            return View();
        }
        [CheckSession]
        [CheckSession]
        [HttpPost]
        public ActionResult StdReg(RegViewModel reg, HttpPostedFileBase perImage)
        {
            //variables for If Exception occurer then deleted all inserted data
            int loginId = 0;
            int PersonId = 0;
            int StudentId = 0;
            int perRegIdId = 0;
            //End///
            int EduIdId = 0;
            Person per = new Person();
            Student std = new Student();
            try
            {

                if (reg.AreaId != 0)
                {
                    var CheckROll = con.std.Where(r => r.stdRollNo == reg.stdRollNo).Any();
                    if (CheckROll == false)
                    {
                        per.perName = reg.perName;
                        per.perGarName = reg.perGarName;

                        if (reg.perCNIC == "" || reg.perCNIC == null)
                        {
                            per.perCNIC = "12345";
                        }
                        else
                        {
                            per.perCNIC = reg.perCNIC;
                        }
                        if (reg.perEmail == "" || reg.perEmail == null)
                        {
                            per.perEmail = "a@a.a";
                        }
                        else
                        {
                            per.perEmail = reg.perEmail;
                        }
                        if (reg.perDOB == "" || reg.perDOB == null)
                        {
                            per.perDOB = "2020-01-01";
                        }
                        else
                        {
                            per.perDOB = reg.perDOB;
                        }
                        if (reg.perBloodGrp == "" || reg.perBloodGrp == null)
                        {
                            per.perBloodGrp = "notEntered";
                        }
                        else
                        {
                            per.perBloodGrp = reg.perBloodGrp;
                        }

                        if (reg.perContactOne == "" || reg.perContactOne == null)
                        {
                            per.perContactOne = "0000";
                        }
                        else
                        {
                            per.perContactOne = reg.perContactOne;
                        }

                        if (reg.perContactTwo == "" || reg.perContactTwo == null)
                        {
                            per.perContactTwo = "0000";
                        }
                        else
                        {
                            per.perContactTwo = reg.perContactTwo;
                        }

                        if (reg.perCurrentAdrs == "" || reg.perCurrentAdrs == null)
                        {
                            per.perCurrentAdrs = "notEntered";
                        }
                        else
                        {
                            per.perCurrentAdrs = reg.perCurrentAdrs;
                        }
                        if (reg.perPermanantAdrs == "" || reg.perPermanantAdrs == null)
                        {
                            per.perPermanantAdrs = "notEntered";
                        }
                        else
                        {
                            per.perPermanantAdrs = reg.perPermanantAdrs;
                        }

                        if (reg.perCast == "" || reg.perCast == null)
                        {
                            per.perCast = "notEntered";
                        }
                        else
                        {
                            per.perCast = reg.perCast;
                        }

                        if (reg.perReligion == "" || reg.perReligion == null)
                        {
                            per.perReligion = "notEntered";
                        }
                        else
                        {
                            per.perReligion = reg.perReligion;
                        }


                        if (perImage == null)
                        {
                            per.perImage = "avatar.jpg";
                        }
                        else
                        {
                            string Imagename = Path.GetFileName(perImage.FileName);
                            string PhysicalPath = Path.Combine(Server.MapPath("~/Images/"), Imagename);
                            perImage.SaveAs(PhysicalPath);
                            per.perImage = Imagename;
                        }


                        per.CityId = reg.CityId;
                        per.AreaId = reg.AreaId;
                        //For Student Login//
                        Login log = new Login();
                        log.usrName = per.perName;
                        log.usrLogin = reg.stdRollNo;
                        log.usrPassword = "abc123";
                        log.roleId = 3;
                        log.usrStatus = "Active";
                        con.login.Add(log);
                        con.SaveChanges();

                        loginId = log.id;

                        ////End Student Login//// 

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
                        std.sesId = reg.sesId;
                        std.secId = reg.secId;
                        std.camId = reg.camId;
                        std.classId = reg.classId;
                        std.stdRollNo = reg.stdRollNo;
                        std.stdStatus = reg.stdStatus;
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

                        //For Previous Educational Info
                        if (reg.DegreeId != 0 && reg.StdInsId != 0 && reg.TotalMarks != 0 && reg.ObtainMarks != 0)
                        {
                            StudentPreEduInfo info = new StudentPreEduInfo();
                            info.stdId = std.stdId;
                            info.DegreeId = reg.DegreeId;
                            info.StdInsId = reg.StdInsId;
                            info.TotalMarks = reg.TotalMarks;
                            info.ObtainMarks = reg.ObtainMarks;
                            info.RollNo = reg.RollNo;
                            info.PassingDate = reg.PassingDate;
                            con.stdEduInfos.Add(info);
                            con.SaveChanges();
                            EduIdId = info.StdEduInfoId;
                        }
                        //For Sending SMS
                        var chkModule = con.smsModule.Where(s => s.mnId == 1 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                        if (chkModule != null)
                        {

                            EncryptDecrypt decryption = new EncryptDecrypt();
                            var getAllotment = con.sMSAllotments.FirstOrDefault();
                            if (getAllotment != null)
                            {
                                int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                var expiryDate = decryption.Decrypt(getAllotment.saExpiryDate);
                                var Exdate = Convert.ToDateTime(expiryDate);
                                var Status = decryption.Decrypt(getAllotment.saStatus);
                                if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                {
                                    //Sending SMS

                                    sendMessage.SendSMSTurab(per.perContactOne, chkModule.smText);
                                    SentSMS sms = new SentSMS();
                                    sms.ssDate = DateTime.Now;
                                    sms.ssStatus = true;
                                    sms.perId = per.perId;
                                    sms.ssText = chkModule.smText;
                                    con.sentSMs.Add(sms);
                                    //Minus 1 SMS
                                    int msg = RemainingMsg - 1;
                                    var RemMsg = msg.ToString();
                                    getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());
                                    con.SaveChanges();
                                }
                            }

                        }
                        //End Sending SMS

                        TempData["SuccessMessage"] = "Student registration Successfully completed";
                        return RedirectToAction("StdReg");
                    }
                    else
                    {
                        TempData["Error"] = "This Roll Number Already Exist Please choose different one!";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Select Area. if you can't find your desired one then please Add Area.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Student registration not completed Please contact to soft support " + SoftSupport.SoftContactNo + "";
                //TempData["Info"] = "" + ex.Message + "";
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


                if (EduIdId != 0)
                {
                    var chk = con.stdEduInfos.Where(i => i.StdEduInfoId == EduIdId).FirstOrDefault();
                    if (chk != null)
                    {
                        con.stdEduInfos.Remove(chk);
                        con.SaveChanges();
                    }
                }

            }

            return RedirectToAction("StdReg");
        }
        [CheckSession]
        public ActionResult ShowStd()
        {
            try
            {
                PopulatCampus();
                PopulatClass();
                PopulatAllSes();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error of getting entries Please contact to soft support " + SoftSupport.SoftContactNo + " " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }

        public ActionResult _PopulateSudent(int SesId, int SecId, int ClassId)
        {
            try
            {
                var GetDetails = con.std.Where(c => c.sesId == SesId && c.secId == SecId && c.classId == ClassId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["List"] = GetDetails;
                }
                else
                {
                    TempData["No"] = "No Record found";
                }
                return PartialView();
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error of getting entries Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return PartialView();
            }
        }
        public ActionResult _PopulateCampusSudent(int SesId, int CampId)
        {
            try
            {
                var GetDetails = con.std.Where(c => c.sesId == SesId && c.camId == CampId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["List"] = GetDetails;
                }
                //else
                //{
                //    TempData["No"] = "No Record found";
                //}
                return PartialView("_PopulateSudent");
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error of getting entries Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return PartialView();
            }
        }
        [CheckSession]
        public ActionResult DetailStd(int id)
        {
            try
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
                             p.perCast,
                             p.perReligion,
                             p.perImage,
                             c.CityName,
                             a.AreaName,
                             sc.secName,
                             ss.sesName,
                             cl.classname,
                             s.stdId,
                             s.stdRollNo,
                             s.stdStatus,
                             cm.campusname
                         }).SingleOrDefault();
                if (i != null)
                {
                    RegViewModel reg = new RegViewModel();
                    reg.stdId = i.stdId;
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
                    reg.perCast = i.perCast;
                    reg.perReligion = i.perReligion;
                    reg.CityName = i.CityName;
                    reg.AreaName = i.AreaName;
                    reg.secName = i.secName;
                    reg.sesName = i.sesName;
                    reg.ClassName = i.classname;
                    reg.campusname = i.campusname;
                    reg.stdRollNo = i.stdRollNo;
                    reg.stdStatus = i.stdStatus;

                    //if Previouse Edicational Info available
                    var getInfo = con.stdEduInfos.Where(s => s.stdId == reg.stdId).FirstOrDefault();
                    if (getInfo != null)
                    {
                        TempData["Info"] = getInfo;
                    }
                    return View(reg);
                }
                else
                {
                    TempData["Error"] = "Student information not available please try again";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error of getting entries Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("ShowStd");
            }
            return RedirectToAction("ShowStd");
        }

        [CheckSession]
        public ActionResult PrintStdDetail(int id)
        {
            try
            {
                var getStd = con.std.Find(id);
                if (getStd != null)
                {
                    TempData["Std"] = getStd;
                }
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("DetailStd", new { id });
            }
        }
        [CheckSession]
        public ActionResult EditStd(int id)
        {
            try
            {
                PopulatCity();
                PopulatArea();
                PopulatCampus();
                PopulatClass();
                PopulatSec();
                PopulatAllSes();
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
                             p.perCast,
                             p.perReligion,
                             p.perImage,
                             c.CityName,
                             c.CityId,
                             a.AreaId,
                             a.AreaName,
                             sc.secId,
                             sc.secName,
                             ss.sesId,
                             ss.sesName,
                             cm.camId,
                             cm.campusname,
                             cl.classId,
                             cl.classname,
                             s.stdRollNo,
                             s.stdStatus,
                         }).SingleOrDefault();
                if (i != null)
                {
                    RegViewModel reg = new RegViewModel();
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
                    reg.perCast = i.perCast;
                    reg.perReligion = i.perReligion;
                    reg.CityId = i.CityId;
                    reg.CityName = i.CityName;
                    reg.AreaId = i.AreaId;
                    reg.AreaName = i.AreaName;
                    reg.secId = i.secId;
                    reg.secName = i.secName;
                    reg.sesId = i.sesId;
                    reg.sesName = i.sesName;
                    reg.classId = i.classId;
                    reg.ClassName = i.classname;
                    reg.camId = i.camId;
                    reg.campusname = i.campusname;
                    reg.stdRollNo = i.stdRollNo;
                    reg.stdStatus = i.stdStatus;
                    return View(reg);
                }
                else
                {
                    TempData["Error"] = "Student information not available please try again";
                }
            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error of getting Edit entries Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("ShowStd");
            }

            return RedirectToAction("ShowStd");

        }
        [HttpPost]
        [CheckSession]
        public ActionResult EditStd(int id, RegViewModel reg, HttpPostedFileBase perImage)
        {
            try
            {
                var per = con.person.FirstOrDefault(p => p.perId == id);
                var std = con.std.FirstOrDefault(s => s.perId == id);
                if (std != null)
                {
                    per.perName = reg.perName;
                    per.perGarName = reg.perGarName;
                    per.perDOB = reg.perDOB;
                    per.perCurrentAdrs = reg.perCurrentAdrs;
                    per.perPermanantAdrs = reg.perPermanantAdrs;
                    per.perContactOne = reg.perContactOne;
                    per.perContactTwo = reg.perContactTwo;
                    per.perCNIC = reg.perCNIC;
                    per.perEmail = reg.perEmail;
                    per.perBloodGrp = reg.perBloodGrp;
                    per.perCast = reg.perCast;
                    per.perReligion = reg.perReligion;
                    if (perImage == null)
                    {
                        per.perImage = per.perImage;
                    }
                    else
                    {
                        string Imagename = Path.GetFileName(perImage.FileName);
                        string PhysicalPath = Path.Combine(Server.MapPath("~/Images/"), Imagename);
                        perImage.SaveAs(PhysicalPath);
                        per.perImage = Imagename;
                    }

                    per.CityId = reg.CityId;
                    per.AreaId = reg.AreaId;
                    per.UpdatedBy = LoginInfo.UserID;
                    per.UpdatedDate = DateTime.Now;
                    con.SaveChanges();
                    std.sesId = reg.sesId;
                    std.secId = reg.secId;
                    std.camId = reg.camId;
                    std.classId = reg.classId;
                    std.stdRollNo = reg.stdRollNo;
                    std.stdStatus = reg.stdStatus;
                    std.UpdatedBy = LoginInfo.UserID;
                    std.UpdatedDate = DateTime.Now;
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Student information updated Successfully";
                    return RedirectToAction("ShowStd");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ShowStd");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Student information not updated Please contact to soft support " + SoftSupport.SoftContactNo + "";
            }
            return RedirectToAction("ShowStd");
        }
        [CheckSession]
        public ActionResult DelStudent(int id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var getstd = con.std.Where(s => s.perId == id).FirstOrDefault();
                    if (getstd != null)
                    {
                        var getperson = con.person.Where(p => p.perId == getstd.perId).FirstOrDefault();
                        var getLogin = con.login.Where(l => l.id == getperson.id).FirstOrDefault();
                        var getPerReg = con.perReg.Where(p => p.perId == getperson.perId).FirstOrDefault();
                        if (getPerReg != null)
                        {
                            con.perReg.Remove(getPerReg);
                        }
                        con.std.Remove(getstd);
                        con.person.Remove(getperson);
                        con.login.Remove(getLogin);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Student information Deleted Successfully";
                        return RedirectToAction("ShowStd");
                    }
                }
                else
                {
                    TempData["Error"] = "Student not deleted. Please contact to soft support " + SoftSupport.SoftContactNo + "";
                }

            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Student cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Student not deleted Please contact to soft support " + SoftSupport.SoftContactNo + "";
                }
            }
            return RedirectToAction("ShowStd");
        }
        [CheckSession]

        public JsonResult SearchStd(string term)
        {

            List<string> msg = new List<string> { "No Record found" };
            //var msg = "No Record Found";
            List<string> AutoStd;
            // AutoStd = con.std.Where(s => s.stdRollNo.StartsWith(term) || s.barcode_number.StartsWith(term)).Select(a => a.item_name).ToList();
            AutoStd = (from p in con.person
                       join s in con.std on p.perId equals s.perId
                       where p.perName.StartsWith(term) || s.stdRollNo.StartsWith(term) || p.perContactOne.StartsWith(term) || p.perContactTwo.StartsWith(term)
                       && s.IsDeleted == false && p.IsDeleted == false
                       select new
                       {
                           p.perName
                       }).Select(p => p.perName).ToList();
            if (AutoStd.Count == 0 || AutoStd == null)
            {
                return Json(msg, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(AutoStd, JsonRequestBehavior.AllowGet);
            }

        }
        [CheckSession]

        public ActionResult SearchedStd(string PopulateStd)
        {
            List<RegViewModel> List = new List<RegViewModel>();
            var std = (from p in con.person
                       join s in con.std on p.perId equals s.perId
                       where s.IsDeleted == false && p.IsDeleted == false && p.perName == PopulateStd
                       select new
                       {
                           p.perId,
                           p.perName,
                           p.perGarName,
                           p.perContactOne,
                           s.stdRollNo,
                           p.perImage
                       }).ToList();
            if (std.Count != 0)
            {
                foreach (var i in std)
                {
                    RegViewModel reg = new RegViewModel();
                    reg.perId = i.perId;
                    reg.perName = i.perName;
                    reg.perGarName = i.perGarName;
                    reg.perContactOne = i.perContactOne;
                    reg.stdRollNo = i.stdRollNo;
                    reg.perImage = i.perImage;
                    List.Add(reg);
                    TempData["List"] = List;
                }
                return View(TempData["List"]);
            }
            else
            {
                TempData["Info"] = "No Record found";
                return View();
            }

        }

        [CheckSession]
        public ActionResult ClassFeePkg()
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            PopulatFeetype();
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ClassFeePkg(ClassFeePkg cfp)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                if (ModelState.IsValid)
                {
                    var chkFee = con.clfpkg.Where(c => c.feeTypeId == cfp.feeTypeId && c.sesId == cfp.sesId && c.secId == cfp.secId && c.classId == cfp.classId && c.IsDeleted == false).Any();

                    if (chkFee == true)
                    {
                        TempData["Info"] = "Class Fee Package of this fee type is already exist!";
                        return View();
                    }
                    else
                    {
                        if (cfp.cfpDis > 100)
                        {
                            TempData["Error"] = "Maximum Discount is 100%";
                            return View();
                        }
                        else
                        {
                            cfp.CreatedBy = LoginInfo.UserID;
                            cfp.CreatedDate = DateTime.Now;
                            cfp.UpdatedBy = 0;
                            //cfp.UpdatedDate = DateTime.Now;
                            cfp.IsDeleted = false;
                            cfp.DeletedBy = 0;
                            //cfp.DeletedDate = DateTime.Now;
                            con.clfpkg.Add(cfp);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Class Fee Package created successfully";
                            return View();
                        }

                    }

                }
                else
                {
                    TempData["Error"] = "Class Fee Package not generated Please contact to soft support " + SoftSupport.SoftContactNo + "";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Class Fee Package not generated Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }

        }

        [CheckSession]
        public ActionResult EditClassFeePkg(int id)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                var getData = con.clfpkg.Where(c => c.cfpId == id && c.IsDeleted == false).FirstOrDefault();
                return View(getData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Class Fee Package not Edited. Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("ClassFeePkg");
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult EditClassFeePkg(int id, ClassFeePkg cl)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                var getData = con.clfpkg.Where(c => c.cfpId == id).FirstOrDefault();
                var ChkFee = con.stdfpkg.Where(s => s.secId == getData.secId && s.sesId == getData.sesId && s.classId == getData.classId && s.feeTypeId == getData.feeTypeId).Any();
                if (ChkFee == false)
                {
                    var chkFee = (from c in con.clfpkg
                                  where c.feeTypeId == cl.feeTypeId && c.sesId == cl.sesId && c.secId == cl.secId && c.classId == cl.classId && c.IsDeleted == false && c.cfpId != id
                                  select new
                                  {
                                      c.classId
                                  }).Any();
                    if (chkFee == true)
                    {
                        TempData["Info"] = "Class Fee Package of this fee type is already exist!";
                        return View();
                    }
                    else
                    {
                        if (cl.cfpDis > 100)
                        {
                            TempData["Error"] = "Maximum Discount is 100%";
                            return View();
                        }
                        else
                        {
                            getData.cfpAmt = cl.cfpAmt;
                            getData.cfpDis = cl.cfpDis;
                            getData.classId = cl.classId;
                            getData.secId = cl.secId;
                            getData.sesId = cl.sesId;
                            getData.UpdatedBy = LoginInfo.UserID;
                            getData.UpdatedDate = DateTime.Now;
                            con.Entry(getData).State = EntityState.Modified;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Class fee package updated successfully";
                            return RedirectToAction("ClassFeePkg");
                        }

                    }
                }
                else
                {
                    TempData["Info"] = "you cannot update this fee package because this fee package is associated with some students.";
                    return RedirectToAction("ClassFeePkg");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fee package cannot updated Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return RedirectToAction("ClassFeePkg");
            }
        }
        [CheckSession]
        public ActionResult StdFeePkg(string RollNo, string btn)
        {
            try
            {
                if (RollNo != null && RollNo != "")
                {
                    if (btn == "Show")
                    {
                        var StdRoll = RollNo;
                        List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
                        List<StdFeeDetail> StdFeDtl = new List<StdFeeDetail>();
                        if (StdRoll != "")
                        {
                            var StdData = (from s in con.std
                                           join p in con.person on s.perId equals p.perId
                                           join ss in con.InstSes on s.sesId equals ss.sesId
                                           join sc in con.InstSec on s.secId equals sc.secId
                                           join cl in con.cls on s.classId equals cl.classId
                                           where s.stdRollNo == StdRoll && s.IsDeleted == false
                                           select new
                                           {
                                               s.stdRollNo,
                                               s.stdId,
                                               s.classId,
                                               s.sesId,
                                               s.secId,
                                               p.perName,
                                               ss.sesName,
                                               sc.secName,
                                               cl.classname,
                                           }).FirstOrDefault();
                            //TempData["RollNo"] = StdData.stdRollNo;
                            if (StdData != null)
                            {
                                ViewBag.StdRoll = StdData.stdRollNo;
                                ViewBag.StdName = StdData.perName;
                                ViewBag.StdClass = StdData.classname;
                                ViewBag.StdSec = StdData.secName;
                                ViewBag.StdSes = StdData.sesName;

                                ///Populate Fee type against Student Roll Number ////
                                var PoplateFeeType = (from s in con.std
                                                      join cf in con.clfpkg on s.classId equals cf.classId
                                                      join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                                      where s.stdRollNo == StdRoll && cf.sesId == StdData.sesId && cf.classId == StdData.classId && cf.secId == StdData.secId && s.IsDeleted == false
                                                      select new
                                                      {
                                                          ft.feeTypeId,
                                                          ft.feeTypeName
                                                      }).ToList();
                                SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                                ViewData["FeeType"] = sl;
                                ///End Populate Fee type against Student Roll Number ////
                                var FeeDtl = (from s in con.std
                                              join cf in con.clfpkg on s.classId equals cf.classId
                                              join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                              where s.stdRollNo == StdRoll && cf.sesId == StdData.sesId && cf.secId == StdData.secId && cf.classId == StdData.classId && s.IsDeleted == false
                                              select new
                                              {
                                                  cf.cfpAmt,
                                                  cf.cfpDis,
                                                  ft.feeTypeName
                                              }).ToList();
                                foreach (var i in FeeDtl)
                                {
                                    StdFeeDetail sf = new StdFeeDetail();
                                    sf.FeeName = i.feeTypeName;
                                    sf.FeeAmt = i.cfpAmt;
                                    sf.Dis = i.cfpDis;
                                    StdFee.Add(sf);
                                }

                                TempData["FeeDetail"] = StdFee;

                                var FeDtl = (from s in con.std
                                             join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                             join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                             where s.stdRollNo == StdRoll && sf.sesId == StdData.sesId && sf.secId == StdData.secId && sf.classId == StdData.classId && s.IsDeleted == false
                                             select new
                                             {
                                                 sf.sfpId,
                                                 sf.sfpAmt,
                                                 sf.sfpDis,
                                                 ft.feeTypeId,
                                                 ft.feeTypeName
                                             }).ToList();
                                if (FeDtl.Count != 0)
                                {
                                    foreach (var i in FeDtl)
                                    {
                                        StdFeeDetail sf = new StdFeeDetail();
                                        sf.sfpId = i.sfpId;
                                        sf.FeeName = i.feeTypeName;
                                        sf.FeeAmt = i.sfpAmt;
                                        sf.Dis = i.sfpDis;
                                        StdFeDtl.Add(sf);
                                    }
                                    ViewData["FeDetail"] = StdFeDtl;
                                }
                                return View();
                            }
                            else
                            {
                                TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                                return RedirectToAction("StdFeePkg");
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll No";
                            return RedirectToAction("StdFeePkg");
                        }
                    }
                }

            }
            catch (Exception ex)
            {


            }
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult StdFeePkg(StdFeeDetail sfd, string StdRoll, string submitButton,
            string FeeDis, string DicOpt, string RollNo)
        {
            List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
            List<StdFeeDetail> StdFeDtl = new List<StdFeeDetail>();

            try
            {
                if (submitButton == "Show")
                {
                    if (StdRoll != "")
                    {
                        var StdData = (from s in con.std
                                       join p in con.person on s.perId equals p.perId
                                       join ss in con.InstSes on s.sesId equals ss.sesId
                                       join sc in con.InstSec on s.secId equals sc.secId
                                       join cl in con.cls on s.classId equals cl.classId
                                       where s.stdRollNo == StdRoll && s.IsDeleted == false
                                       select new
                                       {
                                           s.stdRollNo,
                                           s.stdId,
                                           s.classId,
                                           s.sesId,
                                           s.secId,
                                           p.perName,
                                           ss.sesName,
                                           sc.secName,
                                           cl.classname,
                                       }).FirstOrDefault();
                        //TempData["RollNo"] = StdData.stdRollNo;
                        if (StdData != null)
                        {
                            ViewBag.StdRoll = StdData.stdRollNo;
                            ViewBag.StdName = StdData.perName;
                            ViewBag.StdClass = StdData.classname;
                            ViewBag.StdSec = StdData.secName;
                            ViewBag.StdSes = StdData.sesName;

                            ///Populate Fee type against Student Roll Number ////
                            var PoplateFeeType = (from s in con.std
                                                  join cf in con.clfpkg on s.classId equals cf.classId
                                                  join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                                  where s.stdRollNo == StdRoll && cf.sesId == StdData.sesId && cf.classId == StdData.classId && cf.secId == StdData.secId && s.IsDeleted == false && cf.IsDeleted == false
                                                  select new
                                                  {
                                                      ft.feeTypeId,
                                                      ft.feeTypeName
                                                  }).ToList();
                            SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                            ViewData["FeeType"] = sl;
                            ///End Populate Fee type against Student Roll Number ////
                            var FeeDtl = (from s in con.std
                                          join cf in con.clfpkg on s.classId equals cf.classId
                                          join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                          where s.stdRollNo == StdRoll && cf.sesId == StdData.sesId && cf.secId == StdData.secId && cf.classId == StdData.classId && s.IsDeleted == false && cf.IsDeleted == false
                                          select new
                                          {
                                              cf.cfpAmt,
                                              cf.cfpDis,
                                              ft.feeTypeName
                                          }).ToList();
                            foreach (var i in FeeDtl)
                            {
                                StdFeeDetail sf = new StdFeeDetail();
                                sf.FeeName = i.feeTypeName;
                                sf.FeeAmt = i.cfpAmt;
                                sf.Dis = i.cfpDis;
                                StdFee.Add(sf);
                            }

                            TempData["FeeDetail"] = StdFee;

                            var FeDtl = (from s in con.std
                                         join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                         join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                         where s.stdRollNo == StdRoll && sf.sesId == StdData.sesId && sf.secId == StdData.secId && sf.classId == StdData.classId && s.IsDeleted == false && sf.IsDeleted == false
                                         select new
                                         {
                                             sf.sfpId,
                                             sf.sfpAmt,
                                             sf.sfpDis,
                                             ft.feeTypeId,
                                             ft.feeTypeName
                                         }).ToList();
                            if (FeDtl.Count != 0)
                            {
                                foreach (var i in FeDtl)
                                {
                                    StdFeeDetail sf = new StdFeeDetail();
                                    sf.sfpId = i.sfpId;
                                    sf.FeeName = i.feeTypeName;
                                    sf.FeeAmt = i.sfpAmt;
                                    sf.Dis = i.sfpDis;
                                    StdFeDtl.Add(sf);
                                }
                                ViewData["FeDetail"] = StdFeDtl;
                            }



                            return View();
                        }
                        else
                        {
                            TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                            return RedirectToAction("StdFeePkg");
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Enter Roll No";
                        return RedirectToAction("StdFeePkg");
                    }
                }
                else
                {
                    var btn = "Show";
                    StdFeePkg AdFee = new StdFeePkg();
                    var GetFee = (from s in con.std
                                  join p in con.person on s.perId equals p.perId
                                  join ss in con.InstSes on s.sesId equals ss.sesId
                                  join sc in con.InstSec on s.secId equals sc.secId
                                  join cl in con.cls on s.classId equals cl.classId
                                  join cf in con.clfpkg on s.classId equals cf.classId
                                  join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                  where s.stdRollNo == RollNo && s.sesId == cf.sesId && s.classId == cf.classId && s.secId == cf.secId && ft.feeTypeId == sfd.feeTypeId && s.IsDeleted == false && cf.IsDeleted == false
                                  select new
                                  {
                                      s.stdId,
                                      ss.sesId,
                                      sc.secId,
                                      cl.classId,
                                      cf.cfpAmt,
                                      cf.cfpDis,
                                      ft.feeTypeName
                                  }).FirstOrDefault();
                    TempData["RollNo"] = RollNo;
                    var chkfee = (from s in con.std
                                  join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                  where s.stdRollNo == RollNo && s.stdId == GetFee.stdId && sf.sesId == GetFee.sesId && sf.secId == GetFee.secId && sf.classId == GetFee.classId && sf.feeTypeId == sfd.feeTypeId
                                  && s.IsDeleted == false && sf.IsDeleted == false
                                  select new
                                  {
                                      sf.feeTypeId
                                  }).FirstOrDefault();

                    if (chkfee != null)
                    {
                        TempData["Info"] = "This Fee Package is already created against this roll Number  '" + RollNo + "'";

                        return RedirectToAction("StdFeePkg", new { RollNo, btn });
                    }
                    else
                    {
                        if (GetFee != null)
                        {
                            if (FeeDis == "")
                            {
                                AdFee.sfpDis = 0;
                                AdFee.sfpAmt = GetFee.cfpAmt;
                            }
                            else
                            {
                                if (DicOpt == "InAmount")
                                {
                                    var MxDisc = Convert.ToInt32(GetFee.cfpAmt) * Convert.ToInt32(GetFee.cfpDis) / 100;
                                    if (Convert.ToInt32(FeeDis) > Convert.ToInt32(MxDisc))
                                    {
                                        TempData["Error"] = "Discount must be less than or equal to " + MxDisc + "/Rs.";

                                        return RedirectToAction("StdFeePkg", new { RollNo, btn });

                                    }
                                    else
                                    {
                                        AdFee.sfpDis = Convert.ToInt32(FeeDis);
                                        AdFee.sfpAmt = GetFee.cfpAmt - AdFee.sfpDis;
                                    }
                                }
                                else if (DicOpt == "InPercent")
                                {
                                    if (Convert.ToInt32(FeeDis) > Convert.ToInt32(GetFee.cfpDis))
                                    {
                                        TempData["Error"] = "Discount must be less than Max Discount " + GetFee.cfpDis + "%.";

                                        return RedirectToAction("StdFeePkg", new { RollNo, btn });
                                    }
                                    else
                                    {
                                        var Disco = Convert.ToInt32(GetFee.cfpAmt) * Convert.ToInt32(FeeDis) / 100;
                                        var RoundDisc = String.Format("{0:0.00}", Disco);
                                        AdFee.sfpDis = Convert.ToInt32(Disco);
                                        AdFee.sfpAmt = GetFee.cfpAmt - AdFee.sfpDis;
                                    }
                                }
                                else
                                {
                                    AdFee.sfpDis = 0;
                                    AdFee.sfpAmt = GetFee.cfpAmt;
                                }
                            }
                            if (sfd.sfpRemarks == null)
                            {
                                sfd.sfpRemarks = "No Remarks";
                                AdFee.sfpRemarks = sfd.sfpRemarks;
                                AdFee.sfpRemarks = sfd.sfpRemarks;
                                AdFee.secId = GetFee.secId;
                                AdFee.sesId = GetFee.sesId;
                                AdFee.classId = GetFee.classId;
                                AdFee.sfpstdId = GetFee.stdId;
                                AdFee.feeTypeId = sfd.feeTypeId;
                                AdFee.CreatedBy = LoginInfo.UserID;
                                AdFee.CreatedDate = DateTime.Now;
                                AdFee.UpdatedBy = 0;
                                AdFee.IsDeleted = false;
                                AdFee.DeletedBy = 0;
                                con.stdfpkg.Add(AdFee);
                                con.SaveChanges();
                            }
                            else
                            {
                                AdFee.sfpRemarks = sfd.sfpRemarks;
                                AdFee.secId = GetFee.secId;
                                AdFee.sesId = GetFee.sesId;
                                AdFee.classId = GetFee.classId;
                                AdFee.sfpstdId = GetFee.stdId;
                                AdFee.feeTypeId = sfd.feeTypeId;
                                AdFee.CreatedBy = LoginInfo.UserID;
                                AdFee.CreatedDate = DateTime.Now;
                                AdFee.UpdatedBy = 0;
                                AdFee.IsDeleted = false;
                                AdFee.DeletedBy = 0;
                                con.stdfpkg.Add(AdFee);
                                con.SaveChanges();
                            }

                            TempData["SuccessMessage"] = "Student Fee Package generated successfully";
                            return RedirectToAction("StdFeePkg", new { RollNo, btn });
                        }
                        else
                        {
                            TempData["Error"] = "No record Found against this roll Number  '" + RollNo + "' and Fee type";
                            return RedirectToAction("StdFeePkg");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some issue to generate fee package";
            }
            return RedirectToAction("StdFeePkg");
        }

        [CheckSession]
        public ActionResult EditStdFeePkg(int id, string RollNo, string Page)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                var getData = con.stdfpkg.Where(c => c.sfpId == id).FirstOrDefault();
                var ChkFee = con.stdfee.Where(s => s.secId == getData.secId && s.stdId == getData.sfpstdId && s.sesId == getData.sesId && s.classId == getData.classId && s.feeTypeId == getData.feeTypeId && s.IsDeleted == false).FirstOrDefault();
                if (ChkFee == null)
                {
                    var getMaxDic = con.clfpkg.Where(s => s.secId == getData.secId && s.sesId == getData.sesId && s.classId == getData.classId && s.feeTypeId == getData.feeTypeId && s.IsDeleted == false).FirstOrDefault();
                    var MaxDisc = Convert.ToInt32(getMaxDic.cfpAmt) * Convert.ToInt32(getMaxDic.cfpDis) / 100;
                    TempData["Disc"] = MaxDisc;
                    return View(getData);
                }
                else
                {
                    TempData["Error"] = "You cannot update this fee package because this Fee is already Paid.";
                    if (Page == "Package")
                    {
                        var btn = "Show";
                        return RedirectToAction("StdFeePkg", new { RollNo, btn });
                    }
                    else
                    {
                        var btn = "Show";
                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                    }

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult EditStdFeePkg(int id, StdFeePkg cl, string Dis, string RollNo, string Page)
        {
            try
            {
                int Discount = Convert.ToInt32(Dis);
                if (Discount >= cl.sfpDis)
                {
                    PopulatClass();
                    PopulatSec();
                    PopulatSes();
                    PopulatFeetype();
                    var getData = con.stdfpkg.Where(c => c.sfpId == id).FirstOrDefault();
                    var ChkFee = con.stdfee.Where(s => s.secId == getData.secId && s.stdId == getData.sfpstdId && s.sesId == getData.sesId && s.classId == getData.classId && s.feeTypeId == getData.feeTypeId && s.IsDeleted == false).FirstOrDefault();
                    if (ChkFee == null)
                    {
                        var GetOrgFeeAmount = con.clfpkg.Where(s => s.secId == getData.secId && s.sesId == getData.sesId && s.classId == getData.classId && s.feeTypeId == getData.feeTypeId && s.IsDeleted == false).FirstOrDefault();
                        getData.sfpAmt = GetOrgFeeAmount.cfpAmt - cl.sfpDis;
                        getData.sfpDis = cl.sfpDis;
                        con.Entry(getData).State = EntityState.Modified;
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Student fee package updated successfully";
                        if (Page == "Package")
                        {
                            var btn = "Show";
                            return RedirectToAction("StdFeePkg", new { RollNo, btn });
                        }
                        else
                        {
                            var btn = "Show";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                    }
                    else
                    {
                        TempData["Error"] = "You cannot update this fee package.";
                        if (Page == "Package")
                        {
                            var btn = "Show";
                            return RedirectToAction("StdFeePkg", new { RollNo, btn });
                        }
                        else
                        {
                            var btn = "Show";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Discount must be less than or equal to  " + Discount + "/Rs.!";
                    TempData["Disc"] = Discount;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error!";
                return View(); throw;
            }
        }

        [CheckSession]
        public ActionResult BulkStdFeePkg()
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult BulkStdFeePkg(StdFeeDetail sfd, string submitButton, string FeeDis, string DicOpt)
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
            try
            {
                if (submitButton == "Show")
                {
                    var FeeDtl = (from cf in con.clfpkg
                                  join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                  where sfd.classId == cf.classId && sfd.sesId == cf.sesId && sfd.secId == cf.secId && cf.IsDeleted == false
                                  select new
                                  {
                                      cf.cfpAmt,
                                      cf.cfpDis,
                                      ft.feeTypeName
                                  }).ToList();
                    if (FeeDtl.Count == 0)
                    {
                        TempData["Error"] = "There is no Class Fee Package exist!";
                        return View();
                    }
                    else
                    {
                        foreach (var i in FeeDtl)
                        {
                            StdFeeDetail sf = new StdFeeDetail();
                            sf.FeeName = i.feeTypeName;
                            sf.FeeAmt = i.cfpAmt;
                            sf.Dis = i.cfpDis;
                            StdFee.Add(sf);
                        }
                        TempData["FeeDetail"] = StdFee;
                        ///Populate Fee type against Class ////
                        var PoplateFeeType = (from cf in con.clfpkg
                                              join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                              where sfd.classId == cf.classId && sfd.sesId == cf.sesId && sfd.secId == cf.secId && cf.IsDeleted == false
                                              select new
                                              {
                                                  ft.feeTypeName,
                                                  ft.feeTypeId
                                              }).ToList();
                        SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                        ViewData["FeeType"] = sl;
                        ///End Populate Fee type against Student Roll Number ////
                        return View(TempData["FeeDetail"]);
                    }
                }
                else
                {
                    StdFeePkg AdFee = new StdFeePkg();
                    var GetFee = (from s in con.std
                                  join p in con.person on s.perId equals p.perId
                                  join cf in con.clfpkg on s.classId equals cf.classId
                                  join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                  where ft.feeTypeId == sfd.feeTypeId && s.classId == sfd.classId && s.sesId == sfd.sesId && s.secId == sfd.secId
                                   && cf.classId == sfd.classId && cf.sesId == sfd.sesId && cf.secId == sfd.secId && s.IsDeleted == false && cf.IsDeleted == false
                                  select new
                                  {
                                      s.stdId,
                                      s.sesId,
                                      s.secId,
                                      s.classId,
                                      cf.cfpAmt,
                                      cf.cfpDis,
                                      ft.feeTypeName
                                  }).FirstOrDefault();


                    var StdId = (from sa in con.std
                                 where !con.stdfpkg
                                          .Any(o => o.sfpstdId == sa.stdId && o.feeTypeId == sfd.feeTypeId && o.classId == sfd.classId && o.sesId == sfd.sesId && o.secId == sfd.secId && o.IsDeleted == false)
                                 where sa.classId == sfd.classId && sa.sesId == sfd.sesId && sa.secId == sfd.secId && sa.IsDeleted == false

                                 select new
                                 {
                                     sa.stdId
                                 }).ToList();


                    if (StdId.Count == 0)
                    {
                        TempData["Info"] = "All Student Fee Package already created against this fee type";
                        return View(TempData["FeeDetail"]);
                    }
                    else
                    {
                        foreach (var i in StdId)
                        {
                            if (GetFee != null)
                            {
                                if (FeeDis == "")
                                {
                                    AdFee.sfpDis = 0;
                                    AdFee.sfpAmt = GetFee.cfpAmt;
                                }
                                else
                                {
                                    if (DicOpt == "InAmount")
                                    {
                                        var MxDisc = Convert.ToInt32(GetFee.cfpAmt) * Convert.ToInt32(GetFee.cfpDis) / 100;
                                        if (Convert.ToInt32(FeeDis) > Convert.ToInt32(MxDisc))
                                        {
                                            TempData["Error"] = "Discount must be less than or equal " + MxDisc + "/Rs.";
                                            return View(TempData["FeeDetail"]);

                                        }
                                        else
                                        {
                                            AdFee.sfpDis = Convert.ToInt32(FeeDis);
                                            AdFee.sfpAmt = GetFee.cfpAmt - AdFee.sfpDis;
                                        }
                                    }
                                    else if (DicOpt == "InPercent")
                                    {
                                        if (Convert.ToInt32(FeeDis) > Convert.ToInt32(GetFee.cfpDis))
                                        {
                                            TempData["Error"] = "Discount must be less than Max Discount " + GetFee.cfpDis + "%.";
                                            return View(TempData["FeeDetail"]);
                                        }
                                        else
                                        {
                                            var Disco = Convert.ToInt32(GetFee.cfpAmt) * Convert.ToInt32(FeeDis) / 100;
                                            var RoundDisc = String.Format("{0:0.00}", Disco);
                                            AdFee.sfpDis = Disco;
                                            AdFee.sfpAmt = GetFee.cfpAmt - AdFee.sfpDis;
                                        }
                                    }
                                    else
                                    {
                                        AdFee.sfpDis = 0;
                                        AdFee.sfpAmt = GetFee.cfpAmt;
                                    }
                                }
                                if (sfd.sfpRemarks == null)
                                {
                                    sfd.sfpRemarks = "No Remarks";
                                    AdFee.sfpRemarks = sfd.sfpRemarks;
                                    AdFee.sfpRemarks = sfd.sfpRemarks;
                                    AdFee.secId = GetFee.secId;
                                    AdFee.sesId = GetFee.sesId;
                                    AdFee.classId = GetFee.classId;
                                    AdFee.sfpstdId = i.stdId;
                                    AdFee.feeTypeId = sfd.feeTypeId;
                                    AdFee.CreatedBy = LoginInfo.UserID;
                                    AdFee.CreatedDate = DateTime.Now;
                                    AdFee.UpdatedBy = 0;
                                    AdFee.IsDeleted = false;
                                    AdFee.DeletedBy = 0;
                                    con.stdfpkg.Add(AdFee);
                                    con.SaveChanges();
                                }
                                else
                                {
                                    AdFee.sfpRemarks = sfd.sfpRemarks;
                                    AdFee.secId = GetFee.secId;
                                    AdFee.sesId = GetFee.sesId;
                                    AdFee.classId = GetFee.classId;
                                    AdFee.sfpstdId = i.stdId;
                                    AdFee.feeTypeId = sfd.feeTypeId;
                                    AdFee.CreatedBy = LoginInfo.UserID;
                                    AdFee.CreatedDate = DateTime.Now;
                                    AdFee.UpdatedBy = 0;
                                    AdFee.IsDeleted = false;
                                    AdFee.DeletedBy = 0;
                                    con.stdfpkg.Add(AdFee);
                                    con.SaveChanges();
                                }
                            }
                            else
                            {
                                TempData["Error"] = "No record Found against this Fee type";
                                return View(TempData["FeeDetail"]);
                            }
                        }
                    }

                    TempData["SuccessMessage"] = "Student Fee Package created successfully";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Fee Package not created";
            }
            return RedirectToAction("BulkStdFeePkg");
        }
        [CheckSession]
        public ActionResult ReceiveStdFee(string RollNo, string btn)
        {
            if (RollNo != null && RollNo != "")
            {
                if (btn == "Show")
                {
                    var StdRoll = RollNo;
                    List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
                    List<StdFeeDetail> StdStatus = new List<StdFeeDetail>();
                    if (btn == "Show")
                    {
                        var StdData = (from s in con.std
                                       join p in con.person on s.perId equals p.perId
                                       join ss in con.InstSes on s.sesId equals ss.sesId
                                       join sc in con.InstSec on s.secId equals sc.secId
                                       join cl in con.cls on s.classId equals cl.classId
                                       where s.stdRollNo == StdRoll && s.IsDeleted == false
                                       select new
                                       {
                                           s.stdRollNo,
                                           s.stdId,
                                           s.classId,
                                           s.sesId,
                                           s.secId,
                                           p.perName,
                                           ss.sesName,
                                           ss.sesStatus,
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
                            /// Populate Fee type against Student Roll Number ////
                            var PoplateFeeType = (from s in con.std
                                                  join sf in con.stdfpkg on s.classId equals sf.classId
                                                  join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                                  where s.stdRollNo == StdRoll && sf.sfpstdId == StdData.stdId && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId
                                                  select new
                                                  {
                                                      ft.feeTypeId,
                                                      ft.feeTypeName
                                                  }).ToList();
                            SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                            ViewData["FeeType"] = sl;
                            //// End Populate Fee type against Student Roll Number ////

                            var FeeDtl = (from s in con.std
                                          join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                          join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                          where s.stdRollNo == StdRoll && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId && s.IsDeleted == false
                                          select new
                                          {
                                              sf.sfpId,
                                              sf.sfpAmt,
                                              sf.sfpDis,
                                              ft.feeTypeId,
                                              ft.feeTypeName
                                          }).ToList();
                            if (FeeDtl.Count != 0)
                            {
                                foreach (var i in FeeDtl)
                                {
                                    StdFeeDetail sf = new StdFeeDetail();
                                    sf.sfpId = i.sfpId;
                                    sf.FeeName = i.feeTypeName;
                                    sf.FeeAmt = i.sfpAmt;
                                    sf.Dis = i.sfpDis;
                                    StdFee.Add(sf);
                                }
                                ViewData["FeeDetail"] = StdFee;

                                ///Fee Status Logic ///

                                var Date = DateTime.Now;
                                var Month = Date.Month;
                                var year = Date.Year;
                                var FeeStatus = (from s in con.std
                                                 join sf in con.stdfee on s.stdId equals sf.stdId
                                                 join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                                 where s.stdRollNo == StdRoll && sf.paidDate.Month == Month && sf.paidDate.Year == year && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId && s.IsDeleted == false
                                                 select new
                                                 {
                                                     sf.feeTypeId,
                                                     sf.feeId,
                                                     sf.feeAmount,
                                                     sf.PandingAmount,
                                                     sf.feeStatus,
                                                     sf.paidDate,
                                                     ft.feeTypeName
                                                 }).ToList();
                                if (FeeStatus.Count == 0)
                                {
                                    foreach (var i in FeeDtl)
                                    {
                                        StdFeeDetail ss = new StdFeeDetail();
                                        ss.FeePaidName = i.feeTypeName;
                                        ss.PaidAmt = i.sfpAmt;
                                        ss.FeePaidStatus = "Unpaid";
                                        StdStatus.Add(ss);
                                    }
                                    TempData["StdFeeStatus"] = StdStatus;
                                }
                                else
                                {
                                    foreach (var i in FeeStatus)
                                    {
                                        StdFeeDetail sf = new StdFeeDetail();
                                        sf.feeTypeId = i.feeTypeId;
                                        sf.feeId = i.feeId;
                                        sf.FeePaidName = i.feeTypeName;
                                        sf.PaidAmt = i.feeAmount;
                                        sf.Panding = i.PandingAmount;
                                        sf.FeePaidStatus = i.feeStatus;
                                        sf.FeeDate = i.paidDate;
                                        StdStatus.Add(sf);
                                    }
                                    foreach (var s in FeeDtl)
                                    {
                                        var chkFeeId = StdStatus.Where(sa => sa.feeTypeId == s.feeTypeId).FirstOrDefault();
                                        if (chkFeeId == null)
                                        {
                                            StdFeeDetail ss = new StdFeeDetail();
                                            ss.FeePaidName = s.feeTypeName;
                                            ss.PaidAmt = s.sfpAmt;
                                            ss.FeePaidStatus = "Unpaid";
                                            StdStatus.Add(ss);
                                        }
                                    }
                                    TempData["StdFeeStatus"] = StdStatus;

                                    //Check Partial Fee
                                    var chkPartialFee = con.stdfee.Where(s => s.feeStatus == "Partial"
                                    && s.stdId == StdData.stdId).Any();
                                    if (chkPartialFee == true)
                                    {
                                        TempData["PartialFee"] = chkPartialFee;
                                    }


                                    return View();
                                }
                            }
                            else
                            {
                                TempData["Error"] = "There is no Fee Package created against this Roll Number. please create Fee Package first";
                                return View(ViewData["FeeDetail"]);
                            }
                        }

                        else
                        {
                            TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                            return View("ReceiveStdFee");
                        }
                    }
                }
            }


            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult ReceiveStdFee(StdFeeDetail sd, string submitButton, string StdRoll,
            string RollNo)
        {
            int RoleId = Convert.ToInt32(Session["RoleId"]);
            List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
            List<StdFeeDetail> StdStatus = new List<StdFeeDetail>();
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
                                       s.stdRollNo,
                                       s.stdId,
                                       s.classId,
                                       s.sesId,
                                       s.secId,
                                       p.perName,
                                       ss.sesName,
                                       ss.sesStatus,
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
                        /// Populate Fee type against Student Roll Number ////
                        var PoplateFeeType = (from s in con.std
                                              join sf in con.stdfpkg on s.classId equals sf.classId
                                              join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                              where s.stdRollNo == StdRoll && sf.sfpstdId == StdData.stdId && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId
                                              select new
                                              {
                                                  ft.feeTypeId,
                                                  ft.feeTypeName
                                              }).ToList();
                        SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                        ViewData["FeeType"] = sl;
                        //// End Populate Fee type against Student Roll Number ////

                        var FeeDtl = (from s in con.std
                                      join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                      join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                      where s.stdRollNo == StdRoll && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId && s.IsDeleted == false
                                      select new
                                      {
                                          sf.sfpId,
                                          sf.sfpAmt,
                                          sf.sfpDis,
                                          ft.feeTypeId,
                                          ft.feeTypeName
                                      }).ToList();
                        if (FeeDtl.Count != 0)
                        {
                            foreach (var i in FeeDtl)
                            {
                                StdFeeDetail sf = new StdFeeDetail();
                                sf.sfpId = i.sfpId;
                                sf.FeeName = i.feeTypeName;
                                sf.FeeAmt = i.sfpAmt;
                                sf.Dis = i.sfpDis;
                                StdFee.Add(sf);
                            }
                            ViewData["FeeDetail"] = StdFee;

                            ///Fee Status Logic ///

                            var Date = DateTime.Now;
                            var Month = Date.Month;
                            var year = Date.Year;
                            var FeeStatus = (from s in con.std
                                             join sf in con.stdfee on s.stdId equals sf.stdId
                                             join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                             where s.stdRollNo == StdRoll && s.stdId == StdData.stdId && sf.paidDate.Month == Month && sf.paidDate.Year == year && sf.sesId == StdData.sesId && sf.classId == StdData.classId && sf.secId == StdData.secId && s.IsDeleted == false
                                             select new
                                             {
                                                 sf.feeTypeId,
                                                 sf.feeId,
                                                 sf.feeAmount,
                                                 sf.PandingAmount,
                                                 sf.feeStatus,
                                                 sf.paidDate,
                                                 ft.feeTypeName
                                             }).ToList();
                            if (FeeStatus.Count == 0)
                            {
                                foreach (var i in FeeDtl)
                                {
                                    StdFeeDetail ss = new StdFeeDetail();
                                    ss.FeePaidName = i.feeTypeName;
                                    ss.PaidAmt = i.sfpAmt;
                                    ss.FeePaidStatus = "Unpaid";
                                    StdStatus.Add(ss);
                                }
                                TempData["StdFeeStatus"] = StdStatus;
                            }
                            else
                            {
                                foreach (var i in FeeStatus)
                                {
                                    StdFeeDetail sf = new StdFeeDetail();
                                    sf.feeTypeId = i.feeTypeId;
                                    sf.feeId = i.feeId;
                                    sf.FeePaidName = i.feeTypeName;
                                    sf.PaidAmt = i.feeAmount;
                                    sf.Panding = i.PandingAmount;
                                    sf.FeePaidStatus = i.feeStatus;
                                    sf.FeeDate = i.paidDate;
                                    StdStatus.Add(sf);
                                }
                                foreach (var s in FeeDtl)
                                {
                                    var chkFeeId = StdStatus.Where(sa => sa.feeTypeId == s.feeTypeId).FirstOrDefault();
                                    if (chkFeeId == null)
                                    {
                                        StdFeeDetail ss = new StdFeeDetail();
                                        ss.FeePaidName = s.feeTypeName;
                                        ss.PaidAmt = s.sfpAmt;
                                        ss.FeePaidStatus = "Unpaid";
                                        StdStatus.Add(ss);
                                    }
                                }

                                TempData["StdFeeStatus"] = StdStatus;

                                //Check Partial Fee
                                var chkPartialFee = con.stdfee.Where(s => s.feeStatus == "Partial"
                                && s.stdId == StdData.stdId).Any();
                                if (chkPartialFee == true)
                                {
                                    TempData["PartialFee"] = chkPartialFee;
                                }


                                return View();
                            }
                        }
                        else
                        {
                            TempData["Error"] = "There is no Fee Package created against this Roll Number. please create Fee Package first";
                            return View(ViewData["FeeDetail"]);
                        }
                    }

                    else
                    {
                        TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
                        return RedirectToAction("ReceiveStdFee");
                    }
                }
                else
                {
                    var btn = "Show";
                    StudentFee AdFee = new StudentFee();
                    var GetFee = (from s in con.std
                                  join p in con.person on s.perId equals p.perId
                                  join ss in con.InstSes on s.sesId equals ss.sesId
                                  join sc in con.InstSec on s.secId equals sc.secId
                                  join cl in con.cls on s.classId equals cl.classId
                                  join sf in con.stdfpkg on s.stdId equals sf.sfpstdId
                                  join ft in con.feetype on sf.feeTypeId equals ft.feeTypeId
                                  where s.stdRollNo == RollNo && s.sesId == sf.sesId && s.classId == sf.classId && s.secId == sf.secId && ft.feeTypeId == sd.feeTypeId && s.IsDeleted == false
                                  select new
                                  {
                                      s.stdId,
                                      p.perId,
                                      p.perName,
                                      p.perContactOne,
                                      ss.sesId,
                                      ss.sesStatus,
                                      sc.secId,
                                      cl.classId,
                                      sf.sfpAmt,
                                      sf.sfpDis,
                                      ft.feeTypeName
                                  }).FirstOrDefault();
                    var chkfee = (from s in con.std
                                  join sf in con.stdfee on s.stdId equals sf.stdId
                                  where s.stdRollNo == RollNo && sf.sesId == GetFee.sesId && sf.classId == GetFee.classId && sf.secId == GetFee.secId
                                  && sf.feeTypeId == sd.feeTypeId && sf.feeStatus == "Paid"
                                  && sf.paidDate.Month == sd.FeeDate.Month && sf.paidDate.Year == sd.FeeDate.Year
                                  select new
                                  {
                                      sf.feeTypeId,
                                  }).FirstOrDefault();
                    var chkfeeStatus = (from s in con.std
                                        join sf in con.stdfee on s.stdId equals sf.stdId
                                        where sf.stdId == GetFee.stdId && sf.sesId == GetFee.sesId && sf.classId == GetFee.classId && sf.secId == GetFee.secId && sf.feeTypeId == sd.feeTypeId && sf.feeStatus == "Partial"
                                        && sf.paidDate.Month == sd.FeeDate.Month && sf.paidDate.Year == sd.FeeDate.Year
                                        select new
                                        {
                                            sf.feeTypeId,

                                        }).FirstOrDefault();
                    if (chkfeeStatus == null)
                    {
                        if (chkfee != null)
                        {
                            TempData["Info"] = "This Month Fees is already paid against this roll Number  '" + RollNo + "'";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                        else
                        {
                            if (GetFee != null)
                            {
                                if (GetFee.sfpAmt < sd.FeeAmt)
                                {
                                    TempData["Error"] = "Paid Fee amount must not be greater than Fee Amount Please check the Fee Amount!";
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else if (GetFee.sfpAmt > sd.FeeAmt)
                                {
                                    AdFee.feeAmount = sd.FeeAmt;
                                    AdFee.PandingAmount = GetFee.sfpAmt - sd.FeeAmt;
                                    AdFee.paidDate = sd.FeeDate;
                                    AdFee.feeStatus = "Partial";
                                }
                                else
                                {
                                    AdFee.feeAmount = sd.FeeAmt;
                                    AdFee.PandingAmount = GetFee.sfpAmt - sd.FeeAmt;
                                    AdFee.paidDate = sd.FeeDate;
                                    AdFee.feeStatus = "Paid";
                                }
                                if (sd.sfpRemarks == null)
                                {
                                    AdFee.StdRemarks = "No Remarks";
                                }
                                else
                                {
                                    AdFee.StdRemarks = sd.sfpRemarks;
                                }
                                if (RoleId == 1 || RoleId == 10)
                                {
                                    AdFee.EntryLocked = true;
                                }
                                if (RoleId == 2)
                                {
                                    AdFee.EntryLocked = false;
                                }


                                AdFee.secId = GetFee.secId;
                                AdFee.sesId = GetFee.sesId;
                                AdFee.classId = GetFee.classId;
                                AdFee.stdId = GetFee.stdId;
                                AdFee.feeTypeId = sd.feeTypeId;
                                AdFee.CreatedBy = LoginInfo.UserID;
                                AdFee.CreatedDate = DateTime.Now;
                                AdFee.UpdatedBy = 0;
                                AdFee.IsDeleted = false;
                                AdFee.DeletedBy = 0;
                                con.stdfee.Add(AdFee);
                                con.SaveChanges();

                                //For Sending SMS
                                var chkModule = con.smsModule.Where(s => s.mnId == 4 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                                if (chkModule != null)
                                {
                                    EncryptDecrypt decryption = new EncryptDecrypt();
                                    var getAllotment = con.sMSAllotments.FirstOrDefault();
                                    if (getAllotment != null)
                                    {
                                        int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                        DateTime Exdate = Convert.ToDateTime(decryption.Decrypt(getAllotment.saExpiryDate));
                                        var Status = decryption.Decrypt(getAllotment.saStatus);
                                        if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                        {

                                            //Sending SMS
                                            string msgText = "Dear " + GetFee.perName + " " + chkModule.smText + ". Fee Amount " + AdFee.feeAmount + ". Date: " + DateTime.Now;
                                            sendMessage.SendSMSTurab(GetFee.perContactOne, msgText);

                                            SentSMS sms = new SentSMS();
                                            sms.ssDate = DateTime.Now;
                                            sms.ssStatus = true;
                                            sms.perId = GetFee.perId;
                                            sms.ssText = msgText;
                                            con.sentSMs.Add(sms);

                                            //Minus 1 SMS
                                            int msg = RemainingMsg - 1;
                                            var RemMsg = msg.ToString();
                                            getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());

                                            con.SaveChanges();
                                        }
                                    }

                                }


                                TempData["SuccessMessage"] = "Student Fee received successfully";
                                return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                            }
                            else
                            {
                                TempData["Error"] = "No record Found against this roll Number  '" + RollNo + "' and Fee type";
                                return View();
                            }

                        }
                    }
                    else
                    {
                        var StdFeeId = con.stdfee.OrderByDescending(s => s.feeId).Where(s => s.feeTypeId == sd.feeTypeId && s.stdId == GetFee.stdId && s.sesId == GetFee.sesId &&
                        s.classId == GetFee.classId && s.secId == GetFee.secId && s.feeStatus == "Partial").FirstOrDefault();
                        if (GetFee.sfpAmt < sd.FeeAmt + StdFeeId.feeAmount)
                        {
                            TempData["Error"] = "Paid Fee amount must not be greater than Fee Amount Please check the Fee Amount!";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                        else if (GetFee.sfpAmt > sd.FeeAmt + StdFeeId.feeAmount)
                        {
                            StdFeeId.feeAmount = StdFeeId.feeAmount + sd.FeeAmt;
                            StdFeeId.PandingAmount = GetFee.sfpAmt - StdFeeId.feeAmount;
                            StdFeeId.paidDate = sd.FeeDate;
                            StdFeeId.feeStatus = "Partial";
                            StdFeeId.UpdatedBy = LoginInfo.UserID;
                            StdFeeId.UpdatedDate = DateTime.Now;
                            con.SaveChanges();

                            //For Sending SMS
                            var chkModule = con.smsModule.Where(s => s.mnId == 4 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                            if (chkModule != null)
                            {
                                EncryptDecrypt decryption = new EncryptDecrypt();
                                var getAllotment = con.sMSAllotments.FirstOrDefault();
                                if (getAllotment != null)
                                {
                                    int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                    DateTime Exdate = Convert.ToDateTime(decryption.Decrypt(getAllotment.saExpiryDate));
                                    var Status = decryption.Decrypt(getAllotment.saStatus);
                                    if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                    {

                                        //Sending SMS
                                        string msgText = "Mr " + GetFee.perName + " " + chkModule.smText + ". Fee Amount " + sd.FeeAmt + ". Date: " + DateTime.Now;
                                        sendMessage.SendSMSTurab(GetFee.perContactOne, msgText);

                                        SentSMS sms = new SentSMS();
                                        sms.ssDate = DateTime.Now;
                                        sms.ssStatus = true;
                                        sms.perId = GetFee.perId;
                                        sms.ssText = msgText;
                                        con.sentSMs.Add(sms);

                                        //Minus 1 SMS
                                        int msg = RemainingMsg - 1;
                                        var RemMsg = msg.ToString();
                                        getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());

                                        con.SaveChanges();
                                    }
                                }

                            }

                            TempData["SuccessMessage"] = "Student Pending Fee received successfully";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                        else
                        {
                            StdFeeId.feeAmount = StdFeeId.feeAmount + sd.FeeAmt;
                            StdFeeId.PandingAmount = GetFee.sfpAmt - StdFeeId.feeAmount;
                            StdFeeId.paidDate = sd.FeeDate;
                            StdFeeId.feeStatus = "Paid";
                            StdFeeId.UpdatedBy = LoginInfo.UserID;
                            StdFeeId.UpdatedDate = DateTime.Now;
                            con.SaveChanges();

                            //For Sending SMS
                            var chkModule = con.smsModule.Where(s => s.mnId == 4 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                            if (chkModule != null)
                            {
                                EncryptDecrypt decryption = new EncryptDecrypt();
                                var getAllotment = con.sMSAllotments.FirstOrDefault();
                                if (getAllotment != null)
                                {
                                    int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                    DateTime Exdate = Convert.ToDateTime(decryption.Decrypt(getAllotment.saExpiryDate));
                                    var Status = decryption.Decrypt(getAllotment.saStatus);
                                    if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                    {

                                        //Sending SMS
                                        string msgText = "Mr " + GetFee.perName + " " + chkModule.smText + ". Fee Amount " + sd.FeeAmt + ". Date: " + DateTime.Now;
                                        sendMessage.SendSMSTurab(GetFee.perContactOne, msgText);

                                        SentSMS sms = new SentSMS();
                                        sms.ssDate = DateTime.Now;
                                        sms.ssStatus = true;
                                        sms.perId = GetFee.perId;
                                        sms.ssText = msgText;
                                        con.sentSMs.Add(sms);

                                        //Minus 1 SMS
                                        int msg = RemainingMsg - 1;
                                        var RemMsg = msg.ToString();
                                        getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());

                                        con.SaveChanges();
                                    }
                                }

                            }


                            TempData["SuccessMessage"] = "Student Pending Fee received successfully";
                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No record Found against this roll Number  '" + StdRoll + "'";
            }
            return View();
        }

        //Find Partial Fees
        public ActionResult FindPartialFee(string RollNo)
        {
            try
            {
                if (RollNo != null)
                {
                    var getStdId = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStdId != null)
                    {
                        var getPartialFees = con.stdfee.Where(s => s.feeStatus == "Partial" && s.stdId == getStdId.stdId).ToList();
                        if (getPartialFees.Count != 0)
                        {
                            TempData["Partial"] = getPartialFees;
                            return PartialView("_PartialFees");
                        }
                    }
                }
                return PartialView("_PartialFees");
            }
            catch (Exception ex)
            {
                return RedirectToAction("ReceiveStdFee");
            }
        }

        public ActionResult PaidPartialFee(int fee, int feeId)
        {
            try
            {
                if (fee != 0 && feeId != 0)
                {
                    string data = "";
                    var getFeeRecord = con.stdfee.Where(s => s.feeId == feeId).FirstOrDefault();
                    if (getFeeRecord != null)
                    {
                        var chkTotalFee = con.stdfpkg.Where(sf => sf.sfpstdId == getFeeRecord.stdId && sf.sesId == getFeeRecord.sesId
                        && sf.classId == getFeeRecord.classId && sf.secId == getFeeRecord.secId && sf.feeTypeId == getFeeRecord.feeTypeId).FirstOrDefault();
                        if (chkTotalFee != null)
                        {
                            if (chkTotalFee.sfpAmt > getFeeRecord.feeAmount + fee)
                            {
                                getFeeRecord.feeAmount = getFeeRecord.feeAmount + fee;
                                getFeeRecord.PandingAmount = getFeeRecord.PandingAmount - fee;
                                getFeeRecord.paidDate = DateTime.Now;
                                getFeeRecord.UpdatedBy = LoginInfo.UserID;
                                getFeeRecord.UpdatedDate = DateTime.Now;
                                con.SaveChanges();
                                data = "Partial Fee updated successfully.";
                                return Json(new { Success = "true", Data = new { data } });
                            }
                            else if (chkTotalFee.sfpAmt < getFeeRecord.feeAmount + fee)
                            {
                                data = "Entered fee must not be greater than Total Fee.";
                                return Json(new { Success = "true", Data = new { data } });
                            }
                            else
                            {
                                getFeeRecord.feeAmount = getFeeRecord.feeAmount + fee;
                                getFeeRecord.feeStatus = "Paid";
                                getFeeRecord.PandingAmount = getFeeRecord.PandingAmount - fee;
                                getFeeRecord.paidDate = DateTime.Now;
                                getFeeRecord.UpdatedBy = LoginInfo.UserID;
                                getFeeRecord.UpdatedDate = DateTime.Now;
                                con.SaveChanges();
                                data = "Partial Fee received successfully.";
                                return Json(new { Success = "true", Data = new { data } });
                            }
                        }
                    }

                }
                return Json(new { Success = "true", Data = new { } });
            }
            catch (Exception ex)
            {

                return RedirectToAction("ReceiveStdFee");
            }
        }

        //Download Fee Voucher
        public ActionResult DownloadVoucher(int? id, string RollNo, int? Month, int? Year, DateTime Due)
        {
            try
            {
                List<StdFeeDetail> stdList = new List<StdFeeDetail>();
                if (id != null && id != 0)
                {
                    var getFeeDetail = con.stdfpkg.Where(s => s.sfpId == id).FirstOrDefault();
                    if (getFeeDetail != null)
                    {
                        var getTotalfee = con.clfpkg.Where(c => c.sesId == getFeeDetail.sesId && c.classId == getFeeDetail.classId
                        && c.secId == getFeeDetail.secId && c.feeTypeId == getFeeDetail.feeTypeId).FirstOrDefault();
                        if (getTotalfee != null)
                        {
                            TempData["TotalFee"] = getTotalfee.cfpAmt;
                        }
                        var getStdInfo = con.std.Where(s => s.stdId == getFeeDetail.sfpstdId).FirstOrDefault();
                        if (getStdInfo != null)
                        {
                            TempData["Student"] = getStdInfo;
                        }
                        TempData["Voucher"] = getFeeDetail;
                        TempData["Due"] = Due.ToString("dd-MMM-yyyy");

                    }
                    return View();
                }
                if (RollNo != null && RollNo != "")
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo && s.IsDeleted == false).FirstOrDefault();
                    if (getStd != null)
                    {
                        var getFee = con.stdfpkg.Where(s => s.sfpstdId == getStd.stdId && s.sesId == getStd.sesId && s.classId == getStd.classId && s.secId == getStd.secId
                        ).ToList();
                        if (getFee.Count != 0)
                        {
                            foreach (var i in getFee)
                            {
                                StdFeeDetail std = new StdFeeDetail();
                                var getTotalfee = con.clfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId
                                && c.secId == i.secId && c.feeTypeId == i.feeTypeId).FirstOrDefault();
                                if (getTotalfee != null)
                                {
                                    std.TotalFee = getTotalfee.cfpAmt;
                                }
                                std.FeeAmt = i.sfpAmt;
                                std.Fee = i.ft.feeTypeName;
                                std.Dis = i.sfpDis;
                                stdList.Add(std);
                            }
                            TempData["AllReceipt"] = getFee;


                            TempData["AllVoucher"] = stdList;
                            TempData["Std"] = getStd;
                            TempData["FeeMonth"] = "" + Month + "-" + Year + "";
                            TempData["Due"] = Due.ToString("dd-MMM-yyyy");
                            ViewBag.RollNo = getStd.stdRollNo;
                            return View();
                        }


                    }
                    else
                    {
                        TempData["Error"] = "Invalid Roll Number";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number";
                }
                return View();

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [CheckSession]
        public ActionResult _GenVoucher(string SfpIds, string Data)
        {
            try
            {
                List<StdFeeDetail> stdList = new List<StdFeeDetail>();
                if (SfpIds != null)
                {
                    var ObjDetail = JsonConvert.DeserializeObject<List<StdFeePkg>>(SfpIds);
                    var ObjData = JsonConvert.DeserializeObject<StdViewModel>(Data);
                    foreach (var st in ObjDetail)
                    {
                        var getFee = con.stdfpkg.Where(s => s.sfpId == st.sfpId).FirstOrDefault();
                        if (getFee != null)
                        {
                            var getStd = con.std.Where(s => s.stdId == getFee.sfpstdId && s.IsDeleted == false).FirstOrDefault();
                            if (getStd != null)
                            {
                                TempData["Std"] = getStd;
                            }
                            StdFeeDetail std = new StdFeeDetail();
                            var getTotalfee = con.clfpkg.Where(c => c.sesId == getFee.sesId && c.classId == getFee.classId
                            && c.secId == getFee.secId && c.feeTypeId == getFee.feeTypeId).FirstOrDefault();
                            if (getTotalfee != null)
                            {
                                std.TotalFee = getTotalfee.cfpAmt;
                            }
                            std.FeeAmt = getFee.sfpAmt;
                            std.Fee = getFee.ft.feeTypeName;
                            std.Dis = getFee.sfpDis;
                            stdList.Add(std);

                            TempData["AllReceipt"] = getFee;


                            TempData["Voucher"] = stdList;

                            TempData["FeeMonth"] = "" + ObjData.Month + "-" + ObjData.Year + "";
                            TempData["Due"] = ObjData.DueDate.ToString("dd-MMM-yyyy");
                            ViewBag.RollNo = getStd.stdRollNo;

                            //For Bank Info
                            if (ObjData.BankId != 0 && ObjData.BankId != 0000)
                            {
                                var getBankInfo = con.bankinfos.Where(b => b.BankId == ObjData.BankId).FirstOrDefault();
                                if (getBankInfo != null)
                                {
                                    TempData["BankInfo"] = getBankInfo;
                                }
                            }
                        }

                    }

                    return PartialView("_GenVoucher");
                }
                else
                {
                    TempData["Info"] = "No Record Found";
                    return RedirectToAction("FeeVoucher");
                }
            }
            catch (Exception ex)
            {
                TempData["Info"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("FeeVoucher");
            }
        }

        [CheckSession]
        public ActionResult FeeVoucher()
        {
            return View();
        }
        [CheckSession]
        [HttpPost]

        public ActionResult FeeVoucher(string RollNo, int month, int year, DateTime Due)
        {
            try
            {

                if (RollNo != null && RollNo != "")
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo && s.IsDeleted == false).FirstOrDefault();
                    if (getStd != null)
                    {
                        var getFee = con.stdfpkg.Where(s => s.sfpstdId == getStd.stdId && s.sesId == getStd.sesId && s.classId == getStd.classId && s.secId == getStd.secId).ToList();
                        if (getFee.Count != 0)
                        {
                            TempData["AllVoucher"] = getFee;
                            //TempData["Std"] = getStd;
                            ViewBag.RollNo = getStd.stdRollNo;
                            ViewBag.Month = month;
                            ViewBag.Due = Due;
                            ViewBag.Year = year;
                            // PopulatBank();
                            //Populating the dropdown for Bank


                            List<Bank> banklist = new List<Bank>();
                            var getBank = con.banks.Where(b => b.IsVisible == true).ToList();
                            if (getBank.Count != 0)
                            {
                                foreach (var i in getBank)
                                {
                                    var chkbank = con.bankinfos.Where(b => b.BankId == i.BankId && b.IsVisible == true).FirstOrDefault();
                                    if (chkbank != null)
                                    {
                                        Bank bnk = new Bank();
                                        bnk.BankId = i.BankId;
                                        bnk.BankName = i.BankName;
                                        banklist.Add(bnk);
                                    }
                                }
                                SelectList sl = new SelectList(banklist, "BankId", "BankName");
                                ViewData["Bank"] = sl;
                            }

                            return View();
                        }
                        else
                        {
                            TempData["Error"] = "No Fee Package Record Found";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invalid Roll Number";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number";
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [CheckSession]
        public ActionResult BulkFeeVoucher()
        {
            try
            {
                PopulatSes();
                PopulatClass();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult BulkFeeVoucher(ClassFeePkg cls, DateTime Due, string month, string year)
        {
            try
            {
                PopulatSes();
                PopulatClass();
                PopulatSec();
                if (cls.sesId != 0 && cls.secId != 0 && cls.classId != 0)
                {
                    var getClassFeePkg = con.clfpkg.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId).ToList();
                    if (getClassFeePkg.Count != 0)
                    {
                        TempData["Class"] = getClassFeePkg;
                        TempData["FeeMonth"] = "" + month + "-" + year + "";
                        TempData["Due"] = Due.ToString("dd-MMM-yyyy");

                        //Populating the dropdown for Bank


                        List<Bank> banklist = new List<Bank>();
                        var getBank = con.banks.Where(b => b.IsVisible == true).ToList();
                        if (getBank.Count != 0)
                        {
                            foreach (var i in getBank)
                            {
                                var chkbank = con.bankinfos.Where(b => b.BankId == i.BankId && b.IsVisible == true).FirstOrDefault();
                                if (chkbank != null)
                                {
                                    Bank bnk = new Bank();
                                    bnk.BankId = i.BankId;
                                    bnk.BankName = i.BankName;
                                    banklist.Add(bnk);
                                }
                            }
                            SelectList sl = new SelectList(banklist, "BankId", "BankName");
                            ViewData["Bank"] = sl;
                        }
                        return View();
                    }
                    else
                    {
                        TempData["Info"] = "No Class fee package found against this Class Please Create a Class Fee Package for generating Voucher";
                    }
                }
                else
                {
                    TempData["Error"] = "Please select all the parameters";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        [CheckSession]
        public ActionResult _GenBulkVoucher(string Ids, string Data)
        {
            try
            {
                List<ClassFeePkg> ClassFeePkgList = new List<ClassFeePkg>();
                List<StdFeeDetail> FeePkgList = new List<StdFeeDetail>();
                List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                if (Ids != null)
                {
                    var ObjDetail = JsonConvert.DeserializeObject<List<ClassFeePkg>>(Ids);
                    var ObjData = JsonConvert.DeserializeObject<StdViewModel>(Data);

                    foreach (var i in ObjDetail)
                    {
                        ClassFeePkg cls = new ClassFeePkg();
                        var getData = con.clfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId && c.IsDeleted == false).FirstOrDefault();
                        if (getData != null)
                        {
                            ViewBag.ClassId = getData.classId;
                            ViewBag.SesId = getData.sesId;
                            ViewBag.SecId = getData.secId;
                            ClassFeePkgList.Add(getData);
                        }
                    }
                    if (ClassFeePkgList.Count != 0)
                    {
                        int cId = Convert.ToInt32(ViewBag.ClassId);
                        int sId = Convert.ToInt32(ViewBag.SecId);
                        int ssId = Convert.ToInt32(ViewBag.SesId);
                        var getAllStd = con.std.Where(c => c.sesId == ssId && c.classId == cId && c.secId == sId && c.stdStatus == "Active" && c.IsDeleted == false).ToList();
                        if (getAllStd.Count != 0)
                        {
                            foreach (var s in getAllStd)
                            {
                                List<StdFeePkg> StdFeePkgList = new List<StdFeePkg>();
                                foreach (var cl in ClassFeePkgList)
                                {
                                    var getFeePkg = con.stdfpkg.Where(c => c.sesId == cl.sesId && c.classId == cl.classId && c.secId == cl.secId && c.feeTypeId == cl.feeTypeId && c.sfpstdId == s.stdId).FirstOrDefault();
                                    if (getFeePkg != null)
                                    {
                                        StdFeePkgList.Add(getFeePkg);
                                    }
                                }
                                //var getStdFeePkgs = con.stdfpkg.Where(c => c.sesId == cId && c.classId == cId && c.secId == sId && c.sfpstdId == s.stdId).ToList();

                                if (StdFeePkgList.Count != 0)
                                {
                                    foreach (var i in StdFeePkgList)
                                    {
                                        StdFeeDetail stdfee = new StdFeeDetail();
                                        var getTotalFee = ClassFeePkgList.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId).FirstOrDefault();
                                        if (getTotalFee != null)
                                        {
                                            stdfee.stdId = s.stdId;
                                            stdfee.TotalFee = getTotalFee.cfpAmt;
                                            stdfee.FeeAmt = i.sfpAmt;
                                            stdfee.Dis = i.sfpDis;
                                            stdfee.Fee = i.ft.feeTypeName;
                                            stdfee.FeeStatus = "UnPaid";
                                            FeePkgList.Add(stdfee);
                                            TempData["MultiVoucher"] = FeePkgList;
                                        }
                                    }
                                    StdFeeDetail std = new StdFeeDetail();
                                    std.stdId = s.stdId;
                                    std.Stdname = s.pr.perName;
                                    std.FatherName = s.pr.perGarName;
                                    std.Contact = s.pr.perContactOne;
                                    std.RollNo = s.stdRollNo;
                                    std.Ses = s.ses.sesName;
                                    std.Class = s.cls.classname;
                                    std.Sec = s.sec.secName;
                                    StdList.Add(std);
                                    TempData["StdList"] = StdList;
                                }
                            }

                            TempData["FeeMonth"] = ObjData.FeeMonth;
                            TempData["Due"] = ObjData.DueDate.ToString("dd-MMM-yyyy");

                            //For Bank Info
                            if (ObjData.BankId != 0 && ObjData.BankId != 0000)
                            {
                                var getBankInfo = con.bankinfos.Where(b => b.BankId == ObjData.BankId).FirstOrDefault();
                                if (getBankInfo != null)
                                {
                                    TempData["BankInfo"] = getBankInfo;
                                }
                            }


                            return PartialView("_MultipleFeeVoucher");
                        }
                        else
                        {
                            TempData["Error"] = "No Student found of this class";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "No Class Fee Package found of this class.Please create Class Fee Package for generating vouchers";
                        return View();

                    }
                }
                else
                {
                    TempData["Info"] = "No Record Found";
                    return RedirectToAction("BulkFeeVoucher");
                }
            }
            catch (Exception ex)
            {
                TempData["Info"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("BulkFeeVoucher");
            }
        }

        [CheckSession]
        public ActionResult MultipleFeeVoucher()
        {
            try
            {
                PopulatSes();
                PopulatClass();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult MultipleFeeVoucher(ClassFeePkg cls, DateTime Due, string month, string year)
        {
            try
            {
                List<StdFeeDetail> FeePkgList = new List<StdFeeDetail>();
                List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                PopulatSes();
                PopulatClass();
                PopulatSec();
                if (cls.sesId != 0 && cls.secId != 0 && cls.classId != 0)
                {
                    var getAllStd = con.std.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId && c.stdStatus == "Active" && c.IsDeleted == false).ToList();
                    if (getAllStd.Count != 0)
                    {
                        var getClassFeePkg = con.clfpkg.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId).ToList();
                        if (getClassFeePkg.Count != 0)
                        {
                            foreach (var s in getAllStd)
                            {
                                var getStdFeePkgs = con.stdfpkg.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId && c.sfpstdId == s.stdId).ToList();
                                if (getStdFeePkgs.Count != 0)
                                {
                                    foreach (var i in getStdFeePkgs)
                                    {
                                        StdFeeDetail stdfee = new StdFeeDetail();
                                        var getTotalFee = getClassFeePkg.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId).FirstOrDefault();
                                        if (getTotalFee != null)
                                        {
                                            stdfee.stdId = s.stdId;
                                            stdfee.TotalFee = getTotalFee.cfpAmt;
                                            stdfee.FeeAmt = i.sfpAmt;
                                            stdfee.Dis = i.sfpDis;
                                            stdfee.Fee = i.ft.feeTypeName;
                                            stdfee.FeeStatus = "UnPaid";
                                            FeePkgList.Add(stdfee);
                                            TempData["MultiVoucher"] = FeePkgList;

                                        }
                                    }
                                    StdFeeDetail std = new StdFeeDetail();
                                    std.stdId = s.stdId;
                                    std.Stdname = s.pr.perName;
                                    std.FatherName = s.pr.perGarName;
                                    std.Contact = s.pr.perContactOne;
                                    std.RollNo = s.stdRollNo;
                                    std.Ses = s.ses.sesName;
                                    std.Class = s.cls.classname;
                                    std.Sec = s.sec.secName;
                                    StdList.Add(std);
                                    TempData["StdList"] = StdList;
                                }
                            }

                            TempData["FeeMonth"] = "" + month + "-" + year + "";
                            TempData["Due"] = Due.ToString("dd-MMM-yyyy");
                            return PartialView("_MultipleFeeVoucher");
                        }
                        else
                        {
                            TempData["Error"] = "No Class Fee Package found of this class.Please create Class Fee Package for generating vouchers";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "No Student found of this class";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please select the parameters";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        //Download Fee Receipt
        public ActionResult DownloadFeeVoucher(int? id, string RollNo, int? Month, int? Year)
        {
            try
            {
                List<StdFeeDetail> stdList = new List<StdFeeDetail>();
                if (id != null && id != 0)
                {
                    var getFeeDetail = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                    if (getFeeDetail != null)
                    {
                        var getTotalfee = con.clfpkg.Where(c => c.sesId == getFeeDetail.sesId && c.classId == getFeeDetail.classId
                        && c.secId == getFeeDetail.secId && c.feeTypeId == getFeeDetail.feeTypeId).FirstOrDefault();
                        if (getTotalfee != null)
                        {
                            TempData["TotalFee"] = getTotalfee.cfpAmt;
                            var getStdTotalfee = con.stdfpkg.Where(c => c.sesId == getFeeDetail.sesId && c.classId == getFeeDetail.classId
                            && c.secId == getFeeDetail.secId && c.feeTypeId == getFeeDetail.feeTypeId && c.sfpstdId == getFeeDetail.stdId).FirstOrDefault();
                            if (getStdTotalfee != null)
                            {
                                TempData["StdFee"] = getStdTotalfee.sfpAmt;
                                TempData["Discount"] = getStdTotalfee.sfpDis;
                            }
                        }
                        TempData["Voucher"] = getFeeDetail;
                    }
                    return View();
                }
                if (RollNo != null && RollNo != "")
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            //var Date = DateTime.Now;
                            //int Month = Date.Month;
                            //int Year = Date.Year;
                            var getFee = con.stdfee.Where(s => s.stdId == getStd.stdId && s.sesId == getStd.sesId && s.classId == getStd.classId && s.secId == getStd.secId
                            && s.paidDate.Month == Month && s.paidDate.Year == Year).ToList();
                            if (getFee.Count != 0)
                            {
                                foreach (var i in getFee)
                                {
                                    StdFeeDetail std = new StdFeeDetail();
                                    var getTotalfee = con.clfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId
                                    && c.secId == i.secId && c.feeTypeId == i.feeTypeId).FirstOrDefault();
                                    if (getTotalfee != null)
                                    {
                                        std.TotalFee = getTotalfee.cfpAmt;
                                        //TempData["TotalFee"] = getTotalfee.cfpAmt;
                                        var getStdTotalfee = con.stdfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId
                                        && c.secId == i.secId && c.feeTypeId == i.feeTypeId && c.sfpstdId == i.stdId).FirstOrDefault();
                                        if (getStdTotalfee != null)
                                        {
                                            std.Dis = getStdTotalfee.sfpDis;
                                            //TempData["StdFee"] = getStdTotalfee.sfpAmt;
                                            //TempData["Discount"] = getStdTotalfee.sfpDis;
                                        }
                                    }
                                    std.FeeAmt = i.feeAmount;
                                    std.PandingAmt = i.PandingAmount;
                                    std.Fee = i.ft.feeTypeName;
                                    std.FeeStatus = i.feeStatus;
                                    stdList.Add(std);
                                }
                                TempData["AllReceipt"] = stdList;


                                TempData["AllVoucher"] = getFee;
                                TempData["Std"] = getStd;
                                TempData["FeeMonth"] = "" + Month + "-" + Year + "";
                                ViewBag.RollNo = getStd.stdRollNo;
                                return View();
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Student Status is " + getStd.stdStatus + ". so the fee voucher cannot be downloaded!";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invalid Roll Number";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number";
                }
                return View();

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [CheckSession]
        public ActionResult DownloadAllFeeVoucher()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DownloadAllFeeVoucher(string RollNo, int month, int year)
        {
            try
            {

                if (RollNo != null && RollNo != "")
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                    if (getStd != null)
                    {
                        if (getStd.stdStatus == "Active")
                        {
                            //var Date = DateTime.Now;
                            //int Month = Date.Month;
                            //int Year = Date.Year;
                            var getFee = con.stdfee.Where(s => s.stdId == getStd.stdId && s.sesId == getStd.sesId && s.classId == getStd.classId && s.secId == getStd.secId
                            && s.paidDate.Month == month && s.paidDate.Year == year).ToList();
                            if (getFee.Count != 0)
                            {
                                TempData["AllVoucher"] = getFee;
                                TempData["Std"] = getStd;
                                ViewBag.RollNo = getStd.stdRollNo;
                                ViewBag.Month = month;
                                ViewBag.Year = year;
                                return View();
                            }
                            else
                            {
                                TempData["Error"] = "No Record Found of the Month";
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Student Status is " + getStd.stdStatus + ". so the fee voucher cannot be downloaded!";
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Invalid Roll Number";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter Roll Number";
                }
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }




        [CheckSession]
        public ActionResult BulkFeeReceipt()
        {
            try
            {
                PopulatSes();
                PopulatClass();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        [CheckSession]
        [HttpPost]
        public ActionResult BulkFeeReceipt(ClassFeePkg cls, string month, string year)
        {
            try
            {
                PopulatSes();
                PopulatClass();
                PopulatSec();
                if (cls.sesId != 0 && cls.secId != 0 && cls.classId != 0)
                {
                    int Month = Convert.ToInt32(month);
                    int Year = Convert.ToInt32(year);
                    var chkFee = con.stdfee.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId
                    && c.paidDate.Month == Month && c.paidDate.Year == Year).ToList();
                    if (chkFee.Count != 0)
                    {
                        var getClassFeePkg = con.clfpkg.Where(c => c.sesId == cls.sesId && c.classId == cls.classId && c.secId == cls.secId).ToList();
                        if (getClassFeePkg.Count != 0)
                        {
                            TempData["Class"] = getClassFeePkg;
                            TempData["Month"] = "" + month + "";
                            TempData["Year"] = "" + year + "";
                        }
                        else
                        {
                            TempData["Info"] = "No Class fee package found against this Class Please Create a Class Fee Package for generating Voucher";
                        }
                    }
                    else
                    {
                        TempData["Info"] = "No Fee Record found of this month";
                        return View();
                    }

                }
                else
                {
                    TempData["Error"] = "Please select all the parameters";
                    return View();
                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + ".";
                return View();
            }
        }

        [CheckSession]
        public ActionResult _GenBulkReceipt(string Ids, string Data)
        {
            try
            {
                List<ClassFeePkg> ClassFeePkgList = new List<ClassFeePkg>();
                List<StdFeeDetail> FeePkgList = new List<StdFeeDetail>();
                List<StdFeeDetail> StdList = new List<StdFeeDetail>();
                if (Ids != null)
                {
                    var ObjDetail = JsonConvert.DeserializeObject<List<ClassFeePkg>>(Ids);
                    var ObjData = JsonConvert.DeserializeObject<StdViewModel>(Data);

                    foreach (var i in ObjDetail)
                    {
                        ClassFeePkg cls = new ClassFeePkg();
                        var getData = con.clfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId && c.IsDeleted == false).FirstOrDefault();
                        if (getData != null)
                        {
                            ViewBag.ClassId = getData.classId;
                            ViewBag.SesId = getData.sesId;
                            ViewBag.SecId = getData.secId;
                            ClassFeePkgList.Add(getData);
                        }
                    }
                    if (ClassFeePkgList.Count != 0)
                    {
                        int cId = Convert.ToInt32(ViewBag.ClassId);
                        int sId = Convert.ToInt32(ViewBag.SecId);
                        int ssId = Convert.ToInt32(ViewBag.SesId);
                        int month = Convert.ToInt32(ObjData.Month);
                        int year = Convert.ToInt32(ObjData.Year);
                        var getAllStd = con.std.Where(c => c.sesId == ssId && c.classId == cId && c.secId == sId && c.stdStatus == "Active" && c.IsDeleted == false).ToList();
                        if (getAllStd.Count != 0)
                        {
                            foreach (var s in getAllStd)
                            {
                                List<StudentFee> StdFeeList = new List<StudentFee>();
                                foreach (var cl in ClassFeePkgList)
                                {
                                    var getFee = con.stdfee.Where(c => c.sesId == cl.sesId && c.classId == cl.classId && c.secId == cl.secId && c.feeTypeId == cl.feeTypeId
                                    && c.stdId == s.stdId && c.paidDate.Month == month && c.paidDate.Year == year).FirstOrDefault();
                                    if (getFee != null)
                                    {
                                        StdFeeList.Add(getFee);
                                    }
                                }
                                //var getStdFeePkgs = con.stdfpkg.Where(c => c.sesId == cId && c.classId == cId && c.secId == sId && c.sfpstdId == s.stdId).ToList();

                                if (StdFeeList.Count != 0)
                                {
                                    foreach (var i in StdFeeList)
                                    {
                                        StdFeeDetail stdfee = new StdFeeDetail();
                                        var getStdFeePkg = con.stdfpkg.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId && c.sfpstdId == s.stdId).FirstOrDefault();
                                        var getTotalFee = ClassFeePkgList.Where(c => c.sesId == i.sesId && c.classId == i.classId && c.secId == i.secId && c.feeTypeId == i.feeTypeId).FirstOrDefault();
                                        if (getTotalFee != null)
                                        {
                                            stdfee.stdId = s.stdId;
                                            stdfee.TotalFee = getTotalFee.cfpAmt;
                                            stdfee.FeeAmt = i.feeAmount;
                                            stdfee.Dis = getStdFeePkg.sfpDis;
                                            stdfee.PandingAmt = i.PandingAmount;
                                            stdfee.Fee = i.ft.feeTypeName;
                                            stdfee.FeeStatus = i.feeStatus;
                                            FeePkgList.Add(stdfee);
                                            TempData["BulkReceipt"] = FeePkgList;
                                        }
                                    }
                                    StdFeeDetail std = new StdFeeDetail();
                                    std.stdId = s.stdId;
                                    std.Stdname = s.pr.perName;
                                    std.FatherName = s.pr.perGarName;
                                    std.Contact = s.pr.perContactOne;
                                    std.RollNo = s.stdRollNo;
                                    std.Ses = s.ses.sesName;
                                    std.Class = s.cls.classname;
                                    std.Sec = s.sec.secName;
                                    StdList.Add(std);
                                    TempData["StdList"] = StdList;
                                }
                            }

                            TempData["FeeMonth"] = "" + month + "- " + year + "";
                            return PartialView("_GenBulkReceipt");
                        }
                        else
                        {
                            TempData["Error"] = "No Student found of this class";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "No Class Fee Package found of this class.Please create Class Fee Package for generating vouchers";
                        return View();

                    }
                }
                else
                {
                    TempData["Info"] = "No Record Found";
                    return RedirectToAction("BulkFeeVoucher");
                }
            }
            catch (Exception ex)
            {
                TempData["Info"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("BulkFeeVoucher");
            }
        }

        //Update Received Fee
        [CheckSession]
        public ActionResult UpdateReceivedFee(int id, string RollNo)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                var GetStdRecord = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                if (GetStdRecord != null)
                {
                    if (RoleId > 2 && RoleId != 10)
                    {
                        TempData["Error"] = "You are not allowed to enter this page";
                        return RedirectToAction("Logout", "Home");
                    }
                    else
                    {
                        if (RoleId == 1 || RoleId == 10)
                        {
                            return View(GetStdRecord);
                        }
                        else if (RoleId == 2)
                        {
                            if (GetStdRecord.EntryLocked == true)
                            {
                                TempData["Error"] = "You are not allowed to Edit this record because this record is locked down by Admin.";
                                var btn = "Show";
                                if (RollNo != null && RollNo != "")
                                {
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else
                                {
                                    return RedirectToAction("StudentFeeReport");
                                }
                            }
                            else
                            {
                                return View(GetStdRecord);
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ReceiveStdFee", "Student");
                }
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        [HttpPost]
        public ActionResult UpdateReceivedFee(StudentFee stdfe, int id, string RollNo)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                var GetStdRecord = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                if (GetStdRecord != null)
                {
                    if (RoleId > 2 && RoleId != 10)
                    {
                        TempData["Error"] = "You are not allowed to enter this page";
                        return RedirectToAction("Logout", "Home");
                    }
                    else
                    {
                        if (RoleId == 1 || RoleId == 10)
                        {
                            var getFee = con.stdfpkg.Where(s => s.feeTypeId == stdfe.feeTypeId && s.sfpstdId == GetStdRecord.stdId && s.sesId == GetStdRecord.sesId
                            && s.classId == GetStdRecord.classId && s.secId == GetStdRecord.secId).FirstOrDefault();
                            if (getFee != null)
                            {
                                if (getFee.sfpAmt == stdfe.feeAmount)
                                {
                                    GetStdRecord.feeAmount = stdfe.feeAmount;
                                    GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                    GetStdRecord.PandingAmount = 0;
                                    GetStdRecord.feeStatus = "Paid";
                                    GetStdRecord.paidDate = DateTime.Now;
                                    GetStdRecord.UpdatedDate = DateTime.Now;
                                    GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                    con.SaveChanges();
                                    TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                    var btn = "Show";
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else if (getFee.sfpAmt > stdfe.feeAmount)
                                {
                                    GetStdRecord.feeAmount = stdfe.feeAmount;
                                    GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                    GetStdRecord.PandingAmount = getFee.sfpAmt - stdfe.feeAmount;
                                    GetStdRecord.feeStatus = "Partial";
                                    GetStdRecord.paidDate = DateTime.Now;
                                    GetStdRecord.UpdatedDate = DateTime.Now;
                                    GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                    con.SaveChanges();
                                    TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                    var btn = "Show";
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else if (getFee.sfpAmt < stdfe.feeAmount)
                                {
                                    TempData["Error"] = "Fee Amount must not greater than Total Fee (" + getFee.sfpAmt + ")";
                                    return View(GetStdRecord);
                                }
                            }
                            else
                            {
                                var getClassPkgFee = con.clfpkg.Where(c => c.sesId == GetStdRecord.sesId && c.classId == GetStdRecord.classId && c.secId == GetStdRecord.secId && c.feeTypeId == stdfe.feeTypeId && c.IsDeleted == false).FirstOrDefault();
                                if (getClassPkgFee != null)
                                {
                                    if (getClassPkgFee.cfpAmt == stdfe.feeAmount)
                                    {
                                        GetStdRecord.feeAmount = stdfe.feeAmount;
                                        GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                        GetStdRecord.PandingAmount = 0;
                                        GetStdRecord.feeStatus = "Paid";
                                        GetStdRecord.paidDate = DateTime.Now;
                                        GetStdRecord.UpdatedDate = DateTime.Now;
                                        GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                        con.SaveChanges();
                                        TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                        var btn = "Show";
                                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                    }
                                    else if (getClassPkgFee.cfpAmt > stdfe.feeAmount)
                                    {
                                        GetStdRecord.feeAmount = stdfe.feeAmount;
                                        GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                        GetStdRecord.PandingAmount = getClassPkgFee.cfpAmt - stdfe.feeAmount;
                                        GetStdRecord.feeStatus = "Partial";
                                        GetStdRecord.paidDate = DateTime.Now;
                                        GetStdRecord.UpdatedDate = DateTime.Now;
                                        GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                        con.SaveChanges();
                                        TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                        var btn = "Show";
                                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                    }
                                    else if (getClassPkgFee.cfpAmt < stdfe.feeAmount)
                                    {
                                        TempData["Error"] = "Fee Amount must not greater than Total Fee (" + getClassPkgFee.cfpAmt + ")";
                                        return View(GetStdRecord);
                                    }

                                }
                            }
                        }
                        else if (RoleId == 2)
                        {
                            if (GetStdRecord.EntryLocked == true)
                            {
                                TempData["Error"] = "You are not allowed to Edit this record because this record is locked down by Admin.";
                                var btn = "Show";
                                return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                            }
                            else
                            {
                                var getFee = con.stdfpkg.Where(s => s.feeTypeId == stdfe.feeTypeId && s.sfpstdId == GetStdRecord.stdId).FirstOrDefault();
                                if (getFee != null)
                                {
                                    if (getFee.sfpAmt == stdfe.feeAmount)
                                    {
                                        GetStdRecord.feeAmount = stdfe.feeAmount;
                                        GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                        GetStdRecord.PandingAmount = 0;
                                        GetStdRecord.feeStatus = "Paid";
                                        GetStdRecord.paidDate = DateTime.Now;
                                        GetStdRecord.UpdatedDate = DateTime.Now;
                                        GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                        con.SaveChanges();
                                        TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                        var btn = "Show";
                                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                    }
                                    else if (getFee.sfpAmt > stdfe.feeAmount)
                                    {
                                        GetStdRecord.feeAmount = stdfe.feeAmount;
                                        GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                        GetStdRecord.PandingAmount = getFee.sfpAmt - stdfe.feeAmount;
                                        GetStdRecord.feeStatus = "Partial";
                                        GetStdRecord.paidDate = DateTime.Now;
                                        GetStdRecord.UpdatedDate = DateTime.Now;
                                        GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                        con.SaveChanges();
                                        TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                        var btn = "Show";
                                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                    }
                                    else if (getFee.sfpAmt < stdfe.feeAmount)
                                    {
                                        TempData["Error"] = "Fee Amount must not greater than Total Fee (" + getFee.sfpAmt + ")";
                                        return View(GetStdRecord);
                                    }
                                }
                                else
                                {
                                    var getClassPkgFee = con.clfpkg.Where(c => c.sesId == GetStdRecord.sesId && c.classId == GetStdRecord.classId && c.secId == GetStdRecord.secId && c.feeTypeId == stdfe.feeTypeId).FirstOrDefault();
                                    if (getClassPkgFee != null)
                                    {
                                        if (getClassPkgFee.cfpAmt == stdfe.feeAmount)
                                        {
                                            GetStdRecord.feeAmount = stdfe.feeAmount;
                                            GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                            GetStdRecord.PandingAmount = 0;
                                            GetStdRecord.feeStatus = "Paid";
                                            GetStdRecord.paidDate = DateTime.Now;
                                            GetStdRecord.UpdatedDate = DateTime.Now;
                                            GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                            con.SaveChanges();
                                            TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                            var btn = "Show";
                                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                        }
                                        else if (getClassPkgFee.cfpAmt > stdfe.feeAmount)
                                        {
                                            GetStdRecord.feeAmount = stdfe.feeAmount;
                                            GetStdRecord.feeTypeId = stdfe.feeTypeId;
                                            GetStdRecord.PandingAmount = getClassPkgFee.cfpAmt - stdfe.feeAmount;
                                            GetStdRecord.feeStatus = "Partial";
                                            GetStdRecord.paidDate = DateTime.Now;
                                            GetStdRecord.UpdatedDate = DateTime.Now;
                                            GetStdRecord.UpdatedBy = LoginInfo.UserID;
                                            con.SaveChanges();
                                            TempData["SuccessMessage"] = "Student Fee Updated Successfully";
                                            var btn = "Show";
                                            return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                        }
                                        else if (getClassPkgFee.cfpAmt < stdfe.feeAmount)
                                        {
                                            TempData["Error"] = "Fee Amount must not greater than Total Fee (" + getClassPkgFee.cfpAmt + ")";
                                            return View(GetStdRecord);
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ReceiveStdFee", "Student");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "No Record Found, Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return RedirectToAction("ReceiveStdFee", "Student");
            }
            return View();
        }
        //Delete Received Fee


        public ActionResult DeleteReceivedFee(int id, string RollNo)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                var GetStdRecord = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                if (GetStdRecord != null)
                {
                    if (RoleId > 2 && RoleId != 10)
                    {
                        TempData["Error"] = "You are not allowed to enter this page";
                        return RedirectToAction("Logout", "Home");
                    }
                    else
                    {
                        if (RoleId == 1 || RoleId == 10)
                        {
                            con.stdfee.Remove(GetStdRecord);
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Student Fee Deleted Successfully";
                            var btn = "Show";
                            if (RollNo != null && RollNo != "")
                            {
                                return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                            }
                            else
                            {
                                return RedirectToAction("StudentFeeReport");
                            }

                        }
                        else if (RoleId == 2)
                        {
                            if (GetStdRecord.EntryLocked == true)
                            {
                                TempData["Error"] = "You are not allowed to delete this record because this record is locked down by Admin.";
                                var btn = "Show";
                                if (RollNo != null && RollNo != "")
                                {
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else
                                {
                                    return RedirectToAction("StudentFeeReport");
                                }
                            }
                            else
                            {
                                con.stdfee.Remove(GetStdRecord);
                                con.SaveChanges();
                                TempData["SuccessMessage"] = "Student Fee Deleted Successfully";
                                var btn = "Show";
                                if (RollNo != null && RollNo != "")
                                {
                                    return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                                }
                                else
                                {
                                    return RedirectToAction("StudentFeeReport");
                                }
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ReceiveStdFee", "Student");
                }
            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Student Fee cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Student Fee not deleted Please contact to soft support " + SoftSupport.SoftContactNo + "";
                }
                return RedirectToAction("ReceiveStdFee", "Student");
            }
            return RedirectToAction("ReceiveStdFee", "Student");
        }


        //Entry Locked Down View
        public ActionResult LockedDownReceivedFee()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId == 1 || RoleId == 10)
                {
                    var getRecRecord = con.stdfee.ToList();
                    if (getRecRecord.Count != 0)
                    {
                        var RecRocrd = getRecRecord.Where(s => s.EntryLocked == false).OrderByDescending(s => s.feeId).ToList();
                        if (RecRocrd.Count != 0)
                        {
                            TempData["LockedDownReceivedFee"] = RecRocrd;
                            return View();
                        }
                        else
                        {
                            TempData["Info"] = "All Fee record locked down";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Info"] = "No Record found";
                        return View();
                    }

                }
                else
                {
                    TempData["Error"] = "This is page is desigend for Admin";
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to sof support";
            }
            return View();
        }


        //Logic for Entry Locked Down Received fee
        [CheckSession]
        public ActionResult RecFeeLockedDown(string val, string unChecked)
        {
            if (val != null)
            {
                int FeeId = Convert.ToInt32(val);
                var getRecord = con.stdfee.Where(f => f.feeId == FeeId).FirstOrDefault();
                if (getRecord != null)
                {
                    getRecord.EntryLocked = true;
                    con.SaveChanges();
                    return Json(new { Success = "true", Data = new { } });
                }
            }

            if (unChecked != null)
            {
                int FeeId = Convert.ToInt32(unChecked);
                var getRecord = con.stdfee.Where(f => f.feeId == FeeId).FirstOrDefault();
                if (getRecord != null)
                {
                    getRecord.EntryLocked = false;
                    con.SaveChanges();
                    return Json(new { Success = "true", Data = new { } });
                }
            }

            return Json(new { Success = "false" });
        }


        [CheckSession]
        public ActionResult BulkReceiveStdFee()
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            return View();
        }
        [HttpPost]
        [CheckSession]
        public ActionResult BulkReceiveStdFee(StdFeeDetail sfd, string submitButton)
        {
            PopulatClass();
            PopulatSec();
            PopulatSes();
            List<StdFeeDetail> StdFee = new List<StdFeeDetail>();
            List<StdFeeDetail> NotFeePaidList = new List<StdFeeDetail>();
            try
            {
                if (submitButton == "Show")
                {
                    var FeeDtl = (from cf in con.clfpkg
                                  join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                  where sfd.classId == cf.classId && sfd.sesId == cf.sesId && sfd.secId == cf.secId
                                  select new
                                  {
                                      cf.cfpAmt,
                                      cf.cfpDis,
                                      ft.feeTypeName
                                  }).ToList();
                    if (FeeDtl.Count == 0)
                    {
                        TempData["Error"] = "There is no Class Fee Package exist!";
                        return View();
                    }
                    else
                    {
                        foreach (var i in FeeDtl)
                        {
                            StdFeeDetail sf = new StdFeeDetail();
                            sf.FeeName = i.feeTypeName;
                            sf.FeeAmt = i.cfpAmt;
                            sf.Dis = i.cfpDis;
                            StdFee.Add(sf);
                        }
                        TempData["FeeDetail"] = StdFee;
                        ///Populate Fee type against Class ////
                        var PoplateFeeType = (from cf in con.clfpkg
                                              join ft in con.feetype on cf.feeTypeId equals ft.feeTypeId
                                              where sfd.classId == cf.classId && sfd.sesId == cf.sesId && sfd.secId == cf.secId
                                              select new
                                              {
                                                  ft.feeTypeName,
                                                  ft.feeTypeId
                                              }).ToList();
                        SelectList sl = new SelectList(PoplateFeeType, "feeTypeId", "feeTypeName");
                        ViewData["FeeType"] = sl;
                        ///End Populate Fee type against Student Roll Number ////
                        return View(TempData["FeeDetail"]);

                    }

                }
                else
                {
                    if (sfd.FeeAmt != 0)
                    {
                        if (sfd.FeeDate != null && sfd.FeeDate.Year != 1)
                        {
                            StudentFee AdFee = new StudentFee();
                            var StdId = (from sa in con.std
                                         join pr in con.person on sa.perId equals pr.perId
                                         join sf in con.stdfpkg on sa.stdId equals sf.sfpstdId
                                         where !con.stdfee
                                                  .Any(o => o.stdId == sa.stdId && o.feeTypeId == sfd.feeTypeId && o.classId == sfd.classId
                                                  && o.sesId == sfd.sesId && o.secId == sfd.secId
                                                  && o.paidDate.Month == sfd.FeeDate.Month && o.paidDate.Year == sfd.FeeDate.Year)
                                         where sa.classId == sfd.classId && sa.sesId == sfd.sesId && sa.secId == sfd.secId && sa.IsDeleted == false
                                         && sf.classId == sfd.classId && sf.sesId == sfd.sesId && sf.secId == sfd.secId && sf.feeTypeId == sfd.feeTypeId
                                         select new
                                         {
                                             sa.stdId,
                                             pr.perId,
                                             pr.perName,
                                             pr.perContactOne,
                                             sa.classId,
                                             sa.secId,
                                             sa.sesId,
                                             sf.sfpAmt,
                                             sf.sfpId
                                         }).ToList();
                            if (StdId.Count == 0)
                            {
                                TempData["Info"] = "Either All Student Fee already paid against this fee type! OR Please create Bulk Student Fee Package to proceed!";
                                return View(TempData["FeeDetail"]);
                            }
                            foreach (var i in StdId)
                            {
                                if (i.sfpAmt != sfd.FeeAmt)
                                {
                                    StdFeeDetail s = new StdFeeDetail();
                                    var getFeePkg = con.stdfpkg.Where(sfg => sfg.sfpId == i.sfpId).FirstOrDefault();
                                    if (getFeePkg != null)
                                    {
                                        var getStd = con.std.Where(st => st.stdId == getFeePkg.sfpstdId).FirstOrDefault();
                                        if (getStd != null)
                                        {
                                            s.Stdname = getStd.pr.perName;
                                            s.RollNo = getStd.stdRollNo;
                                            s.FeeName = getFeePkg.ft.feeTypeName;
                                            s.FeeAmt = i.sfpAmt;
                                            s.Class = getStd.cls.classname;
                                            s.Sec = getStd.sec.secName;
                                            s.stdId = i.stdId;
                                            NotFeePaidList.Add(s);
                                            TempData["NfPaid"] = NotFeePaidList;
                                        }
                                    }
                                }
                                else if (i.sfpAmt == sfd.FeeAmt)
                                {

                                    if (sfd.sfpRemarks == null)
                                    {
                                        AdFee.StdRemarks = "No Remarks";
                                    }
                                    else
                                    {
                                        AdFee.StdRemarks = sfd.sfpRemarks;
                                    }

                                    AdFee.feeAmount = sfd.FeeAmt;
                                    if (sfd.FeeDate.Day == 1)
                                    {
                                        AdFee.paidDate = DateTime.Now;
                                    }
                                    else
                                    {
                                        AdFee.paidDate = sfd.FeeDate;
                                    }
                                    AdFee.PandingAmount = 0;
                                    AdFee.feeStatus = "Paid";
                                    AdFee.secId = i.secId;
                                    AdFee.sesId = i.sesId;
                                    AdFee.classId = i.classId;
                                    AdFee.stdId = i.stdId;
                                    AdFee.feeTypeId = sfd.feeTypeId;
                                    AdFee.CreatedBy = LoginInfo.UserID;
                                    AdFee.CreatedDate = DateTime.Now;
                                    AdFee.UpdatedBy = 0;
                                    AdFee.IsDeleted = false;
                                    AdFee.DeletedBy = 0;
                                    con.stdfee.Add(AdFee);
                                    con.SaveChanges();

                                    //For Sending SMS
                                    var chkModule = con.smsModule.Where(s => s.mnId == 4 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                                    if (chkModule != null)
                                    {
                                        EncryptDecrypt decryption = new EncryptDecrypt();
                                        var getAllotment = con.sMSAllotments.FirstOrDefault();
                                        if (getAllotment != null)
                                        {
                                            int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                            DateTime Exdate = Convert.ToDateTime(decryption.Decrypt(getAllotment.saExpiryDate));
                                            var Status = decryption.Decrypt(getAllotment.saStatus);
                                            if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                            {

                                                //Sending SMS
                                                string msgText = "Dear " + i.perName + " " + chkModule.smText + ". Fee Amount " + AdFee.feeAmount + ". Date: " + DateTime.Now;
                                                sendMessage.SendSMSTurab(i.perContactOne, msgText);

                                                SentSMS sms = new SentSMS();
                                                sms.ssDate = DateTime.Now;
                                                sms.ssStatus = true;
                                                sms.perId = i.perId;
                                                sms.ssText = msgText;
                                                con.sentSMs.Add(sms);

                                                //Minus 1 SMS
                                                int msg = RemainingMsg - 1;
                                                var RemMsg = msg.ToString();
                                                getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());

                                                con.SaveChanges();
                                            }
                                        }

                                    }
                                }
                            }
                            var NfgStd = (from sa in con.std
                                          where !con.stdfpkg
                                                   .Any(o => o.sfpstdId == sa.stdId && o.feeTypeId == sfd.feeTypeId && o.classId == sfd.classId && o.sesId == sfd.sesId && o.secId == sfd.secId)
                                          where sa.classId == sfd.classId && sa.sesId == sfd.sesId && sa.secId == sfd.secId && sa.IsDeleted == false

                                          select new
                                          {
                                              sa.stdId
                                          }).ToList();

                            if (NfgStd.Count != 0)
                            {
                                List<Student> nfpStdList = new List<Student>();
                                foreach (var i in NfgStd)
                                {
                                    var getStd = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                                    nfpStdList.Add(getStd);
                                }
                                TempData["nfpStd"] = nfpStdList;
                            }


                            TempData["SuccessMessage"] = "Student Fee Paid successfully";
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Fee Date to proceed";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Enter Fee Amount to proceed";
                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error to received bulk student fee";
            }
            return View();
        }

        public ActionResult _ClassFeeDetail(int SesId, int SecId, int ClassId)
        {
            try
            {
                var GetDetails = con.clfpkg.Where(c => c.sesId == SesId && c.secId == SecId && c.classId == ClassId && c.IsDeleted == false).ToList();
                if (GetDetails.Count != 0)
                {
                    TempData["Class"] = GetDetails;
                }
                return PartialView(TempData["Class"]);
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        [CheckSession]
        public ActionResult DelClassFee(int id)
        {
            try
            {
                var GetData = con.clfpkg.Where(c => c.cfpId == id).FirstOrDefault();

                if (GetData != null)
                {
                    var ChkFee = con.stdfpkg.Where(s => s.secId == GetData.secId && s.sesId == GetData.sesId && s.classId == GetData.classId && s.feeTypeId == GetData.feeTypeId).FirstOrDefault();
                    if (ChkFee == null)
                    {
                        con.clfpkg.Remove(GetData);
                        con.SaveChanges();
                        TempData["SuccessMessage"] = "Class fee Package deleted successfully";
                        return RedirectToAction("ClassFeePkg");

                    }
                    else
                    {
                        TempData["Error"] = "You cannot delete this fee package because this class fee package is associated with Student Fee Package";
                        return RedirectToAction("ClassFeePkg");
                    }
                }
                else
                {
                    TempData["Error"] = "you cannot delete this fee package.";
                    return RedirectToAction("ClassFeePkg");
                }

            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Class Fee Package cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Class Fee Package not deleted Please contact to soft support " + SoftSupport.SoftContactNo + "";
                }
                return RedirectToAction("ClassFeePkg");
            }
        }
        [CheckSession]
        public ActionResult DelStdFee(int id, string RollNo, string Page)
        {
            try
            {
                var GetData = con.stdfpkg.Where(c => c.sfpId == id).FirstOrDefault();
                //var ChkFee = con.stdfee.Where(s => s.secId == GetData.secId && s.stdId == GetData.sfpstdId && s.sesId == GetData.sesId && s.classId == GetData.classId && s.feeTypeId == GetData.feeTypeId).FirstOrDefault();
                if (GetData != null)
                {
                    con.stdfpkg.Remove(GetData);
                    con.SaveChanges();
                    TempData["SuccessMessage"] = "Student fee Package deleted successfully";
                    if (Page == "Package")
                    {
                        var btn = "Show";
                        return RedirectToAction("StdFeePkg", new { RollNo, btn });
                    }
                    else
                    {
                        var btn = "Show";
                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                    }
                }
                else
                {
                    TempData["Error"] = "You cannot delete this fee package because Fee Paid against this Fee Package";
                    if (Page == "Package")
                    {
                        var btn = "Show";
                        return RedirectToAction("StdFeePkg", new { RollNo, btn });
                    }
                    else
                    {
                        var btn = "Show";
                        return RedirectToAction("ReceiveStdFee", new { RollNo, btn });
                    }
                }

            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Student Fee Package cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Student Fee Package not deleted Please contact to soft support " + SoftSupport.SoftContactNo + "";
                }
                return RedirectToAction("ReceiveStdFee");
            }
        }

        [CheckSession]
        public ActionResult StudentFeeReport()
        {
            PopulatClass();
            PopulatSec();
            PopulatAllSes();
            PopulatFeetype();
            return View();
        }

        [CheckSession]
        public ActionResult _GetFeeStatus(int SesId, int SecId, int ClassId, int fType, string Status, int Month, int Year)
        {
            try
            {
                List<StdFeeDetail> sList = new List<StdFeeDetail>();
                if (Status == "Paid")
                {
                    var GetDetails = con.stdfee.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId
                    && s.feeTypeId == fType && s.IsDeleted == false && s.paidDate.Month == Month && s.paidDate.Year == Year).ToList();
                    if (GetDetails.Count != 0)
                    {
                        // var GetStdList = con.std.Where(s => s.sesId == SesId && s.secId == SecId && s.classId == ClassId).ToList();
                        foreach (var i in GetDetails)
                        {
                            StdFeeDetail sd = new StdFeeDetail();
                            sd.feeId = i.feeId;
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
                        //TempData["StdList"] = GetStdList;
                        TempData["Fee"] = sList;
                        TempData["Month"] = Month;
                        TempData["Year"] = Year;

                    }
                    else
                    {
                        TempData["No"] = "No Record found";
                    }
                }
                else if (Status == "UnPaid")
                {
                    var GetStdId = (from s in con.std
                                    where s.sesId == SesId && s.secId == SecId && s.classId == ClassId &&
                                    !(from sf in con.stdfee
                                      where s.stdId == sf.stdId && sf.sesId == SesId && sf.secId == SecId && sf.classId == ClassId
                                      && sf.feeTypeId == fType && sf.paidDate.Month == Month && sf.paidDate.Year == Year
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
                            TempData["Month"] = Month;
                            TempData["Year"] = Year;
                        }
                    }
                    else
                    {
                        TempData["No"] = "No Record found";
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

        public ActionResult GetLatestRollNo()
        {
            try
            {
                var getAllStudent = con.std.OrderByDescending(s => s.stdRollNo).ToList();
                TempData["StdList"] = getAllStudent;
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }

        //Direct Received Fee
        [CheckSession]
        public ActionResult DirectReceivedFee()
        {
            try
            {
                PopulatCampus();
                PopulatSes();
                PopulatClass();
                PopulatSec();
                PopulatFeetype();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult DirectReceivedFee(Student std, string Fee, string Remarks, string feeMonth)
        {
            try
            {
                PopulatCampus();
                PopulatSes();
                PopulatClass();
                PopulatSec();
                PopulatFeetype();
                List<Student> StdList = new List<Student>();

                if (std.camId != 0 && std.sesId != 0 && std.classId != 0 && std.secId != 0 && Fee != "")
                {
                    DateTime Feedate = Convert.ToDateTime(feeMonth);
                    int FeeId = Convert.ToInt32(Fee);
                    var StdId = (from sa in con.std
                                 where !con.stdfee
                                          .Any(o => o.stdId == sa.stdId && o.classId == sa.classId
                                          && o.sesId == sa.sesId && o.secId == sa.secId && o.feeTypeId == FeeId
                                          && o.paidDate.Month == Feedate.Month && o.paidDate.Year == Feedate.Year)
                                 where sa.classId == std.classId && sa.sesId == std.sesId && sa.secId == std.secId && sa.IsDeleted == false && sa.stdStatus == "Active"

                                 select new
                                 {
                                     sa.stdId
                                 }).ToList();

                    if (StdId.Count != 0)
                    {
                        foreach (var i in StdId)
                        {
                            var getStd = con.std.Where(s => s.stdId == i.stdId).FirstOrDefault();
                            if (getStd != null)
                            {
                                StdList.Add(getStd);
                            }
                        }

                        ViewBag.FeeId = Convert.ToInt32(Fee);
                        ViewBag.Remarks = Remarks;
                        ViewBag.FeeMonth = feeMonth;
                        TempData["StdData"] = StdList;
                    }
                    else
                    {
                        TempData["Info"] = "All Students Fee of selected class has been paid of selected month";
                    }
                }
                else
                {
                    TempData["Error"] = "Please Select all the Parameters";
                    return View();
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                TempData["info"] = "" + ex + "";
                return View();
            }
            return View();
        }

        [CheckSession]
        public ActionResult ReceivedDirectFee(string Data, string AccDetails)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);

                if (Data != null)
                {
                    if (AccDetails != null)
                    {
                        List<StudentFee> StdList = new List<StudentFee>();

                        var ObjDetail = JsonConvert.DeserializeObject<List<StudentFee>>(AccDetails);
                        var ObjData = JsonConvert.DeserializeObject<StdViewModel>(Data);
                        DateTime feeDate = Convert.ToDateTime(ObjData.FeeMonth);
                        string remarks = ObjData.Remarks;
                        foreach (var st in ObjDetail)
                        {
                            var ChkRecord = con.stdfee.Where(s => s.stdId == st.stdId && s.sesId == st.sesId &&
                            s.secId == st.secId && s.classId == st.classId && s.feeTypeId == st.feeTypeId &&
                            s.paidDate.Month == feeDate.Month && s.paidDate.Year == feeDate.Year).FirstOrDefault();
                            if (ChkRecord != null)
                            {

                            }
                            else
                            {
                                if (st.feeAmount != 0000)
                                {
                                    StudentFee sm = new StudentFee();
                                    sm.stdId = st.stdId;
                                    sm.classId = st.classId;
                                    sm.secId = st.secId;
                                    sm.sesId = st.sesId;
                                    sm.feeTypeId = st.feeTypeId;
                                    sm.feeAmount = st.feeAmount;
                                    sm.PandingAmount = 0;
                                    sm.paidDate = feeDate;
                                    sm.feeStatus = "Paid";
                                    if (ObjData.Remarks != null && ObjData.Remarks != "")
                                    {
                                        sm.StdRemarks = ObjData.Remarks;
                                    }
                                    else
                                    {
                                        sm.StdRemarks = "No Remarks";
                                    }

                                    sm.CreatedBy = LoginInfo.UserID;
                                    sm.CreatedDate = DateTime.Now;
                                    if (RoleId > 2 && RoleId != 10)
                                    {
                                        sm.EntryLocked = false;
                                    }
                                    else
                                    {
                                        sm.EntryLocked = true;
                                    }
                                    con.stdfee.Add(sm);
                                    con.SaveChanges();
                                }
                            }
                        }
                        return Json(new { success = true, responseText = "All Student Fee Entered Successfully." }, JsonRequestBehavior.AllowGet);

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
                TempData["info"] = "" + ex + "";
                return Json(new { success = false, responseText = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "." }, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckSession]
        public ActionResult UpdateDirectRecFee(int id)
        {
            try
            {
                PopulatClass();
                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                var GetStdRecord = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                if (GetStdRecord != null)
                {
                    if (RoleId > 2 && RoleId != 10)
                    {
                        TempData["Error"] = "You are not allowed to enter this page";
                        return RedirectToAction("Logout", "Home");
                    }
                    else
                    {
                        if (RoleId == 1 || RoleId == 10)
                        {
                            return View(GetStdRecord);
                        }
                        else if (RoleId == 2)
                        {
                            if (GetStdRecord.EntryLocked == true)
                            {
                                TempData["Error"] = "You are not allowed to Edit this record because this record is locked down by Admin.";
                                return RedirectToAction("StudentFeeReport");

                            }
                            else
                            {
                                return View(GetStdRecord);
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("StudentFeeReport", "Student");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult UpdateDirectRecFee(int id, StudentFee std)
        {
            try
            {
                PopulatClass();

                PopulatSec();
                PopulatSes();
                PopulatFeetype();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                var GetStdRecord = con.stdfee.Where(s => s.feeId == id).FirstOrDefault();
                if (GetStdRecord != null)
                {
                    if (RoleId > 2 && RoleId != 10)
                    {
                        TempData["Error"] = "You are not allowed to enter this page";
                        return RedirectToAction("Logout", "Home");
                    }
                    else
                    {
                        if (RoleId == 1 || RoleId == 10)
                        {
                            GetStdRecord.feeAmount = std.feeAmount;
                            con.SaveChanges();
                            TempData["SuccessMessage"] = "Student received Fee Updated successfully";
                            return RedirectToAction("StudentFeeReport");
                        }
                        else if (RoleId == 2)
                        {
                            if (GetStdRecord.EntryLocked == true)
                            {
                                TempData["Error"] = "You are not allowed to Edit this record because this record is locked down by Admin.";
                                return RedirectToAction("StudentFeeReport");

                            }
                            else
                            {
                                GetStdRecord.feeAmount = std.feeAmount;
                                con.SaveChanges();
                                TempData["SuccessMessage"] = "Student received Fee Updated successfully";
                                return RedirectToAction("StudentFeeReport");
                            }
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("StudentFeeReport", "Student");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
            }
            return View();
        }

        [CheckSession]
        public ActionResult DelDirectFee(string val)
        {
            try
            {
                if (val != null)
                {
                    int FeeId = Convert.ToInt32(val);
                    var getRecord = con.stdfee.Where(f => f.feeId == FeeId).FirstOrDefault();
                    if (getRecord != null)
                    {
                        con.stdfee.Remove(getRecord);
                        con.SaveChanges();
                        return Json(new { Success = "true", Data = new { } });
                    }
                    else
                    {
                        return Json(new { Success = "false", Data = new { } });
                    }
                }
                else
                {
                    return Json(new { Success = "false", Data = new { } });
                }
            }
            catch (Exception ex)
            {
                return Json(new { Success = "false", Data = new { } });
            }
        }

        [CheckSession]
        public ActionResult DirectFeeVoucher()
        {
            try
            {
                PopulatFeetype();
                return View();
            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult DirectFeeVoucher(string RollNo, string Due, string feemonth, string amount, string feeId)
        {
            try
            {
                PopulatFeetype();
                if (RollNo != null && RollNo != "")
                {
                    if (amount != null && amount != "")
                    {
                        if (feeId != null && feeId != "")
                        {
                            var getStd = con.std.Where(s => s.stdRollNo == RollNo && s.stdStatus == "Active").FirstOrDefault();
                            if (getStd != null)
                            {
                                int feetypeId = Convert.ToInt32(feeId);
                                var getFeeName = con.feetype.Where(f => f.feeTypeId == feetypeId).FirstOrDefault();
                                if (getFeeName != null)
                                {
                                    TempData["Fee"] = getFeeName.feeTypeName;
                                }
                                TempData["Std"] = getStd;
                                TempData["Amount"] = amount;
                                DateTime fee = Convert.ToDateTime(feemonth);
                                TempData["feeMonth"] = fee.ToString("MMM-yyyy");
                                DateTime due = Convert.ToDateTime(Due);
                                TempData["Due"] = due.ToString("dd-MMM-yyyy");
                                //TempData["feeMonth"] = feemonth.ToString("MMM-yyyy");
                                //TempData["Due"] = Due.ToString("dd-MMM-yyyy");
                                return View("DirectVoucher");
                            }
                            else
                            {
                                TempData["Error"] = "Incorrect Roll Number!";
                                return View();
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please select fee to generate Voucher";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Enter fee amount to generate Voucher";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please Enter RollNo to generate Voucher";
                    return View();
                }


            }
            catch (Exception ex)
            {
                TempData["info"] = "" + ex + "";
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }

        public ActionResult DirectVoucher()
        {
            return View();
        }

        public ActionResult BulkDirectFeeVoucher()
        {
            try
            {
                PopulatCampus();
                PopulatSec();
                PopulatAllSes();
                PopulatClass();
                PopulatFeetype();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }
        [HttpPost]
        public ActionResult BulkDirectFeeVoucher(RegViewModel vm, string Due, string feemonth, string Amount)
        {

            try
            {
                PopulatCampus();
                PopulatSec();
                PopulatAllSes();
                PopulatClass();
                PopulatFeetype();
                if (vm.camId != 0 && vm.classId != 0 && vm.sesId != 0 && vm.secId != 0 && vm.feeTypeId != 0)
                {
                    if (Amount != null && Amount != "")
                    {

                        var getData = con.std.Where(s => s.camId == vm.camId && s.sesId == vm.sesId
                        && s.classId == vm.classId && s.secId == vm.secId && s.stdStatus == "Active").ToList();
                        if (getData.Count != 0)
                        {
                            int feetypeId = Convert.ToInt32(vm.feeTypeId);
                            var getFeeName = con.feetype.Where(f => f.feeTypeId == feetypeId).FirstOrDefault();
                            if (getFeeName != null)
                            {
                                TempData["Fee"] = getFeeName.feeTypeName;
                            }
                            TempData["Data"] = getData;
                            TempData["Amount"] = Amount;
                            //TempData["feeMonth"] = feemonth;
                            //TempData["Due"] = Due;
                            DateTime fee = Convert.ToDateTime(feemonth);
                            TempData["feeMonth"] = fee.ToString("MMM-yyyy");
                            DateTime due = Convert.ToDateTime(Due);
                            TempData["Due"] = due.ToString("dd-MMM-yyyy");
                            return View("DirectVoucher");
                        }
                        else
                        {
                            TempData["Error"] = "No Record found!";
                            return View();
                        }

                    }
                    else
                    {
                        TempData["Error"] = "Please Enter fee amount to generate Voucher";
                        return View();
                    }
                }
                else
                {
                    TempData["Error"] = "Please select aa the parameters to Print Voucher";
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["info"] = "" + ex + "";
                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }
        [CheckSession]
        public ActionResult DirectFeeReceipt()
        {
            try
            {
                PopulatFeetype();
                return View();
            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }
        [CheckSession]
        [HttpPost]
        public ActionResult DirectFeeReceipt(string RollNo, DateTime feemonth, string feeId)
        {
            try
            {
                PopulatFeetype();

                if (RollNo != null && RollNo != "")
                {

                    if (feeId != null && feeId != "")
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == RollNo && s.stdStatus == "Active").FirstOrDefault();
                        if (getStd != null)
                        {
                            int feetypeId = Convert.ToInt32(feeId);
                            //var getFeeName = con.feetype.Where(f => f.feeTypeId == feetypeId).FirstOrDefault();
                            //if (getFeeName != null)
                            //{
                            //    TempData["Fee"] = getFeeName.feeTypeName;
                            //}
                            var getFee = con.stdfee.Where(s => s.stdId == getStd.stdId && s.sesId == getStd.sesId
                            && s.classId == getStd.classId && s.secId == getStd.secId && s.feeTypeId == feetypeId
                            && s.paidDate.Month == feemonth.Month && s.paidDate.Year == feemonth.Year).FirstOrDefault();
                            if (getFee != null)
                            {
                                TempData["Std"] = getFee;

                                TempData["feeMonth"] = feemonth.ToString("MMM-yyyy");

                                return View("DirectReceipt");
                            }
                            else
                            {
                                TempData["Info"] = "No Record found to this Fee type in this Month!";
                                return View();
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Incorrect Roll Number!";
                            return View();
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please select fee to generate Voucher";
                        return View();
                    }

                }
                else
                {
                    TempData["Error"] = "Please Enter RollNo to generate Voucher";
                    return View();
                }



            }
            catch (Exception ex)
            {

                TempData["Error"] = "There is some error Please contact to soft support " + SoftSupport.SoftContactNo + "";
                return View();
            }
        }

        public ActionResult DirectReceipt()
        {
            return View();
        }

        public void PopulatCity()
        {
            //Populating the dropdown for City
            SelectList sl = new SelectList(con.city.ToList(), "CityId", "CityName");
            ViewData["City"] = sl;
        }
        public void PopulatArea()
        {
            //Populating the dropdown for City
            SelectList sl = new SelectList(con.area.ToList(), "AreaId", "AreaName");
            ViewData["Area"] = sl;
        }
        public void PopulatCampus()
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
        public void PopulatClass()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.cls.ToList(), "classId", "classname");
            ViewData["Class"] = sl;
        }
        public void PopulatFeetype()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.feetype.Where(c => c.isDeleted == false).ToList(), "feeTypeId", "feeTypeName");
            ViewData["FeeType"] = sl;
        }

        [HttpPost]
        public ActionResult isRollNoExist(string val)
        {
            if (val != null)
            {
                var Pre = con.std.Where(a => a.stdRollNo == val.ToString()).FirstOrDefault();
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


        public ActionResult GenRollNo(int? SecId, int? SesId, int? ClassId, int? CampId)
        {
            if (SecId != null && SesId != null && ClassId != null && CampId != null)
            {
                var getCampCode = con.camp.Where(c => c.camId == CampId).Select(c => c.CampCode).FirstOrDefault();
                if (getCampCode != null && getCampCode != "")
                {
                    var getSesCode = con.InstSes.Where(c => c.sesId == SesId).Select(c => c.sesCode).FirstOrDefault();
                    if (getSesCode != null && getSesCode != "")
                    {
                        var getClassCode = con.cls.Where(c => c.classId == ClassId).Select(c => c.classCode).FirstOrDefault();
                        if (getClassCode != null && getClassCode != "")
                        {
                            var getSecCode = con.InstSec.Where(c => c.secId == SecId).Select(c => c.secCode).FirstOrDefault();
                            if (getSecCode != null && getSecCode != "")
                            {
                                var ROllNo = getCampCode + getSesCode + getClassCode + getSecCode;

                                //CheckCode 
                                var chkRollNo = (from s in
                                                    con.std
                                                 where s.stdRollNo.StartsWith(ROllNo)
                                                 select new
                                                 {
                                                     s.stdId,
                                                     s.stdRollNo
                                                 }).OrderByDescending(s => s.stdId).Select(s => s.stdRollNo).FirstOrDefault();
                                if (chkRollNo == null)
                                {
                                    var MakeRollNo = ROllNo + "01";
                                    return Json(new { Success = "true", Data = new { MakeRollNo } });
                                }
                                else
                                {
                                    if (chkRollNo.Length > 1)
                                    {
                                        var getLastTwo = chkRollNo.Substring(chkRollNo.Length - 2);
                                        int Two = Convert.ToInt32(getLastTwo) + 1;
                                        var count = Two.ToString().Count();
                                        //var count = getLastTwo.Count();
                                        if (count == 1)
                                        {
                                            var add = "0" + Two;
                                            var getEight = chkRollNo.Substring(0, 8);
                                            var MakeRollNo = getEight + add;
                                            return Json(new { Success = "true", Data = new { MakeRollNo } });
                                        }
                                        else
                                        {
                                            var getEight = chkRollNo.Substring(0, 8);
                                            var MakeRollNo = getEight + Two;
                                            return Json(new { Success = "true", Data = new { MakeRollNo } });
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            //else
            //{
            //    return Json(new { Success = "false" });
            //}
            return Json(new { Success = "false" });
        }

        public ActionResult Populatlocation(int cityId)
        {
            try
            {
                //Populating the dropdown for Locations
                SelectList objcity = new SelectList(con.area.Where(p => p.CityId == cityId).ToList(), "AreaId", "AreaName");
                return Json(objcity);
            }
            catch (Exception ex)
            {
                return Json(null);
            }

        }

        public void PopulatAllSes()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.InstSes.OrderBy(s => s.sesStatus == "Closed").ToList(), "sesId", "sesName");
            ViewData["Session"] = sl;
        }
        public void PopulatDegree()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.degrees.Where(d => d.IsVisible == true).ToList(), "DegreeId", "DegreeName");
            ViewData["Degree"] = sl;
        }
        public void PopulatIstitute()
        {
            //Populating the dropdown for Session
            SelectList sl = new SelectList(con.stdInfos.Where(d => d.IsVisible == true).ToList(), "StdInsId", "Name");
            ViewData["Institute"] = sl;
        }

        public void PopulatBank()
        {
            //Populating the dropdown for Bank
            List<Bank> banklist = new List<Bank>();
            var getBank = con.banks.Where(b => b.IsVisible == true).ToList();
            if (getBank.Count != 0)
            {
                foreach (var i in getBank)
                {
                    var chkbank = con.bankinfos.Where(b => b.BankId == i.BankId && b.IsVisible == true).FirstOrDefault();
                    if (chkbank != null)
                    {
                        Bank bnk = new Bank();
                        bnk.BankId = i.BankId;
                        bnk.BankName = i.BankName;
                        banklist.Add(bnk);
                    }
                }
                SelectList sl = new SelectList(banklist, "BankId", "BankName");
                ViewData["Bank"] = sl;
            }

        }

    }
}