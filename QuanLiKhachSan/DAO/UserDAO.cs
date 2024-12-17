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
using System.Windows.Documents;

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
            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable LayDanhSachLikeUsername(string username)
        {
            string sql = "select username,default_tablespace, temporary_tablespace, lock_date , created, account_status, profile, " +
                " CASE WHEN AUTHENTICATION_TYPE = 'PASSWORD' THEN  '****' ELSE ' ' END AS PASSWORD " +
               $"from dba_users WHERE username LIKE '%{username}%' ";
            
            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public User GetUserByUsername(string username)
        {
            string sql = "select username,default_tablespace, temporary_tablespace, lock_date , created, account_status, profile, " +
                " CASE WHEN AUTHENTICATION_TYPE = 'PASSWORD' THEN  '****' ELSE ' ' END AS PASSWORD " +
               $"from dba_users WHERE username = '{username}' ";

            DataTable dt = DbConnectionOrcl.ExecuteTable(sql);
            DataRow dr = dt.Rows[0];
            User user = new User();
            user.Username = username;
            user.DefaultTablespace = Convert.ToString(dr["DEFAULT_TABLESPACE"]);
            user.TemporaryTablespace = Convert.ToString(dr["TEMPORARY_TABLESPACE"]);
            user.LockDate = Convert.ToString(dr["LOCK_DATE"]);
            user.Created = Convert.ToString(dr["CREATED"]);
            user.AccountStatus = Convert.ToString(dr["ACCOUNT_STATUS"]);
            user.Profile = Convert.ToString(dr["PROFILE"]);
            user.Password = Convert.ToString(dr["PASSWORD"]);
            return user;
        }

        public User GetCurrentUser()
        {
            string sql = "select username,default_tablespace, temporary_tablespace, lock_date , created, account_status " +
                "FROM USER_USERS ";

            DataTable dt = DbConnectionOrcl.ExecuteTable(sql);
            DataRow dr = dt.Rows[0];
            User user = new User();
            user.Username = Convert.ToString(dr["USERNAME"]);
            user.DefaultTablespace = Convert.ToString(dr["DEFAULT_TABLESPACE"]);
            user.TemporaryTablespace = Convert.ToString(dr["TEMPORARY_TABLESPACE"]);
            user.LockDate = Convert.ToString(dr["LOCK_DATE"]);
            user.Created = Convert.ToString(dr["CREATED"]);
            user.AccountStatus = Convert.ToString(dr["ACCOUNT_STATUS"]);
            //user.Profile = Convert.ToString(dr["PROFILE"]);
            //user.Password = Convert.ToString(dr["PASSWORD"]);
            return user;
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
            string sql = $"DROP USER \"{username}\" ";
            string mess = $"Delete user {username} complete";
            DbConnectionOrcl.ExecuteNonQuery(sql, mess);
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
                MessageBox.Show($"Altered user {user.Username}");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Invalid username");
                else if (ex.Number == 1918)
                    MessageBox.Show($"Username does not exist");
                else
                    MessageBox.Show($"Error when alter user: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
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
                MessageBox.Show($"Added user {user.Username}");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Invalid username");
                else if (ex.Number == 1920)
                    MessageBox.Show($"Username already exists");
                else
                    MessageBox.Show($"Error when insert user: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error:  {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
