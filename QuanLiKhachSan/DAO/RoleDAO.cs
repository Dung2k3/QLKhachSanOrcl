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
            string query = "SELECT ROLE FROM sys.list_roles ORDER BY ROLE";
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

        public DataTable GetRoleList(string rolename)
        {
            string query = @"SELECT 
                        r.ROLE AS Role_Name,
                        rp.GRANTEE AS Granted_To_User,
                        tp.PRIVILEGE AS Privilege_Name,
                        tp.TABLE_NAME AS Table_Name
                    FROM 
                        DBA_ROLES r
                    LEFT JOIN 
                        DBA_ROLE_PRIVS rp ON r.ROLE = rp.GRANTED_ROLE
                    LEFT JOIN 
                        DBA_TAB_PRIVS tp ON r.ROLE = tp.GRANTEE
                    WHERE 
                        r.ROLE = '" + rolename.Replace("'", "''") + @"'
                    ORDER BY 
                        r.ROLE, rp.GRANTEE, tp.PRIVILEGE";

            return DbConnectionOrcl.ExecuteTable(query);
        }

        public DataTable GetRoleListBasic()
        {
            string query = @"SELECT 
                                ROLE AS Role_Name, AUTHENTICATION_TYPE AS Authentication_Type
                            FROM 
                                DBA_ROLES";
            return DbConnectionOrcl.ExecuteTable(query);
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
            return DbConnectionOrcl.ExecuteTable(query);
        }

        public DataTable GetRoleToRoleList()
        {
            string query = @"SELECT 
                        rp.GRANTEE AS Target_Role,
                        rp.GRANTED_ROLE AS Source_Role,
                        rp.ADMIN_OPTION AS Admin_Option,
                        rp.DEFAULT_ROLE AS Default_Role
                    FROM 
                        DBA_ROLE_PRIVS rp
                    WHERE 
                        rp.GRANTEE IN (SELECT ROLE FROM DBA_ROLES)
                    ORDER BY 
                        rp.GRANTEE, rp.GRANTED_ROLE";
            return DbConnectionOrcl.ExecuteTable(query);
        }

        public DataTable GetGrantees()
        {
            string query = "SELECT DISTINCT USERNAME AS GranteeName FROM sys.user_profiles";
            return DbConnectionOrcl.ExecuteTable(query);
        }

        public void AddRole(string roleName, string password = null)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.");

            roleName = $"\"{roleName}\"";
            string query = string.IsNullOrEmpty(password)
                ? $"CREATE ROLE {roleName}"
                : $"CREATE ROLE {roleName} IDENTIFIED BY \"{password}\"";

            DbConnectionOrcl.ExecuteNonQuery(query, $"Role {roleName} created successfully.");
        }

        public void DeleteRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("Role name cannot be null or empty.");

            string query = $"DROP ROLE \"{roleName}\"";
            DbConnectionOrcl.ExecuteNonQuery(query, $"Role {roleName} deleted successfully.");
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
                DbConnectionOrcl.ExecuteNonQuery(updateRoleSql, $"Role {roleName} updated successfully.");
            }
        }

        public void AssignRoleToUser(string userName, string roleName, bool allowReGrant)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("User Name and Role Name cannot be null or empty.");

            string adminOption = allowReGrant ? " WITH ADMIN OPTION" : string.Empty;
            string query = $"GRANT \"{roleName}\" TO \"{userName}\"{adminOption}";
            DbConnectionOrcl.ExecuteNonQuery(query, $"Role {roleName} assigned to user {userName} successfully.");
        }
        public void AssignRoleToRole(string targetRole, string sourceRole, bool allowReGrant)
        {
            if (string.IsNullOrWhiteSpace(targetRole) || string.IsNullOrWhiteSpace(sourceRole))
                throw new ArgumentException("Source Role and Target Role cannot be null or empty.");

            string adminOption = allowReGrant ? " WITH ADMIN OPTION" : string.Empty;
            string query = $"GRANT \"{sourceRole}\" TO \"{targetRole}\"{adminOption}";
            DbConnectionOrcl.ExecuteNonQuery(query, $"Role {sourceRole} assigned to role {targetRole} successfully.");
        }

        public void RevokeRoleFromUser(string userName, string roleName, bool revokeOnlyAdminOption)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(roleName))
                throw new ArgumentException("User Name and Role Name cannot be null or empty.");

            if (revokeOnlyAdminOption)
            {
                string revokeQuery = $"REVOKE \"{roleName}\" FROM \"{userName}\"";
                DbConnectionOrcl.ExecuteNonQuery(revokeQuery, $"Role {roleName} revoked from user {userName}.");

                string reGrantQuery = $"GRANT \"{roleName}\" TO \"{userName}\"";
                DbConnectionOrcl.ExecuteNonQuery(reGrantQuery, $"Role {roleName} re-granted to user {userName} without admin option.");
            }
            else
            {
                string query = $"REVOKE \"{roleName}\" FROM \"{userName}\"";
                DbConnectionOrcl.ExecuteNonQuery(query, $"Role {roleName} revoked from user {userName}.");
            }
        }

        public void RevokeRoleFromRole(string targetRole, string sourceRole, bool revokeOnlyAdminOption)
        {
            if (string.IsNullOrWhiteSpace(targetRole) || string.IsNullOrWhiteSpace(sourceRole))
                throw new ArgumentException("Source Role and Target Role cannot be null or empty.");

            if (revokeOnlyAdminOption)
            {
                string revokeQuery = $"REVOKE \"{sourceRole}\" FROM \"{targetRole}\"";
                DbConnectionOrcl.ExecuteNonQuery(revokeQuery, $"Role {sourceRole} revoked from role {targetRole}.");

                string reGrantQuery = $"GRANT \"{sourceRole}\" TO \"{targetRole}\"";
                DbConnectionOrcl.ExecuteNonQuery(reGrantQuery, $"Role {sourceRole} re-granted to role {targetRole} without admin option.");
            }
            else
            {
                string query = $"REVOKE \"{sourceRole}\" FROM \"{targetRole}\"";
                DbConnectionOrcl.ExecuteNonQuery(query, $"Role {sourceRole} revoked from role {targetRole}.");
            }
        }

        private void GrantPrivilege(string roleName, string privilege)
        {
            if (string.IsNullOrWhiteSpace(privilege))
                throw new ArgumentException("Privilege cannot be null or empty.");

            string query = $"GRANT {privilege} TO {roleName}";
            DbConnectionOrcl.ExecuteNonQuery(query, $"Privilege {privilege} granted to role {roleName}.");
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
        public DataTable DSRoleCurrentUser()
        {
            string sql = "SELECT *  FROM USER_ROLE_PRIVS ";
            return DbConnectionOrcl.ExecuteTable(sql);
        }
    }
}
