using System.Data.OracleClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using QuanLiKhachSan.Class;
using System.Collections;

namespace QuanLiKhachSan.DAO
{
    class UserDAO

    {
        public List<string> GetUsers()
        {
            string query = "SELECT USERNAME FROM DBA_USERS ORDER BY USERNAME";
            List<string> users = new List<string>();
            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(reader["USERNAME"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching user list: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return users; // Trả về List<string> trực tiếp
        }

        public DataTable LayDanhSach()
        {
            string sql = "select username,default_tablespace, temporary_tablespace, lock_date , created, account_status, profile, " +
                " CASE WHEN AUTHENTICATION_TYPE = 'PASSWORD' THEN  '****' ELSE ' ' END AS PASSWORD " +
                "from dba_users ";
            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(sql,conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return dt;
        }

        public bool Login(string username, string password)
        {
            bool isSuccess = false;

            OracleConnection conn = DbConnectionOrcl.CreateConnOrcl(username,password);
            try
            {
                conn.Open();
                isSuccess = true;
                DbConnectionOrcl.conn = DbConnectionOrcl.CreateConnOrcl(username, password);
            }

            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return isSuccess;
        }

        public void Delete(string username)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = conn.CreateCommand();
            //cmd.CommandText = "DROP USER " + username ;
            cmd.CommandText = $"DROP USER \"{username}\" ";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Xóa thành công user {username}");
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

        public void Update(User user)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText =
                $"ALTER USER {user.Username} " +
                $"DEFAULT TABLESPACE {user.DefaultTablespace} " +
                $"TEMPORARY TABLESPACE {user.TemporaryTablespace} " +                
                $"PROFILE {user.Profile} " +
                "ACCOUNT " + (user.AccountStatus.Equals("OPEN")? "UNLOCK" : "LOCK") +
                (!user.Password.Equals("") ? $" IDENTIFIED BY {user.Password} " : "");
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Đã cập nhật thông tin cho user {user.Username}");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Username không hợp lệ");
                else
                    MessageBox.Show($"Lỗi khi thay đổi thông tin user: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
        public void Insert(User user)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = 
                $"CREATE USER {user.Username} " +
                $"DEFAULT TABLESPACE {user.DefaultTablespace} " +
                $"TEMPORARY TABLESPACE {user.TemporaryTablespace} " +
                $"PROFILE {user.Profile} " +
                "ACCOUNT " + (user.AccountStatus.Equals("OPEN") ? "UNLOCK" : "LOCK") +
                (!user.Password.Equals("") ? $" IDENTIFIED BY {user.Password} " : "");
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Đã thêm user {user.Username}");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Username không hợp lệ");
                else
                    MessageBox.Show($"Lỗi khi thêm user: {ex.Message} ----- {ex.ErrorCode}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Có lỗi xảy ra: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
