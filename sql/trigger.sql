--- nhập số điện thoại của khác hàng?

 --2.6.1. Trigger when customer come to checkin then (adding a new booking record: đối với khách đặt trực tiếp + checkin liền luôn
 --or update a new booking record: đối với khách đặt online)
 --to update room status to 'Đang cho thuê'

-- drop all trigger
DECLARE @triggerName NVARCHAR(MAX)

DECLARE triggerCursor CURSOR FOR
    SELECT name
    FROM sys.triggers

OPEN triggerCursor
FETCH NEXT FROM triggerCursor INTO @triggerName

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @sql NVARCHAR(MAX)
    SET @sql = 'DROP TRIGGER ' + @triggerName
    EXEC sp_executesql @sql

    FETCH NEXT FROM triggerCursor INTO @triggerName
END

CLOSE triggerCursor
DEALLOCATE triggerCursor



CREATE OR ALTER TRIGGER Trg_Update_Status_Room_And_Customer_Status_When_Checkin_Vs_Online
ON BOOKING_RECORD
AFTER UPDATE
AS
BEGIN
IF UPDATE(status)
BEGIN
  IF EXISTS (SELECT status FROM inserted WHERE status LIKE N'Đã xác nhận')
	BEGIN
		UPDATE ROOM
		SET room_status = N'Đang cho thuê'
		FROM ROOM
		WHERE ROOM.room_id = (SELECT room_id FROM inserted);

		UPDATE CUSTOMER
		SET status = 1
		FROM CUSTOMER
		WHERE CUSTOMER.customer_id = (SELECT representative_id FROM inserted);
	END
END
END;

-- BEFORE UPDATE
UPDATE ROOM SET room_status = N'Trống' WHERE room_id = 4;
UPDATE CUSTOMER SET status = 0 WHERE customer_id = 4;
DELETE FROM BOOKING_RECORD WHERE booking_record_id = 4;

SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- AFTER UPDATE

UPDATE BOOKING_RECORD SET status = N'Đã xác nhận' WHERE booking_record_id = 4;
SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- OK 

CREATE OR ALTER TRIGGER Trg_Update_Status_Room_And_Customer_Status_When_Checkin_Vs_Offline
ON BOOKING_RECORD
AFTER INSERT
AS
BEGIN
  IF EXISTS (SELECT status FROM inserted WHERE status LIKE N'Đã xác nhận')
	BEGIN
		UPDATE ROOM
		SET room_status = N'Đang cho thuê'
		FROM ROOM
		WHERE ROOM.room_id = (SELECT room_id FROM inserted);

		UPDATE CUSTOMER
		SET status = 1
		FROM CUSTOMER
		WHERE CUSTOMER.customer_id = (SELECT representative_id FROM inserted);
	END
END;

-- BEFORE INSERT

UPDATE ROOM SET room_status = N'Trống' WHERE room_id = 4;
UPDATE CUSTOMER SET status = 0 WHERE customer_id = 4;
DELETE FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4 AND booking_record_id = 4;
DELETE FROM BOOKING_RECORD WHERE booking_record_id = 4;

SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- AFTER INSERT

INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 4, 4);

SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- OK

--- trigger insert vô bảng customer_of_booking_record
--thêm khách hàng đại diện vào bảng customer_of_booking_record??
--- cái này là thêm tất cả khách hàng luôn á. Bữa mình nói là ai đi kèm thì thêm vô đây
-- cái này của t làm là chỉ có thêm dc khách hàng đại diện thôi

CREATE OR ALTER TRIGGER Trg_Insert_Representative_Into_Customer_Of_Booking_Record_After_Checkin
ON BOOKING_RECORD
AFTER INSERT
AS
BEGIN
IF EXISTS (SELECT 1 FROM inserted WHERE status like N'Đã xác nhận')
BEGIN
    DECLARE @customer_id INT = (SELECT c.customer_id FROM CUSTOMER c JOIN inserted i ON c.customer_id = i.representative_id);
	DECLARE @booking_record_id INT = (SELECT booking_record_id FROM  inserted);
	INSERT INTO CUSTOMER_OF_BOOKING_RECORD(customer_id, booking_record_id) VALUES (@customer_id, @booking_record_id)
