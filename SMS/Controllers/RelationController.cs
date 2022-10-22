
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    public class RelationController : Controller
    {
        DBCon con = new DBCon();
        // GET: Relation
        [CheckSession]
        public ActionResult AddRelation()
        {
            try
            {
                PopulatParent();
                int role = Convert.ToInt32(Session["RoleId"]);
                if (role > 2 && role != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    Session.Clear();
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [CheckSession]
        [HttpPost]
        public ActionResult AddRelation(string RollNo, string submit, string StdName, string PerID, string LoginId)
        {
            try
            {
                PopulatParent();
                int role = Convert.ToInt32(Session["RoleId"]);
                if (role > 2 && role != 10)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    Session.Clear();
                    return RedirectToAction("LogInUser", "Home");
                }
                else
                {
                    if (submit == "Show")
                    {
                        if (RollNo != "" && RollNo != null)
                        {
                            var GetStd = con.std.Where(s => s.stdRollNo == RollNo).FirstOrDefault();
                            if (GetStd == null)
                            {
                                TempData["Error"] = "Incorrect Roll Number";
                                return View();
                            }
                            else
                            {
                                var chk = con.rel.Where(r => r.perId == GetStd.perId).FirstOrDefault();
                                if (chk != null)
                                {
                                    var getStdName = con.std.Where(s => s.perId == chk.perId).FirstOrDefault();
                                    var getFather = con.person.Where(p => p.id == chk.id).FirstOrDefault();
                                    ViewBag.RollNo = getStdName.stdRollNo;
                                    ViewBag.Student = getStdName.pr.perName;
                                    ViewBag.Father = getFather.perName;
                                    ViewBag.Id = chk.RelationId;
                                    TempData["Relation"] = chk;
                                }
                                ViewBag.PerId = GetStd.pr.perId;
                                ViewBag.Name = GetStd.pr.perName;
                                return View();
                            }

                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll Number to Make Relationship";
                            return View();
                        }
                    }
                    else
                    {
                        if (StdName != null && StdName != "")
                        {
                            if (LoginId != null && LoginId != "")
                            {
                                int id = Convert.ToInt32(LoginId);
                                int perId = Convert.ToInt32(PerID);
                                var getId = con.person.Where(p => p.perId == id).FirstOrDefault();
                                if (getId != null)
                                {
                                    var chk = con.rel.Where(r => r.perId == perId).Any();
                                    if (chk == true)
                                    {
                                        TempData["Info"] = "This person's relationship is already made";
                                        return View();
                                    }
                                    else
                                    {
                                        var check = con.rel.Where(r => r.id == getId.id && r.perId == perId).Any();
                                        if (check == true)
                                        {
                                            TempData["Info"] = "This person's relationship is already made";
                                            return View();
                                        }
                                        else
                                        {
                                            Relation rel = new Relation();
                                            rel.id = getId.id;
                                            rel.perId = perId;
                                            con.rel.Add(rel);
                                            con.SaveChanges();
                                            TempData["Success"] = "Operation completed successfully";
                                            return View();
                                        }
                                    }



                                }
                                else
                                {
                                    TempData["Error"] = "Relation cannot be made because person Id is missing";
                                }

                            }
                            else
                            {
                                TempData["Error"] = "Please Select parent to Make Relationship";
                            }
                        }
                        else
                        {
                            TempData["Error"] = "Please Enter Roll Number to Make Relationship";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }


        public ActionResult EditRelation(int id)
        {
            try
            {
                var getData = con.rel.Where(r => r.RelationId == id).FirstOrDefault();
                if (getData != null)
                {
                    PopulatParent();
                    var getStdName = con.std.Where(s => s.perId == getData.perId).FirstOrDefault();
                    var getFather = con.person.Where(p => p.id == getData.id).FirstOrDefault();
                    ViewBag.RollNo = getStdName.stdRollNo;
                    ViewBag.Student = getStdName.pr.perName;
                    ViewBag.Father = getFather.perName;
                    ViewBag.Id = getData.RelationId;
                    return View(getData);
                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AddRelation");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return RedirectToAction("AddRelation");
            }
        }

        [HttpPost]
        public ActionResult EditRelation(int id, Relation relation)
        {
            try
            {
                PopulatParent();
                var getData = con.rel.Where(r => r.RelationId == id).FirstOrDefault();
                if (getData != null)
                {
                    var getId = con.person.Where(p => p.perId == relation.perId).FirstOrDefault();
                    getData.id = getId.id;
                    con.SaveChanges();
                    TempData["Success"] = "Relation Updated successfully";
                    return RedirectToAction("AddRelation");
                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AddRelation");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return RedirectToAction("AddRelation");
            }
        }

        public ActionResult DelRelation(int id)
        {
            try
            {
                var getData = con.rel.Where(r => r.RelationId == id).FirstOrDefault();
                if (getData != null)
                {
                    con.rel.Remove(getData);
                    con.SaveChanges();
                    TempData["Success"] = "Relation Deleted successfully";
                    return RedirectToAction("AddRelation");
                }
                else
                {
                    TempData["Error"] = "No Record found";
                    return RedirectToAction("AddRelation");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "There is some error please contact to soft support";
                return RedirectToAction("AddRelation");
            }
        }

        //Populate Papa//

        public void PopulatParent()
        {
            //Populating the dropdown for Class
            List<Person> ParentList = new List<Person>();
            var getParent = con.person.Where(r => r.roleId == 4 && r.IsDeleted == false).ToList();
            foreach (var i in getParent)
            {
                Person p = new Person();
                p.perId = i.perId;
                p.perName = i.perName + "-" + i.perCNIC;
                ParentList.Add(p);
            }
            SelectList sl = new SelectList(ParentList, "perId", "perName");
            ViewData["Parent"] = sl;
        }


    }
}