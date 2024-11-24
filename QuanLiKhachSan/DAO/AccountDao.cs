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
    class AccountDao
    {
        public DataTable LayDanhSach()
        {
            string sql = "Select* from View_Front_Desk_Account";
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

            }
            finally { conn.Close(); }
            return dt;
        }

        public bool Login(string username, string password)
        {
            bool isSuccess = false;
            string sql = "SELECT dbo.Login(@username,@password)";
            SqlConnection connAdmin = DBConnection.connAdmin;
            try
            {
                connAdmin.Open();
                SqlCommand cmd = new SqlCommand(sql, connAdmin);
                cmd.Parameters.AddWithValue("username", username);
                cmd.Parameters.AddWithValue("password", password);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                if(dataTable.Rows.Count > 0 && dataTable.Rows[0][0] != DBNull.Value)
                {
                    isSuccess = true;
                    string connStr = dataTable.Rows[0][0].ToString();
                    DBConnection.conn = new SqlConnection(connStr);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { connAdmin.Close(); }
            return isSuccess;
        }

        public void Update(string username, string password)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_updateAccount";
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = password;
            try
            {
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
        public void Insert(string username, string password, int employeeId, string roles)
        {
            SqlConnection conn = DBConnection.conn;
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "proc_insertAccount";
            cmd.Parameters.Add("@username", SqlDbType.NVarChar).Value = username;
            cmd.Parameters.Add("@password", SqlDbType.VarChar).Value =password;
            cmd.Parameters.Add("@employee_id", SqlDbType.Int).Value = employeeId;
            cmd.Parameters.Add("@roles", SqlDbType.VarChar).Value = roles;
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
    }
}