END
END;

-- BEFORE INSERT

UPDATE ROOM SET room_status = N'Trống' WHERE room_id = 4;
UPDATE CUSTOMER SET status = 0 WHERE customer_id = 4;
DELETE FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4 AND booking_record_id = 4;
DELETE FROM BOOKING_RECORD WHERE booking_record_id = 4;

SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- AFTER INSERT

INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 4, 4);

SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

--- OK

CREATE OR ALTER TRIGGER Trg_Insert_Representative_Into_Customer_Of_Booking_Record_After_Update_Booking
ON BOOKING_RECORD
AFTER UPDATE
AS
IF (UPDATE(status))
BEGIN
IF EXISTS (SELECT 1 FROM inserted WHERE status like N'Đã xác nhận')
BEGIN
    DECLARE @customer_id INT = (SELECT c.customer_id FROM CUSTOMER c JOIN inserted i ON c.customer_id = i.representative_id);
	DECLARE @booking_record_id INT = (SELECT booking_record_id FROM  inserted);
	INSERT INTO CUSTOMER_OF_BOOKING_RECORD(customer_id, booking_record_id) VALUES (@customer_id, @booking_record_id)
END;
END

-- BEFORE UPDATE
UPDATE ROOM SET room_status = N'Trống' WHERE room_id = 4;
UPDATE CUSTOMER SET status = 0 WHERE customer_id = 4;

SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 21;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- AFTER UPDATE

UPDATE BOOKING_RECORD SET status = N'Đã xác nhận' WHERE booking_record_id = 21;
SELECT * FROM BOOKING_RECORD WHERE room_id = 4;
SELECT * FROM CUSTOMER_OF_BOOKING_RECORD WHERE customer_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- BEFORE UPDATE
UPDATE ROOM SET room_status = N'Trống' WHERE room_id = 4;
UPDATE CUSTOMER SET status = 0 WHERE customer_id = 4;

SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;

-- AFTER UPDATE

UPDATE BOOKING_RECORD SET status = N'Đã xác nhận' WHERE booking_record_id = 4;

SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 4;
SELECT * FROM ROOM WHERE room_id = 4;
SELECT * FROM CUSTOMER WHERE customer_id = 4;
-- OK 

-- 2.6.2. Trigger when adding a new booking record to insert a new bill
--đã bỏ not null paymethod và employid ở bảng BILL để insert được

CREATE OR ALTER TRIGGER Trg_Create_Bill_When_Create_Booking_Record
ON BOOKING_RECORD
AFTER INSERT
AS
BEGIN
	DECLARE @booking_record_id INT = (SELECT booking_record_id
    FROM inserted);
    INSERT INTO BILL (booking_record_id, payment_method)
    VALUES (@booking_record_id, N'Tiền mặt');
END;


-- BEFORE INSERT

SELECT * FROM BOOKING_RECORD;
SELECT * FROM BILL;

-- AFTER INSERT

INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 5, 8);

SELECT * FROM BOOKING_RECORD;
SELECT * FROM BILL;

