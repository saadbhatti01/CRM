
using SMS.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class EmployeeController : Controller
    {
        DBCon con = new DBCon();
        [CheckSession]
        // GET: Employee
        public ActionResult EmpRegistration()
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
                    PopulatRole();
                    PopulatCity();
                    return View();
                }


            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }

        }

        [HttpPost]
        public ActionResult EmpRegistration(Person per, HttpPostedFileBase perImage)
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
                    PopulatRole();
                    PopulatCity();

                    //Code for registration

                    if (per.perContactOne == "" || per.perContactOne == null)
                    {
                        per.perContactOne = "0123";
                    }

                    if (per.perContactTwo == "" || per.perContactTwo == null)
                    {
                        per.perContactTwo = "02123";
                    }

                    if (per.perCNIC == "" || per.perCNIC == null)
                    {
                        per.perCNIC = "33000";
                    }

                    if (per.perEmail == "" || per.perEmail == null)
                    {
                        per.perEmail = "a@a.a";
                    }

                    if (per.perBloodGrp == "" || per.perBloodGrp == null)
                    {
                        per.perBloodGrp = "notEntered";
                    }
                    if (per.perDOB == "" || per.perDOB == null)
                    {
                        per.perDOB = "notEntered";
                    }
                    if (per.perCast == "" || per.perCast == null)
                    {
                        per.perCast = "notEntered";
                    }
                    if (per.perReligion == "" || per.perReligion == null)
                    {
                        per.perBloodGrp = "notEntered";
                    }
                    if (per.perCurrentAdrs == "" || per.perCurrentAdrs == null)
                    {
                        per.perCurrentAdrs = "notEntered";
                    }
                    if (per.perPermanantAdrs == "" || per.perPermanantAdrs == null)
                    {
                        per.perPermanantAdrs = "notEntered";
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


                    var getmaxCode = con.person.Where(p => p.roleId != 3).OrderByDescending(p => p.perId).Select(s => s.perCode).FirstOrDefault();
                    if (getmaxCode != 0)
                    {
                        per.perCode = Convert.ToInt32(getmaxCode) + 1;
                    }
                    else
                    {
                        per.perCode = 500001;
                    }

                    //For Employee Login//
                    Login log = new Login();
                    log.usrName = per.perName;
                    log.usrLogin = per.perCode.ToString();
                    log.usrPassword = "abc123";
                    log.roleId = per.roleId;
                    log.usrStatus = "Active";
                    con.login.Add(log);
                    con.SaveChanges();
                    ////End Employee Login//// 

                    per.id = log.id;
                    per.CreatedBy = LoginInfo.UserID;
                    per.CreatedDate = DateTime.Now;
                    per.UpdatedBy = 0;
                    per.UpdatedDate = DateTime.Now;
                    per.IsDeleted = false;
                    per.DeletedBy = 0;
                    per.DeletedDate = DateTime.Now;
                    con.person.Add(per);
                    con.SaveChanges();

                    //for Attendance Code as barcode
                    if(per.roleId != 4)
                    {
                        PersonRegNo pr = new PersonRegNo();
                        pr.prBarcode = per.perCode.ToString();
                        pr.roleId = per.roleId;
                        pr.perId = per.perId;
                        con.perReg.Add(pr);
                        con.SaveChanges();
                    }
                    

                    TempData["Success"] = "Person Registration Successfully completed";
                    return RedirectToAction("EmpRegistration");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return View();
            }
        }

        public ActionResult EmpDetail(int id)
        {
            try
            {
                var getEmp = con.person.Where(p => p.perId == id).FirstOrDefault();
                if (getEmp != null)
                {
                    return View(getEmp);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ShowEmpData");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error to getting person info";
                return RedirectToAction("ShowEmpData");
            }
        }

        public ActionResult EmpUpdate(int id)
        {
            try
            {
                var getEmp = con.person.Where(p => p.perId == id).FirstOrDefault();
                if (getEmp != null)
                {
                    return View(getEmp);
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ShowEmpData");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error to getting person info";
                return RedirectToAction("ShowEmpData");
            }
        }

        [HttpPost]
        public ActionResult EmpUpdate(int id, Person pr)
        {
            try
            {
                var getEmp = con.person.Where(p => p.perId == id).FirstOrDefault();
                if (getEmp != null)
                {
                    getEmp.perName = pr.perName;
                    getEmp.perGarName = pr.perGarName;
                    if (pr.perDOB != "" && pr.perDOB != null)
                    {
                        getEmp.perDOB = pr.perDOB;
                    }
                    getEmp.perCurrentAdrs = pr.perCurrentAdrs;
                    getEmp.perPermanantAdrs = pr.perPermanantAdrs;
                    getEmp.perContactOne = pr.perContactOne;
                    getEmp.perContactTwo = pr.perContactTwo;
                    getEmp.perCNIC = pr.perCNIC;
                    getEmp.perEmail = pr.perEmail;
                    con.SaveChanges();
                    TempData["Success"] = "Person detail Updated successfully";
                    return RedirectToAction("ShowEmpData");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ShowEmpData");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error to getting person info";
                return RedirectToAction("ShowEmpData");
            }
        }

        public ActionResult DelEmp(int id)
        {
            try
            {
                var getEmp = con.person.Where(p => p.perId == id).FirstOrDefault();
                if (getEmp != null)
                {
                    var getLogin = con.login.Where(p => p.id == getEmp.id).FirstOrDefault();
                    var getPerReg = con.perReg.Where(pr => pr.perId == getEmp.perId).FirstOrDefault();
                    if(getPerReg != null)
                    {
                        con.perReg.Remove(getPerReg);
                    }
                    con.person.Remove(getEmp);
                    con.login.Remove(getLogin);
                    con.SaveChanges();
                    TempData["Success"] = "Employee Data deleted successfully";
                    return RedirectToAction("ShowEmpData");
                }
                else
                {
                    TempData["Error"] = "No Record Found";
                    return RedirectToAction("ShowEmpData");
                }


            }
            catch (Exception ex)
            {
                string InnerException = ex.InnerException.ToString();
                string value = "DELETE statement conflicted with the REFERENCE constraint";
                if (InnerException.Contains(value))
                {
                    TempData["Error"] = "This Person cannot deleted because it is linked with other information";
                }
                else
                {
                    TempData["Error"] = "Person not deleted please contact to soft support";
                }
                return RedirectToAction("ShowEmpData");
            }
        }
        public ActionResult ShowEmpData()
        {
            try
            {
                PopulatRole();
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2 && RoleId != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    var getEmp = con.person.Where(p => p.roleId != 3 && p.roleId != 10
 && p.IsDeleted == false).OrderByDescending(p => p.perId).ToList();
                    if (getEmp.Count != 0)
                    {
                        TempData["Emp"] = getEmp;
                        return View();
                    }
                    else
                    {
                        TempData["Info"] = "No Record Found";
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error";
                return View();
            }
            return View();
        }

        public ActionResult _PopulatePerson(int roleId)
        {
            try
            {
                var getData = con.person.Where(p => p.roleId == roleId).ToList();
                if(getData.Count != 0)
                {
                    TempData["Data"] = getData;
                    return PartialView("_PopulatePerson");
                }
                else
                {
                    TempData["No"] = "No Record Found";
                    return PartialView("_PopulatePerson");
                }
            }
            catch (Exception ex)
            {

            }
            return PartialView("_PopulatePerson");
        }
        public void PopulatRole()
        {
            //Populating the dropdown for City
            SelectList sl = new SelectList(con.role.Where(r => r.roleId != 3 && r.roleId != 10).ToList(), "roleId", "name");
            ViewData["Role"] = sl;
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
    }
}