using System;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace QuanLiKhachSan.DAO
{
    public class PrivilegeDAO
    {
        public DataTable GetPrivilegeList()
        {
            string query = "SELECT GRANTEE, PRIVILEGE FROM DBA_TAB_PRIVS";
            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }
        public DataTable GetPrivilegeNames()
        {
            string query = "SELECT DISTINCT PRIVILEGE AS PrivilegeName FROM DBA_SYS_PRIVS";
            DataTable dt = new DataTable();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new OracleDataAdapter(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public void GrantPrivilege(string grantee, string privilege, string objectName = null)
        {
            string query = objectName == null ?
                $"GRANT {privilege} TO {grantee}" :
                $"GRANT {privilege} ON {objectName} TO {grantee}";

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Privilege granted successfully.");
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

        public void RevokePrivilege(string grantee, string privilege, string objectName = null)
        {
            string query = objectName == null ?
                $"REVOKE {privilege} FROM {grantee}" :
                $"REVOKE {privilege} ON {objectName} FROM {grantee}";

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Privilege revoked successfully.");
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