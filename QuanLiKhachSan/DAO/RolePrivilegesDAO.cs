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

        public DataTable GetPrivileges()
        {
            string query = @"
                SELECT 
                    grantee AS Username, 
                    CASE 
                        WHEN privilege IS NOT NULL THEN 'System Privileges'
                        ELSE 'Object Privileges'
                    END AS PrivilegeType,
                    privilege AS SpecificPrivilege,
                    table_name AS Object
                FROM (
                    SELECT grantee, privilege, NULL AS table_name FROM dba_sys_privs
                    UNION
                    SELECT grantee, privilege, table_name FROM dba_tab_privs
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
        public void GrantSystemPrivilege(string username, string privilege, bool withGrantOption)
        {
            string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
            string query = $"GRANT {privilege} TO {username}{grantOption}";
            ExecuteNonQuery(query);
        }

        public void RevokeSystemPrivilege(string username, string privilege)
        {
            string query = $"REVOKE {privilege} FROM {username}";
            ExecuteNonQuery(query);
        }

        public void GrantObjectPrivilege(string username, string objectName, string privilege, bool withGrantOption)
        {
            string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
            string query = $"GRANT {privilege} ON {objectName} TO {username}{grantOption}";
            ExecuteNonQuery(query);
        }

        public void RevokeObjectPrivilege(string username, string objectName, string privilege)
        {
            string query = $"REVOKE {privilege} ON {objectName} FROM {username}";
            ExecuteNonQuery(query);
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
    }
}
