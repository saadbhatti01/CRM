using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;

using SMS.Models;

namespace SMS.Models
{
    public class DBCon : DbContext
    {
        public DbSet<LoginLogs> loginlogs { get; set; }
        public DbSet<Attendance> att { get; set; }
        public DbSet<Campus> camp { get; set; }
        public DbSet<Subject> sub { get; set; }
        public DbSet<Term> term { get; set; }
        public DbSet<Class> cls { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<Area> area { get; set; }
        public DbSet<Person> person { get; set; }
        public DbSet<FeeType> feetype { get; set; }
        public DbSet<ExtraFeeType> Efeetype { get; set; }
        public DbSet<ExamType> examtype { get; set; }
        public DbSet<SubMarkType> submark { get; set; }
        public DbSet<StdObtainMarks> stdObtmark { get; set; }
        public DbSet<StudentExtraFee> StudentExtraFee { get; set; }
        public DbSet<FingerReg> fingerReg { get; set; }
        public DbSet<InstituteInfo> InstInfo { get; set; }
        public DbSet<InstSection> InstSec { get; set; }
        public DbSet<InstSession> InstSes { get; set; }
        public DbSet<Login> login { get; set; }
        public DbSet<Role> role { get; set; }
        public DbSet<RptAttendance> rptAttnd { get; set; }
        public DbSet<Student> std { get; set; }
        public DbSet<StudentFee> stdfee { get; set; }
        public DbSet<TempAttenRpt> TempAttRpt { get; set; }
        public DbSet<ClassFeePkg> clfpkg { get; set; }
        public DbSet<StdFeePkg> stdfpkg { get; set; }
        public DbSet<PassingMarks> passing { get; set; }
        public DbSet<Relation> rel { get; set; }
        public DbSet<AttenParameter> atm { get; set; }
        public DbSet<PersonRegNo> perReg { get; set; }
        public DbSet<AccountHeader> acheader { get; set; }
        public DbSet<AccountEntries> acEntry { get; set; }
        public DbSet<short_course_Registration> scr { get; set; }
        public DbSet<short_course_Subject> scs { get; set; }
        public System.Data.Entity.DbSet<SMS.Models.RegViewModel> RegViewModels { get; set; }
        public DbSet<SMSModule> smsModule { get; set; }
        public DbSet<SMSModuleName> smsModuleName { get; set; }
        public DbSet<SMSAllotment> sMSAllotments { get; set; }
        public DbSet<SentSMS> sentSMs { get; set; }
        public DbSet<Bank> banks { get; set; }
        public DbSet<BankInfo> bankinfos { get; set; }
        public DbSet<Degree> degrees { get; set; }
        public DbSet<StudentInsInfo> stdInfos { get; set; }
        public DbSet<StudentPreEduInfo> stdEduInfos { get; set; }
        public DbSet<TeacherClass> teachers { get; set; }
        public DbSet<Documents> documents { get; set; }
        public DbSet<StudentDataFile> studentDataFiles { get; set; }
        public DbSet<StudentBulkData> studentBulkDatas { get; set; }
    }
}