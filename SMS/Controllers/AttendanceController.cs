
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class AttendanceController : Controller
    {
        SendMessage sendMessage = new SendMessage();
        DBCon con = new DBCon();
        // GET: Attendance
        public ActionResult Mark()
        {
            try
            {
                
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [HttpPost]
        public ActionResult Mark(string RFID)
        {
            try
            {
                if (RFID != null)
                {
                    var getPerId = con.perReg.Where(p => p.prRFID == RFID || p.prBarcode == RFID).FirstOrDefault();
                    if(getPerId != null)
                    {
                        if(getPerId.roleId == 3)
                        {
                            var getParam = con.atm.Where(p => p.roleId == 3).FirstOrDefault();
                            if(getParam != null)
                            {
                                DateTime InMinTime = Convert.ToDateTime(getParam.InMinTime);
                                DateTime InMaxTime = Convert.ToDateTime(getParam.InMaxTime); 
                                DateTime OutMinTime = Convert.ToDateTime(getParam.OutMinTime); 
                                DateTime OutMaxTime = Convert.ToDateTime(getParam.OutMaxTime);

                                DateTime Now = DateTime.Now;
                                if(Now >= InMaxTime && Now <= OutMinTime)
                                {
                                    //TempData["Std"] = getStd;
                                    TempData["Marked"] = " 'IN' Attendance Time is over. Your Attendance cannot be marked.Max 'IN' AttenDance Time is '"+ InMaxTime.ToShortTimeString() + "'";
                                    return View();
                                }
                                else if(Now >= OutMaxTime)
                                {
                                    TempData["Marked"] = " 'OUT' Attendance Time is over. Your Attendance cannot be marked.Max 'OUT' AttenDance Time is '" + OutMaxTime.ToShortTimeString() + "'";
                                    return View();
                                }
                                else if (Now >= InMinTime && Now <= InMaxTime)
                                {
                                    var getStd = con.std.Where(s => s.perId == getPerId.perId).FirstOrDefault();
                                    if (getStd != null)
                                    {
                                        if (getStd.stdStatus == "Active")
                                        {
                                            var Date = DateTime.Now.Date;
                                            var chkAttendance = con.att.Where(a => a.perId == getStd.perId && a.attenDate == Date).OrderByDescending(a => a.attenId).FirstOrDefault();
                                            if (chkAttendance == null)
                                            {
                                                var GetDateTime = DateTime.Now;
                                                Attendance at = new Attendance();
                                                at.perId = getStd.perId;
                                                at.sesId = getStd.sesId;
                                                at.classId = getStd.classId;
                                                at.secId = getStd.secId;
                                                at.camId = getStd.camId;
                                                if (Now >= InMinTime && Now <= InMaxTime)
                                                {
                                                    at.attenType = "IN";
                                                }
                                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                                {
                                                    at.attenType = "OUT";
                                                }
                                                else if (Now > InMaxTime && Now < OutMinTime)
                                                {
                                                    at.attenType = "IN(Out of Time)";
                                                }
                                                else if (Now > OutMaxTime)
                                                {
                                                    at.attenType = "OUT(Out of Time)";
                                                }
                                                else
                                                {
                                                    at.attenType = "OUT of Time";
                                                }
                                                at.attenMarkType = "RFID";
                                                at.attenTime = GetDateTime.ToString("hh:mm tt");
                                                at.attenDate = DateTime.Now;
                                                at.attenDateTime = DateTime.Now;
                                                con.att.Add(at);
                                                con.SaveChanges();
                                                ViewBag.attenType = at.attenType;
                                                TempData["Std"] = getStd;

                                                //For Sending SMS
                                                var chkModule = con.smsModule.Where(s => s.mnId == 2 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
                                                if(chkModule != null)
                                                {
                                                    EncryptDecrypt decryption = new EncryptDecrypt();
                                                    var getAllotment = con.sMSAllotments.FirstOrDefault();
                                                    if(getAllotment != null)
                                                    {
                                                        int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                        DateTime Exdate = Convert.ToDateTime(decryption.Decrypt(getAllotment.saExpiryDate));
                                                        var Status = decryption.Decrypt(getAllotment.saStatus);
                                                        if (RemainingMsg > 0 && Exdate > DateTime.Now && Status == "Active")
                                                        {

                                                            //Sending SMS
                                                            string msgText = "Dear " + getStd.pr.perName + " "+ chkModule.smText + " as '"+ at.attenType + "'" + " at " + DateTime.Now ;
                                                            sendMessage.SendSMSTurab(getStd.pr.perContactOne, msgText);

                                                            SentSMS sms = new SentSMS();
                                                            sms.ssDate = DateTime.Now;
                                                            sms.ssStatus = true;
                                                            sms.perId = getStd.perId;
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


                                                return View(TempData["Std"]);
                                            }
                                            else
                                            {
                                                if (chkAttendance.attenDateTime >= InMinTime && chkAttendance.attenDateTime <= InMaxTime)
                                                {
                                                    if (Now >= InMinTime && Now <= InMaxTime)
                                                    {
                                                        TempData["Std"] = getStd;
                                                        TempData["Marked"] = "Your 'IN' Attendance is already Marked.";
                                                    }
                                                }
                                                else if (chkAttendance.attenDateTime >= OutMinTime && chkAttendance.attenDateTime <= OutMaxTime)
                                                {
                                                    TempData["Std"] = getStd;
                                                    TempData["Marked"] = "Your 'OUT' Attendance is already Marked.";
                                                }
                                                else
                                                {
                                                    TempData["Std"] = getStd;
                                                    TempData["Marked"] = "Attendance Time is over.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            TempData["Std"] = getStd;
                                            TempData["Error"] = "Your Attendance cannot be Marked because your Status is " + getStd.stdStatus + ".";
                                        }
                                    }
                                    else
                                    {
                                        TempData["Error"] = "No Record found. Please Enter the correct Code for Attendance.";
                                    }
                                }
                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                {
                                    var getStd = con.std.Where(s => s.perId == getPerId.perId).FirstOrDefault();
                                    if (getStd != null)
                                    {
                                        if (getStd.stdStatus == "Active")
                                        {
                                            var Date = DateTime.Now.Date;
                                            var chkAttendance = con.att.Where(a => a.perId == getStd.perId && a.attenDate == Date).OrderByDescending(a => a.attenId).FirstOrDefault();
                                            if (chkAttendance == null)
                                            {
                                                var GetDateTime = DateTime.Now;
                                                Attendance at = new Attendance();
                                                at.perId = getStd.perId;
                                                at.sesId = getStd.sesId;
                                                at.classId = getStd.classId;
                                                at.secId = getStd.secId;
                                                at.camId = getStd.camId;
                                                if (Now >= InMinTime && Now <= InMaxTime)
                                                {
                                                    at.attenType = "IN";
                                                }
                                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                                {
                                                    at.attenType = "OUT";
                                                }
                                                else if (Now > InMaxTime && Now < OutMinTime)
                                                {
                                                    at.attenType = "IN(Out of Time)";
                                                }
                                                else if (Now > OutMaxTime)
                                                {
                                                    at.attenType = "OUT(Out of Time)";
                                                }
                                                else
                                                {
                                                    at.attenType = "OUT of Time";
                                                }
                                                at.attenMarkType = "RFID";
                                                at.attenTime = GetDateTime.ToString("hh:mm tt");
                                                at.attenDate = DateTime.Now;
                                                at.attenDateTime = DateTime.Now;
                                                con.att.Add(at);
                                                con.SaveChanges();
                                                ViewBag.attenType = at.attenType;
                                                TempData["Std"] = getStd;

                                                //For Sending SMS
                                                var chkModule = con.smsModule.Where(s => s.mnId == 2 && s.smStatus == true && s.isLocked == true).FirstOrDefault();
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
                                                            string msgText = "Dear " + getStd.pr.perName + " " + chkModule.smText + " as '" + at.attenType + "'" + " at " + DateTime.Now;
                                                            sendMessage.SendSMSTurab(getStd.pr.perContactOne, msgText);

                                                            SentSMS sms = new SentSMS();
                                                            sms.ssDate = DateTime.Now;
                                                            sms.ssStatus = true;
                                                            sms.perId = getStd.perId;
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


                                                return View(TempData["Std"]);
                                            }
                                            else
                                            {
                                                if (chkAttendance.attenDateTime >= InMinTime && chkAttendance.attenDateTime <= InMaxTime)
                                                {
                                                    if (Now >= InMinTime && Now <= InMaxTime)
                                                    {
                                                        TempData["Std"] = getStd;
                                                        TempData["Marked"] = "Your 'IN' Attendance is already Marked.";
                                                    }
                                                }
                                                else if (chkAttendance.attenDateTime >= OutMinTime && chkAttendance.attenDateTime <= OutMaxTime)
                                                {
                                                    TempData["Std"] = getStd;
                                                    TempData["Marked"] = "Your 'OUT' Attendance is already Marked.";
                                                }
                                                else
                                                {
                                                    TempData["Std"] = getStd;
                                                    TempData["Marked"] = "Attendance Time is over.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            TempData["Std"] = getStd;
                                            TempData["Error"] = "Your Attendance cannot be Marked because your Status is " + getStd.stdStatus + ".";
                                        }
                                    }
                                    else
                                    {
                                        TempData["Error"] = "No Record found. Please Enter the correct Code for Attendance.";
                                    }
                                }
                            }
                            else
                            {
                                TempData["Error"] = "Please Enter the Parameter of "+ getParam.role.name + " Role";
                            }
                        }
                        else
                        {
                            var getParam = con.atm.Where(p => p.roleId == getPerId.roleId).FirstOrDefault();
                            if (getParam != null)
                            {
                                DateTime InMinTime = Convert.ToDateTime(getParam.InMinTime);
                                DateTime InMaxTime = Convert.ToDateTime(getParam.InMaxTime);
                                DateTime OutMinTime = Convert.ToDateTime(getParam.OutMinTime);
                                DateTime OutMaxTime = Convert.ToDateTime(getParam.OutMaxTime);

                                DateTime Now = DateTime.Now;
                                if (Now >= InMaxTime && Now <= OutMinTime)
                                {
                                    //TempData["Std"] = getStd;
                                    TempData["Marked"] = " 'IN' Attendance Time is over. Your Attendance cannot be marked.Max 'IN' AttenDance Time is '" + InMaxTime.ToShortTimeString() + "'";
                                    return View();
                                }
                                else if (Now >= OutMaxTime)
                                {
                                    TempData["Marked"] = " 'OUT' Attendance Time is over. Your Attendance cannot be marked.Max 'OUT' AttenDance Time is '" + OutMaxTime.ToShortTimeString() + "'";
                                    return View();
                                }
                                else if (Now >= InMinTime && Now <= InMaxTime)
                                {
                                    var getPerson = con.person.Where(s => s.perId == getPerId.perId).FirstOrDefault();
                                    if (getPerson != null)
                                    {
                                        if (getPerson.IsDeleted == false)
                                        {
                                            var Date = DateTime.Now.Date;
                                            var chkAttendance = con.att.Where(a => a.perId == getPerson.perId && a.attenDate == Date).OrderByDescending(a => a.attenId).FirstOrDefault();
                                            if (chkAttendance == null)
                                            {
                                                var GetDateTime = DateTime.Now;
                                                Attendance at = new Attendance();
                                                at.perId = getPerson.perId;
                                                if (Now >= InMinTime && Now <= InMaxTime)
                                                {
                                                    at.attenType = "IN";
                                                }
                                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                                {
                                                    at.attenType = "OUT";
                                                }
                                                else if (Now > InMaxTime && Now < OutMinTime)
                                                {
                                                    at.attenType = "IN(Out of Time)";
                                                }
                                                else if (Now > OutMaxTime)
                                                {
                                                    at.attenType = "OUT(Out of Time)";
                                                }
                                                else
                                                {
                                                    at.attenType = "OUT of Time";
                                                }
                                                at.secId = null;
                                                at.classId = null;
                                                at.sesId = null;
                                                at.camId = null;

                                                at.attenMarkType = "RFID";
                                                at.attenTime = GetDateTime.ToString("hh:mm tt");
                                                at.attenDate = DateTime.Now;
                                                at.attenDateTime = DateTime.Now;
                                                con.att.Add(at);
                                                con.SaveChanges();
                                                ViewBag.attenType = at.attenType;
                                                TempData["Person"] = getPerson;
                                                return View(TempData["Person"]);
                                            }
                                            else
                                            {
                                                if (chkAttendance.attenDateTime >= InMinTime && chkAttendance.attenDateTime <= InMaxTime)
                                                {
                                                    if (Now >= InMinTime && Now <= InMaxTime)
                                                    {
                                                        TempData["Person"] = getPerson;
                                                        TempData["Marked"] = "Your 'IN' Attendance is already Marked.";
                                                    }
                                                }
                                                else if (chkAttendance.attenDateTime >= OutMinTime && chkAttendance.attenDateTime <= OutMaxTime)
                                                {
                                                    TempData["Person"] = getPerson;
                                                    TempData["Marked"] = "Your 'OUT' Attendance is already Marked.";
                                                }
                                                else
                                                {
                                                    TempData["Person"] = getPerson;
                                                    TempData["Marked"] = "Attendance Time is over.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            TempData["Std"] = getPerson;
                                            TempData["Error"] = "Your Attendance cannot be Marked because you no longer part of the organization.";
                                        }
                                    }
                                    else
                                    {
                                        TempData["Error"] = "No Record found. Please Enter the correct Code for Attendance.";
                                    }
                                }
                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                {
                                    var getPerson = con.person.Where(s => s.perId == getPerId.perId).FirstOrDefault();
                                    if (getPerson != null)
                                    {
                                        if (getPerson.IsDeleted == false)
                                        {
                                            var Date = DateTime.Now.Date;
                                            var chkAttendance = con.att.Where(a => a.perId == getPerson.perId && a.attenDate == Date).OrderByDescending(a => a.attenId).FirstOrDefault();
                                            if (chkAttendance == null)
                                            {
                                                var GetDateTime = DateTime.Now;
                                                Attendance at = new Attendance();
                                                at.perId = getPerson.perId;
                                                if (Now >= InMinTime && Now <= InMaxTime)
                                                {
                                                    at.attenType = "IN";
                                                }
                                                else if (Now >= OutMinTime && Now <= OutMaxTime)
                                                {
                                                    at.attenType = "OUT";
                                                }
                                                else if (Now > InMaxTime && Now < OutMinTime)
                                                {
                                                    at.attenType = "IN(Out of Time)";
                                                }
                                                else if (Now > OutMaxTime)
                                                {
                                                    at.attenType = "OUT(Out of Time)";
                                                }
                                                else
                                                {
                                                    at.attenType = "OUT of Time";
                                                }
                                                at.secId = null;
                                                at.classId = null;
                                                at.sesId = null;
                                                at.camId = null;
                                                at.attenMarkType = "RFID";
                                                at.attenTime = GetDateTime.ToString("hh:mm tt");
                                                at.attenDate = DateTime.Now;
                                                at.attenDateTime = DateTime.Now;
                                                con.att.Add(at);
                                                con.SaveChanges();
                                                ViewBag.attenType = at.attenType;
                                                TempData["Person"] = getPerson;
                                                return View(TempData["Person"]);
                                            }
                                            else
                                            {
                                                if (chkAttendance.attenDateTime >= InMinTime && chkAttendance.attenDateTime <= InMaxTime)
                                                {
                                                    if (Now >= InMinTime && Now <= InMaxTime)
                                                    {
                                                        TempData["Person"] = getPerson;
                                                        TempData["Marked"] = "Your 'IN' Attendance is already Marked.";
                                                    }
                                                }
                                                else if (chkAttendance.attenDateTime >= OutMinTime && chkAttendance.attenDateTime <= OutMaxTime)
                                                {
                                                    TempData["Person"] = getPerson;
                                                    TempData["Marked"] = "Your 'OUT' Attendance is already Marked.";
                                                }
                                                else
                                                {
                                                    TempData["Person"] = getPerson;
                                                    TempData["Marked"] = "Attendance Time is over.";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            TempData["Std"] = getPerson;
                                            TempData["Error"] = "Your Attendance cannot be Marked because you no longer part of the organization.";
                                        }
                                    }
                                    else
                                    {
                                        TempData["Error"] = "No Record found. Please Enter the correct Code for Attendance.";
                                    }
                                }
                            }
                            else
                            {
                                TempData["Error"] = "Please Enter the Attendance Parameters.";
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "No Record found.Please Enter the Correct Code for Attendance.";
                    }
                }

            }
            catch (Exception ex)
            {

            }
            return View();
        }
        
        [CheckSession]
        public ActionResult PersonReg()
        {
            try
            {
                PopulatRole();
                return View();
            }
            catch (Exception ex)
            {

            }
            return View();
        }

        [CheckSession]
        [HttpPost]
        public ActionResult PersonReg(string submitButton, string Code, PersonRegNo reg, string roleId, string perId)
        {
            try
            {
                PopulatRole();
                if (submitButton == "Show")
                {
                    if (Code != "" && Code != null)
                    {
                        if(reg.roleId != 0)
                        {
                            if(reg.roleId == 3)
                            {
                                var getStd = con.std.Where(s => s.stdRollNo == Code).FirstOrDefault();
                                if (getStd != null)
                                {
                                    TempData["Std"] = getStd;
                                    ViewBag.Code = Code;
                                    ViewBag.roleId = reg.roleId;
                                    ViewBag.perId = getStd.perId;
                                    return View();
                                }
                                else
                                {
                                    TempData["Error"] = "No Record found";
                                    return View();
                                }
                            }
                            else
                            {
                                int code = Convert.ToInt32(Code);
                                var getPerson = con.person.Where(s => s.perCode == code && s.roleId == reg.roleId).FirstOrDefault();
                                if (getPerson != null)
                                {
                                    TempData["Person"] = getPerson;
                                    ViewBag.Code = Code;
                                    ViewBag.roleId = reg.roleId;
                                    ViewBag.perId = getPerson.perId;
                                    return View();
                                }
                                else
                                {
                                    TempData["Error"] = "No Record found";
                                    return View();
                                }
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please select Role Type";
                            return View();
                        }
                        
                    }
                }
                else
                {
                    var chkRegg = con.perReg.Where(r => r.perId == reg.perId).FirstOrDefault();
                    if (chkRegg == null)
                    {
                        if (reg.prBarcode != null)
                        {
                            var chkcode = con.perReg.Where(r => r.prBarcode == reg.prBarcode || r.prRFID == reg.prBarcode).Any();
                            if (chkcode == false)
                            {
                                if (reg.prRFID != null)
                                {
                                    var chkRFID = con.perReg.Where(r => r.prRFID == reg.prRFID || r.prBarcode == reg.prRFID).Any();
                                    if (chkRFID == false)
                                    {
                                        reg.roleId = Convert.ToInt32(roleId);
                                        reg.perId = Convert.ToInt32(perId);
                                        con.perReg.Add(reg);
                                        con.SaveChanges();
                                        TempData["Success"] = "Person Attendance Registration successfully completed.";
                                        return RedirectToAction("PersonReg");
                                    }
                                    else
                                    {
                                        TempData["Error"] = "This RFID is already registered.";
                                        return View();
                                    }
                                }
                                else
                                {
                                    reg.roleId = Convert.ToInt32(roleId);
                                    reg.perId = Convert.ToInt32(perId);
                                    con.perReg.Add(reg);
                                    con.SaveChanges();
                                    TempData["Success"] = "Person Attendance Registration successfully completed.";
                                    return RedirectToAction("PersonReg");
                                }
                            }
                            else
                            {
                                TempData["Error"] = "This Barcode is already registered.";
                                return View();
                            }
                        }
                        else
                        {
                            if (reg.prRFID != null)
                            {
                                var chkRFID = con.perReg.Where(r => r.prRFID == reg.prRFID).Any();
                                if (chkRFID == false)
                                {
                                    con.perReg.Add(reg);
                                    con.SaveChanges();
                                    TempData["Success"] = "Person Attendance Registration successfully completed.";
                                    return RedirectToAction("PersonReg");
                                }
                                else
                                {
                                    TempData["Error"] = "This RFID is already registered.";
                                    return View();
                                }
                            }
                        }
                    }
                    else
                    {
                       if(reg.prBarcode != null && reg.prBarcode != "")
                        {
                            var chk = con.perReg.Where(b => b.prBarcode == reg.prBarcode && b.prId != chkRegg.prId).Any();
                            if(chk == false)
                            {
                                chkRegg.prBarcode = reg.prBarcode;
                            }
                            else
                            {
                                TempData["Error"] = "This Barcode is  already exist please try a different one";
                            }
                            
                        }
                       if(reg.prRFID != null && reg.prRFID != "")
                        {
                            var chk = con.perReg.Where(b => b.prRFID == reg.prRFID && b.prId != chkRegg.prId).Any();
                            if (chk == false)
                            {
                                chkRegg.prRFID = reg.prRFID;
                                TempData["Success"] = "RFID registered successully";
                            }
                            else
                            {
                                TempData["Error"] = "This RFID is  already exist please try a different one";
                            }
                            
                        }
                        con.SaveChanges();
                        return View();

                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error registration not completed. please contact to soft support";

            }
            return View();
        }

        [CheckSession]
        public ActionResult AddAttendanceParam()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 1 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulatRole();
                    var GetRecord = con.atm.ToList();
                    if (GetRecord.Count != 0)
                    {
                        TempData["Parameter"] = GetRecord;
                        return View();
                    }
                }

                    
            }
            catch (Exception ex)
            {

            }
            return View();
        }
        [CheckSession]
        [HttpPost]
        public ActionResult AddAttendanceParam(AttenParameter att)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to Edit this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulatRole();
                    var chkType = con.atm.Where(a => a.roleId == att.roleId).Any();
                    if (chkType == false)
                    {
                        con.atm.Add(att);
                        con.SaveChanges();
                        TempData["Success"] = "Data entered successfully";
                        return RedirectToAction("AddAttendanceParam");
                    }
                    else
                    {
                        TempData["Error"] = "This type is already entered";
                        return RedirectToAction("AddAttendanceParam");
                    }
                }
                    
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Attendance Parameter cannot be added";
                return View();
            }
           
        }

        public ActionResult EditAttenParam(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to Edit this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulatRole();
                    var getRecord = con.atm.Where(a => a.apId == id).FirstOrDefault();
                    if (getRecord != null)
                    {
                        return View(getRecord);
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddAttendanceParam");
                    }
                }
                   
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Parameter Cannot be added";
                return RedirectToAction("AddAttendanceParam");
            }
        }

        [HttpPost]
        public ActionResult EditAttenParam(AttenParameter at,  int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to Edit this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulatRole();
                    var getRecord = con.atm.Where(a => a.apId == id).FirstOrDefault();
                    if (getRecord != null)
                    {
                        getRecord.roleId = at.roleId;
                        getRecord.InMinTime = at.InMinTime;
                        getRecord.InMaxTime = at.InMaxTime;
                        getRecord.OutMaxTime = at.OutMaxTime;
                        getRecord.OutMinTime = at.OutMinTime;
                        con.SaveChanges();
                        TempData["Success"] = "Parameters Updated Successfully.";
                        return RedirectToAction("AddAttendanceParam");
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found.";
                        return RedirectToAction("AddAttendanceParam");
                    }
                }
                    
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Parameter Cannot be added.";
                return RedirectToAction("AddAttendanceParam");
            }
        }


        public ActionResult DelAttenParam(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to Edit this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getRecord = con.atm.Where(a => a.apId == id).FirstOrDefault();
                    if (getRecord != null)
                    {
                        con.atm.Remove(getRecord);
                        con.SaveChanges();
                        TempData["Success"] = "Parameters Deleted Successfully.";
                        return RedirectToAction("AddAttendanceParam");
                    }
                    else
                    {
                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("AddAttendanceParam");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Parameter Cannot be Deleted";
                return RedirectToAction("AddAttendanceParam");
            }
        }
        public void PopulatRole()
        {
            //Populating the dropdown for City
            SelectList sl = new SelectList(con.role.Where(r => r.roleId != 4 && r.roleId != 10).ToList(), "roleId", "name");
            ViewData["Role"] = sl;
        }
    }
}