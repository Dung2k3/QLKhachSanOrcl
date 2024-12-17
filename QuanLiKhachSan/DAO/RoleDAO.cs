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
            List<string> roles = new();
            OracleConnection conn = DbConnectionOrcl.conn;

            try
            {
                conn.Open();
                OracleCommand cmd = new(query, conn);
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

            return roles;
        }

        public DataTable GetRoleList()
        {
            string query = @"SELECT 
                                r.ROLE AS Role_Name,
                                rp.GRANTEE AS Granted_To_User,
                                tp.PRIVILEGE AS Privilege_Name,
                                tp.TABLE_NAME AS Table_Name,
                                r.AUTHENTICATION_TYPE AS Authentication_Type
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
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.");

            roleName = $"\"{roleName}\"";
            string query = string.IsNullOrEmpty(password)
                ? $"CREATE ROLE {roleName}"
                : $"CREATE ROLE {roleName} IDENTIFIED BY \"{password}\"";

            ExecuteNonQuery(query, $"Role {roleName} created successfully.");
        }

        public void DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.");

            string query = $"DROP ROLE \"{roleName}\"";
            ExecuteNonQuery(query, $"Role {roleName} deleted successfully.");
        }

        public void UpdateRole(string roleName, string newPassword = null, bool? isIdentifiedExternally = null, List<string> newPrivileges = null)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.");

            roleName = $"\"{roleName}\""; 
            List<string> updates = new();

            if (string.IsNullOrWhiteSpace(newPassword))
            {
                updates.Add("NOT IDENTIFIED");
            }
            else if (!string.IsNullOrEmpty(newPassword))
            {
                updates.Add($"IDENTIFIED BY \"{newPassword}\"");
            }
            else if (isIdentifiedExternally.HasValue && isIdentifiedExternally.Value)
            {
                updates.Add("IDENTIFIED EXTERNALLY");
            }

            if (newPrivileges != null)
            {
                foreach (string privilege in newPrivileges)
                    GrantPrivilege(roleName, privilege);
            }

            if (updates.Count > 0)
            {
                string updateRoleSql = $"ALTER ROLE {roleName} {string.Join(" ", updates)}";
                ExecuteNonQuery(updateRoleSql, $"Role {roleName} updated successfully.");
            }
        }


        public void AssignRoleToUser(string userName, string roleName, bool allowReGrant)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("User Name and Role Name cannot be null or empty.");

            string adminOption = allowReGrant ? " WITH ADMIN OPTION" : string.Empty;
            string query = $"GRANT \"{roleName}\" TO \"{userName}\"{adminOption}";
            ExecuteNonQuery(query, $"Role {roleName} assigned to user {userName} successfully.");
        }

        public void RevokeRoleFromUser(string userName, string roleName, bool revokeOnlyAdminOption)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("User Name and Role Name cannot be null or empty.");

            if (revokeOnlyAdminOption)
            {
                string revokeQuery = $"REVOKE \"{roleName}\" FROM \"{userName}\"";
                ExecuteNonQuery(revokeQuery, $"Role {roleName} revoked from user {userName}.");

                string reGrantQuery = $"GRANT \"{roleName}\" TO \"{userName}\"";
                ExecuteNonQuery(reGrantQuery, $"Role {roleName} re-granted to user {userName} without admin option.");
            }
            else
            {
                string query = $"REVOKE \"{roleName}\" FROM \"{userName}\"";
                ExecuteNonQuery(query, $"Role {roleName} revoked from user {userName}.");
            }
        }

        private void GrantPrivilege(string roleName, string privilege)
        {
            if (string.IsNullOrWhiteSpace(privilege))
                throw new ArgumentException("Privilege cannot be null or empty.");

            string query = $"GRANT {privilege} TO {roleName}";
            ExecuteNonQuery(query, $"Privilege {privilege} granted to role {roleName}.");
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
                throw new Exception($"Error executing query: {ex.Message}");
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
    }
}