--- OK

 --2.6.3. Trigger when adding a new booking record to check room availability and roll back if not available
 --- CÁI NÀY PHẢI CÓ UPDATE NỮA: TRƯỜNG HỢP KHÁCH ĐỔI PHÒNG Á.
 --- KHÁCH ĐỔI PHÒNG THÌ MÌNH CHANGE CÁI ROOM_id
 --- VẬY THÊM MỘT CÁI NỮA LÀ UPDATE (CHECK(UPDATE(ROOM_ID))

CREATE OR ALTER TRIGGER Trg_Check_Room_Status_To_Insert_Booking
ON BOOKING_RECORD
INSTEAD OF INSERT, UPDATE
AS
BEGIN
    IF EXISTS (
        SELECT *
        FROM ROOM
        INNER JOIN inserted I ON ROOM.room_id = I.room_id
        WHERE ROOM.room_status LIKE N'Trống'
    )
    BEGIN
        INSERT INTO BOOKING_RECORD (
            expected_checkin_date,
            expected_checkout_date,
            deposit,
            surcharge,
            note,
            status,
            actual_checkin_date,
            actual_checkout_date,
            room_id,
            representative_id
        )
        SELECT 
            I.expected_checkin_date,
            I.expected_checkout_date,
            I.deposit,
            I.surcharge,
            I.note,
            I.status,
            I.actual_checkin_date,
            I.actual_checkout_date,
            I.room_id,
            I.representative_id
        FROM inserted I;
    END
    ELSE
    BEGIN
        ROLLBACK;
		RAISERROR(N'Phòng hiện không trống. Không thể lập hợp đồng', 16, 1)
    END;
END;

--- TEST TRƯỜNG HỢP KHÔNG INSERT ĐƯỢC
-- BEFORE INSERT
SELECT * FROM ROOM WHERE room_id = 1;
SELECT * FROM BOOKING_RECORD;

-- AFTER INSERT

INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 1, 8);


--- TEST TRƯỜNG HỢP INSERT ĐƯỢC
-- BEFORE INSERT
SELECT * FROM ROOM WHERE room_id = 18;
SELECT * FROM BOOKING_RECORD;

-- AFTER INSERT

INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 18, 8);

SELECT * FROM ROOM WHERE room_id = 18;
SELECT * FROM BOOKING_RECORD;

--OK

 --2.6.4. Trigger to delete a booking record if the deposit is not paid after 30 minutes

CREATE OR ALTER TRIGGER Trg_Delete_Booking_Record_Not_Paid_Deposit_After_30_min
ON BOOKING_RECORD
AFTER UPDATE
AS
BEGIN
    -- Delete from the BOOKING_RECORD table
    DELETE FROM BOOKING_RECORD
    WHERE GETDATE() > DATEADD(MINUTE, 30, booking_time)
    AND deposit = 0 
    AND booking_time <> expected_checkin_date;
END;

 --2.6.6. Trigger to update actual checkout date in the booking record after payment

CREATE OR ALTER TRIGGER Trg_Update_Actual_Checkout_Date_If_Pay_Bill
ON BILL
AFTER UPDATE
AS
BEGIN
    IF UPDATE(paytime)
    BEGIN
        UPDATE BOOKING_RECORD
        SET actual_checkout_date = i.paytime
        FROM inserted i
        JOIN BOOKING_RECORD br ON i.booking_record_id = br.booking_record_id;
    END
END;

-- BEFORE UPDATE

SELECT * FROM BILL WHERE bill_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;

-- AFTER UPDATE

UPDATE BILL SET paytime = GETDATE() WHERE bill_id = 6;

SELECT * FROM BILL WHERE booking_record_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;
-- OK


 --2.6.7. Trigger to update incurred cost (phí phụ thu) based on late check-out in the booking record
 --Additional charges for late check-out:
 --- From 12:00 PM to 3:00 PM: 30% room price.
 --- From 3:00 PM to 6:00 PM: 50% room price.
 --- After 6:00 PM: 100% room price.

CREATE OR ALTER TRIGGER Trg_Update_Incurred_Cost_If_Checkout_Late
ON BILL
AFTER UPDATE
AS
BEGIN
    IF UPDATE(paytime)
    BEGIN
        UPDATE b
        SET costs_incurred =
            CASE
                WHEN i.paytime > br.expected_checkout_date THEN
                    CASE
                        WHEN i.paytime <= DATEADD(HOUR, 3, br.expected_checkout_date) THEN (rt.price * 0.3)
                        WHEN i.paytime <= DATEADD(HOUR, 6, br.expected_checkout_date) THEN (rt.price * 0.5)
                        ELSE (rt.price)
                    END
                ELSE 0
            END
        FROM BILL b
        JOIN inserted i ON b.bill_id = i.bill_id
        JOIN BOOKING_RECORD br ON br.booking_record_id = i.booking_record_id
        JOIN ROOM r ON br.room_id = r.room_id
        JOIN ROOM_TYPE rt ON rt.room_type_id = r.room_type_id;
    END
