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
            string sql = "SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, " +
                "          blocks, CASE " +
                "              WHEN max_bytes > 0 THEN max_bytes / 1024 / 1024 " +
                "              ELSE max_bytes " +
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

        public List<String> ListProfile() 
        {
            string sql = "SELECT DISTINCT PROFILE " +
                        " FROM dba_profiles ";
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
                        listTablespaces.Add(reader["PROFILE"].ToString());
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
