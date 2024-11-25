﻿-- USE HotelManagementSystem;

--Code xóa bảng để khỏi xóa database tạo lại quài nè
-- Drop foreign key constraints
-- ALTER TABLE ACCOUNT
-- DROP CONSTRAINT FK_EMPLOYEE_ACCOUNT;

-- -- Drop tables with foreign key relationships
-- DROP TABLE ACCOUNT;

-- -- More foreign key constraints
-- ALTER TABLE ROOM
-- DROP CONSTRAINT FK_ROOM_ROOM_TYPE;

-- ALTER TABLE BOOKING_RECORD
-- DROP CONSTRAINT FK_ROOM_BOOKING_RECORD;

-- ALTER TABLE BOOKING_RECORD
-- DROP CONSTRAINT FK_REPRESENTATIVE_BOOKING_RECORD;

-- ALTER TABLE SERVICE_USAGE_INFOR
-- DROP CONSTRAINT FK_BOOKING_RECORD_SERVICE_USAGE;

-- ALTER TABLE SERVICE_USAGE_INFOR
-- DROP CONSTRAINT FK_SERVICE_ROOM_SERVICE_USAGE;

-- ALTER TABLE BILL
-- DROP CONSTRAINT FK_BOOKING_RECORD_BILL;

-- ALTER TABLE BILL
-- DROP CONSTRAINT FK_EMPLOYEE_BILL;

-- ALTER TABLE CUSTOMER_OF_BOOKING_RECORD
-- DROP CONSTRAINT FK_CUSTOMER_CUST_BOOKING;

-- ALTER TABLE CUSTOMER_OF_BOOKING_RECORD
-- DROP CONSTRAINT FK_BOOKING_RECORD_CUST_BOOKING;

-- -- Drop the remaining tables
-- DROP TABLE ROOM;
-- DROP TABLE ROOM_TYPE;
-- DROP TABLE SERVICE_USAGE_INFOR;
-- DROP TABLE BILL;
-- DROP TABLE BOOKING_RECORD;
-- DROP TABLE SERVICE_ROOM;
-- DROP TABLE CUSTOMER_OF_BOOKING_RECORD;
-- DROP TABLE PHONE_NUMBER_OF_CUSTOMER;
-- DROP TABLE PHONE_NUMBER_OF_EMPLOYEE;
-- DROP TABLE EMPLOYEE;
-- DROP TABLE CUSTOMER;



-- CREATE TABLE CUSTOMER (
--     customer_id INT IDENTITY(1, 1) CONSTRAINT PK_CUSTOMER PRIMARY KEY,
--     customer_name NVARCHAR(50) NOT NULL,
--     gender NVARCHAR(10) CONSTRAINT CK_CUSTOMER_GENDER CHECK (gender IN (N'Nam', N'Nữ')),		
--     email VARCHAR(255) CONSTRAINT CK_EMAIL_CUSTOMER CHECK (email LIKE '%@gmail.com'),
--     birthday DATE NOT NULL,
--     identify_card VARCHAR(25) UNIQUE NOT NULL CONSTRAINT CK_IDENTIFY_CARD_LENGTH_CUSTOMER CHECK (LEN(identify_card) = 12),
--     address NVARCHAR(255) NULL,
-- 	status BIT DEFAULT 0 NOT NULL, -- 1: OFFICIAL - KHÁCH HÀNG ĐÃ ĐẾN CHECKIN; 0: UNOFFICIAL: KHÁCH HÀNG CHỈ MỚI ĐẶT ONLINE
-- );

-- CREATE TABLE EMPLOYEE (
--     employee_id INT IDENTITY(1, 1) CONSTRAINT PK_EMPLOYEE PRIMARY KEY,
--     employee_name NVARCHAR(50) NOT NULL,
--     gender NVARCHAR(10) CONSTRAINT CK_EMPLOYEE_GENDER CHECK (gender IN(N'Nam', N'Nữ')),		
--     birthday DATE CONSTRAINT CK_BIRTHDAY_EMPLOYEE CHECK (DATEDIFF(YEAR, birthday, GETDATE()) >= 18) NOT NULL,
--     identify_card VARCHAR(25) NOT NULL CONSTRAINT CK_IDENTIFY_CARD_LENGTH_EMPLOYEE CHECK (LEN(identify_card) = 12),
--     address NVARCHAR(255) NOT NULL,
--     email VARCHAR(255) CONSTRAINT CK_EMAIL_EMPLOYEE CHECK (email LIKE '%@gmail.com') NOT NULL
-- );

