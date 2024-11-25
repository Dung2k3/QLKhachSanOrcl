GRANT CREATE SESSION TO HotelManagementSystem IDENTIFIED BY admin

ALTER USER HotelManagementSystem QUOTA 10M ON users;

GRANT CREATE  TABLE TO HotelManagementSystem;

GRANT CREATE  SEQUENCE  TO HotelManagementSystem;

CREATE ROLE HotelAdmin;
CREATE ROLE HotelStaff;

GRANT CREATE SESSION TO HotelCheckLogin IDENTIFIED BY login

GRANT SELECT ON HotelManagementSystem.ACCOUNT TO HotelCheckLogin;

GRANT CREATE SESSION TO employee1 IDENTIFIED BY password1

commit;