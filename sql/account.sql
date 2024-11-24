use HotelManagementSystem;

ALTER TABLE ACCOUNT
ADD roles NVARCHAR(20);


SELECT * FROM ACCOUNT;
SELECT * FROM EMPLOYEE;

INSERT INTO ACCOUNT VALUES('admin', 'password1', 5, 'admin');


SELECT * FROM View_Front_Desk_Account;

CREATE ROLE Staff;

--4. **Tài khoản**
SELECT * FROM ACCOUNT;
--THÊM ACCOUNT