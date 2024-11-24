USE HotelManagementSystem;

-- Insert data into CUSTOMER table
INSERT INTO CUSTOMER (customer_name, gender, email, birthday, identify_card, address, status)
VALUES
    ('Nguyen Van A', N'Nam', 'johndoe@gmail.com', '1990-01-15', '123456789012', '123 Main St', 1),
    ('Nguyen Van B', N'Nữ', 'jane.smith@gmail.com', '1985-03-20', '987654321098', '456 Elm St', 1),
    ('Nguyen Van C', N'Nữ', 'alice.johnson@gmail.com', '1995-07-10', '555111222233', '789 Oak St', 1),
    ('Nguyen Van D', N'Nam', 'bob.brown@gmail.com', '1980-12-05', '777888999911', '101 Pine St', 0),
    ('Nguyen Van E', N'Nữ', 'eve.williams@gmail.com', '1987-09-25', '222333444455', '222 Cedar St', 1),
    ('Nguyen Van F', N'Nam', 'johndoe@gmail.com', '1990-01-15', '123456789011', '123 Main St', 1),
    ('Nguyen Van G', N'Nữ', 'jane.smith@gmail.com', '1985-03-20', '987654321091', '456 Elm St', 1),
    ('Nguyen Van H', N'Nữ', 'alice.johnson@gmail.com', '1995-07-10', '555111222231', '789 Oak St', 0),
    ('Nguyen Van I', N'Nam', 'bob.brown@gmail.com', '1980-12-05', '777888999912', '101 Pine St', 0),
    ('Nguyen Van J', N'Nữ', 'eve.williams@gmail.com', '1987-09-25', '222333444456', '222 Cedar St', 1);

-- Insert data into EMPLOYEE table
INSERT INTO EMPLOYEE (employee_name, gender, birthday, identify_card, address, email)
VALUES
    (N'Lê Phúc Hậu', N'Nam', '1980-05-12', '123456789012', '123 Oak St', 'michael.johnson@gmail.com'),
    (N'Huỳnh Thị Ngọc Ánh', N'Nữ', '1988-08-18', '987654321098', '456 Maple St', 'sarah.davis@gmail.com'),
    (N'Phùng Thanh Tiền', N'Nữ', '1985-04-29', '555111222233', '789 Birch St', 'david.wilson@gmail.com'),
    (N'Nguyễn Thị Thanh Tuyền', N'Nữ', '1990-03-15', '777888999911', '101 Spruce St', 'jennifer.lee@gmail.com');

-- Insert data into ACCOUNT table
INSERT INTO ACCOUNT (username, password, employee_id)
VALUES
    ('employee1', 'password1', 1),
    ('employee2', 'password2', 2),
    ('employee3', 'password3', 3),
    ('employee4', 'password4', 4);

-- Insert data into ROOM_TYPE table
INSERT INTO ROOM_TYPE (room_type_name, price, discount_room)
VALUES
    ('Standard', 400000, 0.05),
    ('Superior', 550000, 0.1),
    ('Deluxe', 850000, 0.1),
    ('Suite', 1990000, 0.15),
    ('Family', 2200000, 0.1);

-- Insert data into ROOM table
-- Insert 10 Standard rooms
INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
VALUES
    ('101', 2, N'Đang cho thuê', 'Standard room with a view', null, 1),
    ('102', 2, N'Đang cho thuê', 'Standard room with a view', null, 1),
    ('103', 2, N'Đang cho thuê', 'Standard room with a view', null, 1),
    ('104', 2, N'Trống', 'Standard room with a view', null, 1),
    ('105', 2, N'Trống', 'Standard room with a view', null, 1),
    ('106', 2, N'Đang cho thuê', 'Standard room with a view', null, 1);

-- Insert 5 Superior rooms
INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
VALUES
    ('107', 2, N'Đang cho thuê', 'Superior room with a view', null, 2),
    ('108', 2, N'Trống', 'Superior room with a view', null, 2),
    ('109', 2, N'Đang cho thuê', 'Superior room with a view', null, 2),
    ('110', 2, N'Trống', 'Superior room with a view', null, 2),
    ('201', 2, N'Trống', 'Superior room with a view', null, 2);

-- Insert 5 Deluxe rooms
INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
VALUES
    ('202', 2, N'Trống', 'Deluxe room with a view', null, 3),
    ('203', 2, N'Đang cho thuê', 'Deluxe room with a view', null, 3),
    ('204', 2, N'Đang cho thuê', 'Deluxe room with a view', null, 3),
    ('205', 2, N'Đang cho thuê', 'Deluxe room with a view', null, 3),
    ('206', 2, N'Trống', 'Deluxe room with a view', null, 3);

-- Insert 2 Suite rooms
INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
VALUES
    ('207', 4, N'Đang sửa', 'Suite room with a view', null, 4),
    ('208', 4, N'Trống', 'Suite room with a view', null, 4);

