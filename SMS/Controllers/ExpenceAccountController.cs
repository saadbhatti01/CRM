
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class ExpenceAccountController : Controller
    {
        DBCon con = new DBCon();
        // GET: ExpenceAccount
        public ActionResult AddAccountHeader()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateHeader();
                    var getHeader = con.acheader.OrderByDescending(a => a.ahId).ToList();
                    if (getHeader.Count != 0)
                    {
                        List<ExpenseVM> exList = new List<ExpenseVM>();
                        foreach (var i in getHeader)
                        {
                            ExpenseVM e = new ExpenseVM();
                            var getsubheader = con.acheader.Where(a => a.ahId == i.subHeader).FirstOrDefault();
                            var getsubsubheader = con.acheader.Where(a => a.ahId == i.subSubHeader).FirstOrDefault();
                            e.ahId = i.ahId;
                            e.Header = i.headerName;
                            e.SubHeader = getsubheader.headerName;
                            e.SubSUbHeader = getsubsubheader.headerName;
                            e.Code = i.Code;
                            e.Visible = i.isVisible;
                            exList.Add(e);
                        }
                        TempData["Header"] = exList;
                    }
                    else
                    {
                        //TempData["No"] = "No Record found";
                    }
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View();
            }

        }

        [HttpPost]
        public ActionResult AddAccountHeader(AccountHeader ach)
        {
            try
            {
                PopulateHeader();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var checkName = con.acheader.Where(a => a.headerName == ach.headerName && a.subHeader == ach.subHeader && a.subSubHeader == ach.subSubHeader).Any();
                    if (checkName == false)
                    {
                        if(ach.subHeader == ach.subSubHeader)
                        {
                            var getLatestCode = con.acheader.Where(a => a.subHeader == a.subSubHeader).OrderByDescending(a => a.ahId).FirstOrDefault();
                            if(getLatestCode != null)
                            {
                                if(getLatestCode.Code == 1004)
                                {
                                    ach.Code = 2001;
                                }
                                else
                                {
                                    ach.Code = getLatestCode.Code + 1;
                                }
                                ach.isVisible = true;
                                ach.CreatedBy = LoginInfo.UserID;
                                ach.CreatedDate = DateTime.Now;
                                con.acheader.Add(ach);
                                con.SaveChanges();
                                TempData["Success"] = "Account Header added successfully";
                                return RedirectToAction("AddAccountHeader");
                            }

                        }
                        else
                        {
                            var getLatestcode = con.acheader.Where(a => a.subHeader != a.subSubHeader).OrderByDescending(a => a.ahId).FirstOrDefault();
                            if (getLatestcode != null)
                            {
                                ach.Code = getLatestcode.Code + 1;
                                ach.isVisible = true;
                                ach.CreatedBy = LoginInfo.UserID;
                                ach.CreatedDate = DateTime.Now;
                                con.acheader.Add(ach);
                                con.SaveChanges();
                                TempData["Success"] = "Account Header added successfully";
                                return RedirectToAction("AddAccountHeader");
                            }
                            else
                            {
                                ach.Code = 3001;
                                ach.isVisible = true;
                                ach.CreatedBy = LoginInfo.UserID;
                                ach.CreatedDate = DateTime.Now;
                                con.acheader.Add(ach);
                                con.SaveChanges();
                                TempData["Success"] = "Account Header added successfully";
                                return RedirectToAction("AddAccountHeader");
                            }
                        }
                    }
                    else
                    {
                        TempData["Error"] = "Account Header Name already exist";
                        return RedirectToAction("AddAccountHeader");
                    }

                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Account Header not added";
                return RedirectToAction("AddAccountHeader");
            }
            return RedirectToAction("AddAccountHeader");
        }

        public ActionResult EditAccountHeader(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateHeader();
                    var getheader = con.acheader.Where(a => a.ahId == id).FirstOrDefault();
                    if (getheader != null)
                    {
                        if (getheader.ahId == getheader.subHeader && getheader.ahId == getheader.subSubHeader)
                        {
                            TempData["Error"] = "You cannot Edit this header.";
                            return RedirectToAction("AddAccountHeader");
                        }
                        else
                        {
                            return View(getheader);
                        }
                    }
                    else
                    {
                        TempData["Error"] = "No record found";
                        return RedirectToAction("AddAccountHeader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "header cannot be Edited";
                return RedirectToAction("AddAccountHeader");
            }

        }

        [HttpPost]
        public ActionResult EditAccountHeader(AccountHeader ah, int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateHeader();
                    var getheader = con.acheader.Where(a => a.ahId == id).FirstOrDefault();
                    if (getheader != null)
                    {
                        // getheader.ahId = ae.ahId;
                        getheader.headerName = ah.headerName;
                        getheader.isVisible = ah.isVisible;
                        getheader.UpdatedBy = LoginInfo.UserID;
                        getheader.UpdatedDate = DateTime.Now;
                        con.SaveChanges();
                        TempData["Success"] = "Header updated successfully";
                        return RedirectToAction("AddAccountHeader");
                    }
                    else
                    {
                        TempData["Error"] = "No record found";
                        return RedirectToAction("AddAccountHeader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Not updated";
                return RedirectToAction("AddAccountHeader");
            }

        }

        public ActionResult DelAccountHeader(int id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateHeader();
                    var getheader = con.acheader.Where(a => a.ahId == id).FirstOrDefault();
                    if (getheader != null)
                    {
                        if (getheader.ahId == getheader.subHeader && getheader.ahId == getheader.subSubHeader)
                        {
                            TempData["Error"] = "You cannot delete this header beacuse this is main expense account";
                            return RedirectToAction("AddAccountHeader");
                        }
                        else
                        {
                            var chkHeader = con.acEntry.Where(a => a.ahId == id).Any();
                            if (chkHeader == false)
                            {
                                con.acheader.Remove(getheader);
                                con.SaveChanges();
                                TempData["Success"] = "Header Deleted Successfully";
                                return RedirectToAction("AddAccountHeader");
                            }
                            else
                            {
                                TempData["Error"] = "You cannot delete this header beacuse this header is associated with an Expense Detail.";
                                return RedirectToAction("AddAccountHeader");
                            }

                        }
                    }
                    else
                    {
                        TempData["Error"] = "No record found";
                        return RedirectToAction("AddAccountHeader");
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "header not deleted";
                return RedirectToAction("AddAccountHeader");
            }

        }

        public ActionResult AddAccountEntries()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    PopulateHeaderforEntries();

                    var getAcEntries = con.acEntry.OrderByDescending(a => a.aeId).ToList();
                    TempData["AccEntries"] = getAcEntries;
                    return View();
                }

            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        public ActionResult AddAccountEntries(AccountEntries ace)
        {
            try
            {
                PopulateHeaderforEntries();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    ace.isLocked = false;
                    ace.CreatedBy = LoginInfo.UserID;
                    ace.CreatedDate = DateTime.Now;
                    con.acEntry.Add(ace);
                    con.SaveChanges();
                    TempData["Success"] = "Expense Entry added successfully";
                    return RedirectToAction("AddAccountEntries");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Expense Entry not added";
                return RedirectToAction("AddAccountEntries");
            }

        }

        public ActionResult EditAccountEntries(int id)
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
                    PopulateHeaderforEntries();
                    var getAcEntry = con.acEntry.Where(a => a.aeId == id).FirstOrDefault();
                    if (RoleId == 1)
                    {
                        return View(getAcEntry);
                    }
                    else
                    {
                        if (getAcEntry.isLocked == true)
                        {
                            TempData["Error"] = "You are not allowed to Edit this entry because this entry is locked by admin";
                            return RedirectToAction("AddAccountEntries");
                        }
                        else
                        {
                            return View(getAcEntry);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Expense Entry not updated";
                return RedirectToAction("AddAccountEntries");
            }
        }

        [HttpPost]
        public ActionResult EditAccountEntries(AccountEntries ae, int id)
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
                    PopulateHeaderforEntries();
                    var getAcEntry = con.acEntry.Where(a => a.aeId == id).FirstOrDefault();
                    if (RoleId == 1)
                    {
                        getAcEntry.ahId = ae.ahId;
                        getAcEntry.amount = ae.amount;
                        getAcEntry.description = ae.description;
                        getAcEntry.UpdatedBy = LoginInfo.UserID;
                        getAcEntry.UpdatedDate = DateTime.Now;
                        con.SaveChanges();
                        TempData["Success"] = "Entry updated successfully";
                        return RedirectToAction("AddAccountEntries");
                    }
                    else
                    {
                        if (getAcEntry.isLocked == true)
                        {
                            TempData["Error"] = "You are not allowed to Edit this entry because this entry is locked by admin";
                            return RedirectToAction("AddAccountEntries");
                        }
                        else
                        {
                            getAcEntry.ahId = ae.ahId;
                            getAcEntry.amount = ae.amount;
                            getAcEntry.description = ae.description;
                            getAcEntry.UpdatedBy = LoginInfo.UserID;
                            getAcEntry.UpdatedDate = DateTime.Now;
                            con.SaveChanges();
                            TempData["Success"] = "Entry updated successfully";
                            return RedirectToAction("AddAccountEntries");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Expense Entry not updated";
                return RedirectToAction("AddAccountEntries");
            }
        }
        public ActionResult DelAccountEntries(int id)
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
                    var getAcEntry = con.acEntry.Where(a => a.aeId == id).FirstOrDefault();
                    if (getAcEntry != null)
                    {
                        if(RoleId == 1)
                        {
                            con.acEntry.Remove(getAcEntry);
                            con.SaveChanges();
                            TempData["Success"] = "Expense Detail Deleted Successfully";
                            return RedirectToAction("AddAccountEntries");
                        }
                        else if(RoleId == 2)
                        {
                            if(getAcEntry.isLocked == true)
                            {
                                TempData["Error"] = "You cannot delete this entry because this entry is locked by Admin";
                                return RedirectToAction("AddAccountEntries");
                            }
                        }
                        
                    }
                }
                return RedirectToAction("AddAccountEntries");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Expense Entry not deleted";
                return RedirectToAction("AddAccountEntries");
            }
        }

        public ActionResult AccountEntryLocked()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId == 1)
                {
                    var getRecord = con.acEntry.ToList();
                    if(getRecord.Count != 0)
                    {
                        var Entries = getRecord.Where(s => s.isLocked == false).ToList();
                        if(Entries.Count != 0)
                        {
                            TempData["AccountEntryLocked"] = Entries;
                            return View();
                        }
                        else
                        {
                            TempData["Info"] = "All Expense Entries locked down";
                            return View();
                        }
                    }
                    
                    //var getRecord = con.acEntry.Where(s => s.isLocked == false).ToList();
                    //if (getRecord.Count != 0)
                    //{
                    //    TempData["AccountEntryLocked"] = getRecord;
                    //    return View();
                    //}
                    //else
                    //{
                    //    TempData["Info"] = "All Expense Entries locked down";
                    //    return View();
                    //}
                }
                else
                {
                    TempData["Error"] = "This is page is desigend for Admin";
                    return RedirectToAction("Logout", "Home");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error!";
                return RedirectToAction("AddAccountEntries");
            }
            return View();
        }


        public ActionResult AccountEntryLockedDown(string val, string unChecked)
        {
            if (val != null)
            {
                int aeId = Convert.ToInt32(val);
                var getRecord = con.acEntry.Where(a => a.aeId == aeId).FirstOrDefault();
                if (getRecord != null)
                {
                    getRecord.isLocked = true;
                    con.SaveChanges();
                    return Json(new { Success = "true", Data = new { } });
                }
            }

            if (unChecked != null)
            {
                int aeId = Convert.ToInt32(unChecked);
                var getRecord = con.acEntry.Where(a => a.aeId == aeId).FirstOrDefault();
                if (getRecord != null)
                {
                    getRecord.isLocked = false;
                    con.SaveChanges();
                    return Json(new { Success = "true", Data = new { } });
                }
            }

            return Json(new { Success = "false" });
        }

        public ActionResult ExpenseHeaderRpt()
        {
            try
            {
                PopulateHeader();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult _ExpenseRpt(string Header, string ToDate, string FromDate, string Status)
        {
            try
            {
                List<AccountEntries> GetDetails = new List<AccountEntries>();
                if (Status == "Both")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId).ToList();
                        foreach(var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if(GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }
                            
                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.subSubHeader == headerId).FirstOrDefault();
                            if(getheaderName != null)
                            {
                                TempData["HeaderName"] = getheaderName.headerName;
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;

                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else if(Status == "Locked")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId).ToList();
                        foreach (var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == true).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if (GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }
                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == true).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId).ToList();
                        foreach (var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == false).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if (GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }
                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == false).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error to generateing the report";
                return PartialView("_ExpenseRpt");
            }
            return PartialView("_ExpenseRpt");

        }
        public ActionResult ExpenseSHeaderRpt()
        {
            try
            {
                PopulateHeader();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult _ExpenseSubRpt(string Header, string SubHeader, string ToDate, string FromDate, string Status)
        {
            try
            {
                List<AccountEntries> GetDetails = new List<AccountEntries>();
                if (Status == "Both")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId && a.subHeader == subheaderId).ToList();
                        foreach (var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if (GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }

                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if(getsubheaderName != null)
                                {
                                    TempData["HeaderName"] = getheaderName.headerName +","+ getsubheaderName.headerName;
                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;

                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else if (Status == "Locked")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId && a.subHeader == subheaderId).ToList();
                        foreach (var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == true).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if (GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }
                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == true).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if (getsubheaderName != null)
                                {
                                    TempData["HeaderName"] = getheaderName.headerName + "," + getsubheaderName.headerName;
                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        var getList = con.acheader.Where(a => a.subSubHeader == headerId && a.subHeader == subheaderId).ToList();
                        foreach (var i in getList)
                        {
                            var GetDetail = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == i.ahId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == false).OrderByDescending(c => c.aeId).FirstOrDefault();
                            if (GetDetail != null)
                            {
                                GetDetails.Add(GetDetail);
                            }
                        }
                        //var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == false).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if (getsubheaderName != null)
                                {
                                    TempData["HeaderName"] = getheaderName.headerName + "," + getsubheaderName.headerName;
                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error to generateing the report";
                return PartialView("_ExpenseRpt");
            }
            return PartialView("_ExpenseRpt");

        }
        public ActionResult ExpenseSSHeaderRpt()
        {
            try
            {
                PopulateHeader();
                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public ActionResult _ExpensesubSubRpt(string Header, string SubHeader, string ssHeader, string ToDate, string FromDate, string Status)
        {
            try
            {
                //List<AccountEntries> GetDetails = new List<AccountEntries>();
                if (Status == "Both")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        int subSubheaderId = Convert.ToInt32(ssHeader);
                        var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == subSubheaderId && c.CreatedDate >= From && c.CreatedDate <= To).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if (getsubheaderName != null)
                                {
                                    var getsubSubheaderName = con.acheader.Where(a => a.ahId == subSubheaderId).FirstOrDefault();
                                    if (getsubSubheaderName != null)
                                    {
                                        TempData["HeaderName"] = getheaderName.headerName + "," + getsubheaderName.headerName + "," + getsubSubheaderName.headerName;
                                    }
                                    
                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else if (Status == "Locked")
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        int subSubheaderId = Convert.ToInt32(ssHeader);
                        var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == subSubheaderId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == true).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if (getsubheaderName != null)
                                {
                                    var getsubSubheaderName = con.acheader.Where(a => a.ahId == subSubheaderId).FirstOrDefault();
                                    if (getsubSubheaderName != null)
                                    {
                                        TempData["HeaderName"] = getheaderName.headerName + "," + getsubheaderName.headerName + "," + getsubSubheaderName.headerName;
                                    }

                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
                else
                {
                    if (ToDate != null && ToDate != "" && FromDate != null && FromDate != "")
                    {
                        DateTime From = Convert.ToDateTime(FromDate);
                        DateTime To = Convert.ToDateTime(ToDate);
                        int headerId = Convert.ToInt32(Header);
                        int subheaderId = Convert.ToInt32(SubHeader);
                        int subSubheaderId = Convert.ToInt32(ssHeader);
                        var GetDetails = con.acEntry.Where(c => c.IsDeleted == false && c.ahId == subSubheaderId && c.CreatedDate >= From && c.CreatedDate <= To && c.isLocked == false).OrderByDescending(c => c.aeId).ToList();
                        if (GetDetails.Count != 0)
                        {
                            var getheaderName = con.acheader.Where(a => a.ahId == headerId).FirstOrDefault();
                            if (getheaderName != null)
                            {
                                var getsubheaderName = con.acheader.Where(a => a.ahId == subheaderId).FirstOrDefault();
                                if (getsubheaderName != null)
                                {
                                    var getsubSubheaderName = con.acheader.Where(a => a.ahId == subSubheaderId).FirstOrDefault();
                                    if (getsubSubheaderName != null)
                                    {
                                        TempData["HeaderName"] = getheaderName.headerName + "," + getsubheaderName.headerName + "," + getsubSubheaderName.headerName;
                                    }

                                }
                            }
                            ViewBag.ReportNo = 1;
                            TempData["List"] = GetDetails;
                            TempData["fDate"] = FromDate;
                            TempData["tDate"] = ToDate;
                            TempData["Status"] = Status;
                        }
                        else
                        {
                            TempData["No"] = "No Record found.";
                        }
                        return PartialView("_ExpenseRpt");
                    }
                    else
                    {
                        TempData["No"] = "Please Select Start date and End Date For the report.";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["No"] = "There is some error to generateing the report";
                return PartialView("_ExpenseRpt");
            }
            return PartialView("_ExpenseRpt");
        }

        public void PopulateHeader()
        {
            //Populating the dropdown for Header
            List<AccountHeader> achList = new List<AccountHeader>();

            var getHeader = con.acheader.Where(a => a.ahId == a.subHeader && a.ahId == a.subSubHeader && a.isVisible == true).ToList();

            foreach(var i in getHeader)
            {
                AccountHeader ac = new AccountHeader();
                ac.ahId = i.ahId;
                ac.headerName = i.headerName + "-" + i.Code;
                achList.Add(ac);
            }

            SelectList sl = new SelectList(achList, "ahId", "headerName");
            ViewData["Header"] = sl;
        }

        public ActionResult PopulatSubHeaderForHeader(int HeaderId)
        {
            try
            {
                //Populating the dropdown for subHeader
                List<AccountHeader> achList = new List<AccountHeader>();

                var getHeader = con.acheader.Where(p => p.subSubHeader == HeaderId && p.subHeader == HeaderId && p.isVisible == true).ToList();

                foreach (var i in getHeader)
                {
                    AccountHeader ac = new AccountHeader();
                    ac.ahId = i.ahId;
                    ac.headerName = i.headerName + "-" + i.Code;
                    achList.Add(ac);
                }


                SelectList objheader = new SelectList(achList, "ahId", "headerName");
                return Json(objheader);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
        public void PopulateHeaderforEntries()
        {
            //Populating the dropdown for Header
            List<AccountHeader> achList = new List<AccountHeader>();

            var getHeader = con.acheader.Where(a => a.ahId != a.subHeader && a.ahId != a.subSubHeader && a.isVisible == true).ToList();

            foreach (var i in getHeader)
            {
                AccountHeader ac = new AccountHeader();
                ac.ahId = i.ahId;
                ac.headerName = i.headerName + "-" + i.Code;
                achList.Add(ac);
            }

            SelectList sl = new SelectList(achList, "ahId", "headerName");
            ViewData["Header"] = sl;
        }

        
        public ActionResult PopulatSubHeader(int HeaderId)
        {
            try
            {
                //Populating the dropdown for subHeader
                List<AccountHeader> achList = new List<AccountHeader>();

                var getHeader = con.acheader.Where(p => p.subSubHeader == HeaderId && p.subHeader == HeaderId && p.ahId != HeaderId && p.isVisible == true).ToList();

                foreach (var i in getHeader)
                {
                    AccountHeader ac = new AccountHeader();
                    ac.ahId = i.ahId;
                    ac.headerName = i.headerName + "-" + i.Code;
                    achList.Add(ac);
                }

                SelectList objheader = new SelectList(achList, "ahId", "headerName");
                return Json(objheader);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }

        public ActionResult PopulatssHeader(int ssheader)
        {
            try
            {
                //Populating the dropdown for subHeader

                List<AccountHeader> achList = new List<AccountHeader>();

                var getHeader = con.acheader.Where(p => p.subHeader == ssheader && p.isVisible == true).ToList();

                foreach (var i in getHeader)
                {
                    AccountHeader ac = new AccountHeader();
                    ac.ahId = i.ahId;
                    ac.headerName = i.headerName + "-" + i.Code;
                    achList.Add(ac);
                }
                SelectList objheader = new SelectList(achList, "ahId", "headerName");
                return Json(objheader);
            }
            catch (Exception ex)
            {

            }
            return Json(null);
        }
    }
}