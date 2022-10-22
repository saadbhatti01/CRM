using SMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class BankInfoController : Controller
    {
        DBCon con = new DBCon();
        // GET: BankInfo
        public ActionResult Bank()
        {
            try
            {
                var bankList = con.banks.ToList();
                if (bankList.Count != 0)
                {
                    TempData["Bank"] = bankList;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error. please contact to soft support";
            }
            return View();
        }
        [HttpPost]
        public ActionResult Bank(Bank bnk, HttpPostedFileBase file)
        {
            try
            {
                var chkbankName = con.banks.Where(b => b.BankName == bnk.BankName).Any();
                if (chkbankName == false)
                {
                    if (file == null)
                    {
                        bnk.BankLogo = "avatar.jpg";
                    }
                    else
                    {
                        string Imagename = Path.GetFileName(file.FileName);
                        string PhysicalPath = Path.Combine(Server.MapPath("~/Images/BankLogo/"), Imagename);
                        file.SaveAs(PhysicalPath);
                        bnk.BankLogo = Imagename;
                    }
                    con.banks.Add(bnk);
                    con.SaveChanges();
                    TempData["Success"] = "Bank Added successfully";
                    return RedirectToAction("Bank");
                }
                else
                {
                    TempData["Error"] = "This Bank is already exist. please choose a different bank name";
                    return RedirectToAction("Bank");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot to added. please contact to soft support";
                return RedirectToAction("Bank");
            }
        }

        public ActionResult EditBank(int id)
        {
            try
            {
                var findBank = con.banks.Find(id);
                if (findBank != null)
                {
                    return View(findBank);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("Bank");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot updated. please contact to soft support";
                return RedirectToAction("Bank");
            }
        }
        [HttpPost]
        public ActionResult EditBank(int id, Bank bnk, HttpPostedFileBase file)
        {
            try
            {
                var findBank = con.banks.Find(id);
                if (findBank != null)
                {
                    if (file == null)
                    {

                    }
                    else
                    {
                        string Imagename = Path.GetFileName(file.FileName);
                        string PhysicalPath = Path.Combine(Server.MapPath("~/Images/BankLogo/"), Imagename);
                        file.SaveAs(PhysicalPath);
                        findBank.BankLogo = Imagename;
                    }
                    findBank.BankName = bnk.BankName;
                    findBank.IsVisible = bnk.IsVisible;
                    con.SaveChanges();
                    TempData["Success"] = "Bank updated successfully";
                    return RedirectToAction("Bank");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("Bank");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot updated. please contact to soft support";
                return RedirectToAction("Bank");
            }
        }
        public ActionResult DeleteBank(int id)
        {
            try
            {
                var findBank = con.bankinfos.Where(b => b.BankId == id).Any();
                if (findBank == false)
                {
                    var getBank = con.banks.Find(id);
                    if (getBank != null)
                    {
                        con.banks.Remove(getBank);
                        con.SaveChanges();
                        TempData["Success"] = "Bank deleted successfully";
                        return RedirectToAction("Bank");
                    }
                    else
                    {

                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("Bank");
                    }
                }
                else
                {
                    TempData["Info"] = "This bank name cannot be deleted because it is associated with some information";
                    return RedirectToAction("Bank");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot updated. please contact to soft support";
                return RedirectToAction("Bank");
            }
        }
        
        public ActionResult BankInfo()
        {
            try
            {
                PopulatBank();
                var bankInfoList = con.bankinfos.ToList();
                if (bankInfoList.Count != 0)
                {
                    TempData["BankInfo"] = bankInfoList;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error. please contact to soft support";
            }
            return View();
        }
        [HttpPost]
        public ActionResult BankInfo(BankInfo bnk, HttpPostedFileBase file)
        {
            try
            {
                PopulatBank();
                var chkAccNo = con.bankinfos.Where(b => b.AcNumber == bnk.AcNumber).Any();
                if (chkAccNo == false)
                {
                    con.bankinfos.Add(bnk);
                    con.SaveChanges();
                    TempData["Success"] = "Bank Added successfully";
                    return RedirectToAction("BankInfo");
                }
                else
                {
                    TempData["Error"] = "This Account Number is already exist. please choose a different Account Number.";
                    return RedirectToAction("BankInfo");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot to added. please contact to soft support";
                return RedirectToAction("BankInfo");
            }
        }

        public ActionResult EditBankInfo(int id)
        {
            try
            {
                PopulatBank();
                var findBank = con.bankinfos.Find(id);
                if (findBank != null)
                {
                    return View(findBank);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("BankInfo");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot updated. please contact to soft support";
                return RedirectToAction("BankInfo");
            }
        }
        [HttpPost]
        public ActionResult EditBankInfo(int id, BankInfo bnk)
        {
            try
            {
                PopulatBank();
                var findBank = con.bankinfos.Find(id);
                if (findBank != null)
                {
                    var chkCode = con.bankinfos.Where(b => b.AcNumber == bnk.AcNumber && b.BankInfoId != id).Any();
                    if (chkCode == false)
                    {
                        findBank.BankId = bnk.BankId;
                        findBank.AcNumber = bnk.AcNumber;
                        findBank.AcTitle = bnk.AcTitle;
                        findBank.BranchAddress = bnk.BranchAddress;
                        findBank.BranchContact = bnk.BranchContact;
                        findBank.IsVisible = bnk.IsVisible;
                        con.SaveChanges();
                        TempData["Success"] = "Bank updated successfully";
                        return RedirectToAction("BankInfo");
                    }
                    else
                    {
                        TempData["Error"] = "This Account number is already exist please choose a different one";
                        return View();
                    }

                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("BankInfo");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "BankInfo cannot updated. please contact to soft support";
                return RedirectToAction("BankInfo");
            }
        }
        public ActionResult DeleteBankInfo(int id)
        {
            try
            {

                var getBank = con.bankinfos.Find(id);
                if (getBank != null)
                {
                    con.bankinfos.Remove(getBank);
                    con.SaveChanges();
                    TempData["Success"] = "Bank deleted successfully";
                    return RedirectToAction("BankInfo");
                }
                else
                {

                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("BankInfo");
                }


            }
            catch (Exception ex)
            {
                TempData["Error"] = "Bank cannot updated. please contact to soft support";
                return RedirectToAction("BankInfo");
            }
        }

        public void PopulatBank()
        {
            //Populating the dropdown for Class
            SelectList sl = new SelectList(con.banks.Where(c => c.IsVisible == true).ToList(), "BankId", "BankName");
            ViewData["Bank"] = sl;
        }


    }
}