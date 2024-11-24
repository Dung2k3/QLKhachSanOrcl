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
    class PhoneNumberOfEmployeeDao
    {
        public DataTable LayDanhSach()
        {
            string sql = "select* from View_Employee_Phone";
            SqlConnection conn = DBConnection.conn;
            DataTable dt = new DataTable();
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
        public void Insert(string phoneNumber, int employeeId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_add_phone_of_employee";
                cmd.Parameters.Add("@phone_number", SqlDbType.VarChar).Value = phoneNumber;
                cmd.Parameters.Add("@employee_id", SqlDbType.VarChar).Value = employeeId;
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
        public void Delete(string phoneNumber, int employeeId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_delete_phone_of_employee";
                cmd.Parameters.Add("@phone_number", SqlDbType.VarChar).Value = phoneNumber;
                cmd.Parameters.Add("@employee_id", SqlDbType.VarChar).Value = employeeId;
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

        public DataTable SearchByEmployeeName(string employeeName)
        {
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            string sql = "select* from f_search_phone_employee_by_employee_name(@employee_name)";
            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@employee_name", employeeName);
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
