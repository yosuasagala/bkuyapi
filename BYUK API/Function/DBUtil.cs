using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;

namespace BYUK_API.Function
{
    public class DBUtil
    {
        Util util = new Util();
        Util cu = new Util();

        public DBUtil()
        {
        }

        public string GetConnectionString(string database)
        {

            string constr = "";
            switch (database)
            {
                case "api": constr = ConfigurationManager.ConnectionStrings["BYUKDB"].ConnectionString; break;
            }
            return constr;
        }

        public DataTable SetDataTableSQL(MySqlCommand SQLCommand, string database)
        {
            DataTable dtTableSQL = null;

            try
            {
                using (MySqlConnection DBConn = new MySqlConnection(GetConnectionString(database)))
                {
                    FillDataTable(DBConn, SQLCommand, ref dtTableSQL);
                }
            }
            catch (Exception ex)
            {
                util.Logging("SetDataTableSQL", ex.Message + "\n" + util.SQLCommandToString(SQLCommand));
            }

            return dtTableSQL;
        }

        public int ExecuteSqlCommand(MySqlCommand SQLCommand, string database)
        {
            int affectedRow = 0;

            try
            {
                using (MySqlConnection DBConn = new MySqlConnection(GetConnectionString(database)))
                {
                    ExecuteNonQuery(DBConn, SQLCommand, ref affectedRow);
                }
            }
            catch (Exception ex)
            {
                util.Logging("ExecuteSqlCommand", ex.Message + "\n" + util.SQLCommandToString(SQLCommand));
            }

            return affectedRow;
        }

        private void ExecuteNonQuery(MySqlConnection DBConn, MySqlCommand SQLCommand, ref int affectedRow)
        {
            if (DBConn != null)
            {
                try
                {
                    DBConn.Open();
                    SQLCommand.Connection = DBConn;
                    affectedRow = SQLCommand.ExecuteNonQuery();
                    DBConn.Close();
                }
                catch (Exception e)
                {
                    string a = e.Message;
                }
            }
        }

        private void FillDataTable(MySqlConnection DBConn, MySqlCommand SQLCommand, ref DataTable dtTableSQL)
        {
            if (DBConn != null)
            {
                DBConn.Open();
                dtTableSQL = new DataTable();
                MySqlDataAdapter DataAdapterSQL = new MySqlDataAdapter();
                DataAdapterSQL.SelectCommand = SQLCommand;
                DataAdapterSQL.SelectCommand.Connection = DBConn;
                DataAdapterSQL.Fill(dtTableSQL);
                DBConn.Close();
            }
        }


        private void ExecuteScalar(MySqlConnection DBConn, MySqlCommand SQLCommand, ref string result)
        {
            if (DBConn != null)
            {
                DBConn.Open();
                SQLCommand.Connection = DBConn;
                object res = SQLCommand.ExecuteScalar();
                if (res != null) result = (string)res;
                DBConn.Close();
            }
        }

        public string ExecuteSqlScalar(MySqlCommand SQLCommand, string database)
        {
            string result = "";

            try
            {
                using (MySqlConnection DBConn = new MySqlConnection(GetConnectionString(database)))
                {
                    ExecuteScalar(DBConn, SQLCommand, ref result);
                }
            }
            catch (Exception ex)
            {
                util.Logging("ExecuteScalar", ex.Message + "\n" + util.SQLCommandToString(SQLCommand));
            }

            return result;
        }
    }
}