END;


---- BEFORE UPDATE
UPDATE BILL SET costs_incurred = 0 WHERE bill_id = 6;
SELECT * FROM BILL WHERE bill_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;

---- AFTER UPDATE

UPDATE BILL SET paytime = GETDATE() WHERE bill_id = 6;

SELECT * FROM BILL WHERE bill_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;
-- OK


 --2.6.8. Trigger to update customer status to unofficial, room status to N'Trống', booking_record status to N'Đã hoàn tất' after payment


CREATE OR ALTER TRIGGER Trg_Update_Customer_Status_To_Unofficial_After_Payment
ON BILL
AFTER UPDATE
AS
BEGIN
    IF UPDATE(paytime)
    BEGIN
		UPDATE BOOKING_RECORD
        SET status = N'Đã hoàn tất'
        FROM inserted i
        JOIN BOOKING_RECORD br ON i.booking_record_id = br.booking_record_id;
		
		UPDATE ROOM
        SET room_status = N'Trống'
        FROM inserted i
        JOIN BOOKING_RECORD br ON i.booking_record_id = br.booking_record_id
		JOIN ROOM r ON br.room_id = r.room_id

	---- CÁI UPDATE CUSTOMER NÀY CHƯA THỰC HIỆN ĐƯỢC DO BẢNG CUSTOMER_OF_BOOKING_RECORD KHÔNG CÓ ĐỦ DỮ LIỆU
        -- UPDATE CUSTOMER
        -- SET status = 0
		-- FROM inserted i
        -- JOIN CUSTOMER_OF_BOOKING_RECORD cbr ON i.booking_record_id = cbr.booking_record_id
        -- JOIN CUSTOMER c ON cbr.customer_id = c.customer_id;
    END
END;


-- BEFORE UPDATE

UPDATE BILL SET costs_incurred = 0 WHERE bill_id = 6;
SELECT * FROM BILL WHERE booking_record_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;
SELECT * FROM ROOM WHERE room_id = 6;
SELECT * FROM CUSTOMER WHERE customer_id = 5;

-- AFTER UPDATE

UPDATE BILL SET paytime = GETDATE() WHERE bill_id = 6;

SELECT * FROM BILL WHERE bill_id = 6;
SELECT * FROM BOOKING_RECORD WHERE booking_record_id = 6;
SELECT * FROM ROOM WHERE room_id = 6;
SELECT * FROM CUSTOMER WHERE customer_id = 5;

-- OK (TẠM THỜI LÀ V)


 --2.6.10. Trigger to update room, booking record, and bill when changing rooms (updating a booking record)

CREATE OR ALTER TRIGGER Trg_Update_Room_Booking_Record_Bill_When_Change_Room
ON BOOKING_RECORD
AFTER UPDATE
AS
BEGIN
IF (UPDATE(room_id))
BEGIN
    DECLARE @room_old VARCHAR(25);
    SET @room_old = (SELECT room_id FROM deleted);

    DECLARE @room_new VARCHAR(25);
    SET @room_new = (SELECT room_id FROM inserted);

    DECLARE @booking_record_id_new VARCHAR(25);
    SET @booking_record_id_new = (SELECT booking_record_id FROM inserted);

    DECLARE @number_of_days INT;
    SET @number_of_days = DATEDIFF(DAY, (SELECT actual_checkin_date FROM inserted), (SELECT actual_checkout_date FROM inserted)) + 1;

    UPDATE ROOM
    SET room_status = N'Trống'
    WHERE room_id = @room_old;

    UPDATE ROOM
    SET room_status = N'Đang cho thuê'
    WHERE room_id = @room_new;

    UPDATE BILL
    SET total_cost = (
	SELECT
            (
                (SELECT price FROM ROOM_TYPE WHERE room_type_id = (SELECT room_type_id FROM ROOM WHERE room_id = @room_new)) * @number_of_days
                + (SELECT SUM(number_of_service * service_room_price) FROM
                    (SERVICE_USAGE_INFOR
                    INNER JOIN SERVICE_ROOM ON SERVICE_USAGE_INFOR.service_room_id = SERVICE_ROOM.service_room_id)
                    WHERE booking_record_id = @booking_record_id_new
                )
            )
            + (SELECT deposit FROM inserted)
            + (SELECT surcharge FROM inserted)
        )
    WHERE booking_record_id = @booking_record_id_new;
	END;
