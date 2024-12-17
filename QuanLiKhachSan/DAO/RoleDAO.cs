using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using Oracle.ManagedDataAccess.Client;

namespace QuanLiKhachSan.DAO
{
    public class RoleDAO
    {
        public List<string> GetRoles()
        {
            string query = "SELECT ROLE FROM DBA_ROLES ORDER BY ROLE";
            List<string> roles = new List<string>();
            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand(query, conn);
                OracleDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(reader["ROLE"].ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching role list: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

            return roles; // Trả về List<string> trực tiếp
        }


        public DataTable GetRoleList()
        {
            string query = @"SELECT 
                                r.ROLE AS Role_Name,
                                rp.GRANTEE AS Granted_To_User,
                                tp.PRIVILEGE AS Privilege_Name,
                                tp.TABLE_NAME AS Table_Name,
                                r.AUTHENTICATION_TYPE AS Has_Password
                            FROM 
                                DBA_ROLES r
                            LEFT JOIN 
                                DBA_ROLE_PRIVS rp ON r.ROLE = rp.GRANTED_ROLE
                            LEFT JOIN 
                                DBA_TAB_PRIVS tp ON r.ROLE = tp.GRANTEE
                            ORDER BY 
                                r.ROLE, rp.GRANTEE, tp.PRIVILEGE";
            return ExecuteQuery(query);
        }

        public DataTable GetRoleUserList()
        {
            string query = @"SELECT 
                        rp.GRANTEE AS User_Name,
                        rp.GRANTED_ROLE AS Role_Name,
                        rp.ADMIN_OPTION AS Admin_Option,
                        rp.DEFAULT_ROLE AS Default_Role
                    FROM 
                        DBA_ROLE_PRIVS rp
                    ORDER BY 
                        rp.GRANTEE, rp.GRANTED_ROLE";

            return ExecuteQuery(query);
        }

        public DataTable GetGrantees()
        {
            string query = "SELECT DISTINCT GRANTEE AS GranteeName FROM DBA_ROLE_PRIVS";
            return ExecuteQuery(query);
        }

        public void AddRole(string roleName, string password = null)
        {
            roleName = $"\"{roleName}\""; // Escape role name
            string query = string.IsNullOrEmpty(password)
                ? $"CREATE ROLE {roleName}"
                : $"CREATE ROLE {roleName} IDENTIFIED BY \"{password}\"";

            ExecuteNonQuery(query, "Role added successfully.");
        }

        public void DeleteRole(string roleName)
        {
            string query = $"DROP ROLE \"{roleName}\"";
            ExecuteNonQuery(query, "Role deleted successfully.");
        }

        public void UpdateRole(string roleName, string newPassword = null, bool? isIdentifiedExternally = null, List<string> newPrivileges = null)
        {
            roleName = $"\"{roleName}\"";
            List<string> updates = new();

            if (!string.IsNullOrEmpty(newPassword))
                updates.Add($"IDENTIFIED BY \"{newPassword}\"");
            else if (isIdentifiedExternally.HasValue && isIdentifiedExternally.Value)
                updates.Add("IDENTIFIED EXTERNALLY");

            foreach (string privilege in newPrivileges ?? new List<string>())
                GrantPrivilege(roleName, privilege);

            if (updates.Count > 0)
            {
                string updateRoleSql = $"ALTER ROLE {roleName} {string.Join(" ", updates)}";
                ExecuteNonQuery(updateRoleSql);
            }

            MessageBox.Show("Role updated successfully.");
        }

        public void AssignRoleToUser(string userId, string roleName)
        {
            string query = $"GRANT \"{roleName}\" TO \"{userId}\"";
            ExecuteNonQuery(query, "Role assigned to user successfully.");
        }

        public void RevokeRoleFromUser(string userId, string roleName)
        {
            string query = $"REVOKE \"{roleName}\" FROM \"{userId}\"";
            ExecuteNonQuery(query, "Role revoked from user successfully.");
        }

        public DataTable GetAssignedRoles()
        {
            string query = @"SELECT 
                                rp.GRANTEE AS UserId, 
                                rp.GRANTED_ROLE AS RoleName 
                            FROM 
                                DBA_ROLE_PRIVS rp";
            return ExecuteQuery(query);
        }

        private DataTable ExecuteQuery(string query)
        {
            DataTable dt = new();
            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                OracleDataAdapter adapter = new(query, conn);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        private void ExecuteNonQuery(string sql, string successMessage = null)
        {
            OracleConnection conn = DbConnectionOrcl.conn;
            OracleCommand cmd = new(sql, conn);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                if (!string.IsNullOrEmpty(successMessage))
                    MessageBox.Show(successMessage, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                conn.Close();
            }
        }

        private void GrantPrivilege(string roleName, string privilege)
        {
            string query = $"GRANT \"{privilege}\" TO \"{roleName}\"";
            ExecuteNonQuery(query);
        }

        public DataTable DSRoleCurrentUser()
        {
            string sql = "SELECT *  FROM USER_ROLE_PRIVS ";
            return DbConnectionOrcl.ExecuteTable(sql);
        }
    }
}