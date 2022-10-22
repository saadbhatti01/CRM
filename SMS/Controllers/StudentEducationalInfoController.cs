using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class StudentEducationalInfoController : Controller
    {
        DBCon con = new DBCon();
        // GET: StudentEducationalInfo
        public ActionResult Degree()
        {
            try
            {
                var getData = con.degrees.ToList();
                if(getData.Count != 0)
                {
                    TempData["Data"] = getData;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult Degree(Degree dg)
        {
            try
            {
                var chkDegreeName = con.degrees.Where(b => b.DegreeName == dg.DegreeName).Any();
                if (chkDegreeName == false)
                {
                    con.degrees.Add(dg);
                    con.SaveChanges();
                    TempData["Success"] = "Degree Name Added successfully";
                    return RedirectToAction("Degree");
                }
                else
                {
                    TempData["Error"] = "This Degree name is already exist. please choose a different bank name";
                    return RedirectToAction("Degree");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Degree name cannot to added. please contact to soft support";
                return RedirectToAction("Degree");
            }
        }

        public ActionResult EditDegree(int id)
        {
            try
            {
                var findData = con.degrees.Find(id);
                if (findData != null)
                {
                    return View(findData);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("Degree");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Degree name cannot updated. please contact to soft support";
                return RedirectToAction("Degree");
            }
        }
        [HttpPost]
        public ActionResult EditDegree(int id, Degree dg)
        {
            try
            {
                var findData = con.degrees.Find(id);
                if (findData != null)
                {
                    findData.DegreeName = dg.DegreeName;
                    findData.IsVisible = dg.IsVisible;
                    con.SaveChanges();
                    TempData["Success"] = "Degree Name updated successfully";
                    return RedirectToAction("Degree");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("Degree");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Degree name cannot updated. please contact to soft support";
                return RedirectToAction("Degree");
            }
        }
        public ActionResult DeleteDegree(int id)
        {
            try
            {
                var findDegree = con.stdEduInfos.Where(b => b.DegreeId == id).Any();
                if (findDegree == false)
                {
                    var getData = con.degrees.Find(id);
                    if (getData != null)
                    {
                        con.degrees.Remove(getData);
                        con.SaveChanges();
                        TempData["Success"] = "Degree name deleted successfully";
                        return RedirectToAction("Degree");
                    }
                    else
                    {

                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("Degree");
                    }
                }
                else
                {
                    TempData["Info"] = "Degree name cannot be deleted because it is associated with some information";
                    return RedirectToAction("BaDegreenk");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Degree name cannot updated. please contact to soft support";
                return RedirectToAction("Degree");
            }
        }
        
        public ActionResult StudentInstitute()
        {
            try
            {
                var getData = con.stdInfos.ToList();
                if (getData.Count != 0)
                {
                    TempData["Data"] = getData;
                    return View();
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }
            return View();
        }
        [HttpPost]
        public ActionResult StudentInstitute(StudentInsInfo info)
        {
            try
            {
                var chkName = con.stdInfos.Where(b => b.Name == info.Name).Any();
                if (chkName == false)
                {
                    con.stdInfos.Add(info);
                    con.SaveChanges();
                    TempData["Success"] = "Institute Name Added successfully";
                    return RedirectToAction("StudentInstitute");
                }
                else
                {
                    TempData["Error"] = "Institute name is already exist. please choose a different Institute name";
                    return RedirectToAction("StudentInstitute");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Institute name cannot to added. please contact to soft support";
                return RedirectToAction("StudentInstitute");
            }
        }

        public ActionResult EditStudentInstitute(int id)
        {
            try
            {
                var findData = con.stdInfos.Find(id);
                if (findData != null)
                {
                    return View(findData);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("StudentInstitute");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Institute Name cannot updated. please contact to soft support";
                return RedirectToAction("StudentInstitute");
            }
        }
        [HttpPost]
        public ActionResult EditStudentInstitute(int id, StudentInsInfo info)
        {
            try
            {
                var findData = con.stdInfos.Find(id);
                if (findData != null)
                {
                    findData.Name =info.Name;
                    findData.IsVisible = info.IsVisible;
                    con.SaveChanges();
                    TempData["Success"] = "Institute Name updated successfully";
                    return RedirectToAction("StudentInstitute");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("StudentInstitute");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Institute name cannot updated. please contact to soft support";
                return RedirectToAction("StudentInstitute");
            }
        }
        public ActionResult DeleteStudentInstitute(int id)
        {
            try
            {
                var findDegree = con.stdEduInfos.Where(b => b.StdInsId == id).Any();
                if (findDegree == false)
                {
                    var getData = con.stdInfos.Find(id);
                    if (getData != null)
                    {
                        con.stdInfos.Remove(getData);
                        con.SaveChanges();
                        TempData["Success"] = "Institute name deleted successfully";
                        return RedirectToAction("StudentInstitute");
                    }
                    else
                    {

                        TempData["Error"] = "No Record Found";
                        return RedirectToAction("StudentInstitute");
                    }
                }
                else
                {
                    TempData["Info"] = "Institute name cannot be deleted because it is associated with some information";
                    return RedirectToAction("StudentInstitute");
                }

            }
            catch (Exception ex)
            {
                TempData["Error"] = "Institute name cannot updated. please contact to soft support";
                return RedirectToAction("StudentInstitute");
            }
        }
    }
}