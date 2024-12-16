GRANT CREATE SESSION TO HotelManagementSystem IDENTIFIED BY admin

ALTER USER HotelManagementSystem QUOTA 10M ON users;

GRANT CREATE  TABLE TO HotelManagementSystem;

GRANT CREATE  SEQUENCE  TO HotelManagementSystem;

SELECT * FROM DBA_ROLES WHERE ROLE like 'Hotel%';

CREATE ROLE HotelAdmin;
CREATE ROLE HotelStaff;

SELECT * FROM DBA_ROLES WHERE ROLE like 'HOTEL%';

GRANT CREATE SESSION TO adminuser IDENTIFIED BY admin;

GRANT dba TO adminuser;

--Xem bang user
select username,default_tablespace, temporary_tablespace, lock_date, created, account_status, profile 
from dba_users;
-- Xem bang profile
SELECT * FROM dba_profiles ;
-- Xem table_space va quota
SELECT tablespace_name, username, bytes / 1024 / 1024 AS quota_mb, 
       blocks, CASE 
           WHEN max_bytes > 0 THEN  TO_CHAR(max_bytes / 1024 / 1024)
           ELSE 'Unlimited'  -- max_bytes
       END  AS max_quota_mb
FROM dba_ts_quotas;
-- Xem user_role
SELECT grantee AS username, granted_role, admin_option, default_role
FROM dba_role_privs
ORDER BY grantee;
-- Xem quyen he thong cua user
SELECT grantee AS username, privilege, admin_option
FROM dba_sys_privs;
-- Xem quyen doi tuong cua user
SELECT grantee AS username, owner, table_name, privilege, grantor
FROM dba_tab_privs;
-- Xem Quy?n he thong c?a role
SELECT role, privilege
FROM role_sys_privs;
-- Xem Quy?n trên ??i t??ng c?a m?t role
SELECT role, owner, table_name, privilege
FROM role_tab_privs;
-- quy?n h? th?ng t? roles cua user
SELECT role, privilege
FROM role_sys_privs
WHERE role IN (
    SELECT granted_role 
    FROM dba_role_privs 
    WHERE grantee = 'USERNAME'
);
-- quy?n h? th?ng t? roles cua user
SELECT role, owner, table_name, privilege
FROM role_tab_privs
WHERE role IN (
    SELECT granted_role 
    FROM dba_role_privs 
    WHERE grantee = 'USERNAME'
);

SELECT *
FROM dba_tablespaces
WHERE CONTENTS = 'PERMANENT';

select username,default_tablespace, temporary_tablespace, lock_date , created, account_status, profile from dba_users; 

