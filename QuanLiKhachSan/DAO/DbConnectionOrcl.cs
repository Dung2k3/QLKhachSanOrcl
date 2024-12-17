using QuanLiKhachSan.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using System.Security.Cryptography;

namespace QuanLiKhachSan.DAO
{
    class DbConnectionOrcl
    {
        public static string host = "localhost";
        public static string port = "1521";
        public static string service_name = "ORCLPDB";
        
        public static OracleConnection CreateConnOrcl(string username, string password)
        {
            string connStr = $"User Id={username};Password={password};Data Source=" +
                       $"(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={host})(PORT={port})))" +
                       $"(CONNECT_DATA=(SERVICE_NAME={service_name})))";
    
            return new OracleConnection(connStr);
        }

        public static OracleConnection conn;
        //public static OracleConnection connAdmin = CreateConnOrcl("HotelCheckLogin", "login");


        public DbConnectionOrcl()
        {
        }

        /// <summary>
        /// @param sql
        /// </summary>
        public void ThucThi(string sql)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally { conn.Close(); }
        }

        /// <summary>
        /// @param sql
        /// </summary>
        public DataTable LayDanhSach(string sql)
        {
            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return dt;
        }

    }
}
