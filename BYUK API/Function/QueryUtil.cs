using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace BYUK_API.Function
{
    public class QueryUtil
    {
        DBUtil db = new DBUtil();
        Util cu = new Util();

        #region auth
        public DataTable getUserAuth(string userid, string password)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `user` WHERE email = @USERID AND password = @PASS");
            SQL.Parameters.AddWithValue("USERID", userid);
            SQL.Parameters.AddWithValue("PASS", cu.HashPassword(userid, password));
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getUserAuth2(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `user` WHERE email = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public string getUserId(string username)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT iduser FROM `user` WHERE username = @USERNAME");
            SQL.Parameters.AddWithValue("USERNAME", username);
            return db.ExecuteSqlScalar(SQL, "api");
        }

        public string getEmployeename(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT nama FROM `user` WHERE iduser = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.ExecuteSqlScalar(SQL, "api");
        }

        public int UpdateLoginStat(string username, string status)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `user` SET `login_stat` = @STAT WHERE email = @USERNAME");
            SQL.Parameters.AddWithValue("USERNAME", username);
            SQL.Parameters.AddWithValue("STAT", status);
            return db.ExecuteSqlCommand(SQL, "api");
        }
        #endregion

        #region sequence
        public DataTable getSeqNumByDesc(string desc)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `sequence` WHERE description = @DESC");
            SQL.Parameters.AddWithValue("DESC", desc);
            return db.SetDataTableSQL(SQL, "api");
        }

        public int UpdateSeqNum(string desc, string seq)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `sequence` SET `value` = @SEQ WHERE description = @DESC");
            SQL.Parameters.AddWithValue("SEQ", seq);
            SQL.Parameters.AddWithValue("DESC", desc);
            return db.ExecuteSqlCommand(SQL, "api");
        }
        #endregion

        #region Employee Data
       

        public int UpdateEmployee(string userid, string name, string project, int role)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `user` SET `nama` = @NAME, `project` = @PROJECT, `idrole` = @ROLE  WHERE iduser = @USERID");
            SQL.Parameters.AddWithValue("NAME", name);
            SQL.Parameters.AddWithValue("PROJECT", project);
            SQL.Parameters.AddWithValue("ROLE", role);
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public int UpdateEmployeepass(string userid, string pass)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `user` SET `password` = @PASS WHERE iduser = @USERID");
            SQL.Parameters.AddWithValue("PASS", pass);
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public int DeleteEmployee(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("DELETE FROM `user` WHERE iduser = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.ExecuteSqlCommand(SQL, "api");
        }


        public DataTable getUserbyId(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT u.nama, u.iduser, u.idrole, u.project FROM `user` u JOIN `project` p ON u.project = p.id WHERE iduser = @ID");
            SQL.Parameters.AddWithValue("ID", userid);
            return db.SetDataTableSQL(SQL, "api");
        }

       
        public string getProjectbyUID(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT project FROM `user` WHERE iduser = @ID");
            SQL.Parameters.AddWithValue("ID", userid);
            return db.ExecuteSqlScalar(SQL, "api");
        }


        public DataTable geAlltUser()
        {
            MySqlCommand SQL = new MySqlCommand("SELECT u.nama, u.iduser, u.idrole, p.nama as project FROM `user` u JOIN `project` p ON u.project = p.id");
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geAlltUserPM(string projectid)
        {
            int role = 3;
            MySqlCommand SQL = new MySqlCommand("SELECT u.nama, u.iduser, u.idrole, p.nama as project FROM `user` u JOIN `project` p ON u.project = p.id WHERE u.project = @ID AND u.idrole <> @ROLE");
            SQL.Parameters.AddWithValue("ID", projectid);
            SQL.Parameters.AddWithValue("ROLE", role);
            return db.SetDataTableSQL(SQL, "api");
        }
        #endregion

        #region Data Cuti
        public int InsertCuti(DateTime startdate, DateTime enddate, int type, string userid, string projectid, string assignedid, string assingnedjob, string nohp)
        {
            string status = "Pending";

            MySqlCommand SQL = new MySqlCommand("INSERT INTO `cuti` (`employeeid`, `project`,`tipe_cuti`,`tgl_pengajuan`,`cutienddate`,`assigned_id`,`assigned_job`,`nohp`,`status`) " +
                "VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4, @PARAM5, @PARAM6, @PARAM7, @PARAM8, @PARAM9)");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = userid;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = projectid;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.Int16).Value = type;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.Date).Value = startdate;
            SQL.Parameters.Add("@PARAM5", MySqlDbType.Date).Value = enddate;
            SQL.Parameters.Add("@PARAM6", MySqlDbType.VarChar).Value = assignedid;
            SQL.Parameters.Add("@PARAM7", MySqlDbType.VarChar).Value = assingnedjob;
            SQL.Parameters.Add("@PARAM8", MySqlDbType.VarChar).Value = nohp;
            SQL.Parameters.Add("@PARAM9", MySqlDbType.VarChar).Value = status;
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public DataTable getCutirbyYear(string year)
        {

            MySqlCommand SQL = new MySqlCommand("SELECT c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE year(c.tgl_pengajuan) = @YEAR");
            SQL.Parameters.AddWithValue("YEAR", year);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutirbyMonth(string month, string year)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE MONTH(c.tgl_pengajuan) = @MONTH AND year(tgl_pengajuan) = @YEAR");
            SQL.Parameters.AddWithValue("MONTH", month);
            SQL.Parameters.AddWithValue("YEAR", year);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutirbyProject(string project)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE c.project = @PROJECT");
            SQL.Parameters.AddWithValue("PROJECT", project);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutirbyUserId(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE c.employeeid = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCountCuti(string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT cuti_tahunan, cuti_tambahan FROM `user` WHERE iduser = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiPending(string id)
        {
            string status = "Pending";
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser JOIN `project` p ON u.project = p.id WHERE c.status = @STATUS AND c.id = @ID");
            SQL.Parameters.AddWithValue("STATUS", status);
            SQL.Parameters.AddWithValue("ID", id);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiPending3(string projectid)
        {
            string status = "Pending";
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, u.nama, c.cutienddate, p.nama as project, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.employeeid = a.iduser JOIN `project` p ON c.project = p.id WHERE c.status = @STATUS AND c.project = @PID");
            SQL.Parameters.AddWithValue("STATUS", status);
            SQL.Parameters.AddWithValue("PID", projectid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiPending2(string id)
        {
            string status = "Pending";
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, c.cutienddate,  u.nama, c.tipe_cuti, c.assigned_id, c.assigned_job, c.nohp,  p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE c.status = @STATUS AND c.id = @ID");
            SQL.Parameters.AddWithValue("STATUS", status);
            SQL.Parameters.AddWithValue("ID", id);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiAll()
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id");
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiAllUserId( string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE u.iduser = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiAllUserIdPM(string pmid, string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT c.id, c.tgl_pengajuan, c.cutienddate, u.nama, p.nama as project, a.nama as approvedby, c.status FROM `cuti` c " +
            "JOIN `user` u ON c.employeeid = u.iduser LEFT JOIN `user` a ON c.approvedby = a.iduser JOIN `project` p ON u.project = p.id WHERE u.iduser = @USERID");
            SQL.Parameters.AddWithValue("USERID", userid);
            SQL.Parameters.AddWithValue("PMID", pmid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiSummary()
        {
            MySqlCommand SQL = new MySqlCommand("SELECT MONTH(c.tgl_pengajuan) as Bulan, YEAR(c.tgl_pengajuan) as Tahun, p.id, p.nama, COUNT(c.project) AS Jumlah FROM `cuti` c JOIN `project` p ON c.project = p.id GROUP  by p.nama, p.id, c.tgl_pengajuan");
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getCutiSisaSummary(string projectid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT u.nama, u.cuti_tahunan, u.cuti_tambahan FROM `user` u WHERE u.project = @PROJECTID");
            SQL.Parameters.AddWithValue("PROJECTID", projectid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public int ApprovalCuti(string id, string status)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `cuti` SET `status` = @STATUS WHERE id = @ID");
            SQL.Parameters.AddWithValue("ID", id);
            SQL.Parameters.AddWithValue("STATUS", status);
            return db.ExecuteSqlCommand(SQL, "api");
        }
        #endregion

        #region Data Lembur
        public DataTable geAllPendingLembur()
        {
            string status = "Pending";
            MySqlCommand SQL = new MySqlCommand("SELECT o.id, u.nama, o.employeeid , p.nama as project, o.tanggal, o.status FROM `overtime` o " +
                "JOIN `user` u ON o.employeeid = u.iduser JOIN `project` p ON u.project = p.id WHERE status = @STATUS");
            SQL.Parameters.AddWithValue("STATUS", status);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geAllPendingLemburbyUserid(string pmid, string userid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT o.id, u.nama, o.employeeid , p.nama as project, o.tanggal, o.status FROM `overtime` o " +
                "JOIN `user` u ON o.employeeid = u.iduser JOIN `project` p ON u.project = p.id WHERE o.employeeid = @USERID AND o.pmid = @PMID");
            SQL.Parameters.AddWithValue("USERID", userid);
            SQL.Parameters.AddWithValue("PMID", pmid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geAllPendingLemburbyTeam(string pmid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT o.id, u.nama, o.employeeid , p.nama as project, o.tanggal, o.status FROM `overtime` o " +
                "JOIN `user` u ON o.employeeid = u.iduser JOIN `project` p ON u.project = p.id WHERE o.pmid = @PMID");
            SQL.Parameters.AddWithValue("PMID", pmid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public int ApprovalLembur(string id, string status)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `overtime` SET `status` = @STATUS WHERE id = @ID");
            SQL.Parameters.AddWithValue("ID", id);
            SQL.Parameters.AddWithValue("STATUS", status);
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public int InsertLembur(string userid, DateTime tgllembur, string pmid)
        {
            string status = "Pending";

            MySqlCommand SQL = new MySqlCommand("INSERT INTO `overtime` (`tanggal`, `status`,`employeeid`,`pmid`) " +
                "VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4)");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.Date).Value = tgllembur;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = status;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.VarChar).Value = userid;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.VarChar).Value = pmid;
            return db.ExecuteSqlCommand(SQL, "api");
        }
        #endregion

        #region util
        public DataTable getAllMenu(int role)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT nama, path, icon FROM `menu` WHERE role = @ROLE");
            SQL.Parameters.AddWithValue("ROLE", role);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable getAllProject()
        {
            MySqlCommand SQL = new MySqlCommand("SELECT id, nama FROM `project` order by nama Desc");
            return db.SetDataTableSQL(SQL, "api");
        }
        #endregion


        #region byuk user
        public DataTable getUserbyName(string username)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `user` WHERE email = @USERNAME");
            SQL.Parameters.AddWithValue("USERNAME", username);
            return db.SetDataTableSQL(SQL, "api");
        }

        public int insertNewUser(string email, string password, string nohp)
        {
            string role = "STUDENT";
            MySqlCommand SQL = new MySqlCommand("INSERT INTO `user` (`email`,`password`,`nohp`,`registered_date`,`role`) " +
                "VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4, @PARAM5)");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = email;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = password;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.VarChar).Value = nohp;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.Date).Value = Convert.ToDateTime(DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss.fff"));
            SQL.Parameters.Add("@PARAM5", MySqlDbType.VarChar).Value = role;
            return db.ExecuteSqlCommand(SQL, "api");
        }
        #endregion

        #region byuk course
        public DataTable getCoursebyTitle(string title)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `course` WHERE title = @TITLE");
            SQL.Parameters.AddWithValue("TITLE", title);
            return db.SetDataTableSQL(SQL, "api");
        }


        public int insertNewCourse(string title, string subject, string desc, string url, string file)
        {
            string status = "active";
            MySqlCommand SQL = new MySqlCommand("INSERT INTO `course` (`title`,`subject`,`description`,`videourl`,`file`, `status`) " +
                "VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4, @PARAM5, @PARAM6)");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = title;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = subject;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.VarChar).Value = desc;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.VarChar).Value = url;
            SQL.Parameters.Add("@PARAM5", MySqlDbType.VarChar).Value = file;
            SQL.Parameters.Add("@PARAM6", MySqlDbType.VarChar).Value = status;
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public int updateCourse(string id, string title, string subject, string desc, string url, string file, string status)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `course` SET `title` = @PARAM1,`subject` = @PARAM2,`description` = @PARAM3,`videourl` = @PARAM4,`file` = @PARAM5, `status` = @PARAM6 WHERE id = @ID");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = title;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = subject;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.VarChar).Value = desc;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.VarChar).Value = url;
            SQL.Parameters.Add("@PARAM5", MySqlDbType.VarChar).Value = file;
            SQL.Parameters.Add("@PARAM6", MySqlDbType.VarChar).Value = status;
            SQL.Parameters.Add("@ID", MySqlDbType.VarChar).Value = id;
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public DataTable geAllCourseData()
        {
            string status = "active";
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `course` WHERE status = @STATUS ORDER BY id ASC");
            SQL.Parameters.Add("@STATUS", MySqlDbType.VarChar).Value = status;
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geAllCourseDatawithParam(string title)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `course` WHERE title like @PARAM1 ORDER BY id ASC");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = "%" + title + "%";

            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geDetailCourseData(string id)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `course` WHERE id = @PARAM1");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = id;
            return db.SetDataTableSQL(SQL, "api");
        }

        #endregion

        #region byyuk enrollment

        public DataTable getEnrollCourse(string id, string usrid)
        {
            MySqlCommand SQL = new MySqlCommand("SELECT * FROM `enrollment` WHERE usrid = @USERID AND id_course = @ID");
            SQL.Parameters.AddWithValue("ID", id);
            SQL.Parameters.AddWithValue("USERID", usrid);
            return db.SetDataTableSQL(SQL, "api");
        }

        public int insertEnrollCourse(string usrid, string id, string flag)
        {
            MySqlCommand SQL = new MySqlCommand("INSERT INTO `enrollment` (`usrid`,`id_course`,`enroll_date`,`enrollment_status`) " +
                "VALUES(@PARAM1, @PARAM2, @PARAM3, @PARAM4)");
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = usrid;
            SQL.Parameters.Add("@PARAM2", MySqlDbType.VarChar).Value = id;
            SQL.Parameters.Add("@PARAM3", MySqlDbType.Date).Value = DateTime.Now;
            SQL.Parameters.Add("@PARAM4", MySqlDbType.VarChar).Value = flag;
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public int updateEnrollCourse(string usrid, string id, string flag)
        {
            MySqlCommand SQL = new MySqlCommand("UPDATE `enrollment` SET `enrollment_status` = @FLAG WHERE usrid = @USERID AND id_course = @ID");
            SQL.Parameters.AddWithValue("USERID", usrid);
            SQL.Parameters.AddWithValue("ID",id);
            SQL.Parameters.AddWithValue("FLAG", flag);
            return db.ExecuteSqlCommand(SQL, "api");
        }

        public DataTable geAllCourseDataEnroll(string userid)
        {
            string status = "enroll";
            MySqlCommand SQL = new MySqlCommand("SELECT c.* FROM `enrollment` e JOIN `course` c ON e.id_course = c.id WHERE e.usrid = @USERID AND e.enrollment_status = @STATUS");
            SQL.Parameters.AddWithValue("USERID", userid);
            SQL.Parameters.AddWithValue("STATUS", status);
            return db.SetDataTableSQL(SQL, "api");
        }

        public DataTable geAllCourseDataEnrollsearch(string userid, string title)
        {
            string status = "enroll";
            MySqlCommand SQL = new MySqlCommand("SELECT c.* FROM `enrollment` e JOIN `course` c ON e.id_course = c.id WHERE e.usrid = @USERID AND e.enrollment_status = @STATUS AND c.title like @PARAM1");
            SQL.Parameters.AddWithValue("USERID", userid);
            SQL.Parameters.Add("@PARAM1", MySqlDbType.VarChar).Value = "%" + title + "%";
            SQL.Parameters.AddWithValue("STATUS", status);
            return db.SetDataTableSQL(SQL, "api");
        }
        #endregion

    }
}