--Trong đề tài phải áp dụng hết các loại hàm và thủ tục, nên có đa dạng mỗi loại 2 ba cái
USE HotelManagementSystem;


--1. **Đặt phòng**
CREATE or ALTER PROCEDURE proc_insertBookingRecord
    @expected_checkin_date DATETIME,
    @expected_checkout_date DATETIME,
    @actual_checkin_date DATETIME,
    @actual_checkout_date DATETIME,
    @deposit FLOAT,
    @surcharge FLOAT,
    @note NVARCHAR(255),
    @status NVARCHAR(25),
    @representative_id INT,
    @room_id INT
AS
BEGIN
    BEGIN TRANSACTION;
	BEGIN TRY
        INSERT INTO BOOKING_RECORD(expected_checkin_date, expected_checkout_date, deposit,
		surcharge, note, status, actual_checkin_date, actual_checkout_date, room_id, representative_id)
        VALUES (@expected_checkin_date, @expected_checkout_date, @deposit,
		@surcharge, @note, @status, @actual_checkin_date, @actual_checkout_date, @room_id, @representative_id);
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
	
EXEC proc_insertBookingRecord
    @expected_checkin_date = '2023-11-05',
    @expected_checkout_date = '2023-11-10',
    @actual_checkin_date = '2023-11-06',
    @actual_checkout_date = '2023-11-09',
    @deposit = 1000000,
    @surcharge = 20000,
    @note = N'Additional notes',
    @status = N'Chờ xác nhận',
    @representative_id = 1,
    @room_id = 1;

select * from ROOM
select * from CUSTOMER
select * from BOOKING_RECORD

----Xóa hồ sơ đặt phòng (hủy đặt phòng) 

CREATE or ALTER PROCEDURE proc_deleteBookingRecord
    @booking_record_id INT
AS
BEGIN
	DECLARE @room_id INT, @customer_id INT;
    BEGIN TRANSACTION;
    BEGIN TRY
		--Chuyển trạng thái phòng thành rỗng
		SELECT @room_id = room_id FROM BOOKING_RECORD WHERE booking_record_id = @booking_record_id
		UPDATE ROOM SET room_status = N'Trống' WHERE room_id = @room_id;

		--Xóa hồ sơ đặt phòng
        UPDATE BOOKING_RECORD SET status = N'Đã hủy' WHERE booking_record_id = @booking_record_id;
        COMMIT;

    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
 
 exec proc_deleteBookingRecord @booking_record_id = 1;
 select * from BOOKING_RECORD;

-- --Cập nhật hồ sơ đặt phòng
CREATE or ALTER PROCEDURE proc_updateBookingRecord
	@booking_record_id INT,
    @expected_checkin_date DATETIME,
    @expected_checkout_date DATETIME,
    @actual_checkin_date DATETIME,
    @actual_checkout_date DATETIME,
    @deposit FLOAT,
    @surcharge FLOAT,
    @note NVARCHAR(255),
    @status NVARCHAR(25),
    @representative_id INT,
    @room_id INT
AS
BEGIN
    BEGIN TRANSACTION;
	BEGIN TRY
		UPDATE BOOKING_RECORD 
		SET 
		expected_checkin_date = @expected_checkin_date, 
		expected_checkout_date = @expected_checkout_date, 
		deposit = @deposit,
		surcharge = @surcharge, 
		note = @note, 
		status = @status, 
		actual_checkin_date = @actual_checkin_date, 
		actual_checkout_date = @actual_checkout_date, 
		room_id = @room_id, 
		representative_id = @representative_id
		WHERE booking_record_id = @booking_record_id
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
	
EXEC proc_updateBookingRecord
	@booking_record_id = 1,
    @expected_checkin_date = '2023-11-05',
    @expected_checkout_date = '2023-11-10',
    @actual_checkin_date = '2023-11-06',
    @actual_checkout_date = '2023-11-09',
    @deposit = 1000000,
    @surcharge = 20000,
    @note = N'Additional notes',
    @status = N'Chờ xác nhận',
    @representative_id = 1,
    @room_id = 1;

select * from BOOKING_RECORD WHERE booking_record_id = 1;

-- Tìm hồ sơ đặt phòng của khách hàng có tên là Nguyễn Văn A

CREATE OR ALTER FUNCTION func_getBookingRecordByCustomerName (@customer_name NVARCHAR(50))
RETURNS table
AS
RETURN ( SELECT * FROM View_Booking_Record WHERE @customer_name like @customer_name);

SELECT * FROM View_Booking_Record


SELECT * FROM func_getBookingRecordByCustomerName(N'Nguyen Van A');

--- Tìm hồ sơ đặt phòng của phòng 101

drop FUNCTION func_getBookingRecordByRoomName;

CREATE OR ALTER FUNCTION func_getBookingRecordByRoomName (@room_name NVARCHAR(50))
RETURNS table
AS
RETURN (
    SELECT *
    FROM View_Booking_Record
    WHERE room_name LIKE @room_name
);

