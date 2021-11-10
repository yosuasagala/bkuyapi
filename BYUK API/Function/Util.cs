using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BYUK_API.Function
{
    public class Util
    {
        public string FillLeft(string source, int length, string character)
        {
            string a = "";
            if (source.Length < length)
            {
                for (int i = 1; i <= (length - source.Length); i++)
                {
                    a = a + character;
                }
            }

            return a + source;
        }

        public string FillRight(string source, int length, string character)
        {
            if (source.Length < length)
            {
                int len = length - source.Length;
                for (int i = 0; i < len; i++)
                {
                    source += character;
                }
            }

            return source;
        }

        private static Object StreamLocker = new Object();

       

        public void Logging(string filename, string data)
        {
            lock (StreamLocker)
            {
                string dir = @"C:\LOGGING\BYUK\" + DateTime.Now.ToString("yyMMdd") + "\\";
                string fullpath = dir + filename + ".txt";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                StreamWriter logwriter = new StreamWriter(new FileStream(fullpath, FileMode.Append, FileAccess.Write));
                logwriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                string[] results = data.Split('|');
                for (int i = 0; i < results.Length; i++)
                {
                    logwriter.WriteLine(results[i]);
                }
                logwriter.WriteLine("");
                logwriter.Close();
            }
        }

        public string SQLCommandToString(MySqlCommand SQL)
        {
            string sql = SQL.CommandText + "\n ";
            for (int i = 0; i < SQL.Parameters.Count; i++)
            {
                sql += SQL.Parameters[i].ParameterName + " = " + SQL.Parameters[i].Value + ", ";
            }
            return sql;
        }

        public string HashPassword(string key, string password)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider m = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] h = m.ComputeHash(System.Text.ASCIIEncoding.Default.GetBytes(key + password));
            return System.Text.RegularExpressions.Regex.Replace(BitConverter.ToString(h), "-", "").ToLower();
        }
    }
}