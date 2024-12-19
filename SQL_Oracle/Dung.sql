CREATE OR REPLACE VIEW user_profiles AS 
SELECT USERNAME, PROFILE  FROM DBA_USERS;

CREATE OR REPLACE VIEW list_profiles AS 
SELECT distinct PROFILE FROM dba_profiles;

CREATE OR REPLACE VIEW list_roles AS 
SELECT ROLE FROM DBA_ROLES;

CREATE OR REPLACE VIEW dba_privs AS 
(SELECT 
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
FROM dba_col_privs);

GRANT SELECT ON user_profiles TO PUBLIC;
GRANT SELECT ON list_profiles TO PUBLIC;
GRANT SELECT ON list_roles TO PUBLIC;
GRANT SELECT ON dba_tablespaces TO PUBLIC;
GRANT SELECT ON DBA_USERS TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_TS_QUOTAS TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_PROFILES TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_ROLES TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_PRIVS TO adminuser WITH GRANT OPTION;