SELECT * FROM func_getBookingRecordByRoomName('101');

CREATE OR ALTER FUNCTION func_getBookingRecordPriceRange (@fromPrice float, @toPrice float)
RETURNS @BookingRecordList TABLE (booking_record_id VARCHAR(10), booking_time DATETIME, status NVARCHAR(25), 
room_id INT, room_name NVARCHAR(25), customer_name NVARCHAR(50))
AS
BEGIN
 INSERT INTO @BookingRecordList
 SELECT booking_record_id, booking_time, status, room_id, room_name, customer_name
 FROM View_Booking_Record WHERE total_cost between @fromPrice and @toPrice
 RETURN
END

SELECT * FROM func_getBookingRecordByRang('102');

--2. **Dịch vụ**

--Dịch vụ

--Thêm dịch vụ phòng
CREATE or ALTER PROCEDURE proc_insertServiceRoom
    @service_room_name NVARCHAR(50),
    @service_room_price FLOAT,
    @discount_service FLOAT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO SERVICE_ROOM (service_room_name, service_room_price, discount_service)
        VALUES (@service_room_name, @service_room_price, @discount_service);
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;


EXEC proc_InsertServiceRoom
    @service_room_name = 'Example Room',
    @service_room_price = 100000,
    @discount_service = 0.1;

SELECT * FROM SERVICE_ROOM;

----Xóa dịch vụ phòng

CREATE or ALTER PROCEDURE proc_deleteServiceRoom
    @service_room_id INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM SERVICE_ROOM where SERVICE_ROOM.service_room_id = @service_room_id
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
 
 exec proc_deleteServiceRoom @service_room_id = 6;

 SELECT * FROM SERVICE_ROOM;

-- --Cập nhật dịch vụ phòng
CREATE or ALTER PROCEDURE proc_updateServiceRoom
	@service_room_id INT,
	@service_room_name NVARCHAR(50),
	@service_room_status BIT,
	@service_room_price FLOAT,
	@discount_service FLOAT
AS
BEGIN
BEGIN TRANSACTION;
	BEGIN TRY
		UPDATE SERVICE_ROOM
		SET
			service_room_name = @service_room_name,
			service_room_status = @service_room_status,
			service_room_price = @service_room_price,
			discount_service = @discount_service
		WHERE service_room_id = @service_room_id;
		COMMIT;
	END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;

exec proc_updateServiceRoom 
	@service_room_id = 1002,
	@service_room_name = N'Vay đồ',
	@service_room_status = 1,
	@service_room_price = 32943243,
	@discount_service = 3249333;

Select * from SERVICE_ROOM;

----Tìm kiếm theo tên dịch vụ 

CREATE or ALTER FUNCTION func_searchByServiceName (@service_room_name NVARCHAR(50)) 
RETURNS table
AS 
	RETURN ( SELECT * FROM View_Service WHERE service_room_name = @service_room_name);

SELECT * FROM func_searchByServiceName(N'Giặt ủi, là');

-- Tìm kiếm dịch vụ trong khoảng giá

CREATE OR ALTER FUNCTION func_searchInPriceRange(@service_room_price1 FLOAT, @service_room_price2 FLOAT)
	RETURNS table
AS RETURN ( SELECT * FROM View_Service WHERE service_room_price between @service_room_price1 and @service_room_price2)

SELECT * FROM func_searchInPriceRange(10000, 15000);

--Mẫu thông tin sử dụng dịch vụ

--Thêm mẫu thông tin sử dụng dịch vụ


CREATE or ALTER PROCEDURE proc_insertServiceUsageInfor
	@number_of_service INT,
	@total_fee FLOAT,
	@booking_record_id INT,
	@service_room_id INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO SERVICE_USAGE_INFOR (number_of_service, total_fee, booking_record_id, service_room_id)
        VALUES (@number_of_service, @total_fee, @booking_record_id, @service_room_id);
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
	
exec proc_insertServiceUsageInfor
	@number_of_service = 2,
	@total_fee = 23241,
	@booking_record_id = 1,
	@service_room_id = 2

select * from View_Service_Usage_Info;

----Xóa mẫu thông tin sử dụng dịch vụ
select *from SERVICE_USAGE_INFOR
CREATE or ALTER PROCEDURE proc_deleteServiceUsageInfor
    @service_usage_infor_id INT
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM SERVICE_USAGE_INFOR where SERVICE_USAGE_INFOR.service_usage_infor_id = @service_usage_infor_id
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;
 
 exec proc_deleteServiceUsageInfor @service_usage_infor_id = 1;

-- --Cập nhật mẫu thông tin sử dụng dịch vụ

CREATE or ALTER PROCEDURE proc_updateServiceUsageInfor
	@service_usage_infor_id INT,
	@number_of_service INT,
	@total_fee FLOAT,
	@booking_record_id INT,
	@service_room_id INT