-- CREATE TABLE ACCOUNT (
--     account_id INT IDENTITY(1, 1) CONSTRAINT PK_ACCOUNT PRIMARY KEY,
--     username NVARCHAR(50) NOT NULL UNIQUE,
--     password VARCHAR(25) NOT NULL CONSTRAINT CK_PASSWORD_LENGTH_ACCOUNT CHECK (LEN(password) >= 6),
--     employee_id INT CONSTRAINT FK_EMPLOYEE_ACCOUNT REFERENCES EMPLOYEE(employee_id) ON DELETE CASCADE, 
-- 	-- KHI XÓA NH N VIÊN THÌ SẼ XÓA TÀI KHOẢN NH N VIÊN TƯƠNG ỨNG
-- 	roles NVARCHAR(20) CONSTRAINT CK_ROLES_ACCOUNT CHECK (roles IN ('sysadmin', 'staff')) NOT NULL 
-- );

-- CREATE TABLE ROOM_TYPE (
--     room_type_id INT IDENTITY(1, 1) CONSTRAINT PK_ROOM_TYPE PRIMARY KEY,
--     room_type_name NVARCHAR(25) NOT NULL,
--     price float NOT NULL,
--     discount_room float NOT NULL
-- );

-- CREATE TABLE ROOM (
--     room_id INT IDENTITY(1,1) CONSTRAINT PK_ROOM PRIMARY KEY,
--     room_name NVARCHAR(25) UNIQUE NOT NULL,
--     room_capacity INT NOT NULL,
--     room_status NVARCHAR(20) CONSTRAINT CK_ROOM_STATUS CHECK (room_status IN(N'Đang cho thuê', N'Trống', N'Đang sửa')) NOT NULL,
--     room_description NVARCHAR(255),
--     room_image VARBINARY(MAX),
--     room_update DATETIME DEFAULT (GETDATE()) NOT NULL,
--     room_type_id INT NOT NULL CONSTRAINT FK_ROOM_ROOM_TYPE REFERENCES ROOM_TYPE(room_type_id) ON DELETE CASCADE ON UPDATE CASCADE
-- 	-- KHI XÓA LOẠI PHÒNG THÌ SẼ XÓA PHÒNG TƯƠNG ỨNG -- KHI UPDATE LOẠI PHÒNG THÌ UPDATE LOẠI PHÒNG CỦA PHÒNG TƯƠNG ỨNG 
-- );

-- CREATE TABLE SERVICE_ROOM (
--     service_room_id INT IDENTITY(1, 1) CONSTRAINT PK_SERVICE_ROOM PRIMARY KEY,
--     service_room_name NVARCHAR(50) UNIQUE NOT NULL,
--     service_room_status BIT DEFAULT 1 NOT NULL, -- 1: AVAILABLE; 0: UNAVAILABLE;
--     service_room_price FLOAT NOT NULL,
--     discount_service FLOAT
-- );

-- CREATE TABLE BOOKING_RECORD (
--     booking_record_id INT IDENTITY(1, 1) CONSTRAINT PK_BOOKING_RECORD PRIMARY KEY,
--     booking_time DATETIME DEFAULT (GETDATE()) NOT NULL,
--     expected_checkin_date DATETIME NOT NULL,
--     expected_checkout_date DATETIME NOT NULL,
--     deposit FLOAT DEFAULT 0 NOT NULL, -- SỐ TIỀN CỌC MẶC ĐỊNH LÀ 0
--     surcharge FLOAT DEFAULT 0 NOT NULL, -- PHỤ PHÍ MẶC ĐỊNH LÀ 0
--     note NVARCHAR(255),
--     status NVARCHAR(25) CONSTRAINT CK_BOOKING_RECORD_STATUS CHECK (status IN (N'Chờ xác nhận', N'Đã xác nhận', N'Đã hủy', N'Đã hoàn tất')) NOT NULL,
--     actual_checkin_date DATETIME NULL,
--     actual_checkout_date DATETIME NULL,
--     room_id INT CONSTRAINT FK_ROOM_BOOKING_RECORD REFERENCES ROOM(room_id),
--     representative_id INT CONSTRAINT FK_REPRESENTATIVE_BOOKING_RECORD REFERENCES CUSTOMER(customer_id)
-- );


-- CREATE TABLE SERVICE_USAGE_INFOR (
--     service_usage_infor_id INT IDENTITY(1,1) CONSTRAINT PK_SERVICE_USAGE_INFOR PRIMARY KEY,
--     number_of_service INT NOT NULL,
--     date_used DATETIME DEFAULT (GETDATE()) NOT NULL,
-- 	total_fee FLOAT,
--     booking_record_id INT CONSTRAINT FK_BOOKING_RECORD_SERVICE_USAGE REFERENCES BOOKING_RECORD(booking_record_id) ON UPDATE CASCADE,
--     service_room_id INT CONSTRAINT FK_SERVICE_ROOM_SERVICE_USAGE REFERENCES SERVICE_ROOM(service_room_id)
-- );

