using QuanLiKhachSan.Class;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    class NhanVienDAO
    {
        public NhanVienDAO()
        {
        }

        public DataTable LayDanhSach()
        {
            string sql = "select* from View_Front_Desk_Employee";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sql,conn);
                adapter.Fill(dt);
            }   
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return dt;
        }
        public DataTable LayDanhSachTenNhanVien()
        {
            string sql = "select* from View_Employee_Name";
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
        public void Insert(string employeeName, string gender,DateTime? birthday, 
            string identifyCard,string address, string email)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_insertEmployee";
                cmd.Parameters.Add("@employee_name", SqlDbType.NChar).Value = employeeName;
                cmd.Parameters.Add("@gender", SqlDbType.NChar).Value = gender;
                cmd.Parameters.Add("@birthday", SqlDbType.Date).Value = birthday;
                cmd.Parameters.Add("@identify_card", SqlDbType.VarChar).Value = identifyCard;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = address;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
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
        public void Update(int employeeId, string employeeName, string gender, DateTime? birthday,
            string identifyCard, string address, string email)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_updateEmployee";
                cmd.Parameters.Add("@employee_id", SqlDbType.Int).Value = employeeId;
                cmd.Parameters.Add("@employee_name", SqlDbType.NChar).Value = employeeName;
                cmd.Parameters.Add("@gender", SqlDbType.NChar).Value = gender;
                cmd.Parameters.Add("@birthday", SqlDbType.Date).Value = birthday;
                cmd.Parameters.Add("@identify_card", SqlDbType.VarChar).Value = identifyCard;
                cmd.Parameters.Add("@address", SqlDbType.NVarChar).Value = address;
                cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = email;
                conn.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Sửa thành công");
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
        public void Delete(int employeeId)
        {
            SqlConnection conn = DBConnection.conn;
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "proc_deleteEmployee";
                cmd.Parameters.Add("@employee_id", SqlDbType.Int).Value = employeeId;
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
        public DataTable TimKiemTheoHoTen(string employeeName)
        {
            string sql = "SELECT * FROM func_searchByEmployeeName(@employee_name)";
            DataTable dt = new DataTable();
            SqlConnection conn = DBConnection.conn;
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


        /// <summary>
        /// @return
        /// </summary>
        public DataTable LayDanhSachNhanVien()
        {
            // TODO implement here
            return null;
        }

        /// <summary>
        /// @param nhanVien
        /// </summary>
        public void TaoNhanVien(NhanVien nhanVien)
        {
            // TODO implement here
        }
    }
}