END;

 --2.6.12. Trigger to update customer status to official and apply surcharge (phụ phí) for early check-in and exceeding room capacity (vượt quá số người quy định)
 --This trigger executes after an UPDATE operation on booking record status.

CREATE OR ALTER TRIGGER Trg_Update_Booking_Record_Status_TOfficial
ON BOOKING_RECORD
AFTER INSERT, UPDATE
AS 
BEGIN
  DECLARE @status VARCHAR(10) = (SELECT status FROM inserted);

  IF @status = N'Đã xác nhận'
  BEGIN
    DECLARE @booking_record_id VARCHAR(25);
    DECLARE @actual_checkin_date DATETIME;
    DECLARE @room_id VARCHAR(25);

    SELECT
      @booking_record_id = booking_record_id,
      @room_id = room_id,
      @actual_checkin_date = actual_checkin_date
    FROM inserted;

    -- Calculate room price
    DECLARE @room_price MONEY;
    
    SELECT
      @room_price = price
    FROM ROOM 
    JOIN ROOM_TYPE ON ROOM.room_type_id = ROOM_TYPE.room_type_id            
    WHERE room_id = @room_id;

    DECLARE @checkin_hour INT;
    SET @checkin_hour = FORMAT(@actual_checkin_date, 'HH');

    DECLARE @surcharge INT = (SELECT surcharge FROM inserted);
    DECLARE @note NVARCHAR(255) = (SELECT note FROM inserted);

    -- Apply surcharge for early check-in
    IF @checkin_hour >= 5 AND @checkin_hour < 9
    BEGIN
      SET @surcharge = @surcharge + 0.5 * @room_price;
      SET @note = CONCAT(@note, N'Early check-in from 5 AM to before 9 AM. ');
    END

    IF @checkin_hour >= 9 AND @checkin_hour < 14
    BEGIN
      SET @surcharge = @surcharge + 0.3 * @room_price;
      SET @note = CONCAT(@note, N'Early check-in from 9 AM to before 2 PM. ');

      -- Update the booking record surcharge
      UPDATE BOOKING_RECORD
      SET surcharge = (SELECT surcharge FROM inserted) + 0.3 * @room_price;
    END

    -- Calculate the number of customers
    DECLARE @number_of_customers INT = (
      SELECT COUNT(customer_id) 
      FROM CUSTOMER_OF_BOOKING_RECORD 
      WHERE booking_record_id = @booking_record_id
    );

    -- Get the room capacity
    DECLARE @room_capacity INT = (
      SELECT room_capacity
      FROM ROOM
      WHERE room_id = @room_id
    );

    -- Apply surcharge for exceeding room capacity
    IF @number_of_customers > @room_capacity
    BEGIN
      SET @surcharge = @surcharge + (@number_of_customers - @room_capacity) * 200000;
      SET @note = CONCAT(@note, N'Exceeded room capacity: ', @number_of_customers - @room_capacity, ' people. ');
    END

    -- Update the booking record surcharge and note
    IF @surcharge <> (SELECT surcharge FROM inserted)
    BEGIN
      UPDATE BOOKING_RECORD 
      SET surcharge = @surcharge, note = @note
      WHERE booking_record_id = @booking_record_id;
    END;
  END;
END;