-- CREATE TABLE BILL (
--     bill_id INT IDENTITY(1,1) CONSTRAINT PK_BILL PRIMARY KEY,
--     costs_incurred float DEFAULT 0,
--     content_incurred NVARCHAR(255),
--     total_cost float DEFAULT 0,
--     created_date DATETIME DEFAULT (GETDATE()),
--     payment_method NVARCHAR(15) CONSTRAINT CK_PAYMENT_METHOD CHECK (payment_method IN(N'Tiền mặt', N'Chuyển khoản')),
-- 	paytime DATETIME,
--     booking_record_id INT UNIQUE NOT NULL CONSTRAINT FK_BOOKING_RECORD_BILL REFERENCES BOOKING_RECORD(booking_record_id) ON UPDATE CASCADE ON DELETE CASCADE,
--     employee_id INT CONSTRAINT FK_EMPLOYEE_BILL REFERENCES EMPLOYEE(employee_id)
-- );

-- CREATE TABLE CUSTOMER_OF_BOOKING_RECORD (
--     customer_id INT CONSTRAINT FK_CUSTOMER_CUST_BOOKING REFERENCES CUSTOMER(customer_id),
--     booking_record_id INT CONSTRAINT FK_BOOKING_RECORD_CUST_BOOKING REFERENCES BOOKING_RECORD(booking_record_id) ON UPDATE CASCADE ON DELETE CASCADE
-- 	PRIMARY KEY (customer_id, booking_record_id)
-- );

-- CREATE TABLE PHONE_NUMBER_OF_CUSTOMER (
-- 	phone_number VARCHAR(15) UNIQUE NOT NULL CONSTRAINT CK_PHONE_NUMBER_LENGTH_CUSTOMER CHECK (LEN(phone_number) = 10),
-- 	customer_id INT CONSTRAINT FK_CUSTOMER_PHONE_NUMBER REFERENCES CUSTOMER(customer_id),
-- 	PRIMARY KEY (customer_id, phone_number)
-- );

-- CREATE TABLE PHONE_NUMBER_OF_EMPLOYEE (
-- 	phone_number VARCHAR(15) UNIQUE NOT NULL CONSTRAINT CK_PHONE_NUMBER_LENGTH_EMPLOYEE CHECK (LEN(phone_number) = 10),
-- 	employee_id INT CONSTRAINT FK_EMPLOYEE_PHONE_NUMBER REFERENCES EMPLOYEE(employee_id),
-- 	PRIMARY KEY (employee_id, phone_number)
-- );


-- Drop constraints and tables (Oracle Syntax)
DROP TABLE ACCOUNT CASCADE CONSTRAINTS;
DROP TABLE BILL CASCADE CONSTRAINTS;
DROP TABLE BOOKING_RECORD CASCADE CONSTRAINTS;
DROP TABLE CUSTOMER CASCADE CONSTRAINTS;
DROP TABLE EMPLOYEE CASCADE CONSTRAINTS;
DROP TABLE ROOM CASCADE CONSTRAINTS;
DROP TABLE ROOM_TYPE CASCADE CONSTRAINTS;
DROP TABLE SERVICE_ROOM CASCADE CONSTRAINTS;
DROP TABLE SERVICE_USAGE_INFOR CASCADE CONSTRAINTS;
DROP TABLE CUSTOMER_OF_BOOKING_RECORD CASCADE CONSTRAINTS;
DROP TABLE PHONE_NUMBER_OF_CUSTOMER CASCADE CONSTRAINTS;
DROP TABLE PHONE_NUMBER_OF_EMPLOYEE CASCADE CONSTRAINTS;

commit;
-- Create CUSTOMER table
CREATE TABLE CUSTOMER (
    customer_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    customer_name NVARCHAR2(50) NOT NULL,
    gender NVARCHAR2(10) CHECK (gender IN ('Nam', 'Nữ')),
    email VARCHAR2(255) CHECK (email LIKE '%@gmail.com'),
    birthday DATE NOT NULL,
    identify_card VARCHAR2(25) UNIQUE NOT NULL CHECK (LENGTH(identify_card) = 12),
    address NVARCHAR2(255) NULL,
    status NUMBER(1) DEFAULT 0 NOT NULL -- 1: OFFICIAL, 0: UNOFFICIAL
);

-- Create EMPLOYEE table
CREATE TABLE EMPLOYEE (
    employee_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    employee_name NVARCHAR2(50) NOT NULL,
    gender NVARCHAR2(10) CHECK (gender IN ('Nam', 'Nữ')),
    birthday DATE NOT NULL,
    identify_card VARCHAR2(25) NOT NULL CHECK (LENGTH(identify_card) = 12),
    address NVARCHAR2(255) NOT NULL,
    email VARCHAR2(255) CHECK (email LIKE '%@gmail.com') NOT NULL
);

