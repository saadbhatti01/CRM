
using SMS.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SMS.Controllers
{
    [CheckSession]
    public class DbBackupController : Controller
    {
        private SqlConnection conn;
        private SqlCommand command;
        private SqlDataReader reader;
        string sql = "";
        string connectionString = "";
        

        // GET: DbBackup
        public ActionResult Download()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
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

        public ActionResult DownloadBackUp()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    string dbPath = Server.MapPath("~/DownloadBackup/");
                    connectionString = ConfigurationManager.ConnectionStrings["DBCon"].ConnectionString;
                    conn = new SqlConnection(connectionString);
                    conn.Open();
                    string downloadfilename = "SMS" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".BAK'";
                    string backupfile = dbPath + "SMS" + DateTime.Now.ToString("ddMMyyyy_HHmmss") + ".BAK'";
                    sql = "BACKUP DATABASE SMS TO DISK =  'E:\\DbBackup\\" + "SMS-" + DateTime.Now.Ticks.ToString() + ".BAK'";
                    command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                    sql = "BACKUP DATABASE SMS TO DISK =  '" + backupfile;

                    //sql = "BACKUP DATABASE SMS TO DISK =  '" + dbPath + "SMS-" + DateTime.Now.Ticks.ToString() + ".BAK'";
                    command = new SqlCommand(sql, conn);
                    command.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();

                    //Download Logic

                    byte[] fileContent = null;
                    string fname = backupfile;
                    System.IO.FileStream fs = new System.IO.FileStream(fname, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                    System.IO.BinaryReader binaryReader = new System.IO.BinaryReader(fs);
                    long byteLength = new System.IO.FileInfo(fname).Length;
                    fileContent = binaryReader.ReadBytes((Int32)byteLength);
                    fs.Close();
                    fs.Dispose();
                    binaryReader.Close();
                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + downloadfilename);
                    Response.BinaryWrite(fileContent);
                    Response.Flush();
                    Response.End();
                    TempData["Success"] = "Database Backup Completed Successfully.";
                    return RedirectToAction("Download");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Backup not downloaded Please contact to soft support";
                return RedirectToAction("Download");
            }

        }
        public ActionResult Upload()
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
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

        public ActionResult UploadBackUp(string path, HttpPostedFileBase file)
        {
            try
            {
                int RoleId = Convert.ToInt32(Session["RoleId"]);
                if (RoleId > 2)
                {
                    TempData["Error"] = "You are not allowed to enter this page";
                    return RedirectToAction("Logout", "Home");
                }
                else
                {
                    if (path == "" || path == null)
                    {
                        TempData["Error"] = "Please Select a Path";
                        return RedirectToAction("Download");
                    }
                    else
                    {
                        string filepath = path;
                        string[] split = filepath.Split('.');
                        string splitPath = split[1];
                        if (splitPath == "BAK")
                        {
                            if (file.ContentLength > 0)
                            {
                                var fileName = Path.GetFileName(file.FileName);
                                var saveFilePath = Path.Combine(Server.MapPath("~/UploadBackup/"), fileName);
                                file.SaveAs(saveFilePath);

                                string backupfile = Server.MapPath("~/UploadedBackup/") + file.FileName;

                                connectionString = ConfigurationManager.ConnectionStrings["DBCon"].ConnectionString;
                                conn = new SqlConnection(connectionString);
                                conn.Open();

                                string UseMaster = "USE master";
                                SqlCommand UseMasterCommand = new SqlCommand(UseMaster, conn);
                                UseMasterCommand.ExecuteNonQuery();
                                sql = "Alter Database SMS set SINGLE_USER WITH ROLLBACK IMMEDIATE";
                                //sql = "Alter Database SMS set MULTI_USER WITH ROLLBACK IMMEDIATE";
                                command = new SqlCommand(sql, conn);
                                command.ExecuteNonQuery();
                                sql = "Restore Database SMS FROM Disk = '" + backupfile + "' WITH REPLACE";
                                command = new SqlCommand(sql, conn);
                                command.ExecuteNonQuery();
                                conn.Close();
                                conn.Dispose();
                            }
                            TempData["Success"] = "Database Backup Completed Successfully.";
                            return RedirectToAction("Upload");
                        }
                        else
                        {
                            TempData["Error"] = "Please Choose the correct file for Data Upload";
                            return RedirectToAction("Upload");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Backup not uploaded Please contact to soft support";
                return RedirectToAction("Upload");
            }

        }
    }
}