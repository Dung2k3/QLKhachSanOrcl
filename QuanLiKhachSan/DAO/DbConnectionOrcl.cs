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

namespace QuanLiKhachSan.DAO
{
    class DbConnectionOrcl
    {
        public static OracleConnection conn;
        public static OracleConnection connAdmin = new OracleConnection(@"Data Source=MONEY;Initial Catalog=HotelManagementSystem3;Integrated Security=True");


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