-- Create ACCOUNT table
CREATE TABLE ACCOUNT (
    account_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    username NVARCHAR2(50) NOT NULL UNIQUE,
    password VARCHAR2(25) NOT NULL CHECK (LENGTH(password) >= 6),
    employee_id NUMBER REFERENCES EMPLOYEE(employee_id) ON DELETE CASCADE,
    roles NVARCHAR2(20) NOT NULL
);

-- Create ROOM_TYPE table
CREATE TABLE ROOM_TYPE (
    room_type_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    room_type_name NVARCHAR2(25) NOT NULL,
    price NUMBER NOT NULL,
    discount_room NUMBER NOT NULL
);

-- Create ROOM table
CREATE TABLE ROOM (
    room_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    room_name NVARCHAR2(25) UNIQUE NOT NULL,
    room_capacity NUMBER NOT NULL,
    room_status NVARCHAR2(20) CHECK (room_status IN ('Đang cho thuê', 'Trống', 'Đang sửa')) NOT NULL,
    room_description NVARCHAR2(255),
    room_image BLOB,
    room_update TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    room_type_id NUMBER NOT NULL REFERENCES ROOM_TYPE(room_type_id) ON DELETE CASCADE
);

-- Create SERVICE_ROOM table
CREATE TABLE SERVICE_ROOM (
    service_room_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    service_room_name NVARCHAR2(50) UNIQUE NOT NULL,
    service_room_status NUMBER(1) DEFAULT 1 NOT NULL, -- 1: AVAILABLE, 0: UNAVAILABLE
    service_room_price NUMBER NOT NULL,
    discount_service NUMBER
);

-- Create BOOKING_RECORD table
CREATE TABLE BOOKING_RECORD (
    booking_record_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    booking_time TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    expected_checkin_date TIMESTAMP NOT NULL,
    expected_checkout_date TIMESTAMP NOT NULL,
    deposit NUMBER DEFAULT 0 NOT NULL,
    surcharge NUMBER DEFAULT 0 NOT NULL,
    note NVARCHAR2(255),
    status NVARCHAR2(25) CHECK (status IN ('Chờ xác nhận', 'Đã xác nhận', 'Đã hủy', 'Đã hoàn tất')) NOT NULL,
    actual_checkin_date TIMESTAMP NULL,
    actual_checkout_date TIMESTAMP NULL,
    room_id NUMBER REFERENCES ROOM(room_id),
    representative_id NUMBER REFERENCES CUSTOMER(customer_id)
);

-- Create SERVICE_USAGE_INFOR table
CREATE TABLE SERVICE_USAGE_INFOR (
    service_usage_infor_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    number_of_service NUMBER NOT NULL,
    date_used TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    total_fee NUMBER,
    booking_record_id NUMBER REFERENCES BOOKING_RECORD(booking_record_id),
    service_room_id NUMBER REFERENCES SERVICE_ROOM(service_room_id)
);

-- Create BILL table
CREATE TABLE BILL (
    bill_id NUMBER GENERATED BY DEFAULT ON NULL AS IDENTITY PRIMARY KEY,
    costs_incurred NUMBER DEFAULT 0,
    content_incurred NVARCHAR2(255),
    total_cost NUMBER DEFAULT 0,
    created_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    payment_method NVARCHAR2(15) CHECK (payment_method IN ('Tiền mặt', 'Chuyển khoản')),
    paytime TIMESTAMP,
    booking_record_id NUMBER UNIQUE NOT NULL REFERENCES BOOKING_RECORD(booking_record_id),
    employee_id NUMBER REFERENCES EMPLOYEE(employee_id)
);

-- Create CUSTOMER_OF_BOOKING_RECORD table
CREATE TABLE CUSTOMER_OF_BOOKING_RECORD (
    customer_id NUMBER REFERENCES CUSTOMER(customer_id),
    booking_record_id NUMBER REFERENCES BOOKING_RECORD(booking_record_id) ON DELETE CASCADE,
    PRIMARY KEY (customer_id, booking_record_id)
);

-- Create PHONE_NUMBER_OF_CUSTOMER table
CREATE TABLE PHONE_NUMBER_OF_CUSTOMER (
    phone_number VARCHAR2(15) UNIQUE NOT NULL CHECK (LENGTH(phone_number) = 10),
    customer_id NUMBER REFERENCES CUSTOMER(customer_id),
    PRIMARY KEY (customer_id, phone_number)
);

-- Create PHONE_NUMBER_OF_EMPLOYEE table
CREATE TABLE PHONE_NUMBER_OF_EMPLOYEE (
    phone_number VARCHAR2(15) UNIQUE NOT NULL CHECK (LENGTH(phone_number) = 10),
    employee_id NUMBER REFERENCES EMPLOYEE(employee_id),
    PRIMARY KEY (employee_id, phone_number)
);


