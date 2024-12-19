SELECT * FROM DBA_USERS;

SELECT * FROM USER_DETAILS

CREATE TABLE USER_DETAILS (
    USERNAME VARCHAR2(30) PRIMARY KEY,         -- Khóa chính, liên k?t v?i DBA_USERS.USERNAME
    FULL_NAME VARCHAR2(100) NOT NULL,          -- H? và tên ??y ??
    ADDRESS VARCHAR2(255),                     -- ??a ch?
    PHONE_NUMBER VARCHAR2(15),                 -- S? ?i?n tho?i
    EMAIL VARCHAR2(100) UNIQUE,                -- Email (duy nh?t)
    CREATED_DATE DATE DEFAULT SYSDATE,         -- Ngày t?o b?n ghi
    UPDATED_DATE DATE                          -- Ngày c?p nh?t b?n ghi
);

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('ANH', 'Huỳnh Thị Ngọc Ánh', 'Thành phố Huế', '0905123456', 'anh.ngocanh@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('DUNG', 'Trần Minh Dũng', 'Thành phố Cần Thơ', '0916123456', 'dung.minhdung@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('NGHIA', 'Nguyễn Trung Nghĩa', 'Thành phố Hồ Chí Minh', '0907123456', 'nghia.trungnghia@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('TUYEN', 'Nguyễn Thị Thanh Tuyền', 'Tỉnh Tiền Giang', '0938123456', 'tuyen.thanhtuyen@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('LINH', 'Ngô Phương Linh', 'Thành phố Đà Nẵng', '0972123456', 'linh.phuonglinh@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('HOANG', 'Hoàng Văn Bình', 'Tỉnh Quảng Ninh', '0943123456', 'binh.hoangvan@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('MY', 'Phạm Thị Mỹ Duyên', 'Thành phố Hà Nội', '0981123456', 'duyen.mythi@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('HUNG', 'Lê Minh Hùng', 'Tỉnh Bình Dương', '0965123456', 'hung.leminh@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('QUYNH', 'Trần Quỳnh Anh', 'Thành phố Hải Phòng', '0957123456', 'quynh.trananh@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('THAO', 'Đỗ Thị Thảo', 'Thành phố Nha Trang', '0923123456', 'thao.dothi@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('VIET', 'Nguyễn Công Việt', 'Tỉnh Bắc Ninh', '0918123456', 'viet.congnguyen@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('THUY', 'Vũ Ngọc Thúy', 'Thành phố Đà Lạt', '0934123456', 'thuy.ngocvu@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('KHOA', 'Bùi Đình Khoa', 'Thành phố Huế', '0976123456', 'khoa.buidinh@example.com');

INSERT INTO USER_DETAILS (USERNAME, FULL_NAME, ADDRESS, PHONE_NUMBER, EMAIL)
VALUES ('PHUONG', 'Nguyễn Ngọc Phương', 'Tỉnh Bến Tre', '0983123456', 'phuong.nguyenngoc@example.com');



GRANT CREATE SESSION TO adminuser;
GRANT SELECT ON SYS.USER_DETAILS TO adminuser;
GRANT INSERT ON SYS.USER_DETAILS TO adminuser;
GRANT UPDATE ON SYS.USER_DETAILS TO adminuser;
GRANT DELETE ON SYS.USER_DETAILS TO adminuser;