AS
BEGIN
	BEGIN TRANSACTION;
	BEGIN TRY
		UPDATE SERVICE_USAGE_INFOR
		SET
			number_of_service = @number_of_service,
			total_fee = @total_fee, 
			booking_record_id = @booking_record_id, 
			service_room_id = @service_room_id
		WHERE service_usage_infor_id = @service_usage_infor_id;
		COMMIT;

	END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;

exec proc_updateServiceUsageInfor
	@service_usage_infor_id = 2,
	@number_of_service = 2,
	@total_fee = 23241,
	@booking_record_id = 1,
	@service_room_id = 2


Select * from SERVICE_USAGE_INFOR;

----Tìm kiếm mẫu thông tin sử dụng dịch vụ 
--1. Tìm kiếm những mẫu thông tin dịch vụ sử dụng trong tháng (1,2,..)

CREATE or ALTER FUNCTION func_searchServiceUsageInforByMonth(@month INT)
RETURNS table
AS
	RETURN ( SELECT * FROM View_Service_Usage_Info WHERE month(date_used) = @month);

SELECT * FROM func_searchServiceUsageInforByMonth('11');
Select * from View_Service_Usage_Info;
Select * from SERVICE_USAGE_INFOR;

--2. Tìm kiếm những mẫu thông tin sử dịch vụ của khách hàng A
CREATE or ALTER FUNCTION func_searchServiceUsageInforByCustomerName(@customer_name NVARCHAR(50))
RETURNS table
AS
	RETURN ( SELECT * FROM View_Service_Usage_Info WHERE customer_name = @customer_name);

SELECT * FROM func_searchServiceUsageInforByCustomerName('Nguyen Van A');
Select * from View_Service_Usage_Info;
Select * from SERVICE_USAGE_INFOR;



--3. **Nhân viên**
SELECT * FROM EMPLOYEE;

--THÊM NHÂN VIÊN

CREATE OR ALTER PROCEDURE proc_insertEmployee
    @employee_name NVARCHAR(50),
    @gender NVARCHAR(10),        
    @birthday DATE,
    @identify_card VARCHAR(25),
    @address NVARCHAR(255),
    @email VARCHAR(255) 
AS
BEGIN
    BEGIN TRANSACTION;
    BEGIN TRY
        INSERT INTO EMPLOYEE (employee_name, gender, birthday, identify_card, address, email)
        VALUES (@employee_name, @gender, @birthday, @identify_card, @address, @email);
        COMMIT;
    END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX)
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE() -- Added a closing parenthesis here
        RAISERROR(@err, 16, 1);
    END CATCH
END;

EXEC proc_insertEmployee
    @employee_name = N'Nguyen Văn A',
    @gender = N'Nam',        
    @birthday = '1990-01-01',
    @identify_card = '123456789101',
    @address = N'Some Address', 
    @email = 'email@gmail.com'; 

SELECT * FROM EMPLOYEE;

----Xóa NHÂN VIÊN
--- !!! UPDATE : proc delete employee -> delete account have permission ---
CREATE or ALTER PROCEDURE proc_deleteEmployee
    @employee_id INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @username varchar(15);
		SELECT @username=username FROM ACCOUNT WHERE employee_id=@employee_id
		DECLARE @sql varchar(100)
		DECLARE @SessionID INT;
		SELECT @SessionID = session_id
		FROM sys.dm_exec_sessions
		WHERE login_name = @username;
		IF @SessionID IS NOT NULL
		BEGIN
		SET @sql = 'kill ' + Convert(NVARCHAR(20), @SessionID)
		exec(@sql)
		END
	BEGIN TRANSACTION;
    BEGIN TRY
        DELETE FROM EMPLOYEE where employee_id = @employee_id
		--
		SET @sql = 'DROP USER '+ @username
		exec (@sql)
		--
		SET @sql = 'DROP LOGIN '+ @username
		exec (@sql)
		--
		DELETE FROM ACCOUNT WHERE employee_id=@employee_id;
        COMMIT;
    END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
		ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END;
 
 exec proc_deleteEmployee @employee_id = 7;

 SELECT * FROM EMPLOYEE;

-- --Cập nhật Nhân viên
CREATE or ALTER PROCEDURE proc_updateEmployee
    @employee_id INT,
	@employee_name NVARCHAR(50),
    @gender NVARCHAR(10),        
    @birthday DATE,
    @identify_card VARCHAR(25),
    @address NVARCHAR(255),
    @email VARCHAR(255) 
AS
BEGIN
BEGIN TRANSACTION;
	BEGIN TRY
		UPDATE EMPLOYEE
		SET
			employee_name = @employee_name,
			gender = @gender,
			birthday = @birthday,
			identify_card = @identify_card,
			address = @address,
			email = @email
		WHERE employee_id = @employee_id;
		COMMIT;
	END TRY
   	BEGIN CATCH
		DECLARE @err NVARCHAR(MAX)
		SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
		RAISERROR(@err, 16, 1)
	END CATCH
END;

EXEC proc_updateEmployee
	@employee_id = 8,
    @employee_name = N'Nguyen Văn B',
    @gender = N'Nam',        
    @birthday = '1990-01-01',
    @identify_card = '123456789101',
    @address = N'Some Address', 
    @email = 'email@gmail.com'; 

 SELECT * FROM EMPLOYEE;