-- Insert 2 Family rooms
INSERT INTO ROOM (room_name, room_capacity, room_status, room_description, room_image, room_type_id)
VALUES
    ('209', 4, N'Đang cho thuê', 'Family room with a view', null, 5),
    ('210', 4, N'Đang sửa', 'Family room with a view', null, 5);

-- Insert data into SERVICE_ROOM table
INSERT INTO SERVICE_ROOM (service_room_name, service_room_status, service_room_price, discount_service)
VALUES
    (N'Đưa đón sân bay', 1, 150000, 0.1),
    (N'Buffet', 1, 250000, 0.15),
    (N'Giặt ủi, là', 1, 40000, 0.2),
    (N'Coffee', 1, 15000, 0),
    (N'Mì tôm', 1, 25000, 0),
    (N'Nước uống', 1, 20000, 0);



-- Insert data into BOOKING_RECORD table
INSERT INTO BOOKING_RECORD (booking_time, expected_checkin_date, expected_checkout_date, deposit, surcharge, note, status, room_id, representative_id)
VALUES
    ('2023-01-10 10:00:00', '2023-01-15 14:00:00', '2023-01-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 1, 1),
    ('2023-03-12 11:00:00', '2023-03-17 15:00:00', '2023-03-22 13:00:00', 400000, 0, 'None', N'Đã xác nhận', 2, 2),
    ('2023-03-14 12:00:00', '2023-03-19 16:00:00', '2023-03-24 14:00:00', 300000, 0, 'Late check-in', N'Đã xác nhận', 3, 3),
    ('2023-03-16 13:00:00', '2023-03-21 17:00:00', '2023-03-26 15:00:00', 200000, 100000, 'Special request: Late checkout', N'Chờ xác nhận', 4, 4),
    ('2023-05-18 14:00:00', '2023-05-23 18:00:00', '2023-05-28 16:00:00', 100000, 0, 'None', N'Đã hủy', 5, 5),
    ('2023-05-18 14:00:00', '2023-05-23 18:00:00', '2023-05-28 16:00:00', 100000, 0, 'None', N'Đã hủy', 6, 5),
    ('2023-05-18 14:00:00', '2023-05-23 18:00:00', '2023-05-28 16:00:00', 100000, 0, 'None', N'Đã hủy', 7, 6),
    ('2023-05-18 14:00:00', '2023-05-23 18:00:00', '2023-05-28 16:00:00', 100000, 0, 'None', N'Đã hủy', 8, 7),
    ('2023-05-18 14:00:00', '2023-05-23 18:00:00', '2023-05-28 16:00:00', 100000, 0, 'None', N'Đã hủy', 9, 8),
    ('2023-08-10 10:00:00', '2023-08-15 14:00:00', '2023-08-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 10, 1),
    ('2023-08-12 11:00:00', '2023-08-17 15:00:00', '2023-08-22 13:00:00', 400000, 0, 'None', N'Đã xác nhận', 11, 2),
    ('2023-08-14 12:00:00', '2023-08-19 16:00:00', '2023-08-24 14:00:00', 300000, 0, 'Late check-in', N'Đã xác nhận', 12, 3),
    ('2023-08-16 13:00:00', '2023-08-21 17:00:00', '2023-08-26 15:00:00', 200000, 100000, 'Special request: Late checkout', N'Chờ xác nhận', 13, 4),
    ('2023-08-10 10:00:00', '2023-08-15 14:00:00', '2023-08-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 14, 1),
    ('2023-12-12 11:00:00', '2023-12-17 15:00:00', '2023-12-22 13:00:00', 400000, 0, 'None', N'Đã xác nhận', 15, 2),
    ('2023-12-14 12:00:00', '2023-12-16 16:00:00', '2023-12-24 14:00:00', 300000, 0, 'Late check-in', N'Đã xác nhận', 16, 3),
    ('2023-12-16 13:00:00', '2023-12-21 17:00:00', '2023-12-26 15:00:00', 200000, 100000, 'Special request: Late checkout', N'Chờ xác nhận', 17, 4),
    ('2023-08-10 10:00:00', '2023-08-15 14:00:00', '2023-08-20 12:00:00', 500000, 100000, 'Special request: Early check-in', N'Đã xác nhận', 18, 1),
    ('2023-12-12 11:00:00', '2023-12-17 15:00:00', '2023-12-22 13:00:00', 400000, 0, 'None', N'Đã xác nhận', 19, 2),
    ('2023-12-14 12:00:00', '2023-12-16 16:00:00', '2023-12-24 14:00:00', 300000, 0, 'Late check-in',  N'Đã xác nhận', 20, 3);

INSERT INTO SERVICE_USAGE_INFOR (number_of_service, date_used, total_fee, booking_record_id, service_room_id)
VALUES
    (3, '2023-01-10 14:30:00', 150000, 1, 1),
    (2, '2023-02-15 12:45:00', 100000, 2, 2),
	    (3, '2023-01-10 14:30:00', 150000, 1, 3),
    (2, '2023-02-15 12:45:00', 100000, 2, 4),
	    (3, '2023-01-10 14:30:00', 150000, 1, 5),
    (2, '2023-02-15 12:45:00', 100000, 2, 1);

INSERT INTO BILL (costs_incurred, content_incurred, total_cost, created_date, payment_method, paytime, booking_record_id, employee_id)
VALUES
    (0, NULL, 150000, '2023-01-20 10:00:00', N'Tiền mặt', '2023-01-20 10:00:00', 1, 1),
    (0, NULL, 2200000, '2023-01-21 11:00:00', N'Chuyển khoản', '2023-01-21 11:00:00', 2, 2),
    (0, NULL, 800000, '2023-03-22 12:00:00', N'Tiền mặt', '2023-03-22 12:00:00', 3, 3),
    (0, NULL, 250000, '2023-03-23 13:00:00', N'Chuyển khoản', '2023-03-23 13:00:00', 4, 4),
    (0, NULL, 150000, '2023-03-20 10:00:00', N'Tiền mặt', '2023-03-20 10:00:00', 5, 1),
    (700000, N'Kính nhà vệ sinh vỡ', 2200000, '2023-03-21 11:00:00', N'Chuyển khoản', '2023-03-21 11:00:00', 6, 2),
    (0, NULL, 800000, '2023-05-22 12:00:00', N'Tiền mặt', '2023-05-22 12:00:00', 7, 3),
    (0, NULL, 250000, '2023-05-23 13:00:00', N'Chuyển khoản', '2023-05-23 13:00:00', 8, 4),
    (100000, N'Hư vòi sen', 800000, '2023-05-22 12:00:00', N'Tiền mặt', '2023-05-22 12:00:00', 9, 3),
    (800000, N'Cửa kính vỡ', 2500000, '2023-08-23 13:00:00', N'Chuyển khoản', '2023-08-23 13:00:00', 10, 4),
    (100000, N'Lavabo bị nghẹt', 150000, '2023-08-20 10:00:00', N'Tiền mặt', '2023-08-20 10:00:00', 11, 1),
    (1200000, 'Máy nước nóng bị hư', 3200000, '2023-08-21 11:00:00', N'Chuyển khoản', '2023-08-21 11:00:00', 12, 2),
    (0, NULL, 150000, '2023-10-20 10:00:00', N'Tiền mặt', '2023-10-20 10:00:00', 13, 1),
    (700000, N'Kính nhà vệ sinh vỡ', 2200000, '2023-10-21 11:00:00', N'Chuyển khoản', '2023-10-21 11:00:00', 14, 2),
    (0, NULL, 800000, '2023-10-22 12:00:00', N'Tiền mặt', '2023-10-22 12:00:00', 15, 3),
    (0, NULL, 250000, '2023-11-23 13:00:00', N'Chuyển khoản', '2023-11-23 13:00:00', 16, 4),
    (100000, N'Hư vòi sen', 800000, '2023-11-22 12:00:00', N'Tiền mặt', '2023-11-22 12:00:00', 17, 3),
    (0, NULL, 800000, '2023-11-22 12:00:00', N'Tiền mặt', '2023-11-22 12:00:00', 18, 3),
    (0, NULL, 250000, '2023-11-23 13:00:00', N'Chuyển khoản', '2023-11-23 13:00:00', 19, 4),
    (0, NULL, 150000, '2023-11-20 10:00:00', N'Tiền mặt', '2023-11-20 10:00:00', 20, 1);

-- Insert data into CUSTOMER_OF_BOOKING_RECORD table
INSERT INTO CUSTOMER_OF_BOOKING_RECORD (customer_id, booking_record_id)
VALUES
    (1, 1),
    (2, 2),
    (3, 3),
    (4, 4),
    (5, 5);


INSERT INTO PHONE_NUMBER_OF_CUSTOMER (customer_id, phone_number)
VALUES
    (1, '0636245127'),
	(1, '0436245128'),
	(1, '0336245129'),
    (2, '0936245121'),
	(2, '0936245192'),
	(2, '0936245122'),
    (3, '0836245129'),
    (4, '0836245126'),
    (5, '0836245125'),
	(6, '0836245124'),
    (7, '0836245123'),
    (8, '0836245122'),
    (9, '0836245121'),
    (10, '0836245120');

INSERT INTO PHONE_NUMBER_OF_EMPLOYEE (employee_id, phone_number)
VALUES
    (1, '0836245125'),
    (2, '0836245122'),
    (3, '0836245123'),
    (4, '0836245126');