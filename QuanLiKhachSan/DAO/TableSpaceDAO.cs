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

namespace QuanLiKhachSan.DAO
{
    class TableSpaceDAO
    {
        public DataTable LayDanhSach()
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas ";
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
        public DataTable LayDanhSachLikeUsername(string username)
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas " +
               $"         WHERE username LIKE '%{username}%'";
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

        public DataTable LayDanhSachByUsername(string username)
        {
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN TO_CHAR(max_bytes / 1024 / 1024) " +
                "              ELSE 'Unlimited' " +
                "          END  AS max_quota_mb " +
                "         FROM dba_ts_quotas " +
               $"         WHERE username = '{username}'";
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

        public List<String> ListTempTableSpace() 
        {
            string sql = "SELECT tablespace_name " +
                " FROM dba_tablespaces " +
                " WHERE CONTENTS = 'TEMPORARY' ";
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

        public void Update(Quota quota)
        {
            string size = quota.MaxQuota >= 0 ? quota.MaxQuota.ToString() + "M" : "UNLIMITED";
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = conn.CreateCommand();
            cmd.CommandText = $"ALTER USER {quota.Username} QUOTA {size} ON {quota.TablespaceName} ";
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show($"Đã cập nhật quota cho user {quota.Username} tren {quota.TablespaceName}");
            }
            catch (OracleException ex)
            {
                if (ex.Number == 1935)
                    MessageBox.Show($"Username không hợp lệ");
                else
                    MessageBox.Show($"Lỗi khi cập nhật quota cho user {quota.Username} tren {quota.TablespaceName}");
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