----Tìm kiếm theo tên nhân viên

CREATE or ALTER FUNCTION func_searchByEmployeeName (@employee_name NVARCHAR(50)) 
RETURNS table
AS 
	RETURN ( SELECT * FROM View_Front_Desk_Employee WHERE employee_name = @employee_name);

--4. **Tài khoản
--THÊM ACCOUNT

CREATE OR ALTER PROCEDURE proc_insertAccount
    @username NVARCHAR(50),
    @password VARCHAR(25),
    @employee_id INT,
    @roles NVARCHAR(20)
AS
BEGIN
    BEGIN TRAN
    BEGIN TRY
        -- Thêm tài khoản
        INSERT INTO ACCOUNT (username, password, employee_id, roles) VALUES (@username, @password, @employee_id, @roles);

        DECLARE @sqlString NVARCHAR(2000)

        -- Tạo tài khoản login cho nhân viên, tên người dùng và mật khẩu là tài khoản được tạo trên bảng Account
        SET @sqlString = 'CREATE LOGIN [' + @username + '] WITH PASSWORD=''' + @password + ''', DEFAULT_DATABASE=[HotelManagementSystem], CHECK_EXPIRATION=OFF, CHECK_POLICY=OFF'
        EXEC (@sqlString)

        -- Tạo tài khoản người dùng đối với nhân viên đó trên database (tên người dùng trùng với tên login)
		SET @sqlString = 'CREATE USER ' + @username + ' FOR LOGIN ' + @username;
		EXEC (@sqlString)


        -- Thêm người dùng vào vai trò quyền tương ứng (Staff hoặc Manager(sysadmin))
        IF (@roles = 'sysadmin')
            SET @sqlString = 'ALTER SERVER ROLE sysadmin ADD MEMBER ' + @username;
        ELSE
            SET @sqlString = 'ALTER ROLE Staff ADD MEMBER ' + @username;

        EXEC (@sqlString)

        COMMIT TRAN
    END TRY
    BEGIN CATCH
        ROLLBACK TRAN
        DECLARE @err NVARCHAR(MAX)
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE()
        RAISERROR(@err, 16, 1)
    END CATCH
END


EXEC proc_insertAccount
    @username = 'tuyenne',
    @password = '123446',
    @employee_id = 5,
	@roles = 'staff'

EXEC proc_insertAccount
    @username = 'tuyenne1',
    @password = '123446',
    @employee_id = 5,
	@roles = 'sysadmin'


----Xóa ACCOUNT

-- không cho phép xóa account
-- xóa nhân viên -> cascade xóa account

-- --Cập nhật ACCOUNT
CREATE OR ALTER PROCEDURE proc_updateAccount
    @account_id INT,
    @username NVARCHAR(50),
    @password VARCHAR(25),
    @employee_id INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE ACCOUNT
        SET
            employee_id = @employee_id
        WHERE account_id = @account_id;

        COMMIT;
    END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
END;



select * from account;
EXEC proc_updateAccount
	@account_id = 1,
    @username = 'CAM ON',
    @password = '123446',
    @employee_id = 1

SELECT * FROM ACCOUNT;

--5.Doanh thu:

CREATE OR ALTER FUNCTION f_Calculate_Revenue
(@StartDay DATETIME, @EndDay DATETIME)
RETURNS TABLE
AS
RETURN (SELECT MONTH(BILL.created_date) AS Month, YEAR(BILL.created_date) AS Year, SUM(BILL.total_cost) AS ToTal 
		FROM BILL 
		WHERE BILL.created_date IS NOT NULL AND BILL.created_date >= @StartDay AND BILL.created_date <= @EndDay
		GROUP BY MONTH(BILL.created_date), YEAR(BILL.created_date));


--6.3 Function trả về 1 giá trị
CREATE FUNCTION f_Calculate_Total_Revenue

(@StartDay DATETIME, @EndDay DATETIME) 

RETURNS FLOAT

AS 

BEGIN

DECLARE @Total FLOAT;

SELECT @Total = SUM(BILL.total_cost)

FROM BILL  

WHERE BILL.paytime IS NOT NULL AND BILL.created_date >= @StartDay AND BILL.created_date <= @EndDay ;

RETURN @Total

END


--2. **Khách hàng**

--2.1. **PROCEDURE**
---2.1.1. Add new customer information


 CREATE PROCEDURE proc_add_customer
    	@customer_name nvarchar(50),
    	@gender nvarchar(10),
    	@birthday date,
    	@identify_card varchar(25),
    	@phone_number varchar(15),
    	@email varchar(255),
    	@address nvarchar(255),
    	@status bit
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	INSERT INTO
    	 CUSTOMER(customer_name,gender,birthday,identify_card,email,address,status)
           	VALUES
    	 (@customer_name,@gender,@birthday,@identify_card,@email,@address,@status)
           	DECLARE @id int
           	SET @id = (SELECT TOP 1 CUSTOMER.customer_id FROM CUSTOMER ORDER BY CUSTOMER.customer_id DESC)
           	INSERT INTO
           	PHONE_NUMBER_OF_CUSTOMER(customer_id, phone_number)
           	VALUES
           	(@id, @phone_number)
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_add_customer
     @customer_name = 'Test name',
    	@gender = N'Nữ',
    	@birthday =  '2003-12-10',
    	@identify_card = '723546235875',
    	@phone_number = '0123456781',
    	@email = 'test234@gmail.com',
    	@address = 'aaaa',
    	@status = 0

---2.1.2. Remove customer information


 CREATE PROCEDURE proc_delete_customer
    	@customer_id int
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	DELETE FROM PHONE_NUMBER_OF_CUSTOMER WHERE customer_id=@customer_id --Xoa tat ca sdt cua khach hang do--
           	DELETE FROM CUSTOMER WHERE customer_id=@customer_id
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH	
 END

 exec proc_delete_customer @customer_id = 24

---2.1.3. Update customer information (If update phone number of customer, then update in PHONE_NUMBER_OF_CUSTOMER table too)


 CREATE PROCEDURE proc_update_customer
    	@customer_id int,
    	@customer_name nvarchar(50),
    	@gender nvarchar(10),
    	@birthday date,
    	@identify_card varchar(25),
    	@email varchar(255),
    	@address nvarchar(255),
    	@status bit
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	-- Cập nhật thông tin khách hàng trong bảng CUSTOMER --
           	UPDATE CUSTOMER
           	SET
                   	customer_name = @customer_name,
                   	gender = @gender,
                   	birthday = @birthday,
                   	identify_card = @identify_card,
                   	email = @email,
                   	address = @address,
                   	status = @status
           	WHERE CUSTOMER.customer_id = @customer_id
 
           	COMMIT TRAN  -- Hoàn thành giao dịch cho cả hai lệnh UPDATE
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_update_customer
    	@customer_id = 1,
    	@customer_name = 'heheaaa',
    	@gender = N'Nam',
    	@birthday =  '2003-12-11',
    	@identify_card = '723546235875',
    	@phone_number = '0123456123',
    	@email = 'test1.2.3@gmail.com',
    	@address = 'update ne',
    	@status = 1

--2.2. **FUNCTION**
---2.2.1. Search customer information (NOT AVAILABLE)

 CREATE FUNCTION func_search_customer(@string nvarchar(50))
 RETURNS TABLE
 AS
 RETURN(
 SELECT *
 FROM CUSTOMER
 WHERE CONCAT(customer_name,
 			gender,
 			identify_card,
 			email,
 			address,
 			(SELECT STRING_AGG(phone_number, ',') AS phone_numbers
 				FROM PHONE_NUMBER_OF_CUSTOMER 
 				WHERE customer_id = customer_id)
 			) LIKE '%' + @string + '%')

 SELECT * FROM SEARCH_CUSTOMER('hehe')

--2.2.2. Search customer information by range of birthday

 CREATE FUNCTION func_search_customer_by_dob
 (@fromdate DateTime , @todate DateTime)
 RETURNS TABLE
 AS RETURN
 (
 SELECT *
 FROM CUSTOMER
 WHERE birthday BETWEEN @fromdate AND @todate)

 SELECT * FROM func_search_customer_by_dob('1990-01-15', '2003-12-10')

--3. **HÓA ĐƠN**
---3.1. **PROCEDURE**
--3.1.1. Add new BILL (PENDING)

CREATE PROCEDURE proc_add_new_bill 
@costs_incurred float, 
@content_incurred NVARCHAR(255), 
@total_cost float, 
@payment_method NVARCHAR(15), 
@pay_time DATETIME, 
@booking_record_id int, 
@employee_id int 
AS 
BEGIN 
   	BEGIN TRANSACTION 
   	BEGIN TRY 
          	INSERT INTO BILL (costs_incurred, content_incurred, total_cost, payment_method, paytime, booking_record_id, employee_id) 
			VALUES (@costs_incurred, @content_incurred, @total_cost, @payment_method, @pay_time, @booking_record_id, @employee_id) 
			COMMIT TRAN 
   	END TRY 
   	BEGIN CATCH 
          	DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
   	END CATCH 
END 
 
--3.1.2. Delete BILL

 CREATE PROCEDURE proc_delete_bill
 @bill_id VARCHAR(20)
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	DELETE FROM BILL WHERE bill_id = @bill_id
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_delete_bill @bill_id = 1

--3.1.2. Update BILL information
 CREATE PROCEDURE proc_update_bill
 @bill_id int,
 @costs_incurred float,
 @content_incurred NVARCHAR(255),
 @total_cost float,
 @payment_method NVARCHAR(15),
 @pay_time DATETIME
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	UPDATE BILL
           	SET
           	costs_incurred = @costs_incurred,
           	content_incurred = @content_incurred,
           	total_cost = @total_cost,
           	payment_method = @payment_method,
           	paytime = @pay_time
           	WHERE bill_id = @bill_id
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_update_bill
 @bill_id = 2,
 @costs_incurred = 2,
 @content_incurred = N'Hiiii',
 @total_cost = 811111,
 @payment_method = N'Tiền mặt',
 @pay_time = '2023-11-21 11:00:00'

---3.2. **FUNCTION**
--3.2.1. Search BILL information (PENDING)

CREATE FUNCTION func_search_bill(@string nvarchar(50)) 
RETURNS TABLE 
AS 
RETURN 
( 
SELECT * 
FROM View_Bill 
WHERE CONCAT(costs_incurred, content_incurred, total_cost, payment_method, employee_name, customer_name) LIKE '%' + @string + '%' 
) 

 SELECT * FROM func_search_bill(N'Tiền mặt')

--3.2.2. Search BILL information by customer id

 CREATE FUNCTION func_search_bill_by_customer
 (@customer_id int)
 RETURNS TABLE
 AS
 RETURN
 (
  SELECT DISTINCT b.*
  FROM BILL b
  INNER JOIN BOOKING_RECORD br on b.booking_record_id = br.booking_record_id
  WHERE br.representative_id = @customer_id
 )

 SELECT * FROM func_search_bill_by_customer(1)

--3.2.3. Search BILL information by pay time
 CREATE FUNCTION func_search_bill_by_paydate
 (@pay_time DateTime)
 RETURNS TABLE
 AS
 RETURN
 (
 SELECT *
 FROM BILL
 WHERE DATEDIFF(day, convert(date, paytime), convert(date, @pay_time)) = 0
 )

 SELECT * FROM func_search_bill_by_paydate('2023-11-21 11:00:00')

--3.2.4. Search BILL information by bill status 
CREATE FUNCTION func_search_bill_by_status(@string nvarchar(50)) 
RETURNS TABLE 
AS 
RETURN 
( 
    SELECT * 
    FROM View_Bill 
    WHERE 
        (@string = N'Chưa thanh toán' AND paytime IS NULL) 
        OR 
        (@string = N'Đã thanh toán' AND paytime IS NOT NULL) 
        OR 
        (@string = N'Tất cả') 
); 
 


--4. **PHÒNG**

--4.1. **PROCEDURE**
---4.1.1. Add new room information


 CREATE PROCEDURE proc_add_room
 @room_name NVARCHAR(25),
 @room_capacity INT,
 @room_status NVARCHAR(20),
 @room_description NVARCHAR(255),
 @room_image VARBINARY(MAX),
 @room_type_id INT
 AS
 BEGIN
    	BEGIN TRAN
    	BEGIN TRY
           	INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
           	VALUES (@room_name, @room_capacity, @room_status, @room_description, @room_image, @room_type_id)
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END;

 EXEC proc_add_room
 @room_name = '301',
 @room_capacity = 2,
 @room_status = N'Trống',
 @room_description = N'Suite room with a view',
 @room_image = null,
 @room_type_id = 5

---4.1.2. Delete room information

 CREATE PROCEDURE proc_delete_room
 @room_id int
 AS
 BEGIN
    	BEGIN TRANSACTION
    	BEGIN TRY
           	DELETE FROM ROOM WHERE room_id=@room_id
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_delete_room
 @room_id = 1

---4.1.3. Update room information

 CREATE PROCEDURE proc_update_room
 @room_id INT,
 @room_name NVARCHAR(25),
 @room_capacity INT,
 @room_status NVARCHAR(20),
 @room_description NVARCHAR(255),
 @room_image VARBINARY(MAX),
 @room_type_id INT
 AS
 BEGIN
 BEGIN TRANSACTION
    	BEGIN TRY
           	UPDATE ROOM
           	SET
           	room_name = @room_name,
           	room_capacity = @room_capacity,
           	room_status = @room_status,
           	room_description = @room_description,
           	room_image = @room_image,
           	room_type_id = @room_type_id
           	WHERE room_id = @room_id
           	COMMIT TRAN
    	END TRY
		BEGIN CATCH
			DECLARE @err NVARCHAR(MAX);
			SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
			ROLLBACK; 
			RAISERROR(@err, 16, 1);
		END CATCH
 END

 EXEC proc_update_room
 @room_id = 1,
 @room_name = '303',
 @room_capacity = 4,
 @room_status = N'Đang sửa',
 @room_description = 'Family room with a view',
 @room_image = null,
 @room_type_id = 3

--4.2. **FUNCTION**
---4.2.1. Search room information


 CREATE FUNCTION func_search_room(@string nvarchar(50))
 RETURNS TABLE
 AS
 RETURN
 (
 SELECT *
 FROM ROOM
 WHERE CONCAT(convert(nvarchar(25),room_id), room_name, room_status, room_description) LIKE '%' + @string + '%'
 )

 SELECT * FROM func_search_room(N'Đang cho thuê')

---4.2.2. Search room information by room type

 CREATE FUNCTION func_search_room_by_type(@string nvarchar(50))
 RETURNS TABLE
 AS
 RETURN
 (
 SELECT ROOM.*
 FROM ROOM INNER JOIN ROOM_TYPE ON ROOM.room_type_id = ROOM_TYPE.room_type_id
 WHERE ROOM_TYPE.room_type_name LIKE '%' + @string + '%'
 )

 SELECT * FROM func_search_room_by_type(N'Superior')


--5. **LOẠI PHÒNG**

--5.1. **PROCEDURE**
---5.1.1. Add new room type information

 CREATE PROCEDURE proc_add_room_type
 @room_type_name NVARCHAR(25),
 @price float,
 @discount_room float
 AS
 BEGIN
 	BEGIN TRAN
 	BEGIN TRY
        	INSERT INTO ROOM_TYPE(room_type_name, price, discount_room)
        	VALUES (@room_type_name, @price, @discount_room)
        	COMMIT TRAN
 	END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
 END

 EXEC proc_add_room_type
 @room_type_name = 'Family of 5',
 @price = 3200000,
 @discount_room = 0.2

---5.1.2. Delete room type information

 CREATE OR ALTER PROCEDURE proc_update_room_type
 @room_type_id int,
 @room_type_name NVARCHAR(25),
 @price float,
 @discount_room float
 AS
 BEGIN
 	BEGIN TRANSACTION
 	BEGIN TRY
        	UPDATE ROOM_TYPE
			SET room_type_name = @room_type_name,
			price = @price, 
			discount_room = @discount_room
			WHERE room_type_id=@room_type_id
        	COMMIT TRAN
 	END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
 END

 EXEC proc_update_room_type
 @room_type_id = 2,
  @room_type_name = 'Family of 5',
 @price = 3200000,
 @discount_room = 0.2

 SELECT * FROM ROOM_TYPE
---5.1.3. Update room type information

 CREATE OR ALTER PROCEDURE proc_delete_room_type
 @room_type_id int
 AS
 BEGIN
 	BEGIN TRANSACTION
 	BEGIN TRY
        	DELETE FROM ROOM_TYPE WHERE room_type_id=@room_type_id
        	COMMIT TRAN
 	END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
 END

 EXEC proc_delete_room_type
 @room_type_id = 2

--5.2. **FUNCTION**
---5.2.1. Search room type information

 CREATE FUNCTION func_search_room_type(@string nvarchar(50))
 RETURNS TABLE
 AS
 RETURN
 (
 SELECT *
 FROM ROOM_TYPE
 WHERE CONCAT(convert(nvarchar(25),room_type_id), room_type_name, discount_room) LIKE '%' + @string + '%'
 )

 SELECT * FROM func_search_room_type(N'0.1')

---5.2.1. Search room type by price range

 CREATE FUNCTION func_search_room_type_by_price (@fromprice float, @toprice float)
 RETURNS TABLE
 AS
 RETURN
 (
 SELECT *
 FROM ROOM_TYPE
 WHERE ROOM_TYPE.price >= @fromprice AND ROOM_TYPE.price <= @toprice
 )

 SELECT * FROM func_search_room_type_by_price(1, 400000)


 --CUSTOMER OF BOOKING RECORD


--5.1. **PROCEDURE**
---5.1.1. Add
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD

 CREATE OR ALTER PROCEDURE proc_add_customer_of_booking_record
 @customer_id INT,
 @booking_record_id INT
 AS
 BEGIN
 	BEGIN TRAN
 	BEGIN TRY
        	INSERT INTO CUSTOMER_OF_BOOKING_RECORD(customer_id, booking_record_id)
        	VALUES (@customer_id, @booking_record_id)
        	COMMIT TRAN
 	END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
 END

 EXEC proc_add_customer_of_booking_record
 @customer_id = 1,
 @booking_record_id = 2;

 SELECT * FROM CUSTOMER_OF_BOOKING_RECORD


---5.1.2. Delete


 CREATE OR ALTER PROCEDURE proc_delete_customer_of_booking_record
 @customer_id INT,
 @booking_record_id INT
 AS
 BEGIN
 	BEGIN TRANSACTION
 	BEGIN TRY
        	DELETE FROM CUSTOMER_OF_BOOKING_RECORD WHERE booking_record_id=@booking_record_id and customer_id = @customer_id
        	COMMIT TRAN
 	END TRY
    BEGIN CATCH
        DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
    END CATCH
 END

 EXEC proc_delete_customer_of_booking_record
 @customer_id = 1,
 @booking_record_id = 2;

 SELECT * FROM CUSTOMER_OF_BOOKING_RECORD

 ----**6.SỐ ĐIỆN THOẠI KHÁCH HÀNG**------- 
----6.1. **PROCEDURE** 
---6.1.1. Add new phone number of customer 
CREATE or ALTER PROCEDURE proc_add_phone_of_customer 
@phone_number varchar(15), 
@customer_id int 
AS 
BEGIN 
	BEGIN TRAN 
	BEGIN TRY 
       	INSERT INTO PHONE_NUMBER_OF_CUSTOMER(phone_number, customer_id) 
       	VALUES (@phone_number, @customer_id) 
       	COMMIT TRAN 
	END TRY 
	BEGIN CATCH 
       	DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
	END CATCH 
END 
 
--6.1.2. Delete phone number of customer 
CREATE or ALTER PROCEDURE proc_delete_phone_of_customer 
@phone_number varchar(15), 
@customer_id int 
AS 
BEGIN 
	BEGIN TRAN 
	BEGIN TRY 
       	DELETE FROM PHONE_NUMBER_OF_CUSTOMER 
       	WHERE phone_number = @phone_number AND customer_id = @customer_id 
       	COMMIT TRAN 
	END TRY 
	BEGIN CATCH 
       	DECLARE @err NVARCHAR(MAX);
        SELECT @err = N'Lỗi ' + ERROR_MESSAGE();
        ROLLBACK; 
        RAISERROR(@err, 16, 1);
	END CATCH 
END 
 
--6.2. **FUNCTION** 
-6.2.1. Search phone number of customer 
CREATE FUNCTION func_search_phone_of_customer(@string nvarchar(50)) 
RETURNS TABLE 
AS 
RETURN 
( 
SELECT * 
FROM View_Customer_Phone 
WHERE CONCAT(customer_name, customer_id, phone_number) LIKE '%' + @string + '%' 
)



--6.4 Function trả về 1 giá trị chuỗi kết nối khi đăng nhập

CREATE OR ALTER FUNCTION Login
(@username VARCHAR(50), @password VARCHAR(25))
RETURNS VARCHAR(1000)
AS
BEGIN
	DECLARE @connectString VARCHAR(1000);
	DECLARE @roles VARCHAR(20);
	SELECT @roles = roles
	FROM ACCOUNT 
	WHERE username = @username AND password = @password;
	IF @roles IS NOT NULL
	BEGIN
		SET @connectString = 'Data Source=(localdb)\mssqllocaldb;Initial Catalog=HotelManagementSystem;User Id='
		+ @username + ';Password=' + @password + ';';
	END
	RETURN @connectString;
END

SELECT * FROM ACCOUNT;

--thủ tục thêm xóa sửa số điện thoại nhân viên 
CREATE OR ALTER PROCEDURE proc_add_phone_of_employee 
@phone_number VARCHAR(15), 
@employee_id INT 
AS 
BEGIN 
	IF EXISTS (SELECT* FROM PHONE_NUMBER_OF_EMPLOYEE WHERE phone_number = @phone_number)  
	BEGIN 
		RAISERROR(N'Số điện thoại đã tồn tại',16,1); 
		RETURN 
	END 
	IF LEN(@phone_number) != 10 OR @phone_number LIKE '%[^0-9]%' 
	BEGIN 
		RAISERROR(N'Số điện thoại không hợp lệ',16,1); 
		RETURN 
	END 
	IF NOT EXISTS (SELECT* FROM EMPLOYEE WHERE employee_id = @employee_id) 
	BEGIN 
		RAISERROR(N'Nhân viên không tồn tại',16,1); 
		RETURN 
	END 
    INSERT INTO PHONE_NUMBER_OF_EMPLOYEE(phone_number, employee_id) values(@phone_number, @employee_id); 
END 
 
CREATE OR ALTER PROCEDURE proc_delete_phone_of_employee 
@phone_number VARCHAR(15), 
@employee_id INT 
AS 
BEGIN 
    DELETE FROM PHONE_NUMBER_OF_EMPLOYEE 
    WHERE phone_number = @phone_number AND employee_id = @employee_id 
END 
 
CREATE OR ALTER FUNCTION f_search_phone_employee_by_employee_name(@employee_name nvarchar(50)) 
RETURNS @OutputTable TABLE (employee_id INT,employee_name NVARCHAR(50),phone_number VARCHAR(15)) 
AS 
BEGIN 
	   INSERT INTO @OutputTable 
	   SELECT EMPLOYEE.employee_id, employee_name, phone_number 
	   FROM PHONE_NUMBER_OF_EMPLOYEE JOIN EMPLOYEE ON EMPLOYEE.employee_id = PHONE_NUMBER_OF_EMPLOYEE.employee_id 
	   WHERE EMPLOYEE.employee_name = @employee_name 
	   RETURN; 
END 
<<<<<<< HEAD
	   

CREATE or ALTER PROC proc_updateAccount
(@username NVARCHAR(50), @password VARCHAR(25))
AS
IF EXISTS (SELECT* FROM ACCOUNT WHERE username = @username)
BEGIN
	UPDATE ACCOUNT SET password = @password WHERE username = @username;
	DECLARE @sql VARCHAR(200) = 'ALTER LOGIN [' + @username + '] WITH PASSWORD=''' + @password + '''';
	EXEC(@sql);
END
=======
	   
>>>>>>> 424ff03a86a3c6ae44701598e7ccdedfe7ce0f45
