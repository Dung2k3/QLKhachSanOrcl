using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;
using System.Windows;
using QuanLiKhachSan.Class;

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
                        WHEN table_name IS NULL THEN 'System Privileges'
                        ELSE 'Object Privileges'
                    END AS PrivilegeType,
                    privilege AS SpecificPrivilege,
                    table_name AS Object, GRANTABLE
                FROM sys.dba_privs";

            return DbConnectionOrcl.ExecuteTable(query);
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

            return DbConnectionOrcl.ExecuteTable(query);
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

            string grantOption = withGrantOption ? " WITH ADMIN OPTION" : "";
            string query = $"GRANT {privilege} TO {username}{grantOption}";
            //MessageBox.Show(query);

            DbConnectionOrcl.ExecuteNonQuery(query);
        }

        public void RevokeSystemPrivilege(string username, string privilege)
        {
            string query = $"REVOKE {privilege} FROM {username}";
            DbConnectionOrcl.ExecuteNonQuery(query);
        }

        public void GrantObjectPrivilege(string username, string objectName, string privilege, bool withGrantOption)
        {
            string grantOption = withGrantOption ? " WITH GRANT OPTION" : "";
            string query = $"GRANT {privilege} ON {objectName} TO {username}{grantOption}";
            DbConnectionOrcl.ExecuteNonQuery(query);
        }

        public void RevokeObjectPrivilege(string username, string objectName, string privilege)
        {
            string query = $"REVOKE {privilege} ON {objectName} FROM {username}";
            DbConnectionOrcl.ExecuteNonQuery(query);
        }

        /* Other methods for additional functionality
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

            return DbConnectionOrcl.ExecuteTable(query);
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

            return DbConnectionOrcl.ExecuteTable(query);
        }
        */
        // Methods to populate ComboBoxes
        public string[] GetSourceUserRoles()
        {
            // Retrieve users and roles for source selection
            string query = @"
                SELECT username FROM sys.user_profiles
                UNION
                SELECT role FROM sys.list_roles";

            DataTable dt = DbConnectionOrcl.ExecuteTable(query);
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

        public DataTable DSUserAndRolePrivilegesCurrentUser()
        {
            string sql = @"
                SELECT ROLE AS USERNAME, PRIVILEGE AS SpecificPrivilege, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                UNION
                SELECT ROLE AS USERNAME, PRIVILEGE  AS SpecificPrivilege, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)  
                UNION
                SELECT USERNAME,  PRIVILEGE  AS SpecificPrivilege, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE
                FROM USER_SYS_PRIVS
                UNION
                SELECT GRANTEE AS USERNAME, PRIVILEGE  AS SpecificPrivilege, 'Object Privileges' AS PrivilegeType, 
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

        public void DSCheckPrivilegesCurrentUserByRole()
        {
            string sql = @"
            SELECT                 
                CASE 
                    WHEN user_privileges.privilege IS NOT NULL THEN 'TRUE'
                    ELSE 'FALSE'
                END AS HAS_PRIVILEGE,
                privilege_list.privilege AS PRIVILEGE
            FROM ( 
                SELECT 1 AS order_no, 'CREATE PROFILE' AS privilege, NULL AS object FROM dual UNION ALL
                    SELECT 2, 'ALTER PROFILE', NULL FROM dual UNION ALL
                    SELECT 3, 'DROP PROFILE', NULL FROM dual UNION ALL
                    SELECT 4, 'CREATE ROLE', NULL FROM dual UNION ALL
                    SELECT 5, 'ALTER ANY ROLE', NULL FROM dual UNION ALL
                    SELECT 6, 'DROP ANY ROLE', NULL FROM dual UNION ALL
                    SELECT 7, 'GRANT ANY ROLE', NULL FROM dual UNION ALL
                    SELECT 8, 'CREATE SESSION', NULL FROM dual UNION ALL
                    SELECT 9, 'SELECT ANY TABLE', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 10, 'CREATE USER', NULL FROM dual UNION ALL
                    SELECT 11, 'ALTER USER', NULL FROM dual UNION ALL
                    SELECT 12, 'DROP USER', NULL FROM dual UNION ALL
                    SELECT 13, 'SELECT ON DBA_USERS', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 14, 'SELECT ON DBA_TS_QUOTAS', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 15, 'SELECT ON DBA_PROFILES', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 16, 'SELECT ON DBA_ROLES', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 17, 'SELECT ON DBA_PRIVS', 'TABLE_NAME' FROM dual 
            ) privilege_list
            LEFT JOIN (   
                SELECT PRIVILEGE
                FROM USER_SYS_PRIVS
                UNION
                SELECT PRIVILEGE || ' ON ' || TABLE_NAME AS PRIVILEGE
                FROM USER_TAB_PRIVS 
                UNION
                SELECT PRIVILEGE
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                UNION
                SELECT PRIVILEGE || ' ON ' || TABLE_NAME AS PRIVILEGE
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)  
                ) user_privileges
                ON privilege_list.privilege = user_privileges.privilege
            ORDER BY privilege_list.order_no    ";
            List<String> listHasPrivilege = DbConnectionOrcl.ExecuteListString(sql);
            DbConnectionOrcl.userPrivilege =  new UserPrivilege()
            {
                CreateProfile = listHasPrivilege[0].Equals("TRUE"),
                AlterProfile = listHasPrivilege[1].Equals("TRUE"),
                DropProfile = listHasPrivilege[2].Equals("TRUE"),
                CreateRole = listHasPrivilege[3].Equals("TRUE"),
                AlterAnyRole = listHasPrivilege[4].Equals("TRUE"),
                DropAnyRole = listHasPrivilege[5].Equals("TRUE"),
                GrantAnyRole = listHasPrivilege[6].Equals("TRUE"),
                CreateSession = listHasPrivilege[7].Equals("TRUE"),
                SelectAnyTable = listHasPrivilege[8].Equals("TRUE"),
                CreateUser = listHasPrivilege[9].Equals("TRUE"),
                AlterUser = listHasPrivilege[10].Equals("TRUE"),
                DropUser = listHasPrivilege[11].Equals("TRUE"),
                SelectUser = listHasPrivilege[12].Equals("TRUE"),
                SelectQuota = listHasPrivilege[13].Equals("TRUE"),
                SelectProfile = listHasPrivilege[14].Equals("TRUE"),
                SelectRole = listHasPrivilege[15].Equals("TRUE"),
                SelectPrivs= listHasPrivilege[16].Equals("TRUE")
            };
        }
    }
}
