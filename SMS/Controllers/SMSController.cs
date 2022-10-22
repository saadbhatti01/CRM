using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class SMSController : Controller
    {
        SendMessage sendMessage = new SendMessage();
        DBCon con = new DBCon();
        public ActionResult ModuleName()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getName = con.smsModuleName.ToList();
                    if (getName.Count != 0)
                    {
                        TempData["Name"] = getName;
                        return View();
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support "+SoftSupport.SoftContactNo+"";
            }
            return View();
        }
        [HttpPost]
        public ActionResult ModuleName(SMSModuleName sms)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    con.smsModuleName.Add(sms);
                    con.SaveChanges();
                    TempData["Success"] = "Module Name added successfully";
                    return RedirectToAction("ModuleName");
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult EditModuleName(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getData = con.smsModuleName.Where(s => s.mnId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        return View(getData);
                    }
                }
            }
            catch (Exception ex)
            {
                return View();
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditModuleName(int id, SMSModuleName sms)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getData = con.smsModuleName.Where(s => s.mnId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        getData.mName = sms.mName;
                        getData.mnStatus = sms.mnStatus;
                        con.SaveChanges();
                        TempData["Success"] = "Module Name updated successfully";
                        return RedirectToAction("ModuleName");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("ModuleName");
            }
            return RedirectToAction("ModuleName");
        }

        public ActionResult SMSAllotment()
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
                    var getData = con.sMSAllotments.ToList();
                    List<SMSAllotment> allomentList = new List<SMSAllotment>();
                    EncryptDecrypt decrpt = new EncryptDecrypt();
                    foreach (var i in getData)
                    {
                        SMSAllotment sms = new SMSAllotment();
                        sms.saId = i.saId;
                        sms.saQty = decrpt.Decrypt(i.saQty);
                        sms.saRemainingQty = decrpt.Decrypt(i.saRemainingQty);
                        sms.saAmount = decrpt.Decrypt(i.saAmount);
                        sms.saPaidAmountDate = decrpt.Decrypt(i.saPaidAmountDate);
                        sms.saApprovalDate = decrpt.Decrypt(i.saApprovalDate);
                        sms.saExpiryDate = decrpt.Decrypt(i.saExpiryDate);
                        sms.saStatus = decrpt.Decrypt(i.saStatus);
                        allomentList.Add(sms);
                    }
                    TempData["SMSAllotment"] = allomentList;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("Logout", "Home");
            }
        }

        [HttpPost]
        public ActionResult SMSAllotment(SMSAllotment smsAll)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    EncryptDecrypt encrpt = new EncryptDecrypt();
                    SMSAllotment sl = new SMSAllotment();
                    sl.saQty = encrpt.Encrypt(smsAll.saQty);
                    sl.saRemainingQty = encrpt.Encrypt(smsAll.saQty);
                    sl.saAmount = encrpt.Encrypt(smsAll.saAmount);
                    sl.saPaidAmountDate = encrpt.Encrypt(smsAll.saPaidAmountDate);
                    sl.saApprovalDate = encrpt.Encrypt(smsAll.saApprovalDate);
                    sl.saExpiryDate = encrpt.Encrypt(smsAll.saExpiryDate);
                    sl.saStatus = encrpt.Encrypt(smsAll.saStatus);
                    con.sMSAllotments.Add(sl);
                    con.SaveChanges();
                    TempData["Success"] = "SMS Allotment Details Entered successfully";
                    return RedirectToAction("SMSAllotment");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("Logout", "Home");
            }
        }

        public ActionResult EditAllotmentDetails(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    EncryptDecrypt decrpt = new EncryptDecrypt();
                    var i = con.sMSAllotments.Where(s => s.saId == id).FirstOrDefault();
                    SMSAllotment sms = new SMSAllotment();
                    sms.saQty = decrpt.Decrypt(i.saQty);
                    sms.saRemainingQty = decrpt.Decrypt(i.saRemainingQty);
                    sms.saAmount = decrpt.Decrypt(i.saAmount);
                    sms.saPaidAmountDate = decrpt.Decrypt(i.saPaidAmountDate);
                    sms.saApprovalDate = decrpt.Decrypt(i.saApprovalDate);
                    sms.saExpiryDate = decrpt.Decrypt(i.saExpiryDate);
                    sms.saStatus = decrpt.Decrypt(i.saStatus);
                    return View(sms);
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSAllotment");
            }
        }

        [HttpPost]
        public ActionResult EditAllotmentDetails(SMSAllotment smsAll, int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    EncryptDecrypt encrpt = new EncryptDecrypt();
                    var sl = con.sMSAllotments.Where(s => s.saId == id).FirstOrDefault();
                    sl.saQty = encrpt.Encrypt(smsAll.saQty);
                    sl.saAmount = encrpt.Encrypt(smsAll.saAmount);
                    sl.saPaidAmountDate = encrpt.Encrypt(smsAll.saPaidAmountDate);
                    sl.saApprovalDate = encrpt.Encrypt(smsAll.saApprovalDate);
                    sl.saExpiryDate = encrpt.Encrypt(smsAll.saExpiryDate);
                    sl.saStatus = encrpt.Encrypt(smsAll.saStatus);
                    con.SaveChanges();
                    TempData["Success"] = "SMS Allotment Details updated successfully";
                    return RedirectToAction("SMSAllotment");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSAllotment");
            }
        }

        public ActionResult DelAllotment(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getData = con.sMSAllotments.Where(s => s.saId == id).FirstOrDefault();
                    if (getData != null)
                    {
                        con.sMSAllotments.Remove(getData);
                        con.SaveChanges();
                        TempData["Success"] = "SMS Allotment Details Deleted successfully";
                        return RedirectToAction("SMSAllotment");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSAllotment");
            }
            return RedirectToAction("SMSAllotment");
        }


        public ActionResult SMSModule()
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
                    var getModuleName = con.smsModuleName.Where(s => s.mnStatus == true).ToList();
                    if (getModuleName.Count != 0)
                    {
                        List<SMSModuleName> smslist = new List<SMSModuleName>();
                        foreach (var i in getModuleName)
                        {
                            var chkmodule = con.smsModule.Where(s => s.mnId == i.mnId).Any();
                            if (chkmodule == false)
                            {
                                SMSModuleName sms = new SMSModuleName();
                                sms.mnId = i.mnId;
                                sms.mName = i.mName;
                                smslist.Add(sms);
                                TempData["ModuleName"] = smslist;
                            }
                        }

                        var getSMSModule = con.smsModule.ToList();
                        TempData["Module"] = getSMSModule;

                        return View();
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some Error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("Logout", "Home");
            }
            return View();
        }

        public ActionResult AddSMSModule(string val, string unChecked, string Text)
        {
            try
            {
                if (val != null)
                {
                    int NameId = Convert.ToInt32(val);

                    var getRecord = con.smsModule.Where(f => f.mnId == NameId).Any();
                    if (getRecord == false)
                    {
                        SMSModule sms = new SMSModule();
                        sms.mnId = NameId;
                        if (Text != null && Text != "")
                        {
                            sms.smText = Text;
                        }
                        else
                        {
                            sms.smText = "";
                        }
                        sms.smStatus = true;
                        sms.isLocked = false;
                        sms.smStatusChangeDate = DateTime.Now;
                        con.smsModule.Add(sms);
                        con.SaveChanges();
                        return Json(new { Success = "true", Data = new { } });
                    }
                    else
                    {
                        return Json(new { Success = "Available", Data = new { } });
                    }
                }

                if (unChecked != null)
                {
                    int NameId = Convert.ToInt32(unChecked);

                    var getRecord = con.smsModule.Where(f => f.mnId == NameId).FirstOrDefault();
                    if (getRecord != null)
                    {
                        con.smsModule.Remove(getRecord);
                        con.SaveChanges();
                        return Json(new { Success = "true", Data = new { } });
                    }
                }

                return Json(new { Success = "false" });
            }
            catch (Exception ex)
            {

                return Json(new { Success = "false", Data = new { } });
            }

        }

        public ActionResult EditSMSModule(int id)
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
                    var getModule = con.smsModule.Where(s => s.smId == id).FirstOrDefault();
                    if (getModule != null)
                    {
                        return View(getModule);
                    }
                    else
                    {
                        TempData["Error"] = "No Record found";
                        return RedirectToAction("SMSModule");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSModule");
            }
        }
        [HttpPost]
        public ActionResult EditSMSModule(int id, SMSModule sms)
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
                    var OldModule = con.smsModule.Where(s => s.smId == id).FirstOrDefault();
                    if (OldModule != null)
                    {
                        OldModule.smStatus = sms.smStatus;
                        OldModule.smText = sms.smText;
                        OldModule.isLocked = false;
                        OldModule.smStatusChangeDate = DateTime.Now;
                        con.SaveChanges();
                        TempData["Success"] = "SMS Module Updated Successfully";
                        return RedirectToAction("SMSModule");
                    }
                    else
                    {
                        TempData["Error"] = "No Record found";
                        return RedirectToAction("SMSModule");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSModule");
            }
        }

        public ActionResult DelSMSModule(int id)
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
                    var getModule = con.smsModule.Where(s => s.smId == id).FirstOrDefault();
                    if (getModule != null)
                    {
                        con.smsModule.Remove(getModule);
                        con.SaveChanges();
                        TempData["Success"] = "SMS Module Deleted Successfully";
                        return RedirectToAction("SMSModule");
                    }
                    else
                    {
                        TempData["Error"] = "No Record found";
                        return RedirectToAction("SMSModule");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSModule");
            }
        }

        public ActionResult ApprovedModule(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getModule = con.smsModule.Where(s => s.smId == id).FirstOrDefault();
                    if (getModule != null)
                    {
                        if (getModule.isLocked == true)
                        {
                            getModule.isLocked = false;
                            con.SaveChanges();
                            TempData["Success"] = "SMS Module DisApproved Successfully";
                        }
                        else
                        {
                            getModule.isLocked = true;
                            con.SaveChanges();
                            TempData["Success"] = "SMS Module Approved Successfully";
                        }



                        return RedirectToAction("SMSModule");
                    }
                    else
                    {
                        TempData["Error"] = "No Record found";
                        return RedirectToAction("SMSModule");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("SMSModule");
            }
        }

        public ActionResult _SMSModule()
        {
            try
            {
                var getSMSModule = con.smsModule.ToList();
                TempData["Module"] = getSMSModule;
                return PartialView();
            }
            catch (Exception ex)
            {

            }
            return PartialView();
        }

        public ActionResult SentSMS()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult _Sent(string ToDate, string FromDate, string Status)
        {
            try
            {
                if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                {
                    DateTime From = Convert.ToDateTime(FromDate);
                    DateTime To = Convert.ToDateTime(ToDate);
                    var GetDetails = con.sentSMs.Where(c => c.ssDate >= From && c.ssDate <= To).OrderByDescending(c => c.ssId).ToList();
                    if (GetDetails.Count != 0)
                    {
                        TempData["Details"] = GetDetails;
                        TempData["fDate"] = FromDate;
                        TempData["tDate"] = ToDate;
                    }
                    else
                    {
                        TempData["No"] = "No Record found.";
                    }
                    return PartialView("_Sent");
                }
                else
                {
                    TempData["Info"] = "please select Date first.";
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_Sent");
        }

        public ActionResult CustomSMS()
        {
            try
            {
                var chkMsg = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                if (chkMsg != null)
                {
                    TempData["Msg"] = chkMsg.smText;
                }
                PopulatAllSes();
                PopulatCampus();
                PopulatClass();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please Contact to sof support";
                return RedirectToAction("SMSAllotment");
            }
        }

        public ActionResult SendCustomMsg(RegViewModel reg, string ClassMsg, string StdMsg, string fileMsg, HttpPostedFileBase file)
        {
            try
            {
                if (ClassMsg != null && ClassMsg != "")
                {
                    if (reg.camId != 0 && reg.sesId != 0)
                    {
                        if (reg.classId == 0 && reg.secId == 0)
                        {
                            var getdata = con.std.Where(s => s.camId == reg.camId
                            && s.sesId == reg.sesId 
                            && s.stdStatus == "Active" && s.IsDeleted == false).ToList();
                            if (getdata.Count != 0)
                            {
                                //For Sending SMS
                                var chkModule = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                                if (chkModule != null)
                                {
                                    if (chkModule.smStatus == true)
                                    {
                                        if (chkModule.isLocked == true)
                                        {
                                            EncryptDecrypt decryption = new EncryptDecrypt();
                                            var getAllotment = con.sMSAllotments.FirstOrDefault();
                                            if (getAllotment != null)
                                            {
                                                int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                var expiryDate = decryption.Decrypt(getAllotment.saExpiryDate);
                                                var Exdate = Convert.ToDateTime(expiryDate);
                                                var Status = decryption.Decrypt(getAllotment.saStatus);
                                                if (RemainingMsg > 0)
                                                {
                                                    if (Exdate > DateTime.Now)
                                                    {
                                                        if (Status == "Active")
                                                        {
                                                            //Sending SMS
                                                            int Qty = RemainingMsg;
                                                            foreach (var i in getdata)
                                                            {
                                                                if (Qty > 0)
                                                                {
                                                                    sendMessage.SendSMSTurab(i.pr.perContactOne, chkModule.smText);
                                                                    SentSMS sms = new SentSMS();
                                                                    sms.ssDate = DateTime.Now;
                                                                    sms.ssStatus = true;
                                                                    sms.perId = i.perId;
                                                                    sms.ssText = chkModule.smText;
                                                                    con.sentSMs.Add(sms);
                                                                    //Minus 1 SMS
                                                                    int msg = RemainingMsg - 1;
                                                                    var RemMsg = msg.ToString();
                                                                    getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());
                                                                    con.SaveChanges();
                                                                    Qty = Qty - 1;
                                                                }
                                                                else
                                                                {
                                                                    var chkQty = con.sMSAllotments.FirstOrDefault();
                                                                    int Remaining = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                                    if (Remaining <= 0)
                                                                    {
                                                                        TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                                        return RedirectToAction("CustomSMS");
                                                                    }

                                                                }

                                                            }

                                                            TempData["Success"] = "Custom Message Sent successfully";
                                                        }
                                                        else
                                                        {
                                                            TempData["Info"] = "You SMS Allotment is not active. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                            return RedirectToAction("CustomSMS");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TempData["Info"] = "You SMS Allotment is expired. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                        return RedirectToAction("CustomSMS");
                                                    }

                                                }
                                                else
                                                {
                                                    TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                    return RedirectToAction("CustomSMS");
                                                }
                                            }
                                            else
                                            {
                                                TempData["Info"] = "You have not alloted any messages. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                return RedirectToAction("CustomSMS");
                                            }
                                        }
                                        else
                                        {
                                            TempData["Info"] = "Your Custom Message is not approved by soft support team. Please Contact them to approve your custom message.";
                                            return RedirectToAction("CustomSMS");
                                        }
                                    }
                                    else
                                    {
                                        TempData["Info"] = "Your Custom Message Module Status is not active. please go and active it to send the custom message";
                                        return RedirectToAction("CustomSMS");
                                    }

                                }
                                else
                                {
                                    TempData["Info"] = "Please add some custom message to proceed.";
                                    return RedirectToAction("CustomSMS");
                                }
                                //End Sending SMS

                            }
                            else
                            {
                                TempData["Info"] = "no student record found in selected parameters";
                                return RedirectToAction("CustomSMS");
                            }
                        }
                        else if (reg.classId != 0 && reg.secId == 0)
                        {
                            var getdata = con.std.Where(s => s.camId == reg.camId
                            && s.classId == reg.classId && s.sesId == reg.sesId 
                            && s.stdStatus == "Active" && s.IsDeleted == false).ToList();
                            if (getdata.Count != 0)
                            {
                                //For Sending SMS
                                var chkModule = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                                if (chkModule != null)
                                {
                                    if (chkModule.smStatus == true)
                                    {
                                        if (chkModule.isLocked == true)
                                        {
                                            EncryptDecrypt decryption = new EncryptDecrypt();
                                            var getAllotment = con.sMSAllotments.FirstOrDefault();
                                            if (getAllotment != null)
                                            {
                                                int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                var expiryDate = decryption.Decrypt(getAllotment.saExpiryDate);
                                                var Exdate = Convert.ToDateTime(expiryDate);
                                                var Status = decryption.Decrypt(getAllotment.saStatus);
                                                if (RemainingMsg > 0)
                                                {
                                                    if (Exdate > DateTime.Now)
                                                    {
                                                        if (Status == "Active")
                                                        {
                                                            //Sending SMS
                                                            int Qty = RemainingMsg;
                                                            foreach (var i in getdata)
                                                            {
                                                                if (Qty > 0)
                                                                {
                                                                    sendMessage.SendSMSTurab(i.pr.perContactOne, chkModule.smText);
                                                                    SentSMS sms = new SentSMS();
                                                                    sms.ssDate = DateTime.Now;
                                                                    sms.ssStatus = true;
                                                                    sms.perId = i.perId;
                                                                    sms.ssText = chkModule.smText;
                                                                    con.sentSMs.Add(sms);
                                                                    //Minus 1 SMS
                                                                    int msg = RemainingMsg - 1;
                                                                    var RemMsg = msg.ToString();
                                                                    getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());
                                                                    con.SaveChanges();
                                                                    Qty = Qty - 1;
                                                                }
                                                                else
                                                                {
                                                                    var chkQty = con.sMSAllotments.FirstOrDefault();
                                                                    int Remaining = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                                    if (Remaining <= 0)
                                                                    {
                                                                        TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                                        return RedirectToAction("CustomSMS");
                                                                    }

                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            TempData["Info"] = "You SMS Allotment is active. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                            return RedirectToAction("CustomSMS");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TempData["Info"] = "You SMS Allotment is expired. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                        return RedirectToAction("CustomSMS");
                                                    }

                                                }
                                                else
                                                {
                                                    TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                    return RedirectToAction("CustomSMS");
                                                }
                                            }
                                            else
                                            {
                                                TempData["Info"] = "You have not alloted any messages. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                return RedirectToAction("CustomSMS");
                                            }
                                        }
                                        else
                                        {
                                            TempData["Info"] = "Your Custom Message is not approved by soft support team. Please Contact them to approve your custom message.";
                                            return RedirectToAction("CustomSMS");
                                        }
                                    }
                                    else
                                    {
                                        TempData["Info"] = "Your Custom Message Module Status is not active. please go and active it to send the custom message";
                                        return RedirectToAction("CustomSMS");
                                    }

                                }
                                else
                                {
                                    TempData["Info"] = "Please add some custom message to proceed.";
                                    return RedirectToAction("CustomSMS");
                                }
                                //End Sending SMS

                            }
                            else
                            {
                                TempData["Info"] = "no student record found in selected parameters";
                                return RedirectToAction("CustomSMS");
                            }
                        }
                        else if (reg.classId != 0 && reg.secId != 0)
                        {
                            var getdata = con.std.Where(s => s.camId == reg.camId
                            && s.classId == reg.classId && s.sesId == reg.sesId && s.secId == reg.secId
                            && s.stdStatus == "Active" && s.IsDeleted == false).ToList();
                            if (getdata.Count != 0)
                            {
                                //For Sending SMS
                                var chkModule = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                                if (chkModule != null)
                                {
                                    if (chkModule.smStatus == true)
                                    {
                                        if (chkModule.isLocked == true)
                                        {
                                            EncryptDecrypt decryption = new EncryptDecrypt();
                                            var getAllotment = con.sMSAllotments.FirstOrDefault();
                                            if (getAllotment != null)
                                            {
                                                int RemainingMsg = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                var expiryDate = decryption.Decrypt(getAllotment.saExpiryDate);
                                                var Exdate = Convert.ToDateTime(expiryDate);
                                                var Status = decryption.Decrypt(getAllotment.saStatus);
                                                if (RemainingMsg > 0)
                                                {
                                                    if (Exdate > DateTime.Now)
                                                    {
                                                        if (Status == "Active")
                                                        {
                                                            //Sending SMS
                                                            int Qty = RemainingMsg;
                                                            foreach (var i in getdata)
                                                            {
                                                                if (Qty > 0)
                                                                {
                                                                    sendMessage.SendSMSTurab(i.pr.perContactOne, chkModule.smText);
                                                                    SentSMS sms = new SentSMS();
                                                                    sms.ssDate = DateTime.Now;
                                                                    sms.ssStatus = true;
                                                                    sms.perId = i.perId;
                                                                    sms.ssText = chkModule.smText;
                                                                    con.sentSMs.Add(sms);
                                                                    //Minus 1 SMS
                                                                    int msg = RemainingMsg - 1;
                                                                    var RemMsg = msg.ToString();
                                                                    getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());
                                                                    con.SaveChanges();
                                                                    Qty = Qty - 1;
                                                                }
                                                                else
                                                                {
                                                                    var chkQty = con.sMSAllotments.FirstOrDefault();
                                                                    int Remaining = Convert.ToInt32(decryption.Decrypt(getAllotment.saRemainingQty));
                                                                    if (Remaining <= 0)
                                                                    {
                                                                        TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                                        return RedirectToAction("CustomSMS");
                                                                    }

                                                                }

                                                            }
                                                        }
                                                        else
                                                        {
                                                            TempData["Info"] = "You SMS Allotment is active. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                            return RedirectToAction("CustomSMS");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        TempData["Info"] = "You SMS Allotment is expired. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                        return RedirectToAction("CustomSMS");
                                                    }

                                                }
                                                else
                                                {
                                                    TempData["Info"] = "You SMS Quantity is not sufficient. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                    return RedirectToAction("CustomSMS");
                                                }
                                            }
                                            else
                                            {
                                                TempData["Info"] = "You have not alloted any messages. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                return RedirectToAction("CustomSMS");
                                            }
                                        }
                                        else
                                        {
                                            TempData["Info"] = "Your Custom Message is not approved by soft support team. Please Contact them to approve your custom message.";
                                            return RedirectToAction("CustomSMS");
                                        }
                                    }
                                    else
                                    {
                                        TempData["Info"] = "Your Custom Message Module Status is not active. please go and active it to send the custom message";
                                        return RedirectToAction("CustomSMS");
                                    }

                                }
                                else
                                {
                                    TempData["Info"] = "Please add some custom message to proceed.";
                                    return RedirectToAction("CustomSMS");
                                }
                                //End Sending SMS

                            }
                            else
                            {
                                TempData["Info"] = "no student record found in selected parameters";
                                return RedirectToAction("CustomSMS");
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Please Select Campus and Session to proceed";
                    }
                }
                else if (StdMsg != null && StdMsg != "")
                {
                    if (reg.stdRollNo != "" && reg.stdRollNo != null)
                    {
                        var getStd = con.std.Where(s => s.stdRollNo == reg.stdRollNo).FirstOrDefault();
                        if (getStd != null)
                        {
                            if (getStd.stdStatus == "Active")
                            {
                                //For Sending SMS
                                var chkModule = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                                if (chkModule != null)
                                {
                                    if (chkModule.smStatus == true)
                                    {
                                        if (chkModule.isLocked == true)
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
                                                    sendMessage.SendSMSTurab(getStd.pr.perContactOne, chkModule.smText);
                                                    SentSMS sms = new SentSMS();
                                                    sms.ssDate = DateTime.Now;
                                                    sms.ssStatus = true;
                                                    sms.perId = getStd.perId;
                                                    sms.ssText = chkModule.smText;
                                                    con.sentSMs.Add(sms);
                                                    //Minus 1 SMS
                                                    int msg = RemainingMsg - 1;
                                                    var RemMsg = msg.ToString();
                                                    getAllotment.saRemainingQty = decryption.Encrypt(msg.ToString());
                                                    con.SaveChanges();
                                                    TempData["Success"] = "Custom Message Sent successfully";
                                                }
                                                else
                                                {
                                                    TempData["Info"] = "Your custom message has not been sent. Please check your remaining SMS quantity or Expiry Date or Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                    return RedirectToAction("CustomSMS");
                                                }
                                            }
                                            else
                                            {
                                                TempData["Info"] = "You have not alloted any messages. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                                                return RedirectToAction("CustomSMS");
                                            }
                                        }
                                        else
                                        {
                                            TempData["Info"] = "Your Custom Message is not approved by soft support team. Please Contact them to approve your custom message.";
                                            return RedirectToAction("CustomSMS");
                                        }
                                    }
                                    else
                                    {
                                        TempData["Info"] = "Your Custom Message Module Status is not active. please go and active it to send the custom message";
                                        return RedirectToAction("CustomSMS");
                                    }

                                }
                                else
                                {
                                    TempData["Info"] = "Please add some custom message to proceed.";
                                    return RedirectToAction("CustomSMS");
                                }
                            }
                            else
                            {
                                TempData["Info"] = "This " + reg.stdRollNo + " roll Number status is " + getStd.stdStatus + ". please active status to send message";
                                return RedirectToAction("CustomSMS");
                            }
                        }
                        else
                        {
                            TempData["Info"] = "No Record found of this " + reg.stdRollNo + " roll Number.";
                            return RedirectToAction("CustomSMS");
                        }
                    }

                    //End Sending SMS
                }
                else if (fileMsg != null && fileMsg != "")
                {

                }
                return RedirectToAction("CustomSMS");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error. Please contact to soft support "+SoftSupport.SoftContactNo+"";
                return RedirectToAction("CustomSMS");
            }
        }

        public ActionResult AddMsgText(string MsgText)
        {
            if (MsgText != null && MsgText != "")
            {
                bool AlertMsg;
                SMSModule sms = new SMSModule();
                SendMessage msg = new SendMessage();
                var chkMsg = con.smsModule.Where(s => s.mnId == 7).FirstOrDefault();
                if (chkMsg != null)
                {
                    chkMsg.smText = MsgText;
                    if (msg.MaskingName == "SMS Alert")
                    {
                        if (chkMsg.smText == MsgText)
                        {
                            if (chkMsg.isLocked == true)
                            {
                                chkMsg.isLocked = true;
                            }
                            else
                            {
                                chkMsg.isLocked = false;
                            }
                        }
                    }
                    else
                    {
                        chkMsg.isLocked = true;
                    }
                    chkMsg.smStatusChangeDate = DateTime.Now;
                    con.SaveChanges();
                    AlertMsg = chkMsg.isLocked;
                    return Json(new { Success = "true", Data = new { MsgText, AlertMsg } });
                }
                else
                {
                    sms.mnId = 7;
                    sms.smText = MsgText;
                    sms.smStatus = true;
                    sms.smStatusChangeDate = DateTime.Now;
                    if (msg.MaskingName == "SMS Alert")
                    {
                        sms.isLocked = false;
                    }
                    else
                    {
                        sms.isLocked = true;
                    }
                    con.smsModule.Add(sms);
                    con.SaveChanges();
                    AlertMsg = sms.isLocked;
                    return Json(new { Success = "true", Data = new { MsgText, AlertMsg } });
                }
            }
            else
            {
                return Json(new { Success = "false", Data = new { MsgText } });
            }
        }


        //Send OTP Test
        public ActionResult OTP()
        {
            return View();
        }

        [HttpPost]
        public ActionResult OTP(string number)
        {
            //string get10digit = number.Substring(number.Length - 10);
            //string completeNumber = "92" + get10digit;
            //string msg = Convert.ToString((new Random()).Next(100000));
            //string sApiKey = "b7d8297f-9435-4c12-b19f-ce3221f92212";
            //string sClientId = "a56b9346-7003-4575-a3d9-d8cb9c1c9cb3";
            //string sNumber = completeNumber;
            ////string sSID = "SMS+Alert";
            //string sMessage = "Your OTP for account registration is "+ msg;

            //string sURL = "https://my.forwardvaluesms.com/vendorsms/pushsms.aspx?&apikey=" + sApiKey + "&clientid=" + sClientId+ "&msisdn=" + sNumber + "&sid=FVALUESMS&msg=" + sMessage + " &fl= 0";

            //string sResponse = SMS.Models.OTP.SentOTP(sURL);
            //Response.Write(sResponse);

            string msg = Convert.ToString((new Random()).Next(100000));
            string sMessage = "Your OTP for account registration is " + msg;
            SMS.Models.OTP otp = new OTP();
            //otp.SendOTP(number, sMessage);
            sendMessage.SendSMSTurab(number, sMessage);
            ViewBag.MSG = "OTP Sent";
            return View();
        }

        public void PopulatCampus()
        {
            //Populating the dropdown for Campus
            SelectList sl = new SelectList(con.camp.ToList(), "camId", "campusname");
            ViewData["Campus"] = sl;
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

    }
}
