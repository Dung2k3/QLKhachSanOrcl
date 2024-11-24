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
    class PhoneNumberOfCustomerDao
    {
        public DataTable LayDanhSach()
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            string sql = "select* from View_Customer_Phone";
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

        public void Them(string phone_number, int customer_id)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_add_phone_of_customer";
            cmd.Parameters.Add("@phone_number", SqlDbType.VarChar).Value = phone_number;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Thêm thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void Xoa(string phone_number, int customer_id)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_delete_phone_of_customer";
            cmd.Parameters.Add("@phone_number", SqlDbType.VarChar).Value = phone_number;
            cmd.Parameters.Add("@customer_id", SqlDbType.Int).Value = customer_id;

            try
            {
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Xóa thành công");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public DataTable TimKiem(string searchString)
        {
            string sql = "SELECT * FROM func_search_phone_of_customer(@string)";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@string", searchString);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
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
