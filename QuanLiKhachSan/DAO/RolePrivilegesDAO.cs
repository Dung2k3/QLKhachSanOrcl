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
        private bool DoesSystemPrivilegeExist(string username, string privilege)
        {
            string query = @"
            SELECT COUNT(*) 
            FROM sys.DBA_PRIVS 
            WHERE grantee = :username 
            AND privilege = :privilege
            AND PRIVILEGE_TYPE = 'System Privileges'";

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
                query = $"SELECT COUNT(*) FROM sys.DBA_PRIVS WHERE grantee = '{username}' AND table_name = '{objectName}' AND privilege = '{privilege}' AND column_name = '{column}'";
            }
            else
            {
                query = $"SELECT COUNT(*) FROM sys.DBA_PRIVS WHERE grantee = '{username}' AND table_name = '{objectName}' AND privilege = '{privilege}'";
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
            FROM sys.DBA_PRIVS ";

            return DbConnectionOrcl.ExecuteTable(query);
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
            if(DbConnectionOrcl.ExecuteNonQuery(query))
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
            if (DbConnectionOrcl.ExecuteNonQuery(query))
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
            MessageBox.Show(query);
            if (DbConnectionOrcl.ExecuteNonQuery(query))
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
            if (DbConnectionOrcl.ExecuteNonQuery(query))
                MessageBox.Show("Privilege operation completed successfully.");
        }
        // Methods to populate ComboBoxes
        public string[] GetSourceUserRoles()
        {
            // Retrieve users and roles for source selection
            string query = @"
                SELECT username FROM sys.user_profiles                
                UNION
                SELECT role FROM sys.list_roles
                ORDER BY username";

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
                     null AS Object, ADMIN_OPTION AS GRANTABLE, NULL AS COLUMN_NAME
                FROM USER_SYS_PRIVS
                UNION
                SELECT PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, NULL AS COLUMN_NAME
                FROM USER_TAB_PRIVS 
                UNION
                SELECT PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, COLUMN_NAME
                FROM USER_COL_PRIVS ";

            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable DSUserAndRolePrivilegesCurrentUser()
        {
            string sql = @"
                SELECT ROLE AS USERNAME, PRIVILEGE AS SpecificPrivilege, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE, NULL AS COLUMN_NAME
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                UNION
                SELECT ROLE AS USERNAME, PRIVILEGE  AS SpecificPrivilege, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, COLUMN_NAME
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)  
                UNION
                SELECT USERNAME,  PRIVILEGE  AS SpecificPrivilege, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE, NULL AS COLUMN_NAME
                FROM USER_SYS_PRIVS
                UNION
                SELECT GRANTEE AS USERNAME, PRIVILEGE  AS SpecificPrivilege, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, NULL AS COLUMN_NAME
                FROM USER_TAB_PRIVS 
                UNION
                SELECT GRANTEE AS USERNAME, PRIVILEGE  AS SpecificPrivilege, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, COLUMN_NAME
                FROM USER_COL_PRIVS ";

            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable DSRolePrivilegesCurrentUser()
        {
            string sql = @"
                SELECT ROLE, PRIVILEGE, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE, NULL AS COLUMN_NAME
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                UNION
                SELECT ROLE, PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, COLUMN_NAME
                FROM ROLE_TAB_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)  ";

            return DbConnectionOrcl.ExecuteTable(sql);
        }

        public DataTable DSRolePrivilegesCurrentUserByRole(string role)
        {
            string sql = @"
                SELECT ROLE, PRIVILEGE, 'System Privileges' AS PrivilegeType, 
                     null AS Object, ADMIN_OPTION AS GRANTABLE, NULL AS COLUMN_NAME
                FROM ROLE_SYS_PRIVS
                WHERE ROLE IN (SELECT GRANTED_ROLE FROM USER_ROLE_PRIVS)
                    AND ROLE = '" + role + @"'
                UNION
                SELECT ROLE, PRIVILEGE, 'Object Privileges' AS PrivilegeType, 
                     TABLE_NAME AS Object, GRANTABLE, COLUMN_NAME
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
                    SELECT 17, 'SELECT ON DBA_PRIVS', 'TABLE_NAME' FROM dual UNION ALL
                    SELECT 18, 'SELECT ON USER_DETAILS', 'TABLE_NAME' FROM dual 
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
                SelectPrivs= listHasPrivilege[16].Equals("TRUE"),
                SelectUserDetail = listHasPrivilege[17].Equals("TRUE")
            };
        }
    }
}
