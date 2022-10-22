using SMS.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class IdentityCardController : Controller
    {
        DBCon con = new DBCon();
        // GET: IdentityCard
        public ActionResult PrintCard(int? id)
        {
            try
            {
                if (id == 1)
                {
                    TempData["Student"] = "Student";
                    return View(id);
                }
                if (id == 2)
                {
                    TempData["Person"] = "Person";
                    return View();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            return View();
        }
        [HttpPost]
        public ActionResult PrintCard(string RollNo, int? Code, int? id)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {

                }
                else
                {
                    if (id == 1)
                    {
                        if (RollNo != "" && RollNo != null)
                        {
                            var getStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                            if (getStd != null)
                            {
                                TempData["Std"] = getStd;
                                TempData["Student"] = "Student";
                                return View(id);
                            }
                            else
                            {
                                TempData["Error"] = "Incorrect Roll Number";
                                TempData["Student"] = "Student";
                                return View(id);
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll number";
                            TempData["Student"] = "Student";
                            return View(id);
                        }
                    }

                    else if (id == 2)
                    {
                        if (Code != 0 && Code != null)
                        {
                            var getPer = con.person.Where(s => s.perCode == Code).FirstOrDefault();
                            if (getPer != null)
                            {
                                if (getPer.roleId == 4)
                                {
                                    TempData["Error"] = "Parent Identity Card cannot be printed";
                                    TempData["Person"] = "Person";
                                    return View(id);
                                }
                                else
                                {
                                    TempData["Per"] = getPer;
                                    TempData["Person"] = "Person";
                                    return View(id);
                                }

                            }
                            else
                            {
                                TempData["Error"] = "Incorrect Code Number";
                                TempData["Person"] = "Person";
                                return View(id);
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Code number";
                            TempData["Person"] = "Person";
                            return View(id);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return View(id);
        }

        public ActionResult Card(string RollNo, int? Code)
        {
            try
            {
                if (RollNo != null && RollNo != "")
                {
                    var getStd = con.std.Where(s => s.stdRollNo == RollNo && s.stdStatus == "Active").FirstOrDefault();
                    if (getStd != null)
                    {
                        TempData["Std"] = getStd;

                        //Barcode
                        
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (Bitmap bitMap = new Bitmap(RollNo.Length * 40, 80))
                            {
                                using (Graphics graphics = Graphics.FromImage(bitMap))
                                {
                                    Font oFont = new Font("IDAutomationHC39M", 16);
                                    PointF point = new PointF(2f, 2f);
                                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                                    graphics.DrawString("*" + RollNo + "*", oFont, blackBrush, point);
                                }

                                bitMap.Save(memoryStream, ImageFormat.Jpeg);

                                ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }
                        
                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Roll Number";
                        return RedirectToAction("PrintCard", new { id = 1 });
                    }
                }
                else if (Code != 0 && Code != null)
                {
                    var getPer = con.person.Where(s => s.perCode == Code).FirstOrDefault();
                    if (getPer != null)
                    {
                        TempData["Per"] = getPer;

                        //Barcode
                        string code = Code.ToString();

                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (Bitmap bitMap = new Bitmap(code.Length * 40, 80))
                            {
                                using (Graphics graphics = Graphics.FromImage(bitMap))
                                {
                                    Font oFont = new Font("IDAutomationHC39M", 16);
                                    PointF point = new PointF(2f, 2f);
                                    SolidBrush whiteBrush = new SolidBrush(Color.White);
                                    graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                                    SolidBrush blackBrush = new SolidBrush(Color.Black);
                                    graphics.DrawString("*" + code + "*", oFont, blackBrush, point);
                                }

                                bitMap.Save(memoryStream, ImageFormat.Jpeg);

                                ViewBag.BarcodeImage = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                            }
                        }




                        return View();
                    }
                    else
                    {
                        TempData["Error"] = "Incorrect Code Number";
                        return RedirectToAction("PrintCard", new { id = 2 });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public ActionResult BulkIDCard()
        {
            try
            {
                PopulatAllSes();
                PopulatClass();
                PopulatSec();
                return View();
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some issue please contact to soft support";
                return RedirectToAction("Logout", "Home");
            }
        }

        public ActionResult BulkCard(int sesId, int classId, int secId)
        {
            try
            {
                if (sesId != 0 && classId != 0 && secId != 0)
                {
                    var getStd = con.std.Where(s => s.sesId == sesId && s.classId == classId && s.secId == secId && s.stdStatus == "Active").ToList();
                    if (getStd.Count != 0)
                    {
                        TempData["StdList"] = getStd;

                        List<BulkBarcode> BulkList = new List<BulkBarcode>();
                        foreach (var i in getStd)
                        {
                            BulkBarcode code = new BulkBarcode();
                            code.StdId = i.stdId;
                            
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (Bitmap bitMap = new Bitmap(i.stdRollNo.Length * 40, 80))
                                {
                                    using (Graphics graphics = Graphics.FromImage(bitMap))
                                    {
                                        Font oFont = new Font("IDAutomationHC39M", 16);
                                        PointF point = new PointF(2f, 2f);
                                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);
                                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                                        graphics.DrawString("*" + i.stdRollNo + "*", oFont, blackBrush, point);
                                    }

                                    bitMap.Save(memoryStream, ImageFormat.Jpeg);

                                    code.Barcode = "data:image/png;base64," + Convert.ToBase64String(memoryStream.ToArray());
                                }
                            }
                            BulkList.Add(code);
                            TempData["BarcodeList"] = BulkList;
                        }
                        
                        return View();
                    }
                    else
                    {
                        TempData["Info"] = "No Record found";
                        return RedirectToAction("BulkIDCard");
                    }
                }
                else
                {
                    TempData["Error"] = "Please Select Session, Class and Section";
                    return RedirectToAction("BulkIDCard");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some issue please contact to soft support";
                return RedirectToAction("BulkIDCard");
            }
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