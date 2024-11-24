-- Khách Hàng
CREATE TRIGGER Trg_Insert_New_Customer
ON CUSTOMER
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of customer
   DECLARE @customer_id INT 
   SET @customer_id = (SELECT customer_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM CUSTOMER WHERE customer_id IN (@customer_id)) AND @customer_id <> null
   BEGIN
       RAISERROR(N'Mã khách hàng không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Customer Name
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(customer_name) = ' ')
   BEGIN
       RAISERROR(N'Tên khách hàng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Gender of Customer
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(gender) = ' ')
   BEGIN
       RAISERROR(N'Giới tính khách hàng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
   IF NOT EXISTS (SELECT * FROM inserted WHERE gender IN (N'Nam', N'Nữ'))
   BEGIN
       RAISERROR(N'Giới tính khách hàng phải là "Nam" hoặc "Nữ"', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Status of Customer
   IF EXISTS (SELECT * FROM inserted WHERE status is null)
   BEGIN
       RAISERROR(N'Trạng thái khách hàng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Hóa Đơn
CREATE TRIGGER Trg_Insert_New_Bill
ON BILL
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Bill
   DECLARE @bill_id INT 
   SET @bill_id = (SELECT bill_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM BILL WHERE bill_id IN (@bill_id)) AND @bill_id <> null
   BEGIN
       RAISERROR(N'Mã hóa đơn không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check payment_method 
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(payment_method) = ' ')
   BEGIN
       RAISERROR(N'Phương thức thanh toán không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check booking_record_id 
   IF EXISTS (SELECT * FROM inserted WHERE booking_record_id is NULL)
   BEGIN
       RAISERROR(N'Mã hồ sơ đặt phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Phòng
CREATE TRIGGER Trg_Insert_New_Room
ON ROOM
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Room
   DECLARE @room_id INT 
   SET @room_id = (SELECT room_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM ROOM WHERE room_id IN (@room_id)) AND @room_id <> null
   BEGIN
       RAISERROR(N'Mã phòng không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Name of Room 
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(room_name) = ' ')
   BEGIN
       RAISERROR(N'Tên phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Capacity of Room 
   IF EXISTS (SELECT * FROM inserted WHERE room_capacity is NULL)
   BEGIN
       RAISERROR(N'Sức chứa của phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Status of Room 
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(room_status) = ' ')
   BEGIN
       RAISERROR(N'Trạng thái phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Room Type of Room 
   IF EXISTS (SELECT * FROM inserted WHERE room_type_id is NULL)
   BEGIN
       RAISERROR(N'Loại phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Loại Phòng
CREATE TRIGGER Trg_Insert_New_Room_Type
ON ROOM_TYPE
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Room Type
   DECLARE @room_type_id INT 
   SET @room_type_id = (SELECT room_type_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM Room_TYPE WHERE room_type_id IN (@room_type_id)) AND @room_type_id <> null
   BEGIN
       RAISERROR(N'Mã loại phòng không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Name of Room Type
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(room_type_name) = ' ')
   BEGIN
       RAISERROR(N'Tên loại phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Price of Room Type
   IF EXISTS (SELECT * FROM inserted WHERE price is NULL)
   BEGIN
       RAISERROR(N'Giá phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Discount of Room 
   IF EXISTS (SELECT * FROM inserted WHERE room_type_id is NULL)
   BEGIN
       RAISERROR(N'Giảm giá không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Đặt phòng
CREATE TRIGGER Trg_Insert_New_Booking_Record
ON BOOKING_RECORD
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Booking Record
   DECLARE @booking_record_id INT 
   SET @booking_record_id = (SELECT booking_record_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM BOOKING_RECORD WHERE booking_record_id IN (@booking_record_id)) AND @booking_record_id <> null
   BEGIN
       RAISERROR(N'Mã hồ sơ đặt phòng không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Expected Checkin Date of Booking Record
   IF EXISTS (SELECT * FROM inserted WHERE expected_checkin_date is NULL)
   BEGIN
       RAISERROR(N'Ngày checkin dự kiến không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Expected Checkout Date of Booking Record
   IF EXISTS (SELECT * FROM inserted WHERE expected_checkout_date is NULL)
   BEGIN
       RAISERROR(N'Ngày checkout dự kiến không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Status of Booking Record
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(status) = ' ')
   BEGIN
       RAISERROR(N'Trạng thái hồ sơ không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check room_id of Booking Record
   IF EXISTS (SELECT * FROM inserted WHERE room_id is null)
   BEGIN
       RAISERROR(N'Mã phòng không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check representative_id of Booking Record
   IF EXISTS (SELECT * FROM inserted WHERE representative_id is null)
   BEGIN
       RAISERROR(N'Mã người đại diện không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
    
END;

-- Dịch vụ
CREATE TRIGGER Trg_Insert_New_Service_Room
ON SERVICE_ROOM
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Service
   DECLARE @service_room_id INT 
   SET @service_room_id = (SELECT service_room_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM SERVICE_ROOM WHERE service_room_id IN (@service_room_id)) AND @service_room_id <> null
   BEGIN
       RAISERROR(N'Mã dịch vụ không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Name of Service
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(service_room_name) = ' ')
   BEGIN
       RAISERROR(N'Tên dịch vụ không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Status of Service
   IF EXISTS (SELECT * FROM inserted WHERE service_room_status is NULL)
   BEGIN
       RAISERROR(N'Trạng thái dịch vụ không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Price of Service
   IF EXISTS (SELECT * FROM inserted WHERE service_room_price is null or service_room_price < 0)
   BEGIN
       RAISERROR(N'Giá thái dịch vụ không được để trống và lớn hơn 0', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Nhân viên
CREATE TRIGGER Trg_Insert_New_Employee
ON EMPLOYEE
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Employee
   DECLARE @employee_id INT 
   SET @employee_id = (SELECT employee_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM EMPLOYEE WHERE employee_id IN (@employee_id)) AND @employee_id <> null
   BEGIN
       RAISERROR(N'Mã nhân viên không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Name of Employee
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(employee_name) = ' ')
   BEGIN
       RAISERROR(N'Tên nhân viên không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Birthday of Employee
   IF EXISTS (SELECT * FROM inserted WHERE birthday is null)
   BEGIN
       RAISERROR(N'Sinh nhật không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Address of Employee
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(address) = ' ')
   BEGIN
       RAISERROR(N'Địa chỉ không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Email of Employee
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(email) = ' ')
   BEGIN
       RAISERROR(N'Email không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Identify Card of Employee
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(identify_card) = ' ')
   BEGIN
       RAISERROR(N'Chứng minh thư không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
END;

-- Tài khoản
CREATE TRIGGER Trg_Insert_New_Account
ON ACCOUNT
FOR INSERT, UPDATE
AS
BEGIN
   --Check Id of Account
   DECLARE @account_id INT 
   SET @account_id = (SELECT account_id FROM inserted)
   IF NOT EXISTS (SELECT * FROM ACCOUNT WHERE account_id IN (@account_id)) AND @account_id <> null
   BEGIN
       RAISERROR(N'Mã tài khoản không tồn tại', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check Username of Account
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(username) = ' ')
   BEGIN
       RAISERROR(N'Username không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check password of Account
   IF EXISTS (SELECT * FROM inserted WHERE TRIM(password) = ' ')
   BEGIN
       RAISERROR(N'Mật khẩu không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END

   --Check employee_id of Employee
   IF EXISTS (SELECT * FROM inserted WHERE employee_id is null)
   BEGIN
       RAISERROR(N'Mã nhân viên không được để trống', 16, 1)
       ROLLBACK TRANSACTION
       RETURN
   END
    
END;
