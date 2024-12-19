using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows;

namespace QuanLiKhachSan.DAO
{
    public class RolePrivilegesDAO
    {
        private DbConnectionOrcl dbConnection = new DbConnectionOrcl();
        private bool DoesSystemPrivilegeExist(string username, string privilege)
        {
            string query = @"
            SELECT COUNT(*) 
            FROM dba_sys_privs 
            WHERE grantee = :username 
            AND privilege = :privilege";

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add(":privilege", OracleDbType.Varchar2).Value = privilege;
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking system privilege: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        // Helper method to check if object privilege exists
        private bool DoesObjectPrivilegeExist(string username, string objectName, string privilege, string column = null)
        {
            string query;


            if (!string.IsNullOrEmpty(column))
            {
                query = $"SELECT COUNT(*) FROM dba_col_privs WHERE grantee = '{username}' AND table_name = '{objectName}' AND privilege = '{privilege}' AND column_name = '{column}'";
            }
            else
            {
                query = $"SELECT COUNT(*) FROM dba_tab_privs WHERE grantee = '{username}' AND table_name = '{objectName}' AND privilege = '{privilege}'";
            }

            OracleConnection conn = DbConnectionOrcl.conn;
            try
            {
                conn.Open();
                using (OracleCommand cmd = new OracleCommand(query, conn))
                {
                    cmd.Parameters.Add(":username", OracleDbType.Varchar2).Value = username;
                    cmd.Parameters.Add(":objectName", OracleDbType.Varchar2).Value = objectName;
                    cmd.Parameters.Add(":privilege", OracleDbType.Varchar2).Value = privilege;
                    if (!string.IsNullOrEmpty(column))
                    {
                        cmd.Parameters.Add(":column", OracleDbType.Varchar2).Value = column;
                    }
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error checking object privilege: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
            }
        }

        private bool IsSystemPrivilegeRestricted(string privilege)
        {
            string[] restrictedPrivileges = new[]
            {
            "CREATE PROFILE", "ALTER PROFILE", "DROP PROFILE",
            "CREATE ROLE", "ALTER ANY ROLE", "DROP ANY ROLE", "GRANT ANY ROLE",
            "CREATE SESSION", "SELECT ANY TABLE",
            "CREATE USER", "ALTER USER", "DROP USER"
        };

            return restrictedPrivileges.Contains(privilege);
        }

        public DataTable GetPrivileges()
        {
            string query = @"
            SELECT 
                grantee AS USERNAME, 
                privilege_type AS PrivilegeType,
                privilege AS SpecificPrivilege,
                table_name AS Object,
                column_name AS Column_name,
                grantable
            FROM (
                SELECT 
                    grantee, 
                    'System Privileges' as privilege_type,
                    privilege,
                    NULL as table_name,
                    NULL as column_name,
                    admin_option as grantable
                FROM dba_sys_privs
                UNION ALL
                SELECT 
                    grantee,
                    'Object Privileges' as privilege_type,
                    privilege,
                    table_name,
                    NULL as column_name,
                    grantable
                FROM dba_tab_privs
                UNION ALL
                SELECT 
                    grantee,
                    'Column Privileges' as privilege_type,
                    privilege,
                    table_name,
                    column_name,
                    grantable
                FROM dba_col_privs

            )";

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
        public DataTable GetRoles()
        {
            string query = @"
                SELECT 
                    role AS RoleName, privilege AS privilege,
                    LISTAGG(grantee, ', ') WITHIN GROUP (ORDER BY grantee) AS AssignedUsers
                FROM (
                    SELECT 
                        role, 
                        privilege, 
                        grantee
                    FROM role_sys_privs
                    JOIN dba_role_privs ON role_sys_privs.role = dba_role_privs.granted_role
                )
                GROUP BY role, privilege";

            return ExecuteQuery(query);
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
        // Methods for granting and revoking privileges
        public void GrantSystemPrivilege(string username, string privilege, bool withGrantOption = false)
        {
            if (IsSystemPrivilegeRestricted(privilege))
            {
                if (DoesSystemPrivilegeExist(username, privilege))
                {
                    MessageBox.Show($"The system privilege {privilege} already exists for {username}");
                    return;
                }
            }

            string grantOption = withGrantOption ? " WITH ADMIN OPTION" : "";
            string query = $"GRANT {privilege} TO {username}{grantOption}";
            //MessageBox.Show(query);
            try { ExecuteNonQuery(query); }
            catch (Exception ex) {
                return;

            }
            MessageBox.Show("Privilege operation completed successfully.");

        }

        public void RevokeSystemPrivilege(string username, string privilege)
        {
            if (IsSystemPrivilegeRestricted(privilege))
            {
                if (!DoesSystemPrivilegeExist(username, privilege))
                {
                    MessageBox.Show($"The system privilege {privilege} does not exists for {username}");
                    return;
                }
            }
            string query = $"REVOKE {privilege} FROM {username}";
            try { ExecuteNonQuery(query); }
            catch (Exception ex)
            {
                return;

            }
            MessageBox.Show("Privilege operation completed successfully.");
        }

        public void GrantObjectPrivilege(string username, string objectName, string privilege, bool withGrantOption, string column = null)
        {
            if (privilege.Equals("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                if (DoesObjectPrivilegeExist(username, objectName, privilege, column))
                {
                    MessageBox.Show($"The object privilege {privilege} already exists for {username} on {objectName}" +
                        (column != null ? $".{column}" : ""));
                    return;
                }
            }

            string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
            string columnSpec = !string.IsNullOrEmpty(column) ? $"({column})" : "";
            string query = $"GRANT {privilege}{columnSpec} ON {objectName} TO {username}{grantOption}";
            try { ExecuteNonQuery(query); }
            catch (Exception ex)
            {
                return;

            }
            MessageBox.Show("Privilege operation completed successfully.");
        }

        public void RevokeObjectPrivilege(string username, string objectName, string privilege, string column = null)
        {
            if (privilege.Equals("SELECT", StringComparison.OrdinalIgnoreCase))
            {
                if (!DoesObjectPrivilegeExist(username, objectName, privilege, column))
                {
                    MessageBox.Show($"The object privilege {privilege} does not exist for {username} on {objectName}" +
                        (column != null ? $".{column}" : ""));
                    return;
                }
            }

            string columnSpec = !string.IsNullOrEmpty(column) ? $"({column})" : "";
            string query = $"REVOKE {privilege}{columnSpec} ON {objectName} FROM {username}";
            try { ExecuteNonQuery(query); }
            catch (Exception ex)
            {
                return;

            }
            MessageBox.Show("Privilege operation completed successfully.");
        }

        // Other methods for additional functionality
        public DataTable GetProfiles()
        {
            string query = @"
               SELECT 
                    p.profile AS ProfileName, 
                    p.resource_name AS Resources,
                    LISTAGG(u.username, ', ') WITHIN GROUP (ORDER BY u.username) AS AssignedUsers
                FROM dba_profiles p
                JOIN dba_users u ON u.profile = p.profile
                GROUP BY p.profile, p.resource_name";

            return ExecuteQuery(query);
        }

        public DataTable GetUserInformation()
        {
            string query = @"
                SELECT 
                    username AS Username,
                    account_status AS AccountStatus,
                    created AS CreatedDate,
                    default_tablespace || ', ' || temporary_tablespace AS Tablespaces,
                    profile AS Profile,
                    LISTAGG(granted_role, ', ') WITHIN GROUP (ORDER BY granted_role) AS Roles
                FROM dba_users
                LEFT JOIN dba_role_privs ON dba_users.username = dba_role_privs.grantee
                GROUP BY 
                    username, account_status, created, 
                    default_tablespace, temporary_tablespace, profile";

            return ExecuteQuery(query);
        }

        // Methods to populate ComboBoxes
        public string[] GetSourceUserRoles()
        {
            // Retrieve users and roles for source selection
            string query = @"
                SELECT username FROM dba_users
                UNION
                SELECT role FROM dba_roles";

            DataTable dt = ExecuteQuery(query);
            return dt.AsEnumerable()
                     .Select(row => row[0].ToString())
                     .ToArray();
        }

        public string[] GetTargetUserRoles()
        {
            // Similar to GetSourceUserRoles, but you might want to filter differently
            return GetSourceUserRoles();
        }

        public DataTable DSUserPrivilegesCurrentUser()
        {
            string sql = @"
                SELECT PRIVILEGE, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE
                FROM USER_SYS_PRIVS
                UNION
                SELECT PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE
                FROM USER_TAB_PRIVS ";

            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable DSRolePrivilegesCurrentUser()
        {
            string sql = @"
                SELECT ROLE, PRIVILEGE, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                UNION
                SELECT ROLE, PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)  ";

            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable DSRolePrivilegesCurrentUserByRole(string role)
        {
            string sql = @"
                SELECT ROLE, PRIVILEGE, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                    AND ROLE = '" + role + @"'
                UNION
                SELECT ROLE, PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS) 
                AND ROLE = '" + role + "' ";
            return DbConnectionOrcl.ExecuteTable(sql);
        }
    }
}
