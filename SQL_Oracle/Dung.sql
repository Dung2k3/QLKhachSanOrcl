CREATE OR REPLACE VIEW user_profiles AS 
SELECT USERNAME, PROFILE  FROM DBA_USERS;

CREATE OR REPLACE VIEW list_profiles AS 
SELECT distinct PROFILE FROM dba_profiles;

CREATE OR REPLACE VIEW list_roles AS 
SELECT ROLE FROM DBA_ROLES;

CREATE OR REPLACE VIEW dba_privs AS 
(SELECT grantee, privilege, NULL AS table_name,ADMIN_OPTION AS GRANTABLE
FROM dba_sys_privs
UNION
SELECT grantee, privilege, table_name, GRANTABLE
FROM dba_tab_privs);

GRANT SELECT ON user_profiles TO PUBLIC;
GRANT SELECT ON list_profiles TO PUBLIC;
GRANT SELECT ON list_roles TO PUBLIC;
GRANT SELECT ON dba_tablespaces TO PUBLIC;
GRANT SELECT ON DBA_USERS TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_TS_QUOTAS TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_PROFILES TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_ROLES TO adminuser WITH GRANT OPTION;
GRANT SELECT ON DBA_PRIVS TO adminuser WITH GRANT OPTION;