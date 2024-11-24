using QuanLiKhachSan.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    class DBConnection
    {
        public static SqlConnection conn;
        public static SqlConnection connAdmin = new SqlConnection(@"Data Source=MONEY;Initial Catalog=HotelManagementSystem3;Integrated Security=True");


        public DBConnection()
        {
        }

        /// <summary>
        /// @param sql
        /// </summary>
        public void ThucThi(string sql)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
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
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql, conn);
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
