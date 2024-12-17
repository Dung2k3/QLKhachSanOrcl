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

namespace QuanLiKhachSan.DAO
{
    class ProfileDAO
    {
        public DataTable LayDanhSach()
        {
            string sql = "SELECT p1.PROFILE, " +
                             "p1.LIMIT AS SESSIONS_PER_USER, " +
                             "p2.LIMIT AS CONNECT_TIME, " +
                             "p3.LIMIT AS IDLE_TIME " +
                             "FROM DBA_PROFILES p1 " +
                             "LEFT JOIN DBA_PROFILES p2 ON p1.PROFILE = p2.PROFILE AND p2.RESOURCE_NAME = 'CONNECT_TIME' " +
                             "LEFT JOIN DBA_PROFILES p3 ON p1.PROFILE = p3.PROFILE AND p3.RESOURCE_NAME = 'IDLE_TIME' " +
                             "WHERE p1.RESOURCE_NAME = 'SESSIONS_PER_USER' " +
                             "ORDER BY p1.PROFILE";

            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(sql, conn);
                adapter.Fill(dt);
            }
            catch (OracleException ex)
            {
                if (ex.Number == 942 || ex.Number == 1031) // ORA-00942 or ORA-01031
                {
                    MessageBox.Show("Error: You do not have permission to perform this operation. Please check your access rights.", "Access Denied");
                }
                else
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "System Error");
                }
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataTable LayDanhSachUserTheoProfile(string nameProfile)
        {
            string sql = $"SELECT USERNAME, PROFILE FROM DBA_USERS WHERE PROFILE = '{nameProfile}' ORDER BY USERNAME";


            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);

                // Thêm tham số để truyền giá trị nameProfile
                cmd.Parameters.Add(new OracleParameter("nameProfile", nameProfile));

                OracleDataAdapter adapter = new OracleDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (OracleException ex)
            {
                // Bắt lỗi khi không đủ quyền hạn hoặc bảng không tồn tại
                if (ex.Number == 942 || ex.Number == 1031) // ORA-00942 or ORA-01031
                {
                    MessageBox.Show("Error: Can't show user by profile. You do not have permission to perform this operation. Please check your access rights.", "Access Denied");
                }
                else
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "System Error");
                }
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }


        public List<String> ListProfile() 
        {
            string sql = " SELECT distinct PROFILE FROM dba_profiles ";
            List<string> listTablespaces = new List<string>();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listTablespaces.Add(reader[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return listTablespaces;
        }

        public List<String> ListDefaultTableSpace()
        {
            string sql = "SELECT tablespace_name " +
                " FROM dba_tablespaces " +
                " WHERE CONTENTS = 'PERMANENT' ";
            List<string> listTablespaces = new List<string>();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                using (OracleDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listTablespaces.Add(reader["tablespace_name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally { conn.Close(); }
            return listTablespaces;
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
        public void CreateNewProfile(string profileName, string sessionsPerUser, string connectTime, string idleTime)
        {
            // Tạo câu lệnh CREATE PROFILE
            string sql = $"CREATE PROFILE {profileName} LIMIT " +
                         $"SESSIONS_PER_USER {sessionsPerUser} " +
                         $"CONNECT_TIME {connectTime} " +
                         $"IDLE_TIME {idleTime}";

            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();  // Thực thi câu lệnh CREATE PROFILE
                MessageBox.Show($"Profile {profileName} create successful.");
            }
            catch (OracleException ex)
            {
                // Bắt lỗi khi không đủ quyền hạn hoặc bảng không tồn tại
                if (ex.Number == 942 || ex.Number == 1031) // ORA-00942 or ORA-01031
                {
                    MessageBox.Show("Error: Can't create profile. You do not have permission to perform this operation. Please check your access rights.", "Access Denied");
                }
                else
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "System Error");
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public void AlterProfile(string profileName, string sessionsPerUser, string connectTime, string idleTime)
        {
            // Tạo câu lệnh ALTER PROFILE
            string sql = $"ALTER PROFILE {profileName} LIMIT " +
                         $"SESSIONS_PER_USER {sessionsPerUser} " +
                         $"CONNECT_TIME {connectTime} " +
                         $"IDLE_TIME {idleTime}";

            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();  // Thực thi câu lệnh ALTER PROFILE
                MessageBox.Show($"Profile {profileName} update successful.");
            }
            catch (OracleException ex)
            {
                // Bắt lỗi khi không đủ quyền hạn hoặc bảng không tồn tại
                if (ex.Number == 942 || ex.Number == 1031) // ORA-00942 or ORA-01031
                {
                    MessageBox.Show("Error: Can't update profile. You do not have permission to perform this operation. Please check your access rights.", "Access Denied");
                }
                else
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "System Error");
                }
            }
            finally
            {
                conn.Close();
            }
        }
        public void DropProfile(string profileName)
        {
            // Tạo câu lệnh DROP PROFILE
            string sql = $"DROP PROFILE {profileName}";

            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(sql, conn);
                cmd.ExecuteNonQuery();  // Thực thi câu lệnh DROP PROFILE
                MessageBox.Show($"Profile {profileName} delete successful.");
            }
            catch (OracleException ex)
            {
                // Bắt lỗi khi không đủ quyền hạn hoặc bảng không tồn tại
                if (ex.Number == 942 || ex.Number == 1031) // ORA-00942 or ORA-01031
                {
                    MessageBox.Show("Error: Can't drop profile. You do not have permission to perform this operation. Please check your access rights.", "Access Denied");
                }
                else
                {
                    MessageBox.Show("Unexpected error: " + ex.Message, "System Error");
                }
            }
            finally
            {
                conn.Close();
            }
        }



    }

